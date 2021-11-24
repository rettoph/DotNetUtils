using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace System
{
    /// <summary>
    /// Simple static helper class used to interact
    /// with all loaded assemblies and their types.
    /// 
    /// This is required to dynamically load and configure
    /// games, scenes, layers, and even service loaders.
    /// </summary>
    public class AssemblyHelper : IDisposable
    {
        /// <summary>
        /// A list of assemblies that should be referenced by the adding
        /// assembly in order to be tracked.
        /// </summary>
        private AssemblyName[] _shouldReference;

        /// <summary>
        /// List of all unique assemblies loaded
        /// </summary>
        private HashSet<Assembly> _assemblies;

        /// <summary>
        /// List of all unique types loaded
        /// </summary>
        private HashSet<Type> _types;

        /// <summary>
        /// List of all unique assemblies loaded
        /// </summary>
        public IReadOnlyCollection<Assembly> Assemblies => _assemblies;

        /// <summary>
        /// List of all unique types loaded
        /// </summary>
        public IReadOnlyCollection<Type> Types => _types;

        public IReadOnlyCollection<AssemblyName> WithAssembliesReferencing => _shouldReference;
        #region Helper Methods


        /// <summary>
        /// Create a new <see cref="AssemblyHelper"/> instance with a default <paramref name="entry"/>
        /// and <paramref name="withAssembliesReferencing"/> assembly.
        /// </summary>
        /// <param name="entry">The assembly to begin adding references too. This will default to <see cref="Assembly.GetEntryAssembly()"/>.</param>
        /// <param name="withAssembliesReferencing">The assembly that must be referenced by all tracked nested assemblies. If no values are included this will default to <see cref="Assembly.GetExecutingAssembly()"/></param>
        public AssemblyHelper(Assembly entry = default, IEnumerable<Assembly> withAssembliesReferencing = default)
        {
            _assemblies = new HashSet<Assembly>();
            _types = new HashSet<Type>();

            _shouldReference = withAssembliesReferencing?.Select(a => a.GetName()).ToArray() ?? new AssemblyName[] 
            {
                Assembly.GetExecutingAssembly().GetName()
            };

            this.TryAddAssembly(entry ?? Assembly.GetEntryAssembly());

            Console.WriteLine("Start Output:");
            foreach(Assembly assembly in _assemblies)
            {
                Console.WriteLine(assembly.FullName);
            }
        }

        /// <summary>
        /// Ensure the recieved assembly references at least one item within <see cref="WithAssembliesReferencing"/>.
        /// If it does, add the <paramref name="assembly"/> and all of its reference <see cref="Type"/>s to the <see cref="AssemblyHelper"/>.
        /// </summary>
        /// <param name="assembly"></param>
        public void TryAddAssembly(Assembly assembly)
        {
            if(this.ShouldAddAssembly(assembly) && _assemblies.Add(assembly))
            {
                this.AddAssembly(assembly);
            }
        }

        /// <summary>
        /// Add a new <see cref="Assembly"/> and all of its reference types to the <see cref="AssemblyHelper"/>.
        /// </summary>
        /// <param name="assembly"></param>
        public void AddAssembly(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                _types.Add(type);
            }

            foreach (Assembly referencedAssembly in assembly.GetReferencedAssemblies().Select(an => Assembly.Load(an)))
            {
                this.TryAddAssembly(referencedAssembly);
            }
        }

        protected Boolean ShouldAddAssembly(Assembly assembly)
        {
            if(_shouldReference.Any(r => AssemblyName.ReferenceMatchesDefinition(assembly.GetName(), r)))
            { // The recieved assmebly is the _shouldReference assembly...
                return true;
            }
            
            if(assembly.GetReferencedAssemblies().Any(nan => _shouldReference.Any(r => AssemblyName.ReferenceMatchesDefinition(nan, r))))
            { // The recieved assembly references the _shuoldReference assembly...
                return true;
            }

            return false;
        }

        public void Dispose()
        {
            _assemblies.Clear();
            _types.Clear();
        }
        #endregion
    }
}
