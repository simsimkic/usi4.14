using System.IO;
using System.Collections.Generic;

namespace ZdravoCorp.Serializer;

public class Serializer<T> where T: ISerializable, new()
{
    private const char Delimiter = '|';

    public void ToCSV(string fileName, List<T> objects)
    {
        StreamWriter streamWriter = new StreamWriter(fileName);

        foreach (T obj in objects)
        {
            string line = string.Join(Delimiter.ToString(), obj.ToCSV());
            streamWriter.WriteLine(line);
        }
        
        streamWriter.Close();
    }

    public List<T> FromCSV(string fileName)
    {
        List<T> objects = new List<T>();

        foreach (string line in File.ReadLines(fileName))
        {
            string[] csvValues = line.Split(Delimiter);
            T obj = new T();
            obj.FromCSV(csvValues);
            objects.Add(obj);
        }

        return objects;
    }
}