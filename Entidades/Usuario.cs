using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
  /// <summary>
  /// Representa a un usuario interno de la empresa
  /// Observar que NO TIENE LA PASSWORD...
  /// </summary>
  public class Usuario
  {
    public string Login { get; set; }

    public DateTime? PasswordExpiration { get; set; }

    public DateTime? LastSuccessLogin { get; set; }

    public DateTime? LastFailLogin { get; set; }

    public bool? MustChangePassword { get; set; }

    public bool? EnforceExpiration { get; set; }

    public bool? EnforceStrong { get; set; }

    public bool Enabled { get; set; }

    public bool Blocked { get; set; }

    public virtual Empleado Empleado { get; set; }

    //  public virtual HashSet<Perfil> Perfiles { get; set; }
  }
}
