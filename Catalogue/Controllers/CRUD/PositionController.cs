using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Catalogue.Models.Tables;
using System.Net;
using PagedList;

namespace Catalogue.Controllers.CRUD
{
    public class PositionController : Controller
    {
        CatalogueContext db = new CatalogueContext();

        // Ajax pagination PartialView Position 
        [Authorize(Roles = "admin")]
        public ActionResult AjaxPositionList(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return PartialView(db.Positions.OrderBy(i => i.PositionName).ToPagedList(pageNumber, pageSize));
        }

        // GET: Position
        [Authorize(Roles = "admin")]
        public ActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(db.Positions.OrderBy(i => i.PositionName).ToPagedList(pageNumber, pageSize));
        }

        // GET: Position/Details/5
        [Authorize(Roles = "admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
                 return HttpNotFound();
            Position position = db.Positions.Find(id);
            if (position == null)
                return HttpNotFound();
            return View(position);
        }

        // GET: Position/Create
        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Position/Create
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Create(Position collection)
        {
            try
            {
                // TODO: Add insert logic here
                db.Positions.Add(collection);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Position/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                 return HttpNotFound();
            Position position = db.Positions.Find(id);
            if (position == null)
                return HttpNotFound();
            return View(position);
        }

        // POST: Position/Edit/5
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id, Position collection)
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

        // GET: Position/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return HttpNotFound();
            Position position = db.Positions.Find(id);
            if (position != null)
                return PartialView("Delete", position);
            return View("Index");
        }

        // POST: Position/Delete/5
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ActionName("Delete")]
        public ActionResult Delete(int? id, Position collection)
        {
            Position position = new Position();
            try
            {
                if (id == null)
                     return HttpNotFound();
                position = db.Positions.Find(id);
                if (position == null)
                    return HttpNotFound();
                db.Positions.Remove(position);
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