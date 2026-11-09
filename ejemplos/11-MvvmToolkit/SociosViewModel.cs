using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace MvvmToolkit;

/// <summary>
/// Lista de socios con el CommunityToolkit.Mvvm: compárala con la del tema 10.
/// </summary>
public partial class SociosViewModel : ObservableObject
{
    /// <summary>
    /// Crea el ViewModel con tres socios de ejemplo.
    /// </summary>
    public SociosViewModel()
    {
        Socios =
        [
            new Socio("Lucía Pérez", "Adulto", 12),
            new Socio("Mario Gil", "Infantil", 2),
            new Socio("Carmen Ruiz", "Senior", 25)
        ];
        Estado = "Listo";
    }

    /// <summary>Socios del club.</summary>
    public ObservableCollection<Socio> Socios { get; }

    /// <summary>Categorías que se pueden elegir.</summary>
    public string[] Categorias { get; } = ["Infantil", "Adulto", "Senior"];

    /// <summary>Socio elegido. Al cambiar, el comando Eliminar vuelve a comprobar si se puede ejecutar.</summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(EliminarCommand))]
    public partial Socio? SocioSeleccionado { get; set; }

    /// <summary>Mensaje de la barra de estado.</summary>
    [ObservableProperty]
    public partial string Estado { get; set; }

    /// <summary>Genera <c>NuevoCommand</c>.</summary>
    [RelayCommand]
    private void Nuevo()
    {
        var socio = new Socio("Nuevo socio", "Adulto", 0);
        Socios.Add(socio);
        SocioSeleccionado = socio;
    }

    /// <summary>Genera <c>EliminarCommand</c>, que solo se puede ejecutar si <see cref="HaySeleccion"/>.</summary>
    [RelayCommand(CanExecute = nameof(HaySeleccion))]
    private void Eliminar()
    {
        if (SocioSeleccionado is not { } socio)
        {
            return;
        }

        Socios.Remove(socio);
        SocioSeleccionado = null;

        // Quien esté suscrito a este tipo de mensaje se entera, sin que este ViewModel lo conozca
        WeakReferenceMessenger.Default.Send(new SocioEliminadoMensaje(socio.Nombre));
    }

    private bool HaySeleccion() => SocioSeleccionado is not null;

    /// <summary>
    /// Genera <c>ExportarCommand</c> (asíncrono: se deshabilita mientras se ejecuta)
    /// y, con <c>IncludeCancelCommand</c>, <c>ExportarCancelCommand</c> para cancelarlo.
    /// </summary>
    /// <param name="cancelacion">Se activa al ejecutar el comando de cancelar.</param>
    [RelayCommand(IncludeCancelCommand = true)]
    private async Task ExportarAsync(CancellationToken cancelacion)
    {
        Estado = "Exportando…";
        try
        {
            for (var i = 0; i < Socios.Count; i++)
            {
                await Task.Delay(500, cancelacion); // simula una exportación lenta: medio segundo por socio
            }

            Estado = $"Exportados {Socios.Count} socios";
        }
        catch (OperationCanceledException)
        {
            Estado = "Exportación cancelada";
        }
    }
}
