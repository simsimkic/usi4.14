using System;
using System.Linq;
using System.Windows;
using ZdravoCorp.Controller;
using ZdravoCorp.Model;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using ZdravoCorp.Observer;
using System.Windows.Controls;

namespace ZdravoCorp.View
{

    public partial class TransferWindow : Window
    {
        private EquipmentTransferController _controller;
        private RoomController _rcontroller;
        public Equipment transferItem;
        public TransferWindow(Equipment eq)
        {
            InitializeComponent();
            DataContext = this;
            transferItem = eq;
            _controller = new EquipmentTransferController();
            _rcontroller = new RoomController();

            List<Room> rooms = _rcontroller.GetAllRooms();
            List<string> roomNames = new List<string>();
            foreach (Room room in rooms)
            {
                foreach(Equipment eqq in room.PresentEquipment)
                {
                    if(eqq.Quantity > 0 && eqq.Name == eq.Name)
                    {
                        roomNames.Add(room.Type + "," + room.Name);
                        break;
                    }
                }
            }
            FromRoom.ItemsSource = roomNames;
            EquipmentTextBox.Text = eq.Name;
            char num = eq.Room[eq.Room.Length - 1];
            int occurance = eq.Room.IndexOf(num);

            string selectedItem = string.Join(",", eq.Room.Substring(0, occurance -1), eq.Room.Substring(occurance));
            if (!roomNames.Contains(selectedItem))
            {
                roomNames.Add(selectedItem);
            }
            ToRoom.ItemsSource = roomNames;
            ToRoom.SelectedIndex = roomNames.IndexOf(selectedItem);
            ToRoom.IsEnabled = false;
            roomNames.Remove(selectedItem);
            
            if (eq.IsDynamic)
            {
                TransferDate.IsEnabled = false;
                TransferDate.SelectedDate = DateTime.Now;
            }
        }

        private void Transfer_Click(object sender, RoutedEventArgs e)
        {
            if (TransferDate.SelectedDate > DateTime.Now || transferItem.IsDynamic)
            {
                if (FromRoom.SelectedIndex != -1)
                {
                    string[] fromRoomData = FromRoom.SelectedItem.ToString().Split(",");
                    Room froom = _rcontroller.GetByName(fromRoomData[0], fromRoomData[1]);
                    string[] toRoomData = ToRoom.SelectedItem.ToString().Split(",");
                    Room troom = _rcontroller.GetByName(toRoomData[0], toRoomData[1]);


                    EquipmentTransfer equipmentTransfer = new EquipmentTransfer(transferItem.Name, int.Parse(Quantity.SelectedItem.ToString())
                        ,froom,troom,(DateTime) TransferDate.SelectedDate, transferItem.IsDynamic);
                    _controller.Create(equipmentTransfer);
                    MessageBox.Show("Order succesfully sent.");
                    _controller.ValidateTransfers();
                    TransferRequestWindow window = new TransferRequestWindow();
                    window.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Please select a room to transfer equipment from.");
                }
            }
            else
            {
                MessageBox.Show("Date must be in the future.");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            TransferRequestWindow window = new TransferRequestWindow();
            window.Show();
            Close();
        }

        private void FromRoom_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string[] selection = FromRoom.SelectedItem.ToString().Split(",");
            Room fromRoom = _rcontroller.GetByName(selection[0], selection[1]);
            int maxNum = 0;
            foreach(Equipment eq in fromRoom.PresentEquipment)
            {
                if(eq.Name == transferItem.Name)
                {
                    maxNum = eq.Quantity;
                }
            }
            if(maxNum == 0)
            {
                MessageBox.Show("This room doesnt have sufficient equipment for transfer, try a different one.");
                return;
            }
            List<string> quantityComboBox = new List<string>();
            for(int i = 1; i<= maxNum; i++)
            {
                quantityComboBox.Add(i.ToString());
            }
            Quantity.ItemsSource = quantityComboBox;
            Quantity.SelectedIndex = 0;
        }
    }
}
