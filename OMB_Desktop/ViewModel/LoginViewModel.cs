using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Entidades;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Infraestructura;
using Prism.Interactivity.InteractionRequest;
using Servicios;

namespace OMB_Desktop.ViewModel
{
  public class LoginViewModel : ViewModelBase, IInteractionRequestAware
  {
    private string _userid;

    public string LoginID
    {
      get { return _userid; }
      set
      {
        Set(() => LoginID, ref _userid, value); 
      }
    }
        private string _password; //campo soporte para la propiedad Password
    public string Password
    {
        get { return _password; }
        set { Set(() => Password, ref _password, value); } //Metodo Set q le dice a la vista q la prop cambio, (paso propiedad, referencia y valor)
    }


        public InteractionRequest<INotification> FaltanDatos { get; set; }

    public InteractionRequest<INotification> CredencialesInvalidas { get; set; }

    public RelayCommand LoginCommand { get; set; }

    public INotification Notification { get; set; }

    public Action FinishInteraction { get; set; }

    public LoginViewModel()
    {
      //  LoginID = "---";
      //
      //  bindeamos comandos
      LoginCommand = new RelayCommand(DoLogin);

      FaltanDatos = new InteractionRequest<INotification>();
      CredencialesInvalidas = new InteractionRequest<INotification>();
    }
   
    public void DoLogin()
    {
      SecurityServices seg = new SecurityServices(); //new NullMailService() serivicio que usamos para todo lo que tenga que ver con accesos, seguridad, etc

            if (!string.IsNullOrWhiteSpace(Password))
      {
        Console.WriteLine(Password);

        if (seg.Login(LoginID, Password) && !string.IsNullOrWhiteSpace(LoginID))
        {
          //  OMBSesion sesion = new OMBSesion(user);

          //  MessengerInstance.Send<OMBSesion>(sesion);
          if (FinishInteraction != null)
            FinishInteraction();
          
          //MessengerInstance.Send<LoginMessage>(new LoginMessage() { Show = false });
        }
        else
        {
          CredencialesInvalidas.Raise(new Notification()
          {
            Title = "ERROR INGRESO",
            Content = seg.ErrorInfo
          });
        }
      }
      else
      {
        FaltanDatos.Raise(new Notification()
        {
          Title = "ERROR INGRESO",
          Content = "Debe especificarse un usuario y contraseña"
        });
      }
    }
  }
}


