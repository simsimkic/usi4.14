using System.Collections.ObjectModel;
using System.Windows;
using ZdravoCorp.Controller;
using ZdravoCorp.Model;
using ZdravoCorp.Observer;

namespace ZdravoCorp.View;

public partial class NotificationWindow : Window, IObserver
{
    private Patient? Patient;

    private Doctor? Doctor;
    
    private NotificationController _notificationController;
    public ObservableCollection<Notification> Notifications { get; set; }
    public Notification SelectedNotification { get; set; }

    public NotificationWindow(Patient? patient, Doctor? doctor, NotificationController notificationController)
    {
        InitializeComponent();

        DataContext = this;

        Patient = patient;
        
        Doctor = doctor;

        if (Patient == null)
        {
            Notifications = new ObservableCollection<Notification>(Doctor.Notifications);
        }
        else
        {
            Notifications = new ObservableCollection<Notification>(Patient.Notifications);
        }

        _notificationController = notificationController;
        _notificationController.Subscribe(this);
    }
    
    private void ReadBtn_OnClick(object sender, RoutedEventArgs e)
    {
        if (SelectedNotification == null)
        {
            MessageBox.Show("You must select a notification in order to read it.");
            return;
        }

        ReadNotificationWindow readNotificationWindow = new ReadNotificationWindow(SelectedNotification, _notificationController);
        readNotificationWindow.ShowDialog();
    }

    private void CloseBtn_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void UpdateNotificationList()
    {
        Notifications.Clear();
        if (Patient == null)
        {
            foreach (var notification in Doctor.Notifications)
            {
                Notifications.Add(notification);
            }
        }
        else
        {
            foreach (var notification in Patient.Notifications)
            {
                Notifications.Add(notification);
            }
        }
    }

    public void Update()
    {
        UpdateNotificationList();
    }
}