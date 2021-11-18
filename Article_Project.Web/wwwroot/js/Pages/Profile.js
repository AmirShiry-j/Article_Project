$(document).ready(function () {

    $(".follows-div button").click(function () {

        if (!($(this).hasClass("disable"))) {

            btnElement = $(this);

            SetDisableFollow(btnElement);

            var hasFollowed = $(this).hasClass("following-btn");

            var userId = $(".follows-div").attr("UserId");

            if (hasFollowed) {
                //Remove Follow

                $.ajax({
                    url: "/Follow/RemoveUser/" + userId,
                    method: "Delete",
                    success: function (res) {

                        if (res == true) {

                            setTimeout(function () {

                                SetEnableFollow(btnElement);

                                ChangeSituationFollow();

                            }, 500);
                        }
                    },
                    error: function () {

                        SetEnableFollow(btnElement);

                        ShowModal();
                    }
                });
            }
            else {
                //Add Follow

                $.ajax({
                    url: "/Follow/AddUser/" + userId,
                    method: "Post",
                    success: function (res) {

                        if (res == true) {

                            setTimeout(function () {

                                SetEnableFollow(btnElement);

                                ChangeSituationFollow();

                            }, 500);
                        }
                    },
                    error: function () {

                        SetEnableFollow(btnElement);

                        ShowModal();

                    }
                });
            }

        }
        
    });

});
//Start...
//Follow

function ChangeSituationFollow() {

    $(".follows-div button").toggleClass("d-none");

}
function SetDisableFollow(element) {

    $(element).addClass("disable")
    $(element).css("cursor", "not-allowed");
}
function SetEnableFollow(element) {

    element.removeClass("disable");
    element.css("cursor", "pointer");
}
//...End

function ShowModal() {

    $("#modal-account-btn").trigger("click");

}
