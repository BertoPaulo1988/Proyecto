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
using AlertDialog = Android.Support.V7.App.AlertDialog;

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
            fab.Click += PreguntaBorradoCompleto;

            //Ignora la URI de los archivos
            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());

            CrearListado();
            //Eventos del ListView       
            lista.ItemClick += AbrirArchivo;
            lista.ItemLongClick += PreguntaBorradoSimple;

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
                    File archivo = archivos[i];
                    nombres.Add(archivo.Name);
                }
            }
            //Localizamos y llenamos la lista con el array
            listaArchivos = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, nombres);
            lista.Adapter = listaArchivos;
        }


        //Metodo para abrir archivos al pulsar en ellos en la lista 
        void AbrirArchivo(object sender, AdapterView.ItemClickEventArgs item)
        {
            var t = archivos[item.Position];
            File archivo = new File(t.ToString());
            var uri = Android.Net.Uri.Parse("file:///" + archivo.Path);
            // Android.Net.Uri uri = FileProvider.GetUriForFile(this, ApplicationContext.PackageName + ".fileprovider", new File(archivo.Path));
            Intent intent = new Intent();
            intent.AddFlags(ActivityFlags.NewTask);
            intent.SetAction(Intent.ActionView);
            intent.SetDataAndType(uri, "text/plain");
            //Toast.MakeText(this, Android.Net.Uri.Parse("file:///" + archivo.Path).ToString(), ToastLength.Short).Show();
            try
            {
                StartActivity(intent);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }


        //Metodo para volver al activity anterior.
        private void Volver(object sender, EventArgs eventArgs)
        {
            this.Finish();
        }

        //Metodo para borrar archivos de la lista.
        private void Borrado(object sender, AdapterView.ItemLongClickEventArgs item)
        {
            var t = archivos[item.Position];
            File archivo = new File(t.ToString());
            for (int i = archivos.Length - 1; i >= 0; i--)
            {
                if (archivo.Name == archivos[i].Name)
                {
                    archivos[i].Delete();
                    Toast.MakeText(this, "Archivo " + nombres[i].ToString() + " borrado", ToastLength.Short).Show();
                    nombres.RemoveAt(i);

                }
            }
            Actualizar();
        }


        //Metodo para refrescar la pantalla despues de un cambio en el listview
        private void Actualizar()
        {
            //Dejo la comprobacion hecha para adaptarlo a Android 9 (utilizando "Recreate" en android 9 da un pequeño retardo la pantalla)
            if ((int)Build.VERSION.SdkInt > 27)
            {
                Recreate();
            }
            else
            {
                Recreate();
            }

        }


        //Metodo para confirmar el borrado del archivo seleccionado
        public void PreguntaBorradoSimple(object sender, AdapterView.ItemLongClickEventArgs item)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Borrar?");
            alert.SetMessage("Quieres borrar el elemento seleccionado?");
            alert.SetPositiveButton("Borrar", (senderAlert, args) =>
            {
                Borrado(sender, item);
            });

            alert.SetNegativeButton("Cancelar", (senderAlert, args) =>
            {
                //Toast.MakeText(this, "Cancelado!", ToastLength.Short).Show();
            });

            Dialog dialogo = alert.Create();
            dialogo.Show();
        }

        //Metodo para confirmar el borrado de todos los archivos
        public void PreguntaBorradoCompleto(object sender, EventArgs eventArgs)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Borrar?");
            alert.SetMessage("Quieres borrar toda la lista?");
            alert.SetPositiveButton("Borrar", (senderAlert, args) =>
            {
                for (int i = archivos.Length - 1; i >= 0; i--)
                {
                    archivos[i].Delete();
                    nombres.RemoveAt(i);
                }
                Actualizar();
                Toast.MakeText(this, "Lista borrada", ToastLength.Short).Show();
            });
            alert.SetNegativeButton("Cancelar", (senderAlert, args) =>
            {
                //Toast.MakeText(this, "Cancelado!", ToastLength.Short).Show();
            });

            Dialog dialogo = alert.Create();
            dialogo.Show();
        }

    }
}