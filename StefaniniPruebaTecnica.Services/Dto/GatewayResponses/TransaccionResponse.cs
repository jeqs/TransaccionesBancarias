using Stefanini.Services.Dto.Model;
using System.Collections.Generic;

namespace Stefanini.Services.Dto.GatewayResponses
{
    public sealed class TransaccionResponse : BaseGatewayResponse
    {
        public TransaccionModel Transaccion { get; set; }

        public TransaccionResponse(TransaccionModel transaccionModel, bool success = false, IEnumerable<Message> messages = null) : base(success, messages)
        {
            Transaccion = transaccionModel;
        }
    }

    public sealed class TransaccionesResponse : BaseGatewayResponse
    {
        public List<TransaccionModel> data { get; set; }

        public TransaccionesResponse(List<TransaccionModel> transaccionesModel, bool success = false, IEnumerable<Message> messages = null) : base(success, messages)
        {
            data = transaccionesModel;
        }
    }
}
