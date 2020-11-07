using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data
{
    public class VerifyEmailRequest
    {
        [Required]
        public string Token { get; set; }
    }
}
