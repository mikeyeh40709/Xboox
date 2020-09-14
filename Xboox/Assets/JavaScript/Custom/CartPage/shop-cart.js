let cart_total = document.querySelectorAll('.cart__total');
let cart_price = document.querySelectorAll('.cart__price');
let cart_count = document.querySelectorAll('.cart__count');
let discounted_price = document.querySelector('.discounted_price');
let site_btn = document.querySelector('.site-btn');
let decrease = document.querySelectorAll('.decrease');
//decrease or increase button event!
function CalculateCount(cauculate, idx) {
    if (cauculate ==='-' && cart_count[idx].textContent <= 1) {
    }
    else {
        change_localstorage(cauculate, idx);
        ajaxFun(this, '/Cart/AddToCart');
        item_func(idx);
        total_function();    
    }
   
    
};

//change_count_to_change_localstorage_function
let change_localstorage = (operator,num) => {
    let getStorage = localStorage.getItem("CartItems");
    let cartItems = [];
    let cartItem = {
        ProductName: cart_count[num].getAttribute('data-target'),
        Count: cart_count[num].textContent,
        ProductId: cart_count[num].id
    } 
    JSON.parse(getStorage).forEach(ele => {
        if (ele.ProductId == cart_count[num].id) {
            if (operator=='+') {
                ele.Count = parseInt(ele.Count) + parseInt(1);
            }
            else {
                ele.Count = parseInt(ele.Count)-parseInt(1);
            }
            cartItem.Count = ele.Count;
        }
        else {
            cartItems.push(ele);
        }
    })
    cartItems.push(cartItem);
    localStorage.setItem("CartItems", JSON.stringify(cartItems));
    headerdropdown.innerHTML = "";
    renewNavbar();
};
//every item price function!
let item_func = (num) => { cart_total[num].innerHTML = `${parseFloat(cart_price[num].innerText) * parseFloat(cart_count[num].textContent)}` };
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
    //coupon_function();
}
//Cauculate count func
let changeCount = () => {
    for (let i = 0; i < cart_count.length; i++) {
        item_func(i);
        total_function();
    }
}
////coupon price fun
//using btn_open_close to control discounted's price . 
//let btn_poen_close = false;
//let coupon_function = () => {
//    if (btn_poen_close) {
//        discounted_price.innerText = `${Math.ceil(parseFloat(cart_total[cart_total.length - 1].innerText) * 0.9)}`;    
//    }
//    else {
//        discounted_price.innerText = cart_total[cart_total.length - 1].innerText;
//    }
//    site_btn.addEventListener('click', (e) => {
//        btn_poen_close = true;
//        e.preventDefault();
//        discounted_price.innerText = `${Math.ceil(parseFloat(cart_total[cart_total.length - 1].innerText) * 0.9)}`;    
//        cart_total[cart_total.length - 1].setAttribute("style", "color:rgba(0,0,0,0.3);text-decoration:line-through;") ;    
//    });
//};

if (cart_count.length > 0) {
    changeCount();
    //coupon_function();
}

function UseCoupon(event) {
    let couponCode = (event.target.id);
    document.getElementById("discountCode").value = couponCode.split('+')[0];
    document.getElementById("discountRate").value = couponCode.split('+')[1];
    $('#couponModalCenter').modal('hide');
}

