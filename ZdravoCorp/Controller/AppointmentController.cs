using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Model.DAO;
using ZdravoCorp.Model;
using ZdravoCorp.Observer;

namespace ZdravoCorp.Controller
{
    public class AppointmentController
    {
        private AppointmentDAO _appointments;

        public AppointmentController()
        {
            _appointments = new AppointmentDAO();
        }

        public List<Appointment> GetAllAppointments()
        {
            return _appointments.GetAll();
        }
        
        public List<Appointment> GetAppointmentsForPatient(string patientEmail)
        {
            return _appointments.GetForPatient(patientEmail);
        }
        
        public List<Appointment> GetAppointmentsForDoctor(string doctorEmail)
        {
            return _appointments.GetForDoctor(doctorEmail);
        }
        
        public Appointment GetAppointmentById(int appointmentId)
        {
            return _appointments.GetById(appointmentId);
        }
        
        public void Create(Appointment appointment)
        {
            _appointments.Add(appointment);
        }
        
        public void Delete(Appointment appointment)
        {
            _appointments.Remove(appointment);
        }

        public void Subscribe(IObserver observer)
        {
            _appointments.Subscribe(observer);
        }

        public bool IsDoctorsPatient(string doctorEmail, string patientEmail) 
        {
            return _appointments.IsDoctorsPatient(doctorEmail, patientEmail);
        }
    }
}
