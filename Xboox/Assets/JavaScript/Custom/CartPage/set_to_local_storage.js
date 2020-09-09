let AddBtnGroup = $(".addCartBtn");
let tip = document.querySelector('.cart_count');
let cart_close_group = document.querySelectorAll(".cart__close");

//set localstorage count
function setLocalStorage(ProductId, ProductName ) {
    let cartItems = [];
    let cartItem = {
        ProductName: ProductName,
        Count: 1,
        ProductId:ProductId
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
let headerdropdown = document.querySelector('span.icon_bag_alt~ul.headerdropdown');



localStorageFun();

window.addEventListener("ready", function () {

    localStorageFun();
   
})
AddBtnGroup.each(function() {
    console.log(this);
    $(this).on('click', function () {
        var getProductName = $(this).attr("data-target");
        setLocalStorage(this.id, getProductName);
        headerdropdown.innerHTML = "";
        localStorageFun();
        swal(`${getProductName}`,"成功加入購物車!!", "success");
    })
});
//deleteCartItem();
//CartPage can delete localstorage product
function deleteCartItem(event) {
    let productId = event.target.id;
    let getLocalItems = localStorage.getItem("CartItems");
    console.log(getLocalItems);
    let ItemsArray = JSON.parse(getLocalItems);
    let FindItemIndex = ItemsArray.findIndex(x => x.ProductId == productId);
    if (FindItemIndex!==-1) {
        ItemsArray.splice(FindItemIndex, 1);
        localStorage.setItem("CartItems", JSON.stringify(ItemsArray));
    }
    //cart_close_group.forEach((ele, idx) => ele.addEventListener('click', () => {
    //    if (ItemsArray[idx].ProductId == ele.id) {
    //        ItemsArray.splice(idx, 1);
    //    }
    //    localStorage.removeItem("CartItems");
    //    localStorage.setItem("CartItems", JSON.stringify(ItemsArray));

    //}));
}

//Let homepage's products save to localstorage
function localStorageFun() {

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
        });

        var arrayFromStroage = JSON.parse(localStorage.getItem("CartItems"));
        var arrayLength = arrayFromStroage.length;

        tip.textContent = arrayLength;
    }

}

//ajax  post type 
$(".header span.icon_bag_alt").click(function (e) {
    e.preventDefault();
    let getLocalStorage = localStorage.getItem("CartItems");
    console.log(`{values : ${getLocalStorage}}`);
    if (window.localStorage !== undefined) {
        $.ajax({
            url: '/Cart/AddToCart',
            data: { values: getLocalStorage },
            dataType: "json",
            type: 'post',
            success: function (data) {
                console.log(data.redirectToUrl);
                location.href = data.redirectToUrl;
                console.log(data);
                console.log(typeof data);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr.status);
                console.log(thrownError);
            }
        })
    }
})









