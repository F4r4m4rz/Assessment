using Assessment.Data.Interfaces;
using Assessment.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.Data.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly AssessmentDbContext db;

        public UserRepository(AssessmentDbContext db)
        {
            this.db = db;
        }

        public async Task<User> GetUser(string userId)
        {
            var user = (await this.db.Users.Include(user => (user as User).ShoppingCards)
                         .FirstOrDefaultAsync(user => user.Id == userId)) as User;

            return user;
        }
    }
}
