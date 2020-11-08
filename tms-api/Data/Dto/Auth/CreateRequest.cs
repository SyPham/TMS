using Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Dto.Auth
{
    public class CreateRequest
    {
        [Required]
        public string EmployeeID { get; set; }
        public string Username { get; set; }
        public byte[] ImageBase64 { get; set; }
       
        public string ImageURL { get; set; }
        [Required]
        [EnumDataType(typeof(Role))]
        public string Role { get; set; }
        public bool IsShow { get; set; } = true;
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
