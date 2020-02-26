using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DTOs
{
    public class PlaceDto
    {
        public int PlaceId { get; set; }
        public string Nit { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Admin { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public Byte CoverPage { get; set; }
        public Byte Logo { get; set; }
        public string Contract { get; set; }
        public Boolean State { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<Product> Products { get; set; }
    }
    public class PlaceCreateDto
    {
        public string Nit { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Admin { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public Byte CoverPage { get; set; }
        public Byte Logo { get; set; }
        public string Contract { get; set; }
        public Boolean State { get; set; }
        public DateTime CreationDate { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<Product> Products { get; set; }
    }
}
