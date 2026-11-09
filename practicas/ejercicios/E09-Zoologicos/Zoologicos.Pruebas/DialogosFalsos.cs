using Zoologicos.Nucleo.Servicios;

namespace Zoologicos.Pruebas;

/// <summary>
/// Doble de prueba de <see cref="IServicioDialogos"/>: responde lo que diga <see cref="Respuesta"/>
/// y apunta los mensajes, sin abrir ninguna ventana.
/// </summary>
public class DialogosFalsos : IServicioDialogos
{
    /// <summary>Lo que responde a las preguntas.</summary>
    public bool Respuesta { get; set; } = true;

    /// <summary>Las preguntas que se han hecho.</summary>
    public List<string> Preguntas { get; } = [];

    /// <summary>Los errores que se han mostrado.</summary>
    public List<string> Errores { get; } = [];

    /// <inheritdoc />
    public bool Confirmar(string mensaje, string titulo)
    {
        Preguntas.Add(mensaje);
        return Respuesta;
    }

    /// <inheritdoc />
    public void MostrarError(string mensaje, string titulo) => Errores.Add(mensaje);
}
