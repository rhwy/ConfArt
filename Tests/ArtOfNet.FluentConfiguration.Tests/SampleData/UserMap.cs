using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConfArt.Core;

namespace ConfArtTests.SampleData
{
    public class UserMap: ConfigurationMappingsBase<User>
    {
        public UserMap()
        {
            Map(u => u.Name).With("test_name");
            Map(u => u.Id).With("test_id");
            Map(u => u.Birth).With("test_birth");
        }
    }
}
