using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAntioquia.Models
{
    public class StoreView
    {
        public int ProductId { get; set; }

        public string Name { get; set; }
        public string ProductUrl { get; set; }
        public string SquareCover { get; set; }
        public string Description { get; set; }
        public string Mineral { get; set; }
        public string PriceWhitIncrement { get; set; }
        public string ProductWithDiscounts { get; set; }
        public string ShippingValue { get; set; }
        public int Discounts { get; set; }
        public bool Statud { get; set; }

        //Claves foreanas
        public Place Place { get; set; }
    }

    public class StoreDetailView
    {
        public string Name { get; set; }
        public string CoverPage { get; set; }
        public string Description { get; set; }
        public string Mineral { get; set; }
        public string PriceWhitIncrement { get; set; }
        public string ProductWithDiscounts { get; set; }
        public string ShippingValue { get; set; }
        public string Discounts { get; set; }

        public Place Place { get; set; }
        public string Urban { get; set; }
        public ICollection<Gallery> Galleries { get; set; }
    }

}
