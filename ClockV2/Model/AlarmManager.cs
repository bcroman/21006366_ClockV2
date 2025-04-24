using System;
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
            alarmQueue.Remove();
        }

        public bool IsQueueEmpty()
        {
            return alarmQueue.IsEmpty();
        }

        public override string ToString()
        {
            return alarmQueue.ToString();
        }
    }
}
