using Club.Nucleo.Modelos;

namespace Club.Nucleo.Servicios;

/// <summary>
/// Socios guardados en memoria: se pierden al cerrar. En el tema 12 se cambia por SQLite
/// sin tocar los ViewModels.
/// </summary>
public class ServicioSociosMemoria : IServicioSocios
{
    private readonly List<Socio> _socios = [];
    private int _ultimoId;

    /// <summary>
    /// Crea el servicio con los socios indicados.
    /// </summary>
    /// <param name="iniciales">Socios de partida (se les asigna Id).</param>
    public ServicioSociosMemoria(IEnumerable<Socio>? iniciales = null)
    {
        foreach (var socio in iniciales ?? [])
        {
            Guardar(socio);
        }
    }

    /// <inheritdoc />
    public IReadOnlyList<Socio> ObtenerTodos() =>
        _socios.OrderBy(s => s.Nombre).Select(Copiar).ToList();

    /// <inheritdoc />
    public bool ExisteNombre(string nombre, int idExcluido) =>
        _socios.Any(s => s.Id != idExcluido && string.Equals(s.Nombre, nombre.Trim(), StringComparison.CurrentCultureIgnoreCase));

    /// <inheritdoc />
    public void Guardar(Socio socio)
    {
        if (socio.Id == 0)
        {
            socio.Id = ++_ultimoId;
            _socios.Add(Copiar(socio));
            return;
        }

        var indice = _socios.FindIndex(s => s.Id == socio.Id);
        if (indice < 0)
        {
            throw new InvalidOperationException($"No existe el socio {socio.Id}.");
        }

        _socios[indice] = Copiar(socio);
    }

    /// <inheritdoc />
    public void Eliminar(int id) => _socios.RemoveAll(s => s.Id == id);

    // Se guardan y se devuelven copias, como haría una base de datos: quien recibe un socio
    // no puede cambiar los datos guardados sin llamar a Guardar
    private static Socio Copiar(Socio s) =>
        new() { Id = s.Id, Nombre = s.Nombre, Categoria = s.Categoria, Antiguedad = s.Antiguedad };
}
