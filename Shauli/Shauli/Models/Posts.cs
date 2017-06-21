using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Shauli.Models
{
    public class Posts
    {
        public int ID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Author Name")]
        public string AuthorName { get; set; }

        [Required]
        [DataType(DataType.Url)]
        [Display(Name = "Author Website")]
        public string AuthorURL { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Publish Date")]
        public DateTime PostDate { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string PostContent { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Image Url")]
        public string ImagePath { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Video Url")]
        public string VideoPath { get; set; }

       // public virtual ICollection<BlogPost> BlogPosts { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }

    public class PostsDbContext : DbContext
    {
        public DbSet<Posts> Posts { get; set; }

        public System.Data.Entity.DbSet<Shauli.Models.Comment> Comments { get; set; }
    }
}