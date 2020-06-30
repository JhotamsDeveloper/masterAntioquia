using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "El Icono es requerido")]
        public string Icono { get; set; }

        public bool Stated { get; set; }
        public List<Place> Places { get; set; }
        public List<Event> Events { get; set; }
    }
}
