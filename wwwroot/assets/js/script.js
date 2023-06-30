$(document).ready(function () {

    $('.product__modal').click(function (e) {
        e.preventdefault();

        let url = $(this).attr('href');

        fetch(url).then(res =>
        {
            return res.text();
        }).then(data => {
            $('#product-modal').html(data);
        })
    })

    $('.searchinput').keyup(function () {

        $(".search-body").addclass("d-block");

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
            $(".search-body").removeclass("d-block");
        }
    })

    // ------- add to cart ---------

    $('.AddToCart').click(function (e) {
        e.preventdefault();

        let url = $(this).attr('href');

        fetch(url).then(res => res.text())
            .then(data => {
                $('.cart-dropdown').html(data);
            });
    })

    // ------- delete from cart ---------

    $(document).on('click', '#deletecart', function (e) {
        e.preventdefault();

        let url = $(this).attr('href');

        fetch(url)
            .then(res => res.text())
            .then(data => {
                $('.cart-dropdown').html(data);
                //fetch('.cart-dropdown')
                //    .then(res => res.text())
                //    .then(data => {
                //        $('#cartindex .cart-table tbody tr').html(data);
                //    })
            })
    })


    $(document).on('click', '#quantity-minus', function (e) {
        e.preventdefault();

        let inputcount = $(this).prev().prev().val();

        console.log(inputcount)

        if (inputcount >= 2) {
            inputcount--;
            $(this).prev().prev().val(inputcount);
            let url = $(this).attr('href') + '/?count=' + inputcount;

            fetch(url)
                .then(res => res.text())
                .then(data => {
                    $('#cartindex').html(data);
                    fetch('/cart/getcart').then(res => res.text())
                        .then(data => {
                            $('.cart-dropdown').html(data);
                        });
                })
        }
    })


    $(document).on('click', '#quantity-plus', function (e) {
        e.preventdefault();

        let inputcount = $(this).prev().val();
        
        console.log(inputcount)

        if (inputcount > 0) {
            inputcount++;
        }
        else {
            inputcount = 1;
        }

        $(this).prev().val(inputcount);

        let url = $(this).attr('href') + '/?count=' + inputcount;

        fetch(url)
            .then(res => res.text())
            .then(data => {
                $('#cartindex').html(data);
                fetch('/cart/getcart').then(res => res.text())
                    .then(data => {
                        $('.cart-dropdown').html(data);
                    });
            })
    })

    $(document).on('click', '.deletefromcart', function (e) {
        e.preventdefault();

        fetch($(this).attr('href'))
            .then(res => res.text())
            .then(data => {
                $('#cartindex').html(data);
                fetch('/cart/getcart').then(res => res.text())
                    .then(data => {
                        $('.cart-dropdown').html(data);
                    });
            })
    })

    toastr.options = {
        "closebutton": true,
        "debug": false,
        "newestontop": false,
        "progressbar": true,
        "positionclass": "toast-top-left",
        "preventduplicates": false,
        "onclick": null,
        "showduration": "300",
        "hideduration": "1000",
        "timeout": "3000",
        "extendedtimeout": "1000",
        "showeasing": "swing",
        "hideeasing": "linear",
        "showmethod": "fadein",
        "hidemethod": "fadeout"
    }

    if ($('#successinputacc').val().length) {
        toastr["success"]($('#successinputacc').val().slice(6), $('#successinputacc').val().split(' ')[0]);
    }

    if ($('#successinputpassword').val().length) {
        toastr["success"]($('#successinputpassword').val().slice(6), $('#successinputpassword').val().split(' ')[0]);
    }

})

function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#imagePreview').css('background-image', 'url(' + e.target.result + ')');
            $('#imagePreview').hide();
            $('#imagePreview').fadeIn(650);
        }
        reader.readAsDataURL(input.files[0]);
    }
}
$("#imageUpload").change(function () {
    readURL(this);
});

$(".ratingStar").click(function () {
    var starValue = $(this).attr("data-value");

    $("#ratingValue").val(starValue);
});

$(".ratingStar").hover(function () {
    $(".ratingStar").addClass("far").removeClass("fas");

    $(this).addClass("fas").removeClass("far");
    $(this).prevAll(".ratingStar").addClass("fas").removeClass("far");
});