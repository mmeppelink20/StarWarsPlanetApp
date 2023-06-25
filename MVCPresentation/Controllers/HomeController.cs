using DataObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCPresentation.Controllers
{
    public class HomeController : Controller
    {
        private List<PlanetVM> planets;
        private PlanetManager planetManager = new PlanetManager();

        // GET: Homepage planets
        public ActionResult Index()
        {
            try
            {
                planets = planetManager.RetrievePlanetVMsMVCByPlanetID("");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message + ex.InnerException;
                return View("Error");
            }
            return View(planets);
        }

        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}