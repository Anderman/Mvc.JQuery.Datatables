using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Framework.DependencyInjection;


namespace UserManagement.Models
{
    public static class ApplicationExtensions
    {
        public static void EnsureMigrationsApplied(this IApplicationBuilder app)
        {
            ApplicationDbContext context = app.ApplicationServices.GetService<ApplicationDbContext>();
            context.Database.Migrate();
        }

        public async static Task EnsureSampleData(this IApplicationBuilder app)
        {
            var context = app.ApplicationServices.GetService<ApplicationDbContext>();
            var userManager = app.ApplicationServices.GetService<UserManager<ApplicationUser>>();
            var roleManager = app.ApplicationServices.GetService<RoleManager<IdentityRole>>();

            if (context.AllMigrationsApplied())
            {
                foreach (var role in roleManager.Roles)
                {
                    Console.WriteLine(role.Name);
                }
                foreach (string role in Enum.GetNames(typeof(Roles)))
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }

                }

                string password = "P@ssword!";
                await CreateUserIfNotExist(userManager, "thom@medella.nl", password, Roles.Admin.ToString(), "Google", "110018662340682049067");
                await CreateUserIfNotExist(userManager, "Bobbie@kuifje.be", password, Roles.Admin.ToString());
                await CreateUserIfNotExist(userManager, "KapiteinArchibaldHaddock@kuifje.be", password, Roles.Admin.ToString());
                await CreateUserIfNotExist(userManager, "TrifoniusZonnebloem@kuifje.be", password, Roles.Admin.ToString());
                await CreateUserIfNotExist(userManager, "JansenenJanssen@kuifje.be", password, Roles.Admin.ToString());
                await CreateUserIfNotExist(userManager, "BiancaCastofiore@kuifje.be", password, Roles.Admin.ToString());
                await CreateUserIfNotExist(userManager, "Nestor@kuifje.be", password, Roles.Admin.ToString());
                await CreateUserIfNotExist(userManager, "SerafijnLampion@kuifje.be", password, Roles.Admin.ToString());
                await CreateUserIfNotExist(userManager, "Rastapopoulos@kuifje.be", password, Roles.Admin.ToString());
                await CreateUserIfNotExist(userManager, "Sponsz@kuifje.be", password, Roles.Admin.ToString());
            }
            else
                throw new System.Exception("Not all migration are applied");
        }

        private static async System.Threading.Tasks.Task CreateUserIfNotExist(UserManager<ApplicationUser> userManager, string email, string password, string role, string loginProvider = null, string providerKey = null)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser { UserName = email, Email = email };
                var result = await userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    throw new ApplicationException(string.Join("\n", result.Errors.Select(a => a.Description).ToArray()));
                }
                await userManager.AddToRoleAsync(user, role);
                if (loginProvider != null && providerKey != null)
                {
                    await userManager.AddLoginAsync(user, new UserLoginInfo(loginProvider, providerKey, ""));
                }
            }
        }
    }
}