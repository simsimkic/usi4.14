using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Model;

public class MedicalRecord 
{
   public float Height { get; set; } 
   public float Weight { get; set; }
   public List<string> PastConditions { get; set; }
   public List<string> Allergies { get; set; }
   public List<MedicalReport> Reports { get; set; }

   public MedicalRecord(float height, float weight, List<string> pastConditions, List<string> allergies, List<MedicalReport> reports)
   { 
       Height = height; 
       Weight = weight; 
       PastConditions = pastConditions; 
       Allergies = allergies;
       Reports = reports;
   }
   
   public MedicalRecord() { 
       PastConditions= new List<string>();
       Allergies= new List<string>();
       Reports = new List<MedicalReport>();
   }
}