using Club.Nucleo.Mvvm;
using Club.Nucleo.Servicios;

namespace Club.Nucleo.ViewModels;

/// <summary>
/// ViewModel de la ventana principal: decide qué pantalla se ve.
/// </summary>
public class PrincipalViewModel : ObjetoObservable
{
    private readonly IServicioSocios _servicio;
    private readonly SociosViewModel _socios;
    private ObjetoObservable _vistaActual;

    /// <summary>
    /// Crea el ViewModel principal y abre la pantalla de socios.
    /// </summary>
    /// <param name="servicio">Dónde se guardan los socios.</param>
    /// <param name="dialogos">Cómo se pregunta al usuario.</param>
    public PrincipalViewModel(IServicioSocios servicio, IServicioDialogos dialogos)
    {
        _servicio = servicio;
        _socios = new SociosViewModel(servicio, dialogos);
        _vistaActual = _socios;

        IrASociosCommand = new RelayCommand(() => VistaActual = _socios);
        IrAResumenCommand = new RelayCommand(() => VistaActual = new ResumenViewModel(_servicio));
    }

    /// <summary>
    /// ViewModel de la pantalla que se ve. La vista elige cómo dibujarlo con un DataTemplate.
    /// </summary>
    public ObjetoObservable VistaActual
    {
        get => _vistaActual;
        private set => Asignar(ref _vistaActual, value);
    }

    /// <summary>Muestra la pantalla de socios.</summary>
    public RelayCommand IrASociosCommand { get; }

    /// <summary>Muestra el resumen por categorías (se recalcula cada vez).</summary>
    public RelayCommand IrAResumenCommand { get; }
}
