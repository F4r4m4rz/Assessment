using Assessment.Data.Interfaces;
using Assessment.Data.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assessment.Data.Services
{
    public class AssessmentDbContext : IdentityDbContext
    {
        public AssessmentDbContext(DbContextOptions<AssessmentDbContext> option) : base(option)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<ShoppingCard> ShoppingCards { get; set; }
        public DbSet<UserShoppingCardStorage> UserShoppingCardStorages { get; set; }
    }
}
