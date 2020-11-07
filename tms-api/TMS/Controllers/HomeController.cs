using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.Helpers;
using Service.Interface;
using TMS.Helpers;
using static System.Net.Mime.MediaTypeNames;
using AuthorizeAttribute = TMS.Helpers.AuthorizeAttribute;

namespace TMS.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class HomeController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public HomeController(INotificationService notificationService)
        {

            _notificationService = notificationService;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Seen(int id)
        {

            return Ok(await _notificationService.Seen(id));
        }
        
        [HttpGet("{page}/{pageSize}/{userid}")]
        [HttpGet("{page}/{pageSize}")]
        public async Task<IActionResult> GetAllNotificationCurrentUser(int page, int pageSize, int userid)
        {
            string token = Request.Headers["Authorization"];
            var userID = JWTExtensions.GetDecodeTokenByProperty(token, "nameid").ToInt();
            if (userid > 0)
                userID = userid;
            return Ok(await _notificationService.GetAllByUserID(userID, page, pageSize));
        }
        [HttpGet("{page}/{pageSize}")]
        [HttpGet("{page}/{pageSize}/{userid}")]
        public async Task<IActionResult> GetNotificationByUser(int page, int pageSize, int userid)
        {
            string token = Request.Headers["Authorization"];
            var userID = JWTExtensions.GetDecodeTokenByProperty(token, "nameid").ToInt();
            if (userid > 0)
                userID = userid;
            return Ok(await _notificationService.GetNotificationByUser(userID, page, pageSize));
        }
        
    }
}