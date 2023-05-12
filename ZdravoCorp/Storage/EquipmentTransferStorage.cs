using System.Collections.Generic;
using ZdravoCorp.Model;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Storage;

public class EquipmentTransferStorage
{
    private const string StoragePath = @"..\..\..\Data\TransferRequests.csv";
    private Serializer<EquipmentTransfer> _serializer;

    public EquipmentTransferStorage()
    {
        _serializer = new Serializer<EquipmentTransfer>();
    }
    public List<EquipmentTransfer> Load()
    {
        return _serializer.FromCSV(StoragePath);
    }
    public void Save(List<EquipmentTransfer> orders)
    {
        _serializer.ToCSV(StoragePath, orders);
    }
}
