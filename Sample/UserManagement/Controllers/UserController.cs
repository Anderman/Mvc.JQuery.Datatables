using System.Linq.Dynamic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using Mvc.JQuery.Datatables;
using UserManagement.Models;
using System.Linq.Expressions;
using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace UserManagement.Controllers
{

    public class role
    {
        public string Name { get; set; }
    }
    public class roles : List<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>
    {
    }

    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            DbContext = context;
            _userManager = userManager;

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
            //List<string> roles;

            //.Join(DbContext.Roles, ur => ur.Roles, r => r.Id, (ur, Roles) => new { ur, Roles })
            //var q = DbContext.Users.Include(u => u.Logins).Include(u => u.Roles).Join(DbContext.Roles, ur => ur.Roles.RoleId, r => r.Id, (ur, Roles) => new { ur, Roles });

            //var q = DbContext.Users.Include(u => u.Roles).Select(u=>u.Roles.Join(DbContext.Roles, ur => ur.RoleId, r => r.Id, (ur, Roles) => new { ur, Roles }));
            //var q = DbContext.Users.Include(u => u.Logins).Include(u => u.Roles).Select(u=>new {u.Email, RoleName=u.Roles.Join(DbContext.Roles, ur => ur.RoleId, r => r.Id, (ur, Roles) => new { Roles.Name }) });
            var q = DbContext.Users.Include(u => u.Logins).ToArray();
            //var qq = DbContext.Roles.ToArray();
            //var qq = DbContext.Users.Include(u => u.Logins).Include(u => u.Roles);
            //var arr = q.ToArray();
            //var z = new JsonResult(q.ToArray()); 
            //var qq = q.Join(DbContext.Roles, ur => ur.Roles.Select(a=>a.RoleId), r => r.Id, (ur, Roles) => new { ur, Roles });
            var qq = DbContext.UserRoles.Join(DbContext.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { r.Name, ur.UserId });
            var z = (from u in DbContext.Users//.Include(l => l.Logins).Include(r => r.Roles)
                     select new
                     {
                         u.Id,
                         u.Email,
                         u.EmailConfirmed,
                         u.UserName,
                         u.TwoFactorEnabled,
                         u.LockoutEnd,
                         Logins = u.Logins.Select(l => new { LoginProvider = l.LoginProvider }),
                         Roles = qq.Where(ur => ur.UserId == u.Id),
                         //Roles = u.Roles.Join(DbContext.Roles, ur => ur.RoleId, rr => rr.Id, (ur, role2) => role2).Select(rr => rr),
                     });
            var zz = z.ToArray();
            return new DataTables().GetJSonResult(
                z
                , dTRequest);
        }
        [HttpGet]
        [AllowAnonymous]
        public PartialViewResult Create()
        {
            return PartialView();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(ApplicationUser model)
        {
            if (ModelState.IsValid)
            {
                await _userManager.CreateAsync(model);

                return new EmptyResult();
            }
            return PartialView(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public PartialViewResult Edit(string Id)
        {
            //var dummy = DbContext.Users.Include(u => u.Logins).ToArray();
            var user = DbContext.Users.Include(u => u.Logins).Include(u => u.Claims).Include(u => u.Roles).FirstOrDefault(u => u.Id == Id);
            return PartialView(user);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Edit(ApplicationUser model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                user.AccessFailedCount = model.AccessFailedCount;
                user.Email = model.Email;
                user.EmailConfirmed = model.EmailConfirmed;
                user.LockoutEnabled = model.LockoutEnabled;
                user.LockoutEnd = model.LockoutEnd;
                user.PhoneNumber = model.PhoneNumber;
                user.PhoneNumberConfirmed = model.PhoneNumberConfirmed;
                user.TwoFactorEnabled = model.TwoFactorEnabled;
                user.UserName = model.UserName;
                await _userManager.UpdateAsync(user);

                DbContext.SaveChanges();
                return new EmptyResult();
            }
            return PartialView(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public PartialViewResult Delete(string Id)
        {
            var user = DbContext.Users.Single(u => u.Id == Id);
            return PartialView(user);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(ApplicationUser model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            await _userManager.DeleteAsync(user);
            return new EmptyResult();
        }
    }
}
