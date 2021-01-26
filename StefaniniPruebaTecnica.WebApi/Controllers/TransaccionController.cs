using Stefanini.Services;
using Stefanini.Services.Dto.Model;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Stefanini.WebApi.Controllers
{
    [Route("api/Transaccion/{action}", Name = "Transaccion")]
    public class TransaccionController : ApiController
    {
        // GET: api/Transaccion/ObtenerTransferencias
        [HttpGet]
        public HttpResponseMessage ObtenerTransferencias()
        {
            var response = TransaccionServices.ObtenerTransferencias();
            HttpResponseMessage result = Request.CreateResponse(HttpStatusCode.OK, response);
            return result;
        }

        // POST: api/Transaccion/Transferir
        [HttpPost]
        public HttpResponseMessage Transferir([FromBody] TransaccionModel request)
        {
            var response = TransaccionServices.Transferir(request);
            HttpResponseMessage result = Request.CreateResponse(HttpStatusCode.OK, response);
            return result;
        }

        // POST: api/Transaccion/Retirar
        [HttpPost]
        public HttpResponseMessage Retirar([FromBody] TransaccionModel request)
        {
            var response = TransaccionServices.Retirar(request);
            HttpResponseMessage result = Request.CreateResponse(HttpStatusCode.OK, response);
            return result;
        }

    }
}