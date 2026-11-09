# E05 · Visor de imágenes

**Corrección:** autocorrección, con el profesor o con IA

Construye un visor de imágenes de escritorio completo: menús con atajos, barra de herramientas y barra de estado, diálogos, zoom con el teclado y la rueda, estilos en un **diccionario de recursos**, **tema claro y oscuro** y una configuración del usuario que se recupera al volver a abrir.

**CE:** RA1 d, f, g, h · **Dificultad:** ★★★ · **Tiempo estimado:** 5 h · **Temas:** 5, 6 y 7

- [Punto de partida](#punto-de-partida)
- [Requisitos](#requisitos)
- [Contrato de entrega](#contrato-de-entrega)
- [Comprueba tu solución](#comprueba-tu-solución)
- [Criterios de evaluación](#criterios-de-evaluación)

## Punto de partida

Crea el proyecto como en el tema 2 y copia en la carpeta `E05-VisorImagenes` de tu repositorio el `README.md` de esta carpeta (plantilla de entrega). La carpeta `muestras/` trae imágenes para probar el visor: `paisaje.png` (1200 × 800), `cuadricula.png` (400 × 400, con cuadros de 50 px), `retrato.png` (600 × 900) y `danada.png`, que **no** es una imagen. No las añadas al proyecto: se abren con el diálogo. La carpeta `icono/` trae `visor.ico`, el icono de la aplicación (16, 32, 48 y 256 píxeles), que **sí** se añade al proyecto.

```text
┌──────────────────────────────────────────────────────────────┐
│ Archivo  Ver  Ayuda                                          │
│ [Abrir] │ [◀] [▶] │ [−] [+] [1:1] [Ajustar]                  │
├──────────────┬───────────────────────────────────────────────┤
│ paisaje.png  ┃                                               │
│ cuadricula…  ┃                (la imagen)                    │
│ retrato.png  ┃                                               │
├──────────────┴───────────────────────────────────────────────┤
│ cuadricula.png (2 de 3) │ 400 × 400 px            125 %      │
└──────────────────────────────────────────────────────────────┘
```

## Requisitos

**La ventana:**

1. Solución `VisorImagenes.slnx` con el proyecto `VisorImagenes` (WPF, .NET 10). Ventana con un tamaño mínimo de 640 × 420 cuyo título es `Visor de imágenes` o, con una imagen abierta, `<nombre> · Visor de imágenes`. El icono `visor.ico`, en la carpeta `Imagenes/` del proyecto como *Recurso*, es el de la ventana (`Icon`) y el del ejecutable (`ApplicationIcon`, tema 5, apartado 5.7).
2. Un `DockPanel` con el menú y la barra de herramientas arriba, la barra de estado abajo y, en el resto, un `Grid` de tres columnas: la lista de imágenes `lstImagenes`, un `GridSplitter` `splLista` para cambiar su ancho y un `ScrollViewer` `scvImagen` con la imagen `imgVista`. Sobre la imagen, `txtMensaje` muestra un texto centrado cuando no hay imagen o no se puede abrir.

**Menús, barra de herramientas y barra de estado:**

3. Menú **Archivo**: *Abrir imágenes…* (con el comando predefinido `ApplicationCommands.Open`, que ya trae `Ctrl+O`), *Cerrar todas* (`mnuCerrarTodas`) y *Salir*. Menú **Ver**: *Imagen anterior* y *siguiente* (`mnuAnterior`, `mnuSiguiente`), *Acercar*, *Alejar*, *Tamaño real* y *Ajustar a la ventana* (`mnuAcercar`, `mnuAlejar`, `mnuTamanoReal`, `mnuAjustar`), el submenú *Tema* con *Claro*, *Oscuro* y *Del sistema* (`mnuTemaClaro`, `mnuTemaOscuro`, `mnuTemaSistema`, marcables) y *Mostrar la lista* (`mnuMostrarLista`, marcable). Menú **Ayuda**: *Acerca de…*. Todas las opciones tienen tecla de acceso y las que tienen atajo lo muestran (`InputGestureText`).
4. La barra de herramientas repite *Abrir* (con el mismo comando) y, con **el mismo manejador que su opción del menú**, anterior, siguiente, alejar, acercar, tamaño real y ajustar (`btnAnterior`, `btnSiguiente`, `btnAlejar`, `btnAcercar`, `btnTamanoReal`, `btnAjustar`). Cada botón tiene un `ToolTip` con su atajo y, si muestra un símbolo, `AutomationProperties.Name`.
5. La barra de estado muestra el nombre de la imagen y su posición (`cuadricula.png (2 de 3)`) en `txtEstadoNombre`, su tamaño en píxeles en `txtEstadoTamano` y, a la derecha, el zoom (`125 %` o `Ajustada a la ventana`) en `txtEstadoZoom`.
6. Las opciones que no se pueden usar están **desactivadas**, en el menú y en la barra: sin imágenes, todas menos *Abrir* y *Salir*; en la primera imagen, *anterior*; en la última, *siguiente*.

**Abrir y ver:**

7. *Abrir* usa un `OpenFileDialog` con el filtro `Imágenes` (`*.png;*.jpg;*.jpeg;*.bmp;*.gif`) y `Todos los archivos`, que permite **elegir varias** y empieza en la carpeta de la última apertura (o en Imágenes). Las elegidas sustituyen a la lista y se muestra la primera.
8. Al elegir una imagen en la lista, se carga con un `BitmapImage` con `CacheOption = BitmapCacheOption.OnLoad` (así el fichero no queda bloqueado). Si no se puede abrir, `txtMensaje` dice cuál y por qué, sin `MessageBox` y sin que la aplicación se cierre (captura las excepciones concretas).
9. **Zoom:** *Ajustar* hace que la imagen ocupe el espacio visible sin barras de desplazamiento; *Tamaño real* la muestra a su tamaño en píxeles; *Acercar* y *Alejar* multiplican o dividen el zoom por 1,25, entre el 10 % y el 800 %, con barras de desplazamiento cuando no cabe. Al empezar, la imagen está ajustada.
10. **Teclado y ratón**, en el `PreviewKeyDown` de la ventana: `Ctrl++` y `Ctrl+-` (también los del teclado numérico), `Ctrl+0`, `Ctrl+1`, y `←` y `→` para la imagen anterior y la siguiente (salvo dentro de un menú abierto). `Ctrl` + rueda sobre la imagen acerca y aleja (`PreviewMouseWheel`); la rueda sola desplaza.
11. *Mostrar la lista* oculta y muestra la lista y el separador, y la lista recupera su ancho.
12. *Acerca de…* abre una ventana **modal** `VentanaAcercaDe` (en la carpeta `Ventanas/`), centrada sobre la principal, que se cierra con su botón, `Intro` o `Esc` sin escribir código.

**Estilos y tema:**

13. Un diccionario de recursos `Recursos/Estilos.xaml`, incorporado en `App.xaml`, con al menos: un estilo `BotonBarra` para los botones de la barra (con un desencadenador que los atenúa cuando están desactivados y la ayuda visible aunque lo estén), un estilo `TextoEstado` para los textos de la barra de estado y un estilo `MensajeCentral` para `txtMensaje` cuyo color de texto es un `DynamicResource` (cambia con el tema).
14. Tema **Fluent** con `ThemeMode` (en `App.xaml` y, al elegir en el menú, en `Application.Current.ThemeMode`). Solo una opción del submenú está marcada. Suprime el aviso `WPF0001` en el `.csproj`, como en el tema 7.

**Configuración del usuario:**

15. Un servicio `Servicios/ServicioConfiguracion` guarda y lee un objeto `Servicios/ConfiguracionVisor` en `%AppData%\VisorImagenes\configuracion.json`, con la posición y el tamaño de la ventana, si estaba maximizada, el tema, si se muestra la lista y la última carpeta. Se lee en el constructor de la ventana y se guarda en `Closing`. Un fichero que falta o está dañado no impide arrancar, y un error al guardar no impide cerrar.

**Entrega:**

16. Completa el `README.md` y haz commits a medida que avances.

## Contrato de entrega

| Elemento | Debe ser |
|----------|----------|
| Solución y proyecto | `E05-VisorImagenes/VisorImagenes.slnx` con el proyecto `VisorImagenes` (WPF, .NET 10) |
| Carpetas | `Recursos/Estilos.xaml`, `Servicios/ConfiguracionVisor.cs`, `Servicios/ServicioConfiguracion.cs`, `Ventanas/VentanaAcercaDe.xaml` |
| Estilos | `BotonBarra`, `TextoEstado` y `MensajeCentral` en `Recursos/Estilos.xaml` |
| Controles con nombre | Los de los requisitos 2 a 5 (`lstImagenes`, `splLista`, `scvImagen`, `imgVista`, `txtMensaje`, `mnu…`, `btn…`, `txtEstado…`) |
| Manejadores | `Abrir_Executed`, `Anterior_Click`, `Siguiente_Click`, `Acercar_Click`, `Alejar_Click`, `TamanoReal_Click` y `Ajustar_Click` (compartidos por el menú y la barra), `mnuTema_Click` (compartido por los tres temas), `Window_PreviewKeyDown`, `scvImagen_PreviewMouseWheel`, `Window_Closing` |
| Icono | `Imagenes/visor.ico` (recurso), en `Icon` de la ventana principal y en `ApplicationIcon` del proyecto |
| Configuración | `%AppData%\VisorImagenes\configuracion.json` |
| Secciones del README | `## Capturas`, `## Menús y barra de herramientas`, `## Recursos y estilos`, `## Configuración del usuario`, `## Pruebas realizadas` |

## Comprueba tu solución

1. Al arrancar sin imágenes: el mensaje del centro, «Sin imágenes» en la barra de estado y todas las opciones desactivadas menos *Abrir* y *Salir*. Pasa el ratón por un botón desactivado: se ve su ayuda.
2. `Ctrl+O`, elige las cuatro muestras: la lista tiene cuatro nombres y se ve la primera ajustada; el título y la barra de estado dicen su nombre, su posición y su tamaño.
3. Elige `danada.png` en la lista: el centro explica que no se puede abrir, las opciones de zoom se desactivan y la aplicación sigue funcionando. Recorre la lista con `→`: en la última imagen, *siguiente* está desactivado.
4. Con `cuadricula.png`, pulsa **1:1**: los cuadros miden 50 px y la barra dice `100 %`. Pulsa `Ctrl++` dos veces: `156 %` y aparecen barras de desplazamiento. `Ctrl` + rueda hacia atrás: aleja. La rueda sola desplaza la imagen.
5. Pulsa `Ctrl+-` muchas veces: el zoom no baja del `10 %`. `Ctrl+1`: vuelve a `Ajustada a la ventana`, sin barras.
6. *Ver > Tema > Oscuro*: la ventana cambia al tema oscuro, el mensaje del centro sigue leyéndose y solo *Oscuro* está marcado.
7. Desmarca *Mostrar la lista*: la imagen ocupa todo el ancho. Vuelve a marcarla: la lista recupera su ancho.
8. *Ayuda > Acerca de…*: no puedes usar la ventana principal hasta cerrarlo; `Esc` lo cierra.
9. Mueve la ventana, cambia su tamaño, deja el tema oscuro y la lista oculta, y ciérrala. Abre `%AppData%\VisorImagenes\configuracion.json`: están esos valores. Vuelve a ejecutar: la ventana aparece igual. Maximízala, ciérrala y vuelve a abrir: sale maximizada, y al restaurarla recupera el tamaño anterior.
10. *Abrir* empieza en la carpeta de las muestras. Borra el JSON, escribe en él algo que no sea JSON y ejecuta: arranca con los valores por defecto.
11. La barra de título, la barra de tareas y `Alt+Tab` muestran el icono del visor, y el Explorador de archivos lo muestra en `VisorImagenes.exe` (en `bin/`).

> **Desafío (opcional):** guarda también el zoom y la última imagen abierta, y ábrela al arrancar si sigue existiendo.

## Criterios de evaluación

- ✅ Menús, barra de herramientas y barra de estado completos, con atajos, ayudas y opciones desactivadas cuando no se pueden usar
- ✅ El diálogo de abrir y la ventana modal se configuran y se usan correctamente
- ✅ El zoom y la navegación funcionan con el menú, la barra, el teclado y la rueda
- ✅ Una imagen que no se puede abrir se explica sin cerrar la aplicación
- ✅ Los estilos están en un diccionario de recursos, el color que depende del tema es dinámico y la aplicación tiene su icono
- ✅ El tema claro, oscuro o del sistema se elige en el menú
- ✅ La configuración del usuario se guarda al cerrar y se recupera al abrir
- ✅ El README está completo y el historial de commits es progresivo
