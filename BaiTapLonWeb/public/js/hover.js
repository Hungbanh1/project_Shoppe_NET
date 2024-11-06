$(document).ready(function() {

    $(".product-suggest").hover(function() {
   
        $(this).children('.buy-now').toggleClass('active');
        return false;
    });
})

$(document).ready(function() {

    $(".user-login").hover(function() {

        $(this).children('.box-order-profile-user').toggleClass('visible');
        return false;
    });
})
$(document).ready(function() {

    $("#fa-shopping-cart").hover(function() {

        $(this).children('.cart-img-none').toggleClass('visible');
        return false;
    });
})