using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace System.Reflection
{
    public static class MemberInfoExtensions
    {
        /// <summary>
        /// Check if a property contains an attribute.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="member"></param>
        /// <param name="filter"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public static Boolean HasCustomAttribute<TAttribute>(this MemberInfo member, Func<TAttribute, Boolean> filter = null, Boolean inherit = true)
            where TAttribute : Attribute
                => member.GetCustomAttributes(inherit).Any(a => typeof(TAttribute).IsAssignableFrom(a.GetType()) && (filter?.Invoke(member as TAttribute) ?? true));

        /// <summary>
        /// Return the first matching specific attribute, if any.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="member"></param>
        /// <param name="filter"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public static TAttribute GetAttribute<TAttribute>(this MemberInfo member, Func<TAttribute, Boolean> filter = null, Boolean inherit = true)
            where TAttribute : Attribute
                => member.GetCustomAttributes(inherit).FirstOrDefault(a => typeof(TAttribute).IsAssignableFrom(a.GetType()) && (filter?.Invoke(member as TAttribute) ?? true)) as TAttribute;

        /// <summary>
        /// Attempt to get the defined attribute, if it exists.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="member"></param>
        /// <param name="attribute"></param>
        /// <param name="filter"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public static Boolean TryGetAttribute<TAttribute>(this MemberInfo member, out TAttribute attribute, Func<TAttribute, Boolean> filter = null, Boolean inherit = true)
            where TAttribute : Attribute
        {
            if(member.HasCustomAttribute<TAttribute>(filter, inherit))
            {
                attribute = member.GetAttribute<TAttribute>(filter, inherit);
                return true;
            }

            attribute = default;
            return false;
        } 
    }
}
