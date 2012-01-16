using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfArtTests.SampleData
{
    public static class UserHelper
    {
        public static User GetUserOne()
        {
            User user = new User();
            user.Id = 1;
            user.Name = "TheOne";
            user.Birth = DateTime.Now.AddMonths(-1000);
            return user;
        }

        public static User GetUserRui()
        {
            User user = new User();
            user.Id = 100;
            user.Name = "Rui";
            DateTime birth;
            DateTime.TryParse("1975/04/24", out birth);
            user.Birth = birth;
            return user;
        }
    }
}
