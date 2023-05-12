using System.Collections.Generic;
using ZdravoCorp.Model;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Storage;

public class MedicalReportStorage
{
    private const string StoragePath = @"..\..\..\Data\MedicalReports.csv";

    private Serializer<MedicalReport> _serializer;

    public MedicalReportStorage()
    {
        _serializer = new Serializer<MedicalReport>();
    }

    public List<MedicalReport> Load()
    {
        return _serializer.FromCSV(StoragePath);
    }

    public void Save(List<MedicalReport> medicalReports)
    {
        _serializer.ToCSV(StoragePath, medicalReports);
    }
}