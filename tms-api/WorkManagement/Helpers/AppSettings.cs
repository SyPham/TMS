using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkManagement.Helpers
{
    public class AppSettings
    {
        public string URL { get; set; }
        public string Token { get; set; }
        public string applicationUrl { get; set; }
        public string [] CorsPolicy { get; set; }
    }
}
