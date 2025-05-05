using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.BaseReadModels
{
    public class RefSqlPaging
    {
        public RefSqlPaging(int pageIndex) : this(pageIndex, 30)
        {
        }

        public RefSqlPaging(int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            if (PageSize <= 0)
            {
                PageSize = 30;
            }

            if (PageSize > 100000)
            {
                PageSize = 100000;
            }

            if (PageIndex <= 0)
            {
                PageIndex = 0;
            }
        }

        public int PageIndex { get; private set; }
        public int PageSize { get; set; }

        public int OffSet => PageIndex * PageSize;

        public int TotalRow { get; set; }
    }
}
