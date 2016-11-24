using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
  public class Contacto
  {
    public int IDContacto { get; set; }

    public string Dato { get; set; }

    /// <summary>
    /// Por ejemplo si el Contacto solo deberia ser usado en ciertos horarios o ciertos dias de la semana
    /// </summary>
    public string Comentario { get; set; }

    public virtual TipoContacto Tipo { get; set; }
  }
}
