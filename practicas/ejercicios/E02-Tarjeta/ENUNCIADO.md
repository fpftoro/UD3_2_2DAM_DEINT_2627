# E02 · Tarjeta de presentación

**Corrección:** autocorrección, con el profesor o con IA

Escribe **a mano** el XAML de una tarjeta de presentación con texto enriquecido, un enlace, un degradado y un botón de contenido compuesto; construye otra parte de la ventana **desde C#** y compara las dos formas de hacer lo mismo leyendo el código que genera el compilador.

**CE:** RA1 e, f, h · **Dificultad:** ★☆☆ · **Tiempo estimado:** 2 h · **Temas:** 3 y 5

- [Punto de partida](#punto-de-partida)
- [Requisitos](#requisitos)
- [Contrato de entrega](#contrato-de-entrega)
- [Comprueba tu solución](#comprueba-tu-solución)
- [Criterios de evaluación](#criterios-de-evaluación)

## Punto de partida

No hay código de partida. Crea el proyecto como en el tema 2 y copia en la carpeta `E02-Tarjeta` de tu repositorio el `README.md` de esta carpeta (plantilla de entrega).

La tarjeta es de una **persona inventada** (nombre, ciudad, profesión, correo y web de ejemplo, como `https://example.org/…`, un dominio reservado para ejemplos). No pongas datos reales.

```text
┌────────────────────────────────────────────────────────┐
│  ╭────╮   Marta Ríos                                   │
│  │ MR │   Hola, soy Marta Ríos, diseñadora de          │
│  ╰────╯   interfaces de escritorio en Sevilla.         │
│           Puedes ver mis trabajos en mi portafolio o   │
│           escribirme. (Respondo en 24 horas.)          │
│                                                        │
│  Habilidades                                           │
│  (XAML) (C#) (Figma) (Accesibilidad) (MVVM)            │
│                                                        │
│  [Contactar · respuesta en 24 h] [Compartir · copia…]  │
└────────────────────────────────────────────────────────┘
```

## Requisitos

**El XAML, escrito a mano** (puedes usar el diseñador para comprobar el resultado, pero no para colocar los controles):

1. Solución `Tarjeta.slnx` con el proyecto `Tarjeta` (WPF, .NET 10). Ventana con el título `Tarjeta de presentación`, de 560 × 400 y un mínimo de 420 × 340.
2. En `Window.Resources`: un `SolidColorBrush` con la clave `ColorTarjeta` y un `Style` con la clave `BotonTarjeta` para los botones (relleno 12,6 y margen derecho de 8).
3. Un `Grid` raíz con margen 16, dos columnas (`Auto` y `*`) y cuatro filas (`Auto`, `Auto`, `*` y `Auto`).
4. El **avatar**, en la primera columna y ocupando dos filas: un `Grid` de 96 × 96 con un `Ellipse` y un `TextBlock` con las iniciales centradas. El relleno del `Ellipse` es un `LinearGradientBrush` de dos colores, escrito como **elemento de propiedad** (`<Ellipse.Fill>`).
5. El nombre en grande en la primera fila de la segunda columna, y debajo un `TextBlock` `txtPresentacion` que ajusta las líneas y tiene:
   - una primera frase con el nombre en negrita (`Bold`) y la profesión en cursiva (`Italic`), y un salto de línea (`LineBreak`);
   - un `Span` con el color `{StaticResource ColorTarjeta}` que contiene **dos `Run` y un `Hyperlink`** a la web inventada;
   - fuera del `Span`, un último `Run` en gris y cursiva.
6. El `Hyperlink` abre el navegador con su manejador de `RequestNavigate` (tema 5).
7. En la última fila, un `StackPanel` horizontal `pnlBotones` con el botón `btnContactar`, que usa el estilo `BotonTarjeta` y cuyo **contenido** es un `StackPanel` horizontal con tres `TextBlock`: un icono (`✉`) del color de la tarjeta, el texto `Contactar` en negrita y un texto en gris. Al pulsarlo, un `MessageBox` muestra el correo.

**La misma interfaz, desde C#:**

8. En la tercera fila, el título «Habilidades» y un `WrapPanel` `pnlHabilidades` **vacío en el XAML**. El método `CrearEtiquetas()`, llamado desde el constructor después de `InitializeComponent`, le añade al menos cinco etiquetas: un `Border` redondeado con el fondo `ColorTarjeta` (obtenido con `FindResource`) y un `TextBlock` blanco dentro.
9. El método `CrearBotonCompartir()` construye **en C#** un botón con la misma estructura que `btnContactar` (estilo `BotonTarjeta`, `StackPanel` con icono `🔗`, `Compartir` en negrita y un texto en gris), le asocia con `+=` un manejador que copia la dirección de la web al portapapeles (`Clipboard.SetText`) y avisa con un `MessageBox`. El constructor lo añade a `pnlBotones`.

**El análisis, en el README:**

10. **XAML frente a C#**: una tabla que relacione cada forma de escribir algo en XAML que hayas usado en `btnContactar` (atributo, contenido entre etiquetas, `{StaticResource}`, `Click="…"`) con su equivalente en `CrearBotonCompartir()`, y cuántas líneas ocupa cada versión. Con el **Árbol visual activo** (tema 3), los elementos que hay dentro de cada uno de los dos botones. ¿Cuándo usarías cada forma?
11. **Lo que genera el compilador**: compila, abre `obj/Debug/net10.0-windows/MainWindow.g.cs` y explica qué campos genera y por qué el avatar no tiene ninguno, qué hace `InitializeComponent` y qué línea de `Connect` asocia `btnContactar_Click`. Compárala con tu `+=` del requisito 9.
12. Completa las capturas y las pruebas realizadas, y haz commits a medida que avances.

## Contrato de entrega

| Elemento | Debe ser |
|----------|----------|
| Solución y proyecto | `E02-Tarjeta/Tarjeta.slnx` con el proyecto `Tarjeta` (WPF, .NET 10) |
| Recursos | `ColorTarjeta` (`SolidColorBrush`) y `BotonTarjeta` (`Style` de `Button`) en `Window.Resources` |
| Controles con nombre | `txtPresentacion`, `pnlHabilidades`, `pnlBotones`, `btnContactar` |
| Métodos | `CrearEtiquetas()` y `CrearBotonCompartir()` (que devuelve el `Button`), llamados desde el constructor |
| Manejadores | `btnContactar_Click`, `btnCompartir_Click` y `Hyperlink_RequestNavigate` |
| Secciones del README | `## Capturas`, `## XAML frente a C#`, `## Lo que genera el compilador`, `## Pruebas realizadas` |

## Comprueba tu solución

1. La primera frase muestra el nombre en negrita y la profesión en cursiva; la segunda línea, con el enlace, va del color de la tarjeta; el texto final, en gris y cursiva, **no** toma el color del `Span`.
2. Mueve el último `Run` dentro del `Span`: sigue gris. ¿Por qué? (Piensa en el orden de prioridad de las propiedades de dependencia del tema 3.) Devuélvelo a su sitio.
3. Pulsa el enlace: se abre el navegador con la web de ejemplo.
4. Pulsa **Contactar**: un mensaje con el correo. Pulsa **Compartir** y pega en el Bloc de notas: aparece la dirección.
5. Los dos botones se ven iguales (salvo el texto y el icono).
6. Estrecha la ventana: el texto salta de línea y las etiquetas pasan a la línea siguiente.
7. Cambia el color de `ColorTarjeta` en el XAML: cambian a la vez la segunda línea del texto, los iconos y las etiquetas.
8. Quita el `x:Name` de `pnlHabilidades` y compila: ¿qué error da y por qué? Vuelve a ponerlo.

> **Desafío (opcional):** escribe en C# también el avatar, con su degradado (`new LinearGradientBrush(…)`), y compara cuántas líneas ocupa.

## Criterios de evaluación

- ✅ Los recursos y el estilo están en `Window.Resources` y se usan con `{StaticResource}` y `FindResource`
- ✅ Se usan elementos de propiedad y propiedades adjuntas donde hacen falta
- ✅ El texto mezcla formatos con elementos en línea y el enlace abre el navegador
- ✅ El botón tiene un contenido compuesto y un estilo
- ✅ Una parte de la interfaz se construye desde C# con el mismo resultado que en XAML
- ✅ Se comparan el XAML y el C# equivalente, también en el Árbol visual activo
- ✅ Se analiza el código que genera el compilador (`MainWindow.g.cs`)
- ✅ El README está completo y el historial de commits es progresivo
