namespace ReservaPista;

/// <summary>
/// Un socio del club, tal como se muestra en la tabla.
/// </summary>
/// <param name="Nombre">Nombre completo.</param>
/// <param name="Categoria">Categoría: infantil, adulto o senior.</param>
/// <param name="Antiguedad">Años como socio.</param>
public record Socio(string Nombre, string Categoria, int Antiguedad);
