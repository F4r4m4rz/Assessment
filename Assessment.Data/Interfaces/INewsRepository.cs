using Assessment.Data.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.Data.Interfaces
{
    public interface INewsRepository
    {
        Task<News> GetAsync(int id);
        Task<IEnumerable<News>> GetAllAsync();
        Task Add(News news);
        Task RemoveAsyc(int id);
        Task UpdateAsync(News news);
    }
}
