using Data.Models;
using Data.ViewModel.UserSytem;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
   public interface IUserSystemService
    {
        Task<bool> MapUserSystem(UserSystemViewModel entity);
        Task<object> GetAllUserBySystem(int systemID);
        Task<List<SystemCode>> GetAllSystem();
        Task<bool> AddSystem(SystemCode entity);
        Task<bool> DeleteSystem(int systemID);
        Task<bool> UpdateSystem(SystemCode System);
    }
}
