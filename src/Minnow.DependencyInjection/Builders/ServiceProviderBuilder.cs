using Minnow.DependencyInjection.Builders.Interfaces;
using Minnow.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minnow.DependencyInjection.Builders
{
    // public class ServiceProviderBuilder : ServiceProviderBuilder<ServiceProvider>
    // {
    //     public override ServiceProvider Build()
    //     {
    //         throw new NotImplementedException();
    //     }
    // }

    public abstract class ServiceProviderBuilder<TServiceProvider>
        where TServiceProvider : ServiceProvider<TServiceProvider>
    {
        #region Private Fields
        protected List<ITypeFactoryBuilder<TServiceProvider>> typeFactories { get; private set; }
        protected List<ICustomActionBuilder<TServiceProvider, TypeFactory<TServiceProvider>, ITypeFactoryBuilder<TServiceProvider>>> builders { get; private set; }
        protected List<ICustomActionBuilder<TServiceProvider, ServiceConfiguration<TServiceProvider>, IServiceConfigurationBuilder<TServiceProvider>>> setups { get; private set; }
        protected List<IServiceConfigurationBuilder<TServiceProvider>> serviceConfigurations { get; private set; }
        #endregion

        #region Constructors
        public ServiceProviderBuilder()
        {
            this.typeFactories = new List<ITypeFactoryBuilder<TServiceProvider>>();
            this.builders = new List<ICustomActionBuilder<TServiceProvider, TypeFactory<TServiceProvider>, ITypeFactoryBuilder<TServiceProvider>>>();
            this.setups = new List<ICustomActionBuilder<TServiceProvider, ServiceConfiguration<TServiceProvider>, IServiceConfigurationBuilder<TServiceProvider>>>();
            this.serviceConfigurations = new List<IServiceConfigurationBuilder<TServiceProvider>>();
        }
        #endregion

        #region RegisterTypeFactory Methods
        public TypeFactoryBuilder<TFactory, TServiceProvider> RegisterTypeFactory<TFactory>(Type type)
            where TFactory : class
        {
            TypeFactoryBuilder<TFactory, TServiceProvider> typeFactory = new TypeFactoryBuilder<TFactory, TServiceProvider>(type);
            this.typeFactories.Add(typeFactory);

            return typeFactory;
        }
        public TypeFactoryBuilder<TFactory, TServiceProvider> RegisterTypeFactory<TFactory>()
            where TFactory : class
        {
            return this.RegisterTypeFactory<TFactory>(typeof(TFactory));
        }
        public TypeFactoryBuilder<Object, TServiceProvider> RegisterTypeFactory(Type type)
        {
            return this.RegisterTypeFactory<Object>(type);
        }
        #endregion

        #region RegisterDefaultTypeFactory Methods
        public TypeFactoryBuilder<TFactory, TServiceProvider> RegisterDefaultTypeFactory<TFactory>()
            where TFactory : class, new()
        {
            return this.RegisterTypeFactory<TFactory>(typeof(TFactory))
                .SetMethod(p => new TFactory());
        }
        #endregion

        #region RegisterBuilder Methods
        /// <summary>
        /// Define a new builder action that will run immidiately after a <see cref="TypeFactory{T}"/> creates a new instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assignableFactoryType">All <see cref="TypeFactoryBuilder"/>s who's <see cref="TypeFactoryBuilder.Type"/>
        /// is <see cref="Type.IsAssignableFrom(Type)"/> will utilize the defined <see cref="IFactoryActionBuilder"/>.</param>
        /// <returns></returns>
        public CustomActionBuilder<T, TServiceProvider, TypeFactory<TServiceProvider>, ITypeFactoryBuilder<TServiceProvider>> RegisterBuilder<T>(Type assignableFactoryType)
            where T : class
        {
            CustomActionBuilder<T, TServiceProvider, TypeFactory<TServiceProvider>, ITypeFactoryBuilder<TServiceProvider>> builder = new CustomActionBuilder<T, TServiceProvider, TypeFactory<TServiceProvider>, ITypeFactoryBuilder<TServiceProvider>>(assignableFactoryType);
            this.builders.Add(builder);

            return builder;
        }

        /// <summary>
        /// Define a new builder action that will run immidiately after a <see cref="TypeFactory{T}"/> creates a new instance.
        /// </summary>
        /// <typeparam name="TAssignableFactoryType">All <see cref="TypeFactoryBuilder"/>s who's <see cref="TypeFactoryBuilder.Type"/>
        /// is <see cref="Type.IsAssignableFrom(Type)"/> will utilize the defined <see cref="IFactoryActionBuilder"/>.</typeparam>
        /// <returns></returns>
        public CustomActionBuilder<TAssignableFactoryType, TServiceProvider, TypeFactory<TServiceProvider>, ITypeFactoryBuilder<TServiceProvider>> RegisterBuilder<TAssignableFactoryType>()
            where TAssignableFactoryType : class
        {
            return this.RegisterBuilder<TAssignableFactoryType>(typeof(TAssignableFactoryType));
        }

        /// <summary>
        /// Define a new builder action that will run immidiately after a <see cref="TypeFactory{T}"/> creates a new instance.
        /// </summary>
        /// <param name="assignableFactoryType">All <see cref="TypeFactoryBuilder"/>s who's <see cref="TypeFactoryBuilder.Type"/>
        /// is <see cref="Type.IsAssignableFrom(Type)"/> will utilize the defined <see cref="IFactoryActionBuilder"/>.</param>
        /// <returns></returns>
        public CustomActionBuilder<Object, TServiceProvider, TypeFactory<TServiceProvider>, ITypeFactoryBuilder<TServiceProvider>> RegisterBuilder(Type assignableFactoryType)
        {
            return this.RegisterBuilder<Object>(assignableFactoryType);
        }
        #endregion

        #region RegisterSetup Methods
        /// <summary>
        /// Define a new setup action that will run immidiately after a <see cref="TypeFactory{T}"/> selects a new instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assignableFactoryType">All <see cref="TypeFactoryBuilder"/>s who's <see cref="TypeFactoryBuilder.Type"/>
        /// is <see cref="Type.IsAssignableFrom(Type)"/> will utilize the defined <see cref="IFactoryActionBuilder"/>.</param>
        /// <returns></returns>
        public CustomActionBuilder<T, TServiceProvider, ServiceConfiguration<TServiceProvider>, IServiceConfigurationBuilder<TServiceProvider>> RegisterSetup<T>(Type assignableFactoryType)
            where T : class
        {
            CustomActionBuilder<
                T, 
                TServiceProvider, 
                ServiceConfiguration<TServiceProvider>, 
                IServiceConfigurationBuilder<TServiceProvider>
            > setup = new CustomActionBuilder<
                        T, TServiceProvider, 
                        ServiceConfiguration<TServiceProvider>, 
                        IServiceConfigurationBuilder<TServiceProvider>>(assignableFactoryType);

            this.setups.Add(setup);

            return setup;
        }

        /// <summary>
        /// Define a new setup action that will run immidiately after a <see cref="TypeFactory{T}"/> selects a new instance.
        /// </summary>
        /// <typeparam name="TAssignableFactoryType">All <see cref="TypeFactoryBuilder"/>s who's <see cref="TypeFactoryBuilder.Type"/>
        /// is <see cref="Type.IsAssignableFrom(Type)"/> will utilize the defined <see cref="IFactoryActionBuilder"/>.</typeparam>
        /// <returns></returns>
        public CustomActionBuilder<TAssignableFactoryType, TServiceProvider, ServiceConfiguration<TServiceProvider>, IServiceConfigurationBuilder<TServiceProvider>> RegisterSetup<TAssignableFactoryType>()
            where TAssignableFactoryType : class
        {
            return this.RegisterSetup<TAssignableFactoryType>(typeof(TAssignableFactoryType));
        }

        /// <summary>
        /// Define a new setup action that will run immidiately after a <see cref="TypeFactory{T}"/> selects a new instance.
        /// </summary>
        /// <param name="assignableFactoryType">All <see cref="TypeFactoryBuilder"/>s who's <see cref="TypeFactoryBuilder.Type"/>
        /// is <see cref="Type.IsAssignableFrom(Type)"/> will utilize the defined <see cref="IFactoryActionBuilder"/>.</param>
        /// <returns></returns>
        public CustomActionBuilder<Object, TServiceProvider, ServiceConfiguration<TServiceProvider>, IServiceConfigurationBuilder<TServiceProvider>> RegisterSetup(Type assignableFactoryType)
        {
            return this.RegisterSetup<Object>(assignableFactoryType);
        }
        #endregion

        #region RegisterService Methods
        public ServiceConfigurationBuilder<TService, TServiceProvider> RegisterService<TService>(String name)
            where TService : class
        {
            ServiceConfigurationBuilder<TService, TServiceProvider> serviceConfigurationBuilder = new ServiceConfigurationBuilder<TService, TServiceProvider>(name, this);
            this.serviceConfigurations.Add(serviceConfigurationBuilder);

            return serviceConfigurationBuilder;
        }
        public ServiceConfigurationBuilder<TService, TServiceProvider> RegisterService<TService>()
            where TService : class
        {
            return this.RegisterService<TService>(typeof(TService).FullName);
        }
        public ServiceConfigurationBuilder<Object, TServiceProvider> RegisterService(String name)
        {
            return this.RegisterService<Object>(name);
        }
        #endregion

        #region Protected Helper Methods
        private void BuildFactories(out Dictionary<Type, TypeFactory<TServiceProvider>> factories)
        {
            // Build all BuilderActions & SetupActions...
            List<CustomAction<TServiceProvider, TypeFactory<TServiceProvider>, ITypeFactoryBuilder<TServiceProvider>>> allBuilders = this.builders.Order().Select(b => b.Build()).ToList();

            // Build all TypeFactories...
            factories = this.typeFactories.PrioritizeBy(f => f.Type)
                .Select(f => f.Build(allBuilders))
                .ToDictionaryByValue(
                    keySelector: f => f.Type);
        }

        private void BuildServiceConfigurations(
            Dictionary<Type, TypeFactory<TServiceProvider>> factories,
            out DoubleDictionary<String, UInt32, ServiceConfiguration<TServiceProvider>> serviceConfigurations)
        {
            List<CustomAction<TServiceProvider, ServiceConfiguration<TServiceProvider>, IServiceConfigurationBuilder<TServiceProvider>>> allSetups = this.setups.Order().Select(b => b.Build()).ToList();

            // Build all ServiceConfigurations...
            List<ServiceConfiguration<TServiceProvider>> allServiceConfigurations = this.serviceConfigurations.PrioritizeBy(s => s.Name)
                .Select(s => s.Build(factories, allSetups))
                .ToList();

            serviceConfigurations = allServiceConfigurations.ToDoubleDictionary(sc => sc.Name, sc => sc.Id);
        }

        /// <summary>
        /// Construct a new <typeparamref name="TServiceProvider"/> instance.
        /// </summary>
        /// <returns></returns>
        public TServiceProvider Build()
        {
            this.BuildFactories(out Dictionary<Type, TypeFactory<TServiceProvider>> factories);
            this.BuildServiceConfigurations(factories, out DoubleDictionary <String, UInt32, ServiceConfiguration<TServiceProvider>> serviceConfigurations);

            return this.Build(serviceConfigurations);
        }
        #endregion

        #region Abstract Methods
        public abstract TServiceProvider Build(DoubleDictionary<String, UInt32, ServiceConfiguration<TServiceProvider>> services);
        #endregion

        #region Deconstructor
        public void Deconstruct(
            out Dictionary<Type, TypeFactory<TServiceProvider>> factories,
            out DoubleDictionary<String, UInt32, ServiceConfiguration<TServiceProvider>> serviceConfigurations)
        {
            this.BuildFactories(out factories);
            this.BuildServiceConfigurations(factories, out serviceConfigurations);
        }
        #endregion
    }
}
