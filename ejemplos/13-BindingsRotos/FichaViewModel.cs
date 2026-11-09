using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BindingsRotos;

/// <summary>
/// Una actividad con plazas.
/// </summary>
/// <param name="Nombre">Nombre de la actividad.</param>
/// <param name="Plazas">Plazas totales.</param>
public record Actividad(string Nombre, int Plazas);

/// <summary>
/// ViewModel de la ventana. Tiene un fallo a propósito: <see cref="Visitas"/> no avisa de sus cambios.
/// </summary>
public class FichaViewModel : INotifyPropertyChanged
{
    private string _nombre = "Lucía Pérez";
    private int _antiguedad = 12;

    /// <summary>Se lanza cuando cambia una propiedad.</summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>Nombre del socio.</summary>
    public string Nombre
    {
        get => _nombre;
        set
        {
            _nombre = value;
            OnPropertyChanged();
        }
    }

    /// <summary>Años como socio.</summary>
    public int Antiguedad
    {
        get => _antiguedad;
        set
        {
            _antiguedad = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(Cuota));
        }
    }

    /// <summary>
    /// Veces que se ha pulsado «Visitar». ERROR 4: cambia, pero no lanza PropertyChanged.
    /// </summary>
    public int Visitas { get; set; }

    /// <summary>Cuota mensual del socio.</summary>
    public decimal Cuota => Antiguedad >= 10 ? 31.5m : 35m;

    /// <summary>Actividades del socio.</summary>
    public List<Actividad> Actividades { get; } = [new("Pádel", 8), new("Yoga", 2)];

    /// <summary>Suma una visita (y no avisa: por eso no se ve).</summary>
    public void Visitar() => Visitas++;

    private void OnPropertyChanged([CallerMemberName] string? propiedad = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propiedad));
    }
}
