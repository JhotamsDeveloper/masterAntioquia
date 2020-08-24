using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Gallery
    {
        public int GalleryId { get; set; }
        public string NameImage { get; set; }

        public int? ProducId { get; set; }
        public Product Products { get; set; }

        public int? ReviewsId { get; set; }
        public Review Reviews { get; set; }

    }
}
