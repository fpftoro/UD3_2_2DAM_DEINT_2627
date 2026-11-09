using System.Windows;

namespace SociosComandos;

/// <summary>
/// Lógica de interacción para MainWindow.xaml.
/// Sin manejadores de eventos: los botones y los atajos usan los comandos del ViewModel.
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var vm = new SociosViewModel
        {
            Confirmar = mensaje => MessageBox.Show(
                this,
                mensaje,
                "Socios del club",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning,
                MessageBoxResult.No) == MessageBoxResult.Yes
        };
        DataContext = vm;
    }
}
