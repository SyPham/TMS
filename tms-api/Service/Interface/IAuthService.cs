using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
   public interface IAuthService
    {
        Task<User> Login(string username, string password);
        Task<User> Register(User user, string password);
        Task<User> FindByNameAsync(string username);
        Task<User> GetById(int Id);
        Task<Role> GetRolesAsync(int role);
        Task<bool> SignInHistory(SignInHistory signInHistory);
        Task<object> GetSignInHistory();
        Task<User> Edit(string username);
    }
}
