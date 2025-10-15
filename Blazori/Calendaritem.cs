using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectOriented_Template
{
    //public record CalendarItem(DateTime Date, string Name);
    public class CalendarItem
    {
        public DateTime Date { get; set; }
        public string Name { get; set; }

        public CalendarItem(DateTime date, string name)
        {
            Date = date;
            Name = name;
        }
    }
}