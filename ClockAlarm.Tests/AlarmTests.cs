using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClockV2.Model;
using NUnit.Framework;

namespace ClockAlarm.Tests
{
    /// <summary>
    ///  Tests Scrpt that checks each function and error handling for each queue method
    /// </summary>
    [TestFixture]
    public class ClockAlarm{

        //variables
        private AlarmManager alarmManager;

        // <summary>
        // SetUp method to initialize the AlarmManager before each test
        // </summary>
        [SetUp]
        public void SetUp()
        {
            alarmManager = new AlarmManager();
        }

        // <summary>
        // Test adding an alarm to the queue
        // </summary>
        [Test]
        public void AddAlarm_And_GetNextAlarm_ReturnsCorrectAlarm()
        {
            // Input Data
            var alarm1 = new Alarm(new DateTime(2025, 4, 25, 8, 0, 0), "Morning Alarm", 1);
            var alarm2 = new Alarm(new DateTime(2025, 4, 25, 9, 0, 0), "Meeting Alarm", 2);

            alarmManager.AddAlarm(alarm1, alarm1.Priority);
            alarmManager.AddAlarm(alarm2, alarm2.Priority);

            // Call GetNextAlarm and set to result
            var result = alarmManager.GetNextAlarm();

            // Check if the output is the highest priority alarm
            Assert.That(result, Is.EqualTo(alarm2));
        }

        // <summary>
        // Test if the next alarm is removed correctly
        // </summary>
        [Test]
        public void RemoveNextAlarm_UpdatesNextAlarmProperly()
        {
            // Input Data
            var alarm1 = new Alarm(new DateTime(2025, 4, 25, 8, 0, 0), "Morning Alarm", 1);
            var alarm2 = new Alarm(new DateTime(2025, 4, 25, 9, 0, 0), "Meeting Alarm", 2);

            alarmManager.AddAlarm(alarm1, alarm1.Priority);
            alarmManager.AddAlarm(alarm2, alarm2.Priority);

            // Remove the next alarm
            alarmManager.RemoveNextAlarm();

            // Call GetNextAlarm and set to result
            var result = alarmManager.GetNextAlarm();

            // Check if the output is the next highest priority alarm
            Assert.That(result, Is.EqualTo(alarm1));
        }

        // <summary>
        // Test if the alarm manager is empty
        // </summary>
        [Test]
        public void IsQueueEmpty_ReturnsTrue_WhenNoAlarms()
        {
            // Call IsQueueEmpty and check if output is true
            Assert.That(alarmManager.IsQueueEmpty(), Is.True);
        }

        // <summary>
        // Test if the alarm manager is not empty after adding alarms
        // </summary>
        [Test]
        public void IsQueueEmpty_ReturnsFalse_WhenAlarmsExist()
        {
            // Input Data
            var alarm = new Alarm(new DateTime(2025, 4, 25, 8, 0, 0), "Morning Alarm", 1);

            alarmManager.AddAlarm(alarm, alarm.Priority);

            // Call IsQueueEmpty and check if output is false
            Assert.That(alarmManager.IsQueueEmpty(), Is.False);
        }

        // <summary>
        // Test removing an alarm from an empty queue throws an exception
        // </summary>
        [Test]
        public void RemoveNextAlarm_ThrowsException_WhenQueueIsEmpty()
        {
            // Call RemoveNextAlarm and check for exception
            Assert.Throws<InvalidOperationException>(() => alarmManager.RemoveNextAlarm());
        }

        // <summary>
        // Test adding alarms with the same priority
        // </summary>
        [Test]
        public void AddAlarm_WithSamePriority_HandlesCorrectly()
        {
            // Input Data
            var alarm1 = new Alarm(new DateTime(2025, 4, 25, 8, 0, 0), "Alarm 1", 1);
            var alarm2 = new Alarm(new DateTime(2025, 4, 25, 8, 30, 0), "Alarm 2", 1);

            alarmManager.AddAlarm(alarm1, alarm1.Priority);
            alarmManager.AddAlarm(alarm2, alarm2.Priority);

            // Call GetNextAlarm and set to result
            var result = alarmManager.GetNextAlarm();

            // Check if the output is the first added alarm with the same priority
            Assert.That(result, Is.EqualTo(alarm1));
        }
    }
}

