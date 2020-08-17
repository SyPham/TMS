using Data.Models;
using Service.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface INotificationDetailService
    {
        Task<bool> Create(NotificationDetail entity);
        Task<bool> Update(NotificationDetail entity);
        Task<bool> Delete(int id);
        Task<NotificationDetail> GetByID(int id);
        Task<List<NotificationDetail>> GetAll();
        Task<PagedList<NotificationDetail>> GetAllPaging(int page, int pageSize);
    }
}
