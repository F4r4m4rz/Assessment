using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Assessment.Data.Model
{
    public class ShoppingCardEntry
    {
        public ShoppingCardEntry()
        {

        }

        public int Id { get; set; }
        public Product Item { get; set; }
        public int Count { get; set; }

        [NotMapped]
        public double TotalPrice => Count * (Item?.Price ?? 0);
    }
}
