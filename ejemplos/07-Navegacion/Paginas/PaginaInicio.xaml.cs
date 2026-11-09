using System.Windows.Controls;
using Navegacion.Modelos;

namespace Navegacion.Paginas;

/// <summary>
/// Página de inicio: un resumen del club.
/// </summary>
public partial class PaginaInicio : Page
{
    public PaginaInicio()
    {
        InitializeComponent();

        txtTotal.Text = $"El club tiene {DatosClub.Socios.Count} socios.";
        var masAntiguo = DatosClub.Socios.MaxBy(s => s.Antiguedad);
        if (masAntiguo is not null)
        {
            ctlMasAntiguo.Mostrar(masAntiguo);
        }
    }
}
