﻿using Model.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GestionAntioquia.Models
{
    public class BlogView
    {
        public int EventId { get; set; }
        public string Name { get; set; }
        public string BlogUrl { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public string Author { get; set; }
        public string CoverPage { get; set; }
        public string SquareCover { get; set; }

        public DateTime CreationDate { get; set; }

        public string UpdateDate { get; set; }

        public Boolean State { get; set; }
        public ICollection<ProductsView> Products { get; set; }

    }
}
