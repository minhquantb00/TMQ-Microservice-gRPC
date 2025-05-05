using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.Config
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum? enumValue)
        {
            try
            {
                if (enumValue == null)
                {
                    return string.Empty;
                }

                var configName = enumValue.GetType()
                    .GetMember(enumValue.ToString())
                    .First()?
                    .GetCustomAttribute<DisplayAttribute>()?
                    .GetName();
                return string.IsNullOrEmpty(configName) ? enumValue.ToString() : configName;
            }
            catch (Exception)
            {
                return enumValue!.ToString();
            }
        }

        public static int GetOrder(this Enum enumValue)
        {
            var orderConfig = enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()?
                .GetCustomAttribute<DisplayAttribute>()?
                .GetOrder().GetValueOrDefault();
            return orderConfig.GetValueOrDefault(0);
        }

        public static string GetConfig(this ConfigSettingEnum enumValue)
        {
            if (ConfigSetting.Configs.TryGetValue(enumValue, out var configValue))
                return configValue;
            return string.Empty;
        }
    }
}
