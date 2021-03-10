using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assessment.Data.Model
{
    public class User : IdentityUser
    {
        public User()
        {

        }

        public ICollection<ShoppingCard> ShoppingCards { get; set; }
    }
}
