using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAntioquia.Models
{
    public class ReviewsGetView
    {
        public string TittleReview { get; set; }
        public string Description { get; set; }
        public int Assessment { get; set; }
        public string NameUser { get; set; }
        public string DateCreateReview { get; set; }
        public List<Gallery> Galleries { get; set; }
    }
}
