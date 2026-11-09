namespace Club.Nucleo.Modelos;

/// <summary>
/// Reglas de las cuotas del club, en un solo sitio.
/// </summary>
public static class CalculadoraCuotas
{
    /// <summary>Categorías de socio admitidas.</summary>
    public static string[] Categorias { get; } = ["Infantil", "Adulto", "Senior"];

    /// <summary>
    /// Calcula la cuota mensual: 15 € infantil, 25 € senior y 35 € adulto,
    /// con un 10 % de descuento a partir de 10 años de antigüedad.
    /// </summary>
    /// <param name="categoria">Categoría del socio.</param>
    /// <param name="antiguedad">Años como socio.</param>
    /// <returns>La cuota en euros.</returns>
    public static decimal Calcular(string categoria, int antiguedad)
    {
        decimal cuota = categoria switch
        {
            "Infantil" => 15m,
            "Senior" => 25m,
            _ => 35m
        };

        return antiguedad >= 10 ? cuota * 0.9m : cuota;
    }
}
