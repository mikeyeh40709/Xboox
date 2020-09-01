window.addEventListener("load", function () {
    let detailsBtn = $("button[data-target='#orderDetails']");
    let detailsBody = $("#orderDetails .modal-body tbody");
    detailsBtn.on("click", function () {
        $.ajax({
            url: `GetOrderDetails/${this.id}`,
            method: "get",
            dataType: "json",
            success: function (res) {
                detailsBody.text("");
                let tr = $("<tr></tr>");
                res.forEach(item => {
                    console.log(item);
                    Object.values(item).forEach(value => {
                        console.log(value);
                        let td = $("<td></td>");
                        td.text(value);
                        tr.append(td);
                    })
                    detailsBody.append(tr);
                });
            },
            error: function (err) { console.log(err) },
        });
    });

    let deleteBtn = $("button[data-target='#deleteOrder']");
    let confirmDetailBtn = $("swal2-confirm");
    deleteBtn.on("click", deleteOrder);
    function deleteOrder() {
        Swal.fire({
            title: '取消訂單',
            text: "取消訂單後就無法返回",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: '刪除!'
        }).then((result) => {
            if (result.value) {
                Swal.fire(
                    'Deleted!',
                    '已成功刪除.',
                    'success'
                )
            }
        })
    }
    confirmDetailBtn.on("click", function () {
        $.ajax({
            url: `GetOrderDetails/${this.id}`,
            method: "post",
            success: function (res) {
                detailsBody.text("");
                let tr = $("<tr></tr>");
                res.forEach(item => {
                    console.log(item);
                    Object.values(item).forEach(value => {
                        console.log(value);
                        let td = $("<td></td>");
                        td.text(value);
                        tr.append(td);
                    })
                    detailsBody.append(tr);
                });
            },
            error: function (err) { console.log(err) },
        });
    })
});
