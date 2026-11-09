using Zoologicos.Nucleo.Modelos;

namespace Zoologicos.Nucleo.Datos;

/// <summary>
/// Acceso a los zoológicos, los animales y qué animales hay en cada zoológico.
/// El ViewModel solo conoce esta interfaz: no sabe que los datos están en SQLite.
/// Todos los métodos lanzan <see cref="DatosException"/> si la base de datos rechaza la operación
/// (un nombre repetido, una relación que ya existe) o no se puede acceder a ella.
/// </summary>
public interface IRepositorioZoos
{
    /// <summary>Todos los zoológicos, ordenados por ciudad.</summary>
    /// <returns>Los zoológicos.</returns>
    IReadOnlyList<Zoo> ObtenerZoos();

    /// <summary>Todos los animales, ordenados por especie.</summary>
    /// <returns>Los animales.</returns>
    IReadOnlyList<Animal> ObtenerAnimales();

    /// <summary>Los animales de un zoológico, ordenados por especie (consulta con <c>JOIN</c>).</summary>
    /// <param name="zooId">Identificador del zoológico.</param>
    /// <returns>Sus animales; vacía si no tiene ninguno.</returns>
    IReadOnlyList<Animal> ObtenerAnimalesDe(int zooId);

    /// <summary>Añade un zoológico.</summary>
    /// <param name="ciudad">Su ciudad, que no puede estar repetida (sin distinguir mayúsculas).</param>
    /// <returns>El zoológico creado, con su identificador.</returns>
    Zoo AnadirZoo(string ciudad);

    /// <summary>Cambia la ciudad de un zoológico.</summary>
    /// <param name="zoo">El zoológico, con su identificador y la ciudad nueva.</param>
    void ActualizarZoo(Zoo zoo);

    /// <summary>Elimina un zoológico y, con él, la relación con sus animales (los animales siguen existiendo).</summary>
    /// <param name="id">Identificador del zoológico.</param>
    void EliminarZoo(int id);

    /// <summary>Añade un animal.</summary>
    /// <param name="especie">Su especie, que no puede estar repetida (sin distinguir mayúsculas).</param>
    /// <returns>El animal creado, con su identificador.</returns>
    Animal AnadirAnimal(string especie);

    /// <summary>Cambia la especie de un animal.</summary>
    /// <param name="animal">El animal, con su identificador y la especie nueva.</param>
    void ActualizarAnimal(Animal animal);

    /// <summary>
    /// Elimina un animal: lo quita de todos los zoológicos y lo borra, en una sola transacción.
    /// </summary>
    /// <param name="id">Identificador del animal.</param>
    void EliminarAnimal(int id);

    /// <summary>Añade un animal a un zoológico.</summary>
    /// <param name="zooId">Identificador del zoológico.</param>
    /// <param name="animalId">Identificador del animal; no puede estar ya en ese zoológico.</param>
    void AnadirAnimalAZoo(int zooId, int animalId);

    /// <summary>Quita un animal de un zoológico (el animal sigue existiendo).</summary>
    /// <param name="zooId">Identificador del zoológico.</param>
    /// <param name="animalId">Identificador del animal.</param>
    void QuitarAnimalDeZoo(int zooId, int animalId);
}
