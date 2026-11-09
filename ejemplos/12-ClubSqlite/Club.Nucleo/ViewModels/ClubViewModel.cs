using System.Collections.ObjectModel;
using Club.Nucleo.Datos;
using Club.Nucleo.Modelos;
using Club.Nucleo.Mvvm;
using Club.Nucleo.Servicios;

namespace Club.Nucleo.ViewModels;

/// <summary>
/// Pantalla principal: socios (maestro) con su ficha y sus actividades (detalle),
/// y la lista de actividades con sus inscritos.
/// </summary>
public class ClubViewModel : ObjetoObservable
{
    private const string Titulo = "Club Deportivo";

    private readonly IRepositorioClub _repositorio;
    private readonly IServicioDialogos _dialogos;
    private string _buscar = "";
    private Socio? _socioSeleccionado;
    private FichaSocioViewModel? _ficha;
    private Actividad? _actividadParaInscribir;
    private Actividad? _actividadDelSocio;

    /// <summary>
    /// Crea el ViewModel y carga los datos.
    /// </summary>
    /// <param name="repositorio">Acceso a los datos.</param>
    /// <param name="dialogos">Mensajes al usuario.</param>
    public ClubViewModel(IRepositorioClub repositorio, IServicioDialogos dialogos)
    {
        _repositorio = repositorio;
        _dialogos = dialogos;

        NuevoCommand = new RelayCommand(Nuevo);
        GuardarCommand = new RelayCommand(Guardar, () => Ficha is { HasErrors: false });
        EliminarCommand = new RelayCommand(Eliminar, () => SocioSeleccionado is not null);
        CancelarCommand = new RelayCommand(() => SocioSeleccionado = null, () => Ficha is not null);
        InscribirCommand = new RelayCommand(Inscribir, () => SocioSeleccionado is not null && ActividadParaInscribir is not null);
        DarDeBajaCommand = new RelayCommand(DarDeBaja, () => SocioSeleccionado is not null && ActividadDelSocio is not null);

        Ejecutar(() => Recargar());
    }

    // --- Maestro: socios ---

    /// <summary>Socios que cumplen el filtro.</summary>
    public ObservableCollection<Socio> Socios { get; } = [];

    /// <summary>Texto del buscador. El filtro lo aplica la consulta SQL.</summary>
    public string Buscar
    {
        get => _buscar;
        set
        {
            if (Asignar(ref _buscar, value))
            {
                Ejecutar(() => Recargar());
            }
        }
    }

    /// <summary>Socio elegido: abre su ficha y carga sus actividades.</summary>
    public Socio? SocioSeleccionado
    {
        get => _socioSeleccionado;
        set
        {
            if (!Asignar(ref _socioSeleccionado, value))
            {
                return;
            }

            Ficha = value is null ? null : new FichaSocioViewModel(value);
            Ejecutar(CargarDetalle);
            EliminarCommand.NotificarCambio();
            InscribirCommand.NotificarCambio();
            DarDeBajaCommand.NotificarCambio();
        }
    }

    /// <summary>Ficha abierta, o <c>null</c>.</summary>
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

    /// <summary>Si hay una ficha abierta.</summary>
    public bool HayFicha => Ficha is not null;

    // --- Detalle: actividades del socio ---

    /// <summary>Actividades en las que está inscrito el socio elegido.</summary>
    public ObservableCollection<Actividad> ActividadesDelSocio { get; } = [];

    /// <summary>Actividades en las que el socio elegido aún no está inscrito.</summary>
    public ObservableCollection<Actividad> ActividadesDisponibles { get; } = [];

    /// <summary>Actividad elegida en el desplegable para inscribir al socio.</summary>
    public Actividad? ActividadParaInscribir
    {
        get => _actividadParaInscribir;
        set
        {
            if (Asignar(ref _actividadParaInscribir, value))
            {
                InscribirCommand.NotificarCambio();
            }
        }
    }

    /// <summary>Actividad elegida en la lista del socio, para darlo de baja.</summary>
    public Actividad? ActividadDelSocio
    {
        get => _actividadDelSocio;
        set
        {
            if (Asignar(ref _actividadDelSocio, value))
            {
                DarDeBajaCommand.NotificarCambio();
            }
        }
    }

    /// <summary>Todas las actividades con sus inscritos (pestaña «Actividades»).</summary>
    public ObservableCollection<Actividad> Actividades { get; } = [];

    // --- Comandos ---

    /// <summary>Abre una ficha vacía para un alta.</summary>
    public RelayCommand NuevoCommand { get; }

    /// <summary>Guarda la ficha: alta (INSERT) o modificación (UPDATE).</summary>
    public RelayCommand GuardarCommand { get; }

    /// <summary>Elimina el socio elegido (DELETE), tras confirmarlo.</summary>
    public RelayCommand EliminarCommand { get; }

    /// <summary>Cierra la ficha sin guardar.</summary>
    public RelayCommand CancelarCommand { get; }

    /// <summary>Inscribe al socio elegido en la actividad elegida.</summary>
    public RelayCommand InscribirCommand { get; }

    /// <summary>Da de baja al socio elegido de la actividad elegida.</summary>
    public RelayCommand DarDeBajaCommand { get; }

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

        var ficha = Ficha;
        Ejecutar(() =>
        {
            var socio = ficha.CrearSocio();
            if (_repositorio.ExisteNombre(socio.Nombre, socio.Id))
            {
                ficha.AnadirError(nameof(FichaSocioViewModel.Nombre), "Ya hay un socio con ese nombre.");
                return;
            }

            if (ficha.EsNuevo)
            {
                socio.Id = _repositorio.AgregarSocio(socio);
            }
            else
            {
                _repositorio.ActualizarSocio(socio);
            }

            Recargar(socio.Id);
        });
    }

    private void Eliminar()
    {
        if (SocioSeleccionado is not { } socio
            || !_dialogos.Confirmar($"¿Seguro que quieres eliminar a {socio.Nombre} y sus inscripciones?", Titulo))
        {
            return;
        }

        Ejecutar(() =>
        {
            _repositorio.EliminarSocio(socio.Id);
            Recargar();
        });
    }

    private void Inscribir()
    {
        if (SocioSeleccionado is not { } socio || ActividadParaInscribir is not { } actividad)
        {
            return;
        }

        Ejecutar(() =>
        {
            _repositorio.Inscribir(socio.Id, actividad.Id);
            CargarDetalle();
        });
    }

    private void DarDeBaja()
    {
        if (SocioSeleccionado is not { } socio || ActividadDelSocio is not { } actividad)
        {
            return;
        }

        Ejecutar(() =>
        {
            _repositorio.DarDeBaja(socio.Id, actividad.Id);
            CargarDetalle();
        });
    }

    /// <summary>Vuelve a leer los socios y las actividades; si se indica, elige un socio.</summary>
    private void Recargar(int? idSeleccionar = null)
    {
        Rellenar(Socios, _repositorio.ObtenerSocios(Buscar.Trim()));
        Rellenar(Actividades, _repositorio.ObtenerActividades());
        SocioSeleccionado = Socios.FirstOrDefault(s => s.Id == idSeleccionar);
    }

    /// <summary>Lee las actividades del socio elegido y las que le quedan por elegir.</summary>
    private void CargarDetalle()
    {
        var todas = _repositorio.ObtenerActividades();
        Rellenar(Actividades, todas);

        if (SocioSeleccionado is null)
        {
            ActividadesDelSocio.Clear();
            ActividadesDisponibles.Clear();
            return;
        }

        var suyas = _repositorio.ObtenerActividadesDeSocio(SocioSeleccionado.Id);
        Rellenar(ActividadesDelSocio, suyas);
        Rellenar(ActividadesDisponibles, todas.Where(a => suyas.All(s => s.Id != a.Id)));
    }

    /// <summary>Ejecuta una operación de datos y, si falla, avisa al usuario.</summary>
    private void Ejecutar(Action operacion)
    {
        try
        {
            operacion();
        }
        catch (DatosException ex)
        {
            _dialogos.MostrarError(ex.Message, Titulo);
        }
    }

    private static void Rellenar<T>(ObservableCollection<T> coleccion, IEnumerable<T> elementos)
    {
        coleccion.Clear();
        foreach (var elemento in elementos)
        {
            coleccion.Add(elemento);
        }
    }

    private void Ficha_ErrorsChanged(object? sender, System.ComponentModel.DataErrorsChangedEventArgs e)
    {
        GuardarCommand.NotificarCambio();
    }
}
