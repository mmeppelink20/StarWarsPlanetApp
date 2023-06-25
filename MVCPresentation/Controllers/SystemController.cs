using DataObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCPresentation.Controllers
{
    public class SystemController : Controller
    {
        private List<PlanetVM> planets;
        private PlanetManager planetManager = new PlanetManager();
        [Authorize(Roles = "Admin")]
        // GET: System
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

        // GET: System/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: System/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: System/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: System/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: System/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: System/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: System/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
