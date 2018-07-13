using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Catalogue.Models.Tables;
using System.Net;
using System.Data.Entity;
using PagedList;
using System.Web.UI;

namespace Catalogue.Controllers.CRUD
{
    public class AdministrationController : Controller
    {
        CatalogueContext db = new CatalogueContext();

        // Ajax pagination PartialView Administration 
        [Authorize(Roles = "admin")]
        [OutputCache(Duration = 30, Location = OutputCacheLocation.Downstream)]
        public ActionResult AjaxPositionList(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return PartialView(db.Administrations.Include(e => e.Division).OrderBy(i => i.AdministrationName).ToPagedList(pageNumber, pageSize));
        }

        // GET: Administration
        [Authorize(Roles = "admin")]
        [OutputCache(Duration = 30, Location = OutputCacheLocation.Downstream)]
        public ActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(db.Administrations.Include(e => e.Division).OrderBy(i => i.AdministrationName).ToPagedList(pageNumber, pageSize));
        }

        // GET: Administration/Details/5
        [Authorize(Roles = "admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
                return HttpNotFound();
            Administration administration = db.Administrations.Include(e => e.Division).SingleOrDefault(d => d.AdministrationId == id);
            return View(administration);
        }

        // GET: Administration/Create
        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            SelectList divisionList = new SelectList(db.Divisions.OrderBy(d => d.DivisionName), "DivisionId", "DivisionName");
            ViewBag.AdministrationList = divisionList;
            return View();
        }

        // POST: Administration/Create
        [HttpPost]
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return HttpNotFound();
            Administration administration = db.Administrations.Find(id);
            if (administration == null)
                return HttpNotFound();

            SelectList divisionList = new SelectList(db.Divisions.OrderBy(d => d.DivisionName), "DivisionId", "DivisionName");
            ViewBag.AdministrationList = divisionList;
            return View(administration);
        }

        // POST: Administration/Edit/5
        [HttpPost]
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {         
            Administration administration = db.Administrations.Find(id);
            if (administration != null)
                return PartialView("Delete", administration);
            return View("Index");
        }

        // POST: Administration/Delete/5
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ActionName("Delete")]
        public ActionResult Delete(int? id, Administration collection)
        {
            Administration administration = new Administration();
            try
            {
                if (id == null)
                    return HttpNotFound();
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