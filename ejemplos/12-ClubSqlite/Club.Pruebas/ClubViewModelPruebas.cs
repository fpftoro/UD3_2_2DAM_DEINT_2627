using Club.Nucleo.Modelos;
using Club.Nucleo.ViewModels;

namespace Club.Pruebas;

/// <summary>
/// Pruebas del ViewModel con un repositorio falso: sin ventana y sin base de datos.
/// </summary>
public class ClubViewModelPruebas
{
    private readonly RepositorioFalso _repositorio = new();
    private readonly DialogosFalsos _dialogos = new();

    public ClubViewModelPruebas()
    {
        _repositorio.Socios.Add(new Socio { Id = 1, Nombre = "Lucía Pérez", Categoria = "Adulto", Antiguedad = 12 });
        _repositorio.Socios.Add(new Socio { Id = 2, Nombre = "Mario Gil", Categoria = "Infantil", Antiguedad = 2 });
        _repositorio.Actividades.Add(new Actividad { Id = 1, Nombre = "Pádel", Plazas = 8 });
        _repositorio.Actividades.Add(new Actividad { Id = 2, Nombre = "Yoga", Plazas = 2 });
    }

    private ClubViewModel CrearViewModel() => new(_repositorio, _dialogos);

    [Fact]
    public void ElegirSocio_CargaSusActividadesYLasDisponibles()
    {
        _repositorio.Inscribir(1, 2);
        var vm = CrearViewModel();

        vm.SocioSeleccionado = vm.Socios.Single(s => s.Id == 1);

        Assert.Equal("Yoga", Assert.Single(vm.ActividadesDelSocio).Nombre);
        Assert.Equal("Pádel", Assert.Single(vm.ActividadesDisponibles).Nombre);
    }

    [Fact]
    public void Inscribir_SoloSePuedeConSocioYActividadElegidos()
    {
        var vm = CrearViewModel();
        vm.SocioSeleccionado = vm.Socios[0];
        Assert.False(vm.InscribirCommand.CanExecute(null));

        vm.ActividadParaInscribir = vm.ActividadesDisponibles[0];

        Assert.True(vm.InscribirCommand.CanExecute(null));
    }

    [Fact]
    public void Inscribir_PasaLaActividadAlDetalle()
    {
        var vm = CrearViewModel();
        vm.SocioSeleccionado = vm.Socios[0];
        vm.ActividadParaInscribir = vm.ActividadesDisponibles.Single(a => a.Nombre == "Pádel");

        vm.InscribirCommand.Execute(null);

        Assert.Contains(vm.ActividadesDelSocio, a => a.Nombre == "Pádel");
        Assert.DoesNotContain(vm.ActividadesDisponibles, a => a.Nombre == "Pádel");
    }

    [Fact]
    public void Inscribir_SiElRepositorioFalla_AvisaAlUsuario()
    {
        _repositorio.FallarAlInscribir = true;
        var vm = CrearViewModel();
        vm.SocioSeleccionado = vm.Socios[0];
        vm.ActividadParaInscribir = vm.ActividadesDisponibles[0];

        vm.InscribirCommand.Execute(null);

        Assert.Contains("completa", Assert.Single(_dialogos.Preguntas));
        Assert.Empty(vm.ActividadesDelSocio);
    }

    [Fact]
    public void Guardar_Alta_LaAnadeAlRepositorioYLaElige()
    {
        var vm = CrearViewModel();
        vm.NuevoCommand.Execute(null);
        vm.Ficha!.Nombre = "Ana Torres";

        vm.GuardarCommand.Execute(null);

        Assert.Equal(3, _repositorio.Socios.Count);
        Assert.Equal("Ana Torres", vm.SocioSeleccionado?.Nombre);
    }

    [Fact]
    public void Buscar_FiltraLaLista()
    {
        var vm = CrearViewModel();

        vm.Buscar = "mario";

        Assert.Equal("Mario Gil", Assert.Single(vm.Socios).Nombre);
    }
}
