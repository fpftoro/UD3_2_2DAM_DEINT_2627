# Proyecto de la UD3: <Nombre de tu aplicación>

<!-- Plantilla de entrega del proyecto de la UD3 . Completa cada sección y borra los comentarios como este cuando termines.
     Las capturas van en la carpeta capturas/ y se enlazan así: ![Socios](capturas/socios.png) -->

## Dominio y cambios respecto al diseño

<!-- 1. Una frase sobre tu aplicación y sus dos entidades.
     2. Qué has cambiado respecto al prototipo de la UD1 y por qué.
     3. Las entidades de tu modelo, su relación y las reglas que has programado. -->

## Planificación

<!-- 1. El enlace a tu tablero de Trello (público o compartido con el profesor).
     2. Una o dos frases: cómo te has organizado y qué te costó más de lo previsto.
     3. Tabla de ramas: una fila por rama feat/… con su tarjeta de Trello y el número de su pull request. -->

| Rama | Tarjeta de Trello | Pull request | Parte (eventos, MVVM o común) |
|------|-------------------|--------------|-------------------------------|
| `feat/` | | # | |

## Parte con eventos

<!-- 0. Qué entidad has programado con eventos y cuál con MVVM, y por qué las has repartido así.
     1. Qué resuelves con eventos (la ventana principal y la vista de la entidad con eventos) y en qué ficheros.
     2. Un fragmento de un manejador de eventos (en un bloque ```csharp): qué lee, a quién llama y qué muestra.
     3. Compara el alta de la entidad con eventos con el alta de la entidad con MVVM: qué cambia
        y qué problemas del code-behind (tema 8) ves en tu propia ventana.
     4. Qué generó el editor visual al empezar la ventana y qué cambiaste después a mano. -->

## Arquitectura

<!-- 1. Un diagrama de los proyectos (Mermaid) y qué contiene cada uno.
     2. Tabla de ViewModels: qué vista dibuja cada uno, qué comandos tiene y qué servicios recibe.
     3. Cómo se navega entre vistas y cómo recibe cada vista su ViewModel o su repositorio. -->

| ViewModel | Vista | Comandos | Recibe |
|-----------|-------|----------|--------|
| | | | |

## Base de datos

<!-- 1. El modelo de datos como diagrama erDiagram de Mermaid.
     2. Tabla con cada consulta del repositorio: qué hace, el SQL y si lleva JOIN o transacción.
     3. Dónde está el .db, cómo se crea el esquema y cómo se tratan los errores. -->

| Método | Qué hace | SQL | JOIN / transacción |
|--------|----------|-----|--------------------|
| | | | |

## Inicio de sesión

<!-- 1. Usuario y contraseña de prueba que traen los datos de ejemplo.
     2. Cómo se guarda la contraseña (tabla Usuarios, sal, hash, algoritmo e iteraciones).
     3. Cómo llega la contraseña del PasswordBox al ViewModel (y por qué lo has hecho así: explícalo también
        en «Decisiones técnicas»). -->

| Usuario de prueba | Contraseña de prueba |
|-------------------|----------------------|
| | |

## Interfaz

<!-- 1. Captura de cada vista junto a su frame del prototipo.
     2. Tabla con los recursos del UI Kit (colores, tipografía, espaciado) y su clave en Recursos/.
     3. Tabla de controles: dónde usas cada control de la lista del enunciado.
     4. Accesibilidad: teclas de acceso, atajos y controles con AutomationProperties.Name. -->

| Elemento del UI Kit | Valor | Recurso |
|---------------------|-------|---------|
| | | |

| Control | Vista | Para qué |
|---------|-------|----------|
| `Grid` | | |
| `TextBlock` o `Label` | | |
| `TextBox` | | |
| `PasswordBox` | | |
| `Button` | | |
| `CheckBox` | | |
| `RadioButton` | | |
| `ListBox` | | |
| `ComboBox` | | |
| `DataGrid` o `ListView` | | |
| `Menu` | | |
| `MessageBox` (servicio de diálogos) | | |

## Decisiones técnicas

<!-- Las decisiones que has tomado y por qué: comandos a mano o con el CommunityToolkit, navegación con
     DataTemplate o ventanas, cómo une el modelo con la base de datos, qué va en una transacción… -->

## Pruebas manuales

<!-- Lista de comprobación ejecutada sobre la versión entregada: una fila por operación del user flow, el inicio
     de sesión (correcto y con una contraseña equivocada) y cada caso de error (validación, regla del modelo,
     base de datos). -->

| # | Qué hago | Qué espero | Resultado |
|---|----------|------------|-----------|
| 1 | | | |

## Cómo ejecutarlo

<!-- Proyecto de inicio, qué hace falta, cómo volver a los datos de ejemplo. -->
