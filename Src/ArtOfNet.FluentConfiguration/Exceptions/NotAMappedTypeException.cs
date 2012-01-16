using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfArt.Exceptions
{
    public class NotAMappedTypeException: Exception
    {
        public ConfArtException TypeException
        {
            get
            {
                return ConfArtException.NotAMappedTypeException;
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
