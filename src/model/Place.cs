using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Model
{
    public class Place
    {
        public int PlaceId { get; set; }
        public string Nit { get; set; }
        public string Name { get; set; }
        public string NameUrl { get; set; }
        public string Phone { get; set; }
        public string Admin { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public string CoverPage { get; set; }
        public string Logo { get; set; }
        public string Contract { get; set; }
        public string CreationDate { get; set; }
        public Boolean State { get; set; }
        public string UpdateDate { get; set; }
        public string LatitudeCoordinates { get; set; }
        public string LengthCoordinates { get; set; }

        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        public List<Product> Products { get; set; }
    }
}
