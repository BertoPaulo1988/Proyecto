# Proyecto

Nombre de la aplicación: CameraScanner.

Info:
Aplicación diseñada para capturar texto en tiempo real mediante el CameraSource (API de Google). 

Funcionamiento:

-El usuario enfoca a cualquier texto con la cámara trasera del teléfono e inmediatamente verá el texto que está recibiendo. Si quiere capturar ese texto solo tiene que pulsar el boton "Capturar texto" e inmediatamente de generará un documento de texto (.txt) con la información recogida.
-Para acceder a esos documentos tendrá que pulsar sobre el botón "Datos guardados" y accederá a la lista de todos los documentos realizados. 
-Si quiere abrir esos documentos tendrá que tocar sobre ellos y saldrá un menú donde seleccionará una app compatible para abrirlos (es necesario tener apps instaladas en el terminal compatibles con documentos.txt).
-Si quiere borrar algún documento de la lista solo tiene que hacer pulsación larga sobre el documento seleccionado y este se eliminará de la lista.
-Para volver a la activity principal tiene que pulsar el boton volver.

----------------------------------------------------------------------------------------------------------------------------------------
IMPORTANTE:

Hay que importar en el android SDK manager "Google play services" (En la opcion extras). Luego, en Referencias del proyecto
hacemos clic derecho y agregamos "Xamarin.GooglePlayServices.Vision (V-60.1142.1)","Xamarin.Android.Support.v7.AppCompact (V-27.0.2)",
"Xamarin.Android.Support.v4 (V-27.0.2)" y  "Xamarin.Forms (V-3.6.0.293080)" en paquetes NuGet.
Es necesario que las versiones de "Xamarin.Android.Support.v7.AppCompact" y "Xamarin.Android.Support.v4" sean la (V-27.0.2) para no dar problemas con Xamarin.Forms.


----------------------------------------------------------------------------------------------------------------------------------------


-En la primera versión crearemos la clase principal (Mainactivity), donde realizaremos el funcionamiento principal de la API de Google para la detección de imagenes y la gestión de permisos necesarios para el funcionamiento de la APP (acceso a la camara, permisos de lectura/escritura, etc).

-Hay que implementar en la calse las interfaces "ISurfaceHolderCallback e IProcessor" (necesarias para la API).Estas interfaces no funcinarán sin sus respectivos metodos.

-El diseño de la APP (Botones, TexView, etc) se definen en el documento axml situado en la carpeta Resources/Layout.

-La aplicación solo se podrá ejecutar en vertical.

-En la segunda versión de la APP crearemos una segunda clase (llamada datos), donde gestionaremos los archivos guardados mediante un ListView, con la posibilidad abrirlos con una pulsacion corta o de borrarlos mediante una pulsación larga.

-Para gestionar el diseño del segundo activity ("Datos") es necesario crear otro documento axml y asociarlo a la clase (accederemos a el desde la clase con  "SetContentView(Resource.Layout."aqui el nombre del archivo axml")".

-Para darle colores a los elementos de la lista hay que crear en resources la carpeta drawable y dentro un archivo .xml. Lo asociamos a la lista con el comando ( android:listSelector="@drawable/aqui nombre del archivo") en el archivo axml antes mencionado.




----------------------------------------------------------------------------------------------------------------------------------------
Ultimas actualizaciones:

Laaplicación ya es funcional, dando solo un error la primera vez que se ejecuta en el terminal (una vez cerrada y vuelta a abrir ya es prefectamente funcional).


El error es :"F/libc    (26772): Fatal signal 11 (SIGSEGV), code 1, fault addr 0xbc in tid 26872 (Thread-5)". 

Ocurre al pasar al segundo activity pulsando en el boton "Datos guardados".

----------------------------------------------------------------------------------------------------------------------------------------

