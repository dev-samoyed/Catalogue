using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Catalogue.Models.Tables;
using System.Net;
using System.Data.Entity;

namespace Catalogue.Controllers.CRUD
{
    public class DepartmentController : Controller
    {
        CatalogueContext db = new CatalogueContext();

        // GET: Department
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            var administration = db.Departments.Include(e => e.Administration);
            return View(administration.ToList());
        }

        // GET: Department/Details/5
        [Authorize(Roles = "admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Department department = db.Departments.Find(id);
            if (department == null)
                return HttpNotFound();
            return View(department);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        // GET: Department/Create
        public ActionResult Create()
        {
            SelectList administrationList = new SelectList(db.Administrations, "AdministrationId", "AdministrationName");
            ViewBag.AdministrationList = administrationList;
            return View();
        }

        // POST: Department/Create
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Create(Department collection)
        {
            try
            {
                db.Departments.Add(collection);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Department/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Department department = db.Departments.Find(id);
            if (department == null)
                return HttpNotFound();
            return View(department);
        }

        // POST: Department/Edit/5
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id, Department collection)
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

        // GET: Department/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Department department = db.Departments.Find(id);
            if (department == null)
                return HttpNotFound();
            return View(department);
        }

        // POST: Department/Delete/5
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id, Department collection)
        {
            Department department = new Department();
            try
            {
                if (id == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                department = db.Departments.Find(id);
                if (department == null)
                    return HttpNotFound();
                db.Departments.Remove(department);
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