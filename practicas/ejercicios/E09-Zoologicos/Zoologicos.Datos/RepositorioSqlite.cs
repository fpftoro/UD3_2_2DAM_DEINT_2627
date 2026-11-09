using Microsoft.Data.Sqlite;
using Zoologicos.Nucleo.Datos;
using Zoologicos.Nucleo.Modelos;

namespace Zoologicos.Datos;

/// <summary>
/// Implementación de <see cref="IRepositorioZoos"/> con SQLite. Abre una conexión por operación,
/// activa las claves ajenas (<c>PRAGMA foreign_keys = ON</c>) y traduce los errores de SQLite
/// (<see cref="SqliteException"/>) a <see cref="DatosException"/>.
/// </summary>
public class RepositorioSqlite : IRepositorioZoos
{
    /// <summary>
    /// Crea el repositorio.
    /// </summary>
    /// <param name="cadenaConexion">Cadena de conexión de SQLite (<c>Data Source=…</c>).</param>
    public RepositorioSqlite(string cadenaConexion)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public IReadOnlyList<Zoo> ObtenerZoos()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public IReadOnlyList<Animal> ObtenerAnimales()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public IReadOnlyList<Animal> ObtenerAnimalesDe(int zooId)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Zoo AnadirZoo(string ciudad)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public void ActualizarZoo(Zoo zoo)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public void EliminarZoo(int id)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Animal AnadirAnimal(string especie)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public void ActualizarAnimal(Animal animal)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public void EliminarAnimal(int id)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public void AnadirAnimalAZoo(int zooId, int animalId)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public void QuitarAnimalDeZoo(int zooId, int animalId)
    {
        throw new NotImplementedException();
    }
}
