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
    public class TweetsController : Controller
    {
        private AccountsDBContext db = new AccountsDBContext();

        public ActionResult Tweet()
        {
            return View();
        }



        [HttpPost]
        public ActionResult Tweet(Tweets tweet)
        {
            if (Session["Admin"] != null && Session["Admin"].ToString().Equals("True") //Check if admin.
                && tweet!=null&&tweet.TweetContent!=null)//check if tweet content is valid.
            {
                Tweetinvi.Auth.SetUserCredentials("OYTu2REUYtl1OvGlJuSD4Bkqv",
                    "3z3Jwu7Wu1EBWRf6zrCRh4Hiv0qDQX9Y2fwkzNDnfN92xTzAfh",
                    "879004861668478977-molXXNoVBwZOwgsLji5YXRRdGjsEXDg",
                    "vifndBdgd216bqy7Gaj4KjrYWOiiQJU5v2cqeCbAB1lIl");
                Tweetinvi.Tweet.PublishTweet(tweet.TweetContent);
            }
            return RedirectToAction("index", "PostsToShow");
        }
    }
}
