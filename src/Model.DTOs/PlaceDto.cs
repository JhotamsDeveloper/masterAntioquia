using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.DTOs
{
    public class PlaceDto
    {
        public int PlaceId { get; set; }
        public string Nit { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Admin { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string CoverPage { get; set; }
        public string Logo { get; set; }
        public string Contract { get; set; }
        public Boolean State { get; set; }
        public string CreationDate { get; set; }
        public string UpdateDate { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
        public List<Product> Products { get; set; }
    }
    public class PlaceCreateDto
    {

        [Required(ErrorMessage = "El Nit es requerido."), 
            MaxLength(15), 
            MinLength(10, ErrorMessage ="Cantidad Mínima de 10.")]
        public string Nit { get; set; }
        [DisplayName("Nombre")]
        [Required(ErrorMessage = "El Nombre es requerido."), MaxLength(10)]
        public string Name { get; set; }

        [DisplayName("Télefono")]
        [Required(ErrorMessage = "El télefono es requerido."), MaxLength(10)]
        public string Phone { get; set; }

        [DisplayName("Administrador")]
        [Required(ErrorMessage = "El administrador es requerido."), MaxLength(20)]
        public string Admin { get; set; }

        [DisplayName("Dirección")]
        [Required(ErrorMessage = "La dirección es requerido."), MaxLength(10)]
        public string Address { get; set; }

        [DisplayName("Descripción")]
        [Required(ErrorMessage = "La descripción es requerido.")]
        public string Description { get; set; }

        [DisplayName("Portada")]
        [Required(ErrorMessage = "La portada es requerida.")]
        public string CoverPage { get; set; }

        [DisplayName("Logo")]
        [Required(ErrorMessage = "La logo es requerida.")]
        public string Logo { get; set; }

        [DisplayName("Contracto")]
        [Required(ErrorMessage = "El tipo de contracto es requerido."), MaxLength(10)]
        public string Contract { get; set; }

        [DisplayName("Activar")]
        public Boolean State { get; set; }
        public string CreationDate { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
        public List<Product> Products { get; set; }
    }

    public class PlaceEditDto
    {
        public int PlaceId { get; set; }
        public string Nit { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Admin { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string CoverPage { get; set; }
        public string Logo { get; set; }
        public string Contract { get; set; }
        public Boolean State { get; set; }
        public string UpdateDate { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
        public List<Product> Products { get; set; }
    }
}
