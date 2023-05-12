using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ZdravoCorp.Controller;
using ZdravoCorp.Model;

namespace ZdravoCorp.View;

public partial class ScheduleUrgentAppointmentWindow : Window
{
    private Patient Patient { get; set; }

    private PatientController _patientController;

    private AppointmentController _appointmentController;

    private DoctorController _doctorController;

    private NotificationController _notificationController;
    
    public ScheduleUrgentAppointmentWindow(Patient patient, PatientController patientController, AppointmentController appointmentController, DoctorController doctorController, NotificationController notificationController)
    {
        InitializeComponent();

        Patient = patient;

        _patientController = patientController;

        _appointmentController = appointmentController;

        _doctorController = doctorController;

        _notificationController = notificationController;
        
        SpecialtyComboBox.ItemsSource = Enum.GetValues(typeof(Doctor.DoctorSpecialties)).Cast<Doctor.DoctorSpecialties>();
    }

    private void ScheduleBtn_OnClick(object sender, RoutedEventArgs e)
    {
        if (SpecialtyComboBox.SelectedItem == null)
        {
            MessageBox.Show("You must select a doctor specialty in order to schedule the appointment.");
            return;
        }

        double appointmentDuration;
        if (IsSurgeryCheckBox.IsChecked == true)
        {
            if (!double.TryParse(DurationTextBox.Text, out appointmentDuration))
            {
                MessageBox.Show("You must input a valid appointment duration.");
                return;
            }
        }
        else
        {
            appointmentDuration = 15;
        }
        
        var doctorSpecialty = (Doctor.DoctorSpecialties)SpecialtyComboBox.SelectedItem;
        
        var capableDoctors = _doctorController.GetDoctorsBySpecialty(doctorSpecialty);
        if (capableDoctors.Count == 0)
        {
            MessageBox.Show("No doctors of such specialty exist in the system.");
            return;
        }

        Dictionary<Doctor, DateTime> doctorAvailabilities = new Dictionary<Doctor, DateTime>();
        foreach (var capableDoctor in capableDoctors)
        {
            doctorAvailabilities.Add(capableDoctor, capableDoctor.GetEarliestAppointmentAvailability(appointmentDuration, _appointmentController));
        }

        var earliestDoctorAvailability = doctorAvailabilities.MinBy(doctorTimePair => doctorTimePair.Value);
        if (earliestDoctorAvailability.Value > DateTime.Now.AddHours(2))
        {
            MessageBox.Show("No doctor is available within the next two hours.\nRedirecting to rescheduling window.");
            
            RescheduleAppointmentWindow rescheduleAppointmentWindow = new RescheduleAppointmentWindow(Patient, _patientController, IsSurgeryCheckBox.IsChecked == true, appointmentDuration, _appointmentController, _doctorController, capableDoctors, _notificationController);
            rescheduleAppointmentWindow.Show();
            
            Close();
            return;
        }

        _appointmentController.Create(new Appointment(earliestDoctorAvailability.Key, Patient, IsSurgeryCheckBox.IsChecked == true, new Timeslot(earliestDoctorAvailability.Value, appointmentDuration), Appointment.AppointmentStatus.Urgent));
        
        Notification doctorNotification =
            new Notification(
                $"You have been scheduled for an urgent appointment on {earliestDoctorAvailability.Value}, lasting {appointmentDuration}, for patient {Patient.Name} {Patient.Surname}.");
        _notificationController.Create(doctorNotification);
        
        _doctorController.Delete(earliestDoctorAvailability.Key);
        earliestDoctorAvailability.Key.Notifications.Add(doctorNotification);
        _doctorController.Create(earliestDoctorAvailability.Key);
        
        MessageBox.Show($"You have successfully scheduled an urgent appointment for {Patient.Name} {Patient.Surname}.");
        Close();
    }

    private void CancelBtn_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}