using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using Android.Support.Design.Widget;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Java.IO;
using Xamarin.Forms;
using Android.Content.PM;



namespace CameraScanner
{
    //Applicaion siempre en vertical
    [Activity(Label = "Archivos Guardados", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class Datos : AppCompatActivity
    {
        //Declaracion de variables


        public Android.Widget.ListView lista;
        //Array para el nombre de los ficheros de la carpeta
        private List<String> nombres = new List<string>();
        //Ruta donde buscar los ficheros
        String ruta;
        //Direcctorio de los ficheros
        File directorio;
        //Array de tipo File con el contenido de la carpeta
        File[] archivos;
        ArrayAdapter<string> listaArchivos;
        private File archivo;


        //Pruebas para abrir archivo
        String paaath;
        String noooom;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            //Inicializacion de variables
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Datos);

            lista = FindViewById<Android.Widget.ListView>(Resource.Id.lista);
            nombres = new List<string>();
            ruta = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/CameraScanner";
            directorio = new File(ruta);
            archivos = directorio.ListFiles();

            //Boton volver
            Android.Widget.Button volver = FindViewById<Android.Widget.Button>(Resource.Id.btn_volver);
            //Evento del boton volver
            volver.Click += Volver;

            //Boton flotante para borrado (No funcional/En pruebas)
            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += Borrado;

            CrearListado();



            //Prueba de evento en la lista            
            lista.ItemClick += AbrirArchivo;

            lista.ItemLongClick += Marcar;
        }


        //Metodo que busca archivos en el directorio y los agrega a la lista
        private void CrearListado()
        {
            //Comprobamos que tengamos archivos en el directorio
            if (archivos.Length != 0)
            {
                //Agregamos el nombre de los archivos a la lista de nombres
                for (int i = 0; i < archivos.Length; i++)
                {
                    archivo = archivos[i];
                    nombres.Add(archivo.Name);
                }
            }
            //Localizamos y llenamos la lista con el array
            listaArchivos = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, nombres);
            lista.Adapter = listaArchivos;
        }


        //Metodo para abrir archivos al pulsar en ellos en la lista (No funcional/En pruebas)
        private void AbrirArchivo(object sender, EventArgs eventArgs)
        {

            Android.Views.View view = (Android.Views.View)sender;
            // Snackbar.Make(view, "Opcion no válida sin permisos de lectura/escritura", Snackbar.LengthLong).SetAction("Action", (View.IOnClickListener)null).Show();
            // Xamarin.Forms.Device.OpenUri(new Uri(archivos[i].Name));
            for (int i = 0; i < archivos.Length; i++)
            {


            }

        }
        //Metodo para abrir archivos al pulsar en ellos en la lista (No funcional/En pruebas)       
        public void AbrirArchivo2(string ruta, string tipo)
        {
            var uri = Android.Net.Uri.Parse("file://" + ruta);
            var intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(uri, tipo);
            intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);

            try
            {
                this.StartActivity(Intent.CreateChooser(intent, "Seleccione app"));
            }
            catch (Exception e)
            {

            }
        }


        public void Marcar(object sender, EventArgs eventArgs)
        {



        }

        //Metodo para volver al activity anterior.
        private void Volver(object sender, EventArgs eventArgs)
        {
            this.Finish();
        }

        //Metodo para borrar archivos de la lista (No funcional/En pruebas)  .
        private void Borrado(object sender, EventArgs eventArgs)
        {

        }
    }
}