using Assessment.Data.Interfaces;
using Assessment.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Assessment.Data.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly AssessmentDbContext db;

        public ProductRepository(AssessmentDbContext db)
        {
            this.db = db;
        }

        public void Add(Product product)
        {
            this.db.Products.Add(product);
            db.SaveChanges();
        }

        public Product Get(int id)
        {
            return this.db.Products.Find(id);
        }

        public IEnumerable<Product> GetAll()
        {
            return this.db.Products.AsNoTracking();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await this.db.Products.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllFilterdAsync(string filter)
        {
            return await this.db.Products.AsNoTracking()
                .Where(product => Regex.IsMatch(product.Name, $@"[\s\S]*{filter}[\s\S]*")).ToListAsync();
        }

        public async Task<Product> GetAsync(int id)
        {
            return await db.FindAsync<Product>(id);
        }

        public IEnumerable<Product> GetPaginated(int page, int perPage)
        {
            var products = db.Products.AsNoTracking()
                                      .Skip((page - 1) * perPage)
                                      .Take(perPage)
                                      .ToList();
            return products;
        }

        public async Task<IEnumerable<Product>> GetPaginatedAsync(int page, int perPage)
        {
            var products = db.Products.AsNoTracking()
                                      .Skip((page - 1) * perPage)
                                      .Take(perPage)
                                      .ToListAsync();
            return await products;
        }

        public void Remove(int id)
        {
            var product = Get(id);
            if (product == null)
                return;
            db.Products.Remove(product);
            db.SaveChanges();
        }

        public void Update(Product product)
        {
            db.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
        }
    }
}
