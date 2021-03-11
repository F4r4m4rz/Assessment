using Assessment.Data.Exceptions;
using Assessment.Data.Interfaces;
using Assessment.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.Data.Services
{
    public class UserShoppingCardStorageRepository : IUserShoppingCardStorageRepository
    {
        private readonly AssessmentDbContext db;

        public UserShoppingCardStorageRepository(AssessmentDbContext db)
        {
            this.db = db;
        }

        public async Task<UserShoppingCardStorage> GetUserShoppingCardStorage(string userId)
        {
            var shoppingCardStorage = (await this.db.UserShoppingCardStorages.Include(user => user.ShoppingCards)
                                                                             .ThenInclude(card => card.Entries)
                                                                             .ThenInclude(entry => entry.Item)
                         .FirstOrDefaultAsync(storage => storage.Id == userId));

            if (shoppingCardStorage == default(UserShoppingCardStorage))
            {
                shoppingCardStorage = new UserShoppingCardStorage(userId);
                db.UserShoppingCardStorages.Add(shoppingCardStorage);
                await db.SaveChangesAsync();
            }

            return shoppingCardStorage;
        }

        public async Task AddShoppingCardToStorage(string userId, ShoppingCard card)
        {
            var shoppingCardStorage = await GetUserShoppingCardStorage(userId)
                ?? throw new EntryNotFoundException($"Could not find or create shopping card storage for user with id: {userId}");

            shoppingCardStorage.ShoppingCards.Add(card);
            await db.SaveChangesAsync();
        }
    }
}
