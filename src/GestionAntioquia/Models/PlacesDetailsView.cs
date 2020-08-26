using Model;
using Model.DTOs;
using System;
using System.Collections.Generic;

namespace GestionAntioquia.Models
{
    public class PlacesDetailsView : ReviewsCreateDto
    {
        public int PlaceId { get; set; }
        public string Nit { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Admin { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string urban { get; set; }
        public string Description { get; set; }
        public string NameUrl { get; set; }
        public string CoverPage { get; set; }
        public string Logo { get; set; }
        public string Contract { get; set; }
        public string CreationDate { get; set; }
        public string LatitudeCoordinates { get; set; }
        public string LengthCoordinates { get; set; }
        public string stateMessageCreate { get; set; }
        public int TotalReviews { get; set; }
        public List<Product> Products { get; set; }
        public List<ReviewsGetView> Reviews { get; set; }
    }
}
