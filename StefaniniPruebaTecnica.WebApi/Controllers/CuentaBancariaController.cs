using Stefanini.Services;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Stefanini.WebApi.Controllers
{
    [Route("api/CuentaBancaria/{action}", Name = "CuentaBancaria")]
    public class CuentaBancariaController : ApiController
    {
        // GET: api/CuentaBancaria/ObtenerCuentasBancarias
        [HttpGet]
        public HttpResponseMessage ObtenerCuentasBancarias()
        {
            var response = CuentaBancariaServices.ObtenerCuentasBancarias();
            HttpResponseMessage result = Request.CreateResponse(HttpStatusCode.OK, response);
            return result;
        }
       
    }
}
