using System;
using System.Collections.Generic;
using System.Text;

namespace Minnow.DependencyInjection
{
    public abstract class ServiceConfigurationManager<TServiceProvider> : IDisposable
        where TServiceProvider : ServiceProvider<TServiceProvider>
    {
        /// <summary>
        /// Get an instance of the managed service configuration.
        /// </summary>
        /// <returns></returns>
        public abstract Object GetInstance();

        /// <summary>
        /// Get an instance of the managed service configuration.
        /// </summary>
        /// <returns></returns>
        public abstract Object GetInstance(Action<Object, TServiceProvider, ServiceConfiguration<TServiceProvider>> customSetup, Int32 customSetupOrder);

        #region IDisposable Implementation
        public virtual void Dispose()
        {
            //
        }
        #endregion
    }
}
