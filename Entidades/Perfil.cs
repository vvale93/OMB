using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
  public class Perfil
  {
    public Guid IDPerfil { get; set; }

    public string Nombre { get; set; }

    public string Descripcion { get; set; }
  }
}
