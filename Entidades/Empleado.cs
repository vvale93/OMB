using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
  public class Empleado
  {
    public string Legajo { get; set; }
    public string CUIT { get; set; }
    public DateTime? FechaIngreso { get; set; }
    public DateTime? FechaEgreso { get; set; }

    //  public byte[] Foto { get; set; }
    //  public CategoriaEmpleado Categoria { get; set; }
    public virtual Persona Persona { get; set; }
  }
}
