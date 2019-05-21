$(function () {
    $('.sr-button').click(function () {
        $("#specificRulesModalLabel").text($(this).data("tournament"));
        $("#specificRulesModalContent").text($(this).data("sr"));
        console.log("kakao");
        console.log($(this).data("tournament"));
        console.log($(this).data("sr"));
        console.log($(this));
    })
});

