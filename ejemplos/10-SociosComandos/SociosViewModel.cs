using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Windows.Data;
using SociosComandos.Comandos;

namespace SociosComandos;

/// <summary>
/// Datos y acciones de la ventana de socios. Las acciones son comandos: la ventana ya no tiene manejadores.
/// </summary>
public class SociosViewModel : ObjetoObservable
{
    private string _buscar = "";
    private Socio? _socioSeleccionado;
    private int _progreso;
    private string _estado = "Listo";
    private CancellationTokenSource? _cancelacion;

    /// <summary>
    /// Crea el ViewModel con tres socios de ejemplo y sus comandos.
    /// </summary>
    public SociosViewModel()
    {
        Socios =
        [
            new Socio { Nombre = "Lucía Pérez", Categoria = "Adulto", Antiguedad = 12 },
            new Socio { Nombre = "Mario Gil", Categoria = "Infantil", Antiguedad = 2 },
            new Socio { Nombre = "Carmen Ruiz", Categoria = "Senior", Antiguedad = 25 }
        ];
        foreach (var socio in Socios)
        {
            socio.PropertyChanged += Socio_PropertyChanged;
        }

        Socios.CollectionChanged += Socios_CollectionChanged;

        VistaSocios = CollectionViewSource.GetDefaultView(Socios);
        VistaSocios.Filter = CumpleFiltro;
        VistaSocios.SortDescriptions.Add(new SortDescription(nameof(Socio.Nombre), ListSortDirection.Ascending));
        if (VistaSocios is ICollectionViewLiveShaping vista)
        {
            vista.IsLiveSorting = true;
            vista.IsLiveFiltering = true;
            vista.LiveSortingProperties.Add(nameof(Socio.Nombre));
            vista.LiveFilteringProperties.Add(nameof(Socio.Nombre));
        }

        NuevoCommand = new RelayCommand(Nuevo);
        EliminarCommand = new RelayCommand(Eliminar, () => HaySeleccion);
        ExportarCommand = new ComandoAsincrono(ExportarAsync, () => Socios.Count > 0);
        CancelarExportacionCommand = new RelayCommand(() => _cancelacion?.Cancel(), () => _cancelacion is not null);
    }

    /// <summary>Todos los socios del club.</summary>
    public ObservableCollection<Socio> Socios { get; }

    /// <summary>Los socios que se ven en la lista, filtrados y ordenados.</summary>
    public ICollectionView VistaSocios { get; }

    /// <summary>Texto del buscador.</summary>
    public string Buscar
    {
        get => _buscar;
        set
        {
            if (Asignar(ref _buscar, value))
            {
                VistaSocios.Refresh();
            }
        }
    }

    /// <summary>Socio elegido en la lista.</summary>
    public Socio? SocioSeleccionado
    {
        get => _socioSeleccionado;
        set
        {
            if (Asignar(ref _socioSeleccionado, value))
            {
                OnPropertyChanged(nameof(HaySeleccion));
                EliminarCommand.NotificarCambio();
            }
        }
    }

    /// <summary>Si hay un socio elegido.</summary>
    public bool HaySeleccion => SocioSeleccionado is not null;

    /// <summary>Número de socios.</summary>
    public int NumeroSocios => Socios.Count;

    /// <summary>Suma de las cuotas mensuales.</summary>
    public decimal TotalCuotas => Socios.Sum(s => s.Cuota);

    /// <summary>Progreso de la exportación, de 0 a 100.</summary>
    public int Progreso
    {
        get => _progreso;
        private set => Asignar(ref _progreso, value);
    }

    /// <summary>Mensaje de la barra de estado.</summary>
    public string Estado
    {
        get => _estado;
        private set => Asignar(ref _estado, value);
    }

    /// <summary>
    /// Pregunta al usuario y devuelve su respuesta. La asigna la ventana: el ViewModel
    /// no muestra mensajes (en el tema 11 lo hará un servicio).
    /// </summary>
    public Func<string, bool> Confirmar { get; set; } = _ => true;

    /// <summary>Añade un socio y lo elige.</summary>
    public RelayCommand NuevoCommand { get; }

    /// <summary>Elimina el socio elegido, tras confirmarlo.</summary>
    public RelayCommand EliminarCommand { get; }

    /// <summary>Exporta los socios a un CSV en Documentos, sin bloquear la ventana.</summary>
    public ComandoAsincrono ExportarCommand { get; }

    /// <summary>Cancela la exportación en curso.</summary>
    public RelayCommand CancelarExportacionCommand { get; }

    private void Nuevo()
    {
        Buscar = "";
        var socio = new Socio { Nombre = "Nuevo socio" };
        Socios.Add(socio);
        SocioSeleccionado = socio;
    }

    private void Eliminar()
    {
        if (SocioSeleccionado is null || !Confirmar($"¿Seguro que quieres eliminar a {SocioSeleccionado.Nombre}?"))
        {
            return;
        }

        Socios.Remove(SocioSeleccionado);
        SocioSeleccionado = null;
    }

    private async Task ExportarAsync()
    {
        var ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "socios.csv");
        var lineas = Socios.Select(s => $"{s.Nombre};{s.Categoria};{s.Antiguedad};{s.Cuota:0.00}").ToList();

        _cancelacion = new CancellationTokenSource();
        CancelarExportacionCommand.NotificarCambio();
        Estado = "Exportando…";
        try
        {
            // Simula una exportación lenta: medio segundo por socio
            for (var i = 0; i < lineas.Count; i++)
            {
                await Task.Delay(500, _cancelacion.Token);
                Progreso = (i + 1) * 100 / lineas.Count;
            }

            await File.WriteAllLinesAsync(ruta, lineas, _cancelacion.Token);
            Estado = $"Exportados {lineas.Count} socios a {ruta}";
        }
        catch (OperationCanceledException)
        {
            Estado = "Exportación cancelada";
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            Estado = $"No se ha podido exportar: {ex.Message}";
        }
        finally
        {
            Progreso = 0;
            _cancelacion.Dispose();
            _cancelacion = null;
            CancelarExportacionCommand.NotificarCambio();
        }
    }

    private bool CumpleFiltro(object elemento) =>
        elemento is Socio socio && socio.Nombre.Contains(Buscar.Trim(), StringComparison.CurrentCultureIgnoreCase);

    private void Socios_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        foreach (Socio socio in e.NewItems ?? Array.Empty<Socio>())
        {
            socio.PropertyChanged += Socio_PropertyChanged;
        }

        foreach (Socio socio in e.OldItems ?? Array.Empty<Socio>())
        {
            socio.PropertyChanged -= Socio_PropertyChanged;
        }

        OnPropertyChanged(nameof(NumeroSocios));
        OnPropertyChanged(nameof(TotalCuotas));
        ExportarCommand.NotificarCambio();
    }

    private void Socio_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Socio.Cuota))
        {
            OnPropertyChanged(nameof(TotalCuotas));
        }
    }
}
