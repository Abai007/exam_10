using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace exam_10.Models
{
    public class DBContext: IdentityDbContext<User>
    {
        public DbSet<User> ContextUsers { get; set; }
        public DbSet<Institution> Institutions { get; set; }
        public DbSet<ImageModel> ImageModels { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DBContext(DbContextOptions<DBContext> options): base(options)
        {
        }
    }
}
