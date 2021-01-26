using Stefanini.Transverse;
using StefaniniPruebaTecnica.DataStorage.BD;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stefanini.DataStorage.DataAccess
{
    public class TransaccionAccess : BaseAccess<TransaccionAccess>
    {
        public static List<Transacciones> ObtenerTransacciones()
        {
            try
            {
                using (var dbContextScope = new StefaniniDBEntities(false))
                {
                    return dbContextScope.ObtenerTransacciones().ToList();
                }
            }
            catch (Exception ex)
            {
                new StefaniniExcepcion(ex.Message, ex, log);
                throw ex;
            }
        }

        public static Transaccion ObtenerTransaccion(int cuentaBancariaId)
        {
            try
            {
                using (var dbContextScope = new StefaniniDBEntities(false))
                {
                    return dbContextScope.Transaccion.Where(x => x.CuentaBancariaId == cuentaBancariaId).OrderByDescending(x => x.Id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                new StefaniniExcepcion(ex.Message, ex, log);
                throw ex;
            }
        }

        public static bool RegistrarTransaccion(Transaccion transaccion)
        {
            try
            {
                using (var dbContextScope = new StefaniniDBEntities(false))
                {
                    dbContextScope.Transaccion.Add(transaccion);
                    dbContextScope.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                new StefaniniExcepcion(ex.Message, ex, log);
                throw ex;
            }
        }

    }
}
