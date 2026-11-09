namespace Autoevaluacion;

/// <summary>
/// Una pregunta del test, con cuatro opciones y una sola correcta.
/// </summary>
/// <param name="Enunciado">El texto de la pregunta.</param>
/// <param name="Opciones">Las cuatro opciones, en el orden A, B, C y D.</param>
/// <param name="Correcta">Posición de la opción correcta, de 0 (A) a 3 (D).</param>
/// <param name="Explicacion">Por qué la opción correcta lo es. Se muestra al resolver la pregunta.</param>
public record Pregunta(string Enunciado, string[] Opciones, int Correcta, string Explicacion);
