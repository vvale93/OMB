using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace OMB_Desktop
{
  /// <summary>
  /// Logica general para el startup de la aplicacion
  /// </summary>
  public partial class App : Application
  {
    public App()
    {
      //  aca seteo el service locator, como para que cualquier clase dentro de la app pueda utilizar el mismo contenedor
      //
      ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
    }
  }
}
