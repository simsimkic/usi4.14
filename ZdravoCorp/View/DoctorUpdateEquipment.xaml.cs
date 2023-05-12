using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZdravoCorp.Controller;
using ZdravoCorp.Model;

namespace ZdravoCorp.View
{
    /// <summary>
    /// Interaction logic for DoctorUpdateEquipment.xaml
    /// </summary>
    public partial class DoctorUpdateEquipment : Window
    {
        public Room Room { get; set; }

        public DoctorUpdateEquipment()
        {
            Room = new Room();
            InitializeComponent();
        }

        private void SearchRoom(object sender, RoutedEventArgs e)
        {
            RoomController roomController = new RoomController();

            string roomType =RoomTypeTextBox.Text;
            string roomName =RoomNameTextBox.Text;
            EquipmentGrid.Items.Clear();

            Room foundRoom = roomController.GetByName(roomType, roomName);
            Room = foundRoom;

            if (foundRoom != null)
            {
                foreach(Equipment item in foundRoom.PresentEquipment.ToList())
                {
                    if(item.IsDynamic)
                        EquipmentGrid.Items.Add(item);
                }

                SaveButton.IsEnabled = true;
                UpdateQuantityButton.IsEnabled = true;

            } 
            else
            {
                MessageBox.Show("That room doesn't exist.");
            }
        }

        private void ChangeQuantity(object sender, RoutedEventArgs e)
        {
            string name = EquipmentNameTextBox.Text;
            List<Equipment> equipList = new List<Equipment>(Room.PresentEquipment);

            bool found = false;
            try 
            {
                int quantity = Int32.Parse(QuantityUsedTextBox.Text);
                foreach (Equipment item in equipList)
                {
                    if (item.Name == name)
                    {
                        item.Quantity -= quantity;
                        found = true;
                        Room.PresentEquipment = equipList.ToArray();
                        MessageBox.Show("Quantity changed.");
                        break;
                    }
                }
                if(!found)
                {
                    MessageBox.Show("Equipment with that name doesn't exist.");
                }
            } 
            catch
            {
                MessageBox.Show("Quantity must be an integer.");
            }
        }

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            RoomController roomController = new RoomController();
            ObservableCollection<Room> rooms = new ObservableCollection<Room>(roomController.GetAllRooms());
            foreach (Room room in rooms)
            {
                if(room.Name == Room.Name && room.Type == Room.Type)
                {
                    roomController.Delete(room);
                    break;
                }
            }
            roomController.Create(Room);
            Close();
        }
    }
}
