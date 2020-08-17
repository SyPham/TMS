using Data.Models;
using Data.ViewModel.Notification;
using Service.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface INotificationService
    {
        Task<bool> Create(CreateNotifyParams entity);
        System.Threading.Tasks.Task CreateRange(List<CreateNotifyParams> entity);
        Task<bool> Update(Notification entity);
        Task<bool> Delete(int id);
        Task<Notification> GetByID(int id);
        Task<List<Notification>> GetAll();
        Task<PagedList<Notification>> GetAllPaging(int page, int pageSize);
        Task<bool> Seen(int id);
        Task<object> GetAllByUserID(int userid, int page, int pageSize);
        Task<object> GetNotificationByUser(int userid, int page, int pageSize);

    }
}
