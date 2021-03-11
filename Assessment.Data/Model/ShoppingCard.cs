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
            Entries = new ObservableCollection<ShoppingCardEntry>();
            Entries.CollectionChanged += Entries_CollectionChanged;
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public ObservableCollection<ShoppingCardEntry> Entries { get; set; }
        public double TotalPrice { get; set; }
        public ShoppingCardStatusEnum Status { get; set; } = ShoppingCardStatusEnum.Active;

        [NotMapped]
        public bool IsActive => Status == ShoppingCardStatusEnum.Active;

        public void AddEntry(ShoppingCardEntry entry)
        {
            Entries.Add(entry);
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

        private void Entries_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            TotalPrice = 0;
            for (int i = 0; i < Entries.Count; i++)
            {
                TotalPrice += Entries[i].TotalPrice;
            }
        }

        internal void CheckOut()
        {
            Status = ShoppingCardStatusEnum.Closed;
        }
    }
}
