using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Announcement_Platform
{
    public class Company
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Localization { get; set; }
    }
}
