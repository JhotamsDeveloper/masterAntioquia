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
}
