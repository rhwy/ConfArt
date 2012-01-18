using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfArtTests.SampleData
{
    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        public List<string> Cities { get; set; }

        public Contact()
        {
            Cities = new List<string>();
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            Contact compared = (Contact)obj;
            if (compared == null)
            {
                return false;
            }
            return
                (Id == compared.Id) && (Name == compared.Name) && (Company == compared.Company);
        }
    }
}
