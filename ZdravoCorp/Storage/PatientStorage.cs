using System.Collections.Generic;
using ZdravoCorp.Model;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Storage;

public class PatientStorage
{
    private const string StoragePath = @"..\..\..\Data\Patients.csv";

    private Serializer<Patient> _serializer;

    public PatientStorage()
    {
        _serializer = new Serializer<Patient>();
    }

    public List<Patient> Load()
    {
        return _serializer.FromCSV(StoragePath);
    }

    public void Save(List<Patient> patients)
    {
        _serializer.ToCSV(StoragePath, patients);
    }
}