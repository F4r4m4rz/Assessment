using System;
using System.Collections.Generic;
using System.Text;

namespace Assessment.Data.Model
{
    public class News
    {
        public News()
        {
            DatePublished = DateTime.Now.Ticks;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public long DatePublished { get; set; }
    }
}
