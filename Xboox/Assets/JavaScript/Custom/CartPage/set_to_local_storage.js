﻿//homepage every product btn
let AddBtnGroup = $(".addCartBtn");

//navbar dom
let tip = document.querySelectorAll('.cart_count');
let headerdropdown = document.querySelector('span.icon_bag_alt~ul.headerdropdown');
// Cartpage dom
let cart_close_group = document.querySelectorAll(".cart__close");
// productDetailPage add cart btn
let productdetail_cart_btn = $('.product__details__button .cart-btn');
let productdetail_count_dom = document.querySelector('.product__details__button input');
let unistockDom = document.querySelector('.product__details__widget li:first-of-type p');
//swal_text
let sucess_html = '<h2 style="font-size:18px; font-family:Noto Sans TC, sans-serif;">成功加入購物車!</h2>';
let fail_html = '<h2 style="font-size:25px; font-family:Noto Sans TC, sans-serif;"><b>庫存沒了喔</b></h2>';
let nan_html = '<h2 style="font-size:25px; font-family:Noto Sans TC, sans-serif;"><b>請輸入數字喔</b></h2>';
//homepage product btn click event
AddBtnGroup.each(function () {
    $(this).on('click', function () {
        var imgLink = $(this).attr("data-img");
        var getProductName = $(this).attr("data-target");
        setLocalStorage(this.id, getProductName);
        headerdropdown.innerHTML = "";
        renewNavbar();
        swalSet(sucess_html, getProductName, imgLink );
    })
});
//productdetail's page product btn click event
productdetail_cart_btn.on('click', function () {
    var imgLink = $(this).attr("data-img");
    let unitStock = unistockDom.textContent;
    var getProductName = $(this).attr("data-target");
    if (!isNaN(productdetail_count_dom.value)) {
        if (parseInt(productdetail_count_dom.value) < parseInt(unitStock)) {
            setDetailsLocalStorage(this.id, getProductName);
            headerdropdown.innerHTML = "";
            renewNavbar();
            swalSet(sucess_html, getProductName, imgLink);
        }
        else {
            swalSet(fail_html, getProductName, imgLink);
        }
    }
    else {
        swalSet(nan_html);
    }
  
});

//honepage add items to localstorage 
function setLocalStorage(ProductId, ProductName) {
    let cartItems = [];
    let cartItem = {
        ProductName: ProductName,
        Count: 1,
        ProductId: ProductId
    }
    if (localStorage.length !== 0) {
        let getStorage = localStorage.getItem("CartItems");
        JSON.parse(getStorage).forEach(ele => {
            if (ele.ProductName == ProductName) {
                ele.Count = parseInt(ele.Count) + 1;
                cartItem.Count = ele.Count;
            }
            else {
                cartItems.push(ele);
            }
        })
        cartItems.push(cartItem);
        localStorage.setItem("CartItems", JSON.stringify(cartItems));
    }
    else {
        cartItems.push(cartItem);
        localStorage.setItem("CartItems", JSON.stringify(cartItems));
    }
}
////product detail add Multiple commodities to localstorage
function setDetailsLocalStorage(ProductId, ProductName) {
    let cartItems = [];
    let cartItem = {
        ProductName: ProductName,
        Count: productdetail_count_dom.value,
        ProductId: ProductId
    }
    if (localStorage.length !== 0) {
        let getStorage = localStorage.getItem("CartItems");
        JSON.parse(getStorage).forEach(ele => {
            if (ele.ProductName == ProductName) {
                ele.Count = parseInt(ele.Count) + parseInt(productdetail_count_dom.value)
                cartItem.Count = ele.Count;
            }
            else {
                cartItems.push(ele);
            }
        })
        cartItems.push(cartItem);
        localStorage.setItem("CartItems", JSON.stringify(cartItems));
    }
    else {
        cartItems.push(cartItem);
        localStorage.setItem("CartItems", JSON.stringify(cartItems));
    }
}
//CartPage can delete localstorage product
function deleteCartItem(event) {
    let productId = event.target.id;
    let getLocalItems = localStorage.getItem("CartItems");
    let ItemsArray = JSON.parse(getLocalItems);
    let FindItemIndex = ItemsArray.findIndex(x => x.ProductId == productId);
    if (FindItemIndex !== -1) {
        ItemsArray.splice(FindItemIndex, 1);
        localStorage.setItem("CartItems", JSON.stringify(ItemsArray));
    }
    //if (getLocalItems.length == 2) {
       
    //}

}
//Let homepage's navbar show  localstorage's products
function renewNavbar() {
    let getLocalStorage = localStorage.getItem("CartItems");
    let getItems = JSON.parse(getLocalStorage);
    if (getItems != null) {
        getItems.forEach(ele => {
            let li = document.createElement('li');
            let title = document.createElement('span');
            let count = document.createElement('span');
            title.textContent = ele.ProductName;
            count.textContent = ele.Count;
            li.appendChild(title);
            li.appendChild(count);
            headerdropdown.append(li);
            headerdropdown.setAttribute('style', 'overflow: scroll;overflow-x:hidden; height:300px;');
        });
        var arrayFromStroage = JSON.parse(localStorage.getItem("CartItems"));
        var arrayLength = arrayFromStroage.length;
        tip.forEach(x => x.textContent = arrayLength);
    }
}
//1.  .header span.icon_bag_alt    /Cart/AddToCart
//ajax  post type 
ajaxFun('.header span.icon_bag_alt', '/Cart/AddToCart');
ajaxFun('#phoneCart', '/Cart/AddToCart');

function ajaxFun(clickName, ajaxUrl) {
    $(clickName).click(function (e) {
        e.preventDefault();
        let getLocalStorage = localStorage.getItem("CartItems");
        if (getLocalStorage != null) {
            $.ajax({
                url: ajaxUrl,
                data: { values: getLocalStorage },
                dataType: "json",
                type: 'post',
                success: function (data) {
                    location.href = data.redirectToUrl;
                },
                error: function (xhr, thrownError) {
                    console.log(xhr.status);
                    console.log(thrownError);
                }
            })
        }
        else {
            Swal.fire("", "請先加入商品至購物車哦!!", "error")
        }
    })
}
function swalSet(htmlContext,getProductName="", img_link = "") {

    Swal.fire({
        title: `${getProductName}`,
        html: `${htmlContext}`,
        imageUrl: `${img_link}`,
        imageWidth: 200,
        imageHeight: 200,
        imageAlt: 'Image Broken',
        timer: 800,
        timerProgressBar: true,
        onBeforeOpen: () => {
            Swal.showLoading()
        },
    });
}
renewNavbar();
window.addEventListener("load", function () {
    headerdropdown.innerHTML = "";
    renewNavbar();
   
   
})


