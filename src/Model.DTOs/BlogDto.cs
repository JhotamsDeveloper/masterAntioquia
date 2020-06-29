using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DTOs
{
    public class BlogDto
    {
        public int BlogId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
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
    }
}
