$(document).ready(function () {
    var countTxt = 1;
    var countImg = 0;
    //// Insert Text Area for Writing
    $("#text-btn").click(function () {
        countTxt++;
        $("#services-div").before("<textarea class='text-textarea-" + countTxt + " autoExpand' rows='1' data-min-rows='1' placeholder='هر چی دلت میخواد بنویس...'></textarea>");

        $(".order-input").val($(".order-input").val() + "#*$" + "t" + countTxt);


    });
        //// End Insert Text Area for Writing

    //// Show Image File Dialog

    $("#img-btn").click(function () {

        countImg++;


        $("#submit-input").before("<input class='img-input-" + countImg + "' type='file' accept='.png , .jpeg, .jpg' name='formFiles' />");

        //

        $(".img-input-" + countImg).change(function () {

            $("#services-div").before("<img class='img-img-" + countImg + " img-tag my-5' />");


            var input = this;
            var url = input.value;
            var ext = url.substring(url.lastIndexOf('.') + 1).toLowerCase();
            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    $('.img-img-' + countImg).attr('src', e.target.result);
                }
                reader.readAsDataURL(input.files[0]);
            }
            else {
                $('.img-pro').attr('src', '/assets/no_preview.png');
            }


            $(".order-input").val($(".order-input").val() + "#*$" + "p" + countImg);


        });

        $(".img-input-" + countImg).trigger("click");

    });

    //// End Show Image File Dialog

    // Send Form

    $("#test-show-btn").click(function (e) {


        $(".title-inputt").val($(".title-input").val());

        var txts= $("textarea");

        $(".texts-input").val("");

        for (let i = 0; i < txts.length; i++) {

            $(".texts-input").val($(".texts-input").val() + txts[i].value + "#*$");

        }

        var flagSend = true;

        if ($(".title-inputt").val() == "") {

            $(".modal-1 ul").append("<li>باید برای مقاله خود یک عنوان وارد کنید</li>")
            flagSend = false;

        }

        if ($(".texts-input").val().length < 300) {

            $(".modal-1 ul").append("<li>مقاله شما حداقل باید دارای 300 کاراکتر باشد</li>");
            flagSend = false;
        }


        if ($(".order-input").val().indexOf("p")==-1) {

            $(".modal-1 ul").append("<li>مقاله شما حداقل باید دارای یک تصویر باشد</li>");
            flagSend = false;

        }


        if (flagSend) {

            $("#Show-modal-btn-2").trigger("click");

        }
        else {

            $("#Show-modal-btn-1").trigger("click");

        }
    });

    //End Send Form

    // Validation


    // End Validation


    $("#close-btn-1").click(function () {

        $(".modal-1 ul li").remove();

    });

    $("#send-post-btn").click(function () {

        $(".tags-input").val($(".tags-inputt").val());
        $(".subject-input").val($("#subject-select option:selected").val());

        $("#memory-form").submit();
    });
});


////   Text Area Size
document.addEventListener('input', onExpandableTextareaInput)
function onExpandableTextareaInput({ target: elm }) {

    if (!elm.classList.contains('autoExpand') || !elm.nodeName == 'TEXTAREA') return

    var minRows = elm.getAttribute('data-min-rows') | 0, rows;
    !elm._baseScrollHeight && getScrollHeight(elm)

    elm.rows = minRows
    rows = Math.ceil((elm.scrollHeight - elm._baseScrollHeight) / 35)
    elm.rows = minRows + rows
}
function getScrollHeight(elm) {
    var savedValue = elm.value
    elm.value = ''
    elm._baseScrollHeight = elm.scrollHeight
    elm.value = savedValue
}
////   End Text Area Size

