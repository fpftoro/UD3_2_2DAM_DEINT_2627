namespace Biblioteca.Nucleo.Servicios;

/// <summary>
/// Mensajes al usuario. El ViewModel los pide a esta interfaz, sin saber que son ventanas de WPF;
/// las pruebas usan una implementación falsa que responde sola.
/// </summary>
public interface IServicioDialogos
{
    /// <summary>
    /// Hace una pregunta de sí o no.
    /// </summary>
    /// <param name="mensaje">La pregunta.</param>
    /// <param name="titulo">Título de la ventana.</param>
    /// <returns><c>true</c> si el usuario responde que sí.</returns>
    bool Confirmar(string mensaje, string titulo);

    /// <summary>
    /// Muestra un error.
    /// </summary>
    /// <param name="mensaje">Qué ha pasado.</param>
    /// <param name="titulo">Título de la ventana.</param>
    void MostrarError(string mensaje, string titulo);
}
