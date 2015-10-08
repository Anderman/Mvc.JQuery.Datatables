using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.Relational;
using Microsoft.Framework.OptionsModel;
namespace UserManagement.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public override bool TwoFactorEnabled { get; set; } = false;
        public override bool EmailConfirmed { get; set; } = true;
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public override DateTimeOffset? LockoutEnd { get { return base.LockoutEnd; } set { base.LockoutEnd = value; } }
        [Required]
        [ScaffoldColumn(false)]
        [Display(Description = "Email adress is used for login")]
        public override string Email { get { return base.Email; } set { base.Email = value; } }
    }
    public class ApplicationUserRoles : IdentityUserRole<string>
    {
        public virtual ApplicationUser Role { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
        public bool AllMigrationsApplied()
        {
            return true; //!((IMigrator)((IAccessor<IServiceProvider>)Database).Service.GetService(typeof(IMigrator))).;
        }
    }
    public enum Roles
    {
        Admin,
        Patient,
        Practitioner,
        Secretary,
    }
}
