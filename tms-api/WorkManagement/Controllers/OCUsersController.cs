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
using Data.ViewModel.OC;
using WorkManagement.Helpers;

namespace WorkManagement.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OCUsersController : ControllerBase
    {
        private readonly IOCUserService _ocUserService;

        public OCUsersController(IOCUserService ocUserService)
        {
            _ocUserService = ocUserService;
        }

        [HttpGet("{ocid}")]
        public async Task<ActionResult> GetListUser(int ocid)
        {
            return Ok(await _ocUserService.GetListUser(ocid));
        }
        [HttpGet("{page}/{pageSize}/{ocid}")]
        [HttpGet("{page}/{pageSize}/{ocid}/{text}")]
        public async Task<ActionResult> GetUsers(int page, int pageSize,int ocid, string text)
        {
            var lists = await _ocUserService.GetListUser(page, pageSize, ocid, text);
            Response.AddPagination(lists.CurrentPage, lists.PageSize, lists.TotalCount, lists.TotalPages);

            return Ok(lists);
        }
        [HttpGet("{userid}/{ocid}/{status}")]
        public async Task<ActionResult> AddOrUpdate(int userid, int ocid, bool status)
        {
            return Ok( await _ocUserService.AddOrUpdate(userid,ocid, status));
        }
    }
}
