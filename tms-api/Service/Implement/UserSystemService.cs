using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data;
using Data.Models;
using Data.ViewModel.User;
using Data.ViewModel.UserSytem;
using Microsoft.EntityFrameworkCore;
using Service.Helpers;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implement
{
    public class UserSystemService : IUserSystemService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserSystemService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AddSystem(SystemCode entity)
        {
            await _context.SystemCodes.AddAsync(entity);

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

        public async Task<bool> DeleteSystem(int systemID)
        {
          var item =  await _context.SystemCodes.FirstOrDefaultAsync(x=> x.ID == systemID);
            try
            {
                _context.Remove(item);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;

            }
        }

        public async Task<List<SystemCode>> GetAllSystem() 
            => await _context.SystemCodes.ToListAsync();

        public async Task<object> GetAllUserBySystem(int systemID)
        {
            var model = from a in _context.Users.Include(x=>x.UserSystems)
                        select new
                        {
                            a.ID,
                            a.Username,
                            a.Email,
                            a.EmployeeID,
                            a.isLeader,
                            a.OCID,
                            a.LevelOC,
                            a.DeleteBy,
                            a.ModifyTime,
                            Status = a.UserSystems.Any(x=> x.Status == true && x.SystemID == systemID)
                        };
            
            return await model.ToListAsync();
        }
           
        public async Task<bool> MapUserSystem(UserSystemViewModel entity)
        {
            var item = await _context.UserSystems.FirstOrDefaultAsync(x => x.SystemID == entity.SystemID && x.UserID == entity.UserID);
            try
            {
                if (item == null)
                {
                    _context.Add(new UserSystem
                    {
                        UserID =entity.UserID,
                        SystemID = entity.SystemID,
                        Status = true,
                        DateTime = DateTime.UtcNow
                    });
                    await _context.SaveChangesAsync();
                    return true;
                } else
                {
                    item.Status = !item.Status;
                    item.DateTime = DateTime.UtcNow;
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;

            }
        }

        public async Task<bool> UpdateSystem(SystemCode entity)
        {
            try
            {
                var item = await _context.SystemCodes.AnyAsync(x => x.ID == entity.ID);
                if (item == false) return false;
                _context.SystemCodes.Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;

            }
        }
    }
}
