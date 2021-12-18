using Minnow.DependencyInjection.Builders.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Minnow.DependencyInjection
{
    internal sealed class TransientServiceConfiguration<TServiceProvider> : ServiceConfiguration<TServiceProvider>
        where TServiceProvider : ServiceProvider<TServiceProvider>
    {
        public TransientServiceConfiguration(
            string name,
            TypeFactory<TServiceProvider> typeFactory,
            ServiceLifetime lifetime,
            string[] cacheNames,
            CustomAction<TServiceProvider, ServiceConfiguration<TServiceProvider>, IServiceConfigurationBuilder<TServiceProvider>>[] setups) : base(name, typeFactory, lifetime, cacheNames, setups)
        {
        }

        public override ServiceConfigurationManager<TServiceProvider> BuildServiceCofigurationManager(TServiceProvider provider)
        {
            return new TransientServiceConfigurationManager<TServiceProvider>(this, provider);
        }
    }
}
