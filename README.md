# Mvc.JQuery.Datatables (MVC6 with taghelpers)

##1. Install Package

PM> Install-Package Mvc6.JQuery.Datatables -Pre

##2. Create a controller action in your controller 
```
        public JsonResult GetAll([FromBody]DataTablesRequest dTRequest)=>
            new DataTables().GetJSonResult(Ctx.Users, dTRequest);
```


##3. create a view.

```
<table id="example1" class="datatables display"
       asp-datatables-language='getLanguage'
       asp-datatables-lengthmenu='getLengthMenu'
       asp-datatables-url="/Account/GetAll"
       asp-datatables-savestate="true" cellspacing="0" width="100%">
    <thead>
        <tr>
            <th asp-datatables-data="Email" asp-datatables-render="createMailToLink"></th>
            <th asp-datatables-data="EmailConfirmed"></th>
            <th asp-datatables-data="LockoutEnd" asp-datatables-render="formatDate" asp-datatables-render-arg="YYYY-DD-mm hh:mm:ss"></th>
            <th asp-datatables-data="TwoFactorEnabled" asp-datatables-orderable="false"></th>
            <th asp-datatables-data="UserName" asp-datatables-searchable="false"></th>
        </tr>
    </thead>
</table>
```

##4 Add the following javascript to yout project
```
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
```
##5. Run your project
![](http://snag.gy/aETVt.jpg)


## Why this package.
There are more MVC packges but they have a different approach.
problem with other packages.
* layout is defined in c#, view and javascript
* Field must be specified in view, controller, javascript

This package will make the use of jquery.datatable easier by.
* Define your fields only in your view (th tag)
* Use only one javascript for all your tables
* Make use of taghelpers for your fields and other options and use intellisense to support your design

