using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClockV2.Model
{
    /// <summary>
    /// Represents an alarm with a specific time, label, and priority.
    /// </summary>
    public class Alarm
    {
        //Variables
        public DateTime Time { get; set; }
        public string Label { get; set; }
        public int Priority { get; set; }

        // <summary>
        // Constructor to initialize an alarm with time, label, and priority
        // </summary>
        // <param name="time">The time of the alarm</param>
        // <param name="label"
        // <param name="priority"> The priority of the alarm</param>
        public Alarm(DateTime time, string label = "", int priority = 0)
        {
            Time = time;
            Label = label;
            Priority = priority;
        }

        // <summary>
        // Compares two alarms based on their time#
        // </summary>
        /// <param name="other">The other alarm to compare with</param>
        /// <rutrurns>Returns an integer indicating the relative order of the alarms</returns>
        public int CompareTo(Alarm other)
        {
            return Time.CompareTo(other.Time);
        }

        // <summary>
        // Returns a string representation of the alarm
        // </summary>
        // <returns>A string containing the label and time of the alarm</returns>
        public override string ToString()
        {
            return $"{Label} - {Time:HH:mm:ss}";
        }
    }
}
