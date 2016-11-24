using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OMB_Desktop.ViewModels;

namespace OMB_Desktop.Views
{
  public partial class LoginView : UserControl
  {
    public LoginView()
    {
      InitializeComponent();
    }

    //  tiene lugar cuando los controles del control se cargaron completamente
    private void LoginViewLoaded(object sender, RoutedEventArgs e)
    {
      //  Este uso de codebehind es inevitable
      txtUsuario.Focus();
    }
  }
}
