﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAntioquia.Models.ViewPlaces
{
    public class ViewAliados
    {
        public ViewAliados()
        {

        }
        public int PlaceId { get; set; }
        public string Name { get; set; }
        public string CoverPage { get; set; }
        public string Contract { get; set; }
        public string Description { get; set; }
        public DateTime DataCreate { get; set; }
        public string New { get; set; }
    }
}
