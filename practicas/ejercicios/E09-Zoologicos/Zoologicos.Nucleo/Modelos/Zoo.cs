namespace Zoologicos.Nucleo.Modelos;

/// <summary>
/// Un zoológico, identificado por su ciudad.
/// </summary>
public class Zoo
{
    /// <summary>Identificador en la base de datos.</summary>
    public int Id { get; set; }

    /// <summary>Ciudad del zoológico. No se repite.</summary>
    public string Ciudad { get; set; } = "";
}
