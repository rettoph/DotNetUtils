using Minnow.General.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Minnow.DependencyInjection.Builders.Interfaces
{
    public interface ITypeFactoryBuilder<TServiceProvider> : IPrioritizable
        where TServiceProvider : ServiceProvider<TServiceProvider>
    {
        public Type Type { get; }

        TypeFactory<TServiceProvider> Build(IEnumerable<CustomAction<TServiceProvider, TypeFactory<TServiceProvider>, ITypeFactoryBuilder<TServiceProvider>>> allBuilders);
    }
}
