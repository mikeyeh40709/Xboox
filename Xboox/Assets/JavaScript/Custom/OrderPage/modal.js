window.addEventListener("load", function () {
    let detailsBtn = $("button[data-target='#orderDetails']");
    let detailsBody = $("#orderDetails .modal-body tbody");
    let deleteBtn = $("button[data-target='#deleteOrder']");
    detailsBtn.on("click", function () {
        $.ajax({
            url: `GetOrderDetails/${this.id}`,
            method: "get",
            dataType: "json",
            success: function (res) {
                let orderData = {
                    total: 0,
                    discount: "",
                    couponCode: ""
                }
                detailsBody.text('');
                res.forEach(item => {
                    let tr = $("<tr></tr>");
                    let OrderDetailsList = $($("#OrderDetailsList").html());
                    let values = Object.values(item);
                    console.log(item);
                    OrderDetailsList.eq(0).children("img").attr("src", `${values[0]}`);
                    OrderDetailsList.eq(2).text(item.Name);
                    OrderDetailsList.eq(4).text(item.Price);
                    OrderDetailsList.eq(6).text(item.Quantity);
                    OrderDetailsList.eq(8).text(item.Total)
                    tr.append(OrderDetailsList);
                    if (item.Coupon != null) {
                        orderData.total += Math.round(parseFloat(item.Total * item.Coupon.Discount));
                        orderData.discount = item.Coupon.Discount;
                        orderData.couponCode = item.Coupon.CouponCode;
                    } else {
                        orderData.total += Math.round(parseFloat(item.Total));
                    }
                    detailsBody.append(tr);
                })
                if (orderData.discount != "") {
                    let discountRow = $(`<tr><td><span class="text-left d-inline-block font-weight-bold">折扣碼</span></td><td colspan="2"><span style="font-size: 20px" class="d-inline-block text-right text-danger font-weight-bold">${orderData.couponCode}</span></td><td><span class="text-left d-inline-block font-weight-bold">折扣</span></td><td><span style="font-size: 20px" class="d-inline-block text-right text-danger font-weight-bold">${orderData.discount}</span></td></tr>`);
                    detailsBody.append(discountRow);
                }
                let endtr = $(`<tr><td colspan="4"><span class="text-left d-inline-block font-weight-bold">總價</span></td><td colspan="1"><span style="font-size: 20px" class="d-inline-block text-right text-danger font-weight-bold">${orderData.total}</span></td></tr>`);
                detailsBody.append(endtr);
            },
            error: function (err) { console.log(err) },
        });
    });

    deleteBtn.on("click", function () {
        Swal.fire({
            title: '取消訂單',
            text: "取消訂單後就無法返回",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: '刪除!',
            preConfirm: () => {
                let obj = {
                    orderId: this.id
                }
                $.ajax({
                    type: "post",
                    url: `CancelOrder?orderId=${this.id}`,
                    data: JSON.stringify(obj),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        window.location.href = response.redirectToUrl;
                    }
                })
            },
            allowOutsideClick: () => !Swal.isLoading()
        })
    });
});
