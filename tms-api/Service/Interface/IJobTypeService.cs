using Data.Models;
using Service.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
   public interface IJobTypeService
    {
        Task<bool> Create(JobType entity);
        Task<bool> Update(JobType entity);
        Task<bool> Delete(int id);
        Task<JobType> GetByID(int id);
        Task<List<JobType>> GetAll();
        Task<PagedList<JobType>> GetAllPaging(int page, int pageSize);
    }
}
