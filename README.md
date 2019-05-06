# Proyecto

Nombre de la aplicación: CameraScanner.

Info:
Aplicación diseñada para capturar texto en tiempo real mediante el CameraSource (API de Google). 

Funcionamiento:

-El usuario enfoca a cualquier texto con la cámara trasera del teléfono e inmediatamente verá el texto que está recibiendo. Si quiere capturar ese texto solo tiene que pulsar el boton "Capturar texto" e inmediatamente de generará un documento de texto (.txt) con la información recogida.

-Para acceder a esos documentos tendrá que pulsar sobre el botón "Datos guardados" y accederá a la lista de todos los documentos realizados. 

-Si quiere abrir esos documentos tendrá que tocar sobre ellos y saldrá un menú donde seleccionará una app compatible para abrirlos (es necesario tener apps instaladas en el terminal compatibles con documentos.txt).

-Si quiere borrar algún documento de la lista solo tiene que hacer pulsación larga sobre el documento seleccionado y este se eliminará de la lista (Se le preguntará al usuario si desea borrarlo).

-Si quiere borrar toda la lista tiene que hacer click sobre el boton flotante con el simbolo de la papelera.(Se le preguntará al usuario si desea borrarla).

-Para volver a la activity principal tiene que pulsar el boton volver.

----------------------------------------------------------------------------------------------------------------------------------------
IMPORTANTE:

-Paso 1: Actualizar por completo visual Studio 2019 (Si tiene actualización anterior a la 16.0.3 , se producen errores con la gestión de memoria en Android 8.0 y la aplicación se cierra la primera vez que se ejecuta).

-Paso 2: Hay que importar en el android SDK manager "Google play services" (En la opcion extras). Luego, en Referencias del proyecto
hacemos clic derecho y agregamos "Xamarin.GooglePlayServices.Vision (V-60.1142.1)","Xamarin.Android.Support.v7.AppCompact (V-28.0.0.1)",
"Xamarin.Android.Support.v4 (V-28.0.0.1)" y  "Xamarin.Forms (V-3.6.0.344457)" en paquetes NuGet.


----------------------------------------------------------------------------------------------------------------------------------------


Primera versión: 

-Creación la clase principal (Mainactivity), donde realizará el funcionamiento principal de la API de Google para la detección de imagenes y la gestión de permisos necesarios para el funcionamiento de la APP (acceso a la camara, permisos de lectura/escritura, etc).



Segunda versión: 

-Creación de una segunda clase (llamada datos), donde se gestionan los archivos guardados mediante un ListView, con la posibilidad abrirlos con una pulsacion corta o de borrarlos mediante una pulsación larga.



Tercera versión:

-Se arreglan errores de funcionamiento.

-Implementación de boton flotante para borrado completo de la lista.

-Se agregan dialogos modales para preguntar al usuario si desea borrar los datos.



Cuarta versión (Final 1.0):

-Se agregan diferentes iconos a la app (cuadrado y círculo), para adaptarse a los distintos temas de los terminales.

-Adaptar la aplicación a los diferentes tipos de pantallas automaticamente (acepta tablets de hasta 10").



Quinta versión (Final 2.0) (Si da tiempo):

-Adaptar la aplicación para ser 100% compatible con android 9 Pie (Pequeños pantallazos al refrescar el ListView).

-Crear apk de la aplicación y subirla a la Play Store.



----------------------------------------------------------------------------------------------------------------------------------------
Ultimas actualizaciones:

*****La aplicación ya es completamente funcional.****

-El error producido en versiones anteriores ("F/libc (26772): Fatal signal 11 (SIGSEGV), code 1, fault addr 0xbc in tid 26872 (Thread-5)") que hacía que la app se cerrase la primera vez que se iniciaba, se arregla actualizando el Visual Studio a la ultima versión (Visual Studio 2019 versión 16.0.3).

-Probada en diferentes terminales (Motorola Nexus(Android 5.1), Samsung S5 Neo(Android 5.0), Samsung tb S2 (T710) 8" (Android 7), Xiaomi MI6 (Android 8.0) y Xiaomi MI A2 (Android 9)).

----------------------------------------------------------------------------------------------------------------------------------------

