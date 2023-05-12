using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ZdravoCorp.Controller;
using ZdravoCorp.Model;
using ZdravoCorp.Observer;

namespace ZdravoCorp.View
{
    public partial class EquipmentRequestTable : Window, IObserver
    {
        private RoomController _controller;
        public Equipment SelectedEquipment { get; set; }
        public EquipmentRequestTable()
        {
            InitializeComponent();
            DataContext = this;
            _controller = new RoomController();
            _controller.Subscribe(this);
            List<Equipment> Equipment = new List<Equipment>(_controller.GetEquipmentSum());
            EquipmentList.ItemsSource = Equipment;
            EquipmentList.Items.Filter = GetFilter();
        }

        private Predicate<object> GetFilter()
        {
            return QuantityDynamicFilter;
        }

        private bool QuantityDynamicFilter(object obj)
        {
            var Filterobj = obj as Equipment;
            return Filterobj.IsDynamic.Equals(true) && Filterobj.Quantity < 5;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedEquipment != null)
            {
                OrderEquipment orderEquipment = new OrderEquipment(SelectedEquipment);
                orderEquipment.Show();
            }
            else
            {
                MessageBox.Show("You must pick an equipment item in order to order.");
            }
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            ManagerMainWindow window = new ManagerMainWindow();
            window.Show();
            this.Close();
        }
        private void Order_Table(object sender, RoutedEventArgs e)
        {
            OrderTableWindow window = new OrderTableWindow();
            window.Show();
        }

        public void Update()
        {
        }
    }
}
