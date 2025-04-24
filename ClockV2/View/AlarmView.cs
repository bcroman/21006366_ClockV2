using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClockV2.Model;

namespace ClockV2.View
{
    public partial class AlarmView : Form
    {
        private AlarmManager alarmManager;
        private ClockModel clockModel;
        private Timer alarmTimer;

        public AlarmView(ClockModel model)
        {
            InitializeComponent();

            // Corrected parameter usage
            clockModel = model;

            // Initialize alarm manager
            alarmManager = new AlarmManager();

            // Initialize and configure the timer
            alarmTimer = new Timer();
            alarmTimer.Interval = 1000; // 1 second
            alarmTimer.Tick += AlarmTimer_Tick;
            alarmTimer.Start();
        }

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
        

        private void btn_delete_Click(object sender, EventArgs e)
        {
                alarmManager.RemoveNextAlarm(); // Assuming the selected alarm is the next one
                RefreshAlarmList();
        }

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

        private void RefreshAlarmList()
        {
            lb_alarmList.Text = alarmManager.ToString();
        }

        
    }
}
