using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.Models.Shared
{
    public class TreeTableModel<T>
    {
        public T data { get; set; }
        public TreeTableModel<T>[] children { get; set; }
    }
}
