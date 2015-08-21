
//This script is the same for each jquery-datatable.
//Define your own functions to extend the column rendering or custom error/succes message

var mvc = { 'JQuery': { 'Datatables': { ajax: {}, tableTools: {}, column: {} } } };

mvc.JQuery.Datatables.column.createMailToLink = function (data, type, full, meta) {
    return '<a href=mailto:' + data + '>' + data + '</a>';
}
mvc.JQuery.Datatables.column.formatDate = function (data, type, full, meta) {
    return data ? window.moment(data).format(meta.settings.aoColumns[meta.col].mvc6Par || 'YY-DD-mm hh:mm:ss') : null;
}
mvc.JQuery.Datatables.getLengthMenu = function () { return [[5, 50, 100, -1], [5, 50, 100, 'All']] };
mvc.JQuery.Datatables.getLanguage = function () { return {} };

// Bind every table with the class datatable
$(document).ready(function () {
    $('table.datatables').each(function () {
        $(this).MvcDatatable({
            leaveMessage: 'De wijzigingen zijn niet opgeslagen! toch door gaan?',
            error: function (jqXHR, textStatus, errorThrown) {
                var newWindow = window.open();
                newWindow.document.write(jqXHR.responseText);
            },
            succes: function (data, textStatus, jqXHR) { }
        });
    });
});



