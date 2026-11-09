using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Navegacion.Modelos;
using Navegacion.Ventanas;

namespace Navegacion.Paginas;

/// <summary>
/// Página con la lista de socios.
/// </summary>
public partial class PaginaSocios : Page
{
    public PaginaSocios()
    {
        InitializeComponent();
        lstSocios.ItemsSource = DatosClub.Socios;
    }

    private void btnNuevo_Click(object sender, RoutedEventArgs e)
    {
        // Ventana modal: ShowDialog no devuelve el control hasta que se cierra
        var dialogo = new VentanaNuevoSocio { Owner = Window.GetWindow(this) };
        if (dialogo.ShowDialog() != true || dialogo.SocioCreado is null)
        {
            return;
        }

        DatosClub.Socios.Add(dialogo.SocioCreado);
        lstSocios.Items.Refresh(); // una List<T> no avisa de los cambios (tema 9)
        lstSocios.SelectedItem = dialogo.SocioCreado;
    }

    private void btnVerFicha_Click(object sender, RoutedEventArgs e)
    {
        VerFicha();
    }

    private void lstSocios_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        VerFicha();
    }

    private void VerFicha()
    {
        if (lstSocios.SelectedItem is not Socio socio)
        {
            MessageBox.Show("Elige un socio de la lista.", "Socios", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        // La página siguiente recibe el socio en su constructor
        NavigationService.Navigate(new PaginaDetalleSocio(socio));
    }
}
