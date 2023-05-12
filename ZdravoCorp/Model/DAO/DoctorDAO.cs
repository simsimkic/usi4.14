using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Observer;
using ZdravoCorp.Storage;

namespace ZdravoCorp.Model.DAO
{
    public class DoctorDAO
    {
        private List<IObserver> _observers;
        private DoctorStorage _storage;
        private List<Doctor> _doctors;

        public DoctorDAO()
        {
            _observers = new List<IObserver>();
            _storage = new DoctorStorage();
            _doctors = _storage.Load();
        }

        public void Add(Doctor doctor)
        {
            _doctors.Add(doctor);
            _storage.Save(_doctors);
            NotifyObservers();
        }

        public void Remove(Doctor doctor)
        {
            _doctors.Remove(doctor);
            _storage.Save(_doctors);
            NotifyObservers();
        }

        public List<Doctor> GetAll()
        {
            return _doctors;
        }
        
        public Doctor GetByEmail(string doctorEmail)
        {
            return _doctors.FirstOrDefault(doctor => doctor.Email == doctorEmail);
        }

        public List<Doctor> GetBySpecialty(Doctor.DoctorSpecialties specialty)
        {
            return _doctors.Where(doctor => doctor.Specialty == specialty).ToList();
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
    }
}
