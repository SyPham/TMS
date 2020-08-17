using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Service.Helpers;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implement
{
    public class JobTypeService : IJobTypeService
    {
        private readonly DataContext _context;
        public JobTypeService(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(JobType entity)
        {
            await _context.JobTypes.AddAsync(entity);

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;

            }
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _context.JobTypes.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            _context.JobTypes.Remove(entity);
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;

            }
        }

        public async Task<List<JobType>> GetAll()
        {
            return await _context.JobTypes.Where(x => !x.Name.ToLower().Contains("admin")).ToListAsync();
        }

        public async Task<PagedList<JobType>> GetAllPaging(int page, int pageSize)
        {
            var source = _context.JobTypes.Where(x => !x.Name.ToLower().Contains("admin")).AsQueryable();

            return await PagedList<JobType>.CreateAsync(source, page, pageSize);
        }

        public async Task<JobType> GetByID(int id)
        {
            return await _context.JobTypes.FindAsync(id);
        }

        public async Task<bool> Update(JobType entity)
        {
            var item = await _context.JobTypes.FindAsync(entity.ID);
            item.Name = entity.Name;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;

            }
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
    }
}
