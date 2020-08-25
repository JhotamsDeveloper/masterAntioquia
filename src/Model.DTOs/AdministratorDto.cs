using System.ComponentModel.DataAnnotations;

namespace Model.DTOs
{
    public class AdministratorDto
    {
        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }
    }

}
