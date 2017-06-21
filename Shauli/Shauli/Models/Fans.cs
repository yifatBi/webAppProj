using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Shauli.Models
{
    public class Fans
    {
        public int ID { get; set; }

        public string FirstName { get; set; }

        public int YearsOfMemebership { get; set; }

        public string sex { get; set; }

        public string  LastName{ get; set; }

        public DateTime BirthDate { get; set; }

        public string Email { get; set; }

    }

    public class FansDBContext : DbContext {
        public DbSet<Fans> Fans { get; set; }
    }
}