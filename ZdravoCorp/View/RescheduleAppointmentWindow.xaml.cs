using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Automation;
using ZdravoCorp.Controller;
using ZdravoCorp.Model;
using ZdravoCorp.Observer;

namespace ZdravoCorp.View;

public partial class RescheduleAppointmentWindow : Window, IObserver
{
    private  Patient Patient { get; set; }

    private PatientController _patientController;
    
    private bool IsSurgery { get; set; }
    private double AppointmentDuration { get; set; }
    private AppointmentController _appointmentController;
    public ObservableCollection<Appointment> Appointments { get; set; }
    public Appointment SelectedAppointment { get; set; }
    private List<KeyValuePair<Appointment, DateTime>> AppointmentsWithRescheduleTimes;
    private List<Appointment> EarliestReschedulableAppointments;

    private DoctorController _doctorController;

    private NotificationController _notificationController;

    public RescheduleAppointmentWindow(Patient patient, PatientController patientController, bool isSurgery, double appointmentDuration, AppointmentController appointmentController, DoctorController doctorController, List<Doctor> capableDoctors, NotificationController notificationController)
    {
        InitializeComponent();

        Patient = patient;

        _patientController = patientController;

        IsSurgery = isSurgery;

        AppointmentDuration = appointmentDuration;
        
        _appointmentController = appointmentController;
        _appointmentController.Subscribe(this);

        _doctorController = doctorController;

        _notificationController = notificationController;
        
        List<KeyValuePair<Appointment, DateTime>> appointmentsWithRescheduleTimes =
            new List<KeyValuePair<Appointment, DateTime>>();
        foreach (var capableDoctor in capableDoctors)
        {
            appointmentsWithRescheduleTimes.AddRange(capableDoctor.GetReschedulableAppointments(appointmentDuration, _appointmentController));
        }

        AppointmentsWithRescheduleTimes = appointmentsWithRescheduleTimes;
        
        AppointmentsWithRescheduleTimes.Sort((firstAppointment, secondAppointment) => firstAppointment.Value.CompareTo(secondAppointment.Value));

        List<Appointment> earliestReschedulableAppointments = new List<Appointment>();
        int numberOfAppointments =
            AppointmentsWithRescheduleTimes.Count > 5 ? 5 : AppointmentsWithRescheduleTimes.Count;
        for (int i = 0; i < numberOfAppointments; i++)
            earliestReschedulableAppointments.Add(AppointmentsWithRescheduleTimes[i].Key);
        EarliestReschedulableAppointments = earliestReschedulableAppointments;

        Appointments = new ObservableCollection<Appointment>(EarliestReschedulableAppointments);
    }

    private void RescheduleBtn_OnClick(object sender, RoutedEventArgs e)
    {
        if (SelectedAppointment == null)
        {
            MessageBox.Show("You must select an appointment to reschedule.");
            return;
        }
        
        _appointmentController.Delete(SelectedAppointment);

        DateTime oldAppointmentDateTime = SelectedAppointment.Timeslot.DateTime;
        
        _appointmentController.Create(new Appointment(SelectedAppointment.Doctor, Patient, IsSurgery, new Timeslot(SelectedAppointment.Timeslot.DateTime, AppointmentDuration), Appointment.AppointmentStatus.Urgent));

        Notification doctorUrgentNotification = new Notification($"You have been scheduled for an urgent appointment on {SelectedAppointment.Timeslot.DateTime}, lasting {AppointmentDuration}, for patient {Patient.Name} {Patient.Surname}.");
        _notificationController.Create(doctorUrgentNotification);
        
        _doctorController.Delete(SelectedAppointment.Doctor);
        SelectedAppointment.Doctor.Notifications.Add(doctorUrgentNotification);
        _doctorController.Create(SelectedAppointment.Doctor);

        SelectedAppointment.Timeslot.DateTime = AppointmentsWithRescheduleTimes.Find(appointmentWithRescheduleTime => appointmentWithRescheduleTime.Key == SelectedAppointment).Value;
        _appointmentController.Create(SelectedAppointment);

        Notification doctorNotification =
            new Notification(
                $"Your appointment on {oldAppointmentDateTime}, lasting {SelectedAppointment.Timeslot.DurationInMinutes}, for patient {SelectedAppointment.Patient.Name} {SelectedAppointment.Patient.Surname}, has been moved to {SelectedAppointment.Timeslot.DateTime}.");
        _notificationController.Create(doctorNotification);
        
        _doctorController.Delete(SelectedAppointment.Doctor);
        SelectedAppointment.Doctor.Notifications.Add(doctorNotification);
        _doctorController.Create(SelectedAppointment.Doctor);

        Notification patientNotification = new Notification($"Your appointment on {oldAppointmentDateTime}, lasting {SelectedAppointment.Timeslot.DurationInMinutes}, with doctor {SelectedAppointment.Doctor.Name} {SelectedAppointment.Doctor.Surname}, has been moved to {SelectedAppointment.Timeslot.DateTime}.");
        _notificationController.Create(patientNotification);
        
        _patientController.Delete(SelectedAppointment.Patient);
        SelectedAppointment.Patient.Notifications.Add(patientNotification);
        _patientController.Create(SelectedAppointment.Patient);

        MessageBox.Show("You have succesfully rescheduled the appointment.");
        Close();
    }

    private void CancelBtn_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void UpdateAppointmentsList()
    {
        Appointments.Clear();
        foreach (var appointment in EarliestReschedulableAppointments)
        {
            Appointments.Add(appointment);
        }
    }

    public void Update()
    {
        UpdateAppointmentsList();
    }
}