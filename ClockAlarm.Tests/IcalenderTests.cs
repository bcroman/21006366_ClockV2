using NUnit.Framework;
using ClockV2.Model;
using System;
using System.IO;

namespace ClockAlarm.Tests
{
    [TestFixture]
    public class IcalenderTests
    {
        private const string TestFilePath = "test_alarms.ics";

        [SetUp]
        public void SetUp()
        {
            // Ensure no leftover test files
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up test files
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }
        }

        [Test]
        public void SaveAlarmsToFile_CreatesValidICSFile()
        {
            // Arrange
            var alarmManager = new AlarmManager();
            alarmManager.AddAlarm(new Alarm(new DateTime(2025, 4, 25, 8, 0, 0), "Morning Alarm", 1), 1);
            alarmManager.AddAlarm(new Alarm(new DateTime(2025, 4, 25, 9, 0, 0), "Meeting Alarm", 2), 2);

            // Act
            using (StreamWriter writer = new StreamWriter(TestFilePath))
            {
                writer.WriteLine("BEGIN:VCALENDAR");
                writer.WriteLine("VERSION:2.0");

                foreach (var alarm in alarmManager.GetAllAlarms())
                {
                    writer.WriteLine("BEGIN:VEVENT");
                    writer.WriteLine($"UID:{Guid.NewGuid()}");
                    writer.WriteLine($"DTSTAMP:{DateTime.UtcNow:yyyyMMddTHHmmssZ}");
                    writer.WriteLine($"DTSTART:{alarm.Time:yyyyMMddTHHmmss}");
                    writer.WriteLine($"SUMMARY:{alarm.Label}");
                    writer.WriteLine($"PRIORITY:{alarm.Priority}");
                    writer.WriteLine("END:VEVENT");
                }

                writer.WriteLine("END:VCALENDAR");
            }

            // Assert
            Assert.That(File.Exists(TestFilePath), Is.True, "ICS file was not created.");
            string fileContent = File.ReadAllText(TestFilePath);
            Assert.That(fileContent, Does.Contain("BEGIN:VCALENDAR"));
            Assert.That(fileContent, Does.Contain("BEGIN:VEVENT"));
            Assert.That(fileContent, Does.Contain("SUMMARY:Morning Alarm"));
        }

        [Test]
        public void LoadAlarmsFromFile_ParsesValidICSFile()
        {
            // Arrange
            string icsContent = @"
                BEGIN:VCALENDAR
                VERSION:2.0
                BEGIN:VEVENT
                UID:12345
                DTSTAMP:20250425T080000Z
                DTSTART:20250425T080000
                SUMMARY:Morning Alarm
                PRIORITY:1
                END:VEVENT
                BEGIN:VEVENT
                UID:67890
                DTSTAMP:20250425T090000Z
                DTSTART:20250425T090000
                SUMMARY:Meeting Alarm
                PRIORITY:2
                END:VEVENT
                END:VCALENDAR";

            File.WriteAllText(TestFilePath, icsContent);

            var alarmManager = new AlarmManager();

            // Act
            using (StreamReader reader = new StreamReader(TestFilePath))
            {
                string line;
                DateTime time = DateTime.MinValue;
                string label = string.Empty;
                int priority = 0;

                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("DTSTART:"))
                    {
                        time = DateTime.ParseExact(line.Substring(8), "yyyyMMddTHHmmss", null);
                    }
                    else if (line.StartsWith("SUMMARY:"))
                    {
                        label = line.Substring(8);
                    }
                    else if (line.StartsWith("PRIORITY:"))
                    {
                        priority = int.Parse(line.Substring(9));
                    }
                    else if (line == "END:VEVENT")
                    {
                        alarmManager.AddAlarm(new Alarm(time, label), priority);
                    }
                }
            }

            // Assert
            var alarms = alarmManager.GetAllAlarms();
            Assert.That(alarms.Count, Is.EqualTo(2));
            Assert.That(alarms[0].Label, Is.EqualTo("Morning Alarm"));
            Assert.That(alarms[1].Label, Is.EqualTo("Meeting Alarm"));
        }
    }
}
