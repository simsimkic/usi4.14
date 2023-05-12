using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZdravoCorp.Controller;
using ZdravoCorp.Model;

namespace ZdravoCorp.View
{
    /// <summary>
    /// Interaction logic for StartAppointmentWindow.xaml
    /// </summary>
    public partial class StartAppointmentWindow : Window
    {
        Patient Patient { get; set; }

        PatientController _patientController { get; set; }

        private NotificationController _notificationController;

        Appointment Appointment { get; set; }

        Doctor Doctor { get; set; }
        public StartAppointmentWindow(Patient patient, Appointment appointment, Doctor doctor, NotificationController notificationController)
        {

            _patientController = new PatientController();
            Patient = _patientController.GetPatientByEmail(patient.Email);
            _notificationController = notificationController;
            Appointment = appointment;
            Doctor = doctor;
            InitializeComponent();

            NameLabel.Content = Patient.Name.ToString();
            EmailLabel.Content = Patient.Email.ToString();
            SurnameLabel.Content = Patient.Surname.ToString();
            AddressLabel.Content = Patient.Address.ToString();
            DateOfBirthLabel.Content = Patient.DateOfBirth.ToString();
        }

        private void ViewMedicalRecord(object sender, RoutedEventArgs e)
        {
            DoctorMedicalRecordView dmrv = new DoctorMedicalRecordView(Patient, Doctor, false, _notificationController);
            dmrv.Show();
        }

        private void WriteMedicalReport(object sender, RoutedEventArgs e)
        {
            DoctorAddMedicalReport doctorAddMedicalReport = new DoctorAddMedicalReport(Appointment);
            doctorAddMedicalReport.Show();
        }

        private void EndAppointment(object sender, RoutedEventArgs e)
        {
            DoctorUpdateEquipment doctorUpdateEquipment = new DoctorUpdateEquipment();
            doctorUpdateEquipment.Show();
            Close();
        }
    }
}
