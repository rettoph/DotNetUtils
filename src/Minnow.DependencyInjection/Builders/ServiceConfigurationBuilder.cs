using Minnow.General.Interfaces;
using Minnow.DependencyInjection.Builders.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;

namespace Minnow.DependencyInjection.Builders
{
    public class ServiceConfigurationBuilder<TService, TServiceProvider> : IServiceConfigurationBuilder<TServiceProvider>, IFluentPrioritizable<ServiceConfigurationBuilder<TService, TServiceProvider>>
        where TService : class
        where TServiceProvider : ServiceProvider<TServiceProvider>
    {
        #region Protected Properties
        protected ServiceProviderBuilder<TServiceProvider> services { get; private set; }
        #endregion

        #region Public Properties
        /// <inheritdoc />
        public String Name { get; }

        /// <inheritdoc />
        public Int32 Priority { get; set; }

        /// <inheritdoc />
        public Type FactoryType { get; set; }

        /// <inheritdoc />
        public ServiceLifetime Lifetime { get; set; }

        /// <inheritdoc />
        public List<String> CacheNames { get; set; }
        #endregion

        #region Constructor
        public ServiceConfigurationBuilder(
            String name,
            ServiceProviderBuilder<TServiceProvider> services)
        {
            this.services = services;

            this.Name = name;
            this.CacheNames = new List<String>();
            this.SetFactoryType<TService>();
        }
        #endregion

        #region SetFactoryType Methods
        /// <summary>
        /// Set the lookup key of the service's <see cref="TypeFactory"/>.
        /// </summary>
        /// <param name="factoryType"></param>
        /// <returns></returns>
        public ServiceConfigurationBuilder<TService, TServiceProvider> SetFactoryType(Type factoryType)
        {
            typeof(TService).ValidateAssignableFrom(factoryType);

            this.FactoryType = factoryType;

            return this;
        }
        /// <summary>
        /// Set the lookup key of the service's <see cref="TypeFactory"/>.
        /// </summary>
        /// <typeparam name="TFactoryType"></typeparam>
        /// <returns></returns>
        public ServiceConfigurationBuilder<TService, TServiceProvider> SetFactoryType<TFactoryType>()
            where TFactoryType : class, TService
        {
            return this.SetFactoryType(typeof(TFactoryType));
        }
        #endregion

        #region SetTypeFactory Methods
        /// <summary>
        /// Register and define a brand new <see cref="TypeFactoryBuilder"/>, then link it to the
        /// current <see cref="ServiceConfigurationBuilder"/>.
        /// </summary>
        /// <typeparam name="TFactoryType"></typeparam>
        /// <param name="type"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public ServiceConfigurationBuilder<TService, TServiceProvider> SetTypeFactory<TFactoryType>(Type type, Action<TypeFactoryBuilder<TFactoryType, TServiceProvider>> builder)
            where TFactoryType : class, TService
        {
            TypeFactoryBuilder<TFactoryType, TServiceProvider> typeFactory = this.services.RegisterTypeFactory<TFactoryType>(type);
            builder(typeFactory);

            return this.SetFactoryType(typeFactory.Type);
        }
        /// <summary>
        /// Register and define a brand new <see cref="TypeFactoryBuilder"/>, then link it to the
        /// current <see cref="ServiceConfigurationBuilder"/>.
        /// </summary>
        /// <typeparam name="TFactoryType"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public ServiceConfigurationBuilder<TService, TServiceProvider> SetTypeFactory<TFactoryType>(Action<TypeFactoryBuilder<TFactoryType, TServiceProvider>> builder)
            where TFactoryType : class, TService
        {
            return this.SetTypeFactory<TFactoryType>(typeof(TFactoryType), builder);
        }
        /// <summary>
        /// Register and define a brand new <see cref="TypeFactoryBuilder"/>, then link it to the
        /// current <see cref="ServiceConfigurationBuilder"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public ServiceConfigurationBuilder<TService, TServiceProvider> SetTypeFactory(Type type, Action<TypeFactoryBuilder<TService, TServiceProvider>> builder)
        {
            return this.SetTypeFactory<TService>(type, builder);
        }
        /// <summary>
        /// Register and define a brand new <see cref="TypeFactoryBuilder"/>, then link it to the
        /// current <see cref="ServiceConfigurationBuilder"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public ServiceConfigurationBuilder<TService, TServiceProvider> SetTypeFactory(Action<TypeFactoryBuilder<TService, TServiceProvider>> builder)
        {
            return this.SetTypeFactory<TService>(builder);
        }
        #endregion

        #region SetLifetime Methods
        /// <summary>
        /// Set the service lifetime.
        /// </summary>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public ServiceConfigurationBuilder<TService, TServiceProvider> SetLifetime(ServiceLifetime lifetime)
        {
            this.Lifetime = lifetime;

            return this;
        }
        #endregion

        #region AddCacheName(s) Methods
        /// <summary>
        /// Add a singlular cache name.
        /// </summary>
        /// <param name="name">A value with which this service will be cached once activated.
        /// All queries matching  this value will return the defined
        /// configuration.</param>
        /// <returns></returns>
        public ServiceConfigurationBuilder<TService, TServiceProvider> AddCacheName(String name)
        {
            this.CacheNames.Add(name);

            return this;
        }

        /// <summary>
        /// Add a singlular cache name, defaulting to the <see cref="Type.Name"/>
        /// </summary>
        /// <param name="type">A value with which this service will be cached once activated.
        /// All queries matching  this value will return the defined
        /// configuration.</param>
        /// <returns></returns>
        public ServiceConfigurationBuilder<TService, TServiceProvider> AddCacheName(Type type)
        {
            return this.AddCacheName(type.Name);
        }

        /// <summary>
        /// Add many cache names at once.
        /// </summary>
        /// <param name="names">A list of strings with which this service will be cached once activated.
        /// All queries matching any of these values will return the defined
        /// configuration.</param>
        /// <returns></returns>
        public ServiceConfigurationBuilder<TService, TServiceProvider> AddCacheNames(IEnumerable<String> names)
        {
            this.CacheNames.AddRange(names);

            return this;
        }

        /// <summary>
        /// Att many cache names at once, based on the input types.
        /// The names will default to <see cref="Type.Name"/>
        /// </summary>
        /// <param name="types">A list of strings with which this service will be cached once activated.
        /// All queries matching any of these values will return the defined
        /// configuration.</param>
        /// <returns></returns>
        public ServiceConfigurationBuilder<TService, TServiceProvider> AddCacheNames(IEnumerable<Type> types)
        {
            return this.AddCacheNames(types.Select(t => t.FullName));
        }

        /// <summary>
        /// Add a cache name for every single <see cref="Type"/> between <typeparamref name="TService"/>
        /// and <paramref name="childType"/>
        /// </summary>
        /// <param name="childType"></param>
        /// <returns></returns>
        public ServiceConfigurationBuilder<TService, TServiceProvider> AddCacheNamesBetweenTypes(Type parent, Type childType)
        {
            typeof(TService).ValidateAssignableFrom(childType);
            parent.ValidateAssignableFrom(childType);

            return this.AddCacheNames(childType.GetAncestors(parent));
        }

        /// <summary>
        /// Add a cache name for every single <see cref="Type"/> between <typeparamref name="TService"/>
        /// and <typeparamref name="TChild"/> 
        /// </summary>
        /// <typeparam name="TChild"></typeparam>
        /// <returns></returns>
        public ServiceConfigurationBuilder<TService, TServiceProvider> AddCacheNamesBetweenTypes<TParent, TChild>()
            where TChild : class, TService, TParent
        {
            return this.AddCacheNamesBetweenTypes(typeof(TParent), typeof(TChild));
        }

        /// <summary>
        /// Add a cache name for every single <see cref="Type"/> between <typeparamref name="TService"/>
        /// and <paramref name="childType"/>
        /// </summary>
        /// <param name="childType"></param>
        /// <returns></returns>
        public ServiceConfigurationBuilder<TService, TServiceProvider> AddCacheNamesBetweenType(Type childType)
        {
            return this.AddCacheNamesBetweenTypes(typeof(TService), childType);
        }

        /// <summary>
        /// Add a cache name for every single <see cref="Type"/> between <typeparamref name="TService"/>
        /// and <typeparamref name="TChild"/> 
        /// </summary>
        /// <typeparam name="TChild"></typeparam>
        /// <returns></returns>
        public ServiceConfigurationBuilder<TService, TServiceProvider> AddCacheNamesBetweenType<TChild>()
            where TChild : class, TService
        {
            return this.AddCacheNamesBetweenTypes<TService, TChild>();
        }
        #endregion

        #region TypeFactoryBuilder Implementation
        ServiceConfiguration<TServiceProvider> IServiceConfigurationBuilder<TServiceProvider>.Build(
            Dictionary<Type, TypeFactory<TServiceProvider>> typeFactories,
            IEnumerable<CustomAction<TServiceProvider, ServiceConfiguration<TServiceProvider>, IServiceConfigurationBuilder<TServiceProvider>>> allSetups)
        {

            Type factoryType = this.FactoryType ?? typeof(TService);
            TypeFactory<TServiceProvider> typeFactory = typeFactories[factoryType];
            String[] cacheNames = this.CacheNames.Concat(this.Name).Distinct().ToArray();
            CustomAction<TServiceProvider, ServiceConfiguration<TServiceProvider>, IServiceConfigurationBuilder<TServiceProvider>>[] setups = allSetups.Where(b => {
                return typeFactory.Type.IsAssignableToOrSubclassOfGenericDefinition(b.AssignableFactoryType) && b.Filter(this);
            }).ToArray();

            return this.Lifetime switch
            {
                ServiceLifetime.Singleton => new SingletonServiceConfiguration<TServiceProvider>(this.Name, typeFactory, this.Lifetime, cacheNames, setups),
                ServiceLifetime.Scoped => new ScopedServiceConfiguration<TServiceProvider>(this.Name, typeFactory, this.Lifetime, cacheNames, setups),
                _ => new TransientServiceConfiguration<TServiceProvider>(this.Name, typeFactory, this.Lifetime, cacheNames, setups
                )
            };
        }
        #endregion
    }
}
