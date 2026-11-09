namespace Autoevaluacion;

/// <summary>
/// Las preguntas del test. No modifiques esta clase: las comprobaciones del enunciado cuentan con estas preguntas.
/// </summary>
public static class BancoPreguntas
{
    /// <summary>
    /// Crea la lista de preguntas del test, siempre en el mismo orden.
    /// </summary>
    /// <returns>Cinco preguntas sobre los eventos de WPF.</returns>
    public static List<Pregunta> Crear() =>
    [
        new("¿Qué parámetro de un manejador indica qué control ha lanzado el evento?",
            ["e", "sender", "this", "Handled"],
            1,
            "sender es el objeto que lanza el evento; e lleva los datos del evento."),
        new("¿Cómo se llama un evento que sube desde el control hasta la ventana?",
            ["De túnel", "Directo", "De burbujeo", "Adjunto"],
            2,
            "Los eventos de burbujeo suben por el árbol; los de túnel (Preview…) bajan."),
        new("¿Qué consigue e.Handled = true en un evento enrutado?",
            ["Cerrar la ventana", "Deshacer la acción", "Lanzar el evento otra vez", "Que los siguientes manejadores de la ruta no lo reciban"],
            3,
            "Marca el evento como atendido: los manejadores que vienen detrás en la ruta ya no se ejecutan."),
        new("¿Qué evento permite cancelar el cierre de una ventana?",
            ["Closing", "Closed", "Unloaded", "Deactivated"],
            0,
            "Closing se lanza antes de cerrar y e.Cancel = true lo impide; Closed llega cuando ya se ha cerrado."),
        new("¿Qué devuelve MessageBox.Show con MessageBoxButton.YesNoCancel?",
            ["Un bool", "Un MessageBoxResult", "El texto del botón pulsado", "Nada"],
            1,
            "Devuelve un MessageBoxResult (Yes, No o Cancel) que se compara con un switch o un if."),
    ];
}
