using Biblioteca.Nucleo.Modelos;
using Biblioteca.Nucleo.ViewModels;

namespace Biblioteca.Pruebas;

public class FichaLibroViewModelPruebas
{
    private static readonly Libro _platero = new()
    {
        Id = 7,
        Titulo = "Platero y yo",
        Autor = "Juan Ramón Jiménez",
        Anio = 1914,
        Genero = "Poesía",
        Lector = "Mario Gil",
    };

    [Fact]
    public void FichaNueva_TieneErroresEnTituloAutorYAnio()
    {
        var ficha = new FichaLibroViewModel();

        Assert.True(ficha.EsNuevo);
        Assert.True(ficha.HasErrors);
        Assert.NotEmpty(ficha.GetErrors(nameof(FichaLibroViewModel.Titulo)).Cast<string>());
        Assert.NotEmpty(ficha.GetErrors(nameof(FichaLibroViewModel.Autor)).Cast<string>());
        Assert.NotEmpty(ficha.GetErrors(nameof(FichaLibroViewModel.AnioTexto)).Cast<string>());
        Assert.Equal("Novela", ficha.Genero);
    }

    [Fact]
    public void FichaDeUnLibro_CargaSusDatosSinErrores()
    {
        var ficha = new FichaLibroViewModel(_platero);

        Assert.False(ficha.EsNuevo);
        Assert.False(ficha.HasErrors);
        Assert.Equal("Platero y yo", ficha.Titulo);
        Assert.Equal("1914", ficha.AnioTexto);
        Assert.Equal("Poesía", ficha.Genero);
    }

    [Fact]
    public void Titulo_AlEscribirlo_QuitaElErrorYAvisa()
    {
        var ficha = new FichaLibroViewModel();
        var avisos = new List<string?>();
        ficha.ErrorsChanged += (_, e) => avisos.Add(e.PropertyName);

        ficha.Titulo = "Mafalda 1";

        Assert.Empty(ficha.GetErrors(nameof(FichaLibroViewModel.Titulo)).Cast<string>());
        Assert.Contains(nameof(FichaLibroViewModel.Titulo), avisos);
    }

    [Theory]
    [InlineData("")]
    [InlineData("mil novecientos")]
    [InlineData("1449")]
    [InlineData("3000")]
    public void AnioTexto_NoValido_DaError(string anio)
    {
        var ficha = new FichaLibroViewModel(_platero) { AnioTexto = anio };

        Assert.True(ficha.HasErrors);
        Assert.NotEmpty(ficha.GetErrors(nameof(FichaLibroViewModel.AnioTexto)).Cast<string>());
    }

    [Theory]
    [InlineData("1450")]
    [InlineData("1966")]
    public void AnioTexto_Valido_NoDaError(string anio)
    {
        var ficha = new FichaLibroViewModel(_platero) { AnioTexto = anio };

        Assert.False(ficha.HasErrors);
    }

    [Fact]
    public void CrearLibro_FichaValida_DevuelveElLibroSinEspaciosYConSuLector()
    {
        var ficha = new FichaLibroViewModel(_platero) { Titulo = "  Platero y yo (edición ilustrada) " };

        var libro = ficha.CrearLibro();

        Assert.Equal(7, libro.Id);
        Assert.Equal("Platero y yo (edición ilustrada)", libro.Titulo);
        Assert.Equal(1914, libro.Anio);
        Assert.Equal("Mario Gil", libro.Lector);
    }

    [Fact]
    public void CrearLibro_FichaConErrores_LanzaInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => new FichaLibroViewModel().CrearLibro());
    }
}
