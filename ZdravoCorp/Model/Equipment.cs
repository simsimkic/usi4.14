namespace ZdravoCorp.Model
{
    public class Equipment
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Room { get; set; }
        public bool IsDynamic { get; set; }
        public string Type { get; set; }

        public Equipment(string name, int quantity, string room, bool isDynamic, string type)
        {
            Name = name;
            Quantity = quantity;
            Room = room;
            IsDynamic = isDynamic;
            Type = type;
        }

    }
}
