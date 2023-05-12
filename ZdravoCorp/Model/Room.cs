using System;
using System.Collections.Generic;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Model
{
    public class Room : ISerializable
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public Equipment[] PresentEquipment { get; set; }

        public Room() 
        {
            PresentEquipment = new Equipment[0];
        }

        public Room(string type, string name, Equipment[] presentEquipment)
        {
            Type = type;
            Name = name;
            PresentEquipment = presentEquipment;
        }

        public string[] ToCSV()
        {
            string compactEquipment = "";
            foreach (Equipment eq in PresentEquipment)
            {
                compactEquipment += eq.Name + "," + eq.Quantity.ToString() + "," + eq.IsDynamic.ToString() + ",";
                switch (eq.Type)
                {
                    case "Medical Exam Equipment":
                        compactEquipment += "1;";
                        break;
                    case "Surgery Equipment":
                        compactEquipment += "2;";
                        break;
                    case "Room Furniture":
                        compactEquipment += "3;";
                        break;
                    case "Hallway Equipment":
                        compactEquipment += "4;";
                        break;
                    default:
                        compactEquipment += "0;";
                        break;
                }
            }

            compactEquipment = compactEquipment.TrimEnd(';');

            string[] csvValues =
            {
                Type, Name, compactEquipment
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Type = values[0];
            Name = values[1];
            string nameType = Type +" "+ Name;

            string[] compactPresentEquipment = values[2].Split(";");
            List<Equipment> equipment = new List<Equipment>();
            foreach(string eq in compactPresentEquipment)
            {
                string[] equipmentAttributes = eq.Split(",");
                string eqType;
                switch (equipmentAttributes[3])
                {
                    case "1":
                        eqType = "Medical Exam Equipment";
                        break;
                    case "2":
                        eqType = "Surgery Equipment";
                        break;
                    case "3":
                        eqType = "Room Furniture";
                        break;
                    case "4":
                        eqType = "Hallway Equipment";
                        break;
                    default:
                        eqType = "Other";
                        break;
                }

                Equipment currentEquipment = new Equipment(
                    equipmentAttributes[0],
                    Int32.Parse(equipmentAttributes[1]),
                    nameType,
                    bool.Parse(equipmentAttributes[2]),
                    eqType);
                equipment.Add(currentEquipment);
            }

            PresentEquipment = equipment.ToArray();
        }
    }
}
