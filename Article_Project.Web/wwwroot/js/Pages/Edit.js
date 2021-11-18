$(document).ready(function () {

    $(".change-pro-btn").click(function () {

        $(".img-input").trigger("click");

    });

    //// Upload Image in img tag when Window be Show
    $(".img-input").change(function () {
        var input = this;
        var url = input.value;
        var ext = url.substring(url.lastIndexOf('.') + 1).toLowerCase();
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('.img-pro').attr('src', e.target.result);
            }
            reader.readAsDataURL(input.files[0]);
        }
        else {
            $('.img-pro').attr('src', '/assets/no_preview.png');
        }
    });

    $(".delete-pro-btn").click(function () {


        Swal.fire({

            title: 'توجه',
            text: "آیا از حذف کردن تصویر پروفایل خود مطمئن هستید؟",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'

        }).then((result) => {

            if (result.value == true) {

                $.ajax({
                    url: "/Account/DeleteImageProfile",
                    method: "get"
                }).done(function (res) {
                    if (res) {

                        Swal.fire(
                            'Deleted!',
                            'تصویر پروفایل با موفقیت حذف شد',
                            'success'
                        ).then(function () {

                            location.reload();

                        });

                    }
                    else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: 'عملیات ناموق بود و ظاهرا مشکلی وجود داره',
                            footer: '<a href="">Why do I have this issue?</a>'
                        })
                    }
                });
            }
            else if (result.isDenied) {

            }
        });



    });
});



  //Swal.fire({
        //    title: 'Are you sure?',
        //    text: "You won't be able to revert this!",
        //    icon: 'warning',
        //    showCancelButton: true,
        //    confirmButtonColor: '#3085d6',
        //    cancelButtonColor: '#d33',
        //    confirmButtonText: 'Yes, delete it!'
        //}).then((result) => {
        //    if (result.value==true) {
        //        Swal.fire(
        //            'Deleted!',
        //            'Your file has been deleted.',
        //            'success'
        //        )
        //    }
        //