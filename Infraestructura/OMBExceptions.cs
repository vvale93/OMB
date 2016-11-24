using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura
{
  public class OMBExceptionBase : Exception
  {
    public OMBExceptionBase(string msg) : base(msg)
    {
      
    }
  }
}
