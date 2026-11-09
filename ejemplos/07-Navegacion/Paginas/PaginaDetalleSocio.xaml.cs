using System.Windows;
using System.Windows.Controls;
using Navegacion.Modelos;

namespace Navegacion.Paginas;

/// <summary>
/// Ficha de un socio, a la que se llega desde la lista.
/// </summary>
public partial class PaginaDetalleSocio : Page
{
    /// <summary>
    /// Crea la página con los datos del socio que se muestra.
    /// </summary>
    /// <param name="socio">El socio elegido en la lista.</param>
    public PaginaDetalleSocio(Socio socio)
    {
        InitializeComponent();
        ctlTarjeta.Mostrar(socio);
    }

    private void btnVolver_Click(object sender, RoutedEventArgs e)
    {
        if (NavigationService.CanGoBack)
        {
            NavigationService.GoBack();
        }
    }
}
