using Minnow.General.Interfaces;
using Minnow.DependencyInjection.Builders.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;


namespace Minnow.DependencyInjection.Builders
{
    public class CustomActionBuilder<T, TServiceProvider, TMethodArgs, TFilterArgs> : ICustomActionBuilder<TServiceProvider, TMethodArgs, TFilterArgs>, IFluentOrderable<CustomActionBuilder<T, TServiceProvider, TMethodArgs, TFilterArgs>>
        where T : class
        where TServiceProvider : ServiceProvider<TServiceProvider>
    {
        #region Public Properties
        /// <inheritdoc />
        public Type AssignableFactoryType { get; }

        /// <inheritdoc />
        public Int32 Order { get; set; }

        /// <summary>
        /// The action to be invoked.
        /// </summary>
        public CustomAction<T, TServiceProvider, TMethodArgs, TFilterArgs>.MethodDelegate Method { get; set; }

        /// <summary>
        /// A custom filter ran at runtime to confirm if the
        /// recieved <see cref="Type"/> should actually be bound
        /// to the described FactoryAction.
        /// </summary>
        public CustomAction<TServiceProvider, TMethodArgs, TFilterArgs>.FilterDelegate Filter { get; set; }
        #endregion

        #region Constructors
        public CustomActionBuilder(
            Type assignableFactoryType)
        {
            typeof(T).IsAssignableFrom(assignableFactoryType);

            this.AssignableFactoryType = assignableFactoryType;
        }
        #endregion

        #region SetMethod Methods
        /// <summary>
        /// Set the <see cref="Method"/> value.
        /// </summary>
        /// <param name="method">The action to be invoked.</param>
        /// <returns></returns>
        public CustomActionBuilder<T, TServiceProvider, TMethodArgs, TFilterArgs> SetMethod(CustomAction<T, TServiceProvider, TMethodArgs, TFilterArgs>.MethodDelegate method)
        {
            this.Method = method;

            return this;
        }
        #endregion

        #region SetFilter Methods
        /// <summary>
        /// Set the <see cref="Filter"/> value.
        /// </summary>
        /// <param name="filter">A custom filter ran at runtime to confirm if the
        /// recieved <see cref="Type"/> should actually be bound
        /// to the described FactoryAction.</param>
        /// <returns></returns>
        public CustomActionBuilder<T, TServiceProvider, TMethodArgs, TFilterArgs> SetFilter(CustomAction<TServiceProvider, TMethodArgs, TFilterArgs>.FilterDelegate filter)
        {
            this.Filter = filter;

            return this;
        }
        #endregion

        #region IFactoryActionBuilder<TArgs> Implementation
        /// <inheritdoc />
        CustomAction<TServiceProvider, TMethodArgs, TFilterArgs> ICustomActionBuilder<TServiceProvider, TMethodArgs, TFilterArgs>.Build()
        {
            Debug.Assert(this.Method is not null, $"{this.GetType().GetPrettyName()}::{nameof(ICustomActionBuilder<TServiceProvider, TMethodArgs, TFilterArgs>.Build)} - {nameof(Method)} should be defined.");

            void DefaultMethod(T instance, TServiceProvider provider, TMethodArgs args)
            {

            }

            Boolean DefaultFilter(TFilterArgs type)
            {
                return true;
            }

            return new CustomAction<T, TServiceProvider, TMethodArgs, TFilterArgs>(
                this.AssignableFactoryType,
                this.Method ?? DefaultMethod,
                this.Filter ?? DefaultFilter,
                this.Order);
        }
        #endregion
    }
}
