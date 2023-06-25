namespace MVCPresentation.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using MVCPresentation.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<MVCPresentation.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "MVCPresentation.Models.ApplicationDbContext";
        }

        protected override void Seed(MVCPresentation.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            const string admin = "admin@company.com";
            const string adminPassword = "P@ssw0rd";

            // new code for Step 6
            // Note: There is purposely no using statement for LogicLayer. This is to force
            // programmers to use the fully qualified name for our internal UserManager, to
            // keep it clear that this is not the Identity system's UserManager class.

            LogicLayer.UserManager userMgr = new LogicLayer.UserManager();
            var roles = userMgr.RetrieveAllUserRoles();

            foreach(var role in roles)
            {
                context.Roles.AddOrUpdate(r => r.Name, new IdentityRole() { Name = role });
            }
            if (!roles.Contains("Admin"))
            {
                context.Roles.AddOrUpdate(r => r.Name, new IdentityRole() { Name = "Admin" });
            }

            if (!context.Users.Any(u => u.UserName == admin))
            {
                var user = new ApplicationUser()
                {
                    UserName = admin,
                    Email = admin,
                    GivenName = "Admin",
                    FamilyName = "Company"
                };

                IdentityResult result = userManager.Create(user, adminPassword);
                context.SaveChanges(); // updates the changes

                // this code will add the admin role to admin
                if (result.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Admin");
                    context.SaveChanges();
                }
            }

        }
    }
}
