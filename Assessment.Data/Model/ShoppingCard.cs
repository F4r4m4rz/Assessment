using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Assessment.Data.Model
{
    public class ShoppingCard
    {
        public ShoppingCard()
        {
            Entries = new ObservableCollection<ShoppingCardEntry>();
            Entries.CollectionChanged += Entries_CollectionChanged;
        }

        public ShoppingCard(User user) : this()
        {
            User = user;
        }

        public int Id { get; set; }
        public ObservableCollection<ShoppingCardEntry> Entries { get; set; }
        public double TotalPrice { get; set; }
        public User User { get; set; }

        public void AddEntry(ShoppingCardEntry entry)
        {
            Entries.Add(entry);
        }

        public void RemoveEntry(ShoppingCardEntry entry)
        {
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
    }
}
