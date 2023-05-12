namespace ZdravoCorp.Serializer;

public interface ISerializable
{
   string[] ToCSV();

   void FromCSV(string[] values);
}