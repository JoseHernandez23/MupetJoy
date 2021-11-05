using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using MupetJoy.Controllers;
using MupetJoy.Models;
using MupetJoy.Security;
using Newtonsoft.Json;
using Novitec.Mupet.Common;
using Novitec.Mupet.Scenarios;
using Sap.Data.Hana;

namespace MupetJoy.Controllers
{
    /// <summary>
    /// Process Order Controller
    /// </summary>
    [Authorize]
    [RoutePrefix("api/po")]
    public class ProcessOrderController : ApiController
    {

        /*[HttpPost]
        [Route("PostProcessOrder")]
        public object PostProcessOrder(string Payload)
        {
            PayloadModel oPayload = null;
            BusinessPartnerController BP = new BusinessPartnerController();
            InvoiceController INV = new InvoiceController();
            IncomingPaymentsController PAY = new IncomingPaymentsController();
            BusinessPartnerModel oBP = null;
            InvoiceModel oINV = null;
            PaymentModel oPay = null;
            var Response = new ResponseProcessOrder();
            var BPartner = "";
            string Invoi = string.Empty;
            string IncomingPayment = string.Empty;

            oPayload = JsonConvert.DeserializeObject<PayloadModel>(Payload);
            bool ValidacionesNulos = ValidarDatosNulos(oPayload);


            if (ValidacionesNulos != false)
            {
                bool ValidacionesDatos = ValidarDatos(oPayload);
                if (ValidacionesDatos == false)
                {
                    Response = new ResponseProcessOrder
                    {
                        Error = "Error al Procesar la orden, datos erroneos o nulos",
                        BusinessPartner = "",
                        Invoice = "",
                        Payment = ""
                    };
                }
                else
                {
                    oBP = JsonConvert.DeserializeObject<BusinessPartnerModel>(Payload);
                    int VatIDNum = Convert.ToInt32(oBP.VatIDNum);
                    oBP.CardCode = GetCardCode(VatIDNum);

                    if (oBP.CardCode == "-1")
                    {
                        oBP.CardType = "C";
                        if (oBP.GroupCode == 103)
                        {
                            oBP.Series = CreateCardCode(oBP.GroupCode);

                            if (oBP.Series != "-1")
                            {
                                string OdataBP = JsonConvert.SerializeObject(oBP);
                                BPartner = BP.PostBusinessPartner(OdataBP);
                            }
                            else
                            {
                                BPartner = "-1";
                            }
                        }
                        else
                        {
                            BPartner = "-1";
                        }
                    }
                    else
                    {
                        string BussinessP = BP.GetBusinessPartner(oBP.CardCode);
                        if (!string.IsNullOrEmpty(BussinessP) && BussinessP != "-1")
                        {
                            BPartner = BussinessP;
                        }
                        else
                        {
                            Response = new ResponseProcessOrder
                            {
                                Error = "Error al crear el Business Partner",
                                BusinessPartner = "",
                                Invoice = "",
                                Payment = ""
                            };
                        }
                    }

                    if (!string.IsNullOrEmpty(BPartner) && BPartner != "-1")
                    {                       
                        oINV = JsonConvert.DeserializeObject<InvoiceModel>(Payload);
                        oINV.CardCode = BPartner;
                        oINV.DiscountPercent = Convert.ToDouble(ConfigurationSettings.AppSettings["DiscountPercent"]);  //****************Esto se debe determinar como configurar
                       
                        string OdataINV = JsonConvert.SerializeObject(oINV);
                        Invoi = INV.PostCreateInvoice(OdataINV);

                        if (Invoi != "-1")
                        {
                            oPay = JsonConvert.DeserializeObject<PaymentModel>(Payload);
                            oPay.CardCode = BPartner;

                            if (oPay.PaymentCreditCards != null && oPay.PaymentCreditCards.Count > 0)
                            {
                                for (int i = 0; i < oPay.PaymentCreditCards.Count; i++)
                                {
                                    if (oPay.PaymentCreditCards[i].CreditCard != null)
                                    {
                                        string FechaVenCreditCard = CambiarFormatoFechaVencimiento(oPay.PaymentCreditCards[i].CardValidUntil);
                                        oPay.PaymentCreditCards[i].CardValidUntil = FechaVenCreditCard;
                                    }
                                }
                            }

                            Response = new ResponseProcessOrder
                            {
                                Error = "",
                                BusinessPartner = BPartner,
                                Invoice = Invoi,
                                Payment = ""
                            };

                            string OdataPay = JsonConvert.SerializeObject(oPay);
                            IncomingPayment = PAY.PostIncomingPayment(OdataPay);

                            if (IncomingPayment != "-1")
                            {
                                Response = new ResponseProcessOrder
                                {
                                    Error = "",
                                    BusinessPartner = BPartner,
                                    Invoice = Invoi,
                                    Payment = IncomingPayment
                                };
                            }
                            else
                            {
                                Response = new ResponseProcessOrder
                                {
                                    Error = "Error al crear registrar pago",
                                    BusinessPartner = BPartner,
                                    Invoice = Invoi,
                                    Payment = ""
                                };
                            }


                        }
                        else
                        {
                            Response = new ResponseProcessOrder
                            {
                                Error = "Error al crear la factura",
                                BusinessPartner = BPartner,
                                Invoice = "",
                                Payment = ""
                            };
                        }

                    }
                    else
                    {
                        Response = new ResponseProcessOrder
                        {
                            Error = "Error al crear el Business Partner",
                            BusinessPartner = "",
                            Invoice = "",
                            Payment = ""
                        };
                    }

                }
            }
            else
            {
                Response = new ResponseProcessOrder
                {
                    Error = "Error al Procesar la orden, datos erroneos o nulos",
                    BusinessPartner = "",
                    Invoice = "",
                    Payment = ""
                };
            }
            var Resultado = JsonConvert.SerializeObject(Response);
            return Resultado;
        }*/

        [HttpPost]
        [Route("PostProcessOrder")]
        public object PostProcessOrder(string Payload)
        {
            GetConfigurableData Conf = new GetConfigurableData();
            PayloadModel oPayload = null;
            BusinessPartnerController BP = new BusinessPartnerController();
            InvoiceController INV = new InvoiceController();
            IncomingPaymentsController PAY = new IncomingPaymentsController();
            BusinessPartnerModel oBP = null;
            InvoiceModel oINV = null;
            PaymentModel oPay = null;
            var Response = new ResponseProcessOrder();
            var BPartner = "";
            string Invoi = string.Empty;
            string IncomingPayment = string.Empty;
            try
            {
                int? logEntryIdRet = null;                                             
                int? logEntryId = null;

                var result = new LogEntry()
                    .OverrideAction("Create Order Process")
                    .OverrideInput(new { Payload })
                    .Start<SimpleProcess>()
                    .OnProcessingBody((scenario) =>
                    {
                        logEntryId = scenario.LogEntryModelId;
                        oPayload = JsonConvert.DeserializeObject<PayloadModel>(Payload);
                        bool ValidacionesNulos = ValidarDatosNulos(oPayload);

                        if (ValidacionesNulos != false)
                        {
                            bool ValidacionesDatos = ValidarDatos(oPayload);
                            if (ValidacionesDatos == false)
                            {
                                Response = new ResponseProcessOrder
                                {
                                    Error = "Error al Procesar la orden",
                                    BusinessPartner = "",
                                    Invoice = "",
                                    Payment = ""
                                };
                            }
                            else
                            {
                                oBP = JsonConvert.DeserializeObject<BusinessPartnerModel>(Payload);
                                int VatIDNum = Convert.ToInt32(oBP.VatIDNum);
                                oBP.CardCode = GetCardCode(VatIDNum);

                                if (oBP.CardCode == "-1")
                                {
                                    oBP.CardType = "C";
                                    if (oBP.GroupCode == 103)
                                    {
                                        oBP.Series = CreateCardCode(oBP.GroupCode);

                                        if (oBP.Series != "-1")
                                        {
                                            string OdataBP = JsonConvert.SerializeObject(oBP);
                                            BPartner = BP.PostBusinessPartner(OdataBP);
                                        }
                                        else
                                        {
                                            BPartner = "-1";
                                        }
                                    }
                                    else
                                    {
                                        BPartner = "-1";
                                    }
                                }
                                else
                                {
                                    BusinessPartnerModel BussinessP = (BusinessPartnerModel)BP.GetBusinessPartner(oBP.CardCode);
                                    if (!string.IsNullOrEmpty(BussinessP.CardCode) && BussinessP.CardCode != "-1")
                                    {
                                        BPartner = BussinessP.CardCode;
                                    }
                                    else
                                    {
                                        Response = new ResponseProcessOrder
                                        {
                                            Error = "Error al crear el Business Partner",
                                            BusinessPartner = "",
                                            Invoice = "",
                                            Payment = ""
                                        };
                                    }
                                }

                                if (!string.IsNullOrEmpty(BPartner) && BPartner != "-1")
                                {
                                    oINV = JsonConvert.DeserializeObject<InvoiceModel>(Payload);
                                    oINV.CardCode = BPartner;
                                    oINV.DiscountPercent = Convert.ToDouble(Conf.GetDataConfigurationManager("DiscountPercent"));  //****************Esto se debe determinar como configurar

                                    string OdataINV = JsonConvert.SerializeObject(oINV);
                                    Invoi = INV.PostCreateInvoice(OdataINV);

                                    if (Invoi != "-1")
                                    {
                                        oPay = JsonConvert.DeserializeObject<PaymentModel>(Payload);
                                        oPay.CardCode = BPartner;

                                        if (oPay.PaymentCreditCards != null && oPay.PaymentCreditCards.Count > 0)
                                        {
                                            for (int i = 0; i < oPay.PaymentCreditCards.Count; i++)
                                            {
                                                if (oPay.PaymentCreditCards[i].CreditCard != null)
                                                {
                                                    string FechaVenCreditCard = CambiarFormatoFechaVencimiento(oPay.PaymentCreditCards[i].CardValidUntil);
                                                    oPay.PaymentCreditCards[i].CardValidUntil = FechaVenCreditCard;
                                                }
                                            }
                                        }

                                        Response = new ResponseProcessOrder
                                        {
                                            Error = "",
                                            BusinessPartner = BPartner,
                                            Invoice = Invoi,
                                            Payment = ""
                                        };

                                        string OdataPay = JsonConvert.SerializeObject(oPay);
                                        IncomingPayment = PAY.PostIncomingPayment(OdataPay);

                                        if (IncomingPayment != "-1")
                                        {
                                            Response = new ResponseProcessOrder
                                            {
                                                Error = "",
                                                BusinessPartner = BPartner,
                                                Invoice = Invoi,
                                                Payment = IncomingPayment
                                            };
                                        }
                                        else
                                        {
                                            Response = new ResponseProcessOrder
                                            {
                                                Error = "Error al crear registrar pago",
                                                BusinessPartner = BPartner,
                                                Invoice = Invoi,
                                                Payment = ""
                                            };
                                        }
                                    }
                                    else
                                    {
                                        Response = new ResponseProcessOrder
                                        {
                                            Error = "Error al crear la factura",
                                            BusinessPartner = BPartner,
                                            Invoice = "",
                                            Payment = ""
                                        };
                                    }
                                }
                                else
                                {
                                    Response = new ResponseProcessOrder
                                    {
                                        Error = "Error al crear el Business Partner",
                                        BusinessPartner = "",
                                        Invoice = "",
                                        Payment = ""
                                    };
                                }

                            }
                        }
                        else
                        {
                            Response = new ResponseProcessOrder
                            {
                                Error = "Error al Procesar la orden",
                                BusinessPartner = "",
                                Invoice = "",
                                Payment = ""
                            };
                        }
                        NVTResult res = null;
                        var Resultado = JsonConvert.SerializeObject(Response);
                        res = new NVTResult(new { Response.Error, Response.BusinessPartner, Response.Invoice, Response.Payment }, null);
                        return res;

                    })
                    .OnBeforeFinish((scenario, nvtResult) =>
                    {
                        if (!nvtResult.Success)
                        {
                            if (Response.BusinessPartner == string.Empty)
                            {
                                return new NVTResult("Ocurrio un error al crear la orden. " + nvtResult.Error);
                            }
                            return new NVTResult("Ocurrio un error al crear la orden");
                        }
                        return nvtResult;
                    })
                    .Finish();

                logEntryIdRet = logEntryId;

                if (result.Success)
                {
                    var Resultado = JsonConvert.SerializeObject(Response);
                    return Resultado;
                }
                else
                {
                    Response = new ResponseProcessOrder
                    {
                        Error = "Error al Procesar la orden",
                        BusinessPartner = "",
                        Invoice = "",
                        Payment = ""
                    };

                    var Resultado = JsonConvert.SerializeObject(Response);
                    return Resultado;
                }
            }
            catch (Exception ex)
            {
                Response = new ResponseProcessOrder
                {
                    Error = "Error al Procesar la orden " + ex.Message,
                    BusinessPartner = "",
                    Invoice = "",
                    Payment = ""
                };

                var Resultado = JsonConvert.SerializeObject(Response);
                return Resultado;
            }


        }

        private bool ValidarDatosNulos(PayloadModel datos)
        {
            bool resultado = true;
            List<string> ListaErrores = new List<string>();

            //if (string.IsNullOrEmpty(datos.CardCode))
            //{
            //    ListaErrores.Add("CardCode no puede ser nulo" + "--->" + datos.CardCode);
            //}
            if (string.IsNullOrEmpty(datos.VatIDNum.ToString()))
            {
                ListaErrores.Add("VatIDNum no puede ser nulo" + "--->" + datos.VatIDNum);
            }
            if (string.IsNullOrEmpty(datos.CardName))
            {
                ListaErrores.Add("CardName no puede ser nulo" + "--->" + datos.CardName);
            }
            //if (string.IsNullOrEmpty(datos.CardType))
            //{
            //    ListaErrores.Add("CardType no puede ser nulo" + "--->" + datos.CardType);
            //}
            //if (string.IsNullOrEmpty(datos.Address))
            //{
            //    ListaErrores.Add("Address no puede ser nulo" + "--->" + datos.Address);
            //}
            //if (string.IsNullOrEmpty(datos.ContactPerson))
            //{
            //    ListaErrores.Add("ContactPerson no puede ser nulo" + "--->" + datos.ContactPerson);
            //}
            //if (string.IsNullOrEmpty(datos.EmailAddress))
            //{
            //    ListaErrores.Add("EmailAddress no puede ser nulo" + "--->" + datos.EmailAddress);
            //}
            //if (string.IsNullOrEmpty(datos.Phone1))
            //{
            //    ListaErrores.Add("Phone1 no puede ser nulo" + "--->" + datos.Phone1);
            //}
            if (string.IsNullOrEmpty(datos.GroupCode.ToString()))
            {
                ListaErrores.Add("GroupCode no puede ser nulo" + "--->" + datos.GroupCode);
            }
            if (string.IsNullOrEmpty(datos.U_Branch))
            {
                ListaErrores.Add("U_Branch no puede ser nulo" + "--->" + datos.U_Branch);
            }
            if (string.IsNullOrEmpty(datos.U_ReasonCustomer))
            {
                ListaErrores.Add("U_ReasonCustomer no puede ser nulo" + "--->" + datos.U_ReasonCustomer);
            }
            if (string.IsNullOrEmpty(datos.Currency))
            {
                ListaErrores.Add("Currency no puede ser nulo" + "--->" + datos.Currency);
            }
            if (string.IsNullOrEmpty(datos.PayTermsGrpCode.ToString()))
            {
                ListaErrores.Add("PayTermsGrpCode no puede ser nulo" + "--->" + datos.PayTermsGrpCode);
            }
            if (string.IsNullOrEmpty(datos.PriceListNum.ToString()))
            {
                ListaErrores.Add("PriceListNum no puede ser nulo" + "--->" + datos.PriceListNum);
            }
            if (string.IsNullOrEmpty(datos.U_SurgeonAfiliation))
            {
                ListaErrores.Add("U_SurgeonAfiliation no puede ser nulo" + "--->" + datos.U_SurgeonAfiliation);
            }
            if (string.IsNullOrEmpty(datos.DocType))
            {
                ListaErrores.Add("DocType no puede ser nulo" + "--->" + datos.DocType);
            }
            //if (string.IsNullOrEmpty(datos.Dscription))
            //{
            //    ListaErrores.Add("Description no puede ser nulo" + "--->" + datos.Dscription);
            //}
            //if (string.IsNullOrEmpty(datos.AcctCode))
            //{
            //    ListaErrores.Add("AcctCode no puede ser nulo" + "--->" + datos.AcctCode);
            //}
            //if (string.IsNullOrEmpty(datos.OcrCode))
            //{
            //    ListaErrores.Add("OcrCode no puede ser nulo" + "--->" + datos.OcrCode);
            //}
            //if (string.IsNullOrEmpty(datos.TaxCode))
            //{
            //    ListaErrores.Add("TaxCode no puede ser nulo" + "--->" + datos.TaxCode);
            //}
            if (string.IsNullOrEmpty(datos.DocTotal.ToString()))
            {
                ListaErrores.Add("DocTotal no puede ser nulo" + "--->" + datos.DocTotal);
            }
            if (string.IsNullOrEmpty(datos.U_PrescriptionID))
            {
                ListaErrores.Add("U_PrescriptionID no puede ser nulo" + "--->" + datos.U_PrescriptionID);
            }
            //if (string.IsNullOrEmpty(datos.Reference1))
            //{
            //    ListaErrores.Add("Reference1 no puede ser nulo" + "--->" + datos.Reference1);
            //}
            //if (string.IsNullOrEmpty(datos.TotalPayment.ToString()))
            //{
            //    ListaErrores.Add("TotalPayment no puede ser nulo" + "--->" + datos.TotalPayment);
            //}
            //if (string.IsNullOrEmpty(datos.CashAccount))
            //{
            //    ListaErrores.Add("CashAccount no puede ser nulo" + "--->" + datos.CashAccount);
            //}

            for (int i = 0; i < datos.BPAddresses.Count; i++)
            {
                if (string.IsNullOrEmpty(datos.BPAddresses[i].AddressType))
                {
                    ListaErrores.Add("AddressType no puede ser nulo" + "--->" + datos.BPAddresses[i].AddressType);
                }
                if (string.IsNullOrEmpty(datos.BPAddresses[i].AddressName))
                {
                    ListaErrores.Add("AddressName no puede ser nulo" + "--->" + datos.BPAddresses[i].AddressName);
                }
                if (string.IsNullOrEmpty(datos.BPAddresses[i].City))
                {
                    ListaErrores.Add("City no puede ser nulo" + "--->" + datos.BPAddresses[i].City);
                }
                if (string.IsNullOrEmpty(datos.BPAddresses[i].ZipCode))
                {
                    ListaErrores.Add("ZipCode no puede ser nulo" + "--->" + datos.BPAddresses[i].ZipCode);
                }
                if (string.IsNullOrEmpty(datos.BPAddresses[i].RowNum))
                {
                    ListaErrores.Add("RowNum no puede ser nulo" + "--->" + datos.BPAddresses[i].RowNum);
                }
                if (string.IsNullOrEmpty(datos.BPAddresses[i].U_SlpCode))
                {
                    ListaErrores.Add("U_SlpCode no puede ser nulo" + "--->" + datos.BPAddresses[i].U_SlpCode);
                }
                if (string.IsNullOrEmpty(datos.BPAddresses[i].Country))
                {
                    ListaErrores.Add("Country no puede ser nulo" + "--->" + datos.BPAddresses[i].Country);
                }
            }
            for (int j = 0; j < datos.DocumentLines.Count; j++)
            {
                if (string.IsNullOrEmpty(datos.DocumentLines[j].ItemDescription))
                {
                    ListaErrores.Add("ItemDescription no puede ser nulo" + "--->" + datos.DocumentLines[j].ItemDescription);
                }
                if (string.IsNullOrEmpty(datos.DocumentLines[j].AccountCode))
                {
                    ListaErrores.Add("AccountCode no puede ser nulo" + "--->" + datos.DocumentLines[j].AccountCode);
                }
                if (string.IsNullOrEmpty(datos.DocumentLines[j].CostingCode))
                {
                    ListaErrores.Add("CostingCode no puede ser nulo" + "--->" + datos.DocumentLines[j].CostingCode);
                }
                if (string.IsNullOrEmpty(datos.DocumentLines[j].VatGroup))
                {
                    ListaErrores.Add("VatGroup no puede ser nulo" + "--->" + datos.DocumentLines[j].VatGroup);
                }
            }

            if (ListaErrores.Count > 0)
            {
                resultado = false;
            }
            return resultado;
        }

        private bool ValidarDatos(PayloadModel datos)
        {
            bool resultado = true;
            string rgxAlfaNum = @"^[A-Za-z0-9\s-_]+$";
            string rgxEmail = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            string rgxNombre = @"^([a-zA-Z]{2,}\s[a-zA-z]{1,}'?-?[a-zA-Z]{2,}\s?([a-zA-Z]{1,})?)";
            string rgxLetras = @"^[a-zA-Z]+$";
            //string rgxNumeros = @"/^([0-9])*$/";
            Regex rgxNumeros = new Regex(@"^[0-9]+$");
            Regex rgxCifrasDecimales = new Regex(@"^(:?[\d,]+\.)*\d+$");
            //string rgxCifrasDecimales = @"^(:?[\d,]+\.)*\d+$";
            //Regex rgxNumeros = new Regex(@"^-?[0-9][0-9,\.]+$");

            List<string> ListaErrores = new List<string>();

            var cultureInfo = CultureInfo.InvariantCulture;
            NumberStyles styles = NumberStyles.Number;

            //if (!Regex.IsMatch(datos.CardCode, rgxAlfaNum))
            //{
            //    ListaErrores.Add("CardCode" + "--->" + datos.CardCode);
            //}
            if (!rgxNumeros.IsMatch(Convert.ToString(datos.VatIDNum)))
            {
                ListaErrores.Add("VatIDNum" + "--->" + datos.VatIDNum);
            }
            if (!Regex.IsMatch(datos.CardName, rgxAlfaNum))
            {
                ListaErrores.Add("CardName" + "--->" + datos.CardName);
            }
            //if (!Regex.IsMatch(datos.CardType, rgxLetras))
            //{
            //    ListaErrores.Add("CardType" + "--->" + datos.CardType);
            //}
            //if (!Regex.IsMatch(datos.Address, rgxAlfaNum))
            //{
            //    ListaErrores.Add("Address" + "--->" + datos.Address);
            //}
            //if (!Regex.IsMatch(datos.ContactPerson, rgxNombre))
            //{
            //    ListaErrores.Add("ContactPerson" + "--->" + datos.ContactPerson);
            //}
            //if (!Regex.IsMatch(datos.EmailAddress, rgxEmail))
            //{
            //    ListaErrores.Add("EmailAddress" + "--->" + datos.EmailAddress);
            //}
            //if (!Regex.IsMatch(datos.Phone1, rgxAlfaNum))
            //{
            //    ListaErrores.Add("Phone1" + "--->" + datos.Phone1);
            //}
            if (!rgxNumeros.IsMatch(Convert.ToString(datos.GroupCode)))
            {
                ListaErrores.Add("GroupCode" + "--->" + datos.GroupCode);
            }
            if (!Regex.IsMatch(datos.U_Branch, rgxAlfaNum))
            {
                ListaErrores.Add("U_Branch" + "--->" + datos.U_Branch);
            }
            if (!Regex.IsMatch(datos.U_ReasonCustomer, rgxAlfaNum))
            {
                ListaErrores.Add("U_ReasonCustomer" + "--->" + datos.U_ReasonCustomer);
            }
            if (!Regex.IsMatch(datos.Currency, rgxAlfaNum))
            {
                ListaErrores.Add("Currency" + "--->" + datos.Currency);
            }
            if (!rgxNumeros.IsMatch(Convert.ToString(datos.PayTermsGrpCode)))
            {
                ListaErrores.Add("PayTermsGrpCode" + "--->" + datos.PayTermsGrpCode);
            }
            if (!rgxNumeros.IsMatch(Convert.ToString(datos.PriceListNum)))
            {
                ListaErrores.Add("PriceListNum" + "--->" + datos.PriceListNum);
            }
            if (!Regex.IsMatch(datos.U_SurgeonAfiliation, rgxAlfaNum))
            {
                ListaErrores.Add("U_SurgeonAfiliation" + "--->" + datos.U_SurgeonAfiliation);
            }
            if (!Regex.IsMatch(datos.DocType, rgxAlfaNum))
            {
                ListaErrores.Add("DocType" + "--->" + datos.DocType);
            }
            //if (!Regex.IsMatch(datos.Dscription, rgxAlfaNum))
            //{
            //    ListaErrores.Add("Dscription" + "--->" + datos.Dscription);
            //}
            //if (!Regex.IsMatch(datos.AcctCode, rgxAlfaNum))
            //{
            //    ListaErrores.Add("AcctCode" + "--->" + datos.AcctCode);
            //}
            //if (!Regex.IsMatch(datos.OcrCode, rgxAlfaNum))
            //{
            //    ListaErrores.Add("OcrCode" + "--->" + datos.OcrCode);
            //}
            //if (!Regex.IsMatch(datos.TaxCode, rgxAlfaNum))
            //{
            //    ListaErrores.Add("VatGroup" + "--->" + datos.TaxCode);
            //}
            if (!rgxCifrasDecimales.IsMatch(Convert.ToString(datos.DocTotal)))
            {
                ListaErrores.Add("DocTotal" + "--->" + datos.DocTotal);
            }
            else
            {
                double Total = 0;
                cultureInfo = new CultureInfo("en-IT");
                if (double.TryParse(datos.DocTotal.ToString(), styles, cultureInfo, out Total))
                {
                    datos.DocTotal = Total;
                }
            }
            if (!Regex.IsMatch(datos.U_PrescriptionID, rgxAlfaNum))
            {
                ListaErrores.Add("PrescriptionID" + "--->" + datos.U_PrescriptionID);
            }
            //if (!Regex.IsMatch(datos.Reference1, rgxAlfaNum))
            //{
            //    ListaErrores.Add("Reference1" + "--->" + datos.Reference1);
            //}
            //if (!rgxCifrasDecimales.IsMatch(Convert.ToString(datos.TotalPayment)))
            //{
            //    ListaErrores.Add("TotalPayment" + "--->" + datos.TotalPayment);
            //}
            //else
            //{
            //    double Total_TotalPayment = 0;
            //    cultureInfo = new CultureInfo("en-IT");
            //    if (double.TryParse(datos.TotalPayment.ToString(), styles, cultureInfo, out Total_TotalPayment))
            //    {
            //        datos.TotalPayment = Total_TotalPayment;
            //    }
            //}
            //if (!Regex.IsMatch(datos.CashAccount, rgxAlfaNum))
            //{
            //    ListaErrores.Add("CashAccount" + "--->" + datos.CashAccount);
            //}

            for (int i = 0; i < datos.BPAddresses.Count; i++)
            {
                if (!Regex.IsMatch(datos.BPAddresses[i].AddressType, rgxAlfaNum))
                {
                    ListaErrores.Add("AddressType" + "--->" + datos.BPAddresses[i].AddressType);
                }
                if (!Regex.IsMatch(datos.BPAddresses[i].AddressName, rgxAlfaNum))
                {
                    ListaErrores.Add("AddressName" + "--->" + datos.BPAddresses[i].AddressName);
                }
                if (!Regex.IsMatch(datos.BPAddresses[i].City, rgxAlfaNum))
                {
                    ListaErrores.Add("City" + "--->" + datos.BPAddresses[i].City);
                }
                if (!Regex.IsMatch(datos.BPAddresses[i].ZipCode, rgxAlfaNum))
                {
                    ListaErrores.Add("ZipCode" + "--->" + datos.BPAddresses[i].ZipCode);
                }
                if (!Regex.IsMatch(datos.BPAddresses[i].RowNum, rgxAlfaNum))
                {
                    ListaErrores.Add("RowNum" + "--->" + datos.BPAddresses[i].RowNum);
                }
                if (!Regex.IsMatch(datos.BPAddresses[i].U_SlpCode, rgxAlfaNum))
                {
                    ListaErrores.Add("U_SlpCode" + "--->" + datos.BPAddresses[i].U_SlpCode);
                }
                if (!Regex.IsMatch(datos.BPAddresses[i].Country, rgxAlfaNum))
                {
                    ListaErrores.Add("Country" + "--->" + datos.BPAddresses[i].Country);
                }
            }

            for (int j = 0; j < datos.DocumentLines.Count; j++)
            {
                if (!Regex.IsMatch(datos.DocumentLines[j].ItemDescription, rgxAlfaNum))
                {
                    ListaErrores.Add("ItemDescription" + "--->" + datos.DocumentLines[j].ItemDescription);
                }
                if (!Regex.IsMatch(datos.DocumentLines[j].AccountCode, rgxAlfaNum))
                {
                    ListaErrores.Add("AccountCode" + "--->" + datos.DocumentLines[j].AccountCode);
                }
                if (!Regex.IsMatch(datos.DocumentLines[j].CostingCode, rgxAlfaNum))
                {
                    ListaErrores.Add("CostingCode" + "--->" + datos.DocumentLines[j].CostingCode);
                }
                if (!Regex.IsMatch(datos.DocumentLines[j].VatGroup, rgxAlfaNum))
                {
                    ListaErrores.Add("VatGroup" + "--->" + datos.DocumentLines[j].VatGroup);
                }
            }

            if (ListaErrores.Count > 0)
            {
                resultado = false;
            }
            return resultado;
        }

        private string CreateCardCode(int code)
        {
            string respuesta = string.Empty;
            GetConfigurableData Conf = new GetConfigurableData();
            try
            {
                string cnx = Conf.GetDataConfigurationManager("CNX");

                using (HanaConnection connection = new HanaConnection(cnx))
                {

                    string SP = Properties.Resources.SPObtenerCardCode;

                    using (HanaCommand command = new HanaCommand(SP, connection))
                    {
                        connection.Open();
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandText = SP;
                        command.Parameters.AddWithValue("groupCode", code);

                        using (HanaDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                //respuesta = reader.GetString(1) + reader.GetString(6).PadLeft(5, '0');
                                respuesta = reader.GetString(0);
                            }
                        }
                        if (string.IsNullOrEmpty(respuesta))
                        {
                            respuesta = "-1";
                        }
                    }
                }
                return respuesta;
            }
            catch (SqlException e)
            {
                respuesta = "-1";
                return respuesta;
            }
        }

        private string GetCardCode(int VatID)
        {
            string respuesta = string.Empty;
            GetConfigurableData Conf = new Security.GetConfigurableData();
            try
            {
                string cnx = Conf.GetDataConfigurationManager("CNX");

                using (HanaConnection connection = new HanaConnection(cnx))
                {

                    string BDCountry = Conf.GetDataConfigurationManager("BDCountry");
                    string Query = Properties.Resources.GetCardCode.Replace("{BaseDeDatos}", BDCountry).Replace("@VatID", VatID.ToString());

                    using (HanaCommand command = new HanaCommand(Query, connection))
                    {
                        connection.Open();
                        command.CommandType = System.Data.CommandType.Text;
                        command.CommandText = Query;

                        using (HanaDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                respuesta = reader.GetString(0);
                            }
                        }
                        if (string.IsNullOrEmpty(respuesta))
                        {
                            respuesta = "-1";
                        }
                    }
                }
                return respuesta;
            }
            catch (SqlException e)
            {
                respuesta = "-1";
                return respuesta;
            }
        }

        private string CambiarFormatoFechaVencimiento(string FechaVencCardCredit)
        {
            string resultado = string.Empty;
            string mes = FechaVencCardCredit.Substring(0, 2);
            string PartOfyear = FechaVencCardCredit.Substring(3, 2);
            int year = 2000 + Convert.ToInt32(PartOfyear);

            switch (mes)
            {
                case "01":
                    resultado = year.ToString() + "-" + mes + "-" + "31";
                    break;

                case "02":
                    resultado = year.ToString() + "-" + mes + "-" + "28";
                    break;

                case "03":
                    resultado = year.ToString() + "-" + mes + "-" + "31";
                    break;
                case "04":
                    resultado = year.ToString() + "-" + mes + "-" + "30";
                    break;

                case "05":
                    resultado = year.ToString() + "-" + mes + "31";
                    break;

                case "06":
                    resultado = year.ToString() + "-" + mes + "-" + "30";
                    break;

                case "07":
                    resultado = year.ToString() + "-" + mes + "-" + "31";
                    break;

                case "08":
                    resultado = year.ToString() + "-" + mes + "-" + "31";
                    break;

                case "09":
                    resultado = year.ToString() + "-" + mes + "-" + "30";
                    break;

                case "10":
                    resultado = year.ToString() + "-" + mes + "-" + "31";
                    break;

                case "11":
                    resultado = year.ToString() + "-" + mes + "-" + "30";
                    break;

                case "12":
                    resultado = year.ToString() + "-" + mes + "-" + "31";
                    break;

                default:
                    resultado = string.Empty;
                    break;
            }
            return resultado;
        }

      
    }
}