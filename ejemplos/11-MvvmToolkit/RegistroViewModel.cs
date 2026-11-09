using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace MvvmToolkit;

/// <summary>
/// Registro de actividad: recibe los mensajes de socios eliminados.
/// </summary>
public class RegistroViewModel : ObservableRecipient, IRecipient<SocioEliminadoMensaje>
{
    /// <summary>
    /// Crea el registro y lo activa: al activarse se suscribe a los mensajes que implementa.
    /// </summary>
    public RegistroViewModel()
    {
        IsActive = true;
    }

    /// <summary>Entradas del registro, la más reciente primero.</summary>
    public ObservableCollection<string> Entradas { get; } = [];

    /// <summary>Se ejecuta cada vez que alguien envía un <see cref="SocioEliminadoMensaje"/>.</summary>
    /// <param name="message">El mensaje recibido.</param>
    public void Receive(SocioEliminadoMensaje message)
    {
        Entradas.Insert(0, $"{DateTime.Now:HH:mm:ss} · Eliminado: {message.Value}");
    }
}
