//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StefaniniPruebaTecnica.DataStorage.BD
{
    using System;
    using System.Collections.Generic;
    
    public partial class Transaccion
    {
        public int Id { get; set; }
        public Nullable<int> CuentaBancariaId { get; set; }
        public Nullable<int> TipoTransaccionId { get; set; }
        public Nullable<decimal> Monto { get; set; }
        public Nullable<decimal> GMF { get; set; }
    
        public virtual CuentaBancaria CuentaBancaria { get; set; }
        public virtual TipoTransaccion TipoTransaccion { get; set; }
    }
}
