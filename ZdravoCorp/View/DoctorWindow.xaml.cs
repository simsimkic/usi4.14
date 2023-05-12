using System;
using System.ComponentModel;
using System.Windows;
using ZdravoCorp.Controller;
using ZdravoCorp.Model;

namespace ZdravoCorp.View
{
    /// <summary>
    /// Interaction logic for DoctorWindow.xaml
    /// </summary>
    public partial class DoctorWindow : Window
    {
        Doctor Doctor { get; set; }
        
        private NotificationController _notificationController;
        
        public DoctorWindow(Doctor doctor, NotificationController notificationController)
        {
            InitializeComponent();
            
            Doctor = doctor;

            _notificationController = notificationController;
        }
        
        private void DoctorWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            int unreadNotifications = 0;
            foreach (var notification in Doctor.Notifications)
            {
                if (!notification.IsRead())
                    unreadNotifications++;
            }

            if (unreadNotifications != 0)
                MessageBox.Show($"You have {unreadNotifications} unread notifications.");
        }

        private void OpenAppointmentView(object sender, RoutedEventArgs e)
        {
            DoctorAppointmentView dav = new DoctorAppointmentView(Doctor, _notificationController);
            Visibility = Visibility.Hidden;
            dav.Show();
        }

        private void OpenPatientView(object sender, RoutedEventArgs e)
        {
            DoctorPatientView dpv = new DoctorPatientView(Doctor, _notificationController);
            Visibility = Visibility.Hidden;
            dpv.Show();
        }
        
        private void OpenNotificationView(object sender, RoutedEventArgs e)
        {
            NotificationWindow notificationWindow = new NotificationWindow(null, Doctor, _notificationController);
            notificationWindow.Show();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
