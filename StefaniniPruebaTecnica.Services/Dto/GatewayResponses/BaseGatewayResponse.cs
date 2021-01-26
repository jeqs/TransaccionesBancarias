using System.Collections.Generic;

namespace Stefanini.Services.Dto.GatewayResponses
{
    public abstract class BaseGatewayResponse
    {
        public bool Success { get; }
        public IEnumerable<Message> Messages { get; }

        protected BaseGatewayResponse(bool success = false, IEnumerable<Message> messages = null)
        {
            Success = success;
            Messages = messages;
        }
    }
}
