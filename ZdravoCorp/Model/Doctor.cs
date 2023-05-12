using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using ZdravoCorp.Controller;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Model;

public class Doctor : User, ISerializable
{
    public enum DoctorSpecialties
    {
        GeneralPractitioner,
        Immunologist,
        Dermatologist,
        Radiologist,
        Neurologist,
        Gynaecologist,
        Ophtalmologist,
        Pathologist,
        Pediatrician,
        Psychiatrist,
        Oncologist,
        Urologist
    }
    
    //Doctor should not have a list of appointments according to the schema we have agreed on. On the other hand, the appointment should see the doctor that is involved in it.
    public DoctorSpecialties Specialty { get; set; }
    public List<Appointment> Appointments { get; set; }
    public List<Notification> Notifications { get; set; }
    
    public Doctor(string email, string password, string name, string surname, DoctorSpecialties specialty) : base(email, password, name, surname)
    {
        Appointments = new List<Appointment>();
        Specialty = specialty;
        LoadAppointments();
        Specialty = specialty;
        Notifications = new List<Notification>();
    }

    public Doctor() {
        Appointments = new List<Appointment>();
        Notifications = new List<Notification>();
    }

    public bool IsAvailable(Timeslot t)
    {
        //
        // TODO: Refactor this to utilize the appointment controller.
        // is doctor not doing anything else at that time
        //
        LoadAppointments();
        if (Appointments != null)
        {
            foreach (Appointment a in this.Appointments)
            {
                if (t.IsOverlapping(a.Timeslot))
                {
                    return false;
                }
            }
        }

        return true;
    }

    public DateTime GetEarliestAppointmentAvailability(double appointmentDuration, AppointmentController appointmentController)
    {
        var doctorAppointments = appointmentController.GetAppointmentsForDoctor(Email);
        if (doctorAppointments.Count == 0) // If the doctor has no scheduled appointments, they are immediately available
            return DateTime.Now;

        if (IsAvailable(new Timeslot(DateTime.Now, appointmentDuration))) // If the doctor has scheduled appointments, check if they are available before any of them.
            return DateTime.Now;

        doctorAppointments.Sort((firstAppointment, nextAppointment) => firstAppointment.Timeslot.DateTime.CompareTo(nextAppointment.Timeslot.DateTime)); // Sort all appointments of the doctor by their starting time.
                                                                                                                                                         // Since appointments cannot overlap, this will sort all the doctor's appointments chronologically.
        foreach (var doctorAppointment in doctorAppointments)
        {
            var currentAppointmentTimeslot = doctorAppointment.Timeslot;
            var nextAppointmentTime = currentAppointmentTimeslot.DateTime
                .AddMinutes(currentAppointmentTimeslot.DurationInMinutes).AddSeconds(1);
            if (IsAvailable(new Timeslot(nextAppointmentTime, appointmentDuration))) // Check if the doctor will be available after any of their appointments for the duration of the sent appointment.
                                                                                     // Since the appointments are sorted chronologically, this will ensure that I get the earliest availability for the doctor, for the sent appointment.
                                                                                     // Since time has no end, and a doctor cannot be scheduled indefinitely, this will, in the worst case scenario, return the time right after the doctors last appointment.
            {
                return nextAppointmentTime; // This will always return a value. In the worst case scenario, the time right after the doctors last appointment.
            }
        }

        return new DateTime(); // Dummy return, so not to cause a syntactical error.
    }
    
    public List<KeyValuePair<Appointment, DateTime>> GetReschedulableAppointments(double appointmentDuration, AppointmentController appointmentController) // Returns reschedulable appointments of the doctor
    {
        List<KeyValuePair<Appointment, DateTime>> reschedulableAppointments = new List<KeyValuePair<Appointment, DateTime>>();

        var doctorAppointments = appointmentController.GetAppointmentsForDoctor(Email);

        foreach (var doctorAppointment in doctorAppointments)
        {
            bool overlapsOtherAppointment = false;
            foreach (var otherAppointment in doctorAppointments)
            {
                if (doctorAppointment == otherAppointment)
                    continue;
                overlapsOtherAppointment =
                    otherAppointment.Timeslot.IsOverlapping(new Timeslot(doctorAppointment.Timeslot.DateTime,
                        appointmentDuration));
                if (overlapsOtherAppointment)
                {
                    break;
                }
            }
            if (overlapsOtherAppointment)
            {
                continue;
            }
            
            reschedulableAppointments.Add(new KeyValuePair<Appointment, DateTime>(doctorAppointment, GetEarliestAppointmentAvailability(doctorAppointment.Timeslot.DurationInMinutes, appointmentController)));
        }

        return reschedulableAppointments;
    }

    public static Doctor GetDoctorByEmail(string email) // mozda smem da obrisem
    {
        Serializer<Doctor> doctorSerializer = new Serializer<Doctor>();
        List<Doctor> doctors = doctorSerializer.FromCSV(@"..\..\..\Data\Doctors.csv");

        foreach (Doctor d in doctors)
        {
            if (d.Email == email)
            {
                return d;
            }
        }

        return null;
    }

    public void CreateAppointment(string patientEmail, string doctorEmail, Timeslot t, bool isOperation)
    {
        Serializer<Appointment> appointmentSerializer = new Serializer<Appointment>();
        List<Appointment> appointments = appointmentSerializer.FromCSV(@"..\..\..\Data\Appointments.csv");
        
        appointments.Add(new Appointment(GetDoctorByEmail(doctorEmail), Patient.GetPatientByEmail(patientEmail), isOperation, t));

        MessageBox.Show("Completed");
        appointmentSerializer.ToCSV(@"..\..\..\Data\Appointments.csv", appointments);
    }

    public bool ModifyAppointment(int appointmentId, DateTime dt, double duration, Patient p, bool isSurgery)
    {
        Serializer<Appointment> appointmentSerializer = new Serializer<Appointment>();
        List<Appointment> appointments = appointmentSerializer.FromCSV(@"..\..\..\Data\Appointments.csv");

        foreach (Appointment a in appointments)
        {
            if (a.Id == appointmentId)
            {
                a.Timeslot = new Timeslot(dt, duration);
                a.Patient = p;
                a.IsSurgery = isSurgery;
                
                appointmentSerializer.ToCSV(@"..\..\..\Data\Appointments.csv", appointments);
                
                return true;
            }
        }
        return false;
    }
    
    public bool DeleteAppointment(int appointmentId)
    {
        Serializer<Appointment> appointmentSerializer = new Serializer<Appointment>();
        List<Appointment> appointments = appointmentSerializer.FromCSV(@"..\..\..\Data\Appointments.csv");

        foreach (Appointment a in appointments)
        {
            if (a.Id == appointmentId)
            {
                appointments.Remove(a);
                
                appointmentSerializer.ToCSV(@"..\..\..\Data\Appointments.csv", appointments);
                
                return true;
            }
        }
        return false;
    }

    public string[] ToCSV()
    {
        string notificationIds = "";
        foreach (var notification in Notifications)
        {
            notificationIds += notification.Id + ",";
        }
        notificationIds = notificationIds.TrimEnd(',');
        
        string[] tokens = { Email, Password, Name, Surname, Specialty.ToString(), notificationIds };
        return tokens;
    }

    public void FromCSV(string[] values)
    {
        Email = values[0];
        Password = values[1];
        Name = values[2];
        Surname = values[3];
        Enum.TryParse(values[4], out DoctorSpecialties specialty);
        Specialty = specialty;

        List<Notification> notifications = new List<Notification>();
        if (values[5] != "")
        {
            string[] notificationIds = values[5].Split(',');
            NotificationController notificationController = new NotificationController();
            foreach (var notificationId in notificationIds)
            {
                notifications.Add(notificationController.GetNotificationById(int.Parse(notificationId)));
            }
            Notifications = notifications;
        }
    }

    public void LoadAppointments()
    {
        Serializer<Appointment> appointmentSerializer = new Serializer<Appointment>();
        List<Appointment> appointments = appointmentSerializer.FromCSV(@"..\..\..\Data\Appointments.csv");

        foreach (Appointment appointment in appointments)
        {
            if (appointment.Doctor.Email == Email)
            {
                Appointments.Add(appointment);
            }
        }
    }
}