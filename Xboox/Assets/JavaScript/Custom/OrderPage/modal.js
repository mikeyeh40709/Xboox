window.addEventListener("load", function () {
    const AddAntiForgeryToken = function (data) {
        data.__RequestVerificationToken = $('#__AjaxAntiForgeryForm input[name=__RequestVerificationToken]').val();
        return data;
    };
    let detailsBtn = $("button[data-target='#orderDetails']");
    let detailsBody = $("#orderDetails .modal-body tbody");
    let editBtn = $("button[data-target='#editOrder']");
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
                        OrderDetailsList.eq(0).children("img").attr("src", `/Assets/Image/Pics/${values[0]}.jpg`);
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

    editBtn.on("click", function () {
        let checkbox = document.createElement("input");
        checkbox.type = "checkbox";
        Swal.fire({
            title: '編輯付款狀態',
            input: 'radio',
            showCancelButton: true,
            confirmButtonText: '確認',
            showLoaderOnConfirm: true,
            inputOptions: {
                '0': '未付款',
                '1': '已付款',
            },
            preConfirm: (state) => {
                return $.ajax({
                    type: "POST",
                    url: "/Order/ChangeState",
                    data: `{ id: ${state}}`,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (r) {
                        if (!response.ok) {
                            throw new Error(response.statusText)
                        }
                        return response.json()
                    }
                });
                //    })
            },
            allowOutsideClick: () => !Swal.isLoading()
        }).then((result) => {
            if (result.value) {
                Swal.fire({
                    title: `${result.value.login}'s avatar`,
                    imageUrl: result.value.avatar_url
                })
            }
        })
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
                $.ajax({
                    type: "post",
                    url: `DeleteOrder?orderId=${this.id}`,
                    data: "{}",
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
