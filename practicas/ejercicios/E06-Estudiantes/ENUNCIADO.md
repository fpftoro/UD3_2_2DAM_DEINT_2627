# E06 · Estudiantes por curso

**Corrección:** autocorrección, con el profesor o con IA

Muestra una lista de estudiantes **agrupada por curso**, con un buscador y una ficha que edita al elegido, **sin copiar datos a mano**: todo se enlaza con `{Binding}` a un *ViewModel* que avisa de sus cambios. Crea además un control propio con **propiedades de dependencia** que se usa en la lista y en la ficha.

**CE:** RA1 f, h · **Dificultad:** ★★☆ · **Tiempo estimado:** 4 h · **Tema:** 9

- [Punto de partida](#punto-de-partida)
- [Requisitos](#requisitos)
- [Contrato de entrega](#contrato-de-entrega)
- [Comprueba tu solución](#comprueba-tu-solución)
- [Criterios de evaluación](#criterios-de-evaluación)

## Punto de partida

Crea el proyecto como en el tema 2 y copia en la carpeta `E06-Estudiantes` de tu repositorio el `README.md` de esta carpeta (plantilla de entrega). La carpeta `partida/` trae `DatosIniciales.cs`, que copias en el proyecto **sin modificarlo**: crea los diez estudiantes de ejemplo y la lista de cursos. Hasta que escribas la clase `Estudiante`, no compilará.

```text
┌──────────────────────────────────────┬────────────────────────────┐
│ Buscar por nombre: [              ]  │ Ficha del estudiante       │
│ ┌──────────────────────────────────┐ │ Nombre: [Bruno Díaz      ] │
│ │ 1.º DAM (3)                      │ │ Edad:   [20 ]              │
│ │   Ana López      ▓▓▓▓▓▓▓▓░░  8,5 │ │ Nota:   ──────●──── [4,00] │
│ │   Bruno Díaz     ▓▓▓▓░░░░░░  4,0 │ │ ▓▓▓▓░░░░░░  4,0            │
│ │   Carla Ruiz     ▓▓▓▓▓▓▓░░░  6,5 │ │ Suspenso                   │
│ │ 1.º DAW (3)                      │ │ Curso:  [1.º DAM        ▾] │
│ │   …                              │ │                            │
│ └──────────────────────────────────┘ │                            │
│ Media: 6,15 · Aprobados: 7 de 10     │                            │
│ [Nuevo] [Eliminar]                   │                            │
└──────────────────────────────────────┴────────────────────────────┘
```

## Requisitos

**El modelo y el *ViewModel*** (como en el tema 9, en el mismo proyecto):

1. Solución `Estudiantes.slnx` con el proyecto `Estudiantes` (WPF, .NET 10).
2. `ObjetoObservable`: clase base abstracta que implementa `INotifyPropertyChanged`, con `OnPropertyChanged` (con `[CallerMemberName]`) y `Asignar<T>`, que asigna y avisa solo si el valor cambia.
3. `Estudiante : ObjetoObservable` con `Nombre` (`string`), `Edad` (`int`), `Nota` (`double`, recortada entre 0 y 10), `Curso` (`string`) y `Aprobado` (solo lectura: nota de al menos `NotaAprobado`, una constante igual a 5). Al cambiar `Nota`, también se avisa de que ha cambiado `Aprobado`.
4. `EstudiantesViewModel : ObjetoObservable` con:
   - `Estudiantes`: una `ObservableCollection<Estudiante>` con los de `DatosIniciales.Crear()`;
   - `Vista`: un `ICollectionView` de esa colección que **filtra** por `Filtro`, **agrupa** por `Curso` y **ordena** por curso y nombre, y se reorganiza solo (*live shaping*) al cambiar el curso o el nombre de un estudiante;
   - `Cursos` (los de `DatosIniciales.Cursos`), `Filtro`, `Seleccionado` y `HaySeleccion`;
   - `Media`, `Aprobados` y `Total`, que se actualizan al cambiar una nota y al añadir o quitar estudiantes;
   - `Nuevo()` (añade «Nuevo estudiante», de 18 años, en el primer curso, quita el filtro y lo elige) y `Eliminar()` (quita el elegido).
5. En `App.xaml.cs`, el idioma de los *bindings* es el de Windows (apartado 9.4.5 del tema 9): sin esto, `7,5` se lee como `75`.

**El control propio:**

6. `Controles/IndicadorNota`, un `UserControl` con dos **propiedades de dependencia**: `Nota` (`double`, por defecto 0) y `Color` (`Brush`, por defecto verde). Muestra una `ProgressBar` de 0 a 10 con ese color y la nota con un decimal. Sus *bindings* internos usan `RelativeSource` (sin `DataContext = this`).

**La ventana** (todo por *binding*; el *code-behind* solo asigna el `DataContext` y atiende los dos botones):

7. Izquierda: el buscador (`txtFiltro`, filtra con cada letra), la lista con una **cabecera por curso** con el número de estudiantes (`1.º DAM (3)`) y una `DataTemplate` con el nombre y un `IndicadorNota`, el resumen (`Media: 6,15 · Aprobados: 7 de 10`) y los botones **Nuevo** y **Eliminar** (este, activo solo con un elegido y con confirmación).
8. Derecha: la ficha del elegido, que solo se ve cuando hay uno (si no, un aviso «Elige un estudiante de la lista»), con su `DataContext` enlazado a `Seleccionado`:
   - `txtNombre`, que cambia la lista **mientras se escribe**;
   - `txtEdad`, que se actualiza al salir del cuadro;
   - `sldNota` (de 0 a 10, en saltos de 0,25) y un cuadro de texto con la nota con dos decimales, enlazados a la misma propiedad;
   - un `IndicadorNota`, un texto «Aprobado» o «Suspenso» (en rojo);
   - `cmbCurso`, con los cursos del *ViewModel*.
9. Un estilo `IndicadorSegunNota` con un `DataTrigger` sobre `Aprobado` pone el indicador en rojo para los suspensos; lo usan la lista y la ficha. La visibilidad de la ficha y el texto «Aprobado» o «Suspenso» también se resuelven con `DataTrigger` (tema 7), sin código.
10. La ventana tiene `d:DataContext` para que el editor sugiera las propiedades del *ViewModel*.

**Entrega:**

11. Completa el `README.md` y haz commits a medida que avances.

## Contrato de entrega

| Elemento | Debe ser |
|----------|----------|
| Solución y proyecto | `E06-Estudiantes/Estudiantes.slnx` con el proyecto `Estudiantes` (WPF, .NET 10) y `DatosIniciales.cs` sin modificar |
| Clases | `ObjetoObservable`, `Estudiante`, `EstudiantesViewModel` (espacio de nombres `Estudiantes`) y `Controles/IndicadorNota` |
| Miembros públicos | Los de los requisitos 2 a 4 y 6, con esos nombres y tipos (`Vista` es `ICollectionView`; `Media`, `double`; `Aprobados` y `Total`, `int`) |
| Controles con nombre | `txtFiltro`, `txtNombre`, `txtEdad`, `sldNota`, `cmbCurso` |
| Recursos | El estilo `IndicadorSegunNota` |
| *Code-behind* | `MainWindow.xaml.cs` con el constructor y `btnNuevo_Click` y `btnEliminar_Click`, que llaman a `Nuevo()` y `Eliminar()` |
| Secciones del README | `## Capturas`, `## Enlaces`, `## Avisos de cambios`, `## El control IndicadorNota`, `## Pruebas realizadas` |

## Comprueba tu solución

Hazlas en orden: cada una parte de la anterior.

1. Al arrancar: tres grupos, `1.º DAM (3)`, `1.º DAW (3)` y `2.º DAM (4)`, con los nombres en orden; `Media: 6,15 · Aprobados: 7 de 10`; las barras de Bruno, Fernando e Irene en rojo; la ficha oculta.
2. Escribe `an` en el buscador: solo quedan Ana López (`1.º DAM (1)`) y Fernando Soto (`2.º DAM (1)`). Bórralo.
3. Elige a Bruno Díaz y lleva la nota a 5 con el deslizador: su barra (en la lista y en la ficha) se pone verde, dice «Aprobado» y el resumen pasa a `Media: 6,25 · Aprobados: 8 de 10`.
4. Escribe `7,5` en el cuadro de la nota y pulsa `Tab`: el deslizador va a 7,5 y la media a `6,50`.
5. Cambia el curso de Bruno a `2.º DAW`: aparece al final el grupo `2.º DAW (1)` y el de `1.º DAM` pasa a `(2)`.
6. Elige a Carla Ruiz y cambia su nombre a `Alba Ruiz`: mientras escribes, pasa a ser la primera de su grupo.
7. Escribe `veinte` en la edad y pulsa `Tab`: el cuadro se marca en rojo y la edad no cambia.
8. Pulsa **Nuevo**: aparece «Nuevo estudiante» en `1.º DAM`, elegido y con la ficha a la vista; el resumen pasa a `Media: 5,91 · Aprobados: 8 de 11`.
9. Pulsa **Eliminar** y confirma: desaparece, la ficha se oculta y vuelve el aviso; el resumen vuelve a `de 10`.
10. Cuenta las líneas de `MainWindow.xaml.cs`: ¿cuántas cambian algo de la pantalla?

> **Desafío (opcional):** añade una casilla «Solo suspensos» que filtre también por `Aprobado`, sin tocar el *code-behind*.

## Criterios de evaluación

- ✅ El modelo avisa de todos sus cambios, también de las propiedades calculadas
- ✅ El *ViewModel* filtra, agrupa y ordena con una vista de la colección y mantiene el resumen al día
- ✅ La ventana se enlaza con los modos y los momentos de actualización adecuados, con plantillas y cabeceras de grupo
- ✅ Un control propio con propiedades de dependencia que se enlazan desde fuera
- ✅ Los cambios de aspecto según los datos se hacen con estilos y `DataTrigger`
- ✅ El *code-behind* solo asigna el contexto de datos y atiende los botones
- ✅ El README explica los enlaces y los avisos, y el historial de commits es progresivo
