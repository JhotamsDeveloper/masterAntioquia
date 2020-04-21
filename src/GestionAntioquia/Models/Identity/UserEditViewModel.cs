using Model.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAntioquia.Models
{
    public class UserEditViewModel
    {
        public UserEditViewModel()
        {
            Cleims = new List<string>();
            Roles = new List<string>();
        }

        public string Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public Boolean EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Birthday { get; set; }
        [Required]
        public string Country { get; set; }
        public List<string> Cleims { get; set; }
        public IList<string> Roles { get; set; }
        public string RolName { get; set; }
        public string RolId { get; set; }
    }
}
