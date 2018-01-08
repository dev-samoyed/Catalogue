using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Catalogue.Models.Tables;

namespace Catalogue.Controllers.CRUD
{
    public class AdministrationController : Controller
    {
        CatalogueContext db = new CatalogueContext();

        // GET: Administration
        public ActionResult Index()
        {
            return View(db.Administrations);
        }

        // GET: Administration/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Administration/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Administration/Create
        [HttpPost]
        public ActionResult Create(Administration collection)
        {
            try
            {
                // TODO: Add insert logic here
                db.Administrations.Add(collection);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Administration/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Administration/Edit/5
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

        // GET: Administration/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Administration/Delete/5
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
