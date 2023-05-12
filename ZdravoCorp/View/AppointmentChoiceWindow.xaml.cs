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
    /// Interaction logic for AppointmentChoiceWindow.xaml
    /// </summary>
    public partial class AppointmentChoiceWindow : Window
    {
        public ObservableCollection<Appointment> Appointments { get; set; }

        private PatientController _patientController;

        private AppointmentController _appointmentController;
        public Appointment SelectedAppointment { get; set; }
        public AppointmentChoiceWindow(PatientController patientController,AppointmentController appointmentController,ObservableCollection<Appointment> appointments)
        {
            InitializeComponent();
            DataContext = this;
            _patientController = patientController;
            _appointmentController = appointmentController;
            Appointments = appointments;
            MessageBox.Show("Choose one");
        }

        private void Select_Click(object sender, RoutedEventArgs e)
        {
            _appointmentController.Create(SelectedAppointment);
            MessageBox.Show("Completed");
            PatientLogEntry newLog = new PatientLogEntry(SelectedAppointment.Patient.Email, SelectedAppointment.Id, AppointmentStatus.Scheduled, DateTime.Today);
            newLog.AddLog();
            SelectedAppointment.Patient.CheckBlocking(_patientController);
            Close();
           
        }
    }
}
