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
    /// </summary>
    public class AssemblyHelper : IDisposable
    {
        /// <summary>
        /// Any added assembly must reference one of these or be one of these.
        /// </summary>
        private AssemblyName[] _libraries;

        /// <summary>
        /// List of all unique assemblies loaded
        /// </summary>
        private HashSet<Assembly> _assemblies;

        /// <summary>
        /// List of all unique assemblies loaded
        /// </summary>
        public IEnumerable<Assembly> Assemblies => _assemblies;

        /// <summary>
        /// List of all unique types loaded
        /// </summary>
        public IEnumerable<Type> Types => this.Assemblies.SelectMany(a => a.GetTypes());

        /// <summary>
        /// Any added assembly must reference one of these or be one of these.
        /// </summary>
        public IEnumerable<AssemblyName> Libraries => _libraries;

        /// <summary>
        /// Invoked when an assembly gets successfully added.
        /// </summary>
        public event OnEventDelegate<AssemblyHelper, Assembly> OnAssemblyLoaded;

        #region Helper Methods
        /// <summary>
        /// Create a new <see cref="AssemblyHelper"/> instance with a default <paramref name="entry"/>
        /// and <paramref name="libraries"/> assembly.
        /// </summary>
        /// <param name="libraries">Any added assembly must reference one of these or be one of these.</param>
        public AssemblyHelper(IEnumerable<Assembly> libraries = default)
        {
            _assemblies = new HashSet<Assembly>();

            _libraries = libraries?.Select(a => a.GetName()).Distinct().ToArray() ?? Array.Empty<AssemblyName>();
        }

        /// <summary>
        /// Ensure the recieved assembly references at least one item within <see cref="Libraries"/>.
        /// If it does, add the <paramref name="assembly"/> and all of its reference <see cref="Type"/>s to the <see cref="AssemblyHelper"/>.
        /// </summary>
        /// <param name="assembly"></param>
        public bool TryLoadAssembly(Assembly assembly)
        {
            if(this.ShouldAddAssembly(assembly) && _assemblies.Add(assembly))
            {
                this.AddReferenceAssemblies(assembly);

                this.OnAssemblyLoaded?.Invoke(this, assembly);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Add a new <see cref="Assembly"/> and all of its reference types to the <see cref="AssemblyHelper"/>.
        /// </summary>
        /// <param name="assembly"></param>
        private void AddReferenceAssemblies(Assembly assembly)
        {
            foreach (Assembly referencedAssembly in assembly.GetReferencedAssemblies().Select(an => Assembly.Load(an)))
            {
                this.TryLoadAssembly(referencedAssembly);
            }
        }

        protected Boolean ShouldAddAssembly(Assembly assembly)
        {
            if(_libraries.Count() == 0)
            {
                return true;
            }

            if(_libraries.Any(r => AssemblyName.ReferenceMatchesDefinition(assembly.GetName(), r)))
            { // Check if the recieved assembly is an existing library...
                return true;
            }
            
            if(assembly.GetReferencedAssemblies().Any(nan => _libraries.Any(r => AssemblyName.ReferenceMatchesDefinition(nan, r))))
            { // Ensure the assembly references a required library...
                return true;
            }

            return false;
        }

        public void Dispose()
        {
            _assemblies.Clear();
        }
        #endregion
    }
}
