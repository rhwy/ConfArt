using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ConfArtTests
{
    public abstract class TestFixtureBase
    {
        public void Log(object obj)
        {
            System.Diagnostics.Debug.WriteLine(obj);
        }

        public void Log(object obj, string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
            Log(obj);
        }

        public void Log(IEnumerable list)
        {
            foreach (var item in list)
            {
                System.Diagnostics.Debug.WriteLine(item);
            }
        }

        public void Log(IEnumerable list, string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
            Log(list);
        }
    }
}
