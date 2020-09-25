$('.dropdown-menu a').on('click', function () {
    $('.dropdown-toggle').html($(this).html() + '<span class="caret"></span>');
})


$('.header__menu ul li a').click(function () {
    $(this).parent().addClass('active').siblings().removeClass('active');
});
