using Data;
using Data.Dto.Auth;
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
        AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress);
        AuthenticateResponse RefreshToken(string token, string ipAddress);
        bool RevokeToken(string token, string ipAddress);

        void Register(RegisterRequest model, string origin);
        void VerifyEmail(string token);
        void ForgotPassword(ForgotPasswordRequest model, string origin);
        void ValidateResetToken(ValidateResetTokenRequest model);
        void ResetPassword(ResetPasswordRequest model);
    }
}
