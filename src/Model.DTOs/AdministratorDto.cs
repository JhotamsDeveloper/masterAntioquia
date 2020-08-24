using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.DTOs
{
    public class AdministratorDto
    {
        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }
    }

    public class ReviewsGetDto
    {
        public int TotalReviews { get; set; }
        public int TotalReviewsUser { get; set; }
        public string NameUser { get; set; }
        public string TitleReview { get; set; }
        public string Description { get; set; }
        public int Assessment { get; set; }
        public string imgUser { get; set; }
        public List<Gallery> Products { get; set; }
    }

}
