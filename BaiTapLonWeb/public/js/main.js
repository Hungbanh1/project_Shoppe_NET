function getValue(id) {
    return document.getElementById(id).value.trim();
}

function showError(id, mess) {
    document.getElementById(id + '_error').innerHTML = mess;
}

function validate() {
    var flag = true;

    var email = getValue('email');
    var mailformat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
    var password = getValue('password');


    if (email == '' && password == '') {
        // flag = false;
        // showError('email', 'Email không đúng định dạng !')
        alert("Validate không được để trống !!!");

    } else if (email == '' || password == '') {

        // showError('email', 'Email không đúng định dạng !')
        alert("Không được để trống 1 trong 2 trường !!!");
        // return false


        if (email == '') {
            flag = false;

            showError('email', 'Email không được bỏ trống !')
        } else if (!mailformat.test(email)) {
            flag = false;

            showError('email', 'Email không đúng định dạng !')
                // console.log('email sai');
        }

        if (password == '') {
            flag = false;
            showError('password', 'Password không được để trống !')
        } else if (password.length < 8) {
            flag = false;
            showError('password', 'Password không đúng định dạng !')
        }
        return false;
    } else {
        var closable = alertify.alert().setting('closable');
        alertify.alert()
            .setting({
                'label': 'DONE',
                'message': 'Chúc mừng bạn đã submit form thành công !!!',
                'onok': function() {
                    alertify.success('Thành công !!!');
                    setInterval(function() {
                        location.reload();
                    }, 1800);
                }
            }).show();
    }
}

// jquery 
$(document).ready(function() {

    $(".product-suggest").hover(function() {
        $('.mau-sac').removeClass('active');
        // $('.mau-sac').removeClass('test');
        // $(this).children('.buy-now').addClass('test');
        $(this).children('.buy-now').toggleClass('active');
        return false;
    });
    $('.owl-carousel').owlCarousel({
        loop: true,
        margin: 10,
        nav: true,
        navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],
        dots: false,
        responsive: {
            0: {
                items: 2
            },
            600: {
                items: 3
            },
            1000: {
                items: 4
            }
        }
    })

})