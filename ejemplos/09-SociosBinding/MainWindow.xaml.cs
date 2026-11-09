using System.Windows;

namespace SociosBinding;

/// <summary>
/// Lógica de interacción para MainWindow.xaml.
/// Ya no copia datos entre los controles y los socios: de eso se encargan los bindings.
/// </summary>
public partial class MainWindow : Window
{
    private readonly SociosViewModel _vm = new();

    public MainWindow()
    {
        InitializeComponent();
        DataContext = _vm;
    }

    private void btnNuevo_Click(object sender, RoutedEventArgs e)
    {
        _vm.Nuevo();
        txtNombre.Focus();
        txtNombre.SelectAll();
    }

    private void btnEliminar_Click(object sender, RoutedEventArgs e)
    {
        if (_vm.SocioSeleccionado is null)
        {
            return;
        }

        var respuesta = MessageBox.Show(
            $"¿Seguro que quieres eliminar a {_vm.SocioSeleccionado.Nombre}?",
            "Eliminar socio",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning,
            MessageBoxResult.No);
        if (respuesta == MessageBoxResult.Yes)
        {
            _vm.Eliminar();
        }
    }
}
