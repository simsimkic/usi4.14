using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ZdravoCorp.Controller;
using ZdravoCorp.Model;
using ZdravoCorp.Observer;

namespace ZdravoCorp.View
{
    public partial class TransferRequestWindow : Window, IObserver
    {
        private RoomController _controller;
        public Equipment SelectedEquipment { get; set; }
        public ObservableCollection<Equipment> Equipment { get; set; }
        public TransferRequestWindow()
        {
            InitializeComponent();
            DataContext = this;
            _controller = new RoomController();
            _controller.Subscribe(this);
            Equipment = new ObservableCollection<Equipment>(_controller.GetAllEquipment());
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
            return Filterobj.Quantity < 5;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedEquipment != null)
            {
                TransferWindow orderEquipment = new TransferWindow(SelectedEquipment);
                orderEquipment.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("You must pick an equipment item in order to transfer.");
            }
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            ManagerMainWindow window = new ManagerMainWindow();
            window.Show();
            this.Close();
        }

        public void Update()
        {
            Equipment.Clear();
            foreach (var eq in _controller.GetAllEquipment())
            {
                Equipment.Add(eq);
            }
        }

    }
}

