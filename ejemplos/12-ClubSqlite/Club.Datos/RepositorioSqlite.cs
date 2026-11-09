using Club.Nucleo.Datos;
using Club.Nucleo.Modelos;
using Microsoft.Data.Sqlite;

namespace Club.Datos;

/// <summary>
/// Implementación de <see cref="IRepositorioClub"/> con SQLite.
/// Abre una conexión por operación y la cierra al terminar (<c>using</c>).
/// </summary>
public class RepositorioSqlite : IRepositorioClub
{
    // Código de error de SQLite cuando se incumple una restricción (UNIQUE, CHECK, FOREIGN KEY…)
    private const int ErrorRestriccion = 19;

    private readonly string _cadenaConexion;

    /// <summary>
    /// Crea el repositorio.
    /// </summary>
    /// <param name="cadenaConexion">Por ejemplo, <c>Data Source=club.db</c>.</param>
    public RepositorioSqlite(string cadenaConexion)
    {
        _cadenaConexion = cadenaConexion;
    }

    /// <inheritdoc />
    public IReadOnlyList<Socio> ObtenerSocios(string filtro = "")
    {
        return Consultar(conexion =>
        {
            using var comando = conexion.CreateCommand();
            // El filtro va en un parámetro: nunca se concatena lo que escribe el usuario
            comando.CommandText = """
                SELECT Id, Nombre, Categoria, Antiguedad
                FROM Socios
                WHERE Nombre LIKE '%' || $filtro || '%'
                ORDER BY Nombre
                """;
            comando.Parameters.AddWithValue("$filtro", filtro);

            using var lector = comando.ExecuteReader();
            var socios = new List<Socio>();
            while (lector.Read())
            {
                socios.Add(new Socio
                {
                    Id = lector.GetInt32(0),
                    Nombre = lector.GetString(1),
                    Categoria = lector.GetString(2),
                    Antiguedad = lector.GetInt32(3)
                });
            }

            return socios;
        });
    }

    /// <inheritdoc />
    public bool ExisteNombre(string nombre, int idExcluido)
    {
        return Consultar(conexion =>
        {
            using var comando = conexion.CreateCommand();
            comando.CommandText = "SELECT COUNT(*) FROM Socios WHERE Nombre = $nombre AND Id <> $id";
            comando.Parameters.AddWithValue("$nombre", nombre.Trim());
            comando.Parameters.AddWithValue("$id", idExcluido);
            return (long)comando.ExecuteScalar()! > 0;
        });
    }

    /// <inheritdoc />
    public int AgregarSocio(Socio socio)
    {
        return Consultar(conexion =>
        {
            using var comando = conexion.CreateCommand();
            comando.CommandText = """
                INSERT INTO Socios (Nombre, Categoria, Antiguedad)
                VALUES ($nombre, $categoria, $antiguedad)
                RETURNING Id
                """;
            AnadirParametros(comando, socio);
            return (int)(long)comando.ExecuteScalar()!;
        });
    }

    /// <inheritdoc />
    public void ActualizarSocio(Socio socio)
    {
        Consultar(conexion =>
        {
            using var comando = conexion.CreateCommand();
            comando.CommandText = """
                UPDATE Socios
                SET Nombre = $nombre, Categoria = $categoria, Antiguedad = $antiguedad
                WHERE Id = $id
                """;
            AnadirParametros(comando, socio);
            comando.Parameters.AddWithValue("$id", socio.Id);
            return comando.ExecuteNonQuery();
        });
    }

    /// <inheritdoc />
    public void EliminarSocio(int id)
    {
        Consultar(conexion =>
        {
            // ON DELETE CASCADE borra también sus inscripciones (con PRAGMA foreign_keys activo)
            using var comando = conexion.CreateCommand();
            comando.CommandText = "DELETE FROM Socios WHERE Id = $id";
            comando.Parameters.AddWithValue("$id", id);
            return comando.ExecuteNonQuery();
        });
    }

    /// <inheritdoc />
    public IReadOnlyList<Actividad> ObtenerActividades()
    {
        return Consultar(conexion =>
        {
            using var comando = conexion.CreateCommand();
            // LEFT JOIN: también salen las actividades sin inscritos (con 0)
            comando.CommandText = """
                SELECT a.Id, a.Nombre, a.Plazas, COUNT(i.SocioId) AS Inscritos
                FROM Actividades a
                LEFT JOIN Inscripciones i ON i.ActividadId = a.Id
                GROUP BY a.Id, a.Nombre, a.Plazas
                ORDER BY a.Nombre
                """;
            return LeerActividades(comando);
        });
    }

    /// <inheritdoc />
    public IReadOnlyList<Actividad> ObtenerActividadesDeSocio(int socioId)
    {
        return Consultar(conexion =>
        {
            using var comando = conexion.CreateCommand();
            // JOIN con la tabla de relación: las actividades de un socio
            comando.CommandText = """
                SELECT a.Id, a.Nombre, a.Plazas,
                       (SELECT COUNT(*) FROM Inscripciones x WHERE x.ActividadId = a.Id) AS Inscritos
                FROM Actividades a
                JOIN Inscripciones i ON i.ActividadId = a.Id
                WHERE i.SocioId = $socio
                ORDER BY a.Nombre
                """;
            comando.Parameters.AddWithValue("$socio", socioId);
            return LeerActividades(comando);
        });
    }

    /// <inheritdoc />
    public void Inscribir(int socioId, int actividadId)
    {
        Consultar(conexion =>
        {
            // Comprobar las plazas e insertar tienen que ir juntos: si no, dos inscripciones
            // simultáneas podrían ocupar la última plaza a la vez
            using var transaccion = conexion.BeginTransaction();

            using var plazas = conexion.CreateCommand();
            plazas.Transaction = transaccion;
            plazas.CommandText = """
                SELECT a.Plazas - (SELECT COUNT(*) FROM Inscripciones WHERE ActividadId = a.Id)
                FROM Actividades a
                WHERE a.Id = $actividad
                """;
            plazas.Parameters.AddWithValue("$actividad", actividadId);
            var libres = plazas.ExecuteScalar() as long?
                ?? throw new DatosException("La actividad ya no existe.");
            if (libres <= 0)
            {
                throw new DatosException("La actividad está completa: no quedan plazas libres.");
            }

            using var insertar = conexion.CreateCommand();
            insertar.Transaction = transaccion;
            insertar.CommandText = "INSERT INTO Inscripciones (SocioId, ActividadId) VALUES ($socio, $actividad)";
            insertar.Parameters.AddWithValue("$socio", socioId);
            insertar.Parameters.AddWithValue("$actividad", actividadId);
            insertar.ExecuteNonQuery();

            transaccion.Commit();
            return 0;
        });
    }

    /// <inheritdoc />
    public void DarDeBaja(int socioId, int actividadId)
    {
        Consultar(conexion =>
        {
            using var comando = conexion.CreateCommand();
            comando.CommandText = "DELETE FROM Inscripciones WHERE SocioId = $socio AND ActividadId = $actividad";
            comando.Parameters.AddWithValue("$socio", socioId);
            comando.Parameters.AddWithValue("$actividad", actividadId);
            return comando.ExecuteNonQuery();
        });
    }

    /// <summary>
    /// Abre la conexión, activa las claves ajenas, ejecuta la operación y la cierra.
    /// Traduce los errores de SQLite a <see cref="DatosException"/>.
    /// </summary>
    private T Consultar<T>(Func<SqliteConnection, T> operacion)
    {
        try
        {
            using var conexion = new SqliteConnection(_cadenaConexion);
            conexion.Open();

            // SQLite no comprueba las claves ajenas si no se activa en cada conexión
            using (var pragma = conexion.CreateCommand())
            {
                pragma.CommandText = "PRAGMA foreign_keys = ON";
                pragma.ExecuteNonQuery();
            }

            return operacion(conexion);
        }
        catch (SqliteException ex) when (ex.SqliteErrorCode == ErrorRestriccion)
        {
            throw new DatosException("Los datos no cumplen las reglas de la base de datos (un valor repetido o no permitido).", ex);
        }
        catch (SqliteException ex)
        {
            throw new DatosException($"No se ha podido acceder a la base de datos: {ex.Message}", ex);
        }
    }

    private static void AnadirParametros(SqliteCommand comando, Socio socio)
    {
        comando.Parameters.AddWithValue("$nombre", socio.Nombre);
        comando.Parameters.AddWithValue("$categoria", socio.Categoria);
        comando.Parameters.AddWithValue("$antiguedad", socio.Antiguedad);
    }

    private static List<Actividad> LeerActividades(SqliteCommand comando)
    {
        using var lector = comando.ExecuteReader();
        var actividades = new List<Actividad>();
        while (lector.Read())
        {
            actividades.Add(new Actividad
            {
                Id = lector.GetInt32(0),
                Nombre = lector.GetString(1),
                Plazas = lector.GetInt32(2),
                Inscritos = lector.GetInt32(3)
            });
        }

        return actividades;
    }
}
