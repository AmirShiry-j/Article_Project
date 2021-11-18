$(document).ready(function () {
    $(".btn-search-div").click(function () {

        var tag = $(".tag-input").val();

        if (tag != "") {

            window.location.replace("/Post/Search/" + tag);
        }
        else {
            $(".tag-input").focus();
        }

    });
});

