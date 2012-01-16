using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtOfNet.ConfArt.Exceptions
{
    public class DynamicXmlParseException: Exception
    {
        public DynamicXmlParseException(Exception innerException)
            : base(ConfArtException.DynamicXmlParse.ToString(),innerException)
        {         
        }
        public DynamicXmlParseException(string message,Exception innerException)
            : base(message, innerException)
        {
        }

        public DynamicXmlParseException(string message)
            : base(message)
        {
        }
        public ConfArtException TypeException
        {
            get
            {
                return ConfArtException.DynamicXmlParse;
            }
        }

        public int TypeExceptionId
        {
            get
            {
                return (int)TypeException;
            }
        }
    }
}
