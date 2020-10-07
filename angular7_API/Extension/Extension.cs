using System;
using System.ComponentModel;
using System.Reflection;

namespace angular_API.Extension
{
    public static class Extension
    {
        /// <summary>
        /// 擴充套件方法，獲得列舉的Description
        /// <returns>列舉的Description</returns>
        public static string GetDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        /// <summary>
        /// 擴充套件方法，獲得列舉的 數值
        /// <returns>列舉的 數值</returns>
        public static int GetValue(this object value)
        {
            var _value = (int)value;

            return _value;
        }
    }
}
