using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using ZdravoCorp.Controller;
using ZdravoCorp.Model.DAO;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Model;

public class Patient : User, ISerializable
{
    public string Address { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public bool Blocked { get; set; }
    public MedicalRecord MedicalRecord { get; set; }
    public List<Notification> Notifications { get; set; }

    public Patient()
    {
        MedicalRecord = new MedicalRecord();
        Notifications = new List<Notification>();
    }
    
    public Patient(string email, string password, string name, string surname, string address, DateOnly dateOfBirth, MedicalRecord medicalRecord) : base(email, password, name, surname)
    {
        Address = address;
        DateOfBirth = dateOfBirth;
        Blocked = false;
        MedicalRecord = medicalRecord;
        Notifications = new List<Notification>();
    }
    
    public Patient(Patient patient)
    {
        Email = patient.Email;
        Password = patient.Password;
        Name = patient.Name;
        Surname = patient.Surname;
        DateOfBirth = patient.DateOfBirth;
        Address = patient.Address;
        MedicalRecord = new MedicalRecord();
        MedicalRecord.Height = patient.MedicalRecord.Height;
        MedicalRecord.Weight = patient.MedicalRecord.Weight;
        MedicalRecord.PastConditions =  new List<string>(patient.MedicalRecord.PastConditions);
        MedicalRecord.Allergies = new List<string>(patient.MedicalRecord.Allergies);
        Notifications = new List<Notification>();
    }
    
    //Constructor for creating an urgent patient account
    public Patient(string name, string surname, DateOnly dateOfBirth, MedicalRecord medicalRecord)
    {
        long unixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        Email = "Guest" + unixTimestamp;
        Password = "Guest" + unixTimestamp;
        Name = name;
        Surname = surname;
        Address = "";
        DateOfBirth = dateOfBirth;
        MedicalRecord = medicalRecord;
        Notifications = new List<Notification>();
    }

    public static Patient GetPatientByEmail(string email)
    {
        Serializer<Patient> patientSerializer = new Serializer<Patient>();
        List<Patient> patients = patientSerializer.FromCSV(@"..\..\..\Data\Patients.csv");

        foreach (Patient p in patients)
        {
            if (p.Email == email)
            {
                return p;
            }
        }

        return null;
    }
    
    public bool IsAvailable(Timeslot t)
    {
        AppointmentController appointmentController = new AppointmentController();
        ObservableCollection<Appointment> appointments = new ObservableCollection<Appointment>(appointmentController.GetAllAppointments());

        if (appointments != null)
        { 
            foreach (Appointment a in appointments)
            {
                if (t.IsOverlapping(a.Timeslot))
                {
                    return false;
                }
            }
        }

        return true;
    }

    public string[] ToCSV()
    {
        string medicalReportIds = "";
        foreach (var medicalReport in MedicalRecord.Reports)
        {
            medicalReportIds += medicalReport.Id + ",";
        }
        medicalReportIds = medicalReportIds.TrimEnd(',');

        string notificationIds = "";
        foreach (var notification in Notifications)
        {
            notificationIds += notification.Id + ",";
        }
        notificationIds = notificationIds.TrimEnd(',');
        
        string[] csvValues = { Email, Password, Name, Surname, Address, DateOfBirth.ToString(), Blocked.ToString(), MedicalRecord.Height.ToString(), MedicalRecord.Weight.ToString(), string.Join(',', MedicalRecord.PastConditions), string.Join(',', MedicalRecord.Allergies), medicalReportIds, notificationIds };
        return csvValues;
    }

    public void FromCSV(string[] values)
    {
        Email = values[0];
        Password = values[1];
        Name = values[2];
        Surname = values[3];
        Address = values[4];
        DateOfBirth = DateOnly.Parse(values[5]);
        Blocked = Convert.ToBoolean(values[6]);
        
        List<MedicalReport> medicalReports = new List<MedicalReport>();
        if (values[11] != "")
        {
            string[] medicalReportIds = values[11].Split(',');
            MedicalReportController medicalReportController = new MedicalReportController();
            foreach (var medicalReportId in medicalReportIds)
            {
                medicalReports.Add(medicalReportController.GetMedicalReportById(int.Parse(medicalReportId)));
            }
        }
        
        MedicalRecord = new MedicalRecord(float.Parse(values[7]), float.Parse(values[8]), values[9].Split(',').ToList(), values[10].Split(",").ToList(), medicalReports);

        List<Notification> notifications = new List<Notification>();
        if (values[12] != "")
        {
            string[] notificationIds = values[12].Split(',');
            NotificationController notificationController = new NotificationController();
            foreach (var notificationId in notificationIds)
            {
                notifications.Add(notificationController.GetNotificationById(int.Parse(notificationId)));
            }
            Notifications = notifications;
        }
    }
    
    public List<Appointment> getFinishedAppointments()
    {
        List<Appointment> newAppointments = new List<Appointment>();
        AppointmentController _controller = new AppointmentController();
        List<Appointment> appointments = new List<Appointment>(_controller.GetAppointmentsForPatient(Email));
        foreach (Appointment appointment in appointments)
        {
            if (appointment.Status == Appointment.AppointmentStatus.Finished)
            {
                newAppointments.Add(appointment);
            }
        }
        return newAppointments;
    }
    
    public List<Referral> GetExaminationReferrals(string patientEmail)
    {
        List<Referral> examinationReferrals = new List<Referral>();
        //TODO:Add for loop when DAO for Referrals is added
        return examinationReferrals;
    }
    public List<TreatmentReferral>GetTreatmentReferrals(string patientEmail)
    {
        List<TreatmentReferral> treatmentReferrals = new List<TreatmentReferral>();
        //TODO:Add for loop when DAO for treatment Referrals is added
        return treatmentReferrals;
    }
    public void CheckBlocking(PatientController _controller)
    {
        List<PatientLogEntry> logList = new List<PatientLogEntry>();
        Serializer<PatientLogEntry> LogSerializer = new Serializer<PatientLogEntry>();
        logList = LogSerializer.FromCSV(@"..\..\..\Data\PatientLog.csv");

        int modifyCounter = 0, scheduleCounter = 0;
        foreach (PatientLogEntry log in logList)
        {
            if (log.PatientEmail == Email)
            {
                if (log.Status == AppointmentStatus.Modified || log.Status == AppointmentStatus.Cancelled)
                {
                    modifyCounter++;
                }
                else
                {
                    scheduleCounter++;
                }
            }
        }
        if (modifyCounter >= 5 || scheduleCounter >= 8)
        {
            _controller.Delete(this);
            Blocked = true;
            MessageBox.Show("You are blocked");
            _controller.Create(this);
        }
    }
}
