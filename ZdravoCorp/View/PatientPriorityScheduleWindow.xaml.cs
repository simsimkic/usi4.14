using System;
using System.Collections;
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
    /// Interaction logic for PatientPriorityScheduleWindow.xaml
    /// </summary>
    public partial class PatientPriorityScheduleWindow : Window
    {
        public ObservableCollection<Doctor> Doctors { get; set; }
        private Patient currentPatient;
        private PatientController _patientController;
        private AppointmentController _appointmentController;
        private DoctorController _doctorController;
        public PatientPriorityScheduleWindow(Patient patient, AppointmentController appointmentController, PatientController patientController)
        {
            InitializeComponent();
            DataContext = this;
            _patientController = patientController;
            _appointmentController = appointmentController;
            _doctorController = new DoctorController();
            Doctors = new ObservableCollection<Doctor>(_doctorController.GetAllDoctors());
            currentPatient = patient;
            PatientLogEntry patientLogEntry = new PatientLogEntry();
            patientLogEntry.RemoveAllExpiredEntries();
        }
        Appointment FindAppointmentWithDoctorPriority(ref bool found,ref ObservableCollection<Appointment> appointments)
        {
            DateTime floatingDate = DateTime.Now.AddDays(1).Date;
            while (floatingDate.Date < DatePicker.SelectedDate)
            {
                while (floatingDate.TimeOfDay < DateTime.ParseExact("23:59:59", "HH:mm:ss", null).TimeOfDay)
                {

                    Timeslot timeslot = new Timeslot(floatingDate, 15);
                    if (appointments.Count == 0)
                    {
                        Appointment newAppointment = new Appointment();
                        newAppointment.Doctor = Doctor.GetDoctorByEmail(DoctorPicker.SelectedValue.ToString());
                        newAppointment.Patient = currentPatient;
                        newAppointment.IsSurgery = false;
                        newAppointment.Timeslot = timeslot;
                        newAppointment.Status = Appointment.AppointmentStatus.Scheduled;
                        found = true;
                        appointments.Add(newAppointment);
                        return newAppointment;
                    }
                    bool overlapFlag = false;
                    foreach (Appointment appointment in appointments)
                    {
                        if (appointment.Timeslot.IsOverlapping(timeslot))
                        {
                            overlapFlag = true;
                            break;
                        }
                    }
                    if (!overlapFlag)
                    {
                        Appointment newAppointment = new Appointment();
                        newAppointment.Doctor = Doctor.GetDoctorByEmail(DoctorPicker.SelectedValue.ToString());
                        newAppointment.Patient = currentPatient;
                        newAppointment.IsSurgery = false;
                        newAppointment.Timeslot = timeslot;
                        newAppointment.Status = Appointment.AppointmentStatus.Scheduled;
                        found = true;
                        appointments.Add(newAppointment);
                        return newAppointment;
                    }
                    floatingDate = floatingDate.AddMinutes(15);
                }
                if (found)
                {
                    break;
                }
                floatingDate = floatingDate.AddDays(1).Date;
            }
            return new Appointment();
        }
        Appointment FindAppointmentWithTimePriority(ref bool found,DateTime start,DateTime end,ref ObservableCollection<ObservableCollection<Appointment>> appointments, ref ObservableCollection<Doctor> doctors)
        {
            DateTime floatingDate = DateTime.Now.AddDays(1).Date;
            while (floatingDate.Date < DatePicker.SelectedDate)
            {
                floatingDate = floatingDate.Add(start.TimeOfDay);
                while (floatingDate.TimeOfDay < end.TimeOfDay)
                {
                    Timeslot timeslot = new Timeslot(floatingDate, 15);
                    if (appointments.Count == 0)
                    {
                        Appointment newAppointment = new Appointment();
                        newAppointment.Doctor = Doctor.GetDoctorByEmail(DoctorPicker.SelectedValue.ToString());
                        newAppointment.Patient = currentPatient;
                        newAppointment.IsSurgery = false;
                        newAppointment.Timeslot = timeslot;
                        newAppointment.Status = Appointment.AppointmentStatus.Scheduled;
                        found = true;
                        return newAppointment;
                    }
                    else
                    {
                        int doctorCounter = 0;
                        foreach (ObservableCollection<Appointment> doctorAppointments in appointments)
                        {
                            string currentDoctorEmail = doctors[doctorCounter].Email;
                            if (doctorAppointments.Count == 0)
                            {
                                Appointment newAppointment = new Appointment();
                                newAppointment.Doctor = Doctor.GetDoctorByEmail(currentDoctorEmail);
                                newAppointment.Patient = currentPatient;
                                newAppointment.IsSurgery = false;
                                newAppointment.Timeslot = timeslot;
                                newAppointment.Status = Appointment.AppointmentStatus.Scheduled;
                                found = true;
                                doctorAppointments.Add(newAppointment);
                                return newAppointment;
                            }
                            bool overlapFlag = false;
                            foreach (Appointment appointment in doctorAppointments)
                            {
                                if (appointment.Timeslot.IsOverlapping(timeslot))
                                {
                                    overlapFlag = true;
                                    break;
                                }
                            }
                            if (!overlapFlag)
                            {
                                Appointment newAppointment = new Appointment();
                                newAppointment.Doctor = Doctor.GetDoctorByEmail(currentDoctorEmail);
                                newAppointment.Patient = currentPatient;
                                newAppointment.IsSurgery = false;
                                newAppointment.Timeslot = timeslot;
                                newAppointment.Status = Appointment.AppointmentStatus.Scheduled;
                                found = true;
                                doctorAppointments.Add(newAppointment);
                                return newAppointment;
                            }
                            doctorCounter++;
                        }
                        if (found)
                        {
                            break;
                        }
                        floatingDate = floatingDate.AddMinutes(15);
                    }
                }
                if (found)
                {
                    break;
                }
                floatingDate = floatingDate.AddDays(1).Date;
            }
            return new Appointment();
        }
        private void Schedule_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool found=false;
                DateTime start = DateTime.ParseExact(StartTime.Text, "HH:mm:ss",null);
                DateTime end = DateTime.ParseExact(EndTime.Text, "HH:mm:ss", null);
                if(start>end)
                {
                    MessageBox.Show("Incorrectly entered start and end of desired time");
                }
                else
                {
                    if (DatePicker.SelectedDate < DateTime.Now)
                    {
                        MessageBox.Show("You need to select a deadline that is not in the past or today!");
                    }
                    else
                    {
                        if (RadioDoctor.IsChecked==true)
                        {
                            ObservableCollection<Appointment> doctorAppointments = new ObservableCollection<Appointment>(_appointmentController.GetAppointmentsForDoctor(DoctorPicker.SelectedValue.ToString()));
                            Appointment foundAppointment=FindAppointmentWithDoctorPriority(ref found,ref doctorAppointments);
                            if (found)
                            {
                                _appointmentController.Create(foundAppointment);
                                PatientLogEntry newLog = new PatientLogEntry(currentPatient.Email, foundAppointment.Id, AppointmentStatus.Scheduled, DateTime.Today);
                                newLog.AddLog();
                                currentPatient.CheckBlocking(_patientController);
                                MessageBox.Show("Completed");
                            }
                        }
                        else
                        {
                            ObservableCollection<ObservableCollection<Appointment>> appointments = new ObservableCollection<ObservableCollection<Appointment>>();
                            ObservableCollection<Doctor> doctors = new ObservableCollection<Doctor>(_doctorController.GetAllDoctors());
                            foreach (Doctor doctor in doctors)
                            {
                                ObservableCollection<Appointment> tempAppointments = new ObservableCollection<Appointment>(_appointmentController.GetAppointmentsForDoctor(doctor.Email));
                                appointments.Add(tempAppointments);
                            }
                            Appointment foundAppointment=FindAppointmentWithTimePriority(ref found, start, end,ref appointments,ref doctors);
                            if (found)
                            {
                                _appointmentController.Create(foundAppointment);
                                PatientLogEntry newLog = new PatientLogEntry(currentPatient.Email, foundAppointment.Id, AppointmentStatus.Scheduled, DateTime.Today);
                                newLog.AddLog();
                                currentPatient.CheckBlocking(_patientController);
                                MessageBox.Show("Completed");
                            }
                        }
                        if (!found)
                        {
                            ObservableCollection<Appointment> appointmentChoiceList = new ObservableCollection<Appointment>();
                            found = true;
                            if (RadioDoctor.IsChecked == true)
                            {
                                ObservableCollection<ObservableCollection<Appointment>> appointments = new ObservableCollection<ObservableCollection<Appointment>>();
                                ObservableCollection<Doctor> doctors = new ObservableCollection<Doctor>(_doctorController.GetAllDoctors());
                                foreach (Doctor doctor in doctors)
                                {
                                    ObservableCollection<Appointment> tempAppointments = new ObservableCollection<Appointment>(_appointmentController.GetAppointmentsForDoctor(doctor.Email));
                                    appointments.Add(tempAppointments);
                                }
                                for (int i = 0; i < 3; i++)
                                {
                                    Appointment foundAppointment = FindAppointmentWithTimePriority(ref found, start, end,ref appointments,ref doctors);
                                    MessageBox.Show(foundAppointment.Doctor.Email + " " + foundAppointment.Timeslot.DateTime.ToString());
                                    appointmentChoiceList.Add(foundAppointment);
                                }
                            }
                            else
                            {
                                ObservableCollection<Appointment> doctorAppointments = new ObservableCollection<Appointment>(_appointmentController.GetAppointmentsForDoctor(DoctorPicker.SelectedValue.ToString()));
                                for (int i = 0; i < 3; i++)
                                {  
                                    Appointment foundAppointment = FindAppointmentWithDoctorPriority(ref found,ref doctorAppointments);
                                    MessageBox.Show(foundAppointment.Doctor.Email + " " + foundAppointment.Timeslot.DateTime.ToString());
                                    appointmentChoiceList.Add(foundAppointment);
                                }
                            }
                            AppointmentChoiceWindow appointmentChoiceWindow = new AppointmentChoiceWindow(_patientController, _appointmentController, appointmentChoiceList);
                            appointmentChoiceWindow.ShowDialog();
                            Close();
                        }
                        else
                        {
                            Close();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Abandon_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
