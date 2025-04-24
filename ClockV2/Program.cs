using ClockV2.Model;
using ClockV2.Presenter;
using ClockV2.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClockV2
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Create the Model, View and Presenter for Alarm
            var alarmView = new AlarmView();
            
            // Create the Model, View and Presenter for Clock
            var model = new ClockModel();
            var view = new ClockView();
            var presenter = new ClockPresenter(model, view);

            //Display Forms
            alarmView.Show();
            Application.Run(view);
        }
    }
}
