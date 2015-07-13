# Mvc.JQuery.Datatables (MVC6 with taghelpers)

##1. Install Package

PM> Install-Package Mvc6.JQuery.Datatables -Pre

##2. Create the AJAX response for JQuery.datatable 
```
        public JsonResult GetAll([FromBody]DataTablesRequest dTRequest)
        {
            var data = new DataTables().GetRepsonse(Ctx.Users, dTRequest);
            return new JsonResult(data);  
        }
```


##3. Define MVC6 Table definition.
Taghelper intellisense available in taghelper library
```
<table id="example1" class="datatables display"  stateSave="true" cellspacing="0" width="100%">
    <thead>
        <tr>
            <th asp-for="Email" column-renderFunction="createMailToLink(data)"></th>
            <th asp-for="EmailConfirmed"  ></th>
            <th asp-for="LockoutEnd" column-renderFunction="formatDate(data,'YYYY-DD-mm hh:mm:ss')"></th>
            <th asp-for="TwoFactorEnabled"></th>
            <th asp-for="UserName"></th>
        </tr>
    </thead>
</table>
```

##4 Create only one javascript for all your data tables tables
```
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
            
        })
    });
});
```
