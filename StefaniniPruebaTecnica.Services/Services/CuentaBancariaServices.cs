using log4net;
using Stefanini.DataStorage.DataAccess;
using Stefanini.Services.Code;
using Stefanini.Services.Dto;
using Stefanini.Services.Dto.GatewayResponses;
using Stefanini.Services.Dto.Model;
using Stefanini.Transverse;
using StefaniniPruebaTecnica.DataStorage.BD;
using System;
using System.Collections.Generic;

namespace Stefanini.Services
{
    public static class CuentaBancariaServices
    {
        static readonly ILog log = StefaniniExcepcion.GetLogger(typeof(CuentaBancariaServices));

        public static CuentaBancariasResponse ObtenerCuentasBancarias()
        {
            var mensajes = new List<Message>();

            try
            {
                // No tiene errores
                if (mensajes.Count == 0)
                {
                    List<CuentaBancaria> listCuentasBancarias = CuentaBancariaAccess.ObtenerCuentasBancarias();
                    List<CuentaBancariaModel> listCuentasModel = ConfigAutomapper.mapper.Map<List<CuentaBancariaModel>>(listCuentasBancarias);

                    mensajes.Add(new Message(Codes.Ok, listCuentasModel == null
                        ? Constant.RegistroConsultadoNoExiste
                        : Constant.RegistrosConsultadoCorrectamente));

                    return new CuentaBancariasResponse(listCuentasModel, true, mensajes);
                }
                else
                {
                    return new CuentaBancariasResponse(null, false, mensajes);
                }
            }
            catch (Exception ex)
            {
                new StefaniniExcepcion(ex.Message, ex, log);

                //Captura de errores
                mensajes.Add(new Message(Codes.Exception, ex.Message));
                return new CuentaBancariasResponse(null, false, mensajes);
            }
        }

    }
}