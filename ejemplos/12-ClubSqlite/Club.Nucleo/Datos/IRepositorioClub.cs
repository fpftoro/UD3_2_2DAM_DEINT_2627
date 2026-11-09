using Club.Nucleo.Modelos;

namespace Club.Nucleo.Datos;

/// <summary>
/// Acceso a los datos del club. Los ViewModels dependen de esta interfaz y no saben
/// si detrás hay SQLite, otra base de datos o una lista en memoria (en las pruebas).
/// </summary>
public interface IRepositorioClub
{
    /// <summary>Devuelve los socios cuyo nombre contiene el filtro, ordenados por nombre.</summary>
    /// <param name="filtro">Texto que se busca en el nombre; vacío para todos.</param>
    /// <returns>Los socios encontrados.</returns>
    IReadOnlyList<Socio> ObtenerSocios(string filtro = "");

    /// <summary>Indica si otro socio tiene ya ese nombre.</summary>
    /// <param name="nombre">Nombre que se comprueba.</param>
    /// <param name="idExcluido">Id del socio que se edita (0 en un alta).</param>
    /// <returns><c>true</c> si el nombre está repetido.</returns>
    bool ExisteNombre(string nombre, int idExcluido);

    /// <summary>Da de alta un socio.</summary>
    /// <param name="socio">Datos del socio (se ignora su Id).</param>
    /// <returns>El Id que le asigna la base de datos.</returns>
    int AgregarSocio(Socio socio);

    /// <summary>Guarda los cambios de un socio existente.</summary>
    /// <param name="socio">El socio, con su Id.</param>
    void ActualizarSocio(Socio socio);

    /// <summary>Elimina un socio y sus inscripciones.</summary>
    /// <param name="id">Id del socio.</param>
    void EliminarSocio(int id);

    /// <summary>Devuelve todas las actividades con su número de inscritos.</summary>
    /// <returns>Las actividades, ordenadas por nombre.</returns>
    IReadOnlyList<Actividad> ObtenerActividades();

    /// <summary>Devuelve las actividades en las que está inscrito un socio.</summary>
    /// <param name="socioId">Id del socio.</param>
    /// <returns>Sus actividades, ordenadas por nombre.</returns>
    IReadOnlyList<Actividad> ObtenerActividadesDeSocio(int socioId);

    /// <summary>Inscribe a un socio en una actividad.</summary>
    /// <param name="socioId">Id del socio.</param>
    /// <param name="actividadId">Id de la actividad.</param>
    /// <exception cref="DatosException">Si la actividad está completa o el socio ya estaba inscrito.</exception>
    void Inscribir(int socioId, int actividadId);

    /// <summary>Da de baja a un socio de una actividad.</summary>
    /// <param name="socioId">Id del socio.</param>
    /// <param name="actividadId">Id de la actividad.</param>
    void DarDeBaja(int socioId, int actividadId);
}
