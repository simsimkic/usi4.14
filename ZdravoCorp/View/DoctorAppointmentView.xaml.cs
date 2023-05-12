using System.Collections.Generic;
using System.Windows;
using ZdravoCorp.Serializer;
using ZdravoCorp.Model;
using ZdravoCorp.Controller;
using System.Collections.ObjectModel;
using System;

namespace ZdravoCorp.View
{
    /// <summary>
    /// Interaction logic for DoctorAppointmentView.xaml
    /// </summary>
    public partial class DoctorAppointmentView : Window
    {
        public Doctor Doctor { get; set; }

        public PatientController _patientController { get; set; }

        private NotificationController _notificationController;
        public DoctorAppointmentView(Doctor doctor, NotificationController notificationController)
        {
            Doctor = doctor;
            _patientController = new PatientController();
            InitializeComponent();

            //Serializer<Appointment> appointmentSerializer = new Serializer<Appointment>();
            //List<Appointment> appointments = appointmentSerializer.FromCSV(@"..\..\..\Data\Appointments.csv");

            AppointmentController _controller = new AppointmentController();
            ObservableCollection<Appointment> appointments = new ObservableCollection<Appointment>(_controller.GetAppointmentsForDoctor(Doctor.Email));

            foreach(Appointment appointment in appointments)
            {
                if (appointment.Doctor.Email == doctor.Email && appointment.Timeslot.IsAfter(DateTime.Now))
                {
                    AppointmentGrid.Items.Add(appointment);
                }
            }

            _notificationController = notificationController;
        }

        private void ModifyAppointment(object sender, RoutedEventArgs e)
        {
            Appointment appointment = (Appointment) AppointmentGrid.SelectedItem;

            ModifyAppointmentWindow maw = new ModifyAppointmentWindow(Doctor, appointment, _notificationController);
            maw.Show();
            Close();
        }

        private void DeleteAppointment(object sender, RoutedEventArgs e)
        {
            Appointment appointment = (Appointment)AppointmentGrid.SelectedItem;
            AppointmentController appointmentController = new AppointmentController();
            ObservableCollection<Appointment> appointments = new ObservableCollection<Appointment>(appointmentController.GetAllAppointments());
            foreach(Appointment appointment1 in appointments)
            {
                if(appointment1.Id==appointment.Id)
                {
                    appointmentController.Delete(appointment1);
                    break;
                }
            }
            AppointmentGrid.Items.Remove(appointment);
        }

        private void AddApointment(object sender, RoutedEventArgs e)
        {

            AddApointmentWindow aaw = new AddApointmentWindow(Doctor, _notificationController);
            aaw.Show();
            Close();
        }

        public void StartAppointment(object sender, RoutedEventArgs e)
        {
            Appointment appointment = (Appointment)AppointmentGrid.SelectedItem;

            StartAppointmentWindow saw = new StartAppointmentWindow(appointment.Patient, appointment, Doctor, _notificationController);
            saw.Show();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            DoctorWindow doctorWindow = new DoctorWindow(Doctor, _notificationController);
            doctorWindow.Show();
            Close();
        }
    }
}
