namespace Estudiantes;

/// <summary>
/// Datos de ejemplo para la ventana. No modifiques esta clase: las comprobaciones del enunciado cuentan con estos datos.
/// </summary>
public static class DatosIniciales
{
    /// <summary>Los cursos que se pueden elegir. El último no tiene estudiantes al empezar.</summary>
    public static readonly string[] Cursos = ["1.º DAM", "2.º DAM", "1.º DAW", "2.º DAW"];

    /// <summary>
    /// Crea los diez estudiantes de ejemplo.
    /// </summary>
    /// <returns>Una lista nueva cada vez.</returns>
    public static List<Estudiante> Crear() =>
    [
        new Estudiante { Nombre = "Ana López", Edad = 19, Nota = 8.5, Curso = "1.º DAM" },
        new Estudiante { Nombre = "Bruno Díaz", Edad = 20, Nota = 4, Curso = "1.º DAM" },
        new Estudiante { Nombre = "Carla Ruiz", Edad = 18, Nota = 6.5, Curso = "1.º DAM" },
        new Estudiante { Nombre = "David Gil", Edad = 21, Nota = 7, Curso = "2.º DAM" },
        new Estudiante { Nombre = "Elena Martín", Edad = 20, Nota = 9, Curso = "2.º DAM" },
        new Estudiante { Nombre = "Fernando Soto", Edad = 22, Nota = 3.5, Curso = "2.º DAM" },
        new Estudiante { Nombre = "Gema Vidal", Edad = 21, Nota = 5, Curso = "2.º DAM" },
        new Estudiante { Nombre = "Hugo Navarro", Edad = 19, Nota = 6, Curso = "1.º DAW" },
        new Estudiante { Nombre = "Irene Castro", Edad = 25, Nota = 4.5, Curso = "1.º DAW" },
        new Estudiante { Nombre = "Javier Prieto", Edad = 18, Nota = 7.5, Curso = "1.º DAW" },
    ];
}
