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
   public class NotificationDetailService : INotificationDetailService
    {
        private readonly DataContext _context;
        public NotificationDetailService(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(NotificationDetail entity)
        {
            await _context.NotificationDetails.AddAsync(entity);

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
            var entity = await _context.NotificationDetails.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            _context.NotificationDetails.Remove(entity);
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

        public async Task<List<NotificationDetail>> GetAll()
        {
            return await _context.NotificationDetails.ToListAsync();
        }

        public async Task<PagedList<NotificationDetail>> GetAllPaging(int page, int pageSize)
        {
            var source = _context.NotificationDetails.AsQueryable();

            return await PagedList<NotificationDetail>.CreateAsync(source, page, pageSize);
        }

        public async Task<NotificationDetail> GetByID(int id)
        {
            return await _context.NotificationDetails.FindAsync(id);
        }

        public async Task<bool> Update(NotificationDetail entity)
        {
            var item = await _context.NotificationDetails.FindAsync(entity.ID);
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
