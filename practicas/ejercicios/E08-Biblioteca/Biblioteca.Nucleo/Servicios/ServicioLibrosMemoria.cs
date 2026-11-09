using Biblioteca.Nucleo.Modelos;

namespace Biblioteca.Nucleo.Servicios;

/// <summary>
/// Guarda los libros en una lista en memoria. Se pierden al cerrar la aplicación.
/// Guarda y devuelve <b>copias</b>, como haría una base de datos.
/// </summary>
public class ServicioLibrosMemoria : IServicioLibros
{
    /// <summary>
    /// Crea el servicio con unos libros iniciales, a los que da identificadores nuevos (1, 2, 3…).
    /// </summary>
    /// <param name="iniciales">Libros con los que empieza; si se omite, ninguno.</param>
    public ServicioLibrosMemoria(IEnumerable<Libro>? iniciales = null)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public IReadOnlyList<Libro> ObtenerTodos()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Libro Guardar(Libro libro)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public void Eliminar(int id)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public void Prestar(int id, string lector)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public void Devolver(int id)
    {
        throw new NotImplementedException();
    }
}
