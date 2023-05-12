using System.Collections.Generic;
using ZdravoCorp.Model;
using ZdravoCorp.Serializer;


namespace ZdravoCorp.Storage;
public class ManagerStorage
{
    private const string StoragePath = @"..\..\..\Data\Managers.csv";

    private Serializer<Manager> _serializer;

    public ManagerStorage()
    {
        _serializer = new Serializer<Manager>();
    }

    public List<Manager> Load()
    {
        return _serializer.FromCSV(StoragePath);
    }

    public void Save(List<Manager> managers)
    {
        _serializer.ToCSV(StoragePath, managers);
    }
}
