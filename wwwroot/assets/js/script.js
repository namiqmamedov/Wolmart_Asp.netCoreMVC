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

    $('.addtocart').click(function (e) {
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

//FilePond.registerPlugin(
//	FilePondPluginImageCrop,
//	FilePondPluginImagePreview,
//	FilePondPluginImageResize,
//	FilePondPluginImageTransform
//);

//const inputElement = document.querySelector('input[type="file"]');
//const pond = FilePond.create(inputElement, {
//	imageCropAspectRatio: 1,
//	imageResizeTargetWidth: 256,
//	imageResizeMode: 'contain',
//	imageTransformVariants: {
//		thumb_medium_: transforms => {
//			transforms.resize.size.width = 512;
//			transforms.crop.aspectRatio = .5;
//			return transforms;
//		},
//		thumb_small_: transforms => {
//			transforms.resize.size.width = 64;
//			return transforms;
//		}
//	},
//	onaddfile: (err, fileItem) => {
//		console.log(err, fileItem.getMetadata('resize'));
//	},
//	onpreparefile: (fileItem, outputFiles) => {
//		outputFiles.forEach(output => {
//			const img = new Image();
//			img.src = URL.createObjectURL(output.file);
//			document.body.appendChild(img);
//		})
//	}
//});

FilePond.registerPlugin(
    // encodes the file as base64 data
    FilePondPluginFileEncode,
    // validates the size of the file
    FilePondPluginFileValidateSize,
    // corrects mobile image orientation
    FilePondPluginImageExifOrientation,
    // previews dropped images
    FilePondPluginImagePreview
);
// Select the file input and use create() to turn it into a pond
FilePond.create(document.querySelector('.filepond'));

