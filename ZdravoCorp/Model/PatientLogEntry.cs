using System;
using System.Collections.Generic;
using System.Diagnostics;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Model
{
    public enum AppointmentStatus
    {
        Modified,
        Cancelled,
        Scheduled
    }
    public class PatientLogEntry : ISerializable
    {
        public string PatientEmail { get; set; }
        public int AppointmentId { get; set; }
        public AppointmentStatus Status { get; set; }
        public DateTime AppointmentDate { get; set; }

        public PatientLogEntry(string patientEmail, int appointmentId, AppointmentStatus status, DateTime appointmentDate)
        {
            PatientEmail = patientEmail;
            AppointmentId = appointmentId;
            Status = status;
            AppointmentDate = appointmentDate;
        }
        public PatientLogEntry() { }
        public string[] ToCSV()
        {
            string[] csvValues = { PatientEmail, AppointmentId.ToString(), Status.ToString(), AppointmentDate.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            PatientEmail = values[0];
            AppointmentId = int.Parse(values[1]);
            AppointmentStatus StatusParse;
            Enum.TryParse(values[2],out StatusParse);
            Status = StatusParse;
            AppointmentDate = DateTime.Parse(values[3]);
        }

        public bool IsEntryExpired() => (DateTime.Today - AppointmentDate).Days > 30;

        public void RemoveAllExpiredEntries()
        {
            List<PatientLogEntry> logList = new List<PatientLogEntry>();
            Serializer<PatientLogEntry> LogSerializer = new Serializer<PatientLogEntry>();
            logList = LogSerializer.FromCSV(@"..\..\..\Data\PatientLog.csv");
            foreach (PatientLogEntry log in logList)
            {
                if (log.IsEntryExpired())
                {
                    logList.Remove(log);
                }
            }
            LogSerializer.ToCSV(@"..\..\..\Data\PatientLog.csv", logList);
        }
        public void AddLog()
        {
            List<PatientLogEntry> logList = new List<PatientLogEntry>();
            Serializer<PatientLogEntry> LogSerializer = new Serializer<PatientLogEntry>();
            logList = LogSerializer.FromCSV(@"..\..\..\Data\PatientLog.csv");
            logList.Add(this);
            LogSerializer.ToCSV(@"..\..\..\Data\PatientLog.csv", logList);
        }
    }
   
}
