using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
  public enum Sexo
  {
    Femenino,
    Masculino,
    NoInformado,
    Indefinido
  }

  public class TipoDocumento
  {
    public int IDTipoDocumento { get; set; }
    public string Descripcion { get; set; }

    /// <summary>
    /// Se podria incorporar alguna expresion de validacion o un texto con que ayudar al ingreso del mismo
    /// </summary>
    public string Observaciones { get; set; }
  }

  public class TipoContacto
  {
    public int IDTipoContacto { get; set; }

    /// <summary>
    /// Por ejemplo: Movil Particular, Movil Empresa, Domicilio, Trabajo, Correo Electronico, etc... 
    /// </summary>
    public string Descripcion { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string RegExp { get; set; }
  }

  public class CategoriaEmpleado
  {
    public int IDCategoriaEmpleado { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
  }
}
