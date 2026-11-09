using Zoologicos.Nucleo.Datos;
using Zoologicos.Nucleo.Modelos;

namespace Zoologicos.Pruebas;

/// <summary>
/// Doble de prueba de <see cref="IRepositorioZoos"/> en memoria, con las mismas reglas que la base de datos
/// (nombres sin repetir, relaciones sin duplicar). Permite probar el ViewModel sin SQLite.
/// </summary>
public class RepositorioFalso : IRepositorioZoos
{
    private readonly List<Zoo> _zoos = [];
    private readonly List<Animal> _animales = [];
    private readonly HashSet<(int ZooId, int AnimalId)> _relaciones = [];
    private int _ultimoId;

    /// <summary>
    /// Crea el repositorio con dos zoológicos y tres animales: Madrid (Elefante, Jirafa) y Sevilla (sin animales);
    /// Koala no está en ningún zoológico.
    /// </summary>
    public RepositorioFalso()
    {
        var madrid = AnadirZoo("Madrid");
        AnadirZoo("Sevilla");
        var elefante = AnadirAnimal("Elefante");
        var jirafa = AnadirAnimal("Jirafa");
        AnadirAnimal("Koala");
        AnadirAnimalAZoo(madrid.Id, elefante.Id);
        AnadirAnimalAZoo(madrid.Id, jirafa.Id);
    }

    public IReadOnlyList<Zoo> ObtenerZoos() =>
        _zoos.OrderBy(z => z.Ciudad).Select(z => new Zoo { Id = z.Id, Ciudad = z.Ciudad }).ToList();

    public IReadOnlyList<Animal> ObtenerAnimales() =>
        _animales.OrderBy(a => a.Especie).Select(Copiar).ToList();

    public IReadOnlyList<Animal> ObtenerAnimalesDe(int zooId) =>
        _animales.Where(a => _relaciones.Contains((zooId, a.Id))).OrderBy(a => a.Especie).Select(Copiar).ToList();

    public Zoo AnadirZoo(string ciudad)
    {
        if (_zoos.Any(z => z.Ciudad.Equals(ciudad.Trim(), StringComparison.OrdinalIgnoreCase)))
        {
            throw new DatosException("Ese dato ya existe.");
        }

        var zoo = new Zoo { Id = ++_ultimoId, Ciudad = ciudad.Trim() };
        _zoos.Add(zoo);
        return new Zoo { Id = zoo.Id, Ciudad = zoo.Ciudad };
    }

    public void ActualizarZoo(Zoo zoo) => _zoos.Single(z => z.Id == zoo.Id).Ciudad = zoo.Ciudad.Trim();

    public void EliminarZoo(int id)
    {
        _zoos.RemoveAll(z => z.Id == id);
        _relaciones.RemoveWhere(r => r.ZooId == id);
    }

    public Animal AnadirAnimal(string especie)
    {
        if (_animales.Any(a => a.Especie.Equals(especie.Trim(), StringComparison.OrdinalIgnoreCase)))
        {
            throw new DatosException("Ese dato ya existe.");
        }

        var animal = new Animal { Id = ++_ultimoId, Especie = especie.Trim() };
        _animales.Add(animal);
        return Copiar(animal);
    }

    public void ActualizarAnimal(Animal animal) => _animales.Single(a => a.Id == animal.Id).Especie = animal.Especie.Trim();

    public void EliminarAnimal(int id)
    {
        _relaciones.RemoveWhere(r => r.AnimalId == id);
        _animales.RemoveAll(a => a.Id == id);
    }

    public void AnadirAnimalAZoo(int zooId, int animalId)
    {
        if (!_relaciones.Add((zooId, animalId)))
        {
            throw new DatosException("Ese dato ya existe.");
        }
    }

    public void QuitarAnimalDeZoo(int zooId, int animalId) => _relaciones.Remove((zooId, animalId));

    private static Animal Copiar(Animal a) => new() { Id = a.Id, Especie = a.Especie };
}
