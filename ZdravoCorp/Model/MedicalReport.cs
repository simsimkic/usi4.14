using System;
using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Controller;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Model;

public class MedicalReport : ISerializable
{
    public int Id { get; set; }
    public DateTime DateTimeOfCreation { get; set; }
    public List<string> Symptoms { get; set; }
    public string Diagnosis { get; set; }

    public MedicalReport()
    {
        Symptoms = new List<string>();
    }
    
    public MedicalReport(DateTime dateOfCreation, List<string> symptoms)
    {
        DateTimeOfCreation = dateOfCreation;
        Symptoms = symptoms;
    }
    
    public string[] ToCSV()
    { 
       string[] csvValues = { Id.ToString(), DateTimeOfCreation.ToString(), string.Join(',', Symptoms), Diagnosis }; 
       return csvValues;
    }

    public void FromCSV(string[] values)
    {
        Id = int.Parse(values[0]);
        DateTimeOfCreation = DateTime.ParseExact(values[1], "M/d/yyyy h:m:s tt", null);
        Symptoms = values[2].Split(',').ToList();
        Diagnosis = values[3];
    }
}
