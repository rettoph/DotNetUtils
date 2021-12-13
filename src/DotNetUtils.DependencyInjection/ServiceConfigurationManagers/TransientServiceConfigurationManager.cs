using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetUtils.DependencyInjection
{
    internal class TransientServiceConfigurationManager<TServiceProvider> : ServiceConfigurationManager<TServiceProvider>
        where TServiceProvider : ServiceProvider<TServiceProvider>
    {
        #region Private Fields
        protected readonly ServiceConfiguration<TServiceProvider> configuration;
        protected readonly TServiceProvider provider;
        #endregion

        #region Public Properties
        /// <inheritdoc />
        public ServiceConfiguration<TServiceProvider> Configuration => this.configuration;
        #endregion

        #region Constructors
        internal TransientServiceConfigurationManager(
            ServiceConfiguration<TServiceProvider> configuration,
            TServiceProvider provider)
        {
            this.configuration = configuration;
            this.provider = provider;
        }
        #endregion

        #region IServiceManager Implmentation
        /// <inheritdoc />
        public override Object GetInstance()
        {
            this.configuration.TypeFactory.BuildInstance(this.provider, this.configuration, out Object instance);
            return instance;
        }

        /// <inheritdoc />
        public override Object GetInstance(Action<Object, TServiceProvider, ServiceConfiguration<TServiceProvider>> customSetup, Int32 customSetupOrder)
        {
            this.configuration.TypeFactory.BuildInstance(this.provider, this.configuration, customSetup, customSetupOrder, out Object instance);
            return instance;
        }
        #endregion

        #region IDisposable Implementation
        public virtual void Dispose()
        {
            //
        }
        #endregion
    }
}
