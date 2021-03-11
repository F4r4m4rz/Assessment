using Assessment.Data.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.Data.Interfaces
{
    public interface IShoppingCardRepository
    {
        Task AddToActiveShoppingCard(string userId, ShoppingCardEntry entry);
        Task<ShoppingCard> GetShoppingCard(int cardId);
        Task<ShoppingCard> GetActiveShoppingCard(string userId);
        Task RemoveFromActiveShoppingCard(string userId, int entryId);
        Task RemoveFromActiveShoppingCard(int cardId, int entryId);
        Task<ShoppingCard> CheckOutShoppingCard(int cardId);
        Task<ShoppingCard> CheckOutShoppingCard(string userId);
        Task<IEnumerable<ShoppingCard>> ShoppingCardHistory(string userId);
    }
}
