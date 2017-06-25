using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Shauli.Models
{
    public class Tweets
    {
        public int ID { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public String TweetContent { get; set; }
    }


    public class TweetsDbContext : DbContext
    {
        public DbSet<Shauli.Models.Tweets> Tweets { get; set; }
    }
}