using Connect.ClientLib;
using Connect.ClientLib.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
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

            client.SetBaseEndPoint(ServiceURI.SandboxUS2014); 
            Guid fileId = upload(client, "test-10.28.2015", "five-page.pdf", File.ReadAllBytes(@"c:\play\five-page.pdf"));
            Guid docId = createDocument(client, "3930ea12-6675-43ad-8b1c-94c4177cc980", fileId, 5, "Test10.28.2015", "testing", "Test2015");

            // Get Quote
            placeOrder(client, docId.ToString(), true);

            // Place Order
            placeOrder(client, docId.ToString(), false);

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

        static Guid upload(ConnectClientLib client, string folderName, string fileName, byte[] fileBytes)
        {
            Guid newFileId = client.UploadXmlPDF(folderName, fileName, fileBytes).Result;
            return newFileId;

        }


        static Guid createDocument(ConnectClientLib client, string templateId, Guid fileId, int filePages, string docName, string docDescription, string folder)
        {

            string tURI = string.Format("StorageService/NewDocument?DocumentTemplateId={0}", templateId);
            var jsonReq = client.GetJsonRequest(tURI).Result;

            var req = client.ConvertToObject<NewDocument>(jsonReq);

            req.Name = docName;
            req.Description = docDescription;
            req.ReferenceData = "Testing";
            req.Product.Content[0].Range = string.Format("[{0},{1}]", 1, filePages);
            req.Product.Content[0].Source = fileId.ToString();
            req.Product.Quantity = 1;
            req.Product.ApplicationId = "00000000-0000-0000-0000-000000000000";


            client.SetBaseEndPoint(ServiceURI.SandboxUS);  // Set to 2012 API, bug reported with 2014 API
            string uploadURI = string.Format("StorageService/Document/{0}", folder);
            var createdDoc = client.PostRequest(uploadURI, req).Result;

            string fcDocumentId = (from file in createdDoc.Descendants(NameSpaces.ns + "DocumentFile")
                                   select file.Element(NameSpaces.ns + "FileId").Value).FirstOrDefault();


            client.SetBaseEndPoint(ServiceURI.SandboxUS2014); // Set to 2014 API
            return Guid.Parse(fcDocumentId);
        }

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
