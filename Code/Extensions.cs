﻿using System.ComponentModel;
using System.Reflection;

namespace Wryte.Code
{
    public static class Extensions
    {

        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();

            string name = Enum.GetName(type, value);

            if (!string.IsNullOrWhiteSpace(name))
            {
                FieldInfo field = type.GetField(name);

                if (field != null)
                {
                    if (Attribute.GetCustomAttribute(field,
                                typeof(DescriptionAttribute)) is DescriptionAttribute attr)
                    {
                        return attr.Description;
                    }
                }
            }

            return value.ToString();
        }

    }
}
