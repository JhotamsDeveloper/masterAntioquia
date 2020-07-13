using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string ProductUrl { get; set; }
        public string CoverPage { get; set; }
        public string SquareCover { get; set; }
        public string Description { get; set; }
        public string Mineral { get; set; }
        public string Price { get; set; }
        public float Increments { get; set; }
        public float ShippingValue { get; set; }
        public int Discounts { get; set; }
        public int AmountSupported { get; set; }
        public bool Statud { get; set; }

        public int PersonNumber { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }

        //Claves foreanas
        public int PlaceId { get; set; }
        public Place Place { get; set; }

        public List<Gallery> Galleries { get; set; }

        //Comodidades
        //Reservas
    }
}
