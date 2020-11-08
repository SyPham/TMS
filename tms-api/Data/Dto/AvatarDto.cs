using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Dto
{
    public class AvatarDto
    {
        public int UserID { get; set; }
        public IFormFile Photo { get; set; }
    }
}
