//using System;
using Entidades;
using GalaSoft.MvvmLight;
//using OMB_Desktop.Common;
using Prism.Interactivity.InteractionRequest;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Infraestructura;
using Servicios;

namespace OMB_Desktop.ViewModel
{
  /// <summary>
  /// [[VIEWMODEL]]
  /// Logica de la vista principal de la aplicacion
  /// </summary>
  public class MainWindowViewModel : ViewModelBase     //ViewModelBase: implementa "inotifypropertychanged" y metodos
    {
    public ICommand Login { get; set; }

    public ICommand Logout { get; set; }

    public ICommand NullCommand { get; set; }

    public ICommand Buscar { get; set; }

    private Usuario _usuario;

    public Usuario Usuario 
    {
      get { return _usuario; }
      set
      {
        Set(() => Usuario, ref _usuario, value);
      }
    }

    private string _status;

    public string Status
    {
      get { return _status; }
      set { Set(() => Status, ref _status, value); }
    }

    private string _buscar;

    public string TextoBuscar
    {
      get { return _buscar; } //devuelve el valor. 
      set
      {
        Set(() => TextoBuscar, ref _buscar, value); //  Set lanza el evento para la vista. "setea" TextoBuscar y luego 
      }
    }

    public InteractionRequest<INotification> DisplayLogin { get; set; }

    public InteractionRequest<IConfirmation> ConfirmarComando { get; set; }

    /*
      InteractionRequest es la manera que tiene la ui de avisarnos que existe un pedido del
      usuario para por ejemplo mostrar unn dialogo
      En este caso el tipo T es IConfirmation porque tengo que decir si acepto o no la ejecucion

      Para cada interaccion CASI SIEMPRE tambien tenemos que poner un comando
  */
  
    public MainWindowViewModel()
    {
      DisplayLogin = new InteractionRequest<INotification>();

      Login = new RelayCommand(() => //RelayCommand es de MVVMLIte
      {
        DisplayLogin.Raise(new Notification()
        {
            Title = "Ingreso al sistema",
            Content = "PRUEBA"
        },  LoginTerminado);// cuando termina llama a LoginTerminado(el cual chequea si la sesion es o no nula, si no lo es pone la variable login status en "LoginOK"
      }, CanLogin); //interaccionrequest le dice al boton que muestre la ventana
        

      Logout = new RelayCommand(() =>
      {
        SecurityServices serv = new SecurityServices();

        serv.Logout();
        Usuario = null;
        Status = null;
      }, CanLogout);

      NullCommand = new RelayCommand(() => { }, () => false );

      Buscar = new RelayCommand(() => BuscarTexto());

      ConfirmarComando = new InteractionRequest<IConfirmation>();

      //if (IsInDesignMode)
      //{
      //  Usuario = new Usuario()
      //  {
      //    Empleado = new Empleado()
      //    {
      //      Persona = new Persona() { Nombres = "Enrique", Apellidos = "Thedy" }
      //    }
      //  };
      //}
    }

    private bool CanLogin()
    {
            //si no hay usr devuelve null
      return Usuario == null;
    }

    private bool CanLogout()
    {
      return Usuario != null; // si hay usuario devuelve null 
    }

    /// <summary>
    /// Permite buscar o ejecutar un comando a partir de la barra de busqueda
    /// El comando se ejecuta si esta habilitado
    /// Si el texto es un comando, se solicita confirmacion del usuario
    /// </summary>
    private void BuscarTexto()
    {
      if (!string.IsNullOrWhiteSpace(_buscar))
      {
        //  tokenizar la cadena de busqueda
        //  en base a los contenidos, decidir que accion se tiene que realizar
        if (_buscar.ToLower() == "login")
        {
          ConfirmarComando.Raise(new Confirmation()
          {
            Title = "VALIDAR COMANDO",
            Content = "Se esta por intentar ejecutar el comando login desde la barra de busqueda. Es esto correcto?"
          }, conf =>
          {
            if (conf.Confirmed && Login.CanExecute(null))
              Login.Execute(null);
          });
        }
        else
        {
          if (_buscar.ToLower() == "logout" && Logout.CanExecute(null))
            Logout.Execute(null);
        }
      }
    }

    private void LoginTerminado(INotification notification)
    {
      if (OMBSesion.Current != null)
      {
        Status = "Login OK";
        Usuario = OMBSesion.Current.Usuario; //ombsesion definido en infraestructura
      }
    }
  }
}
