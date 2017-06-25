using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Shauli.Models;
using System.Data.Entity.Core.Objects;

namespace Shauli.Controllers
{
    public class PostsToShowController : Controller
    {
        private PostsDbContext db = new PostsDbContext();

        // GET: PostsToShow
        public ActionResult Index(string name,string text,string dateFilter)
        {
            DateTime startDate,endDate;
            //Fetch the comments
            var posts = db.Posts
                .Include(p => p.Comments);
            //Filter according given data
            //filter according author name
            if (!string.IsNullOrEmpty(name))
            {
                posts = posts.Where(p => p.AuthorName.Contains(name));
                ViewBag.NameFilter = name;
            }
            //Filter accortding text
            if (!string.IsNullOrEmpty(text))
            {
                posts = posts.Where(p => p.Title.Contains(text) || p.PostContent.Contains(text));
                ViewBag.TextFilter = text;
            }
            //Filter according date (take complete date)
            if (!string.IsNullOrEmpty(dateFilter) && DateTime.TryParse(dateFilter, out startDate))
            {
                endDate = startDate;
                endDate=endDate.AddDays(1);
                posts = posts.Where(p => p.PostDate>=startDate && p.PostDate<endDate);
                ViewBag.DateFilter = dateFilter;
            }
            return View(posts.OrderByDescending(p => p.PostDate).ToList());
        }

        //Create new action result for comment.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComment([Bind(Include = "PostID,Title,AuthorName,AuthorURL,CommentContent")] Comment comment)
        {
            comment.CommentDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                comment.AuthorName = Session["Username"].ToString();
                comment.AuthorURL = "/Accounts/Details/" + Session["UserID"].ToString();
                db.Comments.Add(comment);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: PostsToShow/Details/5
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




    }
}
