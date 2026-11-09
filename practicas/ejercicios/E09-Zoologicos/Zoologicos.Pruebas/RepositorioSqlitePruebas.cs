using Microsoft.Data.Sqlite;
using Zoologicos.Datos;
using Zoologicos.Nucleo.Datos;
using Zoologicos.Nucleo.Modelos;

namespace Zoologicos.Pruebas;

/// <summary>
/// Pruebas del repositorio contra una base de datos SQLite en memoria con los datos de ejemplo:
/// es real, pero no deja ficheros y cada prueba empieza con los mismos datos.
/// </summary>
public sealed class RepositorioSqlitePruebas : IDisposable
{
    private readonly SqliteConnection _mantenerViva;
    private readonly RepositorioSqlite _repositorio;

    public RepositorioSqlitePruebas()
    {
        // Una base en memoria desaparece al cerrar su última conexión. Con un nombre propio
        // y Cache=Shared, las conexiones del repositorio comparten la misma base mientras
        // esta primera conexión siga abierta
        var cadena = $"Data Source=prueba-{Guid.NewGuid()};Mode=Memory;Cache=Shared";
        _mantenerViva = new SqliteConnection(cadena);
        _mantenerViva.Open();

        BaseDatosZoos.Inicializar(cadena, conDatosDeEjemplo: true);
        _repositorio = new RepositorioSqlite(cadena);
    }

    public void Dispose() => _mantenerViva.Dispose();

    private Zoo Zoo(string ciudad) => _repositorio.ObtenerZoos().Single(z => z.Ciudad == ciudad);

    private Animal Animal(string especie) => _repositorio.ObtenerAnimales().Single(a => a.Especie == especie);

    private long FilasDeZooAnimal(string condicion)
    {
        using var comando = _mantenerViva.CreateCommand();
        comando.CommandText = $"SELECT COUNT(*) FROM ZooAnimal WHERE {condicion}";
        return (long)comando.ExecuteScalar()!;
    }

    [Fact]
    public void ObtenerZoos_DevuelveLosDeEjemploOrdenadosPorCiudad()
    {
        Assert.Equal(["Barcelona", "Madrid", "Valencia"], _repositorio.ObtenerZoos().Select(z => z.Ciudad));
    }

    [Fact]
    public void ObtenerAnimales_DevuelveTodosOrdenadosPorEspecie()
    {
        Assert.Equal(
            ["Cebra", "Elefante", "Jirafa", "Koala", "León", "Oso panda", "Pingüino", "Tigre"],
            _repositorio.ObtenerAnimales().Select(a => a.Especie));
    }

    [Fact]
    public void ObtenerAnimalesDe_DevuelveLosDelZooOrdenados()
    {
        var animales = _repositorio.ObtenerAnimalesDe(Zoo("Madrid").Id);

        Assert.Equal(["Elefante", "Jirafa", "Oso panda", "Tigre"], animales.Select(a => a.Especie));
    }

    [Fact]
    public void ObtenerAnimalesDe_ZooSinAnimales_DevuelveUnaListaVacia()
    {
        var sevilla = _repositorio.AnadirZoo("Sevilla");

        Assert.Empty(_repositorio.ObtenerAnimalesDe(sevilla.Id));
    }

    [Fact]
    public void AnadirZoo_DevuelveElZooConSuIdentificador()
    {
        var zoo = _repositorio.AnadirZoo("  Sevilla ");

        Assert.NotEqual(0, zoo.Id);
        Assert.Equal("Sevilla", zoo.Ciudad);
        Assert.Contains(_repositorio.ObtenerZoos(), z => z.Id == zoo.Id && z.Ciudad == "Sevilla");
    }

    [Fact]
    public void AnadirZoo_ConComillas_SeGuardaTalCual()
    {
        var zoo = _repositorio.AnadirZoo("L'Hospitalet");

        Assert.Equal("L'Hospitalet", _repositorio.ObtenerZoos().Single(z => z.Id == zoo.Id).Ciudad);
    }

    [Fact]
    public void AnadirZoo_CiudadRepetida_LanzaDatosException()
    {
        Assert.Throws<DatosException>(() => _repositorio.AnadirZoo("madrid"));
    }

    [Fact]
    public void ActualizarZoo_CambiaLaCiudad()
    {
        var valencia = Zoo("Valencia");

        _repositorio.ActualizarZoo(new Zoo { Id = valencia.Id, Ciudad = "València" });

        Assert.Equal("València", _repositorio.ObtenerZoos().Single(z => z.Id == valencia.Id).Ciudad);
    }

    [Fact]
    public void EliminarZoo_BorraTambienSuRelacionConLosAnimales()
    {
        var madrid = Zoo("Madrid");

        _repositorio.EliminarZoo(madrid.Id);

        Assert.DoesNotContain(_repositorio.ObtenerZoos(), z => z.Id == madrid.Id);
        Assert.Equal(0, FilasDeZooAnimal($"ZooId = {madrid.Id}"));
        Assert.Equal(8, _repositorio.ObtenerAnimales().Count); // los animales siguen existiendo
    }

    [Fact]
    public void AnadirAnimal_DevuelveElAnimalConSuIdentificador()
    {
        var animal = _repositorio.AnadirAnimal("Lince ibérico");

        Assert.NotEqual(0, animal.Id);
        Assert.Contains(_repositorio.ObtenerAnimales(), a => a.Id == animal.Id);
    }

    [Fact]
    public void AnadirAnimal_EspecieRepetida_LanzaDatosException()
    {
        Assert.Throws<DatosException>(() => _repositorio.AnadirAnimal("JIRAFA"));
    }

    [Fact]
    public void ActualizarAnimal_CambiaLaEspecieTambienEnLosZoos()
    {
        var oso = Animal("Oso panda");

        _repositorio.ActualizarAnimal(new Animal { Id = oso.Id, Especie = "Panda gigante" });

        Assert.Contains(_repositorio.ObtenerAnimalesDe(Zoo("Madrid").Id), a => a.Especie == "Panda gigante");
    }

    [Fact]
    public void EliminarAnimal_LoQuitaDeLosZoosYLoBorra()
    {
        var elefante = Animal("Elefante");

        _repositorio.EliminarAnimal(elefante.Id);

        Assert.Equal(7, _repositorio.ObtenerAnimales().Count);
        Assert.Equal(0, FilasDeZooAnimal($"AnimalId = {elefante.Id}"));
        Assert.DoesNotContain(_repositorio.ObtenerAnimalesDe(Zoo("Barcelona").Id), a => a.Id == elefante.Id);
    }

    [Fact]
    public void EliminarAnimal_QueNoEstaEnNingunZoo_LoBorra()
    {
        _repositorio.EliminarAnimal(Animal("Koala").Id);

        Assert.DoesNotContain(_repositorio.ObtenerAnimales(), a => a.Especie == "Koala");
    }

    [Fact]
    public void AnadirAnimalAZoo_LoAnade()
    {
        _repositorio.AnadirAnimalAZoo(Zoo("Valencia").Id, Animal("Koala").Id);

        Assert.Contains(_repositorio.ObtenerAnimalesDe(Zoo("Valencia").Id), a => a.Especie == "Koala");
    }

    [Fact]
    public void AnadirAnimalAZoo_QueYaLoTiene_LanzaDatosException()
    {
        Assert.Throws<DatosException>(() => _repositorio.AnadirAnimalAZoo(Zoo("Madrid").Id, Animal("Tigre").Id));
    }

    [Fact]
    public void AnadirAnimalAZoo_ZooQueNoExiste_LanzaDatosException()
    {
        // La clave ajena solo se comprueba con PRAGMA foreign_keys = ON
        Assert.Throws<DatosException>(() => _repositorio.AnadirAnimalAZoo(999, Animal("Koala").Id));
    }

    [Fact]
    public void QuitarAnimalDeZoo_LoQuitaSinBorrarElAnimal()
    {
        var barcelona = Zoo("Barcelona");
        var pinguino = Animal("Pingüino");

        _repositorio.QuitarAnimalDeZoo(barcelona.Id, pinguino.Id);

        Assert.DoesNotContain(_repositorio.ObtenerAnimalesDe(barcelona.Id), a => a.Id == pinguino.Id);
        Assert.Contains(_repositorio.ObtenerAnimales(), a => a.Id == pinguino.Id);
    }

    [Fact]
    public void SinAccesoALaBaseDeDatos_LanzaDatosException()
    {
        var repositorio = new RepositorioSqlite("Data Source=/carpeta/que/no/existe/zoologicos.db");

        Assert.Throws<DatosException>(() => repositorio.ObtenerZoos());
    }
}
