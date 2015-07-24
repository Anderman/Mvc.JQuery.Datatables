using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Mvc.JQuery.Datatables;
using UserManagement.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace UserManagement.Controllers
{
    public class UserController : Controller
    {
        public UserController(ApplicationDbContext context)
        {
            DbContext = context;
        }

        public ApplicationDbContext DbContext { get; private set; }
        [HttpGet]
        [AllowAnonymous]
        public ViewResult Index()
        {
            //var x = DbContext.Users.Include(u => u.Logins).Select(l => l.Logins).ToArray();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult Index([FromBody]DataTablesRequest dTRequest)
        {
            return new Mvc.JQuery.Datatables.DataTables().GetJSonResult(
                DbContext.Users
                //.Include(u => u.Logins)//.Select(l=>l.LoginProvider))
                //.Include(u => u.Claims)
                //.Include(u => u.Roles)
                , dTRequest);
        }
        [HttpGet]
        [AllowAnonymous]
        public ViewResult Create()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public PartialViewResult Edit(string Id)
        {

            var User = DbContext.Users.Include(u => u.Logins).Include(u => u.Claims).Include(u => u.Roles).Where(u => u.Id == Id).FirstOrDefault();
            return PartialView(User);
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Edit(ApplicationUser model)
        {
            //ModelState.AddModelError("oeps", "oeps");

            if (ModelState.IsValid)
            {
                return new EmptyResult();
            }

            return PartialView(model);
        }
    }
}
