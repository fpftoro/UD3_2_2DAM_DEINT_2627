using Biblioteca.Nucleo.Modelos;
using Biblioteca.Nucleo.Servicios;
using Biblioteca.Nucleo.ViewModels;

namespace Biblioteca.Pruebas;

public class LibrosViewModelPruebas
{
    private readonly DialogosFalsos _dialogos = new();

    private LibrosViewModel CrearViewModel() =>
        new(new ServicioLibrosMemoria(
            [
                new Libro { Titulo = "Platero y yo", Autor = "Juan Ramón Jiménez", Anio = 1914, Genero = "Poesía" },
                new Libro { Titulo = "Don Quijote de la Mancha", Autor = "Miguel de Cervantes", Anio = 1605, Lector = "Lucía Pérez" },
                new Libro { Titulo = "Mafalda 1", Autor = "Quino", Anio = 1966, Genero = "Cómic" },
            ]),
            _dialogos);

    private static void Elegir(LibrosViewModel vm, string titulo) =>
        vm.Seleccionado = vm.LibrosVisibles.Single(l => l.Titulo == titulo);

    [Fact]
    public void AlCrearse_CargaLosLibrosYElResumen()
    {
        var vm = CrearViewModel();

        Assert.Equal(["Don Quijote de la Mancha", "Mafalda 1", "Platero y yo"], vm.LibrosVisibles.Select(l => l.Titulo));
        Assert.Equal(2, vm.Disponibles);
        Assert.Equal(1, vm.Prestados);
        Assert.Null(vm.Ficha);
    }

    [Theory]
    [InlineData("platero", "Platero y yo")]
    [InlineData("QUINO", "Mafalda 1")]
    public void Buscar_FiltraPorTituloOAutorSinDistinguirMayusculas(string buscar, string esperado)
    {
        var vm = CrearViewModel();

        vm.Buscar = buscar;

        Assert.Equal(esperado, Assert.Single(vm.LibrosVisibles).Titulo);
        Assert.Equal(2, vm.Disponibles); // el resumen es de todos los libros
    }

    [Fact]
    public void ElegirUnLibro_AbreSuFicha()
    {
        var vm = CrearViewModel();

        Elegir(vm, "Mafalda 1");

        Assert.Equal("Quino", vm.Ficha?.Autor);
        Assert.True(vm.HayFicha);
    }

    [Fact]
    public void Guardar_NoSePuedeConLaFichaNuevaVacia()
    {
        var vm = CrearViewModel();

        vm.NuevoCommand.Execute(null);

        Assert.Null(vm.Seleccionado);
        Assert.False(vm.GuardarCommand.CanExecute(null));
    }

    [Fact]
    public void Guardar_SeActivaYAvisaAlCorregirLaFicha()
    {
        var vm = CrearViewModel();
        vm.NuevoCommand.Execute(null);
        var avisos = 0;
        vm.GuardarCommand.CanExecuteChanged += (_, _) => avisos++;

        vm.Ficha!.Titulo = "La sombra del viento";
        vm.Ficha.Autor = "Carlos Ruiz Zafón";
        vm.Ficha.AnioTexto = "2001";

        Assert.True(vm.GuardarCommand.CanExecute(null));
        Assert.True(avisos > 0);
    }

    [Fact]
    public void Guardar_LibroNuevo_LoAnadeYLoElige()
    {
        var vm = CrearViewModel();
        vm.NuevoCommand.Execute(null);
        vm.Ficha!.Titulo = "La sombra del viento";
        vm.Ficha.Autor = "Carlos Ruiz Zafón";
        vm.Ficha.AnioTexto = "2001";

        vm.GuardarCommand.Execute(null);

        Assert.Equal(4, vm.LibrosVisibles.Count);
        Assert.Equal("La sombra del viento", vm.Seleccionado?.Titulo);
        Assert.Equal(3, vm.Disponibles);
    }

    [Fact]
    public void Guardar_LibroEditado_GuardaLosCambios()
    {
        var vm = CrearViewModel();
        Elegir(vm, "Platero y yo");

        vm.Ficha!.AnioTexto = "1917";
        vm.GuardarCommand.Execute(null);

        Assert.Equal(1917, vm.Seleccionado?.Anio);
    }

    [Fact]
    public void Eliminar_NoSePuedeConUnLibroPrestado()
    {
        var vm = CrearViewModel();

        Elegir(vm, "Don Quijote de la Mancha");

        Assert.False(vm.EliminarCommand.CanExecute(null));
    }

    [Fact]
    public void Eliminar_ConfirmadoLoQuita()
    {
        var vm = CrearViewModel();
        Elegir(vm, "Mafalda 1");

        vm.EliminarCommand.Execute(null);

        Assert.Single(_dialogos.Preguntas);
        Assert.DoesNotContain(vm.LibrosVisibles, l => l.Titulo == "Mafalda 1");
        Assert.Null(vm.Seleccionado);
    }

    [Fact]
    public void Eliminar_SinConfirmarNoHaceNada()
    {
        var vm = CrearViewModel();
        Elegir(vm, "Mafalda 1");
        _dialogos.Respuesta = false;

        vm.EliminarCommand.Execute(null);

        Assert.Equal(3, vm.LibrosVisibles.Count);
    }

    [Fact]
    public void Prestar_NecesitaUnLectorYUnLibroDisponible()
    {
        var vm = CrearViewModel();
        Elegir(vm, "Platero y yo");
        Assert.False(vm.PrestarCommand.CanExecute(null));

        vm.NombreLector = "Mario Gil";
        Assert.True(vm.PrestarCommand.CanExecute(null));

        Elegir(vm, "Don Quijote de la Mancha");
        Assert.False(vm.PrestarCommand.CanExecute(null));
    }

    [Fact]
    public void Prestar_RegistraElPrestamoYActualizaElResumen()
    {
        var vm = CrearViewModel();
        Elegir(vm, "Platero y yo");
        vm.NombreLector = " Mario Gil ";

        vm.PrestarCommand.Execute(null);

        Assert.Equal("Mario Gil", vm.Seleccionado?.Lector);
        Assert.Equal("", vm.NombreLector);
        Assert.Equal(1, vm.Disponibles);
        Assert.Equal(2, vm.Prestados);
        Assert.Contains("Mario Gil", vm.Estado);
        Assert.True(vm.DevolverCommand.CanExecute(null));
    }

    [Fact]
    public void Devolver_DejaElLibroDisponible()
    {
        var vm = CrearViewModel();
        Elegir(vm, "Don Quijote de la Mancha");

        vm.DevolverCommand.Execute(null);

        Assert.False(vm.Seleccionado?.Prestado);
        Assert.Equal(3, vm.Disponibles);
        Assert.False(vm.DevolverCommand.CanExecute(null));
    }
}
