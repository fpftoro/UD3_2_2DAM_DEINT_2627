using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Club.Wpf.Conversores;

/// <summary>
/// Convierte la categoría de un socio en el color de su marca en la lista.
/// </summary>
public class CategoriaAColorConverter : IValueConverter
{
    /// <summary>De la categoría (origen) al pincel (destino).</summary>
    /// <param name="value">La categoría.</param>
    /// <param name="targetType">Tipo de la propiedad destino.</param>
    /// <param name="parameter">No se usa.</param>
    /// <param name="culture">Cultura del binding.</param>
    /// <returns>Un pincel.</returns>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value switch
        {
            "Infantil" => Brushes.SteelBlue,
            "Senior" => Brushes.DarkOrange,
            _ => Brushes.SeaGreen
        };

    /// <summary>No se usa: el color no se convierte de vuelta en categoría.</summary>
    /// <param name="value">El pincel.</param>
    /// <param name="targetType">Tipo de la propiedad origen.</param>
    /// <param name="parameter">No se usa.</param>
    /// <param name="culture">Cultura del binding.</param>
    /// <returns>Nunca devuelve.</returns>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
