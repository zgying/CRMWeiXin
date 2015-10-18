/*  
 *  Copyright © 2012 Matthew David Elgert - mdelgert@yahoo.com
 *
 *  This program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation; either version 2.1 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 *
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with this program; if not, write to the Free Software
 *  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA 
 * 
 *  URLS
 *  https://cms.paypal.com/us/cgi-bin/?&cmd=_render-content&content_ID=developer/library_code_ipn_code_samples
 *  https://cms.paypal.com/cms_content/US/en_US/files/developer/IPN_ASP_NET_C.txt
 *  https://cms.paypal.com/us/cgi-bin/?cmd=_render-content&content_ID=developer/e_howto_admin_IPNIntro
 *  http://stackoverflow.com/questions/309421/can-you-force-paypal-payments-standard-api-to-show-credit-card-fields-first --force PayPal Payments Standard API to show credit card fields first?
 *  Project URL: http://jquerymobile.codeplex.com/                           
 *  
 */

using System;
using System.Web;
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Net;
using DAL;

namespace BLL
{
    public static class IPN
    {

        public static void ProccessIPN()
        {

            try
            {
                string PayPalSubmitUrl = ConfigurationManager.AppSettings.Get("PayPalSubmitUrl");
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(PayPalSubmitUrl);

                //Set values for the request back
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                byte[] param = HttpContext.Current.Request.BinaryRead(HttpContext.Current.Request.ContentLength);
                string strRequest = Encoding.ASCII.GetString(param);
                strRequest += "&cmd=_notify-validate";
                req.ContentLength = strRequest.Length;

                //for proxy
                //WebProxy proxy = new WebProxy(new Uri("http://url:port#"));
                //req.Proxy = proxy;

                //Send the request to PayPal and get the response
                using (StreamWriter streamOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII))
                {
                    streamOut.Write(strRequest);
                    streamOut.Close();
                }

                using (StreamReader streamIn = new StreamReader(req.GetResponse().GetResponseStream()))
                {
                    string strResponse = streamIn.ReadToEnd();
                    streamIn.Close();
                    if (strResponse == "VERIFIED")
                    {
                        //Store the response in the DB
                        BLL.IPN.InsertResponse(strRequest);

                        //check the payment_status is Completed
                        //check that txn_id has not been previously processed
                        //check that receiver_email is your Primary PayPal email
                        //check that payment_amount/payment_currency are correct
                        //process payment                        
                    }
                    else
                        if (strResponse == "INVALID")
                        {
                            //log for manual investigation
                            ElmahExtension.LogToElmah(new ApplicationException("RECEIVED INVALID RESPONSE FROM PAYPAL IPN"));
                        }
                        else
                        {
                            //log response/ipn data for manual investigation
                            ElmahExtension.LogToElmah(new ApplicationException("RECEIVED THE FOLLOWING INVALID RESPONSE FROM PAYPAL IPN " + strResponse));
                        }
                }

            }
            catch (Exception ex)
            {
                ElmahExtension.LogToElmah(ex);
            }


        }

        public static void InsertResponse(string ResponseIN)
        {

            if (ConfigurationManager.AppSettings.Get("PayPalSubmitUrl") != null)
            {

                try
                {
                    string Environment = null;

                    if (ConfigurationManager.AppSettings.Get("PayPalSubmitUrl") == "https://www.sandbox.paypal.com/cgi-bin/webscr")
                    {
                        Environment = "PayPalSandBox";
                    }

                    if (ConfigurationManager.AppSettings.Get("PayPalSubmitUrl") == "https://www.paypal.com/cgi-bin/webscr")
                    {
                        Environment = "PayPalProduction";
                    }

                    using (EntitiesContext context = new EntitiesContext())
                    {
                        DAL.IPN I = new DAL.IPN
                        {
                            CreateDate = DateTime.Now,
                            Environment = Environment,
                            ResponseIN = ResponseIN
                        };
                        context.IPNs.AddObject(I);
                        context.SaveChanges();
                        ParseResponse(ResponseIN, I.IPNID);
                    }
                }
                catch (Exception ex)
                {
                    ElmahExtension.LogToElmah(ex);
                }
            }
            else
            {
                ElmahExtension.LogToElmah(new ApplicationException("Unable to read the value PayPalSubmitUrl in web.config file"));
            }

        }

        public static void ParseResponse(string ResponseIN, int IPNID)
        {

            NameValueCollection PayPal_Args = HttpUtility.ParseQueryString(ResponseIN);

            using (EntitiesContext context = new EntitiesContext())
            {
                var i = context.IPNs.FirstOrDefault(s => s.IPNID == IPNID);

                if (i != null)
                {
                    
                    if (PayPal_Args["txn_id"] != null)
                    {
                        i.txn_id = PayPal_Args["txn_id"];
                    }

                    if (PayPal_Args["mc_gross"] != null)
                    {
                        i.mc_gross = decimal.Parse(PayPal_Args["mc_gross"]);
                    }

                    if (PayPal_Args["tax"] != null)
                    {
                        i.tax = decimal.Parse(PayPal_Args["tax"]);
                    }

                    if (PayPal_Args["protection_eligibility"] != null)
                    {
                        i.protection_eligibility = PayPal_Args["protection_eligibility"];
                    }

                    if (PayPal_Args["address_status"] != null)
                    {
                        i.address_status = PayPal_Args["address_status"];
                    }

                    if (PayPal_Args["payer_id"] != null)
                    {
                        i.payer_id = PayPal_Args["payer_id"];
                    }

                    if (PayPal_Args["first_name"] != null)
                    {
                        i.first_name = PayPal_Args["first_name"];
                    }

                    if (PayPal_Args["last_name"] != null)
                    {
                        i.last_name = PayPal_Args["last_name"];
                    }

                    if (PayPal_Args["address_street"] != null)
                    {
                        i.address_street = PayPal_Args["address_street"];
                    }

                    if (PayPal_Args["payment_date"] != null)
                    {
                        i.payment_date = PayPal_Args["payment_date"];
                    }

                    if (PayPal_Args["payment_status"] != null)
                    {
                        i.payment_status = PayPal_Args["payment_status"];
                    }

                    if (PayPal_Args["charset"] != null)
                    {
                        i.charset = PayPal_Args["charset"];
                    }

                    if (PayPal_Args["address_zip"] != null)
                    {
                        i.address_zip = int.Parse(PayPal_Args["address_zip"]);
                    }

                    if (PayPal_Args["mc_fee"] != null)
                    {
                        i.mc_fee = decimal.Parse(PayPal_Args["mc_fee"]);
                    }

                    if (PayPal_Args["address_country_code"] != null)
                    {
                        i.address_country_code = PayPal_Args["address_country_code"];
                    }

                    if (PayPal_Args["address_name"] != null)
                    {
                        i.address_name = PayPal_Args["address_name"];
                    }

                    if (PayPal_Args["notify_version"] != null)
                    {
                        i.notify_version = decimal.Parse(PayPal_Args["notify_version"]);
                    }

                    if (PayPal_Args["custom"] != null)
                    {
                        i.custom = PayPal_Args["custom"];
                    }

                    if (PayPal_Args["payer_status"] != null)
                    {
                        i.payer_status = PayPal_Args["payer_status"];
                    }

                    if (PayPal_Args["business"] != null)
                    {
                        i.business = PayPal_Args["business"];
                    }

                    if (PayPal_Args["address_country"] != null)
                    {
                        i.address_country = PayPal_Args["address_country"];
                    }

                    if (PayPal_Args["address_city"] != null)
                    {
                        i.address_city = PayPal_Args["address_city"];
                    }

                    if (PayPal_Args["quantity"] != null)
                    {
                        i.quantity = int.Parse(PayPal_Args["quantity"]);
                    }

                    if (PayPal_Args["verify_sign"] != null)
                    {
                        i.verify_sign = PayPal_Args["verify_sign"];
                    }

                    if (PayPal_Args["payer_email"] != null)
                    {
                        i.payer_email = PayPal_Args["payer_email"];
                    }

                    if (PayPal_Args["payment_type"] != null)
                    {
                        i.payment_type = PayPal_Args["payment_type"];
                    }

                    if (PayPal_Args["address_state"] != null)
                    {
                        i.address_state = PayPal_Args["address_state"];
                    }

                    if (PayPal_Args["receiver_email"] != null)
                    {
                        i.receiver_email = PayPal_Args["receiver_email"];
                    }

                    if (PayPal_Args["payment_fee"] != null)
                    {
                        i.payment_fee = decimal.Parse(PayPal_Args["payment_fee"]);
                    }

                    if (PayPal_Args["receiver_id"] != null)
                    {
                        i.receiver_id = PayPal_Args["receiver_id"];
                    }

                    if (PayPal_Args["txn_type"] != null)
                    {
                        i.txn_type = PayPal_Args["txn_type"];
                    }

                    if (PayPal_Args["item_name"] != null)
                    {
                        i.item_name = PayPal_Args["item_name"];
                    }

                    if (PayPal_Args["mc_currency"] != null)
                    {
                        i.mc_currency = PayPal_Args["mc_currency"];
                    }

                    if (PayPal_Args["item_number"] != null)
                    {
                        i.item_number = PayPal_Args["item_number"];
                    }

                    if (PayPal_Args["residence_country"] != null)
                    {
                        i.residence_country = PayPal_Args["residence_country"];
                    }

                    if (PayPal_Args["test_ipn"] != null)
                    {
                        i.test_ipn = PayPal_Args["test_ipn"];
                    }

                    if (PayPal_Args["handling_amount"] != null)
                    {
                        i.handling_amount = decimal.Parse(PayPal_Args["handling_amount"]);
                    }

                    if (PayPal_Args["transaction_subject"] != null)
                    {
                        i.transaction_subject = PayPal_Args["transaction_subject"];
                    }

                    if (PayPal_Args["payment_gross"] != null)
                    {
                        i.payment_gross = decimal.Parse(PayPal_Args["payment_gross"]);
                    }

                    if (PayPal_Args["shipping"] != null)
                    {
                        i.shipping = decimal.Parse(PayPal_Args["shipping"]);
                    }

                    if (PayPal_Args["ipn_track_id"] != null)
                    {
                        i.ipn_track_id = PayPal_Args["ipn_track_id"];
                    }

                    try
                    {
                        i.LastUpdated = DateTime.Now;
                        i.ResponseParsed = true;
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        i.ResponseParsed = false;
                        context.SaveChanges();
                        ElmahExtension.LogToElmah(ex);
                    }

                }

            }

        }

        public static string CreatePaymentUrl(string item, decimal ammount, int quantity)
        {

            StringBuilder sb = new StringBuilder();

            try
            {

                sb.Append(@"<a href=""");
                if (ConfigurationManager.AppSettings.Get("PayPalSubmitUrl") != null)
                {
                    sb.Append(ConfigurationManager.AppSettings.Get("PayPalSubmitUrl"));
                }
                sb.Append(@"?cmd=_xclick");
                if (ConfigurationManager.AppSettings.Get("PayPalBusiness") != null)
                {
                    sb.Append(@"&business=");
                    sb.Append(ConfigurationManager.AppSettings.Get("PayPalBusiness"));
                }
                sb.Append(@"&item_name=" + item);
                sb.Append(@"&currency_code=USD");
                sb.Append(@"&amount=" + ammount);
                sb.Append(@"&quantity=" + quantity);
                sb.Append(String.Format(@"&return={0}Default.aspx", Utils.ReturnRootUrl()));
                sb.Append(@"&rm=2");
                sb.Append(@"&cbt=Return to jQueryMobile");
                sb.Append(String.Format(@"&cancel_return={0}Default.aspx", Utils.ReturnRootUrl()));
                sb.Append(String.Format(@"&notify_url={0}IPN.aspx", Utils.ReturnRootUrl()));
                //sb.Append(@""">Check Out</a>");
                sb.Append(@"""><img src=""/Images/btn_xpressCheckout.gif""></a>");

            }
            catch (Exception ex)
            {
                ElmahExtension.LogToElmah(ex);
            }

            return sb.ToString();
        }

    }

}
