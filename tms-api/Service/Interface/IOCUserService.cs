using Service.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
   public interface IOCUserService
    {
        Task<object> GetListUser(int ocid);
        Task<PagedList<Data.ViewModel.OCUser.User>> GetListUser(int page = 1, int pageSize = 10, int ocid = 0, string text = "");
        Task<object> AddOrUpdate(int userid, int ocid, bool status);

    }
}
