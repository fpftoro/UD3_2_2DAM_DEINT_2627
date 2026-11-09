namespace Club.Nucleo.Modelos;

/// <summary>
/// Una actividad del club (pádel, natación…) con plazas limitadas.
/// </summary>
public class Actividad
{
    /// <summary>Identificador único.</summary>
    public int Id { get; set; }

    /// <summary>Nombre de la actividad.</summary>
    public string Nombre { get; set; } = "";

    /// <summary>Plazas totales.</summary>
    public int Plazas { get; set; }

    /// <summary>Socios inscritos (se calcula en la consulta).</summary>
    public int Inscritos { get; set; }

    /// <summary>Plazas que quedan libres.</summary>
    public int PlazasLibres => Plazas - Inscritos;
}
