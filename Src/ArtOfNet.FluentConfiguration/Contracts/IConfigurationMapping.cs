using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfArt.Contracts
{
    public interface IConfigurationMapping
    {
        Type ConfiguredType { get; }
    }
}
