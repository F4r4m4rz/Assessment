using Assessment.Data.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.Data.Interfaces
{
    public interface IProductRepository
    {
        Product Get(int id);
        IEnumerable<Product> GetAll();
        IEnumerable<Product> GetPaginated(int page, int perPage);
        Task<Product> GetAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<IEnumerable<Product>> GetPaginatedAsync(int page, int perPage);
        void Add(Product product);
        void Update(Product product);
        void Remove(int id);
    }
}
