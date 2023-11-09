using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Announcement_Platform
{
    public class Courses
    {
        [AutoIncrement,PrimaryKey]
        public int Id { get; set; }
        public string CourseName { get; set; }
        public string CourseOrganizer { get; set; }
        public DateTime CourseDate { get; set; }
    }
}
