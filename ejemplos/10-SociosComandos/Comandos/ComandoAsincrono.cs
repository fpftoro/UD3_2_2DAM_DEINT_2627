using System.Windows.Input;

namespace SociosComandos.Comandos;

/// <summary>
/// Comando que ejecuta una tarea asíncrona y no se puede volver a lanzar mientras está en marcha.
/// </summary>
public class ComandoAsincrono : ICommand
{
    private readonly Func<Task> _ejecutar;
    private readonly Func<bool>? _puedeEjecutar;
    private bool _enMarcha;

    /// <summary>
    /// Crea el comando.
    /// </summary>
    /// <param name="ejecutar">La tarea que lanza el comando.</param>
    /// <param name="puedeEjecutar">Si se puede ejecutar ahora; si se omite, siempre se puede.</param>
    public ComandoAsincrono(Func<Task> ejecutar, Func<bool>? puedeEjecutar = null)
    {
        _ejecutar = ejecutar;
        _puedeEjecutar = puedeEjecutar;
    }

    /// <summary>Avisa a los botones de que deben volver a preguntar <see cref="CanExecute"/>.</summary>
    public event EventHandler? CanExecuteChanged;

    /// <summary>Indica si el comando se puede ejecutar: no lo está ya y lo permite la condición.</summary>
    /// <param name="parameter">No se usa.</param>
    /// <returns><c>true</c> si se puede ejecutar.</returns>
    public bool CanExecute(object? parameter) => !_enMarcha && (_puedeEjecutar?.Invoke() ?? true);

    /// <summary>
    /// Ejecuta la tarea. <c>ICommand.Execute</c> devuelve <c>void</c>, así que este es
    /// uno de los pocos sitios donde se admite <c>async void</c>.
    /// </summary>
    /// <param name="parameter">No se usa.</param>
    public async void Execute(object? parameter)
    {
        _enMarcha = true;
        NotificarCambio();
        try
        {
            await _ejecutar();
        }
        finally
        {
            _enMarcha = false;
            NotificarCambio();
        }
    }

    /// <summary>Lanza <see cref="CanExecuteChanged"/>.</summary>
    public void NotificarCambio() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
