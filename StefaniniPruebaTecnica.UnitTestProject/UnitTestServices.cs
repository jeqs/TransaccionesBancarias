using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stefanini.Services;
using Stefanini.Services.Dto.Model;
using System.Linq;

namespace StefaniniPruebaTecnica.UnitTestProject
{
    [TestClass]
    public class UnitTestServices
    {
        private const string RespuestaTransferencia = "Transferencia realizada con Éxito";
        [TestMethod]
        public void PruebaUno()
        {
            TransaccionModel request = new TransaccionModel();
            request.CuentaBancariaId = 1;
            request.Monto = 100000000;
            request.CuentaBancariaDestinoId = 2;

            var response = TransaccionServices.Transferir(request);
            var msj = response.Messages.FirstOrDefault();

            Assert.AreEqual(RespuestaTransferencia, msj.Description);
        }

        [TestMethod]
        public void PruebaDos()
        {
            TransaccionModel request = new TransaccionModel();
            request.CuentaBancariaId = 2;
            request.Monto = 150000000;
            request.CuentaBancariaDestinoId = 3;

            var response = TransaccionServices.Transferir(request);
            var msj = response.Messages.FirstOrDefault();

            Assert.AreEqual(RespuestaTransferencia, msj.Description);
        }

        private const string RespuestaRetiro = "Retiro realizado con Éxito";
        [TestMethod]
        public void PruebaTres()
        {
            TransaccionModel request = new TransaccionModel();
            request.CuentaBancariaId = 3;
            request.Monto = 3000000;

            var response = TransaccionServices.Retirar(request);
            var msj = response.Messages.FirstOrDefault();

            Assert.AreEqual(RespuestaRetiro, msj.Description);
        }
    }
}
