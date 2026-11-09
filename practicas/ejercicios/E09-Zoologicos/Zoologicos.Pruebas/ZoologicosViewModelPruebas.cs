using Zoologicos.Nucleo.ViewModels;

namespace Zoologicos.Pruebas;

/// <summary>
/// Pruebas del ViewModel con un repositorio falso: no hace falta ni la ventana ni la base de datos.
/// </summary>
public class ZoologicosViewModelPruebas
{
    private readonly DialogosFalsos _dialogos = new();

    private ZoologicosViewModel CrearViewModel() => new(new RepositorioFalso(), _dialogos);

    private static void ElegirZoo(ZoologicosViewModel vm, string ciudad) =>
        vm.ZooSeleccionado = vm.Zoos.Single(z => z.Ciudad == ciudad);

    private static void ElegirAnimal(ZoologicosViewModel vm, string especie) =>
        vm.AnimalSeleccionado = vm.Animales.Single(a => a.Especie == especie);

    [Fact]
    public void AlCrearse_CargaZoosYAnimalesSinElegirNada()
    {
        var vm = CrearViewModel();

        Assert.Equal(["Madrid", "Sevilla"], vm.Zoos.Select(z => z.Ciudad));
        Assert.Equal(["Elefante", "Jirafa", "Koala"], vm.Animales.Select(a => a.Especie));
        Assert.Null(vm.ZooSeleccionado);
        Assert.Empty(vm.AnimalesDelZoo);
    }

    [Fact]
    public void ElegirUnZoo_CargaSusAnimalesYSuCiudad()
    {
        var vm = CrearViewModel();

        ElegirZoo(vm, "Madrid");

        Assert.Equal(["Elefante", "Jirafa"], vm.AnimalesDelZoo.Select(a => a.Especie));
        Assert.Equal("Madrid", vm.TextoZoo);
    }

    [Fact]
    public void AnadirZoo_SoloSePuedeConTexto()
    {
        var vm = CrearViewModel();
        Assert.False(vm.AnadirZooCommand.CanExecute(null));

        vm.TextoZoo = "   ";
        Assert.False(vm.AnadirZooCommand.CanExecute(null));

        vm.TextoZoo = "Valencia";
        Assert.True(vm.AnadirZooCommand.CanExecute(null));
    }

    [Fact]
    public void AnadirZoo_LoAnadeYLoElige()
    {
        var vm = CrearViewModel();
        vm.TextoZoo = " Valencia ";

        vm.AnadirZooCommand.Execute(null);

        Assert.Equal(3, vm.Zoos.Count);
        Assert.Equal("Valencia", vm.ZooSeleccionado?.Ciudad);
        Assert.Contains("Valencia", vm.Estado);
    }

    [Fact]
    public void AnadirZoo_Repetido_MuestraElErrorSinRomperNada()
    {
        var vm = CrearViewModel();
        vm.TextoZoo = "madrid";

        vm.AnadirZooCommand.Execute(null);

        Assert.Single(_dialogos.Errores);
        Assert.Equal(2, vm.Zoos.Count);
    }

    [Fact]
    public void ActualizarZoo_SoloSiElTextoCambiaYLoGuarda()
    {
        var vm = CrearViewModel();
        ElegirZoo(vm, "Sevilla");
        Assert.False(vm.ActualizarZooCommand.CanExecute(null));

        vm.TextoZoo = "Sevilla (Isla Mágica)";
        vm.ActualizarZooCommand.Execute(null);

        Assert.Equal("Sevilla (Isla Mágica)", vm.ZooSeleccionado?.Ciudad);
        Assert.Contains(vm.Zoos, z => z.Ciudad == "Sevilla (Isla Mágica)");
    }

    [Fact]
    public void EliminarZoo_ConfirmadoLoQuitaYVaciaSusAnimales()
    {
        var vm = CrearViewModel();
        ElegirZoo(vm, "Madrid");

        vm.EliminarZooCommand.Execute(null);

        Assert.Single(_dialogos.Preguntas);
        Assert.DoesNotContain(vm.Zoos, z => z.Ciudad == "Madrid");
        Assert.Null(vm.ZooSeleccionado);
        Assert.Empty(vm.AnimalesDelZoo);
        Assert.Equal(3, vm.Animales.Count);
    }

    [Fact]
    public void EliminarZoo_SinConfirmarNoHaceNada()
    {
        var vm = CrearViewModel();
        ElegirZoo(vm, "Madrid");
        _dialogos.Respuesta = false;

        vm.EliminarZooCommand.Execute(null);

        Assert.Equal(2, vm.Zoos.Count);
    }

    [Fact]
    public void AnadirAlZoo_NoSePuedeSiElZooYaTieneEseAnimal()
    {
        var vm = CrearViewModel();
        ElegirZoo(vm, "Madrid");

        ElegirAnimal(vm, "Elefante");
        Assert.False(vm.AnadirAlZooCommand.CanExecute(null));

        ElegirAnimal(vm, "Koala");
        Assert.True(vm.AnadirAlZooCommand.CanExecute(null));
    }

    [Fact]
    public void AnadirAlZoo_AnadeElAnimalElegidoAlZooElegido()
    {
        var vm = CrearViewModel();
        ElegirZoo(vm, "Sevilla");
        ElegirAnimal(vm, "Koala");

        vm.AnadirAlZooCommand.Execute(null);

        Assert.Equal(["Koala"], vm.AnimalesDelZoo.Select(a => a.Especie));
        Assert.False(vm.AnadirAlZooCommand.CanExecute(null));
    }

    [Fact]
    public void QuitarDelZoo_QuitaElAnimalElegidoEnLaListaDelZoo()
    {
        var vm = CrearViewModel();
        ElegirZoo(vm, "Madrid");
        Assert.False(vm.QuitarDelZooCommand.CanExecute(null));
        vm.AnimalDelZooSeleccionado = vm.AnimalesDelZoo.Single(a => a.Especie == "Jirafa");

        vm.QuitarDelZooCommand.Execute(null);

        Assert.Equal(["Elefante"], vm.AnimalesDelZoo.Select(a => a.Especie));
        Assert.Equal(3, vm.Animales.Count);
    }

    [Fact]
    public void ActualizarAnimal_CambiaElNombreEnLasDosListas()
    {
        var vm = CrearViewModel();
        ElegirZoo(vm, "Madrid");
        ElegirAnimal(vm, "Elefante");

        vm.TextoAnimal = "Elefante africano";
        vm.ActualizarAnimalCommand.Execute(null);

        Assert.Contains(vm.Animales, a => a.Especie == "Elefante africano");
        Assert.Contains(vm.AnimalesDelZoo, a => a.Especie == "Elefante africano");
        Assert.Equal("Elefante africano", vm.AnimalSeleccionado?.Especie);
    }

    [Fact]
    public void EliminarAnimal_LoQuitaDeLaListaYDelZoo()
    {
        var vm = CrearViewModel();
        ElegirZoo(vm, "Madrid");
        ElegirAnimal(vm, "Jirafa");

        vm.EliminarAnimalCommand.Execute(null);

        Assert.DoesNotContain(vm.Animales, a => a.Especie == "Jirafa");
        Assert.DoesNotContain(vm.AnimalesDelZoo, a => a.Especie == "Jirafa");
        Assert.Null(vm.AnimalSeleccionado);
    }
}
