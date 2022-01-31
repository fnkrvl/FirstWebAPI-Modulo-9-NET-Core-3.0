using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace EFCore_Módulo9.Tests
{
    [TestClass]  // Indica que ésta clase va a contener pruebas.
    public class TransferenciasTests
    {
        [TestMethod] // Son métodos que sirven para hacer pruebas
        public void TransferenciasEntreCuentasConFondosInsuficientesArrojaUnError()
        {
            // Preparación
            Exception expectedException = null;
            Cuenta origen = new Cuenta() { Fondos = 0 };
            Cuenta destino = new Cuenta() { Fondos = 0 };
            decimal montoATransferir = 5m;
            var mock = new Mock<IServicioValidacionesDeTransferencias>();
            string mensajeError = "mensaje de error";
            mock.Setup(x => x.RealizarValidaciones(origen, destino, montoATransferir))
                .Returns(mensajeError);
            var servicio = new ServicioDeTransferencias(mock.Object);

            // Prueba
            try
            {
                servicio.TransferirEntreCuentas(origen, destino, montoATransferir);
                Assert.Fail("Un error debió ser arrojado");
            }
            catch (Exception ex)
            {
                expectedException = ex;
            }

            // Verificación
            Assert.IsTrue(expectedException is ApplicationException);
            Assert.AreEqual(mensajeError, expectedException.Message);
            // La clase Assert (afirmar) sirve para hacer declaraciones las cuales entendemos deben ser correctas.
            // De no serlo, se arroja un error y la prueba falla.
            // Un prueba es exitosa cuando todas sus afirmaciones son acertadas.
        }

        [TestMethod]
        public void TransferenciaEntreCuentasEditaLosFondos()
        {
            // Preparación
            Cuenta origen = new Cuenta() { Fondos = 10 };
            Cuenta destino = new Cuenta() { Fondos = 5 };
            decimal montoATransferir = 7m;  // Se pone la "m" porque no se puede convertir implicitamente de double a decimal.
            var mock = new Mock<IServicioValidacionesDeTransferencias>();

            mock.Setup(x => x.RealizarValidaciones(origen, destino, montoATransferir));

            var servicio = new ServicioDeTransferencias(mock.Object);

            // Prueba
            servicio.TransferirEntreCuentas(origen, destino, montoATransferir);

            // Verificación
            Assert.AreEqual(3, origen.Fondos);
            Assert.AreEqual(12, destino.Fondos);
        }
    }
}
