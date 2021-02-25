using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IAS_Handyman.Models
{
    public class WeeklyHoursTechnicianReport
    {
        public int YearWeek { get; set; }
        public DateTime WeekStartDate { get; set; } 
        public DateTime WeekEndDate { get; set; }
        // Monday to saturday between 07:00 AM and 08:00 PM
        public int NormalHours { get; set; } = 0;
        // Monday to saturday between 08:00 PM and 07:00 AM
        public int NightHours { get; set; } = 0;
        // Worked days on sunday
        public int SundayHours { get; set; } = 0;
        // Monday to saturday between 07:00 AM and 08:00 PM after 48 hours of week work
        public int ExtraNormalHours { get; set; } = 0;
        // Monday to saturday between 08:00 PM and 07:00 AM after 48 hours of week work
        public int ExtraNightHours { get; set; } = 0;
        // Worked days on sunday after 48 hous of week work
        public int ExtraSundayHours { get; set; } = 0;
        public string TechnicianIdentification { get; set; }
        public String TechnicianName { get; set; }
    }
}