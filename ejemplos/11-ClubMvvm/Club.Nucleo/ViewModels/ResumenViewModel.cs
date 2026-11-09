using Club.Nucleo.Modelos;
using Club.Nucleo.Mvvm;
using Club.Nucleo.Servicios;

namespace Club.Nucleo.ViewModels;

/// <summary>
/// Una fila del resumen: datos de una categoría.
/// </summary>
/// <param name="Categoria">Nombre de la categoría.</param>
/// <param name="Socios">Número de socios.</param>
/// <param name="Cuotas">Suma de sus cuotas mensuales.</param>
public record FilaResumen(string Categoria, int Socios, decimal Cuotas);

/// <summary>
/// Pantalla de resumen: socios y cuotas por categoría.
/// </summary>
public class ResumenViewModel : ObjetoObservable
{
    /// <summary>
    /// Crea el resumen con los datos actuales del servicio.
    /// </summary>
    /// <param name="servicio">Dónde se guardan los socios.</param>
    public ResumenViewModel(IServicioSocios servicio)
    {
        var socios = servicio.ObtenerTodos();
        Filas = CalculadoraCuotas.Categorias
            .Select(c => new FilaResumen(
                c,
                socios.Count(s => s.Categoria == c),
                socios.Where(s => s.Categoria == c).Sum(s => s.Cuota)))
            .ToList();
        Total = socios.Sum(s => s.Cuota);
    }

    /// <summary>Una fila por categoría.</summary>
    public IReadOnlyList<FilaResumen> Filas { get; }

    /// <summary>Suma de todas las cuotas.</summary>
    public decimal Total { get; }
}
