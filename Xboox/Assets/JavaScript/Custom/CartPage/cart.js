
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

let price = document.querySelector('.price');
let price_num = parseFloat(67.00);
price.innerHTML = `$${price_num}`;

let caretleft = document.querySelector('.fa-caret-left');
let count = document.querySelector('#count');
let total_group = document.querySelectorAll('.total');
let caretright=document.querySelector('.fa-caret-right');
let changeCount = () => {
    caretleft.addEventListener('click', () => {
        if (count.innerText > 1) {

            count.innerText--;
            total_group.forEach((element, idx) => { total_group[idx].innerHTML = `$${price_num * count.innerText}`; })

        }
    })
    
    caretright.addEventListener('click', () => {
        count.innerText++;
        total_group.forEach((element, idx) => { total_group[idx].innerHTML = `$${price_num * count.innerText}`; })

    }
        )
}
total_group.forEach((element, idx) => { total_group[idx].innerHTML = `$${price_num * count.innerText}`; })
changeCount();


////proceed  click to next page
proceed.addEventListener('click', () => { location.href = 'Bill' });

//coupon badge
let coupon_price = document.querySelector('.coupon_price');
badge.addEventListener('click', () => {
    total_group[1].style.textDecoration = "line-through";
    coupon_price.innerText = `$${(Math.ceil(price_num * count.innerText * 0.8))}`;



});
