using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using ConfArt.Extensions;
using ConfArt.Contracts;
using NLog;

namespace ConfArt.Core
{
    public abstract class ConfigurationMappingsBase<T> : IConfigurationMappingsBase<T> where T:class,new()
    {
        private Dictionary<string, Action<T, object>> _settings = new Dictionary<string, Action<T, object>>();
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public  Dictionary<string, Action<T, object>> Settings
        {
            get
            {
                return _settings;
            }
        }
        public virtual void Configure(){}

        public Type ConfiguredType
        {
            get
            {
                return typeof(T);
            }
        }

        public PropertySetterExpression<T> Map(Expression<Func<T, object>> memberToSet)
        {
            PropertySetterExpression<T> mapping = new PropertySetterExpression<T>(this, memberToSet);
            mapping.UpdateSource = (s, a) =>
                {
                    if (!_settings.ContainsKey(s))
                    {
                        _settings.Add(s, a);
                    }
                };
            return mapping;
        }

        public T Get(dynamic values)
        {
            this.Configure();
            if (_settings.Count == 0)
            {
                return null;
            }

            T obj = Activator.CreateInstance<T>();

            foreach (string key in _settings.Keys)
            {
                _settings[key](obj, values[key]);
            }
            return obj;
        }

        public T Get(Dictionary<string,object> values)
        {

            this.Configure();
            if (_settings.Count == 0)
            {
                return null;
            }

            T obj = Activator.CreateInstance<T>();

            foreach (string key in _settings.Keys)
            {
                try
                {
                    _settings[key](obj, values[key]);
                }
                catch (Exception ex)
                {
                    _logger.Warn("there is a problem with the value mapping", ex.Message);
                }
                
            }
            return obj;
        }
    }
}
