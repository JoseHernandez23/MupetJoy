using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MupetJoy.Models
{
    public class PaymentModel
    {
        public string CardCode { get; set; }
        //public string Reference1 { get; set; } //PaymentRef
        public string CashAccount { get; set; } // Account para pago contado
        //public double? TotalPayment { get; set; } //Total Payment        
        public string U_PrescriptionID { get; set; }
        public long? DocNum { get; set; }
        public double? CashSum { get; set; }
        //public double? CashSumSys { get; set; }
        public List<LineCreditCard> PaymentCreditCards { get; set; }
    }

    public class LineCreditCard
    {
        public int? CreditCard { get; set; }
        //public string CreditAcct { get; set; }
        public string CreditCardNumber { get; set; }
        public string CardValidUntil { get; set; }      
        public double? CreditSum { get; set; }
        public string CreditCur { get; set; }
        public string VoucherNum { get; set; }
    }
}