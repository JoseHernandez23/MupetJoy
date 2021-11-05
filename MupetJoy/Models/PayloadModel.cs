using System;
using System.Collections.Generic;
using System.Linq;
using System.Web; 

namespace MupetJoy.Models
{
    public class PayloadModel : BusinessPartnerModel
    {
        public string DocType { get; set; } //Item / Service
        public long? DocEntry { get; set; }
        public long? DocNum { get; set; }
        public double DocTotal { get; set; } //Total
        //public double? TransferSum { get; set; } //Total Payment
        public string U_PrescriptionID { get; set; }     
        public List<LinesInvoice> DocumentLines { get; set; }
        //public string Reference1 { get; set; } //PaymentRef
        public string CashAccount { get; set; }
        public double? CashSum { get; set; }
        //public double? CashSumSys { get; set; }
        public List<LineCreditCard> PaymentCreditCards { get; set; }
    }

}