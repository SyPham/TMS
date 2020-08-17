using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModel.OCUser
{
   public class User
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string RoleName { get; set; }
        public int RoleID { get; set; }
        public bool Status { get; set; }
    }
}
