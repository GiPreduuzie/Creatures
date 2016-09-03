using System.Windows;

namespace ImpossibleCreatures
{
    public partial class MainWindow : Window
    {
        private void CanEat_Checked(object sender, RoutedEventArgs e)
        {
            _visualizationType = VisualizationType.CanEat;
            //PrintCurrentMatrix(null, null);
        }

        private void Nation_Checked(object sender, RoutedEventArgs e)
        {
            _visualizationType = VisualizationType.Nation;
            //PrintCurrentMatrix(null, null);
        }

        private void Energy_Checked(object sender, RoutedEventArgs e)
        {
            _visualizationType = VisualizationType.Energy;
            //PrintCurrentMatrix(null, null);
        }

        private void Experimantal_Checked(object sender, RoutedEventArgs e)
        {
            _visualizationType = VisualizationType.Experimantal;
            //PrintCurrentMatrix(null, null);
        }
    }
}
