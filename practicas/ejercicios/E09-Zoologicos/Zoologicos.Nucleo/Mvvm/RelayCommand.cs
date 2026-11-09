using System.Windows.Input;

namespace Zoologicos.Nucleo.Mvvm;

/// <summary>
/// Comando que delega en dos métodos: qué hace (<c>Execute</c>) y si se puede ejecutar (<c>CanExecute</c>).
/// </summary>
public class RelayCommand : ICommand
{
    private readonly Action _ejecutar;
    private readonly Func<bool>? _puedeEjecutar;

    /// <summary>
    /// Crea el comando.
    /// </summary>
    /// <param name="ejecutar">Lo que hace el comando.</param>
    /// <param name="puedeEjecutar">Si se puede ejecutar ahora; si se omite, siempre se puede.</param>
    public RelayCommand(Action ejecutar, Func<bool>? puedeEjecutar = null)
    {
        _ejecutar = ejecutar;
        _puedeEjecutar = puedeEjecutar;
    }

    /// <summary>Avisa a los botones de que deben volver a preguntar <see cref="CanExecute"/>.</summary>
    public event EventHandler? CanExecuteChanged;

    /// <summary>Indica si el comando se puede ejecutar.</summary>
    /// <param name="parameter">No se usa.</param>
    /// <returns><c>true</c> si se puede ejecutar.</returns>
    public bool CanExecute(object? parameter) => _puedeEjecutar?.Invoke() ?? true;

    /// <summary>Ejecuta el comando.</summary>
    /// <param name="parameter">No se usa.</param>
    public void Execute(object? parameter) => _ejecutar();

    /// <summary>
    /// Lanza <see cref="CanExecuteChanged"/>. Se llama cuando cambia algo de lo que depende <c>CanExecute</c>.
    /// </summary>
    public void NotificarCambio() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
