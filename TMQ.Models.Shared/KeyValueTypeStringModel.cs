using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.Common;
using TMQ.Config;

namespace TMQ.Models.Shared
{
    [ProtoContract]
    public record KeyValueTypeStringModel
    {
        [ProtoMember(1)] public string Value { get; set; }
        public string Id => Value;

        [ProtoMember(2)] public string? Text { get; set; }
        [ProtoMember(3)] public bool Checked { get; set; }
        public bool Selected => Checked;

        public static KeyValueTypeStringModel[] FromEnum(Type enumType, bool addDefaultItem = true)
        {
            var values = Enum.GetValues(enumType);
            int length = values.Length;
            if (addDefaultItem)
            {
                length++;
            }

            KeyValueTypeStringModel[] keyValueModels = new KeyValueTypeStringModel[length];
            int i = 0;
            if (addDefaultItem)
            {
                keyValueModels[i] = new KeyValueTypeStringModel()
                {
                    Text = "Select One",
                    Value = string.Empty,
                };
                i++;
            }

            foreach (var value in values)
            {
                keyValueModels[i] = new KeyValueTypeStringModel()
                {
                    Text = ((Enum)value).GetDisplayName(),
                    Value = ((int)value).AsString(),
                };
                i++;
            }

            return keyValueModels;
        }

        public static KeyValueTypeStringModel[] FromEnum(Type enumType, bool addDefaultItem, Enum[] exclusValues)
        {
            var values = Enum.GetValues(enumType);
            List<KeyValueTypeStringModel> keyValueModels = new List<KeyValueTypeStringModel>();
            if (addDefaultItem)
            {
                keyValueModels.Add(new KeyValueTypeStringModel()
                {
                    Text = "Select One",
                    Value = string.Empty,
                });
            }

            foreach (var value in values)
            {
                if (exclusValues.Contains((Enum)value))
                {
                    continue;
                }

                keyValueModels.Add(new KeyValueTypeStringModel()
                {
                    Text = ((Enum)value).GetDisplayName(),
                    Value = ((int)value).AsString(),
                });
            }

            return keyValueModels.ToArray();
        }

        public static KeyValueTypeStringModel[] FromEnum(Type enumType, string selectedValue,
            bool addDefaultItem = true)
        {
            var values = Enum.GetValues(enumType);
            int length = values.Length;
            if (addDefaultItem)
            {
                length++;
            }

            KeyValueTypeStringModel[] keyValueModels = new KeyValueTypeStringModel[length];
            int i = 0;
            if (addDefaultItem)
            {
                keyValueModels[i] = new KeyValueTypeStringModel()
                {
                    Text = "Select One",
                    Value = string.Empty,
                    Checked = false
                };
                i++;
            }

            foreach (var value in values)
            {
                keyValueModels[i] = new KeyValueTypeStringModel()
                {
                    Text = ((Enum)value).GetDisplayName(),
                    Value = ((int)value).AsString(),
                    Checked = selectedValue == ((int)value).AsString()
                };
                i++;
            }

            return keyValueModels;
        }

        public static KeyValueTypeStringModel DefaultItem()
        {
            return new KeyValueTypeStringModel()
            {
                Text = "Select One",
                Value = string.Empty,
                Checked = false
            };
        }

        public static KeyValueTypeStringModel[] AddDefaultItem(IEnumerable<KeyValueTypeStringModel> sources)
        {
            if (sources == null) return new[] { DefaultItem() };
            return new[] { DefaultItem() }.Union(sources).ToArray();
        }

        public static KeyValueTypeStringModel[] FromEnumSelectMultiple(Type enumType, int allStatus,
            bool addDefaultItem = true)
        {
            var values = Enum.GetValues(enumType);
            int length = values.Length;
            if (addDefaultItem)
            {
                length++;
            }

            KeyValueTypeStringModel[] keyValueModels = new KeyValueTypeStringModel[length];
            int i = 0;
            if (addDefaultItem)
            {
                keyValueModels[i] = new KeyValueTypeStringModel()
                {
                    Text = "Select One",
                    Value = string.Empty,
                    Checked = false
                };
                i++;
            }

            foreach (var value in values)
            {
                keyValueModels[i] = new KeyValueTypeStringModel()
                {
                    Text = ((Enum)value).GetDisplayName(),
                    Value = ((int)value).AsString(),
                    Checked = (allStatus & (int)value) == (int)value
                };
                i++;
            }

            return keyValueModels;
        }

        public static KeyValueTypeStringModel[] FromEnumSelectMultiple(Type enumType, long allStatus,
            bool addDefaultItem = true)
        {
            var values = Enum.GetValues(enumType);
            int length = values.Length;
            if (addDefaultItem)
            {
                length++;
            }

            KeyValueTypeStringModel[] keyValueModels = new KeyValueTypeStringModel[length];
            int i = 0;
            if (addDefaultItem)
            {
                keyValueModels[i] = new KeyValueTypeStringModel()
                {
                    Text = "Select One",
                    Value = string.Empty,
                    Checked = false
                };
                i++;
            }

            foreach (var value in values)
            {
                keyValueModels[i] = new KeyValueTypeStringModel()
                {
                    Text = ((Enum)value).GetDisplayName(),
                    Value = ((long)value).AsString(),
                    Checked = (allStatus & (long)value) == (long)value
                };
                i++;
            }

            return keyValueModels;
        }

        public object? Ext { get; set; }
        public object? Ext2 { get; set; }
        public object? Ext3 { get; set; }
    }
}
