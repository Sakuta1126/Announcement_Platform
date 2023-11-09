using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Announcement_Platform
{
    public class Education
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }
        public string SchoolName { get; set; }
        public string Localization { get; set; }
        public string EducationLevel {get;set;}
        public string Course { get; set; }
        public DateTime EducationPaeriodStart { get; set; }
        public DateTime EducationPaeriodEnd { get; set; }
        public Boolean StillLearning { get; set; }
    }
}
