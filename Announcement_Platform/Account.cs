using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Announcement_Platform
{
    public class Account
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public Boolean IsAdmin { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsLoggedIn { get; set; }

        public Account(int id,bool isadmin, string login, string password, bool isLoggedIn)
        {
            this.Id = id;
            this.IsAdmin = isadmin;
            this.Login = login;
            this.Password = password;
            IsLoggedIn = isLoggedIn;
        }
        public Account( bool isadmin, string login, string password, bool isLoggedIn)
        {
           
            this.IsAdmin = isadmin;
            this.Login = login;
            this.Password = password;
            this.IsLoggedIn = isLoggedIn;
        }
        public Account()
        {

           
        }
    }
    
}
