using AutoMapper;
using Data;
using Data.ViewModel.Task;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
   public interface IFollowService
    {

        Task<object> Unsubscribe(int id, int userid);
    }
}
