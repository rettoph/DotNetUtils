using DotNetUtils.General.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetUtils.DependencyInjection.Builders.Interfaces
{
    public interface ICustomActionBuilder<TServiceProvider, TArgs, TArgsBuilder> : IOrderable
        where TServiceProvider : ServiceProvider<TServiceProvider>
    {
        /// <summary>
        /// All <see cref="TypeFactoryBuilder"/>s who's <see cref="ITypeFactoryBuilder.Type"/>
        /// is <see cref="Type.IsAssignableFrom(Type)"/> will utilize the defined <see cref="IFactoryActionBuilder"/>.
        /// </summary>
        Type AssignableFactoryType { get; }

        /// <summary>
        /// Construct a new <see cref="FactoryAction{TArgs}"/> instance based on the current
        /// builder configuration.
        /// </summary>
        /// <returns></returns>
        CustomAction<TServiceProvider, TArgs, TArgsBuilder> Build();
    }
}
