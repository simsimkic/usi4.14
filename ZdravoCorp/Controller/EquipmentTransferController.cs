using System.Collections.Generic;
using ZdravoCorp.Model;
using ZdravoCorp.Model.DAO;
using ZdravoCorp.Observer;

namespace ZdravoCorp.Controller;

public class EquipmentTransferController
{
    private EquipmentTransferDAO _transfers;

    public EquipmentTransferController()
    {
        _transfers = new EquipmentTransferDAO();
    }

    public List<EquipmentTransfer> GetAllTransfers()
    {
        return _transfers.GetAll();
    }

    public void Create(EquipmentTransfer transfer)
    {
        _transfers.Add(transfer);
    }

    public void Delete(EquipmentTransfer transfer)
    {
        _transfers.Remove(transfer);
    }

    public void Subscribe(IObserver observer)
    {
        _transfers.Subscribe(observer);
    }

    public void ValidateTransfers()
    {
        _transfers.ValidateTransfers();
    }
}
