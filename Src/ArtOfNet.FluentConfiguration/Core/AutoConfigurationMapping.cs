using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConfArt.Core;

namespace ArtOfNet.ConfArt.Core
{
    public class AutoConfigurationMapping<T> : ConfigurationMappingsBase<T> where T : class,new()
    {

    }
}
