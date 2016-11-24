using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Entidades;

namespace Infraestructura
{
  /// <summary>
  /// Simplemente se llama OMBSesion para que no se confunda en MVC pero deberia ser Sesion
  /// Mantiene durante el transcurso de la aplicacion toda la informacion necesaria asociada con el usuario conectado
  /// </summary>
  public class OMBSesion
  {
    public static OMBSesion Current { get; private set; }

    public Usuario Usuario { get; private set; }

    public Perfil Perfil { get; private set; }

    /// <summary>
    /// Usar esta propiedad para un acceso mas sencillo al nombre completo del usuario, sin tener que navegar entre las 
    /// propiedades de Empleado y Persona
    /// </summary>
    public string FullName
    {
      get
      {
#if CS5
        return string.Format("{0} {1}", Usuario.Empleado.Persona.Nombres, Usuario.Empleado.Persona.Apellidos);
#else
        return string.Format($"{Usuario.Empleado.Persona.Nombres} {Usuario.Empleado.Persona.Apellidos}");
#endif
      }
    }

    /// <summary>
    /// Como FullName, esta propiedad es una fachada para no tener que acceder al Usuario
    /// </summary>
    public DateTime? FechaExpiracionPass
    {
      get { return Usuario.PasswordExpiration ; }
    }

    public DateTime LastLogin
    {
      get { return Usuario.LastSuccessLogin.Value; }
    }

    static OMBSesion()
    {
      Current = null;
    }

    //  Evitamos la creacion de instancias de OMBSesion, fuera del SINGLETON
    //
    private OMBSesion() { }

    public static void CreateNewSession(Usuario usr, Perfil perfil = null)
    {
      Current = new OMBSesion();

      Current.Usuario = usr;
      Current.Perfil = null;        //  TODO null object!!
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IEnumerable<string> GetMessageFromConnectedUser()
    {
      return null;
    }

    public void Logout()
    {
      Current = null;
    }
  }
}
