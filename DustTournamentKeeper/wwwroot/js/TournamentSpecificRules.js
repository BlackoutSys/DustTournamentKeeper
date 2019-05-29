$(function () {
    $('.sr-button').click(function () {
        $("#specificRulesModalLabel").text($(this).data("tournament"));
        $("#specificRulesModalContent").text($(this).data("sr"));
    })
});

