using Connect.ClientLib;
using Connect.ClientLib.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleClient
{
    class Program
    {
        static void Main(string[] args)
        {

            var client = new ConnectClientLib("jmoncada@mimeo.com", "testing!"); 

            // Get shipping options example
            callGetShippingOptions(client);

            // Place Order example
            doPlaceOrder(client) ;

        }

        static void callGetShippingOptions(ConnectClientLib client)
        {
            client.SetBaseEndPoint(ServiceURI.SandboxUS2014);
            // Document Id is from Mimeo Library
            getShippingOptions(client, "a4cfb527-8462-490f-a77a-44e45a53c9d8"); 
        }

        static void doPlaceOrder(ConnectClientLib client)
        {
            client.SetBaseEndPoint(ServiceURI.SandboxUS2014);
            // Document Id is from Mimeo Library
            placeOrder(client, "a4cfb527-8462-490f-a77a-44e45a53c9d8", false);
        }

        #region Helper

        static void getShippingOptions(ConnectClientLib client, string DocId)
        {
            var jsonReq = client.GetJsonRequest("Orders/GetOrderRequest").Result;

            var req = client.ConvertToObject<OrderRequest>(jsonReq);

            string jsonReq2String = Newtonsoft.Json.JsonConvert.SerializeObject(jsonReq);

            req.LineItems[0].Name = "Test";
            req.LineItems[0].Quantity = 1;
            req.LineItems[0].StoreItemReference.Id = DocId;

            req.Recipients[0].Address.FirstName = "Will";
            req.Recipients[0].Address.LastName = "Smith";
            req.Recipients[0].Address.Street = "460 Park Ave S.";
            req.Recipients[0].Address.StateOrProvince = "NY";
            req.Recipients[0].Address.PostalCode = "10016";
            req.Recipients[0].Address.City = "New York";
            req.Recipients[0].Address.Country = "US";
            req.Recipients[0].Address.TelephoneNumber = "212-333-4444";

            if (client.getBaseEndPoint().Contains("2014"))
            {
                req.PaymentMethod = null;
            }
            else
            {
                req.PaymentMethod.__type = @"UserCreditLimitPaymentMethod:http://schemas.mimeo.com/EnterpriseServices/2008/09/OrderService";
            }

            var shipOpts = client.PostJsonRequest("Orders/GetShippingOptions", req).Result;

            var shipReq = client.ConvertToObject<OrderAvailableDeliveryOptions>(shipOpts);

            string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(shipOpts);
            Console.WriteLine(jsonString);

            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
        }

        static void placeOrder(ConnectClientLib client, string DocId, bool quoteOnly)
        {

            var jsonReq = client.GetJsonRequest("Orders/GetOrderRequest").Result;

            var req = client.ConvertToObject<OrderRequest>(jsonReq);

            req.LineItems[0].Name = "Test";
            req.LineItems[0].Quantity = 1;
            req.LineItems[0].StoreItemReference.Id = DocId;

            req.Recipients[0].Address.FirstName = "Will";
            req.Recipients[0].Address.LastName = "Smith";
            req.Recipients[0].Address.Street = "460 Park Ave S.";
            req.Recipients[0].Address.StateOrProvince = "NY";
            req.Recipients[0].Address.PostalCode = "10016";
            req.Recipients[0].Address.City = "New York";
            req.Recipients[0].Address.Country = "US";
            req.Recipients[0].Address.TelephoneNumber = "212-333-4444";

            req.Options = new Options();
            req.Options.AdditionalProcessingHours = 120;

            req.ShipFromInfo = new Connect.ClientLib.Shipfrominfo();
            req.ShipFromInfo.CompanyName = "Client Connect Test";
            req.ShipFromInfo.Email = "raulworking@gmail.com";
            req.ShipFromInfo.FirstName = "Jesus Raul";
            req.ShipFromInfo.LastName = "Moncada";
            req.ShipFromInfo.TelephoneNumber = "555-555-1111";

            req.PaymentMethod.type = @"Mimeo.Services.MimeoConnect.Enterprise.OrderService.UserCreditLimitPaymentMethod, Mimeo.MimeoConnect.EnterpriseServicesWCFClient";

            var shipOpts = client.PostJsonRequest("Orders/GetShippingOptions", req).Result;

            var shipReq = client.ConvertToObject<OrderAvailableDeliveryOptions>(shipOpts);

            Guid shipId = shipReq.findDeliveryOption("Ground"); // Default Ground

            req.Recipients[0].ShippingMethodId = shipId.ToString();

            if (quoteOnly)
            {
                var quoteReq = client.PostJsonRequest("Orders/GetQuote", req).Result;
                Console.WriteLine("Id: {0}", quoteReq["orderId"].ToString());
                Console.WriteLine("Friendly Id: {0}", "");
                Console.WriteLine("Price: {0}", quoteReq["financialDetail"]["total"].ToString());

            }
            else
            {
                var placeReq = client.PostJsonRequest("Orders/PlaceOrder", req).Result;
                Console.WriteLine("Id: {0}", placeReq["orderId"].ToString());
                Console.WriteLine("Friendly Id: {0}", placeReq["orderFriendlyId"].ToString());
                Console.WriteLine("Price: {0}", placeReq["financialDetail"]["total"].ToString());

            }
        }

        #endregion
    }
}
