using DataObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCPresentation.Controllers
{
    public class PlanetController : Controller
    {
        private List<PlanetVM> planets;
        private PlanetManager planetManager = new PlanetManager();

        [Authorize (Roles = "Admin")]
        // GET: Planet
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

        [Authorize(Roles = "Admin")]
        // GET: Planet/Details/5
        public ActionResult Details(string id)
        {
            Planet planet = new Planet();
            try
            {
                planet = planetManager.RetrievePlanetVMMVCByPlanetID(id);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "There was an error retrieving this page " + ex.Message;
                return View("Error");
            }
            return View(planet);
        }

        [Authorize(Roles = "Admin")]
        // GET: Planet/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.systems = planetManager.RetrieveAllPlanetarySystem();
            }
            catch (Exception ex)
            {
                ViewBag.Message = "there was an error retrieving this page";
                return View("Error");
            }
            return View();

        }

        // POST: Planet/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Planet planet)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if(planetManager.RetrievePlanetVMMVCByPlanetID(planet.PlanetID) == null)
                    {
                        Session["oldPlanet"] = planet.PlanetID;
                        planetManager.AddPlanetMVCRecord(planet);
                        return RedirectToAction("Index");
                    }
                    ViewBag.Message = planet.PlanetID + " already exists";
                    return View("Error");
                }
                catch (Exception)
                {
                    
                }
            }
            return View();
        }

        // GET: Planet/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            PlanetVM planet = new PlanetVM();
            try
            {
                planet = planetManager.RetrievePlanetVMMVCByPlanetID(id);
                Session["oldPlanet"] = planet;
                ViewBag.systems = planetManager.RetrieveAllPlanetarySystem();
                if(planet == null)
                {
                    throw new Exception("Planet not found");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "you have to specify a planet to edit ";
                return View("Error");
            }
            return View(planet);
        }

        // POST: Planet/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(PlanetVM newPlanet)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    PlanetVM oldPlanet = (PlanetVM)Session["oldPlanet"];

                    planetManager.EditPlanetMVC(oldPlanet, newPlanet);

                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                    return View("Error");
                }
            }
            return View();
        }

        // GET: Planet/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            Planet planet = new Planet();
            try
            {
                planet = planetManager.RetrievePlanetVMByPlanetID(id);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "You have to specify a planet to delete";
                return View("Error");
            }
            return View(planet);
        }

        // POST: Planet/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                planetManager.DeletePlanetMVCByPlanetID(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
