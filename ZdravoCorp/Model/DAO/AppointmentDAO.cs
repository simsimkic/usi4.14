using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Controller;
using ZdravoCorp.Observer;
using ZdravoCorp.Storage;

namespace ZdravoCorp.Model.DAO
{
    public class AppointmentDAO:ISubject
    {
        private List<IObserver> _observers;
        private AppointmentStorage _storage;
        private List<Appointment> _appointments;

        public AppointmentDAO()
        {
            _observers = new List<IObserver>();
            _storage = new AppointmentStorage();
            _appointments = _storage.Load();
        }
        public int NextId()
        {
            return _appointments.Count == 0 ? 0 : _appointments.Max(appointment => appointment.Id) + 1;
        }

        public void Add(Appointment appointment)
        {
            appointment.Id = NextId();
            _appointments.Add(appointment);
            _storage.Save(_appointments);
            NotifyObservers();
        }

        public void UpdateIndices(int removedId)
        {
            for (; removedId < _appointments.Count; removedId++)
            {
                _appointments[removedId].Id = removedId;
            }
        }

        public void Remove(Appointment appointment)
        {
            _appointments.Remove(appointment);
            UpdateIndices(appointment.Id);
            _storage.Save(_appointments);
            NotifyObservers();
        }

        public List<Appointment> GetAll()
        {
            return _appointments;
        }     
        public List<Appointment> GetForDoctor(string doctorEmail)
        {
            return _appointments.Where(appointment => appointment.Doctor.Email == doctorEmail).ToList();
        }
        
        public List<Appointment> GetForPatient(string patientEmail)
        {
            return _appointments.Where(appointment => appointment.Patient.Email == patientEmail).ToList();
        }

        public Appointment GetById(int appointmentId)
        {
            return _appointments.FirstOrDefault(appointment => appointment.Id == appointmentId);
        }
        
        public void Subscribe(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.Update();
            }
        }

        public bool IsDoctorsPatient(string doctorEmail, string patientEmail)
        {
            AppointmentController _controller = new AppointmentController();
            ObservableCollection<Appointment> appointments = new ObservableCollection<Appointment>(_controller.GetAppointmentsForDoctor(doctorEmail));

            foreach (Appointment appointment in appointments)
            {
                if(appointment.Patient.Email == patientEmail && appointment.Timeslot.IsBefore(DateTime.Now))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
