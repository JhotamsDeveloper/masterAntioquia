using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Commons
{
    public class DataCollection<T> where T : class
    {
        public bool HasItems
        {
            get
            {
                return Items != null && Items.Any();
            }
        }
        public IEnumerable<T> Items { get; set; }
        public int Total { get; set; }
        public int Page { get; set; }
        public int Pages { get; set; }
    }
}
