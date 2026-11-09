# E07 · Lista de tareas con comandos

**Corrección:** autocorrección, con el profesor o con IA

Programa una lista de tareas **sin un solo manejador de `Click`**: cada acción es un **comando** (`ICommand`) del *ViewModel* que sabe cuándo se puede ejecutar, los botones se activan y desactivan solos, los atajos de teclado ejecutan los mismos comandos y la exportación es **asíncrona**, con progreso y cancelación, sin bloquear la ventana.

**CE:** RA1 f, g, h · **Dificultad:** ★★☆ · **Tiempo estimado:** 4 h · **Tema:** 10

- [Punto de partida](#punto-de-partida)
- [Requisitos](#requisitos)
- [Contrato de entrega](#contrato-de-entrega)
- [Comprueba tu solución](#comprueba-tu-solución)
- [Criterios de evaluación](#criterios-de-evaluación)

## Punto de partida

No hay código de partida. Crea el proyecto como en el tema 2 y copia en la carpeta `E07-Tareas` de tu repositorio el `README.md` de esta carpeta (plantilla de entrega). Puedes partir del `ObjetoObservable` del tema 9.

```text
┌───────────────────────────────────────────────────────┐
│ [Leer el tema 10                 ] [Media ▾] [Añadir] │
│ ┌───────────────────────────────────────────────────┐ │
│ │ ☑ Instalar Visual Studio 2026 (tachada)  Alta  [↑]│ │
│ │ ☐ Repasar el tema 9                      Media [↑]│ │
│ │ ☐ Hacer el ejercicio 7                   Alta  [↑]│ │
│ │ ☐ Preparar el examen                     Baja  [↑]│ │
│ └───────────────────────────────────────────────────┘ │
│ [Eliminar] [Quitar completadas] [Exportar] [Cancelar] │
│ Pendientes: 3   Exportando…              ▓▓▓▓▓░░░░░   │
└───────────────────────────────────────────────────────┘
```

## Requisitos

**Los comandos, escritos a mano** (carpeta `Comandos/`, como en el tema 10):

1. Solución `Tareas.slnx` con el proyecto `Tareas` (WPF, .NET 10).
2. `RelayCommand`: implementa `ICommand` con una `Action` y una `Func<bool>?` opcional para `CanExecute`, y un método público `NotificarCambio()` que lanza `CanExecuteChanged`.
3. `RelayCommand<T>` (en el fichero `RelayCommand{T}.cs`): igual, pero con una `Action<T>` y una `Func<T, bool>?` que reciben el `CommandParameter`. Si el parámetro no es de tipo `T`, no se puede ejecutar.
4. `ComandoAsincrono`: recibe una `Func<Task>`, la ejecuta con `await` y no se puede volver a ejecutar mientras está en marcha.

**El modelo y el *ViewModel*:**

5. `Prioridad`: un `enum` con `Alta`, `Media` y `Baja`, en ese orden. `Tarea : ObjetoObservable` con `Titulo`, `Prioridad` y `Completada`.
6. `TareasViewModel : ObjetoObservable` empieza con cuatro tareas: «Instalar Visual Studio 2026» (alta, completada), «Repasar el tema 9» (media), «Hacer el ejercicio 7» (alta) y «Preparar el examen» (baja). Tiene `Tareas` (`ObservableCollection<Tarea>`), `Prioridades`, `NuevoTitulo`, `NuevaPrioridad` (media por defecto), `Seleccionada`, `Pendientes`, `Estado` y `Progreso`.
7. Sus comandos, cada uno con su condición:

   | Comando | Qué hace | Se puede ejecutar si |
   |---------|----------|----------------------|
   | `AnadirCommand` | Añade la tarea con el título sin espacios en los extremos y la prioridad elegida; vacía el título y vuelve a media. Si ya hay una con ese título (sin distinguir mayúsculas), no la añade y lo dice en `Estado` | El título no está vacío |
   | `EliminarCommand` | Elimina la elegida, tras confirmarlo | Hay una elegida |
   | `SubirPrioridadCommand` (`RelayCommand<Tarea>`) | Sube un nivel la prioridad de la tarea que recibe (baja → media → alta) | La tarea no es de prioridad alta |
   | `LimpiarCompletadasCommand` | Quita todas las completadas, tras confirmarlo | Hay alguna completada |
   | `ExportarCommand` (`ComandoAsincrono`) | Escribe `Documentos\tareas.csv` (cabecera `Tarea;Prioridad;Completada` y una línea por tarea, con `sí` o `no`), esperando 400 ms por tarea para simular un trabajo lento, con `Progreso` de 0 a 100 | Hay tareas y no se está exportando |
   | `CancelarExportacionCommand` | Cancela la exportación (`CancellationTokenSource`) | Se está exportando |

8. Cada vez que cambia algo de lo que depende una condición (el título, la elegida, la colección, el estado o la prioridad de una tarea, el inicio y el fin de la exportación), el *ViewModel* llama a `NotificarCambio()` del comando afectado, y `Pendientes` avisa de su cambio.
9. La exportación informa en `Estado` de cómo ha terminado: con la ruta del fichero, `Exportación cancelada` o el error de escritura (capturado). Al terminar, `Progreso` vuelve a 0.
10. Para confirmar, el *ViewModel* tiene una propiedad `Confirmar` de tipo `Func<string, bool>` que pone la ventana (con un `MessageBox`). El *ViewModel* no usa `MessageBox` ni ninguna otra clase de ventanas.

**La ventana:**

11. Ningún botón tiene `Click`: todos usan `Command="{Binding …}"`. El *code-behind* solo crea el *ViewModel* con su `Confirmar` y lo asigna como `DataContext`.
12. Atajos con `KeyBinding`: `Intro` en el cuadro del título añade; `Supr` en la lista elimina; `Ctrl+E` exporta y `Esc` cancela (en toda la ventana).
13. El cuadro del título actualiza `NuevoTitulo` con cada letra, para que **Añadir** se active en cuanto se escribe.
14. La plantilla de cada tarea tiene una casilla enlazada a `Completada`, el título (tachado y en gris si está completada), la prioridad (en rojo y negrita si es alta) y un botón `↑` con el `SubirPrioridadCommand` del *ViewModel* (búscalo con `RelativeSource`) y la tarea como `CommandParameter`.
15. Abajo: `Pendientes: n`, el `Estado` y una `ProgressBar` enlazada a `Progreso`. El foco empieza en el cuadro del título.

**Entrega:**

16. Completa el `README.md` y haz commits a medida que avances.

## Contrato de entrega

| Elemento | Debe ser |
|----------|----------|
| Solución y proyecto | `E07-Tareas/Tareas.slnx` con el proyecto `Tareas` (WPF, .NET 10) |
| Clases | `Comandos/RelayCommand`, `Comandos/RelayCommand<T>`, `Comandos/ComandoAsincrono`, `ObjetoObservable`, `Prioridad`, `Tarea` y `TareasViewModel` |
| Miembros públicos | Los de los requisitos 2 a 7 y 10, con esos nombres; los comandos, como propiedades de solo lectura de su tipo (`RelayCommand`, `RelayCommand<Tarea>`, `ComandoAsincrono`) |
| Controles con nombre | `txtNuevaTarea` (el cuadro del título); el resto no necesita nombre |
| *Code-behind* | Solo el constructor de `MainWindow` |
| Fichero exportado | `tareas.csv` en Documentos |
| Secciones del README | `## Capturas`, `## Comandos`, `## Atajos`, `## Sin bloquear la ventana`, `## Pruebas realizadas` |

## Comprueba tu solución

1. Al arrancar: cuatro tareas, la primera tachada; `Pendientes: 3`; **Añadir** y **Eliminar** desactivados; **Quitar completadas** activo; el cursor en el cuadro del título.
2. Escribe `Leer el tema 10`: **Añadir** se activa con la primera letra. Pulsa `Intro`: la tarea aparece con prioridad media, el cuadro se vacía y `Pendientes: 4`.
3. Escribe `repasar el TEMA 9` y pulsa `Intro`: no se añade y el estado dice `Ya existe la tarea «repasar el TEMA 9»`.
4. Pulsa `↑` en «Preparar el examen» dos veces: pasa a media y a alta (en rojo), y su `↑` se desactiva. El `↑` de «Instalar Visual Studio 2026» está desactivado desde el principio.
5. Marca «Repasar el tema 9»: se tacha y `Pendientes: 3`. **Quitar completadas** pregunta `¿Quitar las 2 tareas completadas?`; con **No** no pasa nada; con **Sí** desaparecen y el botón se desactiva.
6. Elige una tarea y pulsa `Supr`: pregunta si la eliminas. Con **Sí** desaparece y **Eliminar** se desactiva.
7. Pulsa `Ctrl+E`: la barra avanza y, mientras, puedes escribir en el cuadro y mover la ventana; **Exportar** está desactivado y **Cancelar** activo. Pulsa `Esc`: `Exportación cancelada` y la barra vuelve a cero.
8. Exporta hasta el final: el estado da la ruta de `tareas.csv`; ábrelo y comprueba la cabecera y una línea por tarea.
9. Busca `Click=` en `MainWindow.xaml` y cuenta las líneas de `MainWindow.xaml.cs`.

> **Desafío (opcional):** añade un `EditarCommand` que cargue el título de la elegida en el cuadro y cambie **Añadir** por **Guardar** mientras se edita.

## Criterios de evaluación

- ✅ `RelayCommand`, `RelayCommand<T>` y `ComandoAsincrono` implementan `ICommand` correctamente
- ✅ Cada comando tiene su condición y avisa cuando esta puede haber cambiado
- ✅ La vista usa los comandos en botones, atajos y plantillas, sin manejadores de `Click`
- ✅ La exportación es asíncrona, informa del progreso y se puede cancelar
- ✅ El modelo y la ventana se enlazan bien, con el aspecto según los datos
- ✅ El *ViewModel* pide confirmación sin depender de las ventanas
- ✅ El README explica los comandos y la asincronía, y el historial de commits es progresivo
