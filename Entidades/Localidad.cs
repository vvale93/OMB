using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
  public class Localidad
  {
    public int IDLocalidad { get; set; }
    public string Nombre { get; set; }
    public string CodigoPostal { get; set; }

    public virtual Provincia Provincia { get; set; }
  }
}
