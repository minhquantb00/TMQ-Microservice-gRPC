using ProtoBuf.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseReadModels;
using TMQ.Common;

namespace TMQ.BaseDomains
{
    public class HistoryItemDomain
    {
        public HistoryItemDomain(RHistoryItemModel historyItemModel)
        {
            Group = historyItemModel.Group;
            GroupIndex = historyItemModel.GroupIndex;
            FieldName = historyItemModel.FieldName;
            IsSystem = historyItemModel.IsSystem;
            ValueOld = historyItemModel.ValueOld;
            ValueOldType = historyItemModel.ValueOldType;
            ValueNew = historyItemModel.ValueNew;
            ValueNewType = historyItemModel.ValueNewType;
            ValuesAdd = historyItemModel.ValuesAdd;
            ValuesAddType = historyItemModel.ValuesAddType;
            ValuesRemove = historyItemModel.ValuesRemove;
            ValuesRemoveType = historyItemModel.ValuesRemoveType;
            ValuesChange = historyItemModel.ValuesChange;
            ValuesChangeType = historyItemModel.ValuesChangeType;
            IsChange = historyItemModel.IsChange;
        }

        public HistoryItemDomain(string? group, string groupIndex, string fieldName, string? valueOld, string? valueNew,
            bool isSystem = false)
        {
            Group = group;
            GroupIndex = groupIndex;
            FieldName = fieldName;
            IsSystem = isSystem;
            ValueOld = valueOld;
            ValueNew = valueNew;
            if (string.IsNullOrEmpty(valueOld) && !string.IsNullOrEmpty(valueNew))
                ValuesAdd = valueNew; // Nếu cũ không có, mới có là thêm mới
            if (string.IsNullOrEmpty(valueNew) && !string.IsNullOrEmpty(valueOld))
                ValuesRemove = valueOld; // Nễu cũ có, mới không có là xóa đi
            IsChange = ValueOld != ValueNew;
        }

        public HistoryItemDomain(string? group, string groupIndex, string fieldName, string[]? valueOld, string[]? valueNew,
            bool isSystem = false)
        {
            Group = group;
            GroupIndex = groupIndex;
            FieldName = fieldName;
            IsSystem = isSystem;
            valueOld = valueOld?.ToArray() ?? [];
            valueNew = valueNew?.ToArray() ?? [];
            ValueOld = valueOld.Order().ToArray().AsArrayJoin();
            ValueNew = valueNew.Order().ToArray().AsArrayJoin();
            if (valueOld.Length == 0 && valueNew.Length == 0)
            {
                IsChange = false;
                return;
            }

            if (valueOld.Length == 0 && valueNew.Length > 0)
            {
                IsChange = true;
                ValuesAdd = ValueNew;
                return;
            }

            if (valueOld.Length > 0 && valueNew.Length == 0)
            {
                IsChange = true;
                ValuesRemove = ValueOld;
                return;
            }

            bool equal = valueOld.SequenceEqual(valueNew);
            if (equal)
            {
                IsChange = false;
                return;
            }

            var differOld = valueOld.Except(valueNew).ToArray();
            var differNew = valueNew.Except(valueOld).ToArray();
            if (differOld.Length > 0)
            {
                ValuesRemove = differOld.Order().ToArray().AsArrayJoin();
                IsChange = true;
            }

            if (differNew.Length > 0)
            {
                ValuesAdd = differNew.Order().ToArray().AsArrayJoin();
                IsChange = true;
            }
        }

        public HistoryItemDomain(string? group, string groupIndex, string fieldName,
            Func<(object? ValueOld, object? ValueNew, object? ValuesAdd, object? ValuesRemove, object? ValuesChange, bool
                IsChange)> compare, bool isSystem = false)
        {
            Group = group;
            GroupIndex = groupIndex;
            FieldName = fieldName;
            IsSystem = isSystem;
            var result = compare();
            ValueOld = Serialize.JsonSerializeObject(result.ValueOld);
            ValueOldType = result.ValueOld?.GetType().AssemblyQualifiedName;
            ValueNew = Serialize.JsonSerializeObject(result.ValueNew);
            ValueNewType = result.ValueNew?.GetType().AssemblyQualifiedName;
            ValuesAdd = Serialize.JsonSerializeObject(result.ValuesAdd);
            ValuesAddType = result.ValuesAdd?.GetType().AssemblyQualifiedName;
            ValuesRemove = Serialize.JsonSerializeObject(result.ValuesRemove);
            ValuesRemoveType = result.ValuesRemove?.GetType().AssemblyQualifiedName;
            ValuesChange = Serialize.JsonSerializeObject(result.ValuesChange);
            ValuesChangeType = result.ValuesChange?.GetType().AssemblyQualifiedName;
            IsChange = result.IsChange;
        }

        public string? Group { get; private set; }
        public string? GroupIndex { get; private set; }
        public string FieldName { get; private set; }
        public bool IsSystem { get; private set; }
        public string? ValueOld { get; private set; }
        public string? ValueOldType { get; private set; }
        public string? ValueNew { get; private set; }
        public string? ValueNewType { get; private set; }

        public string? ValuesAdd { get; private set; }
        public string? ValuesAddType { get; private set; }
        public string? ValuesRemove { get; private set; }
        public string? ValuesRemoveType { get; private set; }
        public string? ValuesChange { get; private set; }
        public string? ValuesChangeType { get; private set; }
        public bool IsChange { get; private set; }
    }
}
