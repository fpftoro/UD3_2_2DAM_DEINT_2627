using Club.Nucleo.Datos;
using Microsoft.Data.Sqlite;

namespace Club.Datos;

/// <summary>
/// Crea las tablas de la base de datos del club si no existen y, si se pide, carga datos de ejemplo.
/// </summary>
public static class BaseDatosClub
{
    /// <summary>Instrucciones SQL que crean el esquema. Se pueden ejecutar varias veces.</summary>
    public const string Esquema = """
        CREATE TABLE IF NOT EXISTS Socios (
            Id         INTEGER PRIMARY KEY AUTOINCREMENT,
            Nombre     TEXT    NOT NULL UNIQUE COLLATE NOCASE,
            Categoria  TEXT    NOT NULL CHECK (Categoria IN ('Infantil', 'Adulto', 'Senior')),
            Antiguedad INTEGER NOT NULL CHECK (Antiguedad BETWEEN 0 AND 100)
        );

        CREATE TABLE IF NOT EXISTS Actividades (
            Id     INTEGER PRIMARY KEY AUTOINCREMENT,
            Nombre TEXT    NOT NULL UNIQUE,
            Plazas INTEGER NOT NULL CHECK (Plazas > 0)
        );

        CREATE TABLE IF NOT EXISTS Inscripciones (
            SocioId     INTEGER NOT NULL REFERENCES Socios (Id) ON DELETE CASCADE,
            ActividadId INTEGER NOT NULL REFERENCES Actividades (Id) ON DELETE CASCADE,
            PRIMARY KEY (SocioId, ActividadId)
        );
        """;

    private const string DatosEjemplo = """
        INSERT INTO Socios (Nombre, Categoria, Antiguedad) VALUES
            ('Lucía Pérez', 'Adulto', 12),
            ('Mario Gil', 'Infantil', 2),
            ('Carmen Ruiz', 'Senior', 25),
            ('Javier Soto', 'Adulto', 4),
            ('Elena Martín', 'Infantil', 6);

        INSERT INTO Actividades (Nombre, Plazas) VALUES
            ('Pádel', 8),
            ('Natación', 12),
            ('Yoga', 2),
            ('Atletismo', 10);

        INSERT INTO Inscripciones (SocioId, ActividadId) VALUES
            (1, 1), (1, 3), (2, 2), (3, 3), (4, 1), (5, 2), (5, 4);
        """;

    /// <summary>
    /// Crea las tablas que falten y, si la base está vacía y se pide, inserta los datos de ejemplo.
    /// </summary>
    /// <param name="cadenaConexion">Cadena de conexión de SQLite.</param>
    /// <param name="conDatosDeEjemplo">Si se cargan los datos de ejemplo en una base vacía.</param>
    /// <exception cref="DatosException">Si no se puede abrir o crear la base de datos.</exception>
    public static void Inicializar(string cadenaConexion, bool conDatosDeEjemplo)
    {
        try
        {
            CrearEsquema(cadenaConexion, conDatosDeEjemplo);
        }
        catch (SqliteException ex)
        {
            throw new DatosException($"No se ha podido preparar la base de datos: {ex.Message}", ex);
        }
    }

    private static void CrearEsquema(string cadenaConexion, bool conDatosDeEjemplo)
    {
        using var conexion = new SqliteConnection(cadenaConexion);
        conexion.Open();

        using var crear = conexion.CreateCommand();
        crear.CommandText = Esquema;
        crear.ExecuteNonQuery();

        if (!conDatosDeEjemplo)
        {
            return;
        }

        using var contar = conexion.CreateCommand();
        contar.CommandText = "SELECT COUNT(*) FROM Socios";
        if ((long)contar.ExecuteScalar()! > 0)
        {
            return;
        }

        using var insertar = conexion.CreateCommand();
        insertar.CommandText = DatosEjemplo;
        insertar.ExecuteNonQuery();
    }
}
