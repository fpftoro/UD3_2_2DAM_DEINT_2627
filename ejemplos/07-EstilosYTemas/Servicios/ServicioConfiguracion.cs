using System.IO;
using System.Text.Json;

namespace EstilosYTemas.Servicios;

/// <summary>
/// Guarda y recupera la <see cref="ConfiguracionUsuario"/> en un fichero JSON.
/// </summary>
public class ServicioConfiguracion
{
    private static readonly JsonSerializerOptions _opciones = new() { WriteIndented = true };

    private readonly string _ruta;

    /// <summary>
    /// Crea el servicio con el fichero por defecto:
    /// <c>%AppData%\ClubDeportivo\configuracion.json</c>.
    /// </summary>
    public ServicioConfiguracion()
        : this(Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ClubDeportivo",
            "configuracion.json"))
    {
    }

    /// <summary>
    /// Crea el servicio con un fichero concreto (útil para probarlo).
    /// </summary>
    /// <param name="ruta">Ruta completa del fichero JSON.</param>
    public ServicioConfiguracion(string ruta)
    {
        _ruta = ruta;
    }

    /// <summary>
    /// Lee la configuración guardada. Si no existe o está dañada, devuelve la de por defecto.
    /// </summary>
    /// <returns>La configuración del usuario.</returns>
    public ConfiguracionUsuario Cargar()
    {
        try
        {
            if (File.Exists(_ruta))
            {
                var json = File.ReadAllText(_ruta);
                return JsonSerializer.Deserialize<ConfiguracionUsuario>(json) ?? new ConfiguracionUsuario();
            }
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or JsonException)
        {
            // Un fichero ilegible no debe impedir que la aplicación arranque
        }

        return new ConfiguracionUsuario();
    }

    /// <summary>
    /// Guarda la configuración, creando la carpeta si no existe.
    /// </summary>
    /// <param name="configuracion">La configuración que se guarda.</param>
    public void Guardar(ConfiguracionUsuario configuracion)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_ruta)!);
        File.WriteAllText(_ruta, JsonSerializer.Serialize(configuracion, _opciones));
    }
}
