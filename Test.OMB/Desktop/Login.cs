using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OMB_Desktop.ViewModel; //Hay que referenciarlo antes para que aparezca!
//agregar el prism wpf y el wvvm con el nuget package manager

namespace Test.OMB.Desktop
{
    [TestClass]
    public class Login
    {
        [TestMethod]
        public void LoginFallidoSinUsuario() // hay que verificar que ele evento de faltan datos se haya dispaarado
        {
            LoginViewModel lvm = new LoginViewModel();
            bool evento = false;

            lvm.Password = "cualquiera";
            lvm.LoginID = null;

            //chequea lo q apareceria en pantalla
            lvm.FaltanDatos.Raised += (s, a) =>
            {
                evento = true;
            };
            lvm.LoginCommand.Execute(null);             //accede al disparador LoginCommand
            Assert.IsTrue(evento);

        }
    }
}
