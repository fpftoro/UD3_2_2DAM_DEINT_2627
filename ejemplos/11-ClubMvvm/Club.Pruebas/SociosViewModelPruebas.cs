using Club.Nucleo.Modelos;
using Club.Nucleo.Servicios;
using Club.Nucleo.ViewModels;

namespace Club.Pruebas;

public class SociosViewModelPruebas
{
    private readonly DialogosFalsos _dialogos = new();

    private SociosViewModel CrearViewModel() =>
        new(new ServicioSociosMemoria(
            [
                new Socio { Nombre = "Lucía Pérez", Categoria = "Adulto", Antiguedad = 12 },
                new Socio { Nombre = "Mario Gil", Categoria = "Infantil", Antiguedad = 2 }
            ]),
            _dialogos);

    [Fact]
    public void AlCrearse_CargaLosSociosYLosTotales()
    {
        var vm = CrearViewModel();

        Assert.Equal(2, vm.SociosVisibles.Count);
        Assert.Equal(2, vm.NumeroSocios);
        Assert.Equal(46.5m, vm.TotalCuotas); // 35 · 0,9 + 15
    }

    [Fact]
    public void Buscar_FiltraPorNombreSinDistinguirMayusculas()
    {
        var vm = CrearViewModel();

        vm.Buscar = "mar";

        Assert.Equal("Mario Gil", Assert.Single(vm.SociosVisibles).Nombre);
    }

    [Fact]
    public void Guardar_NoSePuedeConLaFichaVacia()
    {
        var vm = CrearViewModel();

        vm.NuevoCommand.Execute(null);

        Assert.False(vm.GuardarCommand.CanExecute(null));
    }

    [Fact]
    public void Guardar_SocioNuevo_LoAnadeYLoElige()
    {
        var vm = CrearViewModel();
        vm.NuevoCommand.Execute(null);
        vm.Ficha!.Nombre = "Ana Torres";

        vm.GuardarCommand.Execute(null);

        Assert.Equal(3, vm.NumeroSocios);
        Assert.Equal("Ana Torres", vm.SocioSeleccionado?.Nombre);
    }

    [Fact]
    public void Guardar_NombreRepetido_MarcaErrorYNoGuarda()
    {
        var vm = CrearViewModel();
        vm.NuevoCommand.Execute(null);
        vm.Ficha!.Nombre = "lucía pérez";

        vm.GuardarCommand.Execute(null);

        Assert.Equal(2, vm.NumeroSocios);
        Assert.True(vm.Ficha.HasErrors);
    }

    [Fact]
    public void ElegirSocio_HabilitaEliminar()
    {
        var vm = CrearViewModel();
        Assert.False(vm.EliminarCommand.CanExecute(null));

        vm.SocioSeleccionado = vm.SociosVisibles[0];

        Assert.True(vm.EliminarCommand.CanExecute(null));
    }

    [Theory]
    [InlineData(true, 1)]
    [InlineData(false, 2)]
    public void Eliminar_PreguntaYSoloBorraSiSeConfirma(bool respuesta, int quedan)
    {
        var vm = CrearViewModel();
        _dialogos.Respuesta = respuesta;
        vm.SocioSeleccionado = vm.SociosVisibles[0];

        vm.EliminarCommand.Execute(null);

        Assert.Single(_dialogos.Preguntas);
        Assert.Equal(quedan, vm.NumeroSocios);
    }
}
