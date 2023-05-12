using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Observer;
using ZdravoCorp.Storage;

namespace ZdravoCorp.Model.DAO;

public class PatientDAO : ISubject
{
    private List<IObserver> _observers;
    private PatientStorage _storage;
    private List<Patient> _patients;

    public PatientDAO()
    {
        _observers = new List<IObserver>();
        _storage = new PatientStorage();
        _patients = _storage.Load();
    }

    public void Add(Patient patient)
    {
        _patients.Add(patient);
        _storage.Save(_patients);
        NotifyObservers();
    }
    
    public void Remove(Patient patient)
    {
        _patients.Remove(patient);
        _storage.Save(_patients);
        NotifyObservers();
    }

    public List<Patient> GetAll()
    {
        return _patients;
    }
    
    public Patient GetByEmail(string patientEmail)
    {
        return _patients.FirstOrDefault(patient => patient.Email == patientEmail);
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