using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
  public class Provincia
  {
    public string IDProvincia { get; set; }
    public string Nombre { get; set; }

    public virtual HashSet<Localidad> Localidades { get; set; }
  }
}
