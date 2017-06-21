using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace Shauli.Models
{
    public class Comment
    {
        public int ID { get; set; }

        public int PostID { get; set; }

        [Required]
        //[Display(Name = "Title")] 
        public string Title { get; set; }

        [Display(Name = "Author Name")]
        public string AuthorName { get; set; }

        [Required]
        [DataType(DataType.Url)]
        [Display(Name = "Author Website")]
        public string AuthorURL { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime CommentDate { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Comment")]
        public string CommentContent { get; set; }

        public  Posts Post { get; set; }

       // public virtual ICollection<BlogPost> BlogPosts { get; set; }
    }

    public class CommentDbContext : DbContext
    {
        public DbSet<Comment> Comments { get; set; }
    }
}