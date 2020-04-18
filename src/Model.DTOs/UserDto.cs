using Microsoft.AspNetCore.Identity;
using Model.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DTOs
{
    public class UserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public Boolean EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Birthday { get; set; }
        public string Country { get; set; }
        public ICollection<ApplicationRole> Roles { get; set; }
    }

    public class UserRegistreDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public Boolean EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Birthday { get; set; }
        public string Country { get; set; }
        public ICollection<ApplicationRole> Roles { get; set; }
    }
}
