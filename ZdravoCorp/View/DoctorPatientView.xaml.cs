using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
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
    /// Interaction logic for DoctorPatientView.xaml
    /// </summary>
    public partial class DoctorPatientView : Window
    {
        Doctor Doctor { get; set; }
        PatientController _patientController;

        private NotificationController _notificationController;

        public DoctorPatientView(Doctor doctor, NotificationController notificationController)
        {
            this.Doctor = doctor;
            InitializeComponent();

            _patientController = new PatientController();
            ObservableCollection<Patient> patients = new ObservableCollection<Patient>(_patientController.GetAllPatients());

            foreach (Patient patient in patients)
            {
                PatientDataGrid.Items.Add(patient);
            }

            _notificationController = notificationController;
        }

        public void OpenMedicalRecord(object sender, RoutedEventArgs e)
        {
            Patient patient = new Patient();
            if (PatientDataGrid.Visibility == Visibility.Visible)
            {
                patient = (Patient)PatientDataGrid.SelectedItem;
            } else
            {
                patient = (Patient)PatientDataGridAfterSearch.SelectedItem;
            }

            AppointmentController appointmentController = new AppointmentController();
            
            // ako jeste bio njegov pacijent nekad pre moze da vidi medical record ako ne onda ne moze
            if (appointmentController.IsDoctorsPatient(Doctor.Email, patient.Email))
            {
                DoctorMedicalRecordView dmrv = new DoctorMedicalRecordView(patient, Doctor, true, _notificationController);
                dmrv.Show();
                Close();
            }
            else
            {
                MessageBox.Show("You can't view a medical record of a patient that you haven't seen before.");
            }
        }

        private void SearchPatients(object sender, RoutedEventArgs e)
        {
            List<Patient> patients = new List<Patient>();
            if (EmailTextBox.Text != "") // no id, has email
            {
                Patient patient = _patientController.GetPatientByEmail(EmailTextBox.Text);
                patients.Add(patient);
            }
            else if (NameTextBox.Text != "" && SurnameTextBox.Text != "") // no id, no email, has name, has surname
            {
                patients = _patientController.GetPatientsByNameSurname(NameTextBox.Text, SurnameTextBox.Text);
            }
            else if (NameTextBox.Text != "") // only name
            {
                patients = _patientController.GetPatientsByName(NameTextBox.Text);
            }
            else if (SurnameTextBox.Text != "") // only surname
            {
                patients = _patientController.GetPatientsBySurname(SurnameTextBox.Text);
            }
            else // nothing
            {
                MessageBox.Show("No such patient exists.");
                return;
            }

            if(patients.Count == 0)
            {
                MessageBox.Show("No such patient exists.");
                return;
            }

            foreach (Patient patient in patients)
            {
                PatientDataGridAfterSearch.Items.Add(patient);
            }

            PatientDataGridAfterSearch.Visibility = Visibility.Visible;
            RemoveSearchButton.Visibility = Visibility.Visible;

            PatientDataGrid.Visibility = Visibility.Hidden;

            NameTextBox.Visibility = Visibility.Hidden;
            SurnameTextBox.Visibility = Visibility.Hidden;
            EmailTextBox.Visibility = Visibility.Hidden;

            NameLabel.Visibility = Visibility.Hidden;
            SurnameLabel.Visibility = Visibility.Hidden;
            EmailLabel.Visibility = Visibility.Hidden;

            SearchButton.Visibility = Visibility.Hidden;
            GoBackButton.Visibility = Visibility.Hidden;
        }

        private void RemoveSearch(object sender, RoutedEventArgs e)
        {

            PatientDataGridAfterSearch.Items.Clear();

            PatientDataGridAfterSearch.Visibility = Visibility.Hidden;
            RemoveSearchButton.Visibility = Visibility.Hidden;

            PatientDataGrid.Visibility = Visibility.Visible;

            NameTextBox.Visibility = Visibility.Visible;
            SurnameTextBox.Visibility = Visibility.Visible;
            EmailTextBox.Visibility = Visibility.Visible;

            NameLabel.Visibility = Visibility.Visible;
            SurnameLabel.Visibility = Visibility.Visible;
            EmailLabel.Visibility = Visibility.Visible;

            SearchButton.Visibility = Visibility.Visible;
            GoBackButton.Visibility = Visibility.Visible;
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            DoctorWindow doctorWindow = new DoctorWindow(Doctor, _notificationController);
            doctorWindow.Show();
            Close();
        }
    }
}
