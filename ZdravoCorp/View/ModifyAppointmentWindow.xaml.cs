using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using ZdravoCorp.Controller;
using ZdravoCorp.Model;

namespace ZdravoCorp.View
{
    /// <summary>
    /// Interaction logic for ModifyAppointmentWindow.xaml
    /// </summary>
    public partial class ModifyAppointmentWindow : Window
    {
        public Doctor Doctor { get; set; }
        public Appointment Appointment { get; set; }

        public PatientController _patientController { get; set; }

        private NotificationController _notificationController;
        public ModifyAppointmentWindow(Doctor doctor, Appointment appointment, NotificationController notificationController)
        {
            Doctor = doctor;
            _patientController = new PatientController();
            _notificationController = notificationController;
            Appointment = appointment;

            InitializeComponent();

            ObservableCollection<Patient> patients = new ObservableCollection<Patient>(_patientController.GetAllPatients());
            List<string> patientEmails = new List<string>();
            foreach (Patient patient in patients)
            {
                patientEmails.Add(patient.Email);
            }
            PatientEmailComboBox.ItemsSource = patientEmails;
        }

        public void Save(object sender, RoutedEventArgs e)
        {
            AppointmentController appointmentController = new AppointmentController();
            ObservableCollection<Appointment> appointments = new ObservableCollection<Appointment>(appointmentController.GetAllAppointments());

            foreach (Appointment appointment1 in appointments)
            {
                if (appointment1.Id == Appointment.Id)
                {
                    appointmentController.Delete(appointment1);
                    break;
                }
            }

            Patient patient = _patientController.GetPatientByEmail(PatientEmailComboBox.Text);

            DateTime date = Date.SelectedDate.Value.Date;
            DateTime time = DateTime.ParseExact(TimeTextBox.Text, "H:m", CultureInfo.InvariantCulture);
            DateTime dateTime = dateTime = date.Add(time.TimeOfDay);
            bool isSurgery = Convert.ToBoolean(IsSurgeryComboBox.Text);
            double duration = Convert.ToDouble(DurationTextBox.Text);
            Timeslot ts = new Timeslot(dateTime, duration);

            if(patient==null)
            {
                MessageBox.Show("Patient with that id doesn't exist!");
                return;
            }

            if(!patient.IsAvailable(ts))
            {
                MessageBox.Show("Patient not available at that time!");
                return;
            }

            if (!Doctor.IsAvailable(ts))
            {
                MessageBox.Show("You are not available at that time!");
                return;
            }

            if (!isSurgery && duration!=15)
            {
                MessageBox.Show("If appointment is not a surgery then it has to last 15 minutes!");
                return;
            }

            Appointment appointment = new Appointment();
            appointment.Doctor = Doctor;
            appointment.Patient = patient;
            appointment.IsSurgery = isSurgery;
            appointment.Timeslot = ts;

            appointmentController.Create(appointment);

            DoctorAppointmentView dav = new DoctorAppointmentView(Doctor, _notificationController);
            dav.Show();

            Close();
        }
    }
}
