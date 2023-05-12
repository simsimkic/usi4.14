using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using ZdravoCorp.Controller;
using ZdravoCorp.Model;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.View
{
    public partial class ScheduleModifyAppointmentWindow : Window
    {
        public ObservableCollection<Doctor> Doctors { get; set; }
        private Appointment currentAppointment;
        private Patient currentPatient;
        private PatientController _patientController;
        private AppointmentController _appointmentController;
        private DoctorController _doctorController;
        public ScheduleModifyAppointmentWindow(Patient patient, Appointment appointment, bool isModifier,AppointmentController appointmentController,PatientController patientController)
        {
            InitializeComponent();
            try
            {
                DataContext = this;
                _patientController = patientController;
                _appointmentController = appointmentController;
                _doctorController= new DoctorController();
                Doctors= new ObservableCollection<Doctor>(_doctorController.GetAllDoctors());
                PatientName.Text = patient.Email;
                if (isModifier)
                {
                    AppointmentButton.Content = "Modify";
                    PatientName.Text = appointment.Patient.Email;
                    TimePicker.Text = appointment.Timeslot.DateTime.TimeOfDay.ToString();
                    DatePicker.SelectedDate = appointment.Timeslot.DateTime.Date;
                    DoctorPicker.SelectedValue = appointment.Doctor.Email;
                }
                PatientLogEntry patientLogEntry = new PatientLogEntry();
                patientLogEntry.RemoveAllExpiredEntries();
                currentAppointment = appointment;
                currentPatient = patient;
               
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Abandon_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Schedule_Modify_Click(object sender, RoutedEventArgs e)
        {
                ObservableCollection<Appointment> appointments=new ObservableCollection<Appointment>(_appointmentController.GetAllAppointments());
                bool flag = false;
                DateTime date;
                DateTime time= DateTime.ParseExact(TimePicker.Text, "HH:mm:ss", CultureInfo.InvariantCulture);
                DateTime dateTime;
                string pattern = @"^([01][0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$";
                if (Regex.IsMatch(TimePicker.Text, pattern))
                {
                    WarningLabel.Visibility = Visibility.Hidden;
                    if (DatePicker.SelectedDate > DateTime.Now)
                    {
                        date = DatePicker.SelectedDate.Value;
                        dateTime=date.Add(time.TimeOfDay);
                        Timeslot selectedTimeslot = new Timeslot(dateTime, 15);
                        foreach (Appointment appointment in appointments)
                        {

                            if (DoctorPicker.SelectedValue.ToString()==appointment.Doctor.Email)
                            {
                                if (appointment.Timeslot.IsOverlapping(selectedTimeslot))
                                {
                                    if (appointment == currentAppointment) continue;
                                    flag = true;
                                }
                            }
                        }
                        if (!flag)
                        {           
                            PatientLogEntry newLog = new PatientLogEntry(PatientName.Text, 0, AppointmentStatus.Scheduled, DateTime.Today);
                            if (AppointmentButton.Content.ToString() == "Modify")
                            {     
                                _appointmentController.Delete(currentAppointment);
                                newLog.Status = AppointmentStatus.Modified;
                            }
                            currentAppointment.Doctor = Doctor.GetDoctorByEmail(DoctorPicker.SelectedValue.ToString());
                            currentAppointment.Patient = currentPatient;
                            currentAppointment.IsSurgery = false;
                            currentAppointment.Timeslot= selectedTimeslot;
                            currentAppointment.Status = Appointment.AppointmentStatus.Scheduled;
                            _appointmentController.Create(currentAppointment);
                            newLog.AppointmentId = currentAppointment.Id;
                            MessageBox.Show("Completed");
                            newLog.AddLog();
                            currentPatient.CheckBlocking(_patientController);
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("Selected doctor is already scheduled at the selected time");
                        }
                    }
                    else
                    {
                        MessageBox.Show("You cant select a date in the past!");
                    }
                }
                else
                {
                    WarningLabel.Visibility = Visibility.Visible;
                }
            } 
        }

        
    }

