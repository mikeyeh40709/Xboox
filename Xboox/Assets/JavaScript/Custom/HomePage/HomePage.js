$('.dropdown-menu a').on('click', function () {
    $('.dropdown-toggle').html($(this).html() + '<span class="caret"></span>');
})


$('.header__menu ul li a').click(function () {
    $(this).parent().addClass('active').siblings().removeClass('active');
});

function NoPrePage() {
    Swal.fire("Oops!", "您在第一頁!", "error")
}

function NoNextPage() {
    Swal.fire("Oops!", "您在最後一頁!", "error")
}

function SelectPage(element) {
    let selectedPage = $(element).val();
    let url = window.location.href;
    window.location = url.substring(0, url.length - 1) + selectedPage;
}