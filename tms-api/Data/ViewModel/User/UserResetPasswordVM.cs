using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModel.User
{
   public class UserResetPasswordVM
    {
        public bool Status { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UsernameOrEmail { get; set; }
    }
}
