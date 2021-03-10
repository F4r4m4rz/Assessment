using Assessment.Data.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assessment.Data.Services
{
    public class AssessmentDbContext : IdentityDbContext
    {
        public AssessmentDbContext(DbContextOptions<AssessmentDbContext> option) : base(option)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<News> News { get; set; }
    }
}
