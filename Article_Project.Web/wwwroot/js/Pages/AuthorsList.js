$(document).ready(function () {

    $(".btns-div button").click(function () {

        var UserIdAuthor = $(this).attr("UserId");
        var btnClick = $(this);

        if (btnClick.hasClass("following-btn")) {

            //REMOVE Following

            $.ajax({
                url: "/Accounting/ChangeFollowing",
                method: "get",
                data: { FollowTo: UserIdAuthor, flagInsert: false }
            }).done(function (res) {

                SetChangeFollowing(res, UserIdAuthor);

            });

        }
        else if (btnClick.hasClass("follow-btn")) {

            //ADD Following

            $.ajax({
                url: "/Accounting/ChangeFollowing",
                method: "get",
                data: { FollowTo: UserIdAuthor, flagInsert: true }
            }).done(function (res) {

                SetChangeFollowing(res, UserIdAuthor);

            });

        }

    });
});

function SetChangeFollowing(flagLogin, UserIdAuthor) {

    if (flagLogin) {

        $(".btns-div button[UserId='" + UserIdAuthor + "']").toggleClass("d-none");

    }
    else {

        ShowModal();

    }
}

function ShowModal() {

    $("#modal-account-btn").trigger("click");

}
