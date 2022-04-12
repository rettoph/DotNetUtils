using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class TypeExtensions
    {
        public static bool IsConcrete(this Type type)
        {
            return type.IsClass && !type.IsAbstract && !type.IsInterface;
        }

        public static IEnumerable<Type> GetTypesAssignableFrom<TBase>(this IEnumerable<Type> types)
        {
            return types.GetTypesAssignableFrom(typeof(TBase));
        }
        public static IEnumerable<Type> GetTypesAssignableFrom(this IEnumerable<Type> types, Type baseType)
            => types.Where(t => baseType.IsAssignableFrom(t) || (baseType.IsGenericType && t.IsSubclassOfGenericDefinition(baseType)));

        /// <summary>
        /// As advertised, stolen from here:
        /// https://stackoverflow.com/questions/457676/check-if-a-class-is-derived-from-a-generic-class
        /// </summary>
        /// <param name="type"></param>
        /// <param name="generic"></param>
        /// <returns></returns>
        public static bool IsSubclassOfGenericDefinition(this Type type, Type generic)
        {
            if(!generic.IsGenericTypeDefinition)
            {
                return false;
            }

            while (type != null && type != typeof(object))
            {
                var cur = type.IsGenericType ? type.GetGenericTypeDefinition() : type;

                if (generic == cur)
                {
                    return true;
                }
                type = type.BaseType;
            }
            return false;
        }

        public static Boolean IsAssignableToOrSubclassOfGenericDefinition(this Type child, Type parent)
        {
            if (parent.IsAssignableFrom(child))
            {
                return true;
            }

            if (parent.IsGenericTypeDefinition && child.IsSubclassOfGenericDefinition(parent))
            {
                return true;
            }

            return false;
        }


        public static IEnumerable<Type> GetTypesWithAttribute<TBase, TAttribute>(this IEnumerable<Type> types, Boolean inherit = true)
            where TAttribute : Attribute
                => types.GetTypesWithAttribute(typeof(TBase), typeof(TAttribute), inherit);

        public static IEnumerable<Type> GetTypesWithAttribute(this IEnumerable<Type> types, Type baseType, Type attribute, Boolean inherit = true)
        {
            if (!typeof(Attribute).IsAssignableFrom(attribute))
                throw new Exception("Unable to load types with attribute, attribute type does not extend Attribute.");

            return types.GetTypesAssignableFrom(baseType)
                .Where(t =>
                {
                    var info = t.GetCustomAttributes(attribute, inherit);
                    return info != null && info.Length > 0;
                });
        }

        /// <summary>
        /// https://stackoverflow.com/questions/17480990/get-name-of-generic-class-without-tilde
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string GetPrettyName(this Type t)
        {
            if (!t.IsGenericType)
                return t.Name;

            StringBuilder sb = new StringBuilder();
            sb.Append(t.Name.Substring(0, t.Name.IndexOf('`')));
            sb.Append('<');
            bool appendComma = false;
            foreach (Type arg in t.GetGenericArguments())
            {
                if (appendComma) sb.Append(',');
                sb.Append(GetPrettyName(arg));
                appendComma = true;
            }
            sb.Append('>');
            return sb.ToString();
        }

        /// <summary>
        /// Recursively return all <see cref="Type"/> ancestors
        /// between a given child and parent type (inclusive)
        /// </summary>
        /// <param name="child"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetAncestors(this Type child, Type parent)
        {
            var type = child;
            parent.ValidateAssignableFrom(child);

            if(child == parent)
            {
                yield return child;
            }
            else if(parent.IsGenericType && !parent.IsConstructedGenericType)
            {
                while (type.IsSubclassOfGenericDefinition(parent))
                {
                    yield return type;
                    type = type.BaseType;
                }
            }
            else if (parent.IsInterface)
            { // Recersively add types until the interface is no longer implemented...
                while (type.GetInterfaces().Contains(parent))
                {
                    yield return type;
                    type = type.BaseType;
                }

                yield return parent;
            }
            else
            { // return types until the base type is hit...
                while (parent.IsAssignableFrom(type))
                {
                    yield return type;
                    type = type.BaseType;
                }
            }
        }
        public static IEnumerable<Type> GetAncestors<TParent>(this Type child)
            => child.GetAncestors(typeof(TParent));

        /// <summary>
        /// Verify that <paramref name="baseType"/> is assignable from the given <paramref name="targetType"/>.
        /// 
        /// If it is not, throw an exception.
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="baseType"></param>
        public static Boolean ValidateAssignableFrom(this Type baseType, Type targetType)
        {
            if (!baseType.IsAssignableFrom(targetType) && !targetType.IsSubclassOfGenericDefinition(baseType))
                throw new ArgumentException($"Unable to assign Type<{targetType.Name}> to Type<{baseType.Name}>.");

            return true;
        }

        /// <summary>
        /// Verify that the target type is assignable from the given base type.
        /// 
        /// If it is not, throw an exception.
        /// </summary>
        /// <typeparam name="TBase"></typeparam>
        /// <param name="targetType"></param>
        public static Boolean ValidateAssignableFrom<TBase>(this Type targetType)
        {
            return typeof(TBase).ValidateAssignableFrom(targetType);
        }
    }
}
