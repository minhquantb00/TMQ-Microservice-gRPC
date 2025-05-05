using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.Models.Shared
{
    public class JsTreeModel
    {
        public JsTreeModel()
        {
            State = new JstreeStateModel();
        }

        public string Id { get; set; }
        public string Parent { get; set; }
        public string Text { get; set; }
        public JstreeStateModel State { get; set; }
        public bool children { get; set; }
        public int Type { get; set; }
        public string ObjectId { get; set; }
    }
}
