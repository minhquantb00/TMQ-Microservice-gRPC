using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.Config;

namespace TMQ.Models.Shared
{
    [ProtoContract]
    public class KeyValueTypeLongModel
    {
        [ProtoMember(1)] public long Value { get; set; }
        public long Id => Value;

        [ProtoMember(2)] public string Text { get; set; }

        [ProtoMember(3)] public bool Checked { get; set; }

        public static KeyValueTypeLongModel[] FromEnum(Type enumType, bool addDefaultItem = true)
        {
            var values = Enum.GetValues(enumType);
            int length = values.Length;
            if (addDefaultItem)
            {
                length++;
            }

            KeyValueTypeLongModel[] keyValueModels = new KeyValueTypeLongModel[length];
            int i = 0;
            if (addDefaultItem)
            {
                keyValueModels[i] = new KeyValueTypeLongModel()
                {
                    Text = "Select One",
                    Value = 0,
                };
                i++;
            }

            foreach (var value in values)
            {
                keyValueModels[i] = new KeyValueTypeLongModel()
                {
                    Text = ((Enum)value).GetDisplayName(),
                    Value = ((long)value),
                };
                i++;
            }

            return keyValueModels;
        }

        public static KeyValueTypeLongModel[] FromEnumSelectMultiple(Type enumType, long[]? allStatus)
        {
            var values = Enum.GetValues(enumType);
            int length = values.Length;
            KeyValueTypeLongModel[] keyValueModels = new KeyValueTypeLongModel[length];
            int i = 0;
            foreach (var value in values)
            {
                keyValueModels[i] = new KeyValueTypeLongModel()
                {
                    Text = ((Enum)value).GetDisplayName(),
                    Value = ((long)value),
                    Checked = allStatus?.Contains((long)value) == true
                };
                i++;
            }

            return keyValueModels;
        }

        public static KeyValueTypeLongModel[] FromEnumSelectMultiple(Type enumType, Enum? allStatus)
        {
            var values = Enum.GetValues(enumType);
            int length = values.Length;
            KeyValueTypeLongModel[] keyValueModels = new KeyValueTypeLongModel[length];
            int i = 0;
            foreach (var value in values)
            {
                keyValueModels[i] = new KeyValueTypeLongModel()
                {
                    Text = ((Enum)value).GetDisplayName(),
                    Value = ((long)value),
                    Checked = allStatus?.HasFlag((Enum)value) == true
                };
                i++;
            }

            return keyValueModels;
        }

        public static KeyValueTypeLongModel[] FromEnumSelectMultiple(Type enumType)
        {
            var values = Enum.GetValues(enumType);
            int length = values.Length;
            KeyValueTypeLongModel[] keyValueModels = new KeyValueTypeLongModel[length];
            int i = 0;
            foreach (var value in values)
            {
                keyValueModels[i] = new KeyValueTypeLongModel()
                {
                    Text = ((Enum)value).GetDisplayName(),
                    Value = ((long)value)
                };
                i++;
            }

            return keyValueModels;
        }

        public static KeyValueTypeLongModel[] FromEnumSelectMultiple(Array values, Enum? allStatus)
        {
            int length = values.Length;
            KeyValueTypeLongModel[] keyValueModels = new KeyValueTypeLongModel[length];
            int i = 0;
            foreach (var value in values)
            {
                keyValueModels[i] = new KeyValueTypeLongModel()
                {
                    Text = ((Enum)value).GetDisplayName(),
                    Value = ((long)value),
                    Checked = allStatus?.HasFlag((Enum)value) == true
                };
                i++;
            }

            return keyValueModels;
        }

        public static KeyValueTypeLongModel[]? FromEnumSelectByStatuses(Type enumType, long[]? statuses)
        {
            if (statuses is null || statuses is not { Length: > 0 })
            {
                return null;
            }
            var values = Enum.GetValues(enumType);
            int length = statuses.Length;
            KeyValueTypeLongModel[] keyValueModels = new KeyValueTypeLongModel[length];
            int i = 0;
            foreach (var value in values)
            {
                if (statuses.Contains((long)value))
                {
                    keyValueModels[i] = new KeyValueTypeLongModel()
                    {
                        Text = ((Enum)value).GetDisplayName(),
                        Value = ((long)value),
                        Checked = statuses?.Contains((long)value) == true
                    };
                    i++;
                }
            }

            return keyValueModels;
        }
    }
}
