$(function () {
    $('#blockSelect').change(function () {
        $.ajax({
            url: '../Faction/GetAvailableFactions?blockId=' + $(this).val(),
            method: 'GET'
        }).done(function (factions) {
            var factionSelect = $('#factionSelect');
            factionSelect.empty();

            factionSelect.attr('disabled', factions.length == 0);
            
            for (var i = 0; i < factions.length; i++) {
                var option = $('<option/>');
                option.attr('value', factions[i].id).text(factions[i].name);
                factionSelect.append(option);
            }
        }).fail(function (error) {
            console.log(error);
        });
    })
});

