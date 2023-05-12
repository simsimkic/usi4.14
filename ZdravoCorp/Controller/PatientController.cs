using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Model;
using ZdravoCorp.Model.DAO;
using ZdravoCorp.Observer;

namespace ZdravoCorp.Controller;

public class PatientController
{
    private PatientDAO _patients;

    public PatientController()
    {
        _patients = new PatientDAO();
    }

    public List<Patient> GetAllPatients()
    {
        return _patients.GetAll();
    }
    
    public Patient GetPatientByEmail(string patientEmail)
    {
        return _patients.GetByEmail(patientEmail);
    }
    
    public List<Patient> GetPatientsByName(string name)
    {
        return _patients.GetAll().Where(patient => patient.Name == name).ToList();
    }
    
    public List<Patient> GetPatientsBySurname(string surname)
    {
        return _patients.GetAll().Where(patient => patient.Surname == surname).ToList();
    }
    
    public List<Patient> GetPatientsByNameSurname(string name, string surname)
    {
        return _patients.GetAll().Where(patient => (patient.Name == name && patient.Surname == surname)).ToList();
    }
    
    public void Create(Patient patient)
    {
        _patients.Add(patient);
    }

    public void Delete(Patient patient)
    {
        _patients.Remove(patient);
    }

    public void Subscribe(IObserver observer)
    {
        _patients.Subscribe(observer);
    }
}