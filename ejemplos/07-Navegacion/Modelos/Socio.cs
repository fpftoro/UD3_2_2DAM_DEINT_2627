namespace Navegacion.Modelos;

/// <summary>
/// Un socio del club.
/// </summary>
public class Socio
{
    /// <summary>Nombre completo.</summary>
    public string Nombre { get; set; } = "";

    /// <summary>Categoría: Infantil, Adulto o Senior.</summary>
    public string Categoria { get; set; } = "Adulto";

    /// <summary>Años como socio.</summary>
    public int Antiguedad { get; set; }

    /// <summary>Texto que muestran las listas.</summary>
    /// <returns>El nombre y la categoría.</returns>
    public override string ToString() => $"{Nombre} ({Categoria})";
}
