using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Vision;
using Android.Gms.Vision.Texts;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using static Android.Gms.Vision.Detector;


//IMPORTANTE:
//Hay que importar en el android SDK manager "Google play services" (En la opcion extras). Luego, en Referencias del proyecto
//hacemos clic derecho y agregamos "Xamarin.GooglePlayServices.Vision (V-60.1142.1)","Xamarin.Android.Support.v7.AppCompact (V-27.0.2)",
//"Xamarin.Android.Support.v4 (V-27.0.2)" y  "Xamarin.Forms (V-3.6.0.293080)" en paquetes NuGet.
//Es necesario que las versiones de "Xamarin.Android.Support.v7.AppCompact" y "Xamarin.Android.Support.v4"
//sean la (V-27.0.2) para no dar problemas con Xamarin.Forms.
namespace CameraScanner
{
    //Applicaion siempre en vertical
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity, ISurfaceHolderCallback, IProcessor
    //ISurfaceHolderCallback es utilizada para detectar cambios en la Surface. 
    //IProcessor: Interfaz necesaria para el funcionamiento de la api de Google.    
    {
        //Declaracion de variables.
        private CameraSource cameraSource;
        private const int RequestCameraPermissionID = 1001;
        private SurfaceView cameraView;
        private TextRecognizer textRecognizer;
        public TextView texto;
        private Button botonCaptura, botonDatos;
        private bool capturarTexto = false;
        public string ruta;
        bool flag;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            //Establecer nuestra vista "main"
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            //Bandera
            flag = false;

            //SurfaceView
            cameraView = FindViewById<SurfaceView>(Resource.Id.surface_view);

            //TextView
            texto = FindViewById<TextView>(Resource.Id.text_view);

            //Botones
            botonCaptura = FindViewById<Button>(Resource.Id.btn_CapturaTexto);
            botonDatos = FindViewById<Button>(Resource.Id.btn_datos);

            //Evento clic boton datos
            botonDatos.Click += DatosGuardados;

            //Obtener permisos lectura/escritura
            PermisosLecturaEscritura();

            //Crear directorio de la app
            CreacionDirectorioApp();

            //TextRecognizer
            CrearTextRecognizer();

            //Al producirse el evento Click del boton llama al metodo Capturar.
            botonCaptura.Click += Capturar;
        }



        public void CrearTextRecognizer()
        {
            textRecognizer = new TextRecognizer.Builder(ApplicationContext).Build();
            if (!textRecognizer.IsOperational)
                Log.Error("Aviso:", "Detector no disponible!");
            else
            {
                // Caracteristicas del CameraSource
                cameraSource = new CameraSource.Builder(ApplicationContext, textRecognizer)
                    .SetFacing(CameraFacing.Back) //Camara seleccionada
                    .SetRequestedPreviewSize(1920, 1080) //Tamaño de la vista previa en uso por la camara
                    .SetRequestedFps(2.0f) //Velocidad de fotogramas (En fotogramas por segundo)
                    .SetAutoFocusEnabled(true) //Autoenfoque
                    .Build();
                cameraView.Holder.AddCallback(this);//Recibe informacion del SurfaceView.                
                textRecognizer.SetProcessor(this);//Estable Iprocessor para el textRecognizer.               
            }
        }



        //Metodo sobreescritura de los permisos de la camara.
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            switch (requestCode)
            {
                case RequestCameraPermissionID:

                    if (grantResults[0] == Permission.Granted)
                    {
                        CrearTextRecognizer();
                        EncenderCamara();
                        CreacionDirectorioApp();
                    }
                    break;
            }
        }


        //Metodo que activa la captura de texto.
        private void Capturar(object sender, EventArgs eventArgs)
        {
            //Comprueba si existe el directorio
            if (CreacionDirectorioApp())
            {
                capturarTexto = true;
            }
            else
            {
                View view = (View)sender;
                Snackbar.Make(view, "Opcion no válida sin permisos de lectura/escritura", Snackbar.LengthLong).SetAction("Action", (View.IOnClickListener)null).Show();
            }
        }

        //Metodo de ISurfaceHolderCallback
        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Format format, int width, int height)
        {

        }
        //Metodo de ISurfaceHolderCallback
        public void SurfaceCreated(ISurfaceHolder holder)
        {
            CrearTextRecognizer();
            EncenderCamara();
        }

        //Metodo de ISurfaceHolderCallback
        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            ApagarCamara();
        }


        //(Metodo de IProcessor) Metodo que recibe de la API de Google el texto detectado.
        public void ReceiveDetections(Detections detections)
        {
            SparseArray items = detections.DetectedItems;
            List<TextBlock> listaTexto = new List<TextBlock>();
            StringBuilder cadenaTexto = new StringBuilder();
            //Funcion Lambda
            texto.Post(() =>
            {
                for (int cont = 0; cont < items.Size(); cont++)
                {
                    //Agrega los "bloques de texto" a listaTexto
                    listaTexto.Add((TextBlock)items.ValueAt(cont));
                }
                foreach (TextBlock item in listaTexto)
                {
                    //Adjunta TextBlock on a StringBuilder
                    cadenaTexto.Append(item.Value);
                    cadenaTexto.Append("\n");
                }
                //Actualiza el TextView
                texto.Text = cadenaTexto.ToString();
                // Creacion del archivo de texto en la memoria del dispositivo
                if (capturarTexto)
                {
                    //Creacion del nombre del archivo guardado (En este caso se usa la fecha actual como nombre).                   
                    String nombreArchivo = DateTime.Now.ToString();
                    //Quitar caracteres invalidos para los nombres de archivo.
                    nombreArchivo = nombreArchivo.Replace(':', '-');
                    nombreArchivo = nombreArchivo.Replace('/', '-');
                    //Creación de la ruta completa con la ruta mas el nombre del archivo.
                    string rutaCompleta = System.IO.Path.Combine(ruta, nombreArchivo.ToString() + ".txt");
                    GeneradorArchivos(cadenaTexto, rutaCompleta);
                    Toast.MakeText(this.ApplicationContext, "Texto Capturado!", ToastLength.Short).Show();
                }
            });
        }

        //Metodo para guardar los archivos en la memoria del dispositivo.
        public void GeneradorArchivos(StringBuilder caracteres, String archivo)
        {
            //Comprueba que existe el directorio.
            if (CreacionDirectorioApp())
            {
                using (var streamWriter = new StreamWriter(archivo, true))
                {
                    streamWriter.WriteLine(caracteres);
                    streamWriter.Close();
                }
                capturarTexto = false;
            }
        }

        //Metodo de IProcessor.
        public void Release() { }

        //Metodo para obtener permisos lectura/escritura en memoria del telf.
        private void PermisosLecturaEscritura()
        {
            if ((int)Build.VERSION.SdkInt < 21)
            {
                return;
            }
            else
            {
                //Comprueba permisos de acceso a la camara, lectura y escritura.
                if (ActivityCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.Camera) != Permission.Granted
                    || ActivityCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.WriteExternalStorage) != Permission.Granted
                    || ActivityCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.ReadExternalStorage) != Permission.Granted)
                {
                    //Solicitar permisos para usar la API de Google.
                    ActivityCompat.RequestPermissions(this, new string[]
                    {
                     //Escribir los permisos en el manifest
                   Manifest.Permission.Camera,Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage
                    }, RequestCameraPermissionID);
                    return;
                }
            }
        }

        //Metodo para crear el directorio de archivos guardados de la app.
        private bool CreacionDirectorioApp()
        {
            //Variable con la ruta de la app.
            ruta = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/CameraScanner";
            //Comprueba permisos lectura/escritura
            if (PackageManager.CheckPermission(Manifest.Permission.ReadExternalStorage, PackageName).Equals(Permission.Granted)
                    && PackageManager.CheckPermission(Manifest.Permission.WriteExternalStorage, PackageName).Equals(Permission.Granted))
            {
                //Creacion del direcctorio si no existe.
                if (!Directory.Exists(ruta))
                {
                    Directory.CreateDirectory(ruta);
                }
                return true;
            }
            return false;
        }

        //Metodo para abrir segunda Activity si hay directorio de archivos creado.
        private void DatosGuardados(object sender, EventArgs eventArgs)
        {
            //Snackbar.Make(view, "Opcion no activada por ahora", Snackbar.LengthLong).SetAction("Action", (View.IOnClickListener)null).Show();

            //Iniciar activity Datos si hay directorio creado
            if (CreacionDirectorioApp())
            {
                //Segundo Activity
                Intent intent = new Intent(this, typeof(Datos));
                this.StartActivity(intent);
            }
            else
            {
                View view = (View)sender;
                Snackbar.Make(view, "Opcion no válida sin permisos de lectura/escritura", Snackbar.LengthLong).SetAction("Action", (View.IOnClickListener)null).Show();
            }

        }

        //Metodo para iniciar CameraSource
        public void EncenderCamara()
        {
            if (!flag)
            {
                //Comprueba permisos de acceso a la camara.
                if (ActivityCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.Camera) == Permission.Granted)
                {
                    //Toast.MakeText(this.ApplicationContext, "Enciendo Camara!", ToastLength.Short).Show();
                    cameraSource.Start(cameraView.Holder);
                    flag = true;
                }
            }
        }

        //Metodo para desactivar CameraSource
        public void ApagarCamara()
        {
            if (flag)
            {
                if (cameraSource != null)
                {
                    //Toast.MakeText(this.ApplicationContext, "Apago Camara!", ToastLength.Short).Show();
                    cameraSource.Stop();
                    //cameraSource.Dispose();
                    flag = false;
                }

            }

        }

    }

}