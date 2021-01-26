using System;

namespace Stefanini.Services.Dto.GatewayRequest
{
    public class GeneralRequest
    {
        public int Edad { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public int Anio { get; set; }
        public int ClienteId { get; set; }
    }
}
