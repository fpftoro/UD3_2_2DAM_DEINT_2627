using System.Windows;

namespace BindingsRotos;

/// <summary>
/// Lógica de interacción para MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly FichaViewModel _vm = new();

    public MainWindow()
    {
        InitializeComponent();
        DataContext = _vm;
    }

    private void btnVisitar_Click(object sender, RoutedEventArgs e)
    {
        _vm.Visitar();
    }
}
