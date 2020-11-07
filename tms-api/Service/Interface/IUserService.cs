using Data.Models;
using Data.ViewModel.User;
using Service.Dto;
using Service.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
   public interface IUserService
    {
        Task<int> Create(UserViewModel entity);
        Task<bool> UpdateTokenLineForUser(string token, int userID);
        Task<bool> Update(User entity);
        Task<bool> Update(UpdateUserDto entity); 
        Task<bool> Delete(int id);
        Task<bool> Delete(int id, int systemCodeID);
        Task<UserResetPasswordVM> ResetPassword(int id);
        Task<UserResetPasswordVM> ResetPassword(UserResetPasswordVM user);
        Task<bool> ChangePassword(int userId, string password);
        Task<User> GetByID(int id);
        Task<List<User>> GetAll();
        Task<List<string>> GetUsernames();
        Task<bool> UploapProfile(int id, byte[] image);
        Task<object> GetListUser();
        Task<List<User>> GetUserBySystemID(int systemID);
        Task<bool> ChangeAvatar(int userid, string imagePath);
        Task<PagedList<ListViewModel>> GetAllPaging(int page, int pageSize, string keyword);
        Task<PagedList<ListViewModel>> GetAllPaging(int systemCode, int page, int pageSize, string keyword);
        Task<bool> RemoveTokenLineForUser(int userID);
    }
}
