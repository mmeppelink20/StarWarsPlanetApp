using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCPresentation.Models;
using Microsoft.AspNet.Identity; // added
using Microsoft.AspNet.Identity.Owin; // added

namespace MVCPresentation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {


        // private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager userManager;

        // GET: Admin
        public ActionResult Index()
        {
            userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            return View(userManager.Users.OrderBy(n => n.FamilyName).ToList());
        }

        // GET: Admin/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // ApplicationUser applicationUser = db.ApplicationUsers.Find(id);
            userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationUser applicationUser = userManager.FindById(id);

            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            // get a list of roles the user has and put them into a viewbag asroles
            // along with a list of roles the user doesn't have as noRoles

            var usrMgr = new LogicLayer.UserManager();
            var allRoles = usrMgr.RetrieveAllUserRoles();


            var roles = userManager.GetRoles(id);
            var noRoles = allRoles.Except(roles);

            ViewBag.Roles = roles;
            ViewBag.NoRoles = noRoles;

            return View(applicationUser);
        }

        public ActionResult RemoveRole(string id, string role)
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.Users.First(u => u.Id == id);

            // code to prevent removing the last admin
            if (role == "Admin")
            {
                var adminUsers = userManager.Users.ToList()
                        .Where(u => userManager.IsInRole(u.Id, "Admin"))
                        .ToList().Count();
                if(adminUsers < 2)
                {
                    ViewBag.Error = "Can't remove last admins.";
                    return RedirectToAction("Details", "Admin", new { id = user.Id });
                }

            }
            userManager.RemoveFromRole(id, role);

            if (user.UserID != null)
            {
                try
                {
                    var usrMgr = new LogicLayer.UserManager();
                    usrMgr.DeleteUserRole(user.UserName, role);
                }
                catch(Exception)
                {
                    // nothing to do
                }
            }
            return RedirectToAction("Details", "Admin", new { id = user.Id });
        }

        public ActionResult AddRole(string id, string role)
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.Users.First(u => u.Id == id);

            userManager.AddToRole(id, role);

            if(user.UserID != null)
            {
                try
                {
                    var usrMgr = new LogicLayer.UserManager();
                    usrMgr.AddUserRole((int)user.UserID, user.UserName, role);
                }
                catch (Exception)
                {
                    // nothing to do
                }
            }
            return RedirectToAction("Details", "Admin", new { id = user.Id });
        }


    }
}
