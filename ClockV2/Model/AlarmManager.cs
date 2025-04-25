using System;
using System.Collections.Generic;
using PriorityQueue;

namespace ClockV2.Model
{
    /// <summary>
    /// Class to manage the alarms in the clock application.
    /// </summary>
    public class AlarmManager
    {
        /// This class manages a priority queue of alarms.
        private PriorityQueue<Alarm> alarmQueue;

        /// <summary>
        /// 
        public AlarmManager()
        {
            // Initialize with implementation of SortedArrayPriorityQueue
            alarmQueue = new SortedArrayPriorityQueue<Alarm>(10);
        }

        /// <summary>
        /// method to add an alarm to the queue
        /// </summary>
        /// <param name="priority"> The priority of the alarm</param>
        /// <param name="alarm"> The alarm to be added</param>
        public void AddAlarm(Alarm alarm, int priority)
        {
            alarmQueue.Add(alarm, priority);
        }

        /// <summary>
        /// Get the top alarm from the queue
        /// </summary>
        /// <returns> The top alarm in the queue</returns>
        public Alarm GetNextAlarm()
        {
            return alarmQueue.Head();
        }

        /// <summary>
        /// Removes the next alarm from the queue if not, throws an exception
        /// </summary>
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

        /// <summary>
        /// checks if the queue is empty
        /// </summary>
        /// <returns> True if the queue is empty, false otherwise</returns>
        public bool IsQueueEmpty()
        {
            return alarmQueue.IsEmpty();
        }

        /// <summary>
        /// Get All the alarms in the queue and format them into a list
        /// <summary>
        /// <return>A list of all the alarms</return>
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

        /// <summary>
        /// Display the alarms in the queue in a string format
        /// </summary>
        /// <returns> A string representation of the alarms in the queue</returns>
        public override string ToString()
        {
            return alarmQueue.ToString();
        }
    }
}
