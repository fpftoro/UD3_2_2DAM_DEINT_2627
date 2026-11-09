using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Zoologicos.Nucleo.Mvvm;

/// <summary>
/// Clase base que implementa <see cref="INotifyPropertyChanged"/> para no repetirlo en cada clase.
/// </summary>
public abstract class ObjetoObservable : INotifyPropertyChanged
{
    /// <summary>Se lanza cuando cambia el valor de una propiedad.</summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Avisa de que ha cambiado una propiedad.
    /// </summary>
    /// <param name="propiedad">Nombre de la propiedad; si se omite, el compilador pone el de quien llama.</param>
    protected void OnPropertyChanged([CallerMemberName] string? propiedad = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propiedad));
    }

    /// <summary>
    /// Asigna un valor al campo y avisa solo si ha cambiado.
    /// </summary>
    /// <typeparam name="T">Tipo de la propiedad.</typeparam>
    /// <param name="campo">Campo privado que guarda el valor.</param>
    /// <param name="valor">Valor nuevo.</param>
    /// <param name="propiedad">Nombre de la propiedad (lo pone el compilador).</param>
    /// <returns><c>true</c> si el valor ha cambiado.</returns>
    protected bool Asignar<T>(ref T campo, T valor, [CallerMemberName] string? propiedad = null)
    {
        if (EqualityComparer<T>.Default.Equals(campo, valor))
        {
            return false;
        }

        campo = valor;
        OnPropertyChanged(propiedad);
        return true;
    }
}
