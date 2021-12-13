using DotNetUtils.General.Interfaces;
using DotNetUtils.DependencyInjection.Builders.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetUtils.DependencyInjection.Builders
{
    public class TypeFactoryBuilder<TFactory, TServiceProvider> : ITypeFactoryBuilder<TServiceProvider>, IFluentPrioritizable<TypeFactoryBuilder<TFactory, TServiceProvider>>
        where TFactory : class
        where TServiceProvider : ServiceProvider<TServiceProvider>
    {
        #region Private Fields
        private Func<TServiceProvider, TFactory> _method;
        private UInt16? _maxPoolSize;
        #endregion

        #region Public Properties
        /// <inheritdoc />
        public Type Type { get; }

        /// <inheritdoc />
        public Int32 Priority { get; set; }

        /// <summary>
        /// The primary factory method.
        /// </summary>
        public Func<TServiceProvider, TFactory> Method
        {
            get => _method;
            set => this.SetMethod(value);
        }

        /// <summary>
        /// The maximum size of the factory's internal pool.
        /// </summary>
        public UInt16? MaxPoolSize
        {
            get => _maxPoolSize;
            set => this.SetMaxPoolSize(value);
        }
        #endregion

        #region Constructors
        public TypeFactoryBuilder(
            Type type)
        {
            typeof(TFactory).ValidateAssignableFrom(type);

            this.Type = type;
        }
        #endregion

        #region SetMethod Methods
        /// <summary>
        /// Set the primary factory method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public TypeFactoryBuilder<TFactory, TServiceProvider> SetMethod(Func<TServiceProvider, TFactory> method)
        {
            _method = method;

            return this;
        }

        /// <summary>
        /// Set the primary factory method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public TypeFactoryBuilder<TFactory, TServiceProvider> SetMethod<TOut>(Func<TServiceProvider, TOut> method)
            where TOut : class, TFactory
        {
            _method = method;

            return this;
        }

        public TypeFactoryBuilder<TFactory, TServiceProvider> SetDefaultConstructor<TOut>()
            where TOut : class, TFactory, new()
        {
            return this.SetMethod(_ => new TOut());
        }
        #endregion

        #region SetMaxPoolSize Methods
        /// <summary>
        /// Set the maximum size of the factory's internal pool.
        /// </summary>
        /// <param name="implementationType"></param>
        /// <returns></returns>
        public TypeFactoryBuilder<TFactory, TServiceProvider> SetMaxPoolSize(UInt16? maxPoolSize)
        {
            _maxPoolSize = maxPoolSize;

            return this;
        }
        #endregion

        #region TypeFactoryBuilder Implementation
        TypeFactory<TServiceProvider> ITypeFactoryBuilder<TServiceProvider>.Build(
            IEnumerable<CustomAction<TServiceProvider, TypeFactory<TServiceProvider>, ITypeFactoryBuilder<TServiceProvider>>> allBuilders)
        {
            TFactory DefaultMethod(ServiceProvider<TServiceProvider> provider)
            {
                return Activator.CreateInstance(this.Type) as TFactory;
            }

            CustomAction<TServiceProvider, TypeFactory<TServiceProvider>, ITypeFactoryBuilder<TServiceProvider>>[] builders = allBuilders.Where(b => {
                return this.Type.IsAssignableToOrSubclassOfGenericDefinition(b.AssignableFactoryType)&& b.Filter(this);
            }).ToArray();

            return new TypeFactory<TFactory, TServiceProvider>(
                type: this.Type,
                method: this.Method ?? DefaultMethod,
                builders: builders,
                maxPoolSize: this.MaxPoolSize ?? 500);
        }
        #endregion
    }
}
