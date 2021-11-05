using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MupetJoy.Models
{
    public class InvoiceModel
    {
        public string CardCode { get; set; }
        public long? DocEntry { get; set; }
        public long? DocNum { get; set; }
        public string DocType { get; set; } //Item / Service      
        //public double DocTotal { get; set; } //Total
        //public double LineTotal { get; set; }
        public string U_PrescriptionID { get; set; }
        public double? DiscountPercent { get; set; }
        //public double? TotalDiscount { get; set; }
        //public double? TotalDiscountSC { get; set; }     
        public List<LinesInvoice> DocumentLines { get; set; }
    }

    public class LinesInvoice
    {
        public string ItemDescription { get; set; } //Descripcion
        public string AccountCode { get; set; } //GL Account
        public string CostingCode { get; set; } //Dist Rule
        public string VatGroup { get; set; } //TaxCode
        //public double UnitPrice { get; set; }
        public double LineTotal { get; set; }
        //public double? U_B1SYS_Discount { get; set; }
        //public double? U_B1SYS_Discount_SC { get; set; }

    }
}