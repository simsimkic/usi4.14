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
    public class DoctorController
    {
        private DoctorDAO _doctors;

        public DoctorController()
        {
            _doctors = new DoctorDAO();
        }

        public List<Doctor> GetAllDoctors()
        {
            return _doctors.GetAll();
        }
        
        public Doctor GetDoctorByEmail(string doctorEmail)
        {
            return _doctors.GetByEmail(doctorEmail);
        }
        
        public List<Doctor> GetDoctorsBySpecialty(Doctor.DoctorSpecialties specialty)
        {
            return _doctors.GetBySpecialty(specialty);
        }
        
        public void Create(Doctor doctor)
        {
            _doctors.Add(doctor);
        }

        public void Delete(Doctor doctor)
        {
            _doctors.Remove(doctor);
        }

        public void Subscribe(IObserver observer)
        {
            _doctors.Subscribe(observer);
        }
    }
}
