using System;
using System.Globalization;
using System.Windows;
using System.Windows.Documents;
using ZdravoCorp.Model;
using ZdravoCorp.Serializer;
using System.Collections.Generic;
using ZdravoCorp.Controller;
using System.Collections.ObjectModel;
using System.Linq;

namespace ZdravoCorp.View
{
    /// <summary>
    /// Interaction logic for AddApointmentWindow.xaml
    /// </summary>
    public partial class AddApointmentWindow : Window
    {
    public Doctor Doctor { get; set; }

    public PatientController _patientController { get; set; }

    private NotificationController _notificationController;
    public AddApointmentWindow(Doctor doctor, NotificationController notificationController)
    {
        Doctor = doctor;
        _patientController = new PatientController();
        InitializeComponent();

        ObservableCollection<Patient> patients = new ObservableCollection<Patient>(_patientController.GetAllPatients());
        List<string> patientEmails = new List<string>();
        foreach (Patient patient in patients)
        {
            patientEmails.Add(patient.Email);
        }
        PatientEmailComboBox.ItemsSource = patientEmails;

        _notificationController = notificationController;
    }

    public void Save(object sender, RoutedEventArgs e)
    {
        //
        //TODO: Patient should be fetched by email, the id is automatically generated
        //
        PatientController patientController = new PatientController();
        Patient patient = patientController.GetPatientByEmail(PatientEmailComboBox.Text);

        DateTime date = Date.SelectedDate.Value.Date;
        DateTime time = DateTime.ParseExact(TimeTextBox.Text, "H:m", CultureInfo.InvariantCulture);
        DateTime dateTime = dateTime = date.Add(time.TimeOfDay);
        bool isSurgery = Convert.ToBoolean(IsSurgeryComboBox.Text);
        double duration = Convert.ToDouble(DurationTextBox.Text);
        Timeslot ts = new Timeslot(dateTime, duration);
        
        if (patient == null)
        {
            MessageBox.Show("Patient with that id doesn't exist!");
            return;
        }

        if (!patient.IsAvailable(ts))
        {
            MessageBox.Show("Patient not available at that time!");
            return;
        }

        if (!Doctor.IsAvailable(ts))
        {
            MessageBox.Show("You are not available at that time!");
            return;
        }

        if (!isSurgery && duration != 15)
        {
            MessageBox.Show("If appointment is not a surgery then it has to last 15 minutes!");
            return;
        }

        AppointmentController appointmentController = new AppointmentController();

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

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
    }
}
