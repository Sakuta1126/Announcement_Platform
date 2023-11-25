using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Announcement_Platform
{
    public class WorkingExperience
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Position { get; set; }
        public string CompanyName { get; set; }
        public string Localization { get; set; }
        public DateTime WorkingPeriodStart { get; set; }
        public DateTime WorkingPeriodEnd { get; set; }
        public string Duties { get; set; }
      


    }
}
