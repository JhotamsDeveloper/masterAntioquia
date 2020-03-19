using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DTOs
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string CoverPage { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public float HighPrice { get; set; }
        public float HalfPrice { get; set; }
        public float LowPrice { get; set; }
        public int Discounts { get; set; }
        public bool Statud { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }

        //Claves foreanas
        public int PlaceId { get; set; }
        public Place Place { get; set; }

        public List<Gallery> ListGalleries { get; set; }
    }

    public class ProductCreateDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string CoverPage { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public float HighPrice { get; set; }
        public float HalfPrice { get; set; }
        public float LowPrice { get; set; }
        public int Discounts { get; set; }
        public bool Statud { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }

        //Claves foreanas
        public int PlaceId { get; set; }
        public Place Place { get; set; }

        public List<Gallery> ListGalleries { get; set; }
    }
}
