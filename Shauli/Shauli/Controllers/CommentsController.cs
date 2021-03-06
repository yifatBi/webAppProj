﻿using System;
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
    public class CommentsController : Controller
    {
        private PostsDbContext db = new PostsDbContext();

        // GET: Comments
        public ActionResult Index(string pID,string query)
        {
            if (string.IsNullOrEmpty(query))
            { query = ""; }

            var comments = db.Comments.Where(c => (c.AuthorName.Contains(query) || 
            c.CommentContent.Contains(query) || c.Title.Contains(query) || 
            c.Post.Title.Contains(query))).Include(c => c.Post);
            ViewBag.Query = query;
            
            if (!string.IsNullOrEmpty(pID))
            {
                int tempID = Int32.Parse(pID);
                comments = comments.Where(c => c.PostID == tempID).Include(c => c.Post);
                ViewBag.PostID = tempID;
            }

            GetStatisticsCommentPerUser();


            return View(comments.ToList());
        }
        //How much users commented on his post
        public void GetStatisticsCommentPerUser()
        {
            List<JoinCommentPost> joinList = new List<JoinCommentPost>();
            var list = from post in db.Posts
                       join c in db.Comments on post.AuthorName equals c.AuthorName
                       select new { Author = post.AuthorName, PostTitle = post.Title, CommentNum = c.ID };
            var groupBy = list.GroupBy(r => new { r.Author, r.PostTitle });
            var listTmp=groupBy.Select( grp => new {
                PostTitle = grp.Key.PostTitle,
                AuthorName = grp.Key.Author,
                Count = grp.Count()
            }).ToList();

            foreach(var item in listTmp)
            {
                joinList.Add(new JoinCommentPost { Title = item.PostTitle, Author = item.AuthorName, Count = item.Count });
            }

            ViewBag.Stats = joinList;
        }
        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
        public ActionResult Create()
        {
            ViewBag.PostID = new SelectList(db.Posts, "ID", "Title");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PostID,Title,AuthorName,AuthorURL,CommentDate,CommentContent")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PostID = new SelectList(db.Posts, "ID", "Title", comment.PostID);
            return View(comment);
        }

        // GET: Comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.PostID = new SelectList(db.Posts, "ID", "Title", comment.PostID);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PostID,Title,AuthorName,AuthorURL,CommentDate,CommentContent")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PostID = new SelectList(db.Posts, "ID", "Title", comment.PostID);
            return View(comment);
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
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
