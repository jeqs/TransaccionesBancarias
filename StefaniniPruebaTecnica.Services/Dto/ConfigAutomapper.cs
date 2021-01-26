using AutoMapper;
using Stefanini.Services.Dto.Model;
using StefaniniPruebaTecnica.DataStorage.BD;

namespace Stefanini.Services.Dto
{
    public static class ConfigAutomapper
    {
        public static IMapper mapper { get; set; }
        public static void Config()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CuentaBancariaModel, CuentaBancaria>().ReverseMap();
                cfg.CreateMap<TransaccionModel, Transaccion>().ReverseMap();
                cfg.CreateMap<TransaccionModel, Transacciones>().ReverseMap();
            });

            mapper = config.CreateMapper();
        }
    }
}
