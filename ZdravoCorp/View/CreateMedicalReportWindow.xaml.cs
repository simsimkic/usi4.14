using System;
using System.Linq;
using System.Windows;
using ZdravoCorp.Controller;
using ZdravoCorp.Model;

namespace ZdravoCorp.View;

public partial class CreateMedicalReportWindow
{
    private Patient Patient { get; set; }
    private PatientController _patientController;
    
    private Appointment Appointment { get; set; }
    private AppointmentController _appointmentController;

    public CreateMedicalReportWindow(Patient patient, PatientController patientController, Appointment appointment, AppointmentController appointmentController)
    {
        InitializeComponent();

        Patient = patient;

        _patientController = patientController;
        
        Appointment = appointment;

        _appointmentController = appointmentController;
    }

    private void CreateBtn_OnClick(object sender, RoutedEventArgs e)
    {
        MedicalReport medicalReport =
            new MedicalReport(DateTime.Now, SymptomsTextBox.Text.Split(',').ToList());
        
        MedicalReportController medicalReportController = new MedicalReportController();
        medicalReportController.Create(medicalReport);
        
        _patientController.Delete(Patient);
        Patient.MedicalRecord.Reports.Add(medicalReport);
        _patientController.Create(Patient);
        
        _appointmentController.Delete(Appointment);
        Appointment.Report = medicalReport;
        _appointmentController.Create(Appointment);
        
        DialogResult = true;
    }
    
    private void CancelBtn_OnClick(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }
}
