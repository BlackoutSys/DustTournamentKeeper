function getAvailableFactions () {
    $.ajax({
        url: '../../../Faction/GetAvailableFactions?blockId=' + $(this).val(),
        method: 'POST'
    }).done(function (factions) {
        var factionSelect = $('#factionSelect');
        factionSelect.empty();

        factionSelect.attr('disabled', factions.length == 0);

        var optionNull = $('<option/>');
        optionNull.attr('value', null).text("---");
        factionSelect.append(optionNull);

        for (var i = 0; i < factions.length; i++) {
            var option = $('<option/>');
            option.attr('value', factions[i].id).text(factions[i].name);
            factionSelect.append(option);
        }
    }).fail(function (error) {
        console.log(error);
    });
}

function getAvailableBlocks() {
    $.ajax({
        url: '../../../Block/GetAvailableBlocks?gameId=' + $(this).val(),
        method: 'POST'
    }).done(function (blocks) {
        var blockSelect = $('#blockSelect');
        blockSelect.empty();

        blockSelect.attr('disabled', blocks.length == 0);

        for (var i = 0; i < blocks.length; i++) {
            var option = $('<option/>');
            option.attr('value', blocks[i].id).text(blocks[i].name);
            blockSelect.append(option);
        }
    }).fail(function (error) {
        console.log(error);
    });
}

