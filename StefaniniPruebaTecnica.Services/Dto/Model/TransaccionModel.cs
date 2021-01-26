namespace Stefanini.Services.Dto.Model
{
    public class TransaccionModel
    {
        public int Id { get; set; }
        public int? CuentaBancariaId { get; set; }
        public int? TipoTransaccionId { get; set; }
        public decimal? Monto { get; set; }
        public decimal? GMF { get; set; }

        // Propiedades Extendidas
        public int? CuentaBancariaDestinoId { get; set; }
        public string TipoTransaccion { get; set; }
        public string CuentaBancaria { get; set; }
    }
}
