$(document).ready(function () {
    var getColumns = function (datatable) {
        var columns = [];
        $($(datatable).find("thead > tr > th[column-data]")).each(function () {
            var renderFunction = $(this).attr("column-renderFunction");
            if (renderFunction) {
                var sRender = new Function("data", "type", "full", "meta", "return " + renderFunction);
                columns.push({ data: $(this).attr("column-data"), render: sRender });
            }
            else
                columns.push({ data: $(this).attr("column-data") });
        });
        return columns;
    }
    var getLanguage = function (datatable) {
        var v = $(datatable).attr("datatables-lang")
        var language = JSON.parse(v);
        return language
    }
    var getLengthMenu = function (datatable) {
        var lengthMenu = [];
        var v = $(datatable).attr("datatables-lengthMenu")
        lengthMenu = JSON.parse(v);
        return lengthMenu;
    }
    $('table.datatables').each(function () {
        $(this).dataTable({
            "processing": true,
            "serverSide": true,
            "ajax": {
                "url": "/Account/GetAll",
                "type": "POST",
                "contentType": "application/json; charset=utf-8",
                "dataType": "json",
                "data": function (d) {
                    return JSON.stringify(d);
                }
            },
            "columns": getColumns(this),
            "language": getLanguage(this),
            "lengthMenu": getLengthMenu(this)
            ],
        })
    });
});
