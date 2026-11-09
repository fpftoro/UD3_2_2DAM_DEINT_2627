namespace Club.Nucleo.Servicios;

/// <summary>
/// Mensajes al usuario. El ViewModel los pide a esta interfaz y no a <c>MessageBox</c>,
/// para no depender de WPF y poder probarse sin ventanas.
/// </summary>
public interface IServicioDialogos
{
    /// <summary>Hace una pregunta de sí o no.</summary>
    /// <param name="mensaje">La pregunta.</param>
    /// <param name="titulo">Título del mensaje.</param>
    /// <returns><c>true</c> si el usuario responde que sí.</returns>
    bool Confirmar(string mensaje, string titulo);

    /// <summary>Muestra un error.</summary>
    /// <param name="mensaje">Qué ha pasado y qué puede hacer el usuario.</param>
    /// <param name="titulo">Título del mensaje.</param>
    void MostrarError(string mensaje, string titulo);
}
