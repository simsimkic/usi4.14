using System.Linq;
using System.Windows;
using ZdravoCorp.Model;

namespace ZdravoCorp.View;

public partial class ViewMedicalChartWindow : Window
{
    public ViewMedicalChartWindow(Patient patient)
    {
        InitializeComponent();

        Title = patient.Name + " " + patient.Surname + "'s medical chart";

        HeightTextBox.Text = patient.MedicalRecord.Height.ToString();
        WeightTextBox.Text = patient.MedicalRecord.Weight.ToString();
        PastConditionsTextBox.Text = string.Join(',', patient.MedicalRecord.PastConditions);
        AllergiesTextBox.Text = string.Join(',', patient.MedicalRecord.Allergies);
    }

    private void ExitBtn_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}