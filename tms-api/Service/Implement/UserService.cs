using AutoMapper;
using Data;
using Data.Models;
using Data.ViewModel.User;
using Microsoft.EntityFrameworkCore;
using Service.Dto;
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
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Create(UserViewModel entity)
        {
            var item = await _context.Users.FirstOrDefaultAsync(x => x.EmployeeID.ToLower().Equals(entity.Username.ToLower()));
            if (item == null)
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(entity.Password, out passwordHash, out passwordSalt);
                var user = _mapper.Map<User>(entity);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.ModifyTime = DateTime.Now;
                user.IsShow = true;
                await _context.Users.AddAsync(user);
                try
                {
                    await _context.SaveChangesAsync();
                    _context.UserSystems.Add(new UserSystem
                    {
                        UserID = user.ID,
                        SystemID = entity.SystemCode,
                        Status = true,
                        DateTime = DateTime.UtcNow
                    });
                    await _context.SaveChangesAsync();
                    return user.ID;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            else
            {
                var userSystem = await _context.UserSystems.FirstOrDefaultAsync(x => x.UserID == item.ID && x.SystemID == entity.SystemCode);
                if (userSystem == null)
                {
                    _context.UserSystems.Add(new UserSystem
                    {
                        UserID = item.ID,
                        SystemID = entity.SystemCode,
                        Status = true,
                        DateTime = DateTime.UtcNow
                    });
                    await _context.SaveChangesAsync();
                } else
                {
                    _context.Remove(userSystem);
                   await _context.SaveChangesAsync();

                }
                return item.ID;
            }

        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
        public async Task<bool> ChangeAvatar(int id, string imagePath)
        {
            try
            {
                var item = await _context.Users.FindAsync(id);
                item.ImageBase64 = Convert.FromBase64String(imagePath);
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
            var entity = await _context.Users.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            try
            {
                entity.IsShow = false;
                _context.Users.Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;

            }
        }
        public async Task<bool> UploapProfile(int id, byte[] image)
        {
            var entity = await _context.Users.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            try
            {
                entity.ImageBase64 = image;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;

            }
        }

        public async Task<List<User>> GetAll()
        {
            return await _context.Users.Where(x => x.IsShow == true && x.Username != "admin").ToListAsync();
        }

        public async Task<PagedList<ListViewModel>> GetAllPaging(int page, int pageSize, string keyword)
        {
            var source = _context.Users.Include(x => x.UserSystems).Where(x => x.IsShow == true && x.Username != "admin").OrderByDescending(x => x.ID).Select(x => new ListViewModel { isLeader = x.isLeader, ID = x.ID, Username = x.Username, Email = x.Email, RoleName = x.Role.Name, RoleID = x.RoleID, EmployeeID = x.EmployeeID }).AsQueryable();
            if (!keyword.IsNullOrEmpty())
            {
                source = source.Where(x => x.Username.Contains(keyword) || x.Email.Contains(keyword));
            }
            return await PagedList<ListViewModel>.CreateAsync(source, page, pageSize);
        }

        public async Task<User> GetByID(int id)
        {
            return await _context.Users.Include(x => x.UserSystems).FirstOrDefaultAsync(x => x.ID == id);
        }

        public async Task<object> GetListUser()
        {
            return await _context.Users.Where(x => x.IsShow == true && x.Username != "admin").Select(x => new { x.ID, x.Username, x.Email, RoleName = x.Role.Name, x.RoleID }).ToListAsync();

        }

        public async Task<bool> Update(User entity)
        {
            var item = await _context.Users.FindAsync(entity.ID);
            item.Username = entity.Username;
            item.Email = entity.Email;
            item.ModifyTime = DateTime.Now;

            item.RoleID = entity.RoleID;
            item.isLeader = entity.isLeader;
            if (item.PasswordHash.IsNullOrEmpty() && item.PasswordSalt.IsNullOrEmpty())
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash("1", out passwordHash, out passwordSalt);

                item.PasswordHash = passwordHash;
                item.PasswordSalt = passwordSalt;
            }
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
        public async Task<bool> Update(UpdateUserDto entity)
        {
            var item = await _context.Users.FindAsync(entity.ID);
            item.EmployeeID = entity.EmployeeID;
            item.Username = entity.Username;
            item.Email = entity.Email;
            item.ModifyTime = DateTime.Now;
            if (!entity.Password.IsNullOrEmpty())
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(entity.Password, out passwordHash, out passwordSalt);
                item.PasswordHash = passwordHash;
                item.PasswordSalt = passwordSalt;
            }

            item.ModifyTime = DateTime.Now;

            try
            {
                _context.Users.Update(item);
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

        public async Task<List<string>> GetUsernames()
        {
            return await _context.Users.Where(x => x.IsShow == true && x.Username != "admin").Select(x => x.Username).ToListAsync();

        }

        public async Task<UserResetPasswordVM> ResetPassword(int id)
        {
            byte[] passwordHash, passwordSalt;
            var item = await _context.Users.FindAsync(id);
            item.ModifyTime = DateTime.Now;
            CreatePasswordHash("1", out passwordHash, out passwordSalt);
            try
            {
                item.PasswordHash = passwordHash;
                item.PasswordSalt = passwordSalt;
                await _context.SaveChangesAsync();
                var user = new UserViewModel();
                user.Email = item.Email;
                user.Username = item.Username;
                user.Username = item.Username;
                return new UserResetPasswordVM { Status = true };
            }
            catch (Exception)
            {
                return new UserResetPasswordVM();
                throw;
            }
        }
        public async Task<UserResetPasswordVM> ResetPassword(UserResetPasswordVM user)
        {
            byte[] passwordHash, passwordSalt;
            var item = await _context.Users.FirstOrDefaultAsync(x => x.Username.Equals(user.UsernameOrEmail) || x.Email.Equals(user.UsernameOrEmail));
            item.ModifyTime = DateTime.Now;
            CreatePasswordHash("1", out passwordHash, out passwordSalt);
            try
            {
                item.PasswordHash = passwordHash;
                item.PasswordSalt = passwordSalt;
                await _context.SaveChangesAsync();
                user.Status = true;
                return user;
            }
            catch (Exception)
            {
                user.Status = false;
                return user;
                throw;
            }
        }
        public async Task<bool> UpdateTokenLineForUser(string token, int userID)
        {
            var item = await _context.Users.FindAsync(userID);
            item.AccessTokenLineNotify = token;
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
        public async Task<bool> RemoveTokenLineForUser(int userID)
        {
            var item = await _context.Users.FindAsync(userID);
            item.AccessTokenLineNotify = null;
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

        public async Task<PagedList<ListViewModel>> GetAllPaging(int systemCode, int page, int pageSize, string keyword)
        {
            var source = _context.Users.Include(x => x.UserSystems).Where(x => x.IsShow == true && x.Username != "admin" && x.UserSystems.Where(x => x.Status == true).Select(x => x.SystemID).Contains(systemCode)).OrderByDescending(x => x.ID).Select(x => new ListViewModel { isLeader = x.isLeader, ID = x.ID, Username = x.Username, Email = x.Email, RoleName = x.Role.Name, RoleID = x.RoleID, EmployeeID = x.EmployeeID }).AsQueryable();
            if (!keyword.IsNullOrEmpty())
            {
                source = source.Where(x => x.Username.Contains(keyword) || x.Email.Contains(keyword));
            }
            return await PagedList<ListViewModel>.CreateAsync(source, page, pageSize);
        }

        public async Task<bool> ChangePassword(int userId, string password)
        {
            byte[] passwordHash, passwordSalt;
            var item = await _context.Users.FindAsync(userId);
            string pass = password;
            item.ModifyTime = DateTime.Now;

            CreatePasswordHash(pass, out passwordHash, out passwordSalt);
            try
            {
                item.PasswordHash = passwordHash;
                item.PasswordSalt = passwordSalt;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string EmployeeID { get; set; }
        public string Email { get; set; }
        public int SystemID { get; set; }
        public int UserRoleID { get; set; }
        public string Role { get; set; }
        public string Building { get; set; }
        public int BuildingUserID { get; set; }
        public bool IsLock { get; set; }


        public async Task<List<User>> GetUserBySystemID(int systemID)
        {
            var model = from a in _context.Users.Where(x => x.IsShow)
                        join b in _context.UserSystems on a.ID equals b.UserID
                        where b.SystemID == systemID
                        select new User
                        {
                            ID = a.ID,
                            Username = a.Username,
                            Email = a.Email,
                            EmployeeID = a.EmployeeID,
                        };
            var data = await model.ToListAsync();
            return data;

        }

        public async Task<bool> Delete(int id, int systemCodeID)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(x => x.ID == id);
            if (entity == null)
            {
                return false;
            }

            try
            {

                var item = await _context.UserSystems.FirstOrDefaultAsync(x => x.SystemID == systemCodeID && x.UserID == id);
                if (item == null)
                {
                    return true;
                }
                _context.UserSystems.Remove(item);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;

            }
        }
    }
}
