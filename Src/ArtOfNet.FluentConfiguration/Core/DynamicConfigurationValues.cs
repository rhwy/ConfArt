using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

namespace ConfArt.Core
{
    public class DynamicConfigurationValues : DynamicObject
    {
        private Dictionary<string, object> _values = new Dictionary<string, object>();
        private readonly object _syncRoot = new object();

        private bool _emptyIfNull = false;

        public DynamicConfigurationValues()
        {
            
        }

        public DynamicConfigurationValues(bool emptyIfNull)
        {
            _emptyIfNull = emptyIfNull;
        }


        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            bool resultOperation = false;
            result = null;
            if (_values.ContainsKey(binder.Name))
            {
                result = _values[binder.Name];
                resultOperation = true;
            }
            else
            {
                if (_emptyIfNull)
                {
                    result = string.Empty;
                    resultOperation = true;
                }
            }
            return resultOperation;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            bool resultOperation = false;
            lock (_syncRoot)
            {
                if (!_values.ContainsKey(binder.Name))
                {
                    _values.Add(binder.Name, value);
                    resultOperation = true;
                }
                else
                {
                    _values[binder.Name] = value;
                    resultOperation = true;
                }
            }
            
            return resultOperation;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            if (indexes.Length == 0)
            {
                result = null;
                return false;
            }

            string key = indexes[0] as string;
            if (string.IsNullOrEmpty(key))
            {
                result = null;
                return false;
            }

            return _values.TryGetValue(key, out result);
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            if (indexes.Length == 0)
            {
                return false;
            }
            string key = indexes[0] as string;
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            lock (_syncRoot)
            {
                if (_values.ContainsKey(key))
                {
                    _values[key] = value;
                }
                else
                {
                    _values.Add(key, value);
                }
            }
            return true;
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return _values.Keys;
        }

        public IEnumerable<string> Keys
        {
            get
            {
                return _values.Keys;
            }
        }

        public bool HasProperty(string key)
        {
            return _values.Keys.Contains(key);
        }

        public override string ToString()
        {
            string result = string.Empty;
            string pattern = "{0} : {1}" + Environment.NewLine;

            foreach (var item in _values)
            {
                result += string.Format(pattern, item.Key, item.Value);
            }

            return result;
        }
    }
}
