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
using Entidades;
using OMB_Desktop.ViewModel; // para referenciar

namespace OMB_Desktop
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
        InitializeComponent();
        //esto asocia la vista con el view model, esto es una dependencia (demasiado rigida) entre la vista y el view model mediante el new
        //this.DataContext = new MainWindowViewModel();

    }

    //private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    //{
    //  //this.DataContext = new MainWindowViewModel();
    //}
  }
}
