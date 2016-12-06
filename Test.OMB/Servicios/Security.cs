using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Servicios;

// tienen que ser publicos, void, y no tienen que tener argumentos. 

namespace Test.OMB.Servicios
{
    [TestClass] // <-- atributo de la clase
    public class Security
    {
        [TestMethod]
        public void ProbarLoginConDatosIncorrectos()
        {
            SecurityServices serv = new SecurityServices();
            bool result;

            result = serv.Login("cosme", "fulanito");

            Assert.IsFalse(result, "Hay un usuario cosme?????");
        }

        [TestMethod]
        public void ProbarLoginConDatosCorrectos()
        {
            SecurityServices serv = new SecurityServices();
            bool result;

            result = serv.Login("ethedy", "12345678");

            Assert.IsTrue(result, "Hay un usuario ethedy!");
        }
    }
}
