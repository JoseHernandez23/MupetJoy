using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MupetJoy.Models
{
	//Este msj es colocado desde la rama develop
    public class BusinessPartnerModel
    {
        public string CardCode { get; set; } 
        public string Series { get; set; }
        public string VatIDNum { get; set; }
        public string CardName { get; set; }
        public string CardType { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; } = null;
        public string EmailAddress { get; set; }
        public string Phone1 { get; set; }
        public int GroupCode { get; set; }
        public string U_Branch { get; set; }
        public string U_ReasonCustomer { get; set; }
        public string Currency { get; set; }
        public int PayTermsGrpCode { get; set; }
        public int PriceListNum { get; set; }
        //[Display(Name = "SurgeonName")]
        //public string U_SurgeonAfiliation { get; set; }
        public List<ShippingORBilling> BPAddresses { get; set; }
              
      
    }

    public class ShippingORBilling
    {
        public string AddressType { get; set; }
        public string AddressName { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string RowNum { get; set; } //AddressLine
        public string U_SlpCode { get; set; } //Sales employee
        public string Country { get; set; }
    }



    
}