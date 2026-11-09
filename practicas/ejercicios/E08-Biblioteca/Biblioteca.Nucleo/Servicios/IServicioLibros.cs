using Biblioteca.Nucleo.Modelos;

namespace Biblioteca.Nucleo.Servicios;

/// <summary>
/// Dónde se guardan los libros. El ViewModel solo conoce esta interfaz, así que se puede probar
/// con una implementación en memoria y cambiar más adelante a una base de datos.
/// </summary>
public interface IServicioLibros
{
    /// <summary>
    /// Devuelve copias de todos los libros, ordenados por título.
    /// </summary>
    /// <returns>Los libros; cambiarlos no cambia los guardados.</returns>
    IReadOnlyList<Libro> ObtenerTodos();

    /// <summary>
    /// Guarda un libro: si su <see cref="Libro.Id"/> es 0 lo añade con un identificador nuevo; si no, lo actualiza.
    /// </summary>
    /// <param name="libro">El libro que se guarda.</param>
    /// <returns>Una copia del libro guardado, con su identificador.</returns>
    /// <exception cref="KeyNotFoundException">Si se actualiza un libro que no existe.</exception>
    Libro Guardar(Libro libro);

    /// <summary>
    /// Elimina un libro.
    /// </summary>
    /// <param name="id">Identificador del libro.</param>
    /// <exception cref="KeyNotFoundException">Si no existe.</exception>
    /// <exception cref="InvalidOperationException">Si está prestado.</exception>
    void Eliminar(int id);

    /// <summary>
    /// Presta un libro.
    /// </summary>
    /// <param name="id">Identificador del libro.</param>
    /// <param name="lector">Quién se lo lleva (se guarda sin espacios en los extremos).</param>
    /// <exception cref="ArgumentException">Si el lector está vacío.</exception>
    /// <exception cref="KeyNotFoundException">Si no existe.</exception>
    /// <exception cref="InvalidOperationException">Si ya está prestado.</exception>
    void Prestar(int id, string lector);

    /// <summary>
    /// Registra la devolución de un libro.
    /// </summary>
    /// <param name="id">Identificador del libro.</param>
    /// <exception cref="KeyNotFoundException">Si no existe.</exception>
    /// <exception cref="InvalidOperationException">Si no está prestado.</exception>
    void Devolver(int id);
}
