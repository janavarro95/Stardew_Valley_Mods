using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace StardustCore
{
    public static class StaticExtentions
    {
        /// <summary>
        /// Thank you stack overflow. https://stackoverflow.com/questions/3907299/if-statements-matching-multiple-values  
        /// Ex:) if(1.In(1,2,3)) //This returns true since 1 is in the parameter list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">Object to pass in value.</param>
        /// <param name="args">A list like (1,2,3) to see if it's contained.</param>
        public static bool In<T>(this T obj, params T[] args)
        {
            return args.Contains(obj);
        }

        public static bool HasValue(this double value)
        {
            return !double.IsNaN(value) && !double.IsInfinity(value);
        }

        /// <summary>
        /// Returns the description tag above an enum property to return a user friendly string.
        /// Thanks again to Stack Overflow: https://stackoverflow.com/questions/479410/enum-tostring-with-user-friendly-strings
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerationValue"></param>
        public static string GetEnumDescription<T>(this T enumerationValue)
             where T : struct
        {
            Type type = enumerationValue.GetType();
            if (!type.IsEnum)
                throw new ArgumentException("EnumerationValue must be of Enum type", "enumerationValue");

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    //Pull out the description value
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            //If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString();
        }
    }
}
