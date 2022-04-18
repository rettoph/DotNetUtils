using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Minnow.System.Helpers
{
    public static class ScopeHelper
    {
        public static void GetActiveEventData(object instance)
        {
            EventInfo[] eventsInfo = instance.GetType().GetEvents(BindingFlags.Default);

            foreach(EventInfo eventInfo in eventsInfo)
            {
                FieldInfo fieldInfo = instance.GetType().GetField(eventInfo.Name);
                Delegate value = fieldInfo.GetValue(instance) as Delegate;

                if(value.GetInvocationList().Length > 0)
                {
                    Console.WriteLine(instance.GetType().Name);
                }
            }
        }
    }
}
