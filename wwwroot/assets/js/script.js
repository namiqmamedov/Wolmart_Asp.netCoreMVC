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
})