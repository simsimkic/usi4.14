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
using System.Windows.Shapes;
using ZdravoCorp.Controller;
using ZdravoCorp.Model;

namespace ZdravoCorp.View
{
    /// <summary>
    /// Interaction logic for DoctorAddMedicalReport.xaml
    /// </summary>
    public partial class DoctorAddMedicalReport : Window
    {
        Appointment Appointment { get; set; }
        MedicalReport MedicalReport { get; set; }

        public DoctorAddMedicalReport(Appointment appointment)
        {
            Appointment = appointment;
            MedicalReport = Appointment.Report;
            InitializeComponent();
        }

        private void EndMedicalReport(object sender, RoutedEventArgs e)
        {
            MedicalReportController medicalReportController = new MedicalReportController();
            medicalReportController.Delete(MedicalReport);
            MedicalReport.Diagnosis = DiagnosisTextBox.Text;
            medicalReportController.Create(MedicalReport);

            AppointmentController appointmentController = new AppointmentController();
            appointmentController.Delete(Appointment);
            Appointment.Report = MedicalReport;
            appointmentController.Create(Appointment);

            Close();
        }
    }
}
