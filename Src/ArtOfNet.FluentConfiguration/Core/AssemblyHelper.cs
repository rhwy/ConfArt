using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using ConfArt.Contracts;

namespace ArtOfNet.ConfArt.Core
{
    public static class AssemblyHelper
    {
        public static IEnumerable<Assembly> GetUserLoadedAssemblies()
        {
            var assemblies = from a in AppDomain.CurrentDomain.GetAssemblies()
                             where !a.GlobalAssemblyCache
                             select a;

            return assemblies;
        }

        public static IEnumerable<Tuple<Type,Type>> GetAllGenericTypesFromUserLoadedAssemblies()
        {
            var genericTypes = from assembly in GetUserLoadedAssemblies()
                               from type in assembly.GetTypes().AsQueryable()
                               from genericInterface in type.GetInterfaces().AsQueryable()
                               where genericInterface.IsGenericType
                               select new Tuple<Type,Type>(type,genericInterface);
            return genericTypes;
        }


        public static IEnumerable<ConfigurationMappingType> GetConfigurationMappings()
        {
            var mappings = from typeTupple in GetAllGenericTypesFromUserLoadedAssemblies()
                           where typeTupple.Item2.GetGenericTypeDefinition() == typeof(IConfigurationMappingsBase<>)
                                && !typeTupple.Item1.IsAbstract
                           let target = typeTupple.Item1
                           let source = typeTupple.Item1.BaseType.GetGenericArguments().First()
                           select new ConfigurationMappingType(
                               source, target
                               );
            return mappings;

            //var ti = from t in calling.GetTypes().AsQueryable()
            //         from i in t.GetInterfaces()
            //         where i.IsGenericType
            //         let ib = i.GetGenericTypeDefinition()
            //         where ib == typeof(IConfigurationMappingsBase<>)
            //         let mapping = t
            //         let mapped = t.BaseType.GetGenericArguments().First()
            //         select new { souce = mapped, target = mapping };

        }
    }
}
