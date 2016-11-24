using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
  public class Editorial
  {
    public Guid IDEditorial { get; set; }
    public string Nombre { get; set; }
    public string Domicilio { get; set; }
    public string AmpliacionDomicilio { get; set; }
    public string CodigoPostal { get; set; }

    public virtual Localidad Localidad { get; set; }

    public Editorial()
    {
      IDEditorial = Guid.NewGuid();
    }
  }
}
