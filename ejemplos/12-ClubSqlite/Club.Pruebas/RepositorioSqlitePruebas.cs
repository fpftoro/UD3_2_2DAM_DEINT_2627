using Club.Datos;
using Club.Nucleo.Datos;
using Club.Nucleo.Modelos;
using Microsoft.Data.Sqlite;

namespace Club.Pruebas;

/// <summary>
/// Pruebas del repositorio contra una base de datos SQLite en memoria: es real, pero
/// no deja ficheros y cada prueba empieza con los mismos datos.
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

        BaseDatosClub.Inicializar(cadena, conDatosDeEjemplo: true);
        _repositorio = new RepositorioSqlite(cadena);
    }

    public void Dispose() => _mantenerViva.Dispose();

    [Fact]
    public void ObtenerSocios_DevuelveLosDeEjemploOrdenados()
    {
        var socios = _repositorio.ObtenerSocios();

        Assert.Equal(5, socios.Count);
        Assert.Equal("Carmen Ruiz", socios[0].Nombre);
    }

    [Fact]
    public void ObtenerSocios_ConFiltro_BuscaDentroDelNombre()
    {
        var socios = _repositorio.ObtenerSocios("art");

        Assert.Equal("Elena Martín", Assert.Single(socios).Nombre);
    }

    [Fact]
    public void ObtenerSocios_UnFiltroConComillas_NoRompeLaConsulta()
    {
        // Con los parámetros, una comilla es un carácter más, no parte del SQL
        var socios = _repositorio.ObtenerSocios("' OR '1'='1");

        Assert.Empty(socios);
    }

    [Fact]
    public void AgregarSocio_DevuelveSuIdYSePuedeLeer()
    {
        var id = _repositorio.AgregarSocio(new Socio { Nombre = "Ana Torres", Categoria = "Senior", Antiguedad = 1 });

        var leido = Assert.Single(_repositorio.ObtenerSocios("Ana Torres"));
        Assert.Equal(id, leido.Id);
        Assert.Equal("Senior", leido.Categoria);
    }

    [Theory]
    [InlineData("Mario Gil")]
    [InlineData("MARIO GIL")] // COLLATE NOCASE: no distingue mayúsculas (solo en letras sin tilde)
    public void AgregarSocio_NombreRepetido_LanzaDatosException(string nombre)
    {
        var repetido = new Socio { Nombre = nombre, Categoria = "Adulto", Antiguedad = 0 };

        Assert.Throws<DatosException>(() => _repositorio.AgregarSocio(repetido));
    }

    [Fact]
    public void ActualizarSocio_GuardaLosCambios()
    {
        var socio = _repositorio.ObtenerSocios("Mario")[0];
        socio.Antiguedad = 3;

        _repositorio.ActualizarSocio(socio);

        Assert.Equal(3, _repositorio.ObtenerSocios("Mario")[0].Antiguedad);
    }

    [Fact]
    public void ObtenerActividades_CuentaLosInscritos()
    {
        var yoga = _repositorio.ObtenerActividades().Single(a => a.Nombre == "Yoga");

        Assert.Equal(2, yoga.Inscritos);
        Assert.Equal(0, yoga.PlazasLibres);
    }

    [Fact]
    public void Inscribir_ActividadCompleta_LanzaDatosExceptionYNoInscribe()
    {
        var mario = _repositorio.ObtenerSocios("Mario")[0];
        var yoga = _repositorio.ObtenerActividades().Single(a => a.Nombre == "Yoga");

        Assert.Throws<DatosException>(() => _repositorio.Inscribir(mario.Id, yoga.Id));
        Assert.DoesNotContain(_repositorio.ObtenerActividadesDeSocio(mario.Id), a => a.Id == yoga.Id);
    }

    [Fact]
    public void InscribirYDarDeBaja_CambianLasActividadesDelSocio()
    {
        var mario = _repositorio.ObtenerSocios("Mario")[0];
        var padel = _repositorio.ObtenerActividades().Single(a => a.Nombre == "Pádel");

        _repositorio.Inscribir(mario.Id, padel.Id);
        Assert.Contains(_repositorio.ObtenerActividadesDeSocio(mario.Id), a => a.Id == padel.Id);

        _repositorio.DarDeBaja(mario.Id, padel.Id);
        Assert.DoesNotContain(_repositorio.ObtenerActividadesDeSocio(mario.Id), a => a.Id == padel.Id);
    }

    [Fact]
    public void EliminarSocio_BorraTambienSusInscripciones()
    {
        var lucia = _repositorio.ObtenerSocios("Lucía")[0];

        _repositorio.EliminarSocio(lucia.Id);

        // Lucía estaba en Yoga: ahora queda un inscrito (ON DELETE CASCADE)
        var yoga = _repositorio.ObtenerActividades().Single(a => a.Nombre == "Yoga");
        Assert.Equal(1, yoga.Inscritos);
    }
}
