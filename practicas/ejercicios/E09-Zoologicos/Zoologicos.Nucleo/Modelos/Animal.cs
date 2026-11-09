namespace Zoologicos.Nucleo.Modelos;

/// <summary>
/// Una especie animal, que puede vivir en varios zoológicos.
/// </summary>
public class Animal
{
    /// <summary>Identificador en la base de datos.</summary>
    public int Id { get; set; }

    /// <summary>Nombre de la especie. No se repite.</summary>
    public string Especie { get; set; } = "";
}
