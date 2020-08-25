using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.DTOs
{

    public class ReviewDto
    {
        public int ReviewID { get; set; }
        public string TittleReview { get; set; }
        public string Description { get; set; }
        public int Assessment { get; set; }
        public string UserId { get; set; }

        public int? PlaceId { get; set; }
        public Place Place { get; set; }

        public List<Gallery> Galleries { get; set; }
    }

    public class ReviewsCreateDto
    {
        public int ReviewID { get; set; }

        [DisplayName("Titulo")]
        [Required(ErrorMessage = "El tipo de titulo es requerido."), MaxLength(50)]
        public string TittleReview { get; set; }

        [DisplayName("Descripción")]
        [Required(ErrorMessage = "El tipo descripción es requerido."), MaxLength(255)]
        public string DescriptionReview { get; set; }

        [DisplayName("Rango")]
        [Required(ErrorMessage = "El tipo rango es requerido.")]
        [Range(1, 5, ErrorMessage = "Rango entre {1} y {2}")]
        public int AssessmentReview { get; set; }
        public string UserIdReview { get; set; }
        public IEnumerable<IFormFile> GalleryReview { get; set; }
    }

    public class ReviewsGetDto
    {
        public int TotalReviews { get; set; }
        public int TotalReviewsUser { get; set; }
        public string TittleReview { get; set; }
        public string TitleReview { get; set; }
        public string Description { get; set; }
        public int Assessment { get; set; }
        public string UserId { get; set; }
        public List<Gallery> Galleries { get; set; }
    }
}
