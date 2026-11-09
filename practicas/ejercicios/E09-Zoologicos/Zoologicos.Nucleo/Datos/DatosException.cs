namespace Zoologicos.Nucleo.Datos;

/// <summary>
/// Error al leer o guardar datos, con un mensaje para el usuario.
/// El repositorio traduce a esta excepción los errores propios de la base de datos,
/// para que el ViewModel no tenga que conocerlos.
/// </summary>
public class DatosException : Exception
{
    /// <summary>
    /// Crea la excepción.
    /// </summary>
    /// <param name="mensaje">Mensaje para el usuario.</param>
    /// <param name="interna">Excepción original, si la hay.</param>
    public DatosException(string mensaje, Exception? interna = null)
        : base(mensaje, interna)
    {
    }
}
