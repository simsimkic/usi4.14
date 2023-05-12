using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Collections.Generic;
using ZdravoCorp.Controller;
using ZdravoCorp.Model;
using ZdravoCorp.Observer;

namespace ZdravoCorp.View;

public partial class PatientCheckInWindow : Window, IObserver
{
    private Patient Patient { get; set; }

    private PatientController _patientController;
    
    private AppointmentController _appointmentController;
    private List<Appointment> patientAppointments { get; set; }
    public ObservableCollection<Appointment> Appointments { get; set; }
    public Appointment SelectedAppointment { get; set; }

    public PatientCheckInWindow(Patient patient, PatientController patientController)
    {
        InitializeComponent();

        DataContext = this;

        Patient = patient;

        _patientController = patientController; 

        _appointmentController = new AppointmentController();
        _appointmentController.Subscribe(this);
        
        patientAppointments = _appointmentController.GetAppointmentsForPatient(Patient.Email);
        Appointments = new ObservableCollection<Appointment>(patientAppointments);
        AppointmentsGrid.ItemsSource = Appointments;
    }
    
    private void CheckInBtn_OnClick(object sender, RoutedEventArgs e)
    {
        if (SelectedAppointment == null)
        {
            MessageBox.Show("You need to select the appointment which you are checking the patient in for.");
        }
        else
        {
            if (SelectedAppointment.Timeslot.DateTime.Subtract(DateTime.Now).TotalMinutes > 15)
            {
                MessageBox.Show("This appointment isn't within 15 minutes of starting, so the patient cannot be checked in yet.");
                return;
            }
            if (SelectedAppointment.Timeslot.DateTime.Subtract(DateTime.Now).TotalMinutes < -5)
            {
                MessageBox.Show("The patient is late to this appointment and can no longer be checked in.");
                return;
            }
            
            CreateMedicalReportWindow createMedicalReportWindow = new CreateMedicalReportWindow(Patient, _patientController, SelectedAppointment, _appointmentController);

            if (createMedicalReportWindow.ShowDialog() == false)
            {
                Close();
                return;
            }
            
            var checkedInAppointment = SelectedAppointment;
            _appointmentController.Delete(SelectedAppointment);
            checkedInAppointment.Status = Appointment.AppointmentStatus.CheckedIn;
            _appointmentController.Create(checkedInAppointment);
            
            Close();
        }
    }
    
    private void CancelBtn_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
    
    private void UpdateAppointmentsList()
    {
        Appointments.Clear();
        foreach (var patient in patientAppointments)
        {
            Appointments.Add(patient);
        }
    }

    public void Update()
    {
        UpdateAppointmentsList();
    }

}