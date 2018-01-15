using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Catalogue.Models.Tables;
using System.Net;
using System.Data.Entity;
using PagedList;

namespace Catalogue.Controllers.CRUD
{
    public class DepartmentController : Controller
    {
        CatalogueContext db = new CatalogueContext();

        public ActionResult AjaxPositionList(int? page)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return PartialView(db.Departments.Include(e => e.Administration).OrderBy(i => i.DepartmentName).ToPagedList(pageNumber, pageSize));
        }

        // GET: Department
        public ActionResult Index(int? page)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(db.Departments.Include(e => e.Administration).OrderBy(i => i.DepartmentName).ToPagedList(pageNumber, pageSize));
        }

        // GET: Department/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Department administration = db.Departments.Include(e => e.Administration).SingleOrDefault(d => d.DepartmentId == id);
            return View(administration);
        }

        [HttpGet]
        // GET: Department/Create
        public ActionResult Create()
        {
            SelectList administrationList = new SelectList(db.Administrations, "AdministrationId", "AdministrationName");
            ViewBag.AdministrationList = administrationList;
            return View();
        }

        // POST: Department/Create
        [HttpPost]
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
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Department department = db.Departments.Find(id);
            if (department == null)
                return HttpNotFound();

            SelectList administrationList = new SelectList(db.Administrations, "AdministrationId", "AdministrationName");
            ViewBag.AdministrationList = administrationList;
            return View(department);
        }

        // POST: Department/Edit/5
        [HttpPost]
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
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Department administration = db.Departments.Include(e => e.Administration).SingleOrDefault(d => d.DepartmentId == id);
            return View(administration);
        }

        // POST: Department/Delete/5
        [HttpPost]
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