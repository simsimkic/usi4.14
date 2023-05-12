using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ZdravoCorp.Controller;
using ZdravoCorp.Model;
using ZdravoCorp.Observer;


namespace ZdravoCorp.View
{

    public partial class EquipmentTable : Window, IObserver
    {
        private RoomController _controller;

        public EquipmentTable()
        {

            InitializeComponent();
            DataContext = this;
            _controller = new RoomController();
            _controller.Subscribe(this);
            List<Equipment> Equipment = new List<Equipment>(_controller.GetAllEquipment());
            EquipmentList.ItemsSource = Equipment;
            FilterBy.ItemsSource = new string[] { "Name", "Type", "Quantity", "Room"};
            EquipmentList.Items.Filter = GetFilter();

        }
        public Predicate<object> GetFilter()
        {
            switch (FilterBy.SelectedItem as string)
            {
                case "Name":
                    return NameFilter;
                case "Quantity":
                    return QuantityFilter;
                case "Room":
                    return RoomFilter;
                case "Type":
                    return TypeFilter;
            }
            return NameFilter;
        }

        private bool TypeFilter(object obj)
        {
            bool IsDynamic = (bool)IsDynamicBox.IsChecked;
            var Filterobj = obj as Equipment;
            return Filterobj.Type.Contains(Keyword.Text, StringComparison.OrdinalIgnoreCase) && Filterobj.IsDynamic.Equals(IsDynamic);
        }

        private bool NameFilter(object obj)
        {
            bool IsDynamic = (bool) IsDynamicBox.IsChecked;
            var Filterobj = obj as Equipment;
            return Filterobj.Name.Contains(Keyword.Text, StringComparison.OrdinalIgnoreCase) && Filterobj.IsDynamic.Equals(IsDynamic);
        }

        private bool QuantityFilter(object obj)
        {
            bool IsDynamic = (bool)IsDynamicBox.IsChecked;
            var Filterobj = obj as Equipment;
            if(!Int32.TryParse(Keyword.Text, out int qt)) qt = 99;
            return Filterobj.Quantity <= qt && Filterobj.IsDynamic.Equals(IsDynamic);
        }

        private bool RoomFilter(object obj)
        {
            bool IsDynamic = (bool)IsDynamicBox.IsChecked;
            var Filterobj = obj as Equipment;
            return Filterobj.Room.Contains(Keyword.Text, StringComparison.OrdinalIgnoreCase) && Filterobj.IsDynamic.Equals(IsDynamic);
        }

        private void Keyword_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(Keyword.Text == null)
            {
                EquipmentList.Items.Filter = null;
            } else
            {
                EquipmentList.Items.Filter = GetFilter();
            }
        }

        private void EmptyKeyword(object sender, RoutedEventArgs e)
        {
            Keyword.Text = null;
            EquipmentList.Items.Filter = GetFilter();
        }

        public void Update()
        {
        }
    }
}

