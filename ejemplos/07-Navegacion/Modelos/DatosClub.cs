namespace Navegacion.Modelos;

/// <summary>
/// Datos en memoria que comparten todas las páginas de la aplicación.
/// </summary>
public static class DatosClub
{
    /// <summary>Socios del club.</summary>
    public static List<Socio> Socios { get; } =
    [
        new Socio { Nombre = "Lucía Pérez", Categoria = "Adulto", Antiguedad = 12 },
        new Socio { Nombre = "Mario Gil", Categoria = "Infantil", Antiguedad = 2 },
        new Socio { Nombre = "Carmen Ruiz", Categoria = "Senior", Antiguedad = 25 }
    ];
}
