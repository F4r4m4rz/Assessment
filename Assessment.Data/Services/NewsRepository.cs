using Assessment.Data.Interfaces;
using Assessment.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.Data.Services
{
    public class NewsRepository : INewsRepository
    {
        private readonly AssessmentDbContext db;

        public NewsRepository(AssessmentDbContext db)
        {
            this.db = db;
        }

        public async Task Add(News news)
        {
            await this.db.AddAsync(news);
            await this.db.SaveChangesAsync();
        }

        public async Task<IEnumerable<News>> GetAllAsync()
        {
            return await this.db.News.AsNoTracking().ToListAsync();
        }

        public async Task<News> GetAsync(int id)
        {
            return await this.db.FindAsync<News>(id);
        }

        public async Task RemoveAsyc(int id)
        {
            var news = await GetAsync(id);
            this.db.News.Remove(news);
            await this.db.SaveChangesAsync();
        }

        public async Task UpdateAsync(News news)
        {
            db.Entry(news).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}
