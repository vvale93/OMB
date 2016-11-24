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
using OMB_Desktop.Common;
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

    public InteractionRequest<INotification> FaltanDatos { get; set; }

    public InteractionRequest<INotification> CredencialesInvalidas { get; set; }

    public RelayCommand<string> LoginCommand { get; set; }

    public INotification Notification { get; set; }

    public Action FinishInteraction { get; set; }

    public LoginViewModel()
    {
      //  LoginID = "---";
      //
      //  bindeamos comandos
      LoginCommand = new RelayCommand<string>(DoLogin);

      FaltanDatos = new InteractionRequest<INotification>();
      CredencialesInvalidas = new InteractionRequest<INotification>();
    }

    public void DoLogin(string pass)
    {
      SecurityServices seg = new SecurityServices(new NullMailService());

      if (!string.IsNullOrWhiteSpace(pass))
      {
        Console.WriteLine(pass);

        if (seg.Login(LoginID, pass))
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
