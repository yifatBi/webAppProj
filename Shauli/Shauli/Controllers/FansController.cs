using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Shauli.Models;

namespace Shauli.Controllers
{
    public class FansController : Controller
    {
        private FansDBContext db = new FansDBContext();

        // GET: Fans
        public ActionResult Index(string firstName,string lastName,string birthdate)
        {
            DateTime startDate, endDate;
            var fans = from f in db.Fans select f;
            if (!string.IsNullOrEmpty(firstName))
            {
                fans = fans.Where(f => f.FirstName.Contains(firstName));
                ViewBag.FirstNameFilter = firstName;
            }
            //Filter accortding title
            if (!string.IsNullOrEmpty(lastName))
            {
                fans = fans.Where(f => f.LastName.Contains(lastName));
                ViewBag.LastNameFilter = lastName;
            }
            //Filter according date (take complete date)
            if (!string.IsNullOrEmpty(birthdate) && DateTime.TryParse(birthdate, out startDate))
            {
                endDate = startDate;
                endDate = endDate.AddDays(1);
                fans = fans.Where(f => f.BirthDate >= startDate && f.BirthDate < endDate);
                ViewBag.DateFilter = birthdate;
            }
            return View(fans.ToList());
        }

        // GET: Fans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fans fans = db.Fans.Find(id);
            if (fans == null)
            {
                return HttpNotFound();
            }
            return View(fans);
        }

        // GET: Fans/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Fans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,YearsOfMemebership,sex,LastName,BirthDate,Email")] Fans fans)
        {
            if (ModelState.IsValid)
            {
                db.Fans.Add(fans);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fans);
        }

        // GET: Fans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fans fans = db.Fans.Find(id);
            if (fans == null)
            {
                return HttpNotFound();
            }
            return View(fans);
        }

        // POST: Fans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,YearsOfMemebership,sex,LastName,BirthDate,Email")] Fans fans)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fans).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fans);
        }

        // GET: Fans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fans fans = db.Fans.Find(id);
            if (fans == null)
            {
                return HttpNotFound();
            }
            return View(fans);
        }

        // POST: Fans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Fans fans = db.Fans.Find(id);
            db.Fans.Remove(fans);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
