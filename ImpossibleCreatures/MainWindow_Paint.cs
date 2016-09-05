using System.Windows;

namespace ImpossibleCreatures
{
    public partial class MainWindow : Window
    {
        private void CanEat_Checked(object sender, RoutedEventArgs e)
        {
            _visualizationType = VisualizationType.CanEat;
            ProcessCurrentStatus();
        }

        private void Nation_Checked(object sender, RoutedEventArgs e)
        {
            _visualizationType = VisualizationType.Nation;
            ProcessCurrentStatus();
        }

        private void Energy_Checked(object sender, RoutedEventArgs e)
        {
            _visualizationType = VisualizationType.Energy;
            ProcessCurrentStatus();
        }

        private void Experimantal_Checked(object sender, RoutedEventArgs e)
        {
            _visualizationType = VisualizationType.Experimantal;
            ProcessCurrentStatus();
        }
    }
}
