using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Animation;
using ZdravoCorp.Controller;
using ZdravoCorp.Model;
using ZdravoCorp.Observer;

namespace ZdravoCorp.View
{
    /// <summary>
    /// Interaction logic for PatientCrudWindow.xaml
    /// </summary>
    public partial class PatientCrudWindow : Window,IObserver
    {
        public ObservableCollection<Appointment> Appointments { get; set; }

        private PatientController _patientController;

        private AppointmentController _appointmentController;
        public Patient Patient { get; set; }
        public Appointment SelectedAppointment { get; set; }
        public PatientCrudWindow(Patient patient,AppointmentController appointmentController,PatientController patientController)
        {
            InitializeComponent();
            DataContext = this;
            Patient = patient;
            _patientController = patientController;
            _patientController.Subscribe(this);
            _appointmentController = appointmentController;
            _appointmentController.Subscribe(this);
            Appointments = new ObservableCollection<Appointment>(_appointmentController.GetAppointmentsForPatient(patient.Email));
            PatientLogEntry patientLogEntry = new PatientLogEntry();
            patientLogEntry.RemoveAllExpiredEntries();
        }

        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedAppointment.IsModifiable()) 
            {
                ScheduleModifyAppointmentWindow scheduleModifyAppointmentWindow = new ScheduleModifyAppointmentWindow(Patient, SelectedAppointment, true,_appointmentController,_patientController);
                scheduleModifyAppointmentWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("You cant modify an appointment that is one day until due");
            }

        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedAppointment.IsModifiable())
            {
                MessageBoxResult dialogResult = MessageBox.Show("Are you sure?", "Delete Appointment", MessageBoxButton.YesNo);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    MessageBox.Show("Deleting");
                  //  AppointmentGrid.Items.Remove(AppointmentGrid.SelectedItem);
                    PatientLogEntry newLog = new PatientLogEntry(Patient.Email, SelectedAppointment.Id, AppointmentStatus.Cancelled, DateTime.Today);
                    newLog.AddLog();
                    Patient.CheckBlocking(_patientController);
                    _appointmentController.Delete(SelectedAppointment);
                }
            }
            else
            {
                MessageBox.Show("You cant delete an appointment that is one day until due");
            }
        }
        private void UpdateAppointmentsList()
        {
            Appointments.Clear();
            foreach (var appointment in _appointmentController.GetAllAppointments())
            {
                Appointments.Add(appointment);
            }
        }

        public void Update()
        {
            UpdateAppointmentsList();
        }
    }
}
