# E03 · Inscripción en la escuela de música

**Corrección:** autocorrección, con el profesor o con IA

Construye el formulario de inscripción de una escuela de música eligiendo **el control adecuado para cada dato** y configurando sus propiedades: texto, fecha, una lista con imágenes, opciones excluyentes, casillas con una casilla «Todas» de tres estados, un texto largo con corrector ortográfico y contraseñas. Con teclas de acceso, ayudas y nombres para los lectores de pantalla.

**CE:** RA1 c, d, h · **Dificultad:** ★★☆ · **Tiempo estimado:** 3 h · **Temas:** 4 y 5

- [Punto de partida](#punto-de-partida)
- [Requisitos](#requisitos)
- [Contrato de entrega](#contrato-de-entrega)
- [Comprueba tu solución](#comprueba-tu-solución)
- [Criterios de evaluación](#criterios-de-evaluación)

## Punto de partida

Crea el proyecto como en el tema 2 y copia en la carpeta `E03-Inscripcion` de tu repositorio el `README.md` de esta carpeta (plantilla de entrega). La carpeta `Imagenes/` trae los iconos de los cuatro instrumentos (`guitarra.png`, `piano.png`, `bateria.png` y `violin.png`): cópiala dentro del proyecto.

```text
┌ Datos del alumno ──────────────────────────────────┐
│ Nombre y apellidos:  [                           ] │
│ Fecha de nacimiento: [dd/mm/aaaa ▦]                 │
│ Teléfono:            [         ]                   │
└────────────────────────────────────────────────────┘
┌ Curso ─────────────────────────────────────────────┐
│ Instrumento: [(G) Guitarra      ▾]                 │
│ Nivel:       ○ Iniciación  ○ Medio  ○ Avanzado     │
│ Horario:     ○ Mañana  ○ Tarde                     │
└────────────────────────────────────────────────────┘
┌ Actividades extra (10 € al mes cada una) ──────────┐
│ ☐ Todas las actividades                            │
│     ☐ Coro  ☐ Lenguaje musical  ☐ Conciertos…      │
└────────────────────────────────────────────────────┘
┌ Observaciones ─────────────────────────────────────┐
│ [                                                ] │
│                                          0 / 500   │
└────────────────────────────────────────────────────┘
┌ Acceso al portal del alumno ───────────────────────┐
│ Usuario / Contraseña / Repetir contraseña          │
└────────────────────────────────────────────────────┘
                              [ Limpiar ] [Inscribirme]
```

## Requisitos

**Distribución:**

1. Solución `Inscripcion.slnx` con el proyecto `Inscripcion` (WPF, .NET 10). Ventana con el título `Inscripción · Escuela de música`, de 680 × 660 y un mínimo de 560 × 480.
2. Un `Grid` raíz con dos filas: el formulario (`*`), dentro de un `ScrollViewer` que se desplaza cuando la ventana es más baja que el contenido, y los botones (`Auto`), abajo a la derecha y siempre visibles.
3. El formulario tiene cinco `GroupBox` (los del esquema). Dentro de cada uno, un `Grid` de dos columnas (`Auto` para las etiquetas y `*` para los controles) o un `StackPanel` cuando no hay etiquetas.
4. El espaciado de cada tipo de control va en **estilos implícitos** en `Window.Resources` (sin `x:Key`), no en cada control.

**Controles y propiedades:**

5. **Datos del alumno:** `txtNombre` (como mucho 80 caracteres), `dtpNacimiento` (un `DatePicker` que no deja elegir fechas posteriores a hoy: `DisplayDateEnd`, en el constructor) y `txtTelefono` (estrecho, como mucho 9 caracteres y con la ayuda `Nueve cifras, sin espacios`).
6. **Instrumento:** `cmbInstrumento`, con un `ComboBoxItem` por instrumento cuyo contenido es un `StackPanel` horizontal con la imagen (24 × 24) y el nombre. Cada elemento guarda el nombre en `Tag` y tiene un `ToolTip` con una descripción. Las imágenes son **recursos** del proyecto (`Acción de compilación` = `Recurso`), no ficheros sueltos ni direcciones web.
7. **Nivel** (`rbIniciacion`, `rbMedio`, `rbAvanzado`) y **horario** (`rbManana`, `rbTarde`): `RadioButton` con `GroupName` (`Nivel` y `Horario`).
8. **Actividades extra:** `chkTodas` y, con sangría, `chkCoro`, `chkLenguaje` y `chkConciertos`. Al marcar o desmarcar `chkTodas`, las tres hacen lo mismo; al cambiar una de las tres, `chkTodas` queda marcada (las tres), desmarcada (ninguna) o **indeterminada** (algunas). El estado indeterminado lo pone el código: usa el evento `Click`, como en el tema 5.
9. **Observaciones:** `txtObservaciones`, de varias líneas (acepta `Intro`, ajusta las líneas y muestra la barra de desplazamiento solo cuando hace falta), como mucho 500 caracteres y con el **corrector ortográfico en español**. Debajo, `txtContador` muestra `n / 500` mientras se escribe y, si hay texto seleccionado, también cuántos caracteres (`120 / 500 · 14 seleccionados`), con los eventos `TextChanged` y `SelectionChanged` y la propiedad `SelectedText` de la caja.
10. **Acceso al portal:** `txtUsuario`, `pwdContrasena` y `pwdRepetir` (`PasswordBox` de 20 caracteres como mucho; la primera, con la ayuda `Entre 8 y 20 caracteres`).
11. **Botones:** `btnLimpiar` y `btnInscribir`, que es el botón por defecto (`Intro`).

**Accesibilidad:**

12. Cada campo tiene un `Label` con **tecla de acceso** (`_Nombre y apellidos:`) y `Target`, sin repetir letra dentro de la ventana. Los botones también tienen tecla de acceso.
13. El orden de tabulación sigue el orden visual sin usar `TabIndex` (basta con el orden del XAML).
14. Los controles cuyo contenido no es texto (la lista con imágenes, las contraseñas y las observaciones) tienen `AutomationProperties.Name`.

**Funcionamiento:**

15. **Inscribirme** comprueba todos los datos: nombre, fecha (al menos 6 años), teléfono de 9 cifras, instrumento, nivel, horario, usuario, contraseña de al menos 8 caracteres y repetición igual. Si algo falla, un `MessageBox` de aviso los lista **todos** y el foco va al primer campo con error. Si todo está bien, un `MessageBox` muestra el resumen (con la cuota: 40 € más 10 € por actividad) y **nunca la contraseña**.
16. **Limpiar** deja el formulario vacío y el foco en el nombre.

**Entrega:**

17. Completa el `README.md` (capturas, controles y propiedades, accesibilidad y pruebas realizadas) y haz commits a medida que avances.

## Contrato de entrega

| Elemento | Debe ser |
|----------|----------|
| Solución y proyecto | `E03-Inscripcion/Inscripcion.slnx` con el proyecto `Inscripcion` (WPF, .NET 10) |
| Imágenes | `Inscripcion/Imagenes/*.png`, con `Acción de compilación` = `Recurso` |
| Controles con nombre | `txtNombre`, `dtpNacimiento`, `txtTelefono`, `cmbInstrumento`, `rbIniciacion`, `rbMedio`, `rbAvanzado`, `rbManana`, `rbTarde`, `chkTodas`, `chkCoro`, `chkLenguaje`, `chkConciertos`, `txtObservaciones`, `txtContador`, `txtUsuario`, `pwdContrasena`, `pwdRepetir`, `btnLimpiar`, `btnInscribir` |
| Manejadores | `chkTodas_Click`, `chkExtra_Click` (compartido por las tres actividades), `txtObservaciones_TextChanged`, `txtObservaciones_SelectionChanged`, `btnInscribir_Click`, `btnLimpiar_Click` |
| Secciones del README | `## Capturas`, `## Controles y propiedades`, `## Accesibilidad`, `## Pruebas realizadas` |

## Comprueba tu solución

1. Pulsa **Inscribirme** con el formulario vacío: un único mensaje lista los ocho errores (la repetición de la contraseña no se comprueba mientras la contraseña sea demasiado corta) y, al cerrarlo, el cursor está en el nombre.
2. Rellena todo bien con Piano, nivel medio, tarde, Coro y Conciertos: el resumen dice `Cuota mensual: 60 €` y no muestra la contraseña.
3. Escribe dos contraseñas distintas de 8 caracteres: el único error es que no coinciden y el foco va a «Repetir contraseña».
4. Marca **Todas**: se marcan las tres. Desmarca **Coro**: **Todas** queda indeterminada. Desmarca las otras dos: **Todas** queda desmarcada.
5. Abre el desplegable de instrumentos: cada uno con su icono; al pasar el ratón, su descripción.
6. Elige **Medio** y después **Tarde**: el nivel sigue elegido (son grupos distintos).
7. En el calendario de la fecha de nacimiento, los días posteriores a hoy no se pueden elegir.
8. Escribe `instrumeto` en observaciones: aparece subrayada en rojo. El contador cuenta los caracteres. Selecciona una palabra con doble clic: el contador añade `· 10 seleccionados`; haz clic en otro sitio y desaparece.
9. Pulsa `Alt+N`, `Alt+T` y `Alt+I`: el foco salta al nombre, al teléfono y al instrumento. Recorre la ventana con `Tab`: el orden es el visual.
10. Encoge la ventana: aparece la barra de desplazamiento y los botones siguen visibles.
11. Pulsa **Limpiar**: todo vacío (también las contraseñas) y el cursor en el nombre.

> **Desafío (opcional):** marca en rojo el borde de los campos con error (`BorderBrush`) y quítalo cuando se corrigen.

## Criterios de evaluación

- ✅ La ventana se distribuye con un `Grid`, `GroupBox` y un `ScrollViewer`, con los botones siempre visibles
- ✅ Cada dato tiene el control adecuado, con las propiedades que lo limitan y lo explican
- ✅ La lista de instrumentos muestra imágenes incrustadas como recurso
- ✅ La casilla «Todas» refleja el estado de las tres actividades, también el indeterminado
- ✅ Los campos tienen teclas de acceso, un orden de tabulación lógico y nombres accesibles
- ✅ El formulario comprueba los datos, avisa de todos los errores a la vez y no muestra la contraseña
- ✅ El README justifica la elección de los controles y el historial de commits es progresivo
