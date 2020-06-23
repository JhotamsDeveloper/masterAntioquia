using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.DTOs
{
    public class StoreDto
    {
        public int ProductId { get; set; }

        [DisplayName("Nombre")]
        [Required(ErrorMessage = "El Nombre es requerido."), MaxLength(150)]
        [RegularExpression(@"^[-a-zA-Z0-9ñÑáéíóúáéíóúÁÉÍÓÚ ]{1,150}$", ErrorMessage = "No se permiten caracteres especiales.")]
        public string Name { get; set; }
        public string ProductUrl { get; set; }
        public string CoverPage { get; set; }
        public string SquareCover { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public float Increments { get; set; }
        public float ShippingValue { get; set; }
        public int Discounts { get; set; }
        public bool Statud { get; set; }
        public int AmountSupported { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }

        //Claves foreanas
        public int PlaceId { get; set; }
        public Place Place { get; set; }

        public ICollection<Gallery> Galleries { get; set; }
    }

    public class StoreCreateDto
    {
        public int ProductId { get; set; }

        [DisplayName("Nombre")]
        [Required(ErrorMessage = "El Nombre es requerido."), MaxLength(150)]
        [RegularExpression(@"^[-a-zA-Z0-9ñÑáéíóúáéíóúÁÉÍÓÚ ]{1,150}$", ErrorMessage = "No se permiten caracteres especiales.")]
        public string Name { get; set; }
        public string ProductUrl { get; set; }
        public IFormFile CoverPage { get; set; }
        public IFormFile SquareCover { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public float Increments { get; set; }
        public float ShippingValue { get; set; }
        public IEnumerable<IFormFile> Gallery { get; set; }
        public int Discounts { get; set; }
        public bool Statud { get; set; }
        public int AmountSupported { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }

        //Claves foreanas
        public int PlaceId { get; set; }

    }
    public class StoreEditDto
    {
        public int ProductId { get; set; }

        [DisplayName("Nombre")]
        [Required(ErrorMessage = "El Nombre es requerido."), MaxLength(150)]
        [RegularExpression(@"^[-a-zA-Z0-9ñÑáéíóúáéíóúÁÉÍÓÚ ]{1,150}$", ErrorMessage = "No se permiten caracteres especiales.")]
        public string Name { get; set; }
        public string ProductUrl { get; set; }
        public IFormFile CoverPage { get; set; }
        public IFormFile SquareCover { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public float Increments { get; set; }
        public float ShippingValue { get; set; }
        public IEnumerable<IFormFile> Gallery { get; set; }
        public int Discounts { get; set; }
        public bool Statud { get; set; }
        public int AmountSupported { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }

        //Claves foreanas
        public int PlaceId { get; set; }
        public ICollection<Gallery> Galleries { get; set; }

    }
}
