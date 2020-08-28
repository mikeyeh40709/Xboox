
//get html dom
let cart_total = document.querySelectorAll('.cart__total');
let cart_price = document.querySelectorAll('.cart__price');
let cart_count = document.querySelectorAll('.cart__count');
let discounted_price = document.querySelector('.discounted_price');
let site_btn = document.querySelector('.site-btn');
let dec_group = document.querySelectorAll('.dec');
let inc_group = document.querySelectorAll('.inc');

//decrease or increase button event!
let dec_inc_func = (dec_group, inc_group, num) => {
    dec_group[num].addEventListener('click', () => { if (cart_count[num].innerText > 1) { cart_count[num].innerText-- }; item_func(num);total_function(); });
    inc_group[num].addEventListener('click', () => { cart_count[num].innerText++; item_func(num); total_function(); });
}
//every item price function!
let item_func = (num) => { cart_total[num].innerHTML = `${parseFloat(cart_price[num].innerText) * parseFloat(cart_count[num].innerText)}` };
//total item price function!
let total_function = () => {
    let price_array = [];
    let usingMath = 0;
    for (var i = 0; i < cart_total.length - 1; i++) {
        var unit_item_price = parseFloat(cart_total[i].innerText);
        price_array.push(unit_item_price);
    }
    price_array.forEach(ele => {
        usingMath = usingMath + ele;
    });
    cart_total[cart_total.length - 1].innerText = usingMath;
    coupon_function();
}
let = cart_count_fun = () => {
    cart_count.forEach((ele, idx) => cart_count[idx].innerText = 1);
}
//Cauculate count func
let changeCount = () => {
    for (let i = 0; i < cart_count.length; i++) {
        item_func(i);
        total_function();
        dec_inc_func(dec_group, inc_group, i);
    }
}
////coupon price fun
//using btn_open_close to control discounted's price . 
let btn_poen_close = false;
let coupon_function = () => {
    if (btn_poen_close) {
        discounted_price.innerText = `${Math.ceil(parseFloat(cart_total[cart_total.length - 1].innerText) * 0.9)}`;    
    }
    else {
        discounted_price.innerText = cart_total[cart_total.length - 1].innerText;
    }
    site_btn.addEventListener('click', (e) => {
        btn_poen_close = true;
        e.preventDefault();
        discounted_price.innerText = `${Math.ceil(parseFloat(cart_total[cart_total.length - 1].innerText) * 0.9)}`;    
        cart_total[cart_total.length - 1].setAttribute("style", "color:rgba(0,0,0,0.3);text-decoration:line-through;") ;    
    });
 };
if (cart_count.length > 0) {
    cart_count_fun();
    changeCount();
    coupon_function();
}

//////proceed  button click to next page
//proceed.addEventListener('click', () => { location.href = 'Bill' });

/////// localstorage
//let data =
//    [{
//        "name": "Wellness And Paradise",
//        "img": "~/Assets/Image/Pics/Wellness And Paradise.png",
//        "total": `${total_group[1].innerText}`
//    },
//    {
//        "name": "Wellness And Paradise",
//        "img": "~/Assets/Image/Pics/Wellness And Paradise.png",
//        "total": `${total_group[1].innerText}`
//    }];

//var dataString = JSON.stringify(data);
//localStorage.setItem("first", dataString);

