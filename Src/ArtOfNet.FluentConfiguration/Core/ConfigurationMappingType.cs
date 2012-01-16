using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtOfNet.ConfArt.Core
{
    public class ConfigurationMappingType
    {
        public Type Source { get; set; }
        public Type Target { get; set; }

        public ConfigurationMappingType(Type source, Type target)
        {
            Source = source;
            Target = target;
        }

        public ConfigurationMappingType()
        {

        }
    }
}
