using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using ConfArt.Extensions;
using ConfArt.Contracts;

namespace ConfArt.Core
{
    public class PropertySetterExpression<T> where T:class,new()
    {
        private Action<T, object> _setter;
        private Expression<Func<T, object>> _setterExpression;
        private ConfigurationMappingsBase<T> _source;

        public Action<string, Action<T, object>> UpdateSource { get; set; }

        public PropertySetterExpression(ConfigurationMappingsBase<T> source, Expression<Func<T, object>> setterExpression)
        {
            _source = source;
            _setterExpression = setterExpression;
        }

        
        public void With(string field)
        {
            Expression expBoxy = _setterExpression.Body;

            if (expBoxy.NodeType == ExpressionType.Convert)
            {
                var convertExp = (UnaryExpression)expBoxy;
                var operand = (MemberExpression)convertExp.Operand;
                
                if (operand.Member is PropertyInfo)
                {
                    PropertyInfo pi = typeof(T).GetProperty(operand.Member.Name);
                    _setter = pi.GetValueSetter<T>();
                }

            }
            else if (expBoxy.NodeType == ExpressionType.MemberAccess )
            {
                var operand = (MemberExpression)expBoxy;
               
                if (operand.Member is PropertyInfo)
                {
                    PropertyInfo pi = typeof(T).GetProperty(operand.Member.Name);
                    _setter = pi.GetValueSetter<T>();
                }

            }
            else
            {
                throw new InvalidOperationException("Expression is not valid to set a property, should be something like x=>x.Name");
            }

            if (_source != null && _setter != null && UpdateSource != null)
            {
                UpdateSource(field,_setter);
            }
        }
    }
}
