using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Review
    {
        public int ReviewID { get; set; }
        public string TittleReview { get; set; }
        public string Description { get; set; }
        public int Assessment { get; set; }
        public string UserId { get; set; }

        public int? PlaceId { get; set; }
        public Place Place { get; set; }
        
        public List<Gallery> Galleries { get; set; }
    }
}
