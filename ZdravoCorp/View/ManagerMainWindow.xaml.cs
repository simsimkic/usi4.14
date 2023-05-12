using System.Windows;
using ZdravoCorp.Controller;

namespace ZdravoCorp.View
{
    public partial class ManagerMainWindow : Window
    {
        OrderRequestController _orders { get; set; }
        EquipmentTransferController _transfers { get; set; }
        public ManagerMainWindow()
        {
            _orders = new OrderRequestController();
            _transfers = new EquipmentTransferController();
            _orders.ValidateOrders();
            _transfers.ValidateTransfers();
            InitializeComponent();
        }

        private void EquipmentTableBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(this.EquipmentTableBtn))
            {
                EquipmentTable equipmentTable = new EquipmentTable();
                equipmentTable.Show();
            }
        }

        private void ReqstEquipmentTableBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(this.RequestEquipmentTableBtn))
            {
                EquipmentRequestTable equipmentRequestTable = new EquipmentRequestTable();
                equipmentRequestTable.Show();
                this.Close();
            }
        }
        private void EquipmentTransferBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(this.EquipmentTransferBtn))
            {
                TransferRequestWindow equipmentTransfer = new TransferRequestWindow();
                equipmentTransfer.Show();
                this.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
