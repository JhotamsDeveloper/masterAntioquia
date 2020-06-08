using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.DTOs
{
    public class ProductDto
    {
        public int ProductId { get; set; }

        [DisplayName("Nombre")]
        [Required(ErrorMessage = "El Nombre es requerido."), MaxLength(150)]
        [RegularExpression(@"^[-a-zA-Z0-9ñÑáéíóúáéíóúÁÉÍÓÚ ]{1,150}$", ErrorMessage = "No se permiten caracteres especiales.")]
        public string Name { get; set; }
        public string ProductUrl { get; set; }
        public string CoverPage { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int HighPrice { get; set; }
        public int HalfPrice { get; set; }
        public int LowPrice { get; set; }
        public int Discounts { get; set; }
        public bool Statud { get; set; }
        public int AmountSupported { get; set; }
        public int PersonNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }

        //Claves foreanas
        public int PlaceId { get; set; }
        public Place Place { get; set; }

        public ICollection<Gallery> Galleries { get; set; }
    }

    public class ProductCreateDto
    {
        public int ProductId { get; set; }

        [DisplayName("Nombre")]
        [Required(ErrorMessage = "El Nombre es requerido."), MaxLength(150)]
        [RegularExpression(@"^[-a-zA-Z0-9ñÑáéíóúáéíóúÁÉÍÓÚ ]{1,150}$", ErrorMessage = "No se permiten caracteres especiales.")]
        public string Name { get; set; }

        public string ProductUrl { get; set; }

        [DisplayName("Portada")]
        [Required(ErrorMessage = "La portada es requerido.")]
        public IFormFile CoverPage { get; set; }

        [DisplayName("Descripción")]
        [Required(ErrorMessage = "La descripción es requerido.")]

        public string Description { get; set; }

        [DisplayName("Precio enbruto")]
        [Required(ErrorMessage = "El precio enbruto es requerido.")]
        public float Price { get; set; }

        [DisplayName("Temporada alta")]
        [Required(ErrorMessage = "El precio de temporada alta es requerido.")]
        public float HighPrice { get; set; }

        [DisplayName("Temporada media")]
        [Required(ErrorMessage = "El precio de temporada media es requerido.")]
        public float HalfPrice { get; set; }

        [DisplayName("Temporada baja")]
        [Required(ErrorMessage = "El precio de temporada baja es requerido.")]
        public float LowPrice { get; set; }

        [DisplayName("Descuento")]
        public int Discounts { get; set; }

        [DisplayName("Número de Personas")]
        public int PersonNumber { get; set; }

        [DisplayName("Galeria de fotos")]
        public IEnumerable<IFormFile> Gallery { get; set; }
        public bool Statud { get; set; }

        [DisplayName("Cantidad Soportada")]
        [Required(ErrorMessage = "La cantidad soportada es requerido.")]
        public int AmountSupported { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }

        //Claves foreanas
        public int PlaceId { get; set; }
        //public Place Place { get; set; }

        //public List<Gallery> ListGalleries { get; set; }
    }

    public class ProductEditDto
    {

        public int ProductId { get; set; }
        [DisplayName("Nombre")]
        [Required(ErrorMessage = "El Nombre es requerido."), MaxLength(150)]
        [RegularExpression(@"^[-a-zA-Z0-9ñÑáéíóúáéíóúÁÉÍÓÚ ]{1,150}$", ErrorMessage = "No se permiten caracteres especiales.")]
        public string Name { get; set; }
        public string ProductUrl { get; set; }
        public IFormFile CoverPage { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int HighPrice { get; set; }
        public int HalfPrice { get; set; }
        public int LowPrice { get; set; }
        public int Discounts { get; set; }
        public int AmountSupported { get; set; }
        public int PersonNumber { get; set; }
        public IEnumerable<IFormFile> Gallery { get; set; }
        public bool Statud { get; set; }
        public DateTime UpdateDate { get; set; }

        //Claves foreanas
        public int PlaceId { get; set; }
        //public Place Place { get; set; }

        public ICollection<Gallery> Galleries { get; set; }
    }
}
