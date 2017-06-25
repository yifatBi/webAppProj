using Shauli.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shauli.Controllers
{
    public class BlogController : Controller
    {
        // GET: Blog
        public ActionResult Index(string name,string text)
        {
            return View();
        }
    }
}