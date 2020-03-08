using Microsoft.AspNetCore.Http;
using Model.DTOs.CustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Model.DTOs
{
    public class PlaceDto
    {
        public int PlaceId { get; set; }

        [Required(ErrorMessage = "El Nit es requerido."),
            MaxLength(15),
            MinLength(10, ErrorMessage = "Cantidad Mínima de 10.")]
        public string Nit { get; set; }

        [DisplayName("Nombre")]
        [Required(ErrorMessage = "El Nombre es requerido."), MaxLength(10)]
        public string Name { get; set; }

        [DisplayName("Télefono")]
        [Required(ErrorMessage = "El télefono es requerido."), MaxLength(10)]
        [Phone(ErrorMessage = "Formato no válido")]
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

        //[DisplayName("Portada")]
        //[Required(ErrorMessage = "La portada es requerida.")]
        //[ValidateExtensionImg(ErrorMessage = "Utilice archivos con extensiones JPG JPEG GIF PNG")]
        //[ImageSizes(ErrorMessage = "El tamaño no debe de ser superior a 2mb")]
        public string CoverPage { get; set; }

        //[DisplayName("Logo")]
        //[Required(ErrorMessage = "La logo es requerida.")]
        //[ValidateExtensionImg(ErrorMessage = "Utilice archivos con extensiones JPG JPEG GIF PNG")]
        //[ImageSizes(ErrorMessage = "El tamaño no debe de ser superior a 2mb")]
        public string Logo { get; set; }

        [DisplayName("Contracto")]
        [Required(ErrorMessage = "El tipo de contracto es requerido."), MaxLength(10)]
        public string Contract { get; set; }

        [DisplayName("Activar")]
        public Boolean State { get; set; }
        public string CreationDate { get; set; }
        public string UpdateDate { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
        public List<Product> Products { get; set; }
        public UploadImage image { get; set; }
    }

    public class UploadImage
    {
        public IFormFile CoverPageImg { get; set; }
        public IFormFile LogoImg { get; set; }

    }
    public class PlaceCreateDto
    {

        [Required(ErrorMessage = "El Nit es requerido."),
            MaxLength(15),
            MinLength(10, ErrorMessage = "Cantidad Mínima de 10.")]
        public string Nit { get; set; }
        [DisplayName("Nombre")]
        [Required(ErrorMessage = "El Nombre es requerido."), MaxLength(10)]
        public string Name { get; set; }

        [DisplayName("Télefono")]
        [Required(ErrorMessage = "El télefono es requerido."), MaxLength(10)]
        [Phone(ErrorMessage ="Formato no válido")]
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
        [ValidateExtensionImg(ErrorMessage = "Utilice archivos con extensiones JPG JPEG GIF PNG")]
        [ImageSizes(ErrorMessage = "El tamaño no debe de ser superior a 2mb")]
        public IFormFile CoverPage { get; set; }

        [DisplayName("Logo")]
        [Required(ErrorMessage = "La logo es requerida.")]
        [ValidateExtensionImg(ErrorMessage = "Utilice archivos con extensiones JPG JPEG GIF PNG")]
        [ImageSizes(ErrorMessage = "El tamaño no debe de ser superior a 2mb")]
        public IFormFile Logo { get; set; }

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

        [Required(ErrorMessage = "El Nit es requerido."),
            MaxLength(15),
            MinLength(10, ErrorMessage = "Cantidad Mínima de 10.")]
        public string Nit { get; set; }

        [DisplayName("Nombre")]
        [Required(ErrorMessage = "El Nombre es requerido."), MaxLength(10)]
        public string Name { get; set; }

        [DisplayName("Télefono")]
        [Required(ErrorMessage = "El télefono es requerido."), MaxLength(10)]
        [Phone(ErrorMessage = "Formato no válido")]
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
        [ValidateExtensionImg(ErrorMessage = "Utilice archivos con extensiones JPG JPEG GIF PNG")]
        [ImageSizes(ErrorMessage = "El tamaño no debe de ser superior a 2mb")]
        public IFormFile CoverPage { get; set; }

        [DisplayName("Logo")]
        [Required(ErrorMessage = "La logo es requerida.")]
        [ValidateExtensionImg(ErrorMessage = "Utilice archivos con extensiones JPG JPEG GIF PNG")]
        [ImageSizes(ErrorMessage = "El tamaño no debe de ser superior a 2mb")]
        public IFormFile Logo { get; set; }

        [DisplayName("Contracto")]
        [Required(ErrorMessage = "El tipo de contracto es requerido."), MaxLength(10)]
        public string Contract { get; set; }

        [DisplayName("Activar")]
        public Boolean State { get; set; }
        public string UpdateDate { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
        public List<Product> Products { get; set; }
    }

}
