$(document).ready(function () {

    $('.product__modal').click(function (e) {
        e.preventDefault();

        let url = $(this).attr('href');

        fetch(url).then(res =>
        {
            return res.text();
        }).then(data => {
            $('#product-modal').html(data);
        })
    })

    $('.searchInput').keyup(function () {

        $(".search-body").addClass("d-block");

        let search = $(this).val().trim();

        let url = $(this).data("url") 


        console.log(url)

        url = url + '?search=' + search


        if (search) {
            fetch(url).then(res => res.text()).then(data => {
                $('.search-body .list-group').html(data);
            })
        }
        else {
            $('.search-body .list-group').html('');
            $(".search-body").removeClass("d-block");
        }
    })

    // ------- Add To Cart ---------

    $('.AddToCart').click(function (e) {
        e.preventDefault();

        let url = $(this).attr('href');

        fetch(url).then(res => res.text())
            .then(data => {
                $('.cart-dropdown').html(data);
            });
    })

    // ------- Delete From Cart ---------

    $(document).on('click', '#deleteCart', function (e) {
        e.preventDefault();

        let url = $(this).attr('href');

        fetch(url)
            .then(res => res.text())
            .then(data => {
                $('.cart-dropdown').html(data);
            })
    })


    $(document).on('click', '#quantity-minus', function (e) {
        e.preventDefault();

        let inputCount = $(this).prev().prev().val();

        console.log(inputCount)

        if (inputCount >= 2) {
            inputCount--;
            $(this).prev().prev().val(inputCount);
            let url = $(this).attr('href') + '/?count=' + inputCount;

            fetch(url)
                .then(res => res.text())
                .then(data => {
                    $('#cartIndex').html(data);
                    fetch('/cart/getcart').then(res => res.text())
                        .then(data => {
                            $('.cart-dropdown').html(data);
                        });
                })
        }
    })


    $(document).on('click', '#quantity-plus', function (e) {
        e.preventDefault();

        let inputCount = $(this).prev().val();
        
        console.log(inputCount)

        if (inputCount > 0) {
            inputCount++;
        }
        else {
            inputCount = 1;
        }

        $(this).prev().val(inputCount);

        let url = $(this).attr('href') + '/?count=' + inputCount;

        fetch(url)
            .then(res => res.text())
            .then(data => {
                $('#cartIndex').html(data);
                fetch('/cart/getcart').then(res => res.text())
                    .then(data => {
                        $('.cart-dropdown').html(data);
                    });
            })
    })

    $(document).on('click', '.deleteFromCart', function (e) {
        e.preventDefault();

        fetch($(this).attr('href'))
            .then(res => res.text())
            .then(data => {
                $('#cartIndex').html(data);
                fetch('/cart/getcart').then(res => res.text())
                    .then(data => {
                        $('.cart-dropdown').html(data);
                    });
            })
    })

    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toast-top-left",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "3000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }

    if ($('#successInputAcc').val().length) {
        toastr["success"]($('#successInputAcc').val().slice(6), $('#successInputAcc').val().split(' ')[0]);
    }

    if ($('#successInputPassword').val().length) {
        toastr["success"]($('#successInputPassword').val().slice(6), $('#successInputPassword').val().split(' ')[0]);
    }

})