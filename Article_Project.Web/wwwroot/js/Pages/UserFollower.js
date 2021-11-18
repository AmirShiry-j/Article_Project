$(document).ready(function () {

    $(".btns-div button").click(function () {

        if (!($(this).hasClass("disable"))) {

            var btnClick = $(this);

            SetDisableFollow(btnClick);

            var UserIdFollower = $(this).attr("UserId");

            if (btnClick.hasClass("remove-btn")) {

                //REMOVE Follower

                $.ajax({
                    url: "/Follow/RemoveFollower/" + UserIdFollower,
                    method: "Delete",
                    success: function (res) {
                        if (res == true) {

                            setTimeout(function () {

                                SetEnableFollow(btnClick);

                                RemoveFollower(UserIdFollower);

                            }, 500);
                        }
                    },
                    error: function () {

                        SetEnableFollow(btnClick);

                        ShowModal();
                    }
                });

            }
            else if (btnClick.hasClass("following-btn")) {

                //REMOVE Following

                $.ajax({
                    url: "/Follow/RemoveUser/" + UserIdFollower,
                    method: "Delete",
                    success: function (res) {

                        if (res == true) {

                            setTimeout(function () {

                                SetEnableFollow(btnClick);

                                ChangeSituationFollow(UserIdFollower);

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
                    url: "/Follow/AddUser/" + UserIdFollower,
                    method: "Post",
                    success: function (res) {

                        if (res == true) {

                            setTimeout(function () {

                                SetEnableFollow(btnClick);

                                ChangeSituationFollow(UserIdFollower);

                            }, 500);
                        }
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
function ChangeSituationFollow(UserIdFollower) {

    $(".btns-div button[UserId='" + UserIdFollower + "']").toggleClass("d-none");

}
function RemoveFollower(UserIdFollower) {

    $(".follower-div[UserId='" + UserIdFollower + "'").fadeOut(function () {

        $(".follower-div[UserId='" + UserIdFollower + "'").remove();
    });

}

function SetEnableFollow(element) {

    element.removeClass("disable");
    element.css("cursor", "pointer");
}
function SetDisableFollow(element) {

    $(element).addClass("disable")
    $(element).css("cursor", "not-allowed");
}
//...End

function ShowModal() {

    $("#modal-account-btn").trigger("click");

}
