# Mvc.JQuery.Datatables (MVC6 with taghelpers)

[![Build status](https://ci.appveyor.com/api/projects/status/mb4lk10dev4seg71?svg=true)](https://ci.appveyor.com/project/Anderman/mvc-jquery-datatables) ![License](https://img.shields.io/badge/license-MIT-blue.svg) [![nuget package](https://img.shields.io/badge/nuget-beta7-yellow.svg)](https://www.nuget.org/packages/Mvc6.JQuery.Datatables)


##1. Install Package

To install, run the following command in the Package Manager Console.

```csharp
PM> Install-Package Mvc6.JQuery.Datatables -Pre
```
##2. Configuration
Add the following to the `_ViewImports.cshtml` file

```csharp
@addTagHelper "*, Mvc6.JQuery.Datatables"
```

##3. Create a controller action in your controller 
```
        public JsonResult GetAll([FromBody]DataTablesRequest dTRequest)=>
            new DataTables().GetJSonResult(Ctx.Users, dTRequest);
```

##4. create a view.
![](http://snag.gy/10cQg.jpg)


##5 Add the following javascript to your project
```
http://mvcjquerydatatables.azurewebsites.net/js/mvc.jquery.datatables.js
```
##6. Run your project
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

