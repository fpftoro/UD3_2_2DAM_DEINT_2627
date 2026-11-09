# E04 · Test de autoevaluación

**Corrección:** autocorrección, con el profesor o con IA

Programa un test de cinco preguntas que se responde con el ratón o con el teclado: cada pregunta admite **dos intentos**, un solo manejador en el panel atiende a las cuatro opciones gracias al **burbujeo**, el teclado se captura con un evento de **túnel** y la ventana pregunta antes de cerrarse si el resultado no se ha guardado.

**CE:** RA1 d, g, h · **Dificultad:** ★★☆ · **Tiempo estimado:** 3 h · **Temas:** 5 y 6

- [Punto de partida](#punto-de-partida)
- [Requisitos](#requisitos)
- [Contrato de entrega](#contrato-de-entrega)
- [Comprueba tu solución](#comprueba-tu-solución)
- [Criterios de evaluación](#criterios-de-evaluación)

## Punto de partida

Crea el proyecto como en el tema 2 y copia en la carpeta `E04-Autoevaluacion` de tu repositorio el `README.md` de esta carpeta (plantilla de entrega). La carpeta `partida/` trae dos clases que copias en el proyecto **sin modificarlas**:

- `Pregunta.cs`: un `record` con el enunciado, las cuatro opciones, la posición de la correcta (de 0 a 3) y una explicación.
- `BancoPreguntas.cs`: `BancoPreguntas.Crear()` devuelve las cinco preguntas, siempre en el mismo orden.

```text
┌──────────────────────────────────────────────────────────┐
│ Pregunta 2 de 5                                Puntos: 2 │
│                                                          │
│ ¿Cómo se llama un evento que sube desde el control       │
│ hasta la ventana?                                        │
│                                                          │
│ ○ A) De túnel                                            │
│ ○ B) Directo                                             │
│ ● C) De burbujeo                                         │
│ ○ D) Adjunto                                             │
│                                                          │
│ ✓ ¡Correcto! Los eventos de burbujeo suben…              │
│               [Guardar resultado] [Corregir] [Siguiente] │
└──────────────────────────────────────────────────────────┘
```

## Requisitos

**La ventana:**

1. Solución `Autoevaluacion.slnx` con el proyecto `Autoevaluacion` (WPF, .NET 10). Ventana con el título `Test de autoevaluación`, de 640 × 460 y un mínimo de 480 × 400.
2. Arriba, `txtProgreso` (`Pregunta n de 5`) y, a la derecha, `txtPuntos`. Debajo, el enunciado en `txtEnunciado`, que ajusta las líneas.
3. Las cuatro opciones son `RadioButton` (`rbOpcionA` a `rbOpcionD`) dentro de un `StackPanel` `pnlOpciones`. Su texto (`A) …`) se pone desde el código con la pregunta actual.
4. Debajo, `txtResultado` para los mensajes, y abajo a la derecha `btnGuardar` («Guardar resultado»), `btnCorregir` (botón por defecto) y `btnSiguiente`, todos con tecla de acceso.

**El estado de los controles:**

5. `btnCorregir` solo está activo cuando hay una opción marcada y la pregunta no se ha resuelto; `btnSiguiente`, solo cuando la pregunta se ha resuelto; `btnGuardar`, solo al terminar el test.
6. En la última pregunta, `btnSiguiente` dice «Ver resultado».

**Los eventos:**

7. **Burbujeo:** un único manejador `pnlOpciones_Checked`, asociado en el **panel** con `RadioButton.Checked="…"`, activa `btnCorregir` cuando se marca cualquiera de las opciones. Usa `e.OriginalSource` para comprobar que el evento viene de un `RadioButton`. Las opciones no tienen manejador propio de `Checked`.
8. **Ratón:** un **clic derecho** en una opción la **descarta** (se ve atenuada, con `Opacity` 0,4) o la recupera si ya lo estaba, sin marcarla ni desmarcarla; al pasar a otra pregunta, ninguna está descartada. Al **pasar el ratón** por una opción, `txtAyuda` (debajo de las opciones, en gris) dice `Clic derecho para descartar la opción B` (o `recuperar`), y se vacía al salir. Las cuatro opciones **comparten** los manejadores `rbOpcion_MouseRightButtonUp`, `rbOpcion_MouseEnter` y `rbOpcion_MouseLeave`: estos tres eventos son **directos** (no burbujean), así que no se pueden atender en el panel como `Checked`.
9. **Corregir** (`btnCorregir_Click`): si la opción es la correcta, suma 2 puntos en el primer intento y 1 en el segundo, y resuelve la pregunta. Si falla el primer intento, desmarca y **desactiva** la opción elegida y muestra `Te queda 1 intento`. Si falla el segundo, resuelve la pregunta mostrando cuál era la correcta. Al resolver, se muestra la explicación, se desactivan las opciones y el foco va a `btnSiguiente`.
10. **Siguiente** (`btnSiguiente_Click`) pasa a la pregunta siguiente. Tras la última, un `MessageBox` con los botones Sí y No muestra los puntos y pregunta si se quiere repetir: **Sí** vuelve a empezar; **No** deja el resultado en `txtResultado` y activa `btnGuardar`.
11. **Teclado (túnel):** en el `PreviewKeyDown` de la **ventana**, las teclas `A` a `D` y `1` a `4` (también las del teclado numérico) marcan la opción correspondiente si está activa y le dan el foco. La tecla se marca como atendida (`e.Handled = true`). Con `Ctrl` o `Alt` pulsadas no se hace nada, para no romper las teclas de acceso.
12. **Guardar** (`btnGuardar_Click` y un método `GuardarResultado()` que devuelve `bool`) añade al fichero `autoevaluacion.txt` de la carpeta Documentos una línea con la fecha y la hora, los puntos, las preguntas respondidas y si el test está terminado o a medias. Los errores de escritura (`IOException`, `UnauthorizedAccessException`) se capturan y se avisa con un `MessageBox` de error: la aplicación nunca se cierra por ellos.
13. **Cierre** (`Window_Closing`): si hay preguntas respondidas y el resultado no se ha guardado, un `MessageBox` con Sí, No y Cancelar (Cancelar por defecto) pregunta si se quiere guardar antes de salir. **Sí** guarda y cierra (si guardar falla, no cierra); **No** cierra; **Cancelar** no cierra.

**Entrega:**

14. Completa el `README.md`: capturas, la tabla de eventos, la ruta del evento `Checked`, lo que pasa sin el `try-catch` y las pruebas realizadas. Haz commits a medida que avances.

## Contrato de entrega

| Elemento | Debe ser |
|----------|----------|
| Solución y proyecto | `E04-Autoevaluacion/Autoevaluacion.slnx` con el proyecto `Autoevaluacion` (WPF, .NET 10), con `Pregunta.cs` y `BancoPreguntas.cs` sin modificar |
| Controles con nombre | `txtProgreso`, `txtPuntos`, `txtEnunciado`, `pnlOpciones`, `rbOpcionA`, `rbOpcionB`, `rbOpcionC`, `rbOpcionD`, `txtAyuda`, `txtResultado`, `btnGuardar`, `btnCorregir`, `btnSiguiente` |
| Manejadores | `pnlOpciones_Checked`, `rbOpcion_MouseRightButtonUp`, `rbOpcion_MouseEnter` y `rbOpcion_MouseLeave` (compartidos por las cuatro opciones), `btnCorregir_Click`, `btnSiguiente_Click`, `btnGuardar_Click`, `Window_PreviewKeyDown`, `Window_Closing` |
| Métodos | `GuardarResultado()`, que devuelve `bool` |
| Fichero de resultados | `autoevaluacion.txt` en la carpeta Documentos del usuario, una línea por resultado |
| Secciones del README | `## Capturas`, `## Eventos`, `## La ruta del evento Checked`, `## Errores al guardar`, `## Pruebas realizadas` |

## Comprueba tu solución

Las respuestas correctas son, por orden, **B, C, D, A y B**.

1. Al arrancar: `Pregunta 1 de 5`, `Puntos: 0` y **Corregir** desactivado.
2. Pulsa la tecla `B`: se marca la opción B y se activa **Corregir**. Pulsa `Intro`: `¡Correcto!` con la explicación y `Puntos: 2`.
3. **Siguiente**. Elige A y corrige: `Te queda 1 intento`, y la A queda desmarcada y desactivada. Elige B y corrige: se muestra que la correcta era la C; los puntos siguen en 2.
4. **Siguiente**. Pulsa `1`: se marca la A. Corrige (falla), pulsa `4` y corrige: correcto en el segundo intento, `Puntos: 3`.
5. Cierra la ventana: pregunta si quieres guardar. **Cancelar**: la ventana sigue abierta. Ciérrala otra vez y elige **No**: se cierra.
6. Vuelve a ejecutar y responde todo bien a la primera: `10 de 10 puntos` y la pregunta de repetir. **No**: se activa **Guardar resultado**. Púlsalo: el fichero `autoevaluacion.txt` de Documentos tiene una línea nueva con `10 de 10 puntos · 5 de 5 preguntas (terminado)`.
7. Marca `autoevaluacion.txt` como de **solo lectura** (en sus propiedades). Ejecuta, responde la primera pregunta y cierra la ventana eligiendo **Sí**: aparece un mensaje de error y la ventana **no** se cierra. Quita el solo lectura y vuelve a cerrar con **Sí**: se guarda una línea con `(a medias)` y la ventana se cierra.
8. Quita el `try-catch` de `GuardarResultado`, repite la comprobación 7 y apunta en el README qué ocurre. Vuelve a ponerlo.
9. Pon un punto de interrupción en `pnlOpciones_Checked` y marca una opción: ¿qué hay en `sender` y qué en `e.OriginalSource`?
10. Con el foco en un botón, pulsa `Alt+C`: se ejecuta **Corregir** (si está activo) y no se marca la opción C.
11. Pasa el ratón por la opción B: debajo aparece `Clic derecho para descartar la opción B`. Haz clic derecho: la B se atenúa, no se marca y la ayuda dice `recuperar`. Otro clic derecho la recupera. Al pasar a la pregunta siguiente, ninguna opción está atenuada.

> **Desafío (opcional):** muestra junto a cada opción un icono (✅ o ❌) al resolver la pregunta, en lugar de solo el texto de `txtResultado`.

## Criterios de evaluación

- ✅ El estado de cada control (activo, marcado, texto) refleja el momento del test
- ✅ Un solo manejador en el panel atiende a las cuatro opciones gracias al burbujeo, y las opciones comparten los manejadores del ratón
- ✅ La corrección con dos intentos, los puntos y el final del test funcionan con los eventos `Click`
- ✅ El teclado se captura en la ventana con un evento de túnel y `Handled`
- ✅ La ventana pregunta antes de cerrarse y puede cancelar el cierre
- ✅ Los errores al guardar se capturan sin cerrar la aplicación
- ✅ El README explica los eventos usados y el historial de commits es progresivo
