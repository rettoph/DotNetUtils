using Minnow.DependencyInjection.Builders.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Minnow.DependencyInjection
{
    internal class ScopedServiceConfiguration<TServiceProvider> : ServiceConfiguration<TServiceProvider>
        where TServiceProvider : ServiceProvider<TServiceProvider>
    {
        public ScopedServiceConfiguration(
            string name, 
            TypeFactory<TServiceProvider> typeFactory,
            ServiceLifetime lifetime, 
            string[] cacheNames,
            CustomAction<TServiceProvider, ServiceConfiguration<TServiceProvider>, IServiceConfigurationBuilder<TServiceProvider>>[] setups) : base(name, typeFactory, lifetime, cacheNames, setups)
        {
        }

        public override ServiceConfigurationManager<TServiceProvider> BuildServiceCofigurationManager(TServiceProvider provider)
        {
            return new ScopedServiceConfigurationManager<TServiceProvider>(this, provider);
        }
    }
}
