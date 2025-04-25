using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClockV2.Model;

namespace ClockV2.View
{
    /// <summary>
    /// Represents the view for managing alarms in the clock application.
    /// </summary>
    public partial class AlarmView : Form
    {
        // Variables
        private AlarmManager alarmManager;
        private ClockModel clockModel;
        private Timer alarmTimer;
        private const string AlarmFilePath = "alarms.ics";

        /// <summary>
        /// Initializes a new instance of the <see cref="AlarmView"/> class.
        /// </summary>
        /// <param name="model">The clock model providing the current time.</param>
        public AlarmView(ClockModel model)
        {
            InitializeComponent();

            // Corrected parameter usage
            clockModel = model;

            // Initialize alarm manager
            alarmManager = new AlarmManager();

            // Prompt to load alarms
            if (File.Exists(AlarmFilePath) && MessageBox.Show("Do you want to load saved alarms?", "Load Alarms", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                LoadAlarmsFromFile();
            }

            // Initialize and configure the timer
            alarmTimer = new Timer();
            alarmTimer.Interval = 1000; // 1 second
            alarmTimer.Tick += AlarmTimer_Tick;
            alarmTimer.Start();

            this.FormClosing += AlarmView_FormClosing;
        }

        /// <summary>
        /// Handles the timer tick event to check if an alarm should go off.
        /// </summary>
        private void AlarmTimer_Tick(object sender, EventArgs e)
        {
            if (!alarmManager.IsQueueEmpty())
            {
                Alarm nextAlarm = alarmManager.GetNextAlarm();
                DateTime now = clockModel.GetCurrentTime();

                if (now.Hour == nextAlarm.Time.Hour &&
                    now.Minute == nextAlarm.Time.Minute &&
                    now.Second == nextAlarm.Time.Second)
                {
                    // Alert the user
                    MessageBox.Show($"Alarm: {nextAlarm.Label} is going off!", "Alarm Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Remove the alarm from the queue
                    alarmManager.RemoveNextAlarm();

                    // Refresh the alarm list
                    RefreshAlarmList();
                }
            }
        }

        /// <summary>
        /// Handles the click event for the "Add" button to add a new alarm.
        /// </summary>
        private void btn_add_Click(object sender, EventArgs e)
        {
            DateTime time = txt_alarmTime.Value;
            string label = txt_alarmLabel.Text;
            int priority;

            if (string.IsNullOrWhiteSpace(label))
            {
                MessageBox.Show("Please enter a label for the alarm.");
                return;
            }

            if (!int.TryParse(txt_alarmPriority.Text, out priority))
            {
                MessageBox.Show("Please enter a valid numeric priority for the alarm.");
                return;
            }

            Alarm alarm = new Alarm(time, label);
            alarmManager.AddAlarm(alarm, priority);
            RefreshAlarmList();
        }

        /// <summary>
        /// Handles the click event for the "Delete" button to delete the next alarm.
        /// </summary>
        private void btn_delete_Click(object sender, EventArgs e)
        {
            alarmManager.RemoveNextAlarm(); // Assuming the selected alarm is the next one
            RefreshAlarmList();
        }

        /// <summary>
        /// Handles the click event for the "Edit" button to edit the next alarm.
        /// </summary>
        private void btn_edit_Click(object sender, EventArgs e)
        {

            DateTime newTime = txt_alarmTime.Value;
            string newLabel = txt_alarmLabel.Text;
            int newPriority;

            if (string.IsNullOrWhiteSpace(newLabel))
            {
                MessageBox.Show("Please enter a label for the alarm.");
                return;
            }

            if (!int.TryParse(txt_alarmPriority.Text, out newPriority))
            {
                MessageBox.Show("Please enter a valid numeric priority for the alarm.");
                return;
            }

            Alarm selectedAlarm = alarmManager.GetNextAlarm(); // Assuming the selected alarm is the next one
            if (selectedAlarm != null)
            {
                selectedAlarm.Time = newTime;
                selectedAlarm.Label = newLabel;

                alarmManager.RemoveNextAlarm();
                alarmManager.AddAlarm(selectedAlarm, newPriority);

                RefreshAlarmList();
            }
            else
            {
                MessageBox.Show("Failed to retrieve the selected alarm.");
            }
        }

        /// <summary>
        /// Refreshes the alarm list displayed in the view.
        /// </summary>
        private void RefreshAlarmList()
        {
            lb_alarmList.Text = alarmManager.ToString();
        }

        /// <summary>
        /// Handles the form closing event to prompt the user to save alarms.
        /// </summary>
        private void AlarmView_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Prompt to save alarms
            if (MessageBox.Show("Do you want to save your alarms?", "Save Alarms", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                SaveAlarmsToFile();
            }
        }

        /// <summary>
        /// Saves the alarms to a file in iCalendar format.
        /// </summary>
        private void SaveAlarmsToFile()
        {
            using (StreamWriter writer = new StreamWriter(AlarmFilePath))
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
                    writer.WriteLine("BEGIN:VALARM");
                    writer.WriteLine("TRIGGER:-PT0M");
                    writer.WriteLine($"DESCRIPTION:{alarm.Label}");
                    writer.WriteLine("ACTION:DISPLAY");
                    writer.WriteLine("END:VALARM");
                    writer.WriteLine("END:VEVENT");
                }

                writer.WriteLine("END:VCALENDAR");
            }
        }

        /// <summary>
        /// Loads alarms from a file in iCalendar format.
        /// </summary>
        private void LoadAlarmsFromFile()
        {
            using (StreamReader reader = new StreamReader(AlarmFilePath))
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
                        if (!int.TryParse(line.Substring(9), out priority))
                        {
                            priority = 0; // Default to 0 if parsing fails
                        }
                    }
                    else if (line == "END:VEVENT")
                    {
                        alarmManager.AddAlarm(new Alarm(time, label), priority);
                        priority = 0;
                    }
                }
            }
            RefreshAlarmList();
        }

    }
}
