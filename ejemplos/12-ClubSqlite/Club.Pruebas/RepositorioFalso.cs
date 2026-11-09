using Club.Nucleo.Datos;
using Club.Nucleo.Modelos;

namespace Club.Pruebas;

/// <summary>
/// Doble de prueba de <see cref="IRepositorioClub"/> con listas en memoria.
/// Permite probar el ViewModel sin base de datos y simular errores.
/// </summary>
public class RepositorioFalso : IRepositorioClub
{
    private readonly List<(int SocioId, int ActividadId)> _inscripciones = [];

    /// <summary>Socios guardados.</summary>
    public List<Socio> Socios { get; } = [];

    /// <summary>Actividades disponibles.</summary>
    public List<Actividad> Actividades { get; } = [];

    /// <summary>Si es <c>true</c>, <see cref="Inscribir"/> falla como si la actividad estuviera completa.</summary>
    public bool FallarAlInscribir { get; set; }

    /// <inheritdoc />
    public IReadOnlyList<Socio> ObtenerSocios(string filtro = "") =>
        Socios.Where(s => s.Nombre.Contains(filtro, StringComparison.CurrentCultureIgnoreCase))
            .OrderBy(s => s.Nombre)
            .ToList();

    /// <inheritdoc />
    public bool ExisteNombre(string nombre, int idExcluido) =>
        Socios.Any(s => s.Id != idExcluido && s.Nombre.Equals(nombre, StringComparison.CurrentCultureIgnoreCase));

    /// <inheritdoc />
    public int AgregarSocio(Socio socio)
    {
        socio.Id = Socios.Count == 0 ? 1 : Socios.Max(s => s.Id) + 1;
        Socios.Add(socio);
        return socio.Id;
    }

    /// <inheritdoc />
    public void ActualizarSocio(Socio socio)
    {
        Socios[Socios.FindIndex(s => s.Id == socio.Id)] = socio;
    }

    /// <inheritdoc />
    public void EliminarSocio(int id)
    {
        Socios.RemoveAll(s => s.Id == id);
        _inscripciones.RemoveAll(i => i.SocioId == id);
    }

    /// <inheritdoc />
    public IReadOnlyList<Actividad> ObtenerActividades() =>
        Actividades.Select(a => new Actividad
        {
            Id = a.Id,
            Nombre = a.Nombre,
            Plazas = a.Plazas,
            Inscritos = _inscripciones.Count(i => i.ActividadId == a.Id)
        }).ToList();

    /// <inheritdoc />
    public IReadOnlyList<Actividad> ObtenerActividadesDeSocio(int socioId) =>
        ObtenerActividades().Where(a => _inscripciones.Contains((socioId, a.Id))).ToList();

    /// <inheritdoc />
    public void Inscribir(int socioId, int actividadId)
    {
        if (FallarAlInscribir)
        {
            throw new DatosException("La actividad está completa: no quedan plazas libres.");
        }

        _inscripciones.Add((socioId, actividadId));
    }

    /// <inheritdoc />
    public void DarDeBaja(int socioId, int actividadId) => _inscripciones.Remove((socioId, actividadId));
}
