using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;

namespace Model.DTOs
{
    public class BlogDto
    {
        public int EventId { get; set; }
        public string Name { get; set; }
        public string BlogUrl { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string CoverPage { get; set; }
        public string SquareCover { get; set; }

        public DateTime CreationDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime UpdateDate { get; set; }

        public Boolean State { get; set; }
    }

    public class BlogCreateDto
    {

        [DisplayName("Nombre")]
        [Required(ErrorMessage = "El Nombre es requerido."), MaxLength(150)]
        [RegularExpression(@"^[-a-zA-Z0-9ñÑáéíóúáéíóúÁÉÍÓÚ ]{1,150}$", ErrorMessage = "No se permiten caracteres especiales.")]
        public string Name { get; set; }
        public string BlogUrl { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public IFormFile CoverPage { get; set; }
        public IFormFile SquareCover { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public Boolean State { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
    }

    public class BlogEditDto
    {
        public int EventId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public IFormFile CoverPage { get; set; }
        public IFormFile SquareCover { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public Boolean State { get; set; }

    }

}
