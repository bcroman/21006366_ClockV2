using System;
using System.Collections.Generic;
using PriorityQueue;

namespace ClockV2.Model
{
    public class AlarmManager
    {
        private PriorityQueue<Alarm> alarmQueue;

        public AlarmManager()
        {
            // Initialize with implementation of SortedArrayPriorityQueue
            alarmQueue = new SortedArrayPriorityQueue<Alarm>(10);
        }

        public void AddAlarm(Alarm alarm, int priority)
        {
            alarmQueue.Add(alarm, priority);
        }

        public Alarm GetNextAlarm()
        {
            return alarmQueue.Head();
        }

        public void RemoveNextAlarm()
        {
            try
            {
                alarmQueue.Remove();
            }
            catch (QueueUnderflowException ex)
            {
                throw new InvalidOperationException("Cannot remove from an empty alarm queue.", ex);
            }
        }

        public bool IsQueueEmpty()
        {
            return alarmQueue.IsEmpty();
        }

        // New method to retrieve all alarms
        public List<Alarm> GetAllAlarms()
        {
            List<Alarm> alarms = new List<Alarm>();
            while (!alarmQueue.IsEmpty())
            {
                alarms.Add(alarmQueue.Head());
                alarmQueue.Remove();
            }

            // Re-add alarms to maintain the queue
            foreach (var alarm in alarms)
            {
                alarmQueue.Add(alarm, 0); // Assuming priority is 0 for simplicity
            }

            return alarms;
        }

        public override string ToString()
        {
            return alarmQueue.ToString();
        }
    }
}
