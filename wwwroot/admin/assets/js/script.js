$(document).ready(function () {

    $(document).on('click', '#deleteBtn', function (e) {
        e.preventDefault();

        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {

                let url = $(this).attr('href');

                fetch(url).then(res => res.text())
                    .then(data => { $('.table-content').html(data) });


                Swal.fire(
                    'Deleted!',
                    'Your file has been deleted.',
                    'success'
                )
            }
        })
    })

    $(document).on('click', '#restoreBtn', function (e) {
        e.preventDefault();

        Swal.fire({
            title: 'Are you sure?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, restore !'
        }).then((result) => {
            if (result.isConfirmed) {

                let url = $(this).attr('href');

                fetch(url).then(res => {
                    if (res.status == 400) {
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: 'Something went wrong!',
                            footer: '<a href="">Why do I have this issue?</a>'
                        })
                    }
                    else {

                        Swal.fire(
                            'Restore!',
                            'Your file has been restored.',
                            'success'
                        )

                        return res.text();
                    }
                    
                }).then(data => { $('.table-content').html(data) });

            }
        })
    })   

    $(document).on('click', '.deleteProductImage', function (e) {
        e.preventDefault();

        fetch($(this).attr('href'))
            .then(res => res.text())
            .then(data => {
                $('#productImages').html(data);
            })
    })

    if ($('.isMain').is(":checked")) {
        $('.imageContainer').removeClass('d-none')
        $('.parentContainer').addClass('d-none')
    }
    else {
        $('.imageContainer').addClass('d-none')
        $('.parentContainer').removeClass('d-none')
    }

    $(document).on('change', '.isMain', function () {
        if ($(this).is(":checked")) {
            $('.imageContainer').removeClass('d-none')
            $('.parentContainer').addClass('d-none')
        }
        else {
            $('.imageContainer').addClass('d-none')
            $('.parentContainer').removeClass('d-none')
        }
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

    if ($('#successInput').val().length) {
        toastr["success"]($('#successInput').val().slice(6),$('#successInput').val().split(' ')[0]);
    }

})