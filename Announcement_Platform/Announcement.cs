using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Announcement_Platform
{
    public class Announcement
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }
        public string PositionName { get; set; }
        public string PositionLevel { get; set; }
        public string ContractType { get; set; }
        public string Company { get; set; }
        public string Localization { get; set; }
        public string WorkingDimension { get; set; }
        public string Workingtype { get; set; }
        public decimal Sallary { get; set; }
        public string WorkingDays { get; set; }
        public string WorkingHours { get; set; }
        public DateTime AnnouncementStart { get; set; }
        public DateTime AnnouncementEnd { get; set; }
        public string Category { get; set; }
        public string Duties { get; set; }
        public string Requirements { get; set; }
        public string Benefits { get; set; }
        public string AboutCompany { get; set; }


    }
}
