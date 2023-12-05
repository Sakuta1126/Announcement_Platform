using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using SQLite;
namespace Announcement_Platform
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
        public string PFP { get; set; }
        public string ResidenceAddress { get; set; }
        public string Summary { get; set; }
        public int user_id { get; set; }

    }
}
