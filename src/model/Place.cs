using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Place
    {
        public int PlaceId { get; set; }
        public string Nit { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string admin { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string CoverPage { get; set; }
        public string Logo { get; set; }
        public int Contract { get; set; }
        public Boolean State { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<Product> Products { get; set; }
    }
}
