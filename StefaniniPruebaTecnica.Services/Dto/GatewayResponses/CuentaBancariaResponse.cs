using Stefanini.Services.Dto.Model;
using System.Collections.Generic;

namespace Stefanini.Services.Dto.GatewayResponses
{
    public sealed class CuentaBancariaResponse : BaseGatewayResponse
    {
        public CuentaBancariaModel CuentaBancaria { get; set; }

        public CuentaBancariaResponse(CuentaBancariaModel cuentaBancariaModel, bool success = false, IEnumerable<Message> messages = null) : base(success, messages)
        {
            CuentaBancaria = cuentaBancariaModel;
        }
    }

    public sealed class CuentaBancariasResponse : BaseGatewayResponse
    {
        public List<CuentaBancariaModel> CuentasBancarias { get; set; }

        public CuentaBancariasResponse(List<CuentaBancariaModel> cuentaBancariaModel, bool success = false, IEnumerable<Message> messages = null) : base(success, messages)
        {
            CuentasBancarias = cuentaBancariaModel;
        }
    }
}
