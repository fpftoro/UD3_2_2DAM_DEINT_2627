using System.Windows;
using Navegacion.Paginas;
using Navegacion.Ventanas;

namespace Navegacion;

/// <summary>
/// Lógica de interacción para MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private VentanaAyuda? _ayuda;

    public MainWindow()
    {
        InitializeComponent();
        frmContenido.Navigate(new PaginaInicio());
    }

    private void btnInicio_Click(object sender, RoutedEventArgs e)
    {
        frmContenido.Navigate(new PaginaInicio());
    }

    private void btnSocios_Click(object sender, RoutedEventArgs e)
    {
        frmContenido.Navigate(new PaginaSocios());
    }

    private void btnAyuda_Click(object sender, RoutedEventArgs e)
    {
        // Ventana no modal: si ya está abierta, se trae al frente en lugar de abrir otra
        if (_ayuda is null)
        {
            _ayuda = new VentanaAyuda { Owner = this };
            _ayuda.Closed += (_, _) => _ayuda = null;
            _ayuda.Show();
        }
        else
        {
            _ayuda.Activate();
        }
    }
}
