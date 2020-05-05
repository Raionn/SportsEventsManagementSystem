using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportBook.ViewModels
{
    public class ScheduleData
    {
        public string Title { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public string Url { get; set; }
        public string ClassName { get; set; }
        public bool AllDay { get; set; }

        public ScheduleData(string title, DateTime? start, DateTime? end, string url, string className, bool allDay = false)
        {
            AllDay = allDay;
            Title = title;
            Start = start;
            End = end;
            Url = url;
            ClassName = className;
        }
    }
}
