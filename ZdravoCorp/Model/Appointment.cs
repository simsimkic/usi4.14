using System;
using ZdravoCorp.Controller;

namespace ZdravoCorp.Model
{
    public class Appointment:Serializer.ISerializable
    {
        //
        //This needs to be preserved when Appointment class is refactored
        //
        public enum AppointmentStatus
        {
            Scheduled,
            CheckedIn,
            Finished,
            Expired,
            Urgent
        }
        
        public int Id { get; set; }
        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }
        public bool IsSurgery { get; set; }
        public Timeslot Timeslot { get; set; }
        public AppointmentStatus Status { get; set; }
        public MedicalReport? Report { get; set; }
        
        //
        //Added constructor without id as a parameter, since id gets incrementally generated in the controller.
        //Delete original one if it is unused.
        //
        public Appointment(Doctor doctor, Patient patient, bool isSurgery,Timeslot timeslot)
        {
            Timeslot = timeslot;
            Doctor = doctor;
            Patient = patient;
            IsSurgery = isSurgery;
            Status = AppointmentStatus.Scheduled;
        }

        public Appointment(Doctor doctor, Patient patient, bool isSurgery, Timeslot timeslot, AppointmentStatus status)
        {
            Timeslot = timeslot;
            Doctor = doctor;
            Patient = patient;
            IsSurgery = isSurgery;
            Status = status;
        }

        public Appointment()
        {
            Id = 0;
            Doctor = new Doctor();
            Patient = new Patient();
            IsSurgery = false;
            Timeslot = new Timeslot();
        }
        
        public bool IsModifiable() => Math.Abs((DateTime.Today - Timeslot.DateTime).Days) >= 1;
        
        public string[] ToCSV()
        {
            string reportId = Report != null ? Report.Id.ToString() : "";
            
            string[] csvValues = { Id.ToString(), Doctor.Email, Patient.Email, IsSurgery.ToString(), Timeslot.DateTime.ToString("M/dd/yyyy hh:mm:ss tt"), Timeslot.DurationInMinutes.ToString(), Status.ToString(), reportId };
            return csvValues;
        }
        
        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Doctor= Doctor.GetDoctorByEmail(values[1]);
            Patient = Patient.GetPatientByEmail(values[2]);
            IsSurgery = bool.Parse(values[3]);
            Timeslot = new Timeslot(DateTime.ParseExact(values[4], "M/dd/yyyy hh:mm:ss tt", null), Double.Parse(values[5]));
            
            Enum.TryParse(values[6], out AppointmentStatus temporaryStatus);
            Status = temporaryStatus;

            if (values[7] == "")
            {
                Report = null;
            }
            else
            {
                MedicalReportController medicalReportController = new MedicalReportController();
                Report = medicalReportController.GetMedicalReportById(int.Parse(values[7]));
            }
        }
    }
}
