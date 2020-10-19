using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;
using Data.ViewModel.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Dto;
using Service.Helpers;
using Service.Interface;
using WorkManagement.Helpers;

namespace WorkManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        private IHostingEnvironment _currentEnvironment;
        private IMailExtension _mailExtension;
        public UsersController(IUserService userService, IMailExtension mailExtension,IHostingEnvironment currentEnvironment)
        {
            _userService = userService;
            _currentEnvironment = currentEnvironment;
            _mailExtension = mailExtension;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _userService.GetAll());
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetListUser()
        {
            return Ok(await _userService.GetListUser());
        }
        [HttpGet("/GetUser/{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            return Ok(await _userService.GetByID(id));
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserViewModel user)
        {
            //string uniqueFileName = null;
            //if (user.Photo != null)
            //{
            //    string uploadFolder = Path.Combine(_currentEnvironment.WebRootPath,"images");
            //    uniqueFileName = Guid.NewGuid().ToString() + '_' + user.Photo.FileName;
            //   var filePath = Path.Combine(uploadFolder,uniqueFileName);
            //    user.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
            //    user.ImageUrl = "/images/"+ uniqueFileName;
            //}
            return Ok(await _userService.Create(user));
        }

        [HttpPost]
        public async Task<IActionResult> ChangeAvatar([FromForm]IFormFile formFile)
        {
            IFormFile file = Request.Form.Files["UploadedFile"];
            string token = Request.Headers["Authorization"];
            var userID = JWTExtensions.GetDecodeTokenByProperty(token, "nameid").ToInt();
            string uniqueFileName = null;
            if (file != null)
            {
                string uploadFolder = Path.Combine(_currentEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + '_' + file.FileName;
                var filePath = Path.Combine(uploadFolder, uniqueFileName);
                file.CopyTo(new FileStream(filePath, FileMode.Create));
            }
            return Ok(await _userService.ChangeAvatar(userID, uniqueFileName));
        }
        
         [HttpPut("{userId}/{password}")]
        public async Task<IActionResult> ChangePassword(int userId, string password)
        {
            return Ok(await _userService.ChangePassword(userId, password));
        }
        [HttpPost]
        public async Task<IActionResult> ChangedAvatar(UploadDto img)
        {
            return Ok(await _userService.ChangeAvatar(img.UserID, img.Imagebase64));
        }
        [HttpGet("{userid}/{token}")]
        public async Task<IActionResult> UpdateTokenLineForUser(string token, int userid)
        {
            return Ok(await _userService.UpdateTokenLineForUser(token, userid));
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateUserDto user )
        {
            return Ok(await _userService.Update(user));
        }

        [HttpPost]
        public async Task<IActionResult> UploapProfile(IFormFile formFile)
        {
            string token = Request.Headers["Authorization"];
            var userID = JWTExtensions.GetDecodeTokenByProperty(token, "nameid").ToInt();
            byte[] image = null;
            IFormFile file = Request.Form.Files["UploadedFile"];
            if ((file != null) && (file.Length > 0) && !string.IsNullOrEmpty(file.FileName))
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    image = stream.ToArray();
                };
            }
            return Ok(await _userService.UploapProfile(userID,image));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveTokenLineForUser(int id)
        {
            return Ok(await _userService.RemoveTokenLineForUser(id));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _userService.Delete(id));
        }
        [HttpGet("{page}/{pageSize}/{keyword}")]
        [HttpGet("{page}/{pageSize}")]

        public async Task<ActionResult> GetAllPaging(int page, int pageSize, string keyword = "")
        {
            var model = await _userService.GetAllPaging(page, pageSize, keyword);
            Response.AddPagination(model.CurrentPage, model.PageSize, model.TotalCount, model.TotalPages);
            return Ok(new
            {
                data = model,
                total = model.TotalPages,
                page,
                pageSize
            });
        }
        [HttpGet("{systemCode}/{page}/{pageSize}")]
        [HttpGet("{systemCode}/{page}/{pageSize}/{keyword}")]
        public async Task<ActionResult> GetAllUsers(int systemCode, int page, int pageSize, string keyword = "")
        {
            var model = await _userService.GetAllPaging(systemCode, page, pageSize, keyword);
            Response.AddPagination(model.CurrentPage, model.PageSize, model.TotalCount, model.TotalPages);
            return Ok(model);
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult> ResetPassword(int id)
        {
            var model = await _userService.ResetPassword(id);
            await _mailExtension.SendEmailAsync(model.Email, "Reset Password", $"Password: {model.Password}");
            return Ok(model.Status);
        }
        [HttpPost]
        public async Task<ActionResult> ResetPassword(UserResetPasswordVM user)
        {
            var model = await _userService.ResetPassword(user);
            await _mailExtension.SendEmailAsync(model.Email, "Reset Password", $"Password: {model.Password}");
            return Ok(model.Status);
        }
        
        [HttpGet]
        public async Task<ActionResult> GetUsernames()
        {
            var model = await _userService.GetUsernames();
            return Ok(model);
        }
    }
}