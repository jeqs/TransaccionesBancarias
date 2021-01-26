using Stefanini.Transverse;
using StefaniniPruebaTecnica.DataStorage.BD;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stefanini.DataStorage.DataAccess
{
    public class CuentaBancariaAccess : BaseAccess<CuentaBancariaAccess>
    {
        public static List<CuentaBancaria> ObtenerCuentasBancarias()
        {
            try
            {
                using (var dbContextScope = new StefaniniDBEntities(false))
                {
                    return dbContextScope.CuentaBancaria.ToList();
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
