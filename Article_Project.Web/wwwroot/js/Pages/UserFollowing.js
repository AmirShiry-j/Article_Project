$(document).ready(function () {

    $(".btns-div button").click(function () {

        if (!($(this).hasClass("disable"))) {

            var btnClick = $(this);

            SetDisableFollow(btnClick);

            var UserIdFollowing = $(this).attr("UserId");

            if (btnClick.hasClass("following-btn")) {

                //REMOVE Following

                $.ajax({
                    url: "/Follow/RemoveUser/" + UserIdFollowing,
                    method: "Delete",
                    success: function (res) {

                        if (res == true) {

                            setTimeout(function () {

                                SetEnableFollow(btnClick);

                                ChangeSituationFollowing(UserIdFollowing);

                            }, 500);
                        }
                        
                    },
                    error: function () {

                        SetEnableFollow(btnClick);

                        ShowModal();
                    }
                });

            }
            else if (btnClick.hasClass("follow-btn")) {

                //ADD Following

                $.ajax({
                    url: "/Follow/AddUser/" + UserIdFollowing,
                    method: "Post",
                    success: function (res) {

                        setTimeout(function () {

                            SetEnableFollow(btnClick);

                            ChangeSituationFollowing(UserIdFollowing);

                        }, 500);
                    },
                    error: function () {

                        SetEnableFollow(btnClick);

                        ShowModal();
                    }
                });
            }
        }
    });
});

//Start...
//Follow
function ChangeSituationFollowing(UserIdFollowing) {

    $(".btns-div button[UserId='" + UserIdFollowing + "']").toggleClass("d-none");

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
