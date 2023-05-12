using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZdravoCorp.Controller;
using ZdravoCorp.View;
using ZdravoCorp.Model;
using ZdravoCorp.Serializer;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ManagerController _managerController;
        private DoctorController _doctorController;
        private PatientController _patientController;
        private NurseController _nurseController;
        private NotificationController _notificationController;
        
        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            _managerController = new ManagerController();
            _doctorController = new DoctorController();
            _patientController = new PatientController();
            _nurseController = new NurseController();
            _notificationController = new NotificationController();
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Manager manager = _managerController.GetManagerByEmail(EmailTextBox.Text);
            Doctor doctor = _doctorController.GetDoctorByEmail(EmailTextBox.Text);
            Patient patient = _patientController.GetPatientByEmail(EmailTextBox.Text);
            Nurse nurse = _nurseController.GetNurseByEmail(EmailTextBox.Text);
            if (manager != null)
            {
                if (manager.Password != PasswordBox.Password)
                {
                    MessageBox.Show("Wrong password.");
                }
                else
                {
                    ManagerMainWindow managerMainWindow = new ManagerMainWindow();
                    managerMainWindow.Show();
                    Close();
                }
            }
            else if (doctor != null)
            {
                if (doctor.Password != PasswordBox.Password)
                {
                    MessageBox.Show("Wrong password.");
                }
                else    
                {
                    DoctorWindow doctorWindow = new DoctorWindow(doctor, _notificationController);
                    doctorWindow.Show();
                    Close(); 
                }
            }
            else if (patient != null)
            {
                if (patient.Password != PasswordBox.Password)
                {
                    MessageBox.Show("Wrong password.");
                }
                else if (patient.Blocked)
                {
                    MessageBox.Show("User is blocked.");
                }
                else
                {
                    PatientWindow patientWindow = new PatientWindow(patient, _notificationController);
                    patientWindow.Show();
                    Close();
                }
            }
            else if (nurse != null)
            {
                if (nurse.Password != PasswordBox.Password)
                {
                    MessageBox.Show("Wrong password.");
                }
                else
                {
                    NurseMainWindow nurseMainWindow = new NurseMainWindow(nurse, _doctorController);
                    nurseMainWindow.Show();
                    Close();
                }
            }
            else
            {
                MessageBox.Show("No such account exists in the system.");
            }
        }
    }
}
