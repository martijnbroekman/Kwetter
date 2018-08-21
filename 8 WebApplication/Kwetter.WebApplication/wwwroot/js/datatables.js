function initDataTable(templateId, dataTableId, tableContainerId, loadingBarId, getAllEntitiesUrl, callback) {
    var source = $('#' + templateId).html();
    var userTemplate = Handlebars.compile(source);

    var userTable = $('#' + dataTableId);
    userTable.DataTable({
        'pagingType': 'full_numbers',
        'drawCallback': callback
    });



    loadEntities(userTable, tableContainerId, loadingBarId, userTemplate, getAllEntitiesUrl);
}

function loadEntities(table, tableContainerId, loadingBarId, template, getAllEntitiesUrl) {
    $.ajax({
        type: "GET",
        url: getAllEntitiesUrl,
        success: function (result) {
            processUsersArray(result, table, tableContainerId, loadingBarId, template);
            $("#" + tableContainerId).show();
        }
    });
}

function processUsersArray(array, table, tableContainerId, loadingBarId, template) {
    var i = 0;
    var maxTimePerChunk = 100;
    function doChunk() {
        var startTime = Date.now();
        while (i < array.length && (Date.now() - startTime <= maxTimePerChunk)) {
            var row = template(array[i]);
            table.DataTable().row.add($(row));
            i++;
        }

        if (i < array.length) {
            var pc = Math.ceil((i / array.length) * 100);
            $("#" + loadingBarId + " .progress-bar").css('width', pc + '%').attr('aria-valuenow', pc);
            $("#" + loadingBarId + " .progress-bar").html(pc + '%');
            setTimeout(doChunk, 1);
        }

        if (i >= array.length) {
            table.DataTable().draw();
            $("#" + loadingBarId).hide();
        }
    }

    doChunk();
}

function addEntityToTable(table, template, data) {
    var userTemplate = Handlebars.compile(template);
    table.DataTable().row.add($(userTemplate(data))).draw();;
}

function updateEntityInTable(table, template, data, row) {
    var entityTemplate = Handlebars.compile(template);
    var userData = $(entityTemplate(data)).find('td').map(function (i, el) {
        return el.innerHTML;
    }).get();

    table.DataTable().row(row).data(userData).draw();
}

function deleteEntityFromTable(table, row) {
    table.DataTable().row(row).remove().draw();
}