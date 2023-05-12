using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for DoctorMedicalRecordView.xaml
    /// </summary>
    public partial class DoctorMedicalRecordView : Window
    {
        public Patient Patient { get; set; }

        public Patient PatientForChanges { get; set; }

        private Doctor Doctor { get; set; }

        private NotificationController _notificationController;

        bool CalledFromPatientView;
        public DoctorMedicalRecordView(Patient patient, Doctor doctor, bool calledFromPatientView, NotificationController notificationController)
        {
            Patient = patient;
            Doctor = doctor;

            PatientForChanges = new Patient(patient);

            CalledFromPatientView = calledFromPatientView;

            InitializeComponent();
            
            PatientEmailLabel.Content = patient.Email;

            WeightTextBox.Text = patient.MedicalRecord.Weight.ToString();
            HeightTextBox.Text = patient.MedicalRecord.Height.ToString();

            PastConditionsComboBox.ItemsSource = patient.MedicalRecord.PastConditions.ToList();
            AllergiesComboBox.ItemsSource = patient.MedicalRecord.Allergies.ToList();

            _notificationController = notificationController;
        }

        private void AddCondition(object sender, RoutedEventArgs e)
        {
            string condition = AddConditionTextBox.Text;
            if(!PatientForChanges.MedicalRecord.PastConditions.Contains(condition))
            {
                PatientForChanges.MedicalRecord.PastConditions.Add(condition);
                MessageBox.Show("Added condition " + condition + ".");
            } else
            {
                MessageBox.Show("Condition " + condition + " already in list.");
            }
            
        }

        private void RemoveCondition(object sender, RoutedEventArgs e)
        {
            string condition = RemoveConditionTextBox.Text;
            bool foundCondition = PatientForChanges.MedicalRecord.PastConditions.Remove(condition);
            if(foundCondition)
            {
                MessageBox.Show("Removed condition " + condition + ".");
            } else
            {
                MessageBox.Show("No such condition in list.");
            }
        }

        private void AddAllergy(object sender, RoutedEventArgs e)
        {
            string allergy = AddAllergyTextBox.Text;
            if (!PatientForChanges.MedicalRecord.Allergies.Contains(allergy))
            {
                PatientForChanges.MedicalRecord.Allergies.Add(allergy);
                MessageBox.Show("Added allergy " + allergy + ".");
            }
            else
            {
                MessageBox.Show("Allergy " + allergy + " already in list.");
            }

        }
        private void RemoveAllergy(object sender, RoutedEventArgs e)
        {
            string allergy = RemoveAllergyTextBox.Text;
            bool foundAllergy = PatientForChanges.MedicalRecord.Allergies.Remove(allergy);
            if (foundAllergy)
            {
                MessageBox.Show("Removed allergy " + allergy + ".");
            }
            else
            {
                MessageBox.Show("No such allergy in list.");
            }
        }

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            PatientController patientController = new PatientController();
            ObservableCollection<Patient> patients = new ObservableCollection<Patient>(patientController.GetAllPatients());

            foreach (Patient patient in patients)
            {
                if(patient.Email == Patient.Email)
                {
                    patientController.Delete(patient);
                    break;
                }
            }
            PatientForChanges.MedicalRecord.Height = float.Parse(HeightTextBox.Text);
            PatientForChanges.MedicalRecord.Weight = float.Parse(WeightTextBox.Text);
            patientController.Create(PatientForChanges);
            
            MessageBox.Show("Changes saved.");

            if (CalledFromPatientView)
            {
                DoctorPatientView doctorPatientView = new DoctorPatientView(Doctor, _notificationController);
                doctorPatientView.Show();
            }

            Close();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            if(CalledFromPatientView)
            {
                DoctorPatientView doctorPatientView = new DoctorPatientView(Doctor, _notificationController);
                doctorPatientView.Show();
            }

            PatientController patientController = new PatientController();
            patientController.Create(PatientForChanges);
            
            MessageBox.Show("Changes saved.");
            Close();
        }
    }
}
