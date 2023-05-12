using System.Windows;
using ZdravoCorp.Controller;
using ZdravoCorp.Model;

namespace ZdravoCorp.View;

public partial class ReadNotificationWindow : Window
{
    
    public ReadNotificationWindow(Notification notification, NotificationController notificationController)
    {
        InitializeComponent();

        ContentTextBox.Text = notification.Content;
        
        notificationController.Delete(notification);
        notification.Status = Notification.NotificationStatus.Read;
        notificationController.Create(notification);
        
        
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}