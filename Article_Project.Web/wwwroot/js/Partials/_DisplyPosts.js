$(document).ready(function () {

    $(".like-div").click(function () {

        if (!($(this).hasClass("disable"))) {


            var LikeElement = $(this);

            SetDisableLike(LikeElement);

            var PostId = LikeElement.attr("PostId");
            var hasLiked = $(".like-div[PostId=" + PostId + "] i").hasClass("fa-heart");

            if (hasLiked) {
                //Remove Like

                $.ajax({
                    url: "/Like/Remove/" + PostId,
                    method: "Delete",
                    success: function (res) {

                        if (res == true) {

                            setTimeout(function () {

                                RemoveLikeToPostItem(PostId);

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
                    url: "/Like/Add/" + PostId,
                    method: "Post",
                    success: function (res) {

                        if (res == true) {

                            setTimeout(function () {

                                AddLikeToPostItem(PostId);

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

    $(".bookmark-div").click(function () {

        if (!($(this).hasClass("disable"))) {

            var BookmarkElement = $(this);

            SetDisableBookmark(BookmarkElement);


            var PostId = BookmarkElement.attr("PostId");
            var hasBookmarked = $(".bookmark-div[PostId=" + PostId + "] i").hasClass("fa-bookmark");

            if (hasBookmarked == true) {
                //Remove Bookmark

                $.ajax({
                    url: "/Bookmark/Remove/" + PostId,
                    method: "Delete",
                    success: function (res) {

                        if (res == true) {

                            setTimeout(function () {

                                RemoveBookmarkToPostItem(PostId);

                                SetEnableBookmark(BookmarkElement);

                            }, 500);
                        }
                    },
                    error: function () {

                        SetEnableBookmark(BookmarkElement);

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

                                AddBookmarkToPostItem(PostId);

                                SetEnableBookmark(BookmarkElement);

                            }, 500);

                        }
                    },
                    error: function () {

                        SetEnableBookmark(BookmarkElement);

                        ShowModal();
                    }
                });

            }
        }
    });
});


//Start...
//Bookmark

function SetEnableBookmark(element) {

    element.removeClass("disable");
    element.css("cursor", "pointer");
}
function SetDisableBookmark(element) {

    element.addClass("disable");
    element.css("cursor", "not-allowed");
}

function AddBookmarkToPostItem(PostId) {

    $(".bookmark-div[PostId=" + PostId + "] i").removeClass("fa-bookmark-o");

    $(".bookmark-div[PostId=" + PostId + "] i").addClass("fa-bookmark");
}

function RemoveBookmarkToPostItem(PostId) {

    $(".bookmark-div[PostId=" + PostId + "] i").removeClass("fa-bookmark");

    $(".bookmark-div[PostId=" + PostId + "] i").addClass("fa-bookmark-o");
}

//...End


//Start...
//Like
function AddLikeToPostItem(PostId) {

    $(".like-div[PostId=" + PostId + "] i").removeClass("fa-heart-o");

    $(".like-div[PostId=" + PostId + "] i").addClass("fa-heart");
    $(".like-div[PostId=" + PostId + "] i").addClass("text-danger");

    var count = Number.parseInt($(".like-div[PostId = " + PostId + "] .count-like-span").text());

    count++;

    $(".like-div[PostId =" + PostId + "] .count-like-span").text(count);

}
function RemoveLikeToPostItem(PostId) {

    $(".like-div[PostId=" + PostId + "] i").removeClass("fa-heart");
    $(".like-div[PostId=" + PostId + "] i").removeClass("text-danger");

    $(".like-div[PostId=" + PostId + "] i").addClass("fa-heart-o");

    var count = Number.parseInt($(".like-div[PostId = " + PostId + "] .count-like-span").text());

    count--;

    $(".like-div[PostId =" + PostId + "] .count-like-span").text(count);

}

function SetDisableLike(element) {

    element.addClass("disable");
    element.css("cursor", "not-allowed");
}

function SetEnableLike(element) {

    element.css("cursor", "pointer");
    element.removeClass("disable");
}
//..End

function ShowModal() {

    $("#modal-account-btn").trigger("click");

}