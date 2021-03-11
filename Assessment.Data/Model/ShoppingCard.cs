using Assessment.Data.Exceptions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Assessment.Data.Model
{
    public enum ShoppingCardStatusEnum { Active, Closed }

    public class ShoppingCard
    {
        public ShoppingCard()
        {
            Entries = new List<ShoppingCardEntry>();
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public ICollection<ShoppingCardEntry> Entries { get; set; }
        public ShoppingCardStatusEnum Status { get; set; } = ShoppingCardStatusEnum.Active;

        [NotMapped]
        public bool IsActive => Status == ShoppingCardStatusEnum.Active;

        [NotMapped]
        public double TotalPrice => Entries.Sum(entry => entry.TotalPrice);

        public void AddEntry(ShoppingCardEntry entry)
        {
            var alreadyExists = Entries.FirstOrDefault(e => e.ProductId == entry.ProductId);
            if (alreadyExists == null)
            {
                Entries.Add(entry);
                return;
            }

            alreadyExists.Count += entry.Count;
        }

        public void RemoveEntry(ShoppingCardEntry entry)
        {
            Entries.Remove(entry);
        }

        public void RemoveEntry(int entryId)
        {
            var entry = Entries.FirstOrDefault(a => a.Id == entryId)
                ?? throw new EntryNotFoundException($"No entry found in the card by id: {entryId}");
            Entries.Remove(entry);
        }

        internal void CheckOut()
        {
            Status = ShoppingCardStatusEnum.Closed;
        }
    }
}
