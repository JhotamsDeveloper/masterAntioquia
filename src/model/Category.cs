using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Icono { get; set; }
        public bool Stated { get; set; }
        public List<Place> Places { get; set; }
    }
}
