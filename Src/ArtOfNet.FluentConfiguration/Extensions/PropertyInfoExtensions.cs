using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;

namespace ConfArt.Extensions
{
    public static class PropertyInfoExtensions
    {
        /// <summary>
        /// Get an action delegate to get a property of an object T
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="propertyInfo">the property to bind getter method</param>
        /// <returns>the getter delegate</returns>
        public static Func<T, object> GetValueGetter<T>(this PropertyInfo propertyInfo)
        {
            if (typeof(T) != propertyInfo.DeclaringType)
            {
                throw new ArgumentException();
            }
            ParameterExpression instance = Expression.Parameter(propertyInfo.DeclaringType, "i");
            MemberExpression property = Expression.Property(instance, propertyInfo);
            UnaryExpression convert = Expression.TypeAs(property, typeof(object));
            return (Func<T, object>)Expression.Lambda(convert, instance).Compile();

        }
        
        /// <summary>
        /// Get an action delegate to set a property of an object T
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="propertyInfo">the property to bind setter method</param>
        /// <returns>the setter delegate</returns>
        public static Action<T, object> GetValueSetter<T>(this PropertyInfo propertyInfo)
        {
            if (typeof(T) != propertyInfo.DeclaringType)
            {
                throw new ArgumentException();

            }
            var instance = Expression.Parameter(propertyInfo.DeclaringType, "i");
            var argument = Expression.Parameter(typeof(object), "a");
            var setterCall = Expression.Call(
                instance,
                propertyInfo.GetSetMethod(),
                Expression.Convert(argument, propertyInfo.PropertyType));
            return (Action<T, object>)Expression.Lambda(setterCall, instance, argument).Compile();

        }
    }
}
