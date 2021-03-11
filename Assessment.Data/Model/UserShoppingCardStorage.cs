using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Assessment.Data.Model
{
    public class UserShoppingCardStorage
    {
        public UserShoppingCardStorage()
        {
            ShoppingCards = new ObservableCollection<ShoppingCard>();
            ShoppingCards.CollectionChanged += ShoppingCards_CollectionChanged;
        }

        public UserShoppingCardStorage(string userId) : this()
        {
            this.Id = userId;
        }

        private void ShoppingCards_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (ShoppingCard item in e.NewItems)
            {
                item.UserId = Id;
            }
        }

        [Key, ForeignKey(nameof(Identity))]
        public string Id { get; set; }
        public IdentityUser Identity { get; set; }
        public ObservableCollection<ShoppingCard> ShoppingCards { get; set; }
    }
}
