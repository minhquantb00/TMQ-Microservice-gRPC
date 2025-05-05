using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.Common;

namespace TMQ.EsRepositories
{
    public class EsRemoveResult
    {
        public string? result { get; set; }
        public bool errors => result?.AsEmpty() != "deleted";
    }
}
