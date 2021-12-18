using Minnow.DependencyInjection.Builders.Interfaces;
using Minnow.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace Minnow.DependencyInjection
{
    public abstract class TypeFactory<TServiceProvider>
        where TServiceProvider : ServiceProvider<TServiceProvider>
    {
        public readonly Type Type;

        protected TypeFactory(Type type)
        {
            this.Type = type;
        }

        /// <summary>
        /// Return a configured instance of the defined type.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public abstract void BuildInstance(
            TServiceProvider provider, 
            ServiceConfiguration<TServiceProvider> configuration,
            out Object instance);

        /// <summary>
        /// Return a configured instance of the defined type.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="configuration"></param>
        /// <param name="customSetup"></param>
        /// <param name="customSetupOrder"></param>
        /// <returns></returns>
        public abstract void BuildInstance(
            TServiceProvider provider,
            ServiceConfiguration<TServiceProvider> configuration,
            Action<Object, TServiceProvider, ServiceConfiguration<TServiceProvider>> customSetup,
            Int32 customSetupOrder,
            out Object instance);

        /// <summary>
        /// Attempt to return a given item back into the factory pool
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public abstract Boolean TryReturnToPool(Object instance);
    }

    public sealed class TypeFactory<TFactory, TServiceProvider> : TypeFactory<TServiceProvider>
        where TFactory : class
        where TServiceProvider : ServiceProvider<TServiceProvider>
    {
        #region Private Fields
        private Pool<TFactory> _pool;
        #endregion

        #region Public Fields

        public readonly Func<TServiceProvider, TFactory> Method;
        public readonly CustomAction<TServiceProvider, TypeFactory<TServiceProvider>, ITypeFactoryBuilder<TServiceProvider>>[] Builders;
        public UInt16 MaxPoolSize;
        #endregion

        #region Constructor
        public TypeFactory(
            Type type, 
            Func<TServiceProvider, TFactory> method,
            CustomAction<TServiceProvider, TypeFactory<TServiceProvider>, ITypeFactoryBuilder<TServiceProvider>>[] builders,
            ushort maxPoolSize) : base(type)
        {
            this.Method = method;
            this.Builders = builders;
            this.MaxPoolSize = maxPoolSize;

            _pool = new Pool<TFactory>(ref this.MaxPoolSize);
        }
        #endregion

        #region Helper Methods
        private void GetInstance(TServiceProvider provider, out TFactory instance)
        {
            if (!_pool.TryPull(out instance))
            {
                instance = this.Method(provider);

                foreach (CustomAction<TServiceProvider, TypeFactory<TServiceProvider>, ITypeFactoryBuilder<TServiceProvider>> builder in this.Builders)
                {
                    builder.Invoke(instance, provider, this);
                }
            }
        }
        #endregion

        #region TypeFactory Implementation
        /// <inheritdoc />
        public override void BuildInstance(TServiceProvider provider, ServiceConfiguration<TServiceProvider> configuration, out Object instance)
        {
            this.GetInstance(provider, out TFactory item);
            instance = item;

            foreach (CustomAction<TServiceProvider, ServiceConfiguration<TServiceProvider>, IServiceConfigurationBuilder<TServiceProvider>> setup in configuration.Setups)
            {
                setup.Invoke(item, provider, configuration);
            }
        }

        /// <inheritdoc />
        public override void BuildInstance(
            TServiceProvider provider,
            ServiceConfiguration<TServiceProvider> configuration,
            Action<Object, TServiceProvider, ServiceConfiguration<TServiceProvider>> customSetup,
            Int32 customSetupOrder,
            out Object instance)
        {
            this.GetInstance(provider, out TFactory item);
            instance = item;

            Boolean ranCustomSetup = false;
            foreach (CustomAction<TServiceProvider, ServiceConfiguration<TServiceProvider>, IServiceConfigurationBuilder<TServiceProvider>> setup in configuration.Setups)
            {
                if (!ranCustomSetup && setup.Order >= customSetupOrder)
                {
                    customSetup(item, provider, configuration);
                    ranCustomSetup = true;
                }

                setup.Invoke(item, provider, configuration);
            }

            if (!ranCustomSetup)
            {
                customSetup(item, provider, configuration);
            }
        }

        /// <inheritdoc />
        public override bool TryReturnToPool(Object instance)
        {
            if(instance is TFactory item)
            {
                return _pool.TryReturn(item);
            }

            return false;
        }
        #endregion
    }
}
