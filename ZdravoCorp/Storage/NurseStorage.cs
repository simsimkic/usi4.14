using System.Collections.Generic;
using ZdravoCorp.Model;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Storage;

public class NurseStorage
{
   private const string StoragePath = @"..\..\..\Data\Nurses.csv";

   private Serializer<Nurse> _serializer;

   public NurseStorage()
   {
      _serializer = new Serializer<Nurse>();
   }

   public List<Nurse> Load()
   {
      return _serializer.FromCSV(StoragePath);
   }

   public void Save(List<Nurse> nurses)
   {
      _serializer.ToCSV(StoragePath,nurses);
   }
}