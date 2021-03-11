using Assessment.Data.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUser(string userId); 
    }
}
