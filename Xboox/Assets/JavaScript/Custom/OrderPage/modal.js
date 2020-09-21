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
                detailsBody.text('');
                res.forEach(item => {
                    let tr = $("<tr></tr>");
                    let OrderDetailsList = $($("#OrderDetailsList").html());
                    let values = Object.values(item);
                    for (let i = 1; i < values.length; i++) {
                        OrderDetailsList.eq(0).children("img").attr("src", `${values[0]}`);
                        OrderDetailsList.eq(2 * i).text(values[i]);
                        console.log(OrderDetailsList);
                        console.log(values[i]);
                    }
                    tr.append(OrderDetailsList);
                    detailsBody.append(tr);
                })

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
