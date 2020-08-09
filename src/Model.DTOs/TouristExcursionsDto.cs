using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.DTOs
{
    public class TouristExcursionsDto
    {
        public int EventId { get; set; }
        public string Name { get; set; }
        public string TourUrl { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string CoverPage { get; set; }
        public string SquareCover { get; set; }

        public DateTime EventsDate { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public Boolean State { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        public int? PlaceId { get; set; }
        public Place Place { get; set; }
        public string City { get; set; }
        public string Business { get; set; }
        public string Reference { get; set; }
        public List<Gallery> Galleries { get; set; }
    }
}
