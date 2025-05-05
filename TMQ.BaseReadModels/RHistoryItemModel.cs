using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.BaseReadModels
{
    public record RHistoryItemModel
    {
        public string? Group { get; set; }
        public string? GroupIndex { get; set; }
        public required string FieldName { get; set; }
        public bool IsSystem { get; set; }

        public bool IsChange { get; set; }

        public string? ValueOld { get; set; }
        public string? ValueOldType { get; set; }
        public string? ValueNew { get; set; }
        public string? ValueNewType { get; set; }
        public string? ValuesAdd { get; set; }
        public string? ValuesAddType { get; set; }
        public string? ValuesRemove { get; set; }
        public string? ValuesRemoveType { get; set; }
        public string? ValuesChange { get; set; }
        public string? ValuesChangeType { get; set; }
    }
}
