using Club.Nucleo.Servicios;

namespace Club.Pruebas;

/// <summary>
/// Doble de prueba de <see cref="IServicioDialogos"/>: responde lo que se le diga y apunta lo que se le pregunta.
/// </summary>
public class DialogosFalsos : IServicioDialogos
{
    /// <summary>Respuesta que dará <see cref="Confirmar"/>.</summary>
    public bool Respuesta { get; set; } = true;

    /// <summary>Preguntas recibidas, en orden.</summary>
    public List<string> Preguntas { get; } = [];

    /// <inheritdoc />
    public bool Confirmar(string mensaje, string titulo)
    {
        Preguntas.Add(mensaje);
        return Respuesta;
    }

    /// <inheritdoc />
    public void MostrarError(string mensaje, string titulo) => Preguntas.Add(mensaje);
}
