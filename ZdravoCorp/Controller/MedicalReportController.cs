using System.Collections.Generic;
using ZdravoCorp.Model;
using ZdravoCorp.Model.DAO;
using ZdravoCorp.Observer;

namespace ZdravoCorp.Controller;

public class MedicalReportController
{
    private MedicalReportDAO _medicalReports;

    public MedicalReportController()
    {
        _medicalReports = new MedicalReportDAO();
    }

    public List<MedicalReport> GetAllMedicalReports()
    {
        return _medicalReports.GetAll();
    }

    public MedicalReport GetMedicalReportById(int medicalReportId)
    {
        return _medicalReports.GetById(medicalReportId);
    }

    public void Create(MedicalReport medicalReport)
    {
        _medicalReports.Add(medicalReport);
    }

    public void Delete(MedicalReport medicalReport)
    {
        _medicalReports.Remove(medicalReport);
    }

    public void Subscribe(IObserver observer)
    {
        _medicalReports.Subscribe(observer);
    }
}
