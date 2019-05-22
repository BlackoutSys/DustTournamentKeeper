$(function () {
    $('#regUserText').keyup(function () {
        if ($('#regUserText').val() == "")
        {
            $('#regUserText').css('border-color', '');
            $('#regUserText').css('border-width', '2px');
            return;
        }

        $.ajax({
            url: '../../../Account/CheckPlayer?name=' + $(this).val(),
            method: 'POST'
        }).done(function (exists) {
            if (exists === true) {
                $('#regUserText').css('border-color', 'lime');
                $('#regUserText').css('border-width', '3px');
            } else {
                $('#regUserText').css('border-color', 'red');
                $('#regUserText').css('border-width', '3px');
            }
        }).fail(function (error) {
            console.log(error);
        });
    });

    $('#regUserLink').click(function () {
        var userName = $("#regUserText").val();
        $("#regUserText").href = $("#regUserText").href.replace("xxx", userName);
    });
});