using Microsoft.AspNetCore.Http;
using Model.DTOs.CustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.DTOs
{
    public class TouristExcursionsDto
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string ProductUrl { get; set; }
        public string CoverPage { get; set; }
        public string SquareCover { get; set; }
        public string Description { get; set; }
        public bool Statud { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public int? PlaceId { get; set; }
        public Place Place { get; set; }
        public string City { get; set; }
        public string Business { get; set; }
        public string Reference { get; set; }
        public Boolean State { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        public List<Gallery> Galleries { get; set; }
    }

    public class TouristExcursionsCreateDto
    {

        public int TouristExcursionsId { get; set; }
        public string Name { get; set; }
        public string ProductUrl { get; set; }
        [DisplayName("Portada")]
        [Required(ErrorMessage = "La portada es requerida.")]
        [ValidateExtensionImg(ErrorMessage = "Utilice archivos con extensiones JPG JPEG GIF PNG")]
        [ImageSizes(ErrorMessage = "El tamaño no debe de ser superior a 2mb")]
        public IFormFile CoverPage { get; set; }

        [DisplayName("Portada Cuadrada")]
        [Required(ErrorMessage = "La portada cuadrada es requerida.")]
        [ValidateExtensionImg(ErrorMessage = "Utilice archivos con extensiones JPG JPEG GIF PNG")]
        [ImageSizes(ErrorMessage = "El tamaño no debe de ser superior a 2mb")]
        public IFormFile SquareCover { get; set; }

        [DisplayName("Galeria de fotos")]
        public IEnumerable<IFormFile> Gallery { get; set; }
        public string Description { get; set; }
        public bool Statud { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public Boolean State { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        public int PlaceId { get; set; }
        public Place Place { get; set; }
        public string City { get; set; }
        public string Business { get; set; }
        public string Reference { get; set; }
        public List<Gallery> Galleries { get; set; }
    }

}
