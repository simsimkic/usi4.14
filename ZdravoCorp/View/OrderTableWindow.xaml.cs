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
    public partial class OrderTableWindow : Window
    {
        private OrderRequestController _controller;
        public OrderTableWindow()
        {
            InitializeComponent();
            DataContext = this;
            _controller = new OrderRequestController();
            List<OrderRequest> orderRequests = new List<OrderRequest>(_controller.GetAllOrders());
            OrdersList.ItemsSource = orderRequests;
            OrdersList.Items.Filter = GetFilter();
        }
        private Predicate<object> GetFilter()
        {
            return CompletedFilter;
        }

        private bool CompletedFilter(object obj)
        {
            var Filterobj = obj as OrderRequest;
            return Filterobj.IsCompleted.Equals(false);
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
