using Microsoft.Data.Sqlite;
using Zoologicos.Nucleo.Datos;

namespace Zoologicos.Datos;

/// <summary>
/// Crea las tablas de la base de datos de los zoológicos y, si se pide, los datos de ejemplo.
/// </summary>
public static class BaseDatosZoos
{
    /// <summary>
    /// Las tres tablas. <c>ZooAnimal</c> relaciona varios a varios: su clave primaria son las dos claves ajenas.
    /// Al borrar un zoológico se borran sus filas de <c>ZooAnimal</c> (<c>ON DELETE CASCADE</c>); al borrar un
    /// animal, no: hay que quitarlo antes de los zoológicos.
    /// </summary>
    public const string Esquema = """
        CREATE TABLE IF NOT EXISTS Zoo (
            Id     INTEGER PRIMARY KEY AUTOINCREMENT,
            Ciudad TEXT    NOT NULL UNIQUE COLLATE NOCASE
        );

        CREATE TABLE IF NOT EXISTS Animal (
            Id      INTEGER PRIMARY KEY AUTOINCREMENT,
            Especie TEXT    NOT NULL UNIQUE COLLATE NOCASE
        );

        CREATE TABLE IF NOT EXISTS ZooAnimal (
            ZooId    INTEGER NOT NULL REFERENCES Zoo (Id) ON DELETE CASCADE,
            AnimalId INTEGER NOT NULL REFERENCES Animal (Id),
            PRIMARY KEY (ZooId, AnimalId)
        );
        """;

    private const string DatosEjemplo = """
        INSERT INTO Zoo (Ciudad) VALUES ('Madrid'), ('Barcelona'), ('Valencia');

        INSERT INTO Animal (Especie) VALUES
            ('Elefante'), ('Jirafa'), ('León'), ('Pingüino'), ('Tigre'), ('Cebra'), ('Oso panda'), ('Koala');

        INSERT INTO ZooAnimal (ZooId, AnimalId) VALUES
            (1, 1), (1, 2), (1, 7), (1, 5),
            (2, 1), (2, 3), (2, 4),
            (3, 2), (3, 3), (3, 6);
        """;

    /// <summary>
    /// Crea las tablas si no existen y, si se pide y la base está vacía, inserta los datos de ejemplo.
    /// </summary>
    /// <param name="cadenaConexion">Cadena de conexión de SQLite.</param>
    /// <param name="conDatosDeEjemplo">Si se insertan los datos de ejemplo en una base vacía.</param>
    /// <exception cref="DatosException">Si no se puede preparar la base de datos.</exception>
    public static void Inicializar(string cadenaConexion, bool conDatosDeEjemplo)
    {
        try
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
            contar.CommandText = "SELECT COUNT(*) FROM Zoo";
            if ((long)contar.ExecuteScalar()! > 0)
            {
                return;
            }

            using var insertar = conexion.CreateCommand();
            insertar.CommandText = DatosEjemplo;
            insertar.ExecuteNonQuery();
        }
        catch (SqliteException ex)
        {
            throw new DatosException($"No se ha podido preparar la base de datos: {ex.Message}", ex);
        }
    }
}
