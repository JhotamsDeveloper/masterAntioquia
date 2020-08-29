using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAntioquia.Models
{
    public class ProductsView
    {
        public int ProductId { get; set; }

        public string Name { get; set; }
        public string ProductUrl { get; set; }
        public string CoverPage { get; set; }
        public string SquareCover { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string ProductWithDiscounts { get; set; }
        public int Discounts { get; set; }
        public bool Statud { get; set; }
        public int PersonNumber { get; set; }

        //Claves foreanas
        public Place Place { get; set; }
    }

    public class ProductDetailView
    {
        public string Name { get; set; }
        public string CoverPage { get; set; }
        public string SquareCover { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string ProductWithDiscounts { get; set; }
        public string Discounts { get; set; }
        public bool Statud { get; set; }
        public int PersonNumber { get; set; }

        public Place Place { get; set; }
        public List<Gallery> Galleries { get; set; }
    }
}
