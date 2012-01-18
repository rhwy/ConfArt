using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConfArt.Contracts;
using ConfArt.Exceptions;
using System.Reflection;
using ConfArt.Extensions;
using System.Collections;
using NLog;

namespace ConfArt.Core
{
    /// <summary>
    /// A factory to help the creation of objects instances based on
    /// dynamic values and convention mappings
    /// </summary>
    public static class ValueFactory
    {
        private static bool _initialisized = false;
        private static bool _automap = true;
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public static bool Initialized
        {
            get
            {
                return _initialisized;
            }
        }
        
        public static bool Automap
        {
            get
            {
                return _automap;
            }
            set
            {
                _automap = value;
            }
        }

        /// <summary>
        /// The static constructor auto loads the default configuration
        /// by discovery
        /// </summary>
        static ValueFactory()
        {
            ConfigurationMapper.AutoConfigure();
            _initialisized = true;
        }

        /// <summary>
        /// Try to get an instance of type T based on the dynamic object
        /// values and the IConfigurationMappingBase instance that defines
        /// the mapping between a T object and a dynamic instance
        /// </summary>
        /// <typeparam name="T">the type of the object to construct and fill</typeparam>
        /// <param name="values">a dynamic object with the values to fill</param>
        /// <returns></returns>
        public static T TryGet<T>(dynamic values) where T:class,new()
        {
            if (!ConfigurationMapper.HasMappingFor<T>())
            {
                if (Automap)
                {
                    return TryGetWithoutMapping<T>(values);
                }
                else
                {
                    _logger.Warn("Type {0} is not mapped in the configuration", typeof(T));
                    //return new T();
                    throw new NotAMappedTypeException();
                }
            }

            T result = default(T);

            try
            {
                IConfigurationMappingsBase<T> mapper = ConfigurationMapper.GetMapper<T>();
                result = mapper.Get(values);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw new MapperInvocationException();
            }
            return result;
        }

        private static T TryGetWithoutMapping<T>(dynamic values) where T : class,new()
        {
            Type t = typeof(T);

            T obj = Activator.CreateInstance<T>();

            var properties = from p in t.GetProperties()
                             where p.CanWrite
                             select p;

            foreach (PropertyInfo pi in properties.AsQueryable())
            {
                var set = pi.GetValueSetter<T>();
                
                IEnumerable<string> names = values.Keys;

                string key = (from n in names
                              where n.ToLower() == pi.Name.ToLower()
                              select n).FirstOrDefault();

                if(!string.IsNullOrEmpty(key))
                {
                    Type methodTargetType = pi.PropertyType;
                    object typedObject = null;
                    if (IsEnumerable(methodTargetType))
                    {
                        typedObject = (IEnumerable)values[key];
                        //Type[] genericArguments = methodTargetType.GetGenericArguments();
                        //Type defaultEnumeratedType = null;
                        //if (genericArguments != null && genericArguments.Length > 0)
                        //{
                        //    defaultEnumeratedType = genericArguments[0];
                        //    var listOfType = typeof(List<>).MakeGenericType(defaultEnumeratedType);
                            
                        //}
                    }
                    else
                    {
                        typedObject = Convert.ChangeType(values[key], methodTargetType);
                    }
                    
                    set(obj, typedObject);
                }
               
            }


            return obj;
        }

        private static bool IsEnumerable(Type source)
        {
            if (!IsValidTypeForEnumeration(source))
            {
                return false;
            }

            IEnumerable<Type> list = source.GetInterfaces().Where(t => t.Name == "IEnumerable");
            if (list == null)
            {
                return false;
            }
            if (list.Count() == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool IsValidTypeForEnumeration(Type source)
        {
            if (source == typeof(string))
            {
                return false;
            }

            return true;
        }
    }
}
