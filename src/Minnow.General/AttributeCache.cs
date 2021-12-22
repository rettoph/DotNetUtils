using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Minnow.General
{
    public sealed class AttributeCache<TAttribute>
        where TAttribute : Attribute
    {
        private Dictionary<MemberInfo, TAttribute> _attributes = new Dictionary<MemberInfo, TAttribute>();

        public TAttribute this[MemberInfo type] => this.GetAttribute(type);

        public TAttribute GetAttribute(MemberInfo member)
        {
            if(!_attributes.TryGetValue(member, out TAttribute attribute))
            {
                attribute = member.GetCustomAttribute<TAttribute>();
                _attributes.Add(member, attribute);
            }

            return attribute;
        }

        public TAttribute GetAttribute<T>()
        {
            return this.GetAttribute(typeof(T));
        }
    }
}
