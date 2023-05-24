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

        let search = $(this).val();
        console.log(search)

        let url = $(this).data("url")
        console.log(url)

        url = url + '?search=' + search
        console.log(url);

        fetch(url).then(res => {
            return res.json();
        }).then(data => {

            //<li class="list-group-item d-flex align-items-center">
            //    <img src="~/assets/images/products/brand/brand-1.jpg" style="width: 100px" alt="Image" />
            //    <h5>brand</h5>
            //</li>

            console.log(data);
        })
    })
})