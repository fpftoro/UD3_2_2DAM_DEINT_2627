using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Data;

namespace SociosBinding;

/// <summary>
/// Datos que muestra la ventana de socios. Se llama ViewModel porque lo es:
/// el nombre se explica en el tema 11.
/// </summary>
public class SociosViewModel : ObjetoObservable
{
    private string _buscar = "";
    private Socio? _socioSeleccionado;

    /// <summary>
    /// Crea el ViewModel con tres socios de ejemplo.
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

        // Vista de la colección: filtra y ordena sin tocar la colección original
        VistaSocios = CollectionViewSource.GetDefaultView(Socios);
        VistaSocios.Filter = CumpleFiltro;
        VistaSocios.SortDescriptions.Add(new SortDescription(nameof(Socio.Nombre), ListSortDirection.Ascending));
        if (VistaSocios is ICollectionViewLiveShaping vista)
        {
            // Se reordena y se refiltra cuando cambia el nombre de un socio
            vista.IsLiveSorting = true;
            vista.IsLiveFiltering = true;
            vista.LiveSortingProperties.Add(nameof(Socio.Nombre));
            vista.LiveFilteringProperties.Add(nameof(Socio.Nombre));
        }
    }

    /// <summary>Todos los socios del club.</summary>
    public ObservableCollection<Socio> Socios { get; }

    /// <summary>Los socios que se ven en la lista: filtrados por <see cref="Buscar"/> y por nombre.</summary>
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

    /// <summary>Socio elegido en la lista; el formulario lo edita directamente.</summary>
    public Socio? SocioSeleccionado
    {
        get => _socioSeleccionado;
        set
        {
            if (Asignar(ref _socioSeleccionado, value))
            {
                OnPropertyChanged(nameof(HaySeleccion));
            }
        }
    }

    /// <summary>Si hay un socio elegido (habilita el formulario y el botón Eliminar).</summary>
    public bool HaySeleccion => SocioSeleccionado is not null;

    /// <summary>Número de socios.</summary>
    public int NumeroSocios => Socios.Count;

    /// <summary>Suma de las cuotas mensuales de todos los socios.</summary>
    public decimal TotalCuotas => Socios.Sum(s => s.Cuota);

    /// <summary>
    /// Añade un socio nuevo con valores por defecto y lo elige para editarlo.
    /// </summary>
    public void Nuevo()
    {
        Buscar = "";
        var socio = new Socio { Nombre = "Nuevo socio" };
        Socios.Add(socio);
        SocioSeleccionado = socio;
    }

    /// <summary>
    /// Elimina el socio elegido.
    /// </summary>
    public void Eliminar()
    {
        if (SocioSeleccionado is not null)
        {
            Socios.Remove(SocioSeleccionado);
            SocioSeleccionado = null;
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
    }

    private void Socio_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Socio.Cuota))
        {
            OnPropertyChanged(nameof(TotalCuotas));
        }
    }
}
