using Stefanini.Services.Code;

namespace Stefanini.Services
{
    public sealed class Message
    {
        public Codes Code { get; }
        public string Description { get; }

        public Message(Codes code, string description)
        {
            Code = code;
            Description = description;
        }
    }
}
