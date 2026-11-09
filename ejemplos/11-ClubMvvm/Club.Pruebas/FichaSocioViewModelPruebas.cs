using Club.Nucleo.Modelos;
using Club.Nucleo.ViewModels;

namespace Club.Pruebas;

public class FichaSocioViewModelPruebas
{
    [Fact]
    public void FichaNueva_SinNombre_TieneError()
    {
        var ficha = new FichaSocioViewModel(null);

        Assert.True(ficha.HasErrors);
        Assert.Single(ficha.GetErrors(nameof(FichaSocioViewModel.Nombre)));
    }

    [Fact]
    public void EscribirNombre_QuitaElErrorYAvisa()
    {
        var ficha = new FichaSocioViewModel(null);
        var avisos = new List<string?>();
        ficha.ErrorsChanged += (_, e) => avisos.Add(e.PropertyName);

        ficha.Nombre = "Ana Torres";

        Assert.False(ficha.HasErrors);
        Assert.Contains(nameof(FichaSocioViewModel.Nombre), avisos);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("-1")]
    [InlineData("101")]
    [InlineData("")]
    public void AntiguedadNoValida_TieneErrorYNoHayCuota(string texto)
    {
        var ficha = new FichaSocioViewModel(null) { Nombre = "Ana Torres", AntiguedadTexto = texto };

        Assert.True(ficha.HasErrors);
        Assert.Null(ficha.Cuota);
    }

    [Fact]
    public void CambiarCategoria_AvisaDeLaCuota()
    {
        var ficha = new FichaSocioViewModel(null) { Nombre = "Ana Torres" };
        var cambios = new List<string?>();
        ficha.PropertyChanged += (_, e) => cambios.Add(e.PropertyName);

        ficha.Categoria = "Infantil";

        Assert.Contains(nameof(FichaSocioViewModel.Cuota), cambios);
        Assert.Equal(15m, ficha.Cuota);
    }

    [Fact]
    public void EditarSocio_NoCambiaElOriginalHastaGuardar()
    {
        var original = new Socio { Id = 7, Nombre = "Lucía Pérez", Categoria = "Adulto", Antiguedad = 12 };
        var ficha = new FichaSocioViewModel(original) { Nombre = "Lucía Pérez Gil" };

        var socio = ficha.CrearSocio();

        Assert.Equal("Lucía Pérez", original.Nombre);
        Assert.Equal(7, socio.Id);
        Assert.Equal("Lucía Pérez Gil", socio.Nombre);
    }
}
