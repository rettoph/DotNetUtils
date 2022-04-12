using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minnow.System.Helpers
{
    public static class ActivatorUtilitiesHelper
    {
        private static Type[] EmptyTypeArray = Array.Empty<Type>();
        private static object[] EmptyObjectArray = Array.Empty<object>();

        public static Func<IServiceProvider, T> BuildFactory<T>()
            where T : class
        {
            ObjectFactory factory = ActivatorUtilities.CreateFactory(typeof(T), EmptyTypeArray);

            T Method(IServiceProvider provider)
            {
                return factory.Invoke(provider, EmptyObjectArray) as T;
            }

            return Method;
        }
    }
}
