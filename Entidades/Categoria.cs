using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
  /// <summary>
  /// Una Categoria permite catalogar un producto entre el resto
  /// Las Categorias forman un arbol, donde cada una tiene una Categoria parent para permitir la navegacion bidireccional
  /// </summary>
  public class Categoria
  {
    public Guid IDCategoria { get; set; } 
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public int? Cantidad { get; set; }

    public virtual HashSet<Categoria> SubCategorias { get; set; }
    public virtual Categoria Parent { get; set; }

    public Categoria()
    {
      SubCategorias = new HashSet<Categoria>();
      //  Para SQL Compact, colocar la siguiente linea 
      IDCategoria = Guid.NewGuid();
    }
  }
}
