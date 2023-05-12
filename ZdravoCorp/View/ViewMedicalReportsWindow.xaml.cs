using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZdravoCorp.Model;

namespace ZdravoCorp.View
{
    /// <summary>
    /// Interaction logic for ViewMedicalReportsWindow.xaml
    /// </summary>
    public partial class ViewMedicalReportsWindow : Window
    {
        public Patient currentPatient { get; set; }
        public ObservableCollection<Appointment> Appointments { get; set; }
        public ViewMedicalReportsWindow(Patient patient)
        {
            InitializeComponent();
            DataContext = this;
            currentPatient= patient;
            Appointments = new ObservableCollection<Appointment>(currentPatient.getFinishedAppointments());
            SortPicker.Items.Add("Report Date");
            SortPicker.Items.Add("Doctor Email");
            SortPicker.Items.Add("Specialization");
            SortPicker.SelectedItem = "Report Date";
        }

        private void Filter_Button_Click(object sender, RoutedEventArgs e)
        {
            var _itemSourceList = new CollectionViewSource() { Source = Appointments };
            ICollectionView Itemlist = _itemSourceList.View;
            var Filter = new Predicate<object>(item => ((Appointment)item).Report.Diagnosis.Contains(FilterText.Text));
            Itemlist.Filter = Filter;
            ReportGrid.ItemsSource = Itemlist;
        }
        private void Sort_Button_Click(object sender, RoutedEventArgs e)
        {
            ReportGrid.Items.SortDescriptions.Clear();
            ReportGrid.Items.SortDescriptions.Add(new SortDescription(SortPicker.SelectedItem.ToString(), ListSortDirection.Ascending));
            ReportGrid.Items.Refresh();

        }
    }
}
