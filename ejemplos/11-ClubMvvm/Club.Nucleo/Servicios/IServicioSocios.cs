using Club.Nucleo.Modelos;

namespace Club.Nucleo.Servicios;

/// <summary>
/// Dónde se guardan los socios. Los ViewModels solo conocen esta interfaz,
/// no si los datos están en memoria, en un fichero o en una base de datos.
/// </summary>
public interface IServicioSocios
{
    /// <summary>Devuelve todos los socios, ordenados por nombre.</summary>
    /// <returns>Una copia de la lista de socios.</returns>
    IReadOnlyList<Socio> ObtenerTodos();

    /// <summary>Indica si ya existe otro socio con ese nombre (sin distinguir mayúsculas).</summary>
    /// <param name="nombre">Nombre que se comprueba.</param>
    /// <param name="idExcluido">Id del socio que se está editando, para no compararlo consigo mismo.</param>
    /// <returns><c>true</c> si el nombre está repetido.</returns>
    bool ExisteNombre(string nombre, int idExcluido);

    /// <summary>Da de alta (si <c>Id</c> es 0) o actualiza un socio.</summary>
    /// <param name="socio">El socio que se guarda; al darlo de alta recibe su Id.</param>
    void Guardar(Socio socio);

    /// <summary>Elimina un socio.</summary>
    /// <param name="id">Id del socio.</param>
    void Eliminar(int id);
}
