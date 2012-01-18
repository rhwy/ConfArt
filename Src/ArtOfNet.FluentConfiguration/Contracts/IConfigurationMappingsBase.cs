using System;
using ConfArt.Core;
using System.Collections.Generic;

namespace ConfArt.Contracts
{
    public interface IConfigurationMappingsBase<T> : IConfigurationMapping where T : class, new()
    {
        void Configure();
        Dictionary<string, Action<T, object>> Settings { get; }
        T Get(dynamic values);
        T Get(System.Collections.Generic.Dictionary<string, object> values);
        PropertySetterExpression<T> Map(System.Linq.Expressions.Expression<Func<T, object>> memberToSet);
    }
}
