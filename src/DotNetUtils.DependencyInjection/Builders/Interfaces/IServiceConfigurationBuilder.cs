using DotNetUtils.General.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetUtils.DependencyInjection.Builders.Interfaces
{
    public interface IServiceConfigurationBuilder<TServiceProvider> : IPrioritizable
        where TServiceProvider : ServiceProvider<TServiceProvider>
    {
        String Name { get; }

        /// <summary>
        /// The lookup key of the service's <see cref="TypeFactory"/>.
        /// </summary>
        public Type FactoryType { get; }

        /// <summary>
        /// The service lifetime.
        /// </summary>
        public ServiceLifetime Lifetime { get; }

        /// <summary>
        /// A list of strings with which this service will be cached once activated.
        /// All queries matching any of these values will return the defined
        /// configuration.
        /// </summary>
        public List<String> CacheNames { get; }

        ServiceConfiguration<TServiceProvider> Build(
            Dictionary<Type, TypeFactory<TServiceProvider>> typeFactories,
            IEnumerable<CustomAction<TServiceProvider, ServiceConfiguration<TServiceProvider>, IServiceConfigurationBuilder<TServiceProvider>>> allSetups);
    }
}
