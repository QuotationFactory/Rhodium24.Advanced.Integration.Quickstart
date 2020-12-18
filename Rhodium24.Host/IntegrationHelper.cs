using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Rhodium24.Host
{
    public static class IntegrationHelper
    {
        private const string PluginDirectoryPath = ".\\Plugins";
        private const string PluginSearchPattern = "*.Integration.dll";
        private static readonly List<Assembly> PluginAssemblies = new();
        private static readonly Dictionary<Type, IEnumerable<Type>> PluginTypes = new();

        public static IEnumerable<Assembly> GetAssemblies()
        {
            if (!Directory.Exists(PluginDirectoryPath))
                return Enumerable.Empty<Assembly>();
            
            if (PluginAssemblies.Any())
                return PluginAssemblies;
            
            var files = Directory.GetFiles(PluginDirectoryPath, PluginSearchPattern, SearchOption.AllDirectories);
            var assemblies = files.Select(x => Assembly.LoadFile(Path.GetFullPath(x)));
            PluginAssemblies.AddRange(assemblies);
            return PluginAssemblies;
        }
        
        public static IEnumerable<Type> GetClassesFromType<T>()
        {
            var searchType = typeof(T);
            
            if (PluginTypes.ContainsKey(searchType))
                return PluginTypes[searchType];

            var pluginTypes = GetAssemblies().SelectMany(x => x.GetTypes()).Where(x => searchType.IsAssignableFrom(x));
            PluginTypes.Add(searchType, pluginTypes);
            return PluginTypes[searchType];
        }
    }
}