//This script is the same for each jquery-datatable.
//Define your own functions to extend the column rendering or custom error/succes message

var mvc = { "JQuery": { "Datatables": {} } };

mvc.JQuery.Datatables.createMailToLink = function (data) {
    return '<a href=mailto:' + data + '>' + data + '</a>';
}
mvc.JQuery.Datatables.formatDate = function (data, type, full, meta) {
    return moment(data).format(meta.settings.aoColumns[meta.col].mvc6Par || "YY-DD-mm hh:mm:ss");
}
mvc.JQuery.Datatables.noRender = function (data) { return data; }
mvc.JQuery.Datatables.returnNull = function (data) { return null; }
mvc.JQuery.Datatables.getLengthMenu = function () { return [[25, 50, 100, -1], [25, 50, 100, "All"]] };
mvc.JQuery.Datatables.getLanguage = function () { return {} };
mvc.JQuery.Datatables.success = function (data, textStatus, jqXHR) { };
mvc.JQuery.Datatables.error = function (jqXHR, textStatus, errorThrown) { alert(errorThrown) };

// Bind every table with the class datatable
// Read all information that can be used for data request from the table and th tags
$(document).ready(function () {
    $('table.datatables').each(function () {
        var columns = [];
        $($(this).find("thead > tr > th[data-data]")).each(function () {
            var column = {};
            column.data = $(this).attr("data-data");
            column.render = (mvc.JQuery.Datatables[$(this).attr("data-render") || "noRender"]);
            column.mvc6Par = $(this).attr("data-render-arg");
            column.orderable = $(this).attr("data-orderable") != 'False';
            column.searchable = $(this).attr("data-searchable") != 'False';
            columns.push(column);
        });
        var root = {
            processing: true,
            serverSide: true,
            fnServerData: function (sSource, aoData, fnCallback) {
                for (var DataTableRequest = {}, i = 0 ; i < aoData.length; i++) DataTableRequest[aoData[i].name] = aoData[i].value;
                $.ajax({
                    url: $(this).attr("data-url"),
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(DataTableRequest),
                    success: function (data, textStatus, jqXHR) {
                        (mvc.JQuery.Datatables[$(this).attr("data-succes") || "success"])(data, textStatus, jqXHR);
                        fnCallback(data, textStatus, jqXHR);
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        (mvc.JQuery.Datatables[$(this).attr("data-error") || "error"])(jqXHR, textStatus, errorThrown);
                    },
                })
            },
            columns: columns,
            language: (mvc.JQuery.Datatables[$(this).attr("data-language") || "returnNull"])(),
            lengthMenu: (mvc.JQuery.Datatables[$(this).attr("data-lengthMenu") || "returnNull"])(),
        }
        $(this).dataTable(root)
    });
});