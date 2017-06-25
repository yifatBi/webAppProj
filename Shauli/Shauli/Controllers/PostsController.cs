using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Shauli.Models;

namespace Shauli.Controllers
{
    public class PostsController : Controller
    {
        private PostsDbContext db = new PostsDbContext();

        // GET: Posts
        // Get posts that contains this data in the post or the comment
        public ActionResult Index(string name,string title,string text,string dateFilter)
        {
            DateTime startDate,endDate;
            var list=db.Posts.Join(db.Comments,
                p => p.ID,
                c => c.PostID, (p, c) => new { Post = p, Comment = c });
            var posts = from p in db.Posts select p;
            if (!string.IsNullOrEmpty(name))
            {
                list = list.Where(r => r.Comment.AuthorName.Contains(text) || r.Post.AuthorName.Contains(text));
                ViewBag.NameFilter = name;
            }
            //Filter accortding title
            if (!string.IsNullOrEmpty(title))
            {
                list = list.Where(r => r.Post.Title.Contains(title)|| r.Comment.Title.Contains(title));
                ViewBag.TitleFilter = title;
            }
            //Filter accortding content in the post or comment
            if (!string.IsNullOrEmpty(text))
            {
                list = list.Where(r => r.Comment.CommentContent.Contains(text) || r.Post.PostContent.Contains(text));
                ViewBag.TextFilter = text;
            }
            //Filter according date updated of comment or post
            if (!string.IsNullOrEmpty(dateFilter) && DateTime.TryParse(dateFilter, out startDate))
            {
                endDate = startDate;
                endDate = endDate.AddDays(1);
                list = list.Where(r => (r.Comment.CommentDate>=startDate && r.Comment.CommentDate < endDate) ||
                                    (r.Post.PostDate >= startDate && r.Post.PostDate < endDate));
                ViewBag.DateFilter = dateFilter;
            }
            //Return the posts that contain the expected data
            return View(list.Select(r=>r.Post).Distinct().ToList());
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posts posts = db.Posts.Find(id);
            if (posts == null)
            {
                return HttpNotFound();
            }
            return View(posts);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "ID,Title,PostContent,ImagePath,VideoPath")] Posts posts)
        {
            posts.PostDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                if (Session["UserID"] != null)//Check if session is running.
                {

                    posts.AuthorName = Session["Username"].ToString();
                    posts.AuthorURL = "/Accounts/Details/" + Session["UserID"].ToString();
                    db.Posts.Add(posts);
                    db.SaveChanges();
                }
                    
                return RedirectToAction("Index");
            }

            return View(posts);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posts posts = db.Posts.Find(id);
            if (posts == null)
            {
                return HttpNotFound();
            }
            return View(posts);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,AuthorName,AuthorURL,PostDate,PostContent,ImagePath,VideoPath")] Posts posts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(posts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(posts);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posts posts = db.Posts.Find(id);
            if (posts == null)
            {
                return HttpNotFound();
            }
            return View(posts);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Posts posts = db.Posts.Find(id);
            db.Posts.Remove(posts);
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
