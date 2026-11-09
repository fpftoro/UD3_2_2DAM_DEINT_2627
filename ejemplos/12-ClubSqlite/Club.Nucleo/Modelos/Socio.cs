namespace Club.Nucleo.Modelos;

/// <summary>
/// Un socio del club. Es el modelo: solo datos, sin nada de la interfaz.
/// </summary>
public class Socio
{
    /// <summary>Identificador único (0 si todavía no se ha guardado).</summary>
    public int Id { get; set; }

    /// <summary>Nombre completo.</summary>
    public string Nombre { get; set; } = "";

    /// <summary>Categoría: Infantil, Adulto o Senior.</summary>
    public string Categoria { get; set; } = "Adulto";

    /// <summary>Años como socio.</summary>
    public int Antiguedad { get; set; }

    /// <summary>Cuota mensual según las reglas del club.</summary>
    public decimal Cuota => CalculadoraCuotas.Calcular(Categoria, Antiguedad);
}
