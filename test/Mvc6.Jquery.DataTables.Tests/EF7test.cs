using System.Linq;
using System.Linq.Dynamic;
using Microsoft.Data.Entity;
using Xunit;
using UserManagement.Models;
using UserManagement;
namespace Mvc.Jquery.DataTables.Test
{
    public class Ef7Test
    {
        [Xunit.Fact]
        public void Ef7Bug()
        {
            ApplicationDbContextTest DbContext = new ApplicationDbContextTest();
            //var e = DbContext.Users.Include(u => u.Logins).Where("Email.Contains(@0)", "a").OrderBy("Email").ToArray();
            //Assert.Equal(6, e.Count());
            var ij = DbContext.Users.Include(u => u.Logins).Where(u => u.Email.Contains("a")).OrderBy(u => u.Email).Skip(0).ToArray();

            Assert.Equal(1, 0);
        }
    }
    public class ApplicationDbContextTest : ApplicationDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=UserManagement;Trusted_Connection=True;MultipleActiveResultSets=true");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}

