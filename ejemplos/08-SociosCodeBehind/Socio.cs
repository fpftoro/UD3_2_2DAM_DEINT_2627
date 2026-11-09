namespace SociosCodeBehind;

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
}
