using DotNetUtils.DependencyInjection.Builders.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetUtils.DependencyInjection
{
    internal class SingletonServiceConfiguration<TServiceProvider> : ServiceConfiguration<TServiceProvider>
        where TServiceProvider : ServiceProvider<TServiceProvider>
    {
        public SingletonServiceConfiguration(
            string name,
            TypeFactory<TServiceProvider> typeFactory,
            ServiceLifetime lifetime,
            string[] cacheNames,
            CustomAction<TServiceProvider, ServiceConfiguration<TServiceProvider>, IServiceConfigurationBuilder<TServiceProvider>>[] setups) : base(name, typeFactory, lifetime, cacheNames, setups)
        {
        }

        public override ServiceConfigurationManager<TServiceProvider> BuildServiceCofigurationManager(TServiceProvider provider)
        {
            if (provider.IsRoot)
                return new SingletonServiceConfigurationManager<TServiceProvider>(this, provider);
            else
                return provider.Root.GetServiceConfigurationManager(this);
        }
    }
}
