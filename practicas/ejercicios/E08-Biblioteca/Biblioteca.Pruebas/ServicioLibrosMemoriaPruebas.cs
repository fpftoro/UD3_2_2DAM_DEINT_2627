using Biblioteca.Nucleo.Modelos;
using Biblioteca.Nucleo.Servicios;

namespace Biblioteca.Pruebas;

public class ServicioLibrosMemoriaPruebas
{
    private static ServicioLibrosMemoria CrearServicio() => new(
    [
        new Libro { Titulo = "Platero y yo", Autor = "Juan Ramón Jiménez", Anio = 1914, Genero = "Poesía" },
        new Libro { Titulo = "Don Quijote de la Mancha", Autor = "Miguel de Cervantes", Anio = 1605, Lector = "Lucía Pérez" },
    ]);

    [Fact]
    public void ObtenerTodos_DevuelveLosLibrosOrdenadosPorTituloConIdentificadores()
    {
        var libros = CrearServicio().ObtenerTodos();

        Assert.Equal(["Don Quijote de la Mancha", "Platero y yo"], libros.Select(l => l.Titulo));
        Assert.All(libros, l => Assert.NotEqual(0, l.Id));
    }

    [Fact]
    public void ObtenerTodos_DevuelveCopias()
    {
        var servicio = CrearServicio();

        servicio.ObtenerTodos()[0].Titulo = "Cambiado sin guardar";

        Assert.Equal("Don Quijote de la Mancha", servicio.ObtenerTodos()[0].Titulo);
    }

    [Fact]
    public void Guardar_LibroNuevo_LeDaUnIdentificadorNuevo()
    {
        var servicio = CrearServicio();

        var guardado = servicio.Guardar(new Libro { Titulo = "Mafalda 1", Autor = "Quino", Anio = 1966, Genero = "Cómic" });

        Assert.Equal(3, guardado.Id);
        Assert.Equal(3, servicio.ObtenerTodos().Count);
    }

    [Fact]
    public void Guardar_LibroExistente_LoActualiza()
    {
        var servicio = CrearServicio();
        var platero = servicio.ObtenerTodos().Single(l => l.Titulo == "Platero y yo");
        platero.Anio = 1917;

        servicio.Guardar(platero);

        Assert.Equal(1917, servicio.ObtenerTodos().Single(l => l.Id == platero.Id).Anio);
        Assert.Equal(2, servicio.ObtenerTodos().Count);
    }

    [Fact]
    public void Guardar_LibroQueNoExiste_LanzaKeyNotFoundException()
    {
        Assert.Throws<KeyNotFoundException>(() => CrearServicio().Guardar(new Libro { Id = 99, Titulo = "X", Autor = "Y" }));
    }

    [Fact]
    public void Eliminar_LibroDisponible_LoQuita()
    {
        var servicio = CrearServicio();
        var platero = servicio.ObtenerTodos().Single(l => l.Titulo == "Platero y yo");

        servicio.Eliminar(platero.Id);

        Assert.DoesNotContain(servicio.ObtenerTodos(), l => l.Id == platero.Id);
    }

    [Fact]
    public void Eliminar_LibroPrestado_LanzaInvalidOperationException()
    {
        var servicio = CrearServicio();
        var quijote = servicio.ObtenerTodos().Single(l => l.Prestado);

        Assert.Throws<InvalidOperationException>(() => servicio.Eliminar(quijote.Id));
    }

    [Fact]
    public void Prestar_LibroDisponible_GuardaElLectorSinEspacios()
    {
        var servicio = CrearServicio();
        var platero = servicio.ObtenerTodos().Single(l => l.Titulo == "Platero y yo");

        servicio.Prestar(platero.Id, "  Mario Gil ");

        Assert.Equal("Mario Gil", servicio.ObtenerTodos().Single(l => l.Id == platero.Id).Lector);
    }

    [Fact]
    public void Prestar_LibroPrestado_LanzaInvalidOperationException()
    {
        var servicio = CrearServicio();
        var quijote = servicio.ObtenerTodos().Single(l => l.Prestado);

        Assert.Throws<InvalidOperationException>(() => servicio.Prestar(quijote.Id, "Mario Gil"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Prestar_SinLector_LanzaArgumentException(string lector)
    {
        var servicio = CrearServicio();
        var platero = servicio.ObtenerTodos().Single(l => l.Titulo == "Platero y yo");

        Assert.Throws<ArgumentException>(() => servicio.Prestar(platero.Id, lector));
    }

    [Fact]
    public void Devolver_LibroPrestado_LoDejaDisponible()
    {
        var servicio = CrearServicio();
        var quijote = servicio.ObtenerTodos().Single(l => l.Prestado);

        servicio.Devolver(quijote.Id);

        Assert.False(servicio.ObtenerTodos().Single(l => l.Id == quijote.Id).Prestado);
    }

    [Fact]
    public void Devolver_LibroDisponible_LanzaInvalidOperationException()
    {
        var servicio = CrearServicio();
        var platero = servicio.ObtenerTodos().Single(l => l.Titulo == "Platero y yo");

        Assert.Throws<InvalidOperationException>(() => servicio.Devolver(platero.Id));
    }
}
