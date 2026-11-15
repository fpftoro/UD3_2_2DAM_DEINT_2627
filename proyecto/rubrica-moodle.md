# Rúbrica del proyecto para Moodle

> Generado con `herramientas/rubrica-moodle.py` a partir de [`rubrica.md`](rubrica.md). Si cambia la rúbrica, se vuelve a generar; no se edita a mano.

Moodle no importa rúbricas desde un fichero, así que se crean a mano en el editor de rúbricas. Este documento tiene cada texto en un bloque con el botón de copiar de GitHub.

## Cómo crearla

1. En la tarea, en **Configuración > Calificación**, elige como método de calificación **Rúbrica** y pon **10** como calificación máxima.
2. En **Calificación avanzada**, elige **Definir un nuevo formulario de calificación desde cero**.
3. Copia el nombre y la descripción de la rúbrica.
4. Crea los 15 criterios (**Añadir criterio**). Cada uno tiene 4 niveles: Moodle crea 3 por defecto, así que añade uno más (**Añadir nivel**). Los niveles aparecen aquí en el orden en que Moodle los muestra por defecto, de menos a más puntos.
5. En las opciones de la rúbrica, deja marcadas las que muestran a los estudiantes la definición y los puntos de cada nivel y la que permite añadir comentarios por criterio.
6. Guarda la rúbrica y déjala **lista** para usarse. Después podrás reutilizarla en otras tareas eligiendo **Crear un nuevo formulario de calificación a partir de una plantilla** (entre tus propios formularios).

Moodle suma los puntos (150 como máximo) y los convierte en la calificación de 0 a 10 de forma proporcional.

Los requisitos mínimos van en la descripción de la rúbrica, pero Moodle no los aplica: si falta alguno, corrige la nota a mano en el libro de calificaciones.

## Nombre y descripción

**Nombre:**

```text
Proyecto UD3
```

**Descripción:**

```text
RA1. Genera interfaces gráficos de usuario mediante editores visuales utilizando las funcionalidades del editor y adaptando el código generado. 15 criterios, que cubren los CE a–h y uno sin CE, todos con el mismo peso. Niveles: Sobresaliente (10), Notable (8), Suficiente (6) e Insuficiente (0). Requisitos mínimos: la solución compila y la aplicación arranca; la aplicación tiene sus dos partes: la entidad con eventos y la entidad con MVVM, elegidas por el alumno; los datos se guardan en SQLite; la exposición del proyecto. Sin ellos, el proyecto se califica como insuficiente, sea cual sea la puntuación de los criterios.
```

## Criterios

### 1. Análisis de tecnologías y patrones (exposición) (CE a)

**Criterio:**

```text
1. Análisis de tecnologías y patrones (exposición) (CE a)
```

**Insuficiente · 0 puntos:**

```text
No sabe situar WPF entre otras tecnologías ni explicar por qué usa MVVM, o no hace la exposición.
```

**Suficiente · 6 puntos:**

```text
Nombra otras tecnologías y el patrón, pero no los compara ni justifica la elección, o solo repite las definiciones del tema.
```

**Notable · 8 puntos:**

```text
Compara WPF con otras tecnologías y justifica MVVM, pero los argumentos son generales o sin ejemplos de su aplicación.
```

**Sobresaliente · 10 puntos:**

```text
En la exposición compara WPF con al menos otras dos tecnologías de interfaces (WinForms, WinUI 3, MAUI, Avalonia, JavaFX, Flutter…) con argumentos propios (plataforma, lenguaje descriptivo, binding, madurez) y justifica por qué MVVM y no MVC o MVP, con ejemplos de su aplicación; dice en qué otro framework aplicaría lo aprendido.
```

### 2. La interfaz sigue el prototipo y parte del editor visual (CE b)

**Criterio:**

```text
2. La interfaz sigue el prototipo y parte del editor visual (CE b)
```

**Insuficiente · 0 puntos:**

```text
No hay capturas, o la interfaz no tiene relación con el prototipo de la UD1.
```

**Suficiente · 6 puntos:**

```text
Hay capturas solo de algunas vistas, varias diferencias con el prototipo sin explicar, o no se dice nada del editor visual.
```

**Notable · 8 puntos:**

```text
Están las capturas de todas las vistas y siguen el prototipo, pero alguna diferencia no se explica o lo que se cuenta del editor visual es muy breve.
```

**Sobresaliente · 10 puntos:**

```text
Cada vista tiene su captura junto a su frame del prototipo de la UD1, y se reconocen las mismas zonas, la misma navegación y los componentes del UI Kit; las diferencias (también la vista de inicio de sesión, si no estaba) se explican con su motivo; la sección «Parte con eventos» cuenta qué generó el editor visual al empezar la ventana y qué se cambió a mano.
```

### 3. Distribución con paneles (CE c)

**Criterio:**

```text
3. Distribución con paneles (CE c)
```

**Insuficiente · 0 puntos:**

```text
La mayor parte de la ventana usa posiciones absolutas (Canvas o márgenes del diseñador).
```

**Suficiente · 6 puntos:**

```text
Hay paneles, pero la ventana apenas se adapta: casi todo tiene tamaños fijos, o es un StackPanel sin ninguna zona que crezca.
```

**Notable · 8 puntos:**

```text
Falla un detalle: falta el tamaño mínimo, una zona tiene tamaño fijo y no crece, o queda algún margen de posición del diseñador.
```

**Sobresaliente · 10 puntos:**

```text
La ventana principal mide 1280 × 720, tiene MinWidth y MinHeight y se puede maximizar; las zonas del prototipo se reparten con Grid (filas y columnas Auto,  o proporcionales) y DockPanel o StackPanel; las listas y las tablas ocupan las filas o columnas  y crecen con la ventana; no hay Canvas, márgenes de posición del diseñador (Margin="120,85,0,0" con alineación Left y Top) ni tamaños fijos en los contenedores principales.
```

### 4. Controles, propiedades, recursos y estilos (CE d)

**Criterio:**

```text
4. Controles, propiedades, recursos y estilos (CE d)
```

**Insuficiente · 0 puntos:**

```text
No hay recursos (el aspecto se fija en cada control o es el de por defecto), o la aplicación usa muy pocos de los controles pedidos.
```

**Suficiente · 6 puntos:**

```text
Faltan varios controles de la lista; o hay recursos, pero falta una parte del UI Kit (la tipografía o el espaciado) o solo están en Window.Resources; o no hay estilos.
```

**Notable · 8 puntos:**

```text
Falta un control de la lista o uno está mal elegido para su dato, o algunos valores se repiten en varios controles, o la tabla del README no coincide del todo con el código.
```

**Sobresaliente · 10 puntos:**

```text
Usa todos los controles de la lista del enunciado, cada uno para el dato que le corresponde, y la tabla del README dice dónde; cada control tiene las propiedades que pide su dato (IsReadOnly y AutoGenerateColumns="False" en las tablas de consulta, DisplayMemberPath o ItemTemplate en las listas, StringFormat en los números y las fechas, GroupName en las opciones); los colores, la tipografía y el espaciado del UI Kit están en diccionarios de Recursos/ incorporados con MergedDictionaries, con estilos (BasedOn cuando uno amplía a otro), y las vistas no repiten valores en cada control; la tabla del UI Kit coincide con las claves de los recursos.
```

### 5. Accesibilidad (CE d)

**Criterio:**

```text
5. Accesibilidad (CE d)
```

**Insuficiente · 0 puntos:**

```text
No hay teclas de acceso ni nombres accesibles.
```

**Suficiente · 6 puntos:**

```text
Faltan teclas de acceso en varios campos, no hay atajos o faltan casi todos los nombres accesibles.
```

**Notable · 8 puntos:**

```text
Falla un detalle: alguna letra repetida en una vista, algún campo sin tecla de acceso o algún control sin nombre accesible.
```

**Sobresaliente · 10 puntos:**

```text
Los campos tienen un Label con _ y Target; los menús y los botones tienen tecla de acceso, sin letras repetidas en la misma vista; hay atajos de teclado (KeyBinding) para la navegación o las acciones principales, que se muestran en el menú (InputGestureText) o en el ToolTip; las listas, las tablas, el PasswordBox y los botones sin texto tienen AutomationProperties.Name; ningún TabIndex rompe el orden visual; el README lo recoge.
```

### 6. Análisis del código generado por el editor visual (exposición) (CE e)

**Criterio:**

```text
6. Análisis del código generado por el editor visual (exposición) (CE e)
```

**Insuficiente · 0 puntos:**

```text
No sabe explicar el XAML de su propia aplicación ni qué cambió, o no hace la exposición.
```

**Suficiente · 6 puntos:**

```text
Explica solo partes del código generado, o sabe qué hace cada parte pero no por qué la cambió.
```

**Notable · 8 puntos:**

```text
Explica el código generado y lo que cambió, pero con alguna imprecisión o necesitando alguna pista.
```

**Sobresaliente · 10 puntos:**

```text
En la exposición enseña el XAML que generó el diseñador al empezar la ventana (márgenes absolutos, tamaños fijos, nombres por defecto…), explica qué significa cada parte y por qué lo cambió; explica la clase parcial y InitializeComponent, y responde con precisión a las preguntas del profesor sobre ese código.
```

### 7. Parte con eventos y code-behind (CE g)

**Criterio:**

```text
7. Parte con eventos y code-behind (CE g)
```

**Insuficiente · 0 puntos:**

```text
No hay ninguna parte con eventos (todo está hecho con MVVM) o las operaciones no funcionan.
```

**Suficiente · 6 puntos:**

```text
Falta una de las operaciones de la vista (por ejemplo, la baja o la validación) o hay SQL o reglas del modelo en algún manejador; o la vista usa el gestor en memoria y no la base de datos.
```

**Notable · 8 puntos:**

```text
Falta un elemento (el manejador en la sección, la comparación con MVVM) o los problemas son los del tema sin ejemplos propios.
```

**Sobresaliente · 10 puntos:**

```text
La ventana principal y la vista de la entidad con eventos resuelven sus operaciones con manejadores de eventos en los *.xaml.cs (btnGuardar_Click(object sender, RoutedEventArgs e) o similar): el menú decide qué vista se muestra y la vista de la entidad con eventos tiene listado, alta con validación y baja con confirmación; cada manejador lee los controles, llama al gestor o al repositorio y muestra el resultado o el error, sin SQL ni reglas del modelo en la ventana; la vista recibe el repositorio por su constructor, como interfaz; la sección «Parte con eventos» indica qué entidad se ha programado con eventos y cuál con MVVM y por qué, da un manejador, compara esta alta con la de la entidad con MVVM y explica al menos dos problemas del code-behind del tema 8 con ejemplos de su propia ventana.
```

### 8. Comandos con su condición (CE g)

**Criterio:**

```text
8. Comandos con su condición (CE g)
```

**Insuficiente · 0 puntos:**

```text
Todas las acciones siguen con manejadores Click.
```

**Suficiente · 6 puntos:**

```text
Solo parte de las acciones son comandos; el resto sigue con Click.
```

**Notable · 8 puntos:**

```text
Algún comando no tiene condición y la necesita, o su condición no se actualiza (un botón se queda activo o desactivado hasta que cambia otra cosa).
```

**Sobresaliente · 10 puntos:**

```text
En las vistas con MVVM, todas las acciones son comandos (un ICommand propio o [RelayCommand] del CommunityToolkit) enlazados con Command="{Binding …}" en los botones, los menús y los KeyBinding, con CommandParameter cuando hace falta; cada acción que no siempre se puede hacer tiene su condición (CanExecute) y se avisa cuando cambia lo que la condiciona (NotificarCambio(), NotifyCanExecuteChanged() o [NotifyCanExecuteChangedFor]).
```

### 9. Arquitectura MVVM (CE f)

**Criterio:**

```text
9. Arquitectura MVVM (CE f)
```

**Insuficiente · 0 puntos:**

```text
No hay modelo separado (las entidades están en el proyecto WPF), no hay ViewModels (las vistas con MVVM siguen con code-behind) o usan MessageBox y otros tipos de WPF de forma generalizada.
```

**Suficiente · 6 puntos:**

```text
El núcleo no usa WPF, pero los ViewModels crean sus servicios, o queda lógica de la aplicación en algún code-behind; o los ViewModels están en el proyecto WPF.
```

**Notable · 8 puntos:**

```text
Falla un detalle: el modelo tiene menos de tres reglas, un ViewModel crea una dependencia con new, un *.xaml.cs tiene una línea de lógica pequeña o la tabla no coincide con el código.
```

**Sobresaliente · 10 puntos:**

```text
El modelo está en <TuDominio>.Logica (net10.0, sin WPF), con las dos entidades relacionadas, un gestor y al menos tres reglas que lanzan excepciones propias; <TuDominio>.Nucleo es net10.0 sin UseWPF y no usa tipos de WPF (solo System.Windows.Input, el de ICommand); hay un ViewModel por cada vista con MVVM, con una base común de INotifyPropertyChanged; los ViewModels reciben sus servicios por el constructor, como interfaces (diálogos, datos y autenticación); App no tiene StartupUri y crea las dependencias en OnStartup (raíz de composición); los *.xaml.cs de las vistas con MVVM solo tienen InitializeComponent y lo puramente visual; la sección «Arquitectura» tiene el diagrama de proyectos, la tabla de ViewModels, que coincide con el código, y cómo se navega.
```

### 10. Enlace a datos, validación y convertidores (CE h)

**Criterio:**

```text
10. Enlace a datos, validación y convertidores (CE h)
```

**Insuficiente · 0 puntos:**

```text
No hay binding, o casi toda la vista se rellena desde el código.
```

**Suficiente · 6 puntos:**

```text
No hay INotifyDataErrorInfo (se valida con un mensaje o dentro del comando), o no hay ningún convertidor propio; o parte de la vista se rellena desde el código.
```

**Notable · 8 puntos:**

```text
Falla un detalle: el idioma no se fija, no hay estilo de error o algún dato no se actualiza en la vista cuando cambia.
```

**Sobresaliente · 10 puntos:**

```text
Los datos se muestran y se editan por binding (ItemsSource, SelectedItem, Text con UpdateSourceTrigger=PropertyChanged en los formularios, IsChecked en las casillas y las opciones, StringFormat o un convertidor en los números y las fechas); las listas son ObservableCollection<T> o se sustituyen avisando del cambio; los formularios implementan INotifyDataErrorInfo con las reglas del modelo y un estilo muestra el error junto al campo (Validation.HasError); hay al menos un IValueConverter propio usado en un binding; el idioma de los bindings se fija en App.
```

### 11. Persistencia en SQLite (CE h)

**Criterio:**

```text
11. Persistencia en SQLite (CE h)
```

**Insuficiente · 0 puntos:**

```text
No hay base de datos (los datos siguen en memoria o en un JSON), o algún dato se concatena en el SQL (riesgo de inyección).
```

**Suficiente · 6 puntos:**

```text
Faltan dos elementos o más (por ejemplo, no hay JOIN ni transacción).
```

**Notable · 8 puntos:**

```text
Falta un elemento: el PRAGMA, los comandos asociados a la transacción, el .db en el repositorio o parte de la tabla del README.
```

**Sobresaliente · 10 puntos:**

```text
Al menos dos tablas relacionadas con su clave ajena (REFERENCES) y PRAGMA foreign_keys = ON en cada conexión; el esquema se crea al arrancar (CREATE TABLE IF NOT EXISTS); todas las consultas usan parámetros, con una conexión con using por operación; una consulta con JOIN alimenta una vista; una transacción junta dos escrituras que tienen que ir juntas (BeginTransaction, comandos con Transaction y Commit); el repositorio implementa una interfaz del núcleo y lo usan las dos partes de la aplicación; el .db está en <TuDominio>.Wpf/Datos/, en el repositorio, con datos de ejemplo, se copia al compilar y su ruta se forma con AppContext.BaseDirectory; la sección «Base de datos» tiene el erDiagram y la tabla de consultas, que coincide con el código.
```

### 12. Inicio de sesión (CE h)

**Criterio:**

```text
12. Inicio de sesión (CE h)
```

**Insuficiente · 0 puntos:**

```text
No hay inicio de sesión, o la contraseña se guarda o se compara en claro.
```

**Suficiente · 6 puntos:**

```text
Hay inicio de sesión, pero el hash no lleva sal o es un hash simple (SHA-256 de la contraseña), o la comprobación está en el code-behind.
```

**Notable · 8 puntos:**

```text
Funciona y guarda el hash con sal, pero falla un detalle: pocas iteraciones, comparación con ==, el mensaje dice qué ha fallado o la decisión no está justificada.
```

**Sobresaliente · 10 puntos:**

```text
La tabla Usuarios guarda el nombre (único), la sal y el hash calculado con Rfc2898DeriveBytes.Pbkdf2 (sal aleatoria por usuario, SHA-256 y al menos 100 000 iteraciones), nunca la contraseña en claro; la comparación se hace con CryptographicOperations.FixedTimeEquals o equivalente; la vista de inicio de sesión se abre al arrancar, con el PasswordBox pwdContrasena, y pasa a la ventana principal solo con credenciales correctas; el mensaje de error no dice si ha fallado el usuario o la contraseña; la contraseña no aparece en mensajes, registros ni propiedades enlazadas de texto; la solución para llevarla al ViewModel está justificada en «Decisiones técnicas»; el README da el usuario de prueba.
```

### 13. Configuración y tratamiento de errores (CE h)

**Criterio:**

```text
13. Configuración y tratamiento de errores (CE h)
```

**Insuficiente · 0 puntos:**

```text
No hay configuración, o un error de la base de datos o de una regla del modelo cierra la aplicación.
```

**Suficiente · 6 puntos:**

```text
Una de las dos partes está a medias: la configuración no se aplica, o los errores se capturan con catch (Exception) en todas partes.
```

**Notable · 8 puntos:**

```text
Falla un detalle: la configuración no guarda la posición o el estado maximizado, o falta el tratamiento en App.
```

**Sobresaliente · 10 puntos:**

```text
El tamaño, la posición y si estaba maximizada se guardan al cerrar en un JSON de %AppData% y se aplican antes de mostrar la ventana, sin fallar si el fichero no existe o no se puede leer; los errores de SQLite se traducen a una excepción propia en un solo sitio del repositorio; los ViewModels capturan esa excepción y las de las reglas del modelo y las muestran con el servicio de diálogos (o en el campo), sin un catch (Exception) que lo trague todo; si la base de datos no se puede abrir, App avisa y cierra con Shutdown.
```

### 14. Documentación y pruebas (CE h)

**Criterio:**

```text
14. Documentación y pruebas (CE h)
```

**Insuficiente · 0 puntos:**

```text
No hay README o es la plantilla sin completar, o no hay pruebas unitarias nuevas.
```

**Suficiente · 6 puntos:**

```text
Varias secciones del README están vacías, o no hay tabla de pruebas manuales; o las pruebas unitarias son pocas (menos de cuatro), abren la ventana o la base de datos, o alguna falla.
```

**Notable · 8 puntos:**

```text
Falta una sección, la tabla de pruebas manuales no tiene casos de error, hay menos de ocho pruebas unitarias, solo se prueba un ViewModel o falta una de las cuatro clases.
```

**Sobresaliente · 10 puntos:**

```text
Las diez secciones del contrato están completas, sin los comentarios de la plantilla, con el enlace al tablero de Trello; «Decisiones técnicas» justifica al menos tres decisiones con su motivo; la tabla de pruebas manuales tiene una fila por operación del user flow, el inicio de sesión y al menos un caso de error de validación, uno de una regla del modelo y uno de la base de datos, con su resultado; <TuDominio>.Pruebas tiene al menos ocho pruebas unitarias nuevas, repartidas en al menos dos ViewModels (no hace falta probarlos todos), con repositorio y diálogos falsos y sin ventana ni base de datos, que cubren la disponibilidad y el efecto de un comando, la validación y el error del repositorio, con nombres que dicen qué, en qué situación y qué se espera, y todas pasan.
```

### 15. Flujo de trabajo con Git

**Criterio:**

```text
15. Flujo de trabajo con Git
```

**Insuficiente · 0 puntos:**

```text
Todo está en main sin ramas, o hay un solo commit con todo, o los mensajes no siguen ninguna convención.
```

**Suficiente · 6 puntos:**

```text
Hay ramas, pero pocas (dos o tres para todo el proyecto) o sin pull request; o muchos mensajes no siguen la convención o son vagos (cambios, wip); o el historial son pocos commits grandes.
```

**Notable · 8 puntos:**

```text
Falla un detalle: algún commit directo en main, alguna rama sin prefijo feat/ o fusionada con squash, algunos mensajes que no siguen la convención o usan un tipo que no corresponde, o la tabla de ramas no coincide.
```

**Sobresaliente · 10 puntos:**

```text
El primer commit de main crea la solución vacía y, después, main solo recibe fusiones; cada funcionalidad se ha hecho en su rama feat/<nombre-funcionalidad> (minúsculas y guiones), al menos seis, integradas con pull request y merge commit, de modo que sus commits están en el historial; todos los mensajes propios siguen la convención tipo(ámbito): descripción con el tipo adecuado (feat, fix, docs, style, refactor, test, chore) y una descripción concreta en español; hay al menos veinte commits propios de fechas distintas, y cada uno hace una sola cosa; la tabla de ramas del README coincide con el repositorio.
```
