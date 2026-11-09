using System.Windows;

namespace Club.Wpf;

/// <summary>
/// Lógica de interacción para MainWindow.xaml. Con MVVM solo queda InitializeComponent: el DataContext lo asigna App.
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
}
