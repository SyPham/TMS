using AutoMapper;
using Data;
using Data.Models;
using Data.ViewModel.Tutorial;
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
    public class TutorialService : ITutorialService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public TutorialService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<object> Create(Tutorial tutorial)
        {
            await _context.Tutorials.AddAsync(tutorial);
            return await Save();
        }

        public Task<object> CreateSub(Tutorial tutorial)
        {
            throw new NotImplementedException();
        }

        public async Task<object> Delete(int id)
        {
            if (!await _context.Tutorials.AnyAsync(x => x.ID.Equals(id)))
                return false;
            try
            {
                _context.Tutorials.Remove(await _context.Tutorials.FindAsync(id));
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {

                return false;
                throw;
            }
        }

        public async Task<Tutorial> FindItem(int id)
        {
            if (!await _context.Tutorials.AnyAsync(x => x.ID.Equals(id)))
                return new Tutorial();
            return await _context.Tutorials.FindAsync(id);
        }
        public Task<object> Rename()
        {
            throw new NotImplementedException();
        }

        public async Task<List<TreeViewTutorial>> GetListTree(int projectId)
        {
            var listLevels = await _context.Tutorials.Where(x => x.ProjectID.Equals(projectId)).OrderBy(x => x.Level).ToListAsync();
            var levels = new List<TreeViewTutorial>();
            foreach (var item in listLevels)
            {
                var levelItem = new TreeViewTutorial();
                levelItem.ID = item.ID;
                levelItem.Name = item.Name;
                levelItem.Level = item.Level;
                levelItem.ParentID = item.ParentID;
                levelItem.URL = item.URL;
                levelItem.Path = item.Path;
                levels.Add(levelItem);
            }

            List<TreeViewTutorial> hierarchy = new List<TreeViewTutorial>();

            hierarchy = levels.Where(c => c.ParentID == 0)
                            .Select(c => new TreeViewTutorial()
                            {
                                ID = c.ID,
                                Name = c.Name,
                                Level = c.Level,
                                ParentID = c.ParentID,
                                URL = c.URL,
                                Path = c.Path,
                                children = GetChildren(levels, c.ID)
                            })
                            .ToList();


            HieararchyWalk(hierarchy);

            return hierarchy;
        }
        private void HieararchyWalk(List<TreeViewTutorial> hierarchy)
        {
            if (hierarchy != null)
            {
                foreach (var item in hierarchy)
                {
                    //Console.WriteLine(string.Format("{0} {1}", item.Id, item.Text));
                    HieararchyWalk(item.children);
                }
            }
        }
        public List<TreeViewTutorial> GetChildren(List<TreeViewTutorial> levels, int parentid)
        {
            return levels
                    .Where(c => c.ParentID == parentid)
                    .Select(c => new TreeViewTutorial()
                    {
                        ID = c.ID,
                        Name = c.Name,
                        Level = c.Level,
                        ParentID = c.ParentID,
                        URL = c.URL,
                        Path = c.Path,
                        children = GetChildren(levels, c.ID)
                    })
                    .ToList();
        }

        public async Task<bool> Save()
        {
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Edit(Tutorial tutorial)
        {
            var item = await FindItem(tutorial.ID);
            if (!tutorial.URL.IsNullOrEmpty())
            {
                item.URL = tutorial.URL;
            }
            item.Name = tutorial.Name;
            item.Path = tutorial.Path;
            return await Save();
        }
    }
}
