using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.Models.Shared
{
    public class TreeNodeModel
    {
        public TreeNodeModel()
        {
        }

        public string id { get; set; }
        public string text { get; set; }
        public object data { get; set; }
        public string icon { get; set; }
        public string expandedIcon { get; set; }
        public string collapsedIcon { get; set; }
        public bool? leaf { get; set; }
        public string style { get; set; }
        public string styleClass { get; set; }
        public bool? expanded { get; set; }
        public string type { get; set; }
        public bool? draggable { get; set; }
        public bool? droppable { get; set; }
        public bool? selectable { get; set; }

        public string emptyMessage { get; set; }
        public TreeNodeModel[] children { get; set; }
        public JstreeStateModel State { get; set; }
        public object attr { get; set; }
    }
}
