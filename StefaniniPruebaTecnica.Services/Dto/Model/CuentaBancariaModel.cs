namespace Stefanini.Services.Dto.Model
{
    public class CuentaBancariaModel
    {
        public int Id { get; set; }
        public int? BancoId { get; set; }
        public string Nombre { get; set; }
        public int? ClienteId { get; set; }
        public int? TipoCuentaId { get; set; }
    }
}
