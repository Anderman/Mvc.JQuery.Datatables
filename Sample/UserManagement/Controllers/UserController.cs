using System.Linq.Dynamic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
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

        public ApplicationDbContext DbContext { get; }
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
            //var a = DbContext.Users.Include(u => u.Logins);
            //var c = a.Where(u => u.Email == "a").ToArray();
            var d = DbContext.Users.Include(u => u.Logins).Where("Email.Contains(@0)", "a").ToArray();
            var e = DbContext.Users.Include(u => u.Logins).Where("Email.Contains(@0)", "a").OrderBy("Email").ToArray();
            var ij = DbContext.Users.Include(u => u.Logins).Where(u => u.Email.Contains("a")).OrderBy(u => u.Email).Skip(0).ToArray();
            var f = DbContext.Users.Include(u => u.Logins).Where("Email.Contains(@0)", "a").OrderBy("Email").Skip(0).ToArray();
            var g = DbContext.Users.Include(u => u.Logins).Where("Email.Contains(@0)", "a").OrderBy("Email").Skip(0).Take(5).ToArray();

            var h = new JsonResult(d.OrderBy("Email").Skip(0).Take(5).ToArray());
            var i = new DataTables().GetJSonResult(DbContext.Users.Include(u => u.Logins), dTRequest);
            return null;
            //return new DataTables().GetJSonResult(
            //    //DbContext.Users
            //    //.Include(u => u.Logins)//.Select(l=>l.LoginProvider))
            //    //                       //.Include(u => u.Claims)
            //    //                       //.Include(u => u.Roles)
            //    , dTRequest);
        }
        [HttpGet]
        [AllowAnonymous]
        public PartialViewResult Create()
        {
            return PartialView();
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Create(ApplicationUser model)
        {
            if (ModelState.IsValid)
            {
                return new EmptyResult();
            }
            return PartialView(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public PartialViewResult Edit(string Id)
        {
            var user = DbContext.Users.Include(u => u.Logins).Include(u => u.Claims).Include(u => u.Roles).FirstOrDefault(u => u.Id == Id);
            return PartialView(user);
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Edit(ApplicationUser model)
        {
            if (ModelState.IsValid)
            {
                return new EmptyResult();
            }
            return PartialView(model);
        }
    }
}
