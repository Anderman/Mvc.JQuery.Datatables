//using System.Linq;
//using Microsoft.AspNet.Identity.EntityFramework;
//using Microsoft.Data.Entity;
//using Microsoft.Data.Entity.Infrastructure;
//using Microsoft.Data.Entity.Relational.Migrations;
//namespace UserManagement.Models
//{
//    public class ApplicationContext : IdentityDbContext<ApplicationUser>
//    {


//        protected override void OnModelCreating(ModelBuilder builder)
//        {
//            base.OnModelCreating(builder);
//            // Customize the ASP.NET Identity model and override the defaults if needed.
//            // For example, you can rename the ASP.NET Identity table names and more.
//            // Add your customizations after calling base.OnModelCreating(builder);
//        }
//        public bool AllMigrationsApplied()
//        {
//            return !((IAccessor<IMigrator>)Database.AsRelational()).Service.GetUnappliedMigrations().Any();
//        }

//    }
//}