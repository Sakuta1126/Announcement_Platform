using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Announcement_Platform
{
    public class Occupation
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public Boolean Working { get; set; }

        public string WorkDescription { get; set; }

    }
}
