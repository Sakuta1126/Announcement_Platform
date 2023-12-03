using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Announcement_Platform
{
    public class Lang
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }
        public string Language { get; set; }
        public string LanguageLevel { get; set; }
       
    }
}
