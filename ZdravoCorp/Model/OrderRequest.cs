using System;
using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Controller;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Model;

public class OrderRequest : ISerializable
{
    public int Quantity { get; set; }
    public string Equipment { get; set; }
    public DateTime DateTimeOfArrival { get; set; }
    public bool IsCompleted { get; set; }

    public OrderRequest()
    {

    }
    
    public OrderRequest(int quantity, string eq)
    {
        DateTimeOfArrival = DateTime.Now.AddDays(1);
        Quantity = quantity;
        Equipment = eq;
        IsCompleted = false;
    }

    public string[] ToCSV()
    {
        string[] csvValues = { Equipment, Quantity.ToString(), IsCompleted.ToString(), DateTimeOfArrival.ToString() };
        return csvValues;
    }

    public void FromCSV(string[] values)
    {
        Equipment = values[0];
        Quantity = int.Parse(values[1]);
        IsCompleted = bool.Parse(values[2]);
        DateTimeOfArrival = DateTime.Parse(values[3]);
    }
}
