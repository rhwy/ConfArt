using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfArtTests.SampleData
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Birth { get; set; }

        public override string ToString()
        {
            return string.Format("Id: {0},Name: {1},Birth: {2}", Id, Name, Birth);
        }
    }
}
