
$(document).ready(function () {


    var flag = true;

    $(".add-comment-div").click(function () {
        $(".replay-comment-input").val(0);
        $(".title-comment-div span").text("دیدگاه خود را درباره این پست بنویسید");

        $(".write-comment-div").toggleClass("d-none");
    });

    $("textarea").keyup(function () {
        if ($(this).val() != "") {
            $(".send-comment-btn").removeClass("disabled");
        }
        else {
            $(".send-comment-btn").addClass("disabled");
        }

    });

    $(".send-comment-btn").click(function (e) {

        if ($("textarea").val().length == 0) {
            e.preventDefault();
        }
        else {

            $.ajax({
                url: "/Comment/Add",
                method: "Post",
                data: { PostId: $(".post-id-input").val(), TextComment: $(".text-comment-input").val(), ReplayComment: $(".replay-comment-input").val() },
                success: function (result) {
                    if (result == true) {
                        window.location.reload();
                    }
                },
                error: function () {
                    window.location.replace("/Error");
                }
            });
        }

    });

    $(".reply-btn").click(function () {

        var element = $(this);

        $.ajax({
            url: "/Account/HasLogin",
            method: "get"
        }).done(function (res) {

            if (res) {
                $(".replay-comment-input").val(element.attr("comment-id"));
                $(".title-comment-div span").text("جوابتو خودتو میتونی اینجا بنویسی");

                $(".write-comment-div").removeClass("d-none");
                $(".text-comment-input").focus();
            }
            else {
                ShowModal();
            }
        });
    });

    $(".like-div").click(function () {

        if (!($(this).hasClass("disable"))) {


            LikeElement = $(this);

            SetDisableLike(LikeElement);

            var hasLiked = $(".like-div i").hasClass("fa-heart");
            var postId = $(".post-id-input").val();

            if (hasLiked == true) {

                //Remove Like

                $.ajax({
                    url: "/Like/Remove/" + postId,
                    method: "Delete",
                    success: function (res) {

                        if (res == true) {

                            setTimeout(function () {

                                RemoveLike();

                                SetEnableLike(LikeElement);

                            }, 500);
                        }
                    },
                    error: function () {

                        SetEnableLike(LikeElement);

                        //Show Login Modal
                        ShowModal();

                    }
                });
            }
            else {

                //Add Like

                $.ajax({
                    url: "/Like/Add/" + postId,
                    method: "Post",
                    success: function (res) {

                        if (res == true) {

                            setTimeout(function () {

                                AddLike();

                                SetEnableLike(LikeElement);

                            }, 500);
                        }
                    },
                    error: function () {

                        SetEnableLike(LikeElement);

                        //Show Login Modal
                        ShowModal();

                    }
                });

            }

        }
    });

    $(".bookmark-div").click(function (e) {

        if (!($(this).hasClass("disable"))) {

            BookmarkElement = $(this);

            SetDisableBookmark(BookmarkElement);

            var hasBookmarked = $(".bookmark-div i").hasClass("fa-bookmark");
            var PostId = $(".post-id-input").val();

            if (hasBookmarked == true) {
                //Remove Bookmard

                $.ajax({
                    url: "/Bookmark/Remove/" + PostId,
                    method: "Delete",
                    success: function (res) {

                        if (res == true) {

                            setTimeout(function () {

                                RemoveBookmark();

                                SetEnableBookmark(BookmarkElement);

                            }, 500);
                        }
                    },
                    error: function () {

                        SetEnableBookmark(BookmarkElement);

                        //ForLogin
                        ShowModal();
                    }
                });

            }
            else {
                //Add Bookmark

                $.ajax({
                    url: "/Bookmark/Add/" + PostId,
                    method: "Post",
                    success: function (res) {

                        if (res == true) {

                            setTimeout(function () {

                                AddBookmark();

                                SetEnableBookmark(BookmarkElement);

                            }, 500);
                        }
                    },
                    error: function () {

                        SetEnableBookmark(BookmarkElement);

                        //ForLogin
                        ShowModal();
                    }
                });
            }
        }

    });

    $(".follows-div button").click(function () {

        if (!($(this).hasClass("disable"))) {

            btnElement = $(this);

            SetDisableFollow(btnElement);

            var userId = $(".author-id-input").val();

            if ($(this).hasClass("following-btn")) {
                //Remove Follow

                $.ajax({
                    url: "/Follow/RemoveUser/" + userId,
                    method: "Delete",
                    success: function (res) {

                        if (res == true) {

                            setTimeout(function(){

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
            else if ($(this).hasClass("follow-btn")) {
                //Add Follow

                $.ajax({
                    url: "/Follow/AddUser/" + userId,
                    method: "Post",
                    success: function (res) {

                        if (res == true) {

                            setTimeout(function () {

                                SetEnableFollow(btnElement);

                                ChangeSituationFollow();

                            },500);
                        }
                    },
                    error: function () {

                        SetEnableFollow(btnElement);

                        ShowModal();

                    }
                });
            }
            else if ($(this).hasClass("delete-btn")) {

                //Reset Btns For Use
                btnElement.css("cursor", "pointer");
                btnElement.removeClass("disable");

                //For Delete Post
                $("#modal-delete-post-btn").trigger("click");

            }

        }

    });

});


//Start...
//For Follow
function ChangeSituationFollow() {

    $(".follows-div .follow-btn").toggleClass("d-none");
    $(".follows-div .following-btn").toggleClass("d-none");
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


//Start...
//For Bookmark

function SetDisableBookmark(element) {

    $(element).addClass("disable");
    $(element).css("cursor", "not-allowed");
}
function SetEnableBookmark(element) {

    element.css("cursor", "pointer");
    element.removeClass("disable");
}

function AddBookmark() {

    $(".bookmark-div i").removeClass("fa-bookmark-o");

    $(".bookmark-div i").addClass("fa-bookmark");
}
function RemoveBookmark() {

    $(".bookmark-div i").removeClass("fa-bookmark");

    $(".bookmark-div i").addClass("fa-bookmark-o");
}
//...End


//Start...
//For Like
function AddLike() {

    $(".like-div i").removeClass("fa-heart-o");
    $(".like-div i").addClass("fa-heart");
    $(".like-div i").addClass("text-danger");

    var count = Number.parseInt($(".like-div .count-like-span").text());
    $(".like-div .count-like-span").text(++count);
}

function RemoveLike() {

    $(".like-div i").removeClass("fa-heart");
    $(".like-div i").removeClass("text-danger");
    $(".like-div i").addClass("fa-heart-o");

    var count = Number.parseInt($(".like-div .count-like-span").text());
    $(".like-div .count-like-span").text(--count);
}

function SetEnableLike(element) {

    element.css("cursor", "pointer");
    element.removeClass("disable");
}

function SetDisableLike(element) {

    $(element).addClass("disable");
    $(element).css("cursor", "not-allowed");
}


//...End


//Start...


function ShowModal() {

    $("#modal-account-btn").trigger("click");

}