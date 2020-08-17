using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModel.Comment
{
   public class TaskHasComment
    {
        public int TaskID { get; set; }
        public string TaskName { get; set; }
        public Data.Enum.JobType JobType { get; set; }
        public CommentTreeView CommentTreeViews { get; set; }
    }
}
