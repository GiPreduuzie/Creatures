using System.Windows;

namespace ImpossibleCreatures
{
    public partial class MainWindow : Window
    {
        private void FillBlack_Checked(object sender, RoutedEventArgs e)
        {
            _visualizationType = VisualizationType.FillBlackStrokeCanEat;
        }

        private void FillEnergy_Checked(object sender, RoutedEventArgs e)
        {
            _visualizationType = VisualizationType.FillEnergyStrokeCanEat;
        }
    }
}
