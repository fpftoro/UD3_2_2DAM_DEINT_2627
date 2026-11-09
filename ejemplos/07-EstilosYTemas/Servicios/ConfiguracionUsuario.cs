namespace EstilosYTemas.Servicios;

/// <summary>
/// Preferencias del usuario que se conservan entre una ejecución y la siguiente.
/// </summary>
public class ConfiguracionUsuario
{
    /// <summary>Posición horizontal de la ventana, o <c>null</c> si nunca se ha guardado.</summary>
    public double? Izquierda { get; set; }

    /// <summary>Posición vertical de la ventana, o <c>null</c> si nunca se ha guardado.</summary>
    public double? Arriba { get; set; }

    /// <summary>Ancho de la ventana sin maximizar.</summary>
    public double Ancho { get; set; } = 640;

    /// <summary>Alto de la ventana sin maximizar.</summary>
    public double Alto { get; set; } = 480;

    /// <summary>Si la ventana estaba maximizada al cerrarla.</summary>
    public bool Maximizada { get; set; }

    /// <summary>Tema elegido: «Sistema», «Claro» u «Oscuro».</summary>
    public string Tema { get; set; } = "Sistema";

    /// <summary>Color del club en formato #RRGGBB.</summary>
    public string ColorClub { get; set; } = "#2E7D32";
}
