$(document).ready(function() {
    $('.nav-link.active .sub-menu').slideDown();
    // $("p").slideUp();

    $('#sidebar-menu .arrow').click(function() {
        $(this).parents('li').children('.sub-menu').slideToggle();
        $(this).toggleClass('fa-angle-right fa-angle-down');
    });

    $("input[name='checkall']").click(function() {
        var checked = $(this).is(':checked');
        $('.table-checkall tbody tr td input:checkbox').prop('checked', checked);
    });

    $("input[name='checkall']").click(function() {
        var checked = $(this).is(':checked');
        $('#select-box label input:checkbox').prop('checked', checked);
    });

    //Products
    $("input[name='check-box']").click(function() {
        var checked = $(this).is(':checked');
        $('#check-boxlabel input:checkbox').prop('checked', checked);
    });
    //Post
    $("input[name='check-box-post']").click(function() {
        var checked = $(this).is(':checked');
        $('#check-box-post label input:checkbox').prop('checked', checked);
    });

    //page
    $("input[name='check-box-page']").click(function() {
        var checked = $(this).is(':checked');
        $('#check-box-page label input:checkbox').prop('checked', checked);
    });
    //Product

    $("input[name='check-box-product']").click(function() {
        var checked = $(this).is(':checked');
        $('#check-box-product label input:checkbox').prop('checked', checked);
    });

    //role

    $("input[name='check-box-role']").click(function() {
        var checked = $(this).is(':checked');
        $('#check-box-role label input:checkbox').prop('checked', checked);
    });

    //member

    $("input[name='check-box-member']").click(function() {
        var checked = $(this).is(':checked');
        $('#check-box-member label input:checkbox').prop('checked', checked);
    });

    //order

    $("input[name='check-box-order']").click(function() {
        var checked = $(this).is(':checked');
        $('#check-box-order label input:checkbox').prop('checked', checked);
    });

    $("input[name='check-box']").click(function(){
        $(this).parents('#check-box').find('.checkbox_child').prop('checked',$(this).prop('checked'));
    })
    //tim toi thang cha xong find thang con

    // $("input[name='check-box']").click(function() {
    //     var checked = $(this).is(':checked');
    //     $('.card-body label input:checkbox').prop('checked', $(this).prop('checked'));
    // });


  



   
});