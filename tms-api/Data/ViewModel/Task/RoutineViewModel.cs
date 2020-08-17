using Data.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModel.Task
{
  public class RoutineViewModel
    {
        public string JobName { get; set; }
        public string From { get; set; }
        public string TaskCode { get; set; }
        public List<HierarchyNode<TreeViewTask>> Tasks { get; set; }
    }
}
