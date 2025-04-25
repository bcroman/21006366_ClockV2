using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClockV2.Model
{
    public class Alarm
    {
        //Variables
        public DateTime Time { get; set; }
        public string Label { get; set; }
        public int Priority { get; set; }

        public Alarm(DateTime time, string label = "", int priority = 0)
        {
            Time = time;
            Label = label;
            Priority = priority;
        }

        public int CompareTo(Alarm other)
        {
            return Time.CompareTo(other.Time);
        }

        public override string ToString()
        {
            return $"{Label} - {Time:HH:mm:ss}";
        }
    }
}
