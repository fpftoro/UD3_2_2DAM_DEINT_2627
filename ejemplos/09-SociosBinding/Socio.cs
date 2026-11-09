namespace SociosBinding;

/// <summary>
/// Un socio del club. Avisa de sus cambios para que la interfaz se actualice sola.
/// </summary>
public class Socio : ObjetoObservable
{
    private string _nombre = "";
    private string _categoria = "Adulto";
    private int _antiguedad;

    /// <summary>Nombre completo.</summary>
    public string Nombre
    {
        get => _nombre;
        set => Asignar(ref _nombre, value);
    }

    /// <summary>Categoría: Infantil, Adulto o Senior.</summary>
    public string Categoria
    {
        get => _categoria;
        set
        {
            if (Asignar(ref _categoria, value))
            {
                OnPropertyChanged(nameof(Cuota));
            }
        }
    }

    /// <summary>Años como socio.</summary>
    public int Antiguedad
    {
        get => _antiguedad;
        set
        {
            if (Asignar(ref _antiguedad, value))
            {
                OnPropertyChanged(nameof(Cuota));
            }
        }
    }

    /// <summary>Cuota mensual, calculada a partir de la categoría y la antigüedad.</summary>
    public decimal Cuota => CalculadoraCuotas.Calcular(Categoria, Antiguedad);
}
