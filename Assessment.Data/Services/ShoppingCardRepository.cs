using Assessment.Data.Exceptions;
using Assessment.Data.Interfaces;
using Assessment.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.Data.Services
{
    public class ShoppingCardRepository : IShoppingCardRepository
    {
        private readonly AssessmentDbContext db;

        public ShoppingCardRepository(AssessmentDbContext db)
        {
            this.db = db;
        }

        public async Task AddToActiveShoppingCard(string userId, ShoppingCardEntry entry)
        {
            var shoppingCard = await GetActiveShoppingCard(userId);

            if (shoppingCard == null)
            {
                shoppingCard = new ShoppingCard(userId);
                await db.ShoppingCards.AddAsync(shoppingCard);
                await db.SaveChangesAsync();
            }

            shoppingCard.AddEntry(entry);
            await db.SaveChangesAsync();
        }

        public async Task<ShoppingCard> CheckOutShoppingCard(int cardId)
        {
            var card = await GetShoppingCard(cardId)
                ?? throw new EntryNotFoundException($"Card doesn't exist. Id: {cardId}");
            return CheckOutShoppingCard(card);
        }

        private static ShoppingCard CheckOutShoppingCard(ShoppingCard card)
        {
            if (!card.IsActive)
                throw new CheckOutFailedException("Card is already closed.", card);

            return card.CheckOut();
        }

        public async Task<ShoppingCard> CheckOutShoppingCard(string userId)
        {
            var card = await GetActiveShoppingCard(userId)
                ?? throw new EntryNotFoundException($"No active card exists for the user. UserId: {userId}");
            return CheckOutShoppingCard(card);
        }

        public async Task<ShoppingCard> GetActiveShoppingCard(string userId)
        {
            var user = (await this.db.Users.Include(user => (user as User).ShoppingCards)
                                           .FirstOrDefaultAsync(user => user.Id == userId)) as User;
            return user.ShoppingCards.FirstOrDefault(card => card.IsActive);
        }

        public async Task<ShoppingCard> GetShoppingCard(int cardId)
        {
            return await db.ShoppingCards.FindAsync(cardId);
        }

        public async Task RemoveFromActiveShoppingCard(string userId, int entryId)
        {
            var card = await GetActiveShoppingCard(userId)
                ?? throw new EntryNotFoundException($"No active card exists for the user. UserId: {userId}");
            await RemoveEntry(entryId, card);
        }

        private async Task RemoveEntry(int entryId, ShoppingCard card)
        {
            card.RemoveEntry(entryId);
            await db.SaveChangesAsync();
        }

        public async Task RemoveFromActiveShoppingCard(int cardId, int entryId)
        {
            var card = await GetShoppingCard(cardId)
                ?? throw new EntryNotFoundException($"Card doesn't exist. Id: {cardId}");
            await RemoveEntry(entryId, card);
        }
    }
}
