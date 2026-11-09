using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MvvmToolkit;

/// <summary>
/// Mensaje que se envía cuando se elimina un socio. Lleva su nombre.
/// </summary>
/// <param name="nombre">Nombre del socio eliminado.</param>
public class SocioEliminadoMensaje(string nombre) : ValueChangedMessage<string>(nombre);
