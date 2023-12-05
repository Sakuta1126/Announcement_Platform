using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Announcement_Platform
{
    public static class UserStore
    {
        public static int LoggedInUserId { get; private set; }

        public static void SetLoggedInUserId(int userId)
        {
            LoggedInUserId = userId;
        }
    }
}
