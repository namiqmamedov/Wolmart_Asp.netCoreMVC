﻿$(document).ready(function () {
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
        }
    })
})