using System.Collections.ObjectModel;
using Club.Nucleo.Modelos;
using Club.Nucleo.Mvvm;
using Club.Nucleo.Servicios;

namespace Club.Nucleo.ViewModels;

/// <summary>
/// Pantalla de socios: lista con buscador y ficha de alta o edición.
/// </summary>
public class SociosViewModel : ObjetoObservable
{
    private const string Titulo = "Socios del club";

    private readonly IServicioSocios _servicio;
    private readonly IServicioDialogos _dialogos;
    private string _buscar = "";
    private Socio? _socioSeleccionado;
    private FichaSocioViewModel? _ficha;

    /// <summary>
    /// Crea el ViewModel. Recibe sus dependencias en el constructor (inyección de dependencias).
    /// </summary>
    /// <param name="servicio">Dónde se guardan los socios.</param>
    /// <param name="dialogos">Cómo se pregunta al usuario.</param>
    public SociosViewModel(IServicioSocios servicio, IServicioDialogos dialogos)
    {
        _servicio = servicio;
        _dialogos = dialogos;

        NuevoCommand = new RelayCommand(Nuevo);
        GuardarCommand = new RelayCommand(Guardar, () => Ficha is { HasErrors: false });
        EliminarCommand = new RelayCommand(Eliminar, () => SocioSeleccionado is not null);
        CancelarCommand = new RelayCommand(() => SocioSeleccionado = null, () => Ficha is not null);

        Recargar();
    }

    /// <summary>Socios que cumplen el filtro del buscador.</summary>
    public ObservableCollection<Socio> SociosVisibles { get; } = [];

    /// <summary>Texto del buscador: filtra por nombre.</summary>
    public string Buscar
    {
        get => _buscar;
        set
        {
            if (Asignar(ref _buscar, value))
            {
                Recargar();
            }
        }
    }

    /// <summary>Socio elegido en la lista. Al elegirlo se abre su ficha.</summary>
    public Socio? SocioSeleccionado
    {
        get => _socioSeleccionado;
        set
        {
            if (Asignar(ref _socioSeleccionado, value))
            {
                Ficha = value is null ? null : new FichaSocioViewModel(value);
                EliminarCommand.NotificarCambio();
            }
        }
    }

    /// <summary>Ficha que se está editando, o <c>null</c> si no hay ninguna abierta.</summary>
    public FichaSocioViewModel? Ficha
    {
        get => _ficha;
        private set
        {
            if (_ficha is not null)
            {
                _ficha.ErrorsChanged -= Ficha_ErrorsChanged;
            }

            if (Asignar(ref _ficha, value))
            {
                if (_ficha is not null)
                {
                    _ficha.ErrorsChanged += Ficha_ErrorsChanged;
                }

                OnPropertyChanged(nameof(HayFicha));
                GuardarCommand.NotificarCambio();
                CancelarCommand.NotificarCambio();
            }
        }
    }

    /// <summary>Si hay una ficha abierta (la vista la muestra u oculta).</summary>
    public bool HayFicha => Ficha is not null;

    /// <summary>Número de socios del club (sin filtrar).</summary>
    public int NumeroSocios { get; private set; }

    /// <summary>Suma de las cuotas de todos los socios.</summary>
    public decimal TotalCuotas { get; private set; }

    /// <summary>Abre una ficha vacía para dar de alta un socio.</summary>
    public RelayCommand NuevoCommand { get; }

    /// <summary>Guarda la ficha abierta. Solo se puede si no tiene errores.</summary>
    public RelayCommand GuardarCommand { get; }

    /// <summary>Elimina el socio elegido, tras pedir confirmación.</summary>
    public RelayCommand EliminarCommand { get; }

    /// <summary>Cierra la ficha sin guardar.</summary>
    public RelayCommand CancelarCommand { get; }

    private void Nuevo()
    {
        SocioSeleccionado = null;
        Ficha = new FichaSocioViewModel(null);
    }

    private void Guardar()
    {
        if (Ficha is null || Ficha.HasErrors)
        {
            return;
        }

        var socio = Ficha.CrearSocio();
        if (_servicio.ExisteNombre(socio.Nombre, socio.Id))
        {
            Ficha.AnadirError(nameof(FichaSocioViewModel.Nombre), "Ya hay un socio con ese nombre.");
            return;
        }

        _servicio.Guardar(socio);
        Recargar(socio.Id);
    }

    private void Eliminar()
    {
        if (SocioSeleccionado is not { } socio
            || !_dialogos.Confirmar($"¿Seguro que quieres eliminar a {socio.Nombre}?", Titulo))
        {
            return;
        }

        _servicio.Eliminar(socio.Id);
        Recargar();
    }

    /// <summary>Vuelve a leer los socios del servicio y, si se indica, elige uno.</summary>
    private void Recargar(int? idSeleccionar = null)
    {
        var todos = _servicio.ObtenerTodos();
        NumeroSocios = todos.Count;
        TotalCuotas = todos.Sum(s => s.Cuota);
        OnPropertyChanged(nameof(NumeroSocios));
        OnPropertyChanged(nameof(TotalCuotas));

        SociosVisibles.Clear();
        foreach (var socio in todos.Where(s => s.Nombre.Contains(Buscar.Trim(), StringComparison.CurrentCultureIgnoreCase)))
        {
            SociosVisibles.Add(socio);
        }

        SocioSeleccionado = SociosVisibles.FirstOrDefault(s => s.Id == idSeleccionar);
    }

    private void Ficha_ErrorsChanged(object? sender, System.ComponentModel.DataErrorsChangedEventArgs e)
    {
        GuardarCommand.NotificarCambio();
    }
}
