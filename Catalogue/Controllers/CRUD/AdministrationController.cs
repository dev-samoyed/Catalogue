using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Catalogue.Models.Tables;
using System.Net;

namespace Catalogue.Controllers.CRUD
{
    public class AdministrationController : Controller
    {
        CatalogueContext db = new CatalogueContext();

        // GET: Administration
        public ActionResult Index()
        {
            return View(db.Administrations.ToList());
        }

        // GET: Administration/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Administration administration = db.Administrations.Find(id);
            if (administration == null)
                return HttpNotFound();
            return View(administration);
        }

        // GET: Administration/Create
        [HttpGet]
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
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Administration administration = db.Administrations.Find(id);
            if (administration == null)
                return HttpNotFound();
            return View(administration);
        }

        // POST: Administration/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Administration collection)
        {
            try
            {
                db.Entry(collection).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Administration/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Administration administration = db.Administrations.Find(id);
            if (administration == null)
                return HttpNotFound();
            return View(administration);
        }

        // POST: Administration/Delete/5
        [HttpPost]
        public ActionResult Delete(int? id, Administration collection)
        {
            Administration administration = new Administration();
            try
            {
                if (id == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                administration = db.Administrations.Find(id);
                if (administration == null)
                    return HttpNotFound();
                db.Administrations.Remove(administration);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}