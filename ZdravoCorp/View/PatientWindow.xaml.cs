using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using ZdravoCorp.Controller;
using ZdravoCorp.Model;
using ZdravoCorp.Observer;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.View
{
    /// <summary>
    /// Interaction logic for PatientWindow.xaml
    /// </summary>
    public partial class PatientWindow : Window
    { 

        private PatientController _patientController;

        private AppointmentController _appointmentController;

        private NotificationController _notificationController;

        public Patient SelectedPatient { get; set; }

        public PatientWindow(Patient patient, NotificationController notificationController)
        {
            InitializeComponent();
            Title = "Welcome " + patient.Name + "!";
            SelectedPatient = patient;
            _patientController = new PatientController();
            _appointmentController = new AppointmentController();
            _notificationController = notificationController;
        }
        
        private void PatientWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            int unreadNotifications = 0;
            foreach (var notification in SelectedPatient.Notifications)
            {
                if (!notification.IsRead())
                    unreadNotifications++;
            }

            if (unreadNotifications != 0)
                MessageBox.Show($"You have {unreadNotifications} unread notifications.");
        }
        
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show(); 
        }

        private void Logout_Button(object sender, RoutedEventArgs e)
        {
            Close();
        }
        
        private void Notifications_Button(object sender, RoutedEventArgs e)
        {
            NotificationWindow notificationWindow = new NotificationWindow(SelectedPatient, null, _notificationController);
            notificationWindow.Show();
        }

        private void Schedule_Click(object sender, RoutedEventArgs e)
        {
            ScheduleModifyAppointmentWindow scheduleModifyAppointmentWindow = new ScheduleModifyAppointmentWindow(SelectedPatient, new Appointment(), false,_appointmentController,_patientController);
            scheduleModifyAppointmentWindow.ShowDialog();
        }

        private void Crud_Click(object sender, RoutedEventArgs e)
        {
            PatientCrudWindow patientCrudWindow = new PatientCrudWindow(SelectedPatient,_appointmentController,_patientController);
            patientCrudWindow.ShowDialog();
        }

        private void Priority_Schedule_Button_Click(object sender, RoutedEventArgs e)
        {
            PatientPriorityScheduleWindow patientPriorityScheduleWindow=new PatientPriorityScheduleWindow(SelectedPatient,_appointmentController, _patientController);
            patientPriorityScheduleWindow.ShowDialog();
        }

        private void Medical_Record_Button_Click(object sender, RoutedEventArgs e)
        {
            ViewMedicalChartWindow viewMedicalChartWindow = new ViewMedicalChartWindow(SelectedPatient);
            viewMedicalChartWindow.ShowDialog();
        }

        private void Medical_Reports_Button_Click(object sender, RoutedEventArgs e)
        {
            ViewMedicalReportsWindow viewMedicalReportsWindow=new ViewMedicalReportsWindow(SelectedPatient);
            viewMedicalReportsWindow.ShowDialog();
        }
    }
}
