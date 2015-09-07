# Mvc.JQuery.Datatables (MVC6 with taghelpers)

##1. Install Package

PM> Install-Package Mvc6.JQuery.Datatables -Pre

__add the taghelper support to _ViewImports.cshtml and/or _GlobalImport.cshtml__

@addTagHelper "*, Mvc6.JQuery.Datatables"

##2. Create a controller action in your controller 
```
        public JsonResult GetAll([FromBody]DataTablesRequest dTRequest)=>
            new DataTables().GetJSonResult(Ctx.Users, dTRequest);
```

##3. create a view.
![](http://snag.gy/10cQg.jpg)


##4 Add the following javascript to your project
```
http://mvcjquerydatatables.azurewebsites.net/js/mvc.jquery.datatables.js
```
##5. Run your project
![](http://snag.gy/aETVt.jpg)

##6. See Demo
This [Demo Usermanagement](http://mvcjquerydatatables.azurewebsites.net) is an example of.
* jquery Datatables with asp.net 5
* JQuery datatable tools
* JQuery datatable edit (light)
* Bootstrap Material design

the demo uses a CI integration with appveyor and azure


## Why this package.
There are more MVC packges but they have a different approach.
problem with other packages.
* layout is defined in c#, view and javascript
* Field must be specified in view, controller, javascript

This package will make the use of jquery.datatable easier by.
* Define your fields only in your view (th tag)
* Use only one javascript for all your tables
* Make use of taghelpers for your fields and other options and use intellisense to support your design

