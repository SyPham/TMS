using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModel
{
    public class TreeModel
    {
        public int ID { get; set; }
        public List<TreeModel> Children { get; set; }
    }
}
