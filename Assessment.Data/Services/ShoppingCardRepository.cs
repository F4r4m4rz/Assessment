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
        private readonly IUserShoppingCardStorageRepository userRepository;

        public ShoppingCardRepository(AssessmentDbContext db, IUserShoppingCardStorageRepository userRepository)
        {
            this.db = db;
            this.userRepository = userRepository;
        }

        public async Task AddToActiveShoppingCard(string userId, ShoppingCardEntry entry)
        {
            var shoppingCard = await GetActiveShoppingCard(userId);

            if (shoppingCard == null)
            {
                shoppingCard = new ShoppingCard();
            }

            shoppingCard.AddEntry(entry);
            await userRepository.AddShoppingCardToStorage(userId, shoppingCard);
        }

        public async Task<ShoppingCard> CheckOutShoppingCard(int cardId)
        {
            var card = await GetShoppingCard(cardId)
                ?? throw new EntryNotFoundException($"Card doesn't exist. Id: {cardId}");
            return await CheckOutShoppingCard(card);
        }

        private async Task<ShoppingCard> CheckOutShoppingCard(ShoppingCard card)
        {
            if (!card.IsActive)
                throw new CheckOutFailedException("Card is already closed.", card);

            card.CheckOut();
            await UpdateCard(card);

            return card;
        }

        public async Task<ShoppingCard> CheckOutShoppingCard(string userId)
        {
            var card = await GetActiveShoppingCard(userId)
                ?? throw new EntryNotFoundException($"No active card exists for the user. UserId: {userId}");
            return await CheckOutShoppingCard(card);
        }

        public async Task<ShoppingCard> GetActiveShoppingCard(string userId)
        {
            var shoppingCardStorage = (await this.userRepository.GetUserShoppingCardStorage(userId))
                ?? throw new EntryNotFoundException($"Shopping card storage could not be found for user with id: {userId}");

            return shoppingCardStorage.ShoppingCards.FirstOrDefault(card => card.IsActive);
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
            await UpdateCard(card);
        }

        public async Task RemoveFromActiveShoppingCard(int cardId, int entryId)
        {
            var card = await GetShoppingCard(cardId)
                ?? throw new EntryNotFoundException($"Card doesn't exist. Id: {cardId}");
            await RemoveEntry(entryId, card);
        }

        public async Task<IEnumerable<ShoppingCard>> ShoppingCardHistory(string userId)
        {
            var user = (await this.userRepository.GetUserShoppingCardStorage(userId))
                ?? throw new EntryNotFoundException($"User could not be found with id: {userId}");

            return user.ShoppingCards;
        }

        private async Task UpdateCard(ShoppingCard card)
        {
            db.Entry(card).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}
