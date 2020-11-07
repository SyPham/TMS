using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Data.Models;
using Service.Interface;
using TMS.Helpers;
using Data.ViewModel.UserSytem;

namespace TMS.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserSystemController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserSystemService _userSystemService;

        public UserSystemController(DataContext context, IUserSystemService userSystemService)
        {
            _context = context;
            _userSystemService = userSystemService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllSystem()
        {
            var model = await _userSystemService.GetAllSystem();
            return Ok(model); 
        }
        [HttpGet("{systemID}")]
        public async Task<ActionResult> GetAllUserBySystem(int systemID)
        {
            var model = await _userSystemService.GetAllUserBySystem( systemID);
            return Ok(model);
        }
        
      
        [HttpPost]
        public async Task<IActionResult> MapUserSystem(UserSystemViewModel entity)
        {
            return Ok(await _userSystemService.MapUserSystem(entity));
        }

       
        [HttpPost]
        public async Task<ActionResult<Role>> CreateSystem(SystemCode entity)
        {
            return Ok(await _userSystemService.AddSystem(entity));
        }
        [HttpPut]
        public async Task<ActionResult<Role>> UpdateSystem(SystemCode entity)
        {
            return Ok(await _userSystemService.UpdateSystem(entity));
        }

        [HttpDelete("{systemID}")]
        public async Task<ActionResult<Role>> DeleteSystem(int systemID)
        {
            return Ok(await _userSystemService.DeleteSystem(systemID));
        }

      
    }
}
