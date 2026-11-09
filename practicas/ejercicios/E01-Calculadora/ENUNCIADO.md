# E01 · Calculadora con el editor visual

**Corrección:** autocorrección, con el profesor o con IA

Crea con el **editor visual** de Visual Studio una calculadora de cuatro operaciones cuya ventana se adapte al tamaño, **lee el XAML** que genera el diseñador y déjalo limpio: filas y columnas en lugar de márgenes calculados.

**CE:** RA1 b, c, d, e, h · **Dificultad:** ★☆☆ · **Tiempo estimado:** 2 h · **Temas:** 2 y 4

- [Punto de partida](#punto-de-partida)
- [Requisitos](#requisitos)
- [Contrato de entrega](#contrato-de-entrega)
- [Comprueba tu solución](#comprueba-tu-solución)
- [Criterios de evaluación](#criterios-de-evaluación)

## Punto de partida

No hay código de partida: creas el proyecto desde cero, como en el tema 2. Copia en la carpeta `E01-Calculadora` de tu repositorio de la unidad el `README.md` de esta carpeta, que es la **plantilla de entrega**: lo irás completando mientras trabajas.

```text
┌─────────────────────────┐
│                     7 + │  ← txtOperacion: operación pendiente
│ ┌─────────────────────┐ │
│ │                   5 │ │  ← txtPantalla: número actual
│ └─────────────────────┘ │
│ [    C    ] [ ⌫ ] [ ÷ ] │
│ [ 7 ] [ 8 ] [ 9 ] [ × ] │
│ [ 4 ] [ 5 ] [ 6 ] [ − ] │
│ [ 1 ] [ 2 ] [ 3 ] [ + ] │
│ [    0    ] [ , ] [ = ] │
└─────────────────────────┘
```

## Requisitos

**Con el editor visual (antes de tocar el XAML a mano):**

1. Crea en la carpeta `E01-Calculadora` una solución `Calculadora.slnx` con un proyecto **Aplicación WPF** llamado `Calculadora`, para .NET 10.
2. Con la ventana Propiedades (`F4`), pon a la ventana el título `Calculadora`, un tamaño de 320 × 480 y un tamaño mínimo de 260 × 380.
3. Arrastra desde el Cuadro de herramientas un `TextBox` y tres `Button`, y colócalos con el ratón donde te parezca. **Haz un commit en este punto** y copia en el README (sección «El código que generó el diseñador») el XAML que ha generado el diseñador para uno de los botones.

**Distribución con un `Grid`** (ahora sí, editando el XAML y con las reglas del diseñador):

4. El `Grid` raíz tiene un margen de 8, **cuatro columnas** iguales (`*`) y **siete filas**: dos `Auto` para la pantalla y cinco `*` para los botones.
5. Fila 0: un `TextBlock` `txtOperacion` (gris, alineado a la derecha, tamaño 16) que muestra la operación pendiente. Fila 1: un `TextBox` `txtPantalla` de solo lectura, alineado a la derecha, tamaño 36, con el texto inicial `0`. Los dos ocupan las cuatro columnas.
6. Los 18 botones se colocan como en el esquema, con `Grid.Row`, `Grid.Column` y `Grid.ColumnSpan` (el `C` y el `0` ocupan dos columnas). **Ningún control** tiene `Margin` calculado por el diseñador, `Width`, `Height`, `HorizontalAlignment` ni `VerticalAlignment` puestos por el arrastre.
7. Los botones comparten un `Style` en `Window.Resources` (margen 4 y tamaño de letra 22): no repitas esas propiedades en cada botón.
8. Al agrandar la ventana, los botones crecen y la pantalla sigue arriba; al llevarla al tamaño mínimo, se ve todo.
9. Pon `AutomationProperties.Name` al botón `⌫` («Borrar la última cifra») y a la pantalla («Resultado»): un lector de pantalla no sabe leer el símbolo.

**Funcionamiento** (en el *code-behind*):

10. Las diez cifras comparten **un solo manejador** `btnDigito_Click`, y las cuatro operaciones, `btnOperacion_Click`. El botón pulsado llega en `sender`; su texto se lee con `(string)((Button)sender).Content`.
11. Una cifra sustituye al `0` inicial y al resultado anterior; después, se añade al final. La coma (`btnComa`) se añade solo una vez por número.
12. Una operación guarda el número de la pantalla y la operación, y la muestra en `txtOperacion` (`7 +`). Si ya había una pendiente con su segundo número, la resuelve antes (`8 − 3 − 2 =` da `3`).
13. `=` (`btnIgual`) resuelve la operación, muestra el resultado y deja en `txtOperacion` la expresión completa (`7 + 5 =`). Sin operación pendiente, no hace nada.
14. Dividir entre cero muestra `No se puede dividir entre cero`; la siguiente cifra empieza un número nuevo.
15. `C` (`btnBorrar`) vuelve al estado inicial; `⌫` (`btnRetroceso`) borra la última cifra del número que se está escribiendo (de un resultado, no).
16. Los números se leen y se muestran con coma decimal y sin ceros sobrantes (`6`, no `6,0`). Usa `decimal`: con `double`, `0,1 + 0,2` no da `0,3`.

**Entrega:**

17. Completa el `README.md`: capturas, el código que generó el diseñador y lo que cambiaste, la tabla de la distribución y las pruebas realizadas. Haz commits a medida que avances.

## Contrato de entrega

| Elemento | Debe ser |
|----------|----------|
| Solución y proyecto | `E01-Calculadora/Calculadora.slnx` con el proyecto `Calculadora` (WPF, .NET 10) |
| Ventana | `MainWindow`, título `Calculadora`, 320 × 480, mínimo 260 × 380 |
| Controles con nombre | `txtOperacion`, `txtPantalla`, `btnBorrar`, `btnRetroceso`, `btnComa`, `btnIgual` (las cifras y las operaciones no necesitan nombre) |
| Manejadores | `btnDigito_Click`, `btnOperacion_Click`, `btnComa_Click`, `btnIgual_Click`, `btnBorrar_Click` y `btnRetroceso_Click` |
| Commit | Uno con los controles colocados con el ratón, antes de reorganizarlos en el `Grid` |
| Secciones del README | `## Capturas`, `## El código que generó el diseñador`, `## Distribución`, `## Pruebas realizadas` |

## Comprueba tu solución

1. Pulsa `7`, `+`, `5`, `=`: la pantalla muestra `12` y encima se lee `7 + 5 =`.
2. `1`, `,`, `5`, `×`, `4`, `=` da `6` (no `6,0`). `0`, `,`, `1`, `+`, `0`, `,`, `2`, `=` da `0,3`.
3. `8`, `−`, `3`, `−` muestra `5` en la pantalla y `5 −` encima; después, `2`, `=` da `3`.
4. `9`, `÷`, `0`, `=` muestra `No se puede dividir entre cero`. Pulsa `4`: la pantalla muestra `4`.
5. Escribe `123` y pulsa `⌫` dos veces: queda `1`; una tercera vez: `0`. Después de un `=`, `⌫` no borra nada.
6. Pulsa `,` dos veces seguidas: solo aparece una coma (`0,`).
7. Maximiza la ventana: los botones crecen y no queda espacio vacío; llévala al tamaño mínimo: se ven todos los botones y la pantalla.
8. Abre el **Esquema del documento** (`Ver > Otras ventanas > Esquema del documento`): todos los botones cuelgan directamente del `Grid`.
9. Busca en `MainWindow.xaml` las palabras `Width`, `Height` y `Margin`: solo aparecen en la ventana, en el `Grid`, en el estilo y en los dos controles de la pantalla.

> **Desafío (opcional):** maneja el teclado de la ventana (`PreviewKeyDown`, tema 6) para que las cifras, `+`, `-`, `*`, `/`, `Intro` y `Retroceso` funcionen como los botones, llamando a los mismos métodos.

## Criterios de evaluación

- ✅ La ventana y sus primeros controles se crean con el editor visual, y el historial lo muestra
- ✅ Los controles se ubican con un `Grid` de filas y columnas, sin posiciones absolutas
- ✅ La ventana se adapta al tamaño y tiene un tamaño mínimo razonable
- ✅ Se analiza el XAML que generó el diseñador y se explica qué se ha cambiado y por qué
- ✅ Las propiedades comunes van en un estilo y los controles accesibles tienen nombre
- ✅ La calculadora funciona como dicen las comprobaciones, con manejadores compartidos
- ✅ El README está completo y el historial de commits es progresivo
