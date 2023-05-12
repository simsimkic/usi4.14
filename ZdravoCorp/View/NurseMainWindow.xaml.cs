using System.Collections.ObjectModel;
using System.Windows;
using ZdravoCorp.Controller;
using ZdravoCorp.Model;
using ZdravoCorp.Observer;

namespace ZdravoCorp.View;

public partial class NurseMainWindow : Window, IObserver
{
    private AppointmentController _appointmentController;
    
    public ObservableCollection<Patient> Patients { get; set; }
    public Patient SelectedPatient { get; set; }
    private PatientController _patientController;

    private DoctorController _doctorController;

    private NotificationController _notificationController;

    public NurseMainWindow(Nurse nurse, DoctorController doctorController)
    {
        InitializeComponent();
        Title = $"Welcome, {nurse.Name} {nurse.Surname}!";

        DataContext = this;

        _appointmentController = new AppointmentController();

        _patientController = new PatientController();
        _patientController.Subscribe(this);

        _doctorController = doctorController;

        _notificationController = new NotificationController();

        Patients = new ObservableCollection<Patient>(_patientController.GetAllPatients());
    }

    private void LogOutBtn_OnClick(object sender, RoutedEventArgs e)
    {
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        Close();
    }

    private void AddPatientBtn_OnClick(object sender, RoutedEventArgs e)
    {
        CreatePatientWindow createPatientWindow = new CreatePatientWindow(_patientController);
        createPatientWindow.Show();
    }
    
    private void AddUrgentPatientBtn_OnClick(object sender, RoutedEventArgs e)
    {
        CreateUrgentPatientWindow createUrgentPatientWindow = new CreateUrgentPatientWindow(_patientController);
        createUrgentPatientWindow.Show();
    }
    
    private void ModifyPatientBtn_OnClick(object sender, RoutedEventArgs e)
    {
        if (SelectedPatient == null)
        {
            MessageBox.Show("You must pick a patient in order to modify.");
            return;
        }
        
        ModifyPatientWindow modifyPatientWindow = new ModifyPatientWindow(SelectedPatient, _patientController);
        modifyPatientWindow.Show();
    }
    
    private void DeletePatientBtn_OnClick(object sender, RoutedEventArgs e)
    {
        if (SelectedPatient == null)
        {
            MessageBox.Show("You must pick a patient in order to delete.");
            return;
        }
        
        MessageBoxResult result = ConfirmPatientDeletion();

        if (result == MessageBoxResult.Yes)
            _patientController.Delete(SelectedPatient);
    }
    
    private void ViewMedicalChartBtn_OnClick(object sender, RoutedEventArgs e)
    {
        if (SelectedPatient == null)
        {
            MessageBox.Show("You must pick a patient in order to review their medical chart.");
            return;
        }
        
        ViewMedicalChartWindow viewMedicalChartWindow = new ViewMedicalChartWindow(SelectedPatient);
        viewMedicalChartWindow.Show();
    }
    
    private void CheckInBtn_OnClick(object sender, RoutedEventArgs e)
    {
        if (SelectedPatient == null)
        {
            MessageBox.Show("You must select a patient in order to check them in.");
        }
        else
        {
            PatientCheckInWindow patientCheckInWindow = new PatientCheckInWindow(SelectedPatient, _patientController);
            patientCheckInWindow.Show();
        }
    }

    private void ScheduleUrgentAppointmentBtn_OnClick(object sender, RoutedEventArgs e)
    {
        if (SelectedPatient == null)
        {
            MessageBox.Show("You need to select a patient in order to schedule them for an urgent appointment.");
            return;
        }

        ScheduleUrgentAppointmentWindow scheduleUrgentAppointmentWindow = new ScheduleUrgentAppointmentWindow(SelectedPatient, _patientController, _appointmentController, _doctorController, _notificationController);
        scheduleUrgentAppointmentWindow.Show();
    }

    private MessageBoxResult ConfirmPatientDeletion()
    {
        string sMessageBoxText = $"Are you sure you want to delete {SelectedPatient.Name} {SelectedPatient.Surname}?";
        string sCaption = "Confirm deletion";

        MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
        MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

        MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
        return result;
    }

    private void UpdatePatientsList()
    {
        Patients.Clear();
        foreach (var patient in _patientController.GetAllPatients())
        {
            Patients.Add(patient);
        }
    }

    public void Update()
    {
        UpdatePatientsList();
    }

}