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
    public static class TransaccionServices
    {
        static readonly ILog log = StefaniniExcepcion.GetLogger(typeof(CuentaBancariaServices));

        public static TransaccionesResponse ObtenerTransferencias()
        {
            var mensajes = new List<Message>();

            try
            {
                var listaTransacciones = TransaccionAccess.ObtenerTransacciones();
                var transacciones = ConfigAutomapper.mapper.Map<List<TransaccionModel>>(listaTransacciones);

                return new TransaccionesResponse(transacciones, true, mensajes);
            }
            catch (Exception ex)
            {
                new StefaniniExcepcion(ex.Message, ex, log);

                //Captura de errores
                mensajes.Add(new Message(Codes.Exception, ex.Message));
                return new TransaccionesResponse(null, false, mensajes);
            }
        }

        public static TransaccionResponse Transferir(TransaccionModel request)
        {
            var mensajes = new List<Message>();

            try
            {

                if (request.CuentaBancariaId == null)
                {
                    mensajes.Add(new Message(Codes.Mandatory, "La Cuenta Bancaria de Origen es obligatoria"));
                }

                if (request.CuentaBancariaDestinoId == null)
                {
                    mensajes.Add(new Message(Codes.Mandatory, "La Cuenta Bancaria de Destino es obligatoria"));
                }

                if (request.Monto == null)
                {
                    mensajes.Add(new Message(Codes.Mandatory, "El Monto es obligatorio"));
                }
                else
                {
                    if (request.Monto < 1000)
                    {
                        mensajes.Add(new Message(Codes.Mandatory, "El monto a transferir debe ser mayor o Igual a 1.000"));
                    }
                }

                // No tiene errores
                if (mensajes.Count == 0)
                {
                    var transaccionAnterior = TransaccionAccess.ObtenerTransaccion((int)request.CuentaBancariaId);
                    var transaccion = ConfigAutomapper.mapper.Map<Transaccion>(request);

                    if (transaccion.Monto <= transaccionAnterior.Monto)
                    {
                        // Retiro
                        transaccion.Monto = transaccionAnterior.Monto - transaccion.Monto;
                        transaccion.TipoTransaccionId = 2;
                        TransaccionAccess.RegistrarTransaccion(transaccion);

                        // GMF
                        transaccion.GMF = (request.Monto * 4) / 1000;
                        transaccion.Monto = transaccion.Monto - transaccion.GMF;
                        transaccion.TipoTransaccionId = 4;
                        TransaccionAccess.RegistrarTransaccion(transaccion);

                        // Transferencia
                        var transaccionDestino = TransaccionAccess.ObtenerTransaccion((int)request.CuentaBancariaDestinoId);
                        transaccionDestino.Monto = transaccionDestino.Monto + request.Monto;
                        transaccion.TipoTransaccionId = 1;
                        var resultadoTX = TransaccionAccess.RegistrarTransaccion(transaccionDestino);

                        mensajes.Add(new Message(Codes.Ok, "Transferencia realizada con Éxito"));
                    }
                    else
                    {
                        string msj = string.Format("No se pudo realizar la Transacción, el monto a Transferir supera al saldo total de {0}", transaccionAnterior.Monto.ToString());
                        mensajes.Add(new Message(Codes.Mandatory, msj));
                    }

                    return new TransaccionResponse(request, true, mensajes);
                }
                else
                {
                    return new TransaccionResponse(null, false, mensajes);
                }
            }
            catch (Exception ex)
            {
                new StefaniniExcepcion(ex.Message, ex, log);

                //Captura de errores
                mensajes.Add(new Message(Codes.Exception, ex.Message));
                return new TransaccionResponse(null, false, mensajes);
            }
        }

        public static TransaccionResponse Retirar(TransaccionModel request)
        {
            var mensajes = new List<Message>();

            try
            {

                if (request.CuentaBancariaId == null)
                {
                    mensajes.Add(new Message(Codes.Mandatory, "La Cuenta Bancaria de Origen es obligatoria"));
                }

                if (request.Monto == null)
                {
                    mensajes.Add(new Message(Codes.Mandatory, "El Monto es obligatorio"));
                }
                else
                {
                    if (request.Monto < 1000)
                    {
                        mensajes.Add(new Message(Codes.Mandatory, "El monto a transferir debe ser mayor o Igual a 1.000"));
                    }
                }

                // No tiene errores
                if (mensajes.Count == 0)
                {
                    var transaccionAnterior = TransaccionAccess.ObtenerTransaccion((int)request.CuentaBancariaId);
                    var transaccion = ConfigAutomapper.mapper.Map<Transaccion>(request);

                    if (transaccion.Monto <= transaccionAnterior.Monto)
                    {
                        // Retiro
                        transaccion.Monto = transaccionAnterior.Monto - transaccion.Monto;
                        transaccion.TipoTransaccionId = 2;
                        TransaccionAccess.RegistrarTransaccion(transaccion);

                        // GMF
                        transaccion.GMF = (request.Monto * 4) / 1000;
                        transaccion.Monto = transaccion.Monto - transaccion.GMF;
                        transaccion.TipoTransaccionId = 4;
                        TransaccionAccess.RegistrarTransaccion(transaccion);

                        mensajes.Add(new Message(Codes.Ok, "Retiro realizado con Éxito"));
                    }
                    else
                    {
                        string msj = string.Format("No se pudo realizar el Retiro, el monto a Retirar supera al saldo total de {0}", transaccionAnterior.Monto.ToString());
                        mensajes.Add(new Message(Codes.Mandatory, msj));
                    }

                    return new TransaccionResponse(request, true, mensajes);
                }
                else
                {
                    return new TransaccionResponse(null, false, mensajes);
                }
            }
            catch (Exception ex)
            {
                new StefaniniExcepcion(ex.Message, ex, log);

                //Captura de errores
                mensajes.Add(new Message(Codes.Exception, ex.Message));
                return new TransaccionResponse(null, false, mensajes);
            }
        }

    }
}