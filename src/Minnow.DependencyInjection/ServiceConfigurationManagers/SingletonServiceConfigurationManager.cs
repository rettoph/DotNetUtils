using System;
using System.Collections.Generic;
using System.Text;

namespace Minnow.DependencyInjection
{
    internal class SingletonServiceConfigurationManager<TServiceProvider> : ScopedServiceConfigurationManager<TServiceProvider>
        where TServiceProvider : ServiceProvider<TServiceProvider>
    {
        #region Constructors
        internal SingletonServiceConfigurationManager(
            ServiceConfiguration<TServiceProvider> configuration,
            TServiceProvider provider) : base(configuration, provider)
        {
            if (!provider.IsRoot)
                throw new ArgumentException($"Unable to create SingletonServiceManager off non Root service provider.");
        }
        #endregion
    }
}
