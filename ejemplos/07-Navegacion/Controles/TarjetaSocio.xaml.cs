using System.Windows.Controls;
using Navegacion.Modelos;

namespace Navegacion.Controles;

/// <summary>
/// Tarjeta reutilizable con los datos de un socio. La usan varias páginas.
/// </summary>
public partial class TarjetaSocio : UserControl
{
    public TarjetaSocio()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Muestra en la tarjeta los datos de un socio.
    /// </summary>
    /// <param name="socio">El socio que se muestra.</param>
    public void Mostrar(Socio socio)
    {
        txtNombre.Text = socio.Nombre;
        txtCategoria.Text = $"Categoría: {socio.Categoria}";
        txtAntiguedad.Text = $"Antigüedad: {socio.Antiguedad} años";
    }
}
