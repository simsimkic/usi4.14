using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Observer;
using ZdravoCorp.Storage;

namespace ZdravoCorp.Model.DAO;

public class MedicalReportDAO : ISubject
{
    private List<IObserver> _observers;
    private MedicalReportStorage _storage;
    private List<MedicalReport> _medicalReports;

    public MedicalReportDAO()
    {
        _observers = new List<IObserver>();
        _storage = new MedicalReportStorage();
        _medicalReports = _storage.Load();
    }

    public int NextId()
    {
        return _medicalReports.Count == 0 ? 0 : _medicalReports.Max(medicalReport => medicalReport.Id) + 1;
    }

    public void Add(MedicalReport medicalReport)
    {
        medicalReport.Id = NextId();
        _medicalReports.Add(medicalReport);
        _storage.Save(_medicalReports);
        NotifyObservers();
    }
    
    public void UpdateIndices(int removedId)
    {
        for (; removedId < _medicalReports.Count; removedId++)
        {
            _medicalReports[removedId].Id = removedId;
        }
    }

    public void Remove(MedicalReport medicalReport)
    {
        _medicalReports.Remove(medicalReport);
        UpdateIndices(medicalReport.Id);
        _storage.Save(_medicalReports);
        NotifyObservers();
    }

    public List<MedicalReport> GetAll()
    {
        return _medicalReports;
    }

    public MedicalReport GetById(int medicalReportId)
    {
        return _medicalReports.FirstOrDefault(medicalReport => medicalReport.Id == medicalReportId);
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
