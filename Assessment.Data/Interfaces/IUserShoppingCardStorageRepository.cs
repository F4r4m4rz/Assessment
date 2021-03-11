using Assessment.Data.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.Data.Interfaces
{
    public interface IUserShoppingCardStorageRepository
    {
        Task<UserShoppingCardStorage> GetUserShoppingCardStorage(string userId);
        Task AddShoppingCardToStorage(string userId, ShoppingCard card);
    }
}
