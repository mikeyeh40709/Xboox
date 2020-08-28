/////
let check = document.querySelector("#check");
let title = document.querySelector(".container .row .col-sm-12");
let card_section = document.querySelector(".card-section");
let h3 = document.createElement('h3');
let basket = document.querySelector('.basket');
let proceed = document.querySelector('.proceed');
//coupon badge
let badge = document.querySelector('.badge');



//
card_section.setAttribute('style', 'display:none');
basket.setAttribute('style', 'display:none');
proceed.setAttribute('style', 'display:none');
h3.innerText = "Your basket is currently empty.";
title.appendChild(h3);
check.addEventListener('click', () => {
    if (check.checked) {
        h3.setAttribute('style', 'display:none');
        card_section.setAttribute('style', 'display:block');
        basket.setAttribute('style', 'display:block');
        proceed.setAttribute('style', 'display:block');
    }
    else {
        h3.setAttribute('style', 'display:block');
        card_section.setAttribute('style', 'display:none');
        basket.setAttribute('style', 'display:none');
        proceed.setAttribute('style', 'display:none');
    }

})

//setting book price

let price_group = document.querySelectorAll('.price');
//let price_num = parseFloat(67.00);
//price.innerHTML = `$${price_num}`;

let caretleft_group = document.querySelectorAll('.fa-caret-left');
let count_group = document.querySelectorAll('.count');
let total_group = document.querySelectorAll('.total');
let caretright_group = document.querySelectorAll('.fa-caret-right');
let changeCount = () => {
    for (let i = 0; i < caretleft_group.length; i++) {
        caretleft_group[i].addEventListener('click', () => {
            if (count_group[i].innerText > 1) {
                count_group[i].innerText--;
                total_group[i].innerHTML = `$${parseFloat(price_group[i].innerText) * parseFloat(count_group[i].innerText)}`;
                total_function();
               
            }
        })

        caretright_group[i].addEventListener('click', () => {
            count_group[i].innerText++;
            total_group[i].innerHTML = `${parseFloat(price_group[i].innerText) * parseFloat(count_group[i].innerText)}`;
            total_function();
          
        }
        )
        total_group[i].innerHTML = `${parseFloat(price_group[i].innerText) * parseFloat(count_group[i].innerText)}`;
    }
}
//coupon badge
let coupon_price = document.querySelector('.coupon_price');
coupon_price.setAttribute('style', 'display:none');
////全部的總金額
let total_function = () => {
    let price_array = [];
    let usingMath = 0;
    for (var i = 0; i < total_group.length - 1; i++) {
        var unit_item_price = parseFloat(total_group[i].innerHTML);
        price_array.push(parseFloat(total_group[i].innerHTML));
    }
    price_array.forEach(ele => {
        usingMath = usingMath + ele;
    });
    total_group[total_group.length - 1].innerText = usingMath;
    coupon_function();
}



let coupon_function = () => {

    coupon_price.innerText = `$${(Math.ceil(parseFloat(total_group[total_group.length - 1].innerText) * 0.8))}`;
    badge.addEventListener('click', () => {
        coupon_price.setAttribute('style', 'display:block');
        //total_group[total_group.length - 1].style. = "line-through";
        total_group[total_group.length - 1].setAttribute("style", "color:rgba(0,0,0,0.3);text-decoration:line-through;") ;    
    });
 };

///Function Group
changeCount();
total_function();
coupon_function();
////////////////
////proceed  button click to next page
proceed.addEventListener('click', () => { location.href = 'Bill' });

///// localstorage
let data =
    [{
        "name": "Wellness And Paradise",
        "img": "~/Assets/Image/Pics/Wellness And Paradise.png",
        "total": `${total_group[1].innerText}`
    },
    {
        "name": "Wellness And Paradise",
        "img": "~/Assets/Image/Pics/Wellness And Paradise.png",
        "total": `${total_group[1].innerText}`
    }];

var dataString = JSON.stringify(data);
localStorage.setItem("first", dataString);

