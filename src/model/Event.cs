using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;

namespace Model
{
    public class Event
    {
        public int EventId { get; set; }
        public string Name { get; set; }
        public string EventUrl { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string CoverPage { get; set; }
        public string SquareCover { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EventsDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime UpdateDate { get; set; }

        public Boolean State { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        public int? PlaceId { get; set; }
        public Place Place { get; set; }

        public List<Gallery> Galleries { get; set; }

    }
}
