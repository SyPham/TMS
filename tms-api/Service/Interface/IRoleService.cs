using Data.Models;
using Service.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
   public interface IRoleService
    {
        Task<bool> Create(Role entity);
        Task<bool> Update(Role entity);
        Task<bool> Delete(int id);
        Task<Role> GetByID(int id);
        Task<List<Role>> GetAll();
        Task<PagedList<Role>> GetAllPaging(int page, int pageSize, string text);
    }
}
