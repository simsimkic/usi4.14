using System;
using System.Linq;
using System.Windows;
using ZdravoCorp.Controller;
using ZdravoCorp.Model;
using System.Text.RegularExpressions;

namespace ZdravoCorp.View
{
    public partial class OrderEquipment : Window
    {
        private OrderRequestController _controller;
        public Equipment orderItem;
        public OrderEquipment(Equipment eq)
        {
            InitializeComponent();
            DataContext = this;
            orderItem = eq;
            _controller = new OrderRequestController();
            EquipmentTextBox.Text = eq.Name;
        }

        private void Order_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(QuantityTextBox.Text, out int quantity))
            {
                OrderRequest order = new OrderRequest(quantity, EquipmentTextBox.Text);
                _controller.Create(order);
                MessageBox.Show("Order successfully sent.");
                Close();
            } else
            {
                MessageBox.Show("Quantity must be an integer.");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
