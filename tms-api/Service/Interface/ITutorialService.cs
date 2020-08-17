using AutoMapper;
using Data;
using Data.Models;
using Data.ViewModel.Task;
using Data.ViewModel.Tutorial;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
   public interface ITutorialService
    {
        Task<List<TreeViewTutorial>> GetListTree(int projectId);
        Task<object> Create(Tutorial tutorial);
        Task<object> CreateSub(Tutorial tutorial);
        Task<object> Delete(int id);
        Task<object> Rename();
        Task<bool> Edit(Tutorial tutorial);

        Task<Tutorial> FindItem(int id);
        Task<bool> Save();
    }
}
