using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConfArt.Core;
using ConfArt.Contracts;
using System.Reflection;
using ArtOfNet.ConfArt.Core;

namespace ConfArt
{
    public static class ConfigurationMapper
    {
        private static Dictionary<Type, Type> _mappings = new Dictionary<Type, Type>();
        private static Dictionary<Type, IConfigurationMapping> _mappers = new Dictionary<Type, IConfigurationMapping>();
        private static Dictionary<Type, IEnumerable<string>> _automappings = new Dictionary<Type, IEnumerable<string>>();

        public static void Define<T,M>() where  T:class,new()  where M:IConfigurationMappingsBase<T>
        {
            if (!_mappings.ContainsKey(typeof(T)))
            {
                _mappings.Add(typeof(T), typeof(M));
            }
        }

        public static void Define(Type t, Type m)
        {
            if (!_mappings.ContainsKey(t))
            {
                _mappings.Add(t, m);
            }
        }

        public static void Define(Type t, IEnumerable<string> values)
        {
            if (!_automappings.ContainsKey(t))
            {
                _automappings.Add(t, values);
            }
        }

        public static bool HasMappingFor<T>()
        {
            bool result = _mappings.ContainsKey(typeof(T));
            if (!result)
            {
                result = _automappings.ContainsKey(typeof(T));
            }
            return result;
        }

        public static IConfigurationMappingsBase<T>  GetMapper<T>() where T:class,new()
        {
            Type mapperType;
            _mappings.TryGetValue(typeof(T), out mapperType);

            if (mapperType == null)
            {
                return null; //STEX
            }

            if (_mappers.ContainsKey(mapperType))
            {
                return (IConfigurationMappingsBase<T>)_mappers[mapperType];
            }
            else
            {
                return (IConfigurationMappingsBase<T>)Activator.CreateInstance(mapperType);
            }
        }

        public static void AutoConfigure()
        {
            if (_mappings.Count > 0)
            {
                return;
            }

            var ti = AssemblyHelper.GetConfigurationMappings();

            if (ti == null)
            {
                return;
            }

            var list = ti.ToList();

            if (list.Count == 0)
            {
                return;
            }

            foreach (var mapping in list)
            {
               ConfigurationMapper.Define(mapping.Source, mapping.Target);
            }
        }
    }
}
