using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAntioquia.Models
{
    public class ToursView
    {
        public string Name { get; set; }
        public string CoverPage { get; set; }
        public string Description { get; set; }


        public int? PlaceId { get; set; }
        public Place Place { get; set; }
        public bool TourIsUrban { get; set; }

        public ICollection<Gallery> Galleries { get; set; }
        public ICollection<ProductsView> Products { get; set; }
    }
}
