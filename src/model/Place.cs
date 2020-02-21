using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Place
    {
        public int PlaceId { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string admin { get; set; }
        public string CoverPage { get; set; }
        public List<Product> Products { get; set; }
    }
}
