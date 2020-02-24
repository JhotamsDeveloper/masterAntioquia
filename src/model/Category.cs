using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    public class Category
    {
        public int CategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Icono { get; set; }
        public bool Stated { get; set; }
        public List<Place> Places { get; set; }
    }
}
