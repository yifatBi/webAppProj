using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Shauli.Models;
using System.Security.Cryptography;
using System.Text;

namespace Shauli.Controllers
{
    public class AccountsController : Controller
    {
        private AccountsDBContext db = new AccountsDBContext();

        // GET: Accounts
        public ActionResult Index()
        {
            return View(db.Accounts.ToList());
        }

        // GET: Accounts/Details/5
        public ActionResult Details(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(Id);
            if (account == null || !Id.ToString().Equals(Session["UserID"].ToString()))//Prevent url get details of another user.
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // GET: Accounts/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Accounts/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "Id,Usr,Password,ConfirmPassword,Email,Address,City,Country")] Account account)
        {
            try
            {//check if user already exist...
                var acc = db.Accounts.Single(u => u.Usr == account.Usr);
                if (acc.Usr != null)
                {
                    ModelState.AddModelError("", "User already exist.");
                    return View();
                }

            }
            catch (InvalidOperationException e)
            {//if acc is null do next..
                if (ModelState.IsValid)
                {
                    db.Accounts.Add(account);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(account);
        }

        //login page.
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Account user)
        {
            try
            {
                var usr = db.Accounts.Single(u => u.Usr == user.Usr && u.Password == user.Password);
                if (usr != null)
                {
                    Session["UserID"] = usr.Id.ToString();
                    Session["Username"] = usr.Usr.ToString();
                    Session["Admin"] = usr.IsAdmin.ToString();
                    return RedirectToAction("index", "PostsToShow");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Wrong username or password.");
            }



            return View();
        }

        public ActionResult LoggedIn()
        {
            if (Session["UserId"] != null)
                return View();
            else
                return RedirectToAction("Login");
        }

        // GET: Accounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Usr,Password,ConfirmPassword,Email,Address,City,Country")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(account);
        }

        // GET: Accounts/Delete/5
        public ActionResult Delete(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(Id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int Id)
        {
            Account account = db.Accounts.Find(Id);
            db.Accounts.Remove(account);
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
