using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.ServiceModel.Channels;

namespace SampleApp
{
	class Program
	{
		#region Fields

		public static string server = "https://connect.sandbox.mimeo.com/2012/02/";    // Sandbox Service
		//public static string server = "https://connect.mimeo.com/2012/02/";				// Production Service

		public static string authorizationData;
		private static string storageService = "storageservice";
		private static string orderService = "orders";
		public static XNamespace ns = "http://schemas.mimeo.com/MimeoConnect/2012/02/StorageService";
		public static XNamespace nsOrder = "http://schemas.mimeo.com/MimeoConnect/2012/02/Orders";
		public static XNamespace nsESLOrder = "http://schemas.mimeo.com/EnterpriseServices/2008/09/OrderService";


		private static bool verbose = true;
		
		#endregion 
		
		#region Main
 
		static void Main(string[] args)
		{
			string userName_password = string.Empty;

			if(args.Length < 1)
			{
				args = new String[] { "jmoncada@mimeo.com", "pass!" };
			}

			if(args.Length >= 2)
			{
				userName_password = args[0] + ":" + args[1];
				byte[] encData_byte = new byte[userName_password.Length];
				encData_byte = System.Text.Encoding.UTF8.GetBytes(userName_password);
				authorizationData = "Basic " + Convert.ToBase64String(encData_byte);

				XDocument docXml = doUploadDocument();
				String OrderFriendlyId = orderDocument(docXml);

				Program.getOrderInfo(OrderFriendlyId, "GetOrderHistory");   // Display order history
				Program.getOrderInfo(OrderFriendlyId, "status");			// get order status
				Program.getOrderInfo(OrderFriendlyId, "tracking");			// get order tracking

			}
		}

		#endregion 

		#region Business Logic

		static string orderDocument(XDocument docXml)
		{
			string OrderFriendlyId = "0";
			try
			{
				XmlDocument orderDoc = new XmlDocument();
				orderDoc.XmlResolver = null;

				//Step 1- Get New Order Template
				Program.GetNewOrderRequest(orderDoc);

				if(verbose)
				{
					Console.WriteLine("-----------------------------------\r\n{0}\r\n-----------------------------------", XElement.Parse(orderDoc.InnerXml).ToString());
				}

				//Step 2 - Find documents to add to order
				string fcDocumentId = (from file in docXml.Descendants(ns + "DocumentFile")
									   select file.Element(ns + "FileId").Value).FirstOrDefault();
				string docName = (from file in docXml.Descendants(ns + "DocumentFile")
									   select file.Element(ns + "Name").Value).FirstOrDefault();

				if(verbose)
				{
					Console.WriteLine("---------------------\r\n Document Id: {0}\r\n-----------------------------------", fcDocumentId);
				}

				// Step 3 - Add to Order Request
				PopulateAddLineItems(orderDoc, docName, fcDocumentId);

				// Step 4 - Second Item to Order
				//docName = "Doc Two";
				//fcDocumentId = "5A2E6E6C-D706-45EB-8BDA-B77246248B5B";
				//AppendAddLineItems(orderDoc, docName, fcDocumentId);

				if(verbose)
				{
					Console.WriteLine("METHOD: PopulateAddLineItems");
					Console.WriteLine("-----------------------------------\r\n{0}\r\n-----------------------------------", XElement.Parse(orderDoc.InnerXml).ToString());
				}

				// Step 5 - Add Payment
				PopulatePaymentMethod(orderDoc);
				if(verbose)
				{
					Console.WriteLine("-----------------------------------\r\n{0}\r\n-----------------------------------", XElement.Parse(orderDoc.InnerXml).ToString());
				}

				/// Step 6 - Let's continue and add recipient
				Program.PopulateNewRecipientAddress(orderDoc);
				if(verbose)
				{
					Console.WriteLine("-----------------------------------\r\n{0}\r\n-----------------------------------", XElement.Parse(orderDoc.InnerXml).ToString());
				}

				// Step 6.5 Add SI items
				//Program.PopulateSpecialInstructionCodes(orderDoc);
				//if(verbose)
				//{
				//    Console.WriteLine("-----------------------------------\r\n{0}\r\n-----------------------------------", XElement.Parse(orderDoc.InnerXml).ToString());
				//}

				// Step 6.6 Add Options
				Program.AddOptions(orderDoc);

				// Step 6.7 Add Packing Slip
				Program.AddPackagingSlip(orderDoc);

				// Step 7 Shipping Options
				XmlDocument shippingDoc = new XmlDocument();
				shippingDoc.LoadXml(orderDoc.InnerXml);
				shippingDoc = Program.GetShippingOptions(shippingDoc);

				// 1st Recipient
				XmlNode addRecipientRequestRoot = orderDoc.GetElementsByTagName("AddRecipientRequest")[0];
				addRecipientRequestRoot.ChildNodes[1].InnerText = findShippingId(shippingDoc, "Ground");
				// 2nd Recipient
				XmlNode addRecipientRequestRoot2 = orderDoc.GetElementsByTagName("AddRecipientRequest")[1];
				addRecipientRequestRoot2.ChildNodes[1].InnerText = findShippingId(shippingDoc, "Two Day Guaranteed");

				if(verbose)
				{
					Console.WriteLine("-----------------------------------\r\n{0}\r\n-----------------------------------", XElement.Parse(shippingDoc.InnerXml).ToString());
					Console.WriteLine("-----------------------------------\r\n{0}\r\n-----------------------------------", XElement.Parse(orderDoc.InnerXml).ToString());
				}

				//return;

				// Step 7 - Get Quote
				XmlDocument orderQuoteDoc = GetQuote(orderDoc);
				if(verbose)
				{
					Console.WriteLine("-----------------------------------\r\n{0}\r\n-----------------------------------", XElement.Parse(orderQuoteDoc.InnerXml).ToString());
					Console.WriteLine("XML: OrderRequest");
					Console.WriteLine("-----------------------------------\r\n{0}\r\n-----------------------------------", XElement.Parse(orderDoc.InnerXml).ToString());
				}

				//Step 8- PlaceOrder
				Program.PlaceOrder(orderDoc);
				if(verbose)
				{
					Console.WriteLine("-----------------------------------\r\n{0}\r\n-----------------------------------", XElement.Parse(orderDoc.InnerXml).ToString());
				}

				XmlNamespaceManager nsmgr = new XmlNamespaceManager(orderDoc.NameTable);
				nsmgr.AddNamespace("i", "http://schemas.mimeo.com/EnterpriseServices/2008/09/OrderService");
				XmlNode orderFriendlyIdNode = orderDoc.DocumentElement.SelectNodes("//i:OrderFriendlyId", nsmgr)[0];
				OrderFriendlyId = orderFriendlyIdNode.InnerText;
				Console.WriteLine(string.Format("OrderFriendlyId:{0}.", OrderFriendlyId));

				Console.ReadLine();
			}
			catch(FaultException fe)
			{
				Console.WriteLine(fe.Message);
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}

			return OrderFriendlyId;
		}

		static XDocument doUploadDocument()
		{
			XDocument createResult = new XDocument();

			string myPrintFileFolder = "/BOM";           // My Site Upload Folder for BOM
			string myDocumentFolder = "/BOM Documents";           // My Site Upload Folder for BOM
			string fileName = @"C:\play\upload\tenpage.pdf";  // file and location to upload



			Uri storageEndpoint = new Uri(server + storageService + myPrintFileFolder);
			
			XDocument uploadResult = Program.UploadPDF(storageEndpoint, authorizationData, fileName);

			if(verbose)
			{
				Console.WriteLine("-----------------------------------\r\n{0}\r\n-----------------------------------", XElement.Parse(uploadResult.ToString()));
			}

			 createResult = Program.createDocument(myDocumentFolder, uploadResult);



			if(verbose)
			{
				Console.WriteLine("-----------------------------------\r\n{0}\r\n-----------------------------------", XElement.Parse(createResult.ToString()));
			}

			// List files in directory
			//XDocument folderList = GetFolderInfo(storageEndpoint);
			//if(verbose)
			//{
			//    Console.WriteLine("-----------------------------------\r\n{0}\r\n-----------------------------------", XElement.Parse(folderList.ToString()));
			//}

			return createResult;

		}

		#endregion

		#region Document Methods

		public static XDocument UploadPDF(Uri uri, string authorizationData, string fileName)
		{
			XDocument myResult = new XDocument();

			string boundary = "----------" + DateTime.Now.Ticks.ToString("x");

			if(verbose)
			{
				Console.WriteLine("ENDPOINT: {0}", uri);
			}

			try
			{
				HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(uri);
				webrequest.Headers.Add(HttpRequestHeader.Authorization, authorizationData);
				webrequest.ContentType = "multipart/form-data; boundary=" + boundary;
				webrequest.Method = "POST";

				// Build up the post message header
				StringBuilder sb = new StringBuilder();
				sb.Append("--");
				sb.Append(boundary);
				sb.Append("\r\n");
				sb.Append("Content-Disposition: form-data; name=\"");
				sb.Append("file");
				sb.Append("\"; filename=\"" + fileName + "\"");
				sb.Append("\r\n");
				sb.Append("Content-Type: application/octet-stream");
				sb.Append("\r\n");
				sb.Append("\r\n");

				string postHeader = sb.ToString();

				byte[] postHeaderBytes = Encoding.UTF8.GetBytes(postHeader);

				// Build the trailing boundary string as a byte array
				// ensuring the boundary appears on a line by itself
				byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

				using(Stream fileStream = File.OpenRead(fileName))
				{
					if(fileStream != null)
					{
						long length = postHeaderBytes.Length + fileStream.Length + boundaryBytes.Length;

						webrequest.ContentLength = length;

						Stream requestStream = webrequest.GetRequestStream();

						// Write out our post header
						requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);

						// Write out the file contents
						byte[] buffer = new Byte[checked((uint)Math.Min(4096, (int)fileStream.Length))];

						int bytesRead = 0;

						int i = 0;

						while((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
						{
							requestStream.Write(buffer, 0, bytesRead);
							i++;
						}

						// Write out the trailing boundary
						requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);

						WebResponse response = Program.GetWebResponseWithFaultException(webrequest);
						Stream s = response.GetResponseStream();
						StreamReader sr = new StreamReader(s);
						myResult = XDocument.Load(sr);
					}
					else
					{
						throw new Exception("File stream is null");
					}
				}
			}
			catch(WebException we)
			{
				throw;
			}
			catch(Exception)
			{
				throw;
			}

			return myResult;
		}

		public static XDocument GetFolderInfo(Uri uri)
		{
			string boundary = "----------" + DateTime.Now.Ticks.ToString("x");

			try
			{
				XmlDocument doc = new XmlDocument();
				HttpWebGet(doc, uri);

				//string ns = "xmlns=\"http://schemas.mimeo.com/MimeoConnect/2010/09/StorageService\"";
				//string docXML = doc.OuterXml.Replace(ns, "");

				XDocument xDoc = XDocument.Parse(doc.OuterXml);
				return xDoc;
			}
			catch(WebException we)
			{
				throw;
			}
			catch(Exception)
			{
				throw;
			}
		}

		public static XDocument createDocument(string myFolder, XDocument fileXml)
		{
			XDocument retDoc = new XDocument();
			string MyBookletTemplateId = "3f473099-3165-4775-a583-8cf178afa74a";
			try
			{
				string printFileId = (from file in fileXml.Descendants(ns + "File")
									  select file.Element(ns + "FileId").Value).FirstOrDefault();

				string pageCount = (from file in fileXml.Descendants(ns + "File")
									select file.Element(ns + "PageCount").Value).FirstOrDefault();

				string fileName = (from file in fileXml.Descendants(ns + "File")
								   select file.Element(ns + "Name").Value).FirstOrDefault();

				string newDocument = string.Format("/NewDocument?DocumentTemplateId={0}", MyBookletTemplateId);
				Uri storageEndpoint = new Uri(server + storageService + newDocument);
				XmlDocument FolderDoc = callEndPoint(storageEndpoint);

				PopulateProductInfo(printFileId, pageCount, FolderDoc, nsOrder.NamespaceName);

				XmlNamespaceManager nsmgr = new XmlNamespaceManager(FolderDoc.NameTable);
				nsmgr.AddNamespace("i", nsOrder.NamespaceName);
				XmlNode name = FolderDoc.DocumentElement.SelectNodes("//i:Name", nsmgr)[0];
				name.InnerText = fileName;


				if(verbose)
				{
					Console.WriteLine("-----------------------------------\r\n{0}\r\n-----------------------------------", XElement.Parse(FolderDoc.InnerXml));
				}

				string createDocument = string.Format("/Document/{0}", myFolder);
				storageEndpoint = new Uri(server + storageService + createDocument);
				XmlDocument newDoc = HttpWebPost(FolderDoc, storageEndpoint);

				if(verbose)
				{
					Console.WriteLine("-----------------------------------\r\n{0}\r\n-----------------------------------", XElement.Parse(newDoc.InnerXml));
				}

				retDoc = XDocument.Parse(newDoc.InnerXml);
			}
			catch(FaultException fe)
			{
				Console.WriteLine(fe.Message);
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}

			return retDoc;
		}

		public static string findDocumentId(string myFolder, string documentName)
		{
			string retDocId = "-1";
			try
			{
				string docFolder = "/Document" + myFolder + "/";
				Uri storageEndpoint = new Uri(server + storageService + docFolder);

				XDocument FolderDoc = GetFolderInfo(storageEndpoint);

				if(verbose)
				{
					Console.WriteLine("-----------------------------------\r\n{0}\r\n-----------------------------------", FolderDoc.ToString());
				}

				retDocId = (from file in FolderDoc.Descendants(ns + "DocumentFile")
							where file.Element(ns + "Name").Value == documentName
							select file.Element(ns + "FileId").Value).FirstOrDefault();
				return retDocId;

			}
			catch(FaultException fe)
			{
				Console.WriteLine(fe.Message);
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}

			return retDocId;
		}

		private static void PopulateProductInfo(string fileId, string pageCount, XmlDocument orderDoc, string ns)
		{
			XmlNode oldSourceNode = orderDoc.GetElementsByTagName("Source")[0];
			XmlNode newSourceNode = orderDoc.CreateElement("Source", ns);
			XmlText sourceTextNode = orderDoc.CreateTextNode(fileId);
			newSourceNode.AppendChild(sourceTextNode);

			XmlNode oldRangeNode = orderDoc.GetElementsByTagName("Range")[0];
			XmlNode newRangeNode = orderDoc.CreateElement("Range", ns);
			XmlText rangeTextNode = orderDoc.CreateTextNode("[1," + pageCount + "]");
			newRangeNode.AppendChild(rangeTextNode);

			XmlNode documentSectionRoot = orderDoc.GetElementsByTagName("DocumentSection")[0];
			documentSectionRoot.ReplaceChild(newSourceNode, oldSourceNode);
			documentSectionRoot.ReplaceChild(newRangeNode, oldRangeNode);
		}

		#endregion

		#region Order Methods

		private static void GetNewOrderRequest(XmlDocument doc)
		{
			Uri ordersEndpoint;

			ordersEndpoint = new Uri(server + "orders/GetOrderRequest");
			HttpWebGet(doc, ordersEndpoint);
		}

		private static void PopulateAddLineItems(XmlDocument orderRequest, string name, string storeItemId)
		{
			XmlNode addLineItemRequestRoot = orderRequest.GetElementsByTagName("AddLineItemRequest")[0];
			addLineItemRequestRoot.ChildNodes[0].InnerText = name;
			addLineItemRequestRoot.ChildNodes[1].ChildNodes[0].InnerText = storeItemId;
		}

		private static void AppendAddLineItems(XmlDocument orderRequest, string name, string storeItemId)
		{
			XmlNode lineItems = orderRequest.GetElementsByTagName("LineItems")[0];
			XmlNode addLineItemRequestSection = orderRequest.GetElementsByTagName("AddLineItemRequest")[0];
			XmlNode importNode = lineItems.OwnerDocument.ImportNode(addLineItemRequestSection, true);
			importNode.ChildNodes[0].InnerText = name;
			importNode.ChildNodes[1].ChildNodes[0].InnerText = storeItemId;
			lineItems.AppendChild(importNode);
		}

		private static void PopulatePaymentMethod(XmlDocument orderRequest)
		{
			if(verbose)
			{
				Console.WriteLine("METHOD:  PopulatePaymentMethod");
			}

			XmlNode specialInstructionCodesRoot = orderRequest.GetElementsByTagName("PaymentMethod")[0];
			specialInstructionCodesRoot.Attributes["i:type"].Value = "UserCreditLimitPaymentMethod";


			//specialInstructionCodesRoot.ChildNodes[0].InnerText = "John Smith";
			//specialInstructionCodesRoot.ChildNodes[2].InnerText = "4111111111111111";
			//specialInstructionCodesRoot.ChildNodes[3].InnerText = "10016";
			//specialInstructionCodesRoot.ChildNodes[4].InnerText = "460 park ave s";
			//specialInstructionCodesRoot.ChildNodes[5].InnerText = "New york";
			//specialInstructionCodesRoot.ChildNodes[6].InnerText = "Ny";
			//specialInstructionCodesRoot.ChildNodes[7].InnerText = "US";
			//specialInstructionCodesRoot.ChildNodes[8].InnerText = "2";
			//specialInstructionCodesRoot.ChildNodes[9].InnerText = "2030";

		}

		private static void PopulateNewRecipientAddress(XmlDocument orderRequest)
		{
			if(verbose)
			{
				Console.WriteLine("METHOD:  PopulateNewRecipientAddress");
			}
			XmlNode recipientAddressRoot = orderRequest.GetElementsByTagName("Address")[0];

			orderRequest.GetElementsByTagName("City")[0].InnerText = "New York";
			orderRequest.GetElementsByTagName("Country")[0].InnerText = "US";
			orderRequest.GetElementsByTagName("LastName")[0].InnerText = "Smith";
			orderRequest.GetElementsByTagName("FirstName")[0].InnerText = "Will";
			orderRequest.GetElementsByTagName("StateOrProvince")[0].InnerText = "NY";
			orderRequest.GetElementsByTagName("PostalCode")[0].InnerText = "10016";
			orderRequest.GetElementsByTagName("Street")[0].InnerText = "460 Park Ave S.";
			orderRequest.GetElementsByTagName("TelephoneNumber")[0].InnerText = "212-333-4444";

			//=====================================================================================

			XmlNode recipientsRootNode = orderRequest.GetElementsByTagName("Recipients")[0];
			XmlNode addRecipientRequest = orderRequest.CreateElement("AddRecipientRequest");
			XmlNode address = orderRequest.CreateElement("Address");

			XmlNode newFirstnameNode = orderRequest.CreateElement("FirstName");
			XmlNode firstnameTextNode = orderRequest.CreateTextNode("Raul");
			newFirstnameNode.AppendChild(firstnameTextNode);
			address.AppendChild(newFirstnameNode);

			XmlNode newLastnameNode = orderRequest.CreateElement("LastName");
			XmlNode lastnameTextNode = orderRequest.CreateTextNode("Moncada");
			newLastnameNode.AppendChild(lastnameTextNode);
			address.AppendChild(newLastnameNode);

			XmlNode newStreetNode = orderRequest.CreateElement("Street");
			XmlNode streetTextNode = orderRequest.CreateTextNode("10226 Signal Hill RD");
			newStreetNode.AppendChild(streetTextNode);
			address.AppendChild(newStreetNode);

			XmlNode newCityNode = orderRequest.CreateElement("City");
			XmlNode cityTextNode = orderRequest.CreateTextNode("Austin");
			newCityNode.AppendChild(cityTextNode);
			address.AppendChild(newCityNode);

			XmlNode newStateNode = orderRequest.CreateElement("StateOrProvince");
			XmlNode stateTextNode = orderRequest.CreateTextNode("TX");
			newStateNode.AppendChild(stateTextNode);
			address.AppendChild(newStateNode);

			XmlNode newCountryNode = orderRequest.CreateElement("Country");
			XmlNode countryTextNode = orderRequest.CreateTextNode("US");
			newCountryNode.AppendChild(countryTextNode);
			address.AppendChild(newCountryNode);

			XmlNode newPostalCodeNode = orderRequest.CreateElement("PostalCode");
			XmlNode postalCodeTextNode = orderRequest.CreateTextNode("78737");
			newPostalCodeNode.AppendChild(postalCodeTextNode);
			address.AppendChild(newPostalCodeNode);

			XmlNode newTelephoneNumberNode = orderRequest.CreateElement("TelephoneNumber");
			XmlNode TelephoneNumberTextNode = orderRequest.CreateTextNode("212-333-4444");
			newTelephoneNumberNode.AppendChild(TelephoneNumberTextNode);
			address.AppendChild(newTelephoneNumberNode);

			XmlNode newResidentialNode = orderRequest.CreateElement("IsResidential");
			XmlNode residentialTextNode = orderRequest.CreateTextNode("true");
			newResidentialNode.AppendChild(residentialTextNode);
			address.AppendChild(newResidentialNode);


			XmlNode shippingMethodIdNode = orderRequest.CreateElement("ShippingMethodId");
			XmlNode shippingMethodIdTextNode = orderRequest.CreateTextNode("00000000-0000-0000-0000-000000000001");
			shippingMethodIdNode.AppendChild(shippingMethodIdTextNode);
			address.AppendChild(shippingMethodIdNode);


			addRecipientRequest.AppendChild(address);
			addRecipientRequest.AppendChild(shippingMethodIdNode);
			recipientsRootNode.AppendChild(addRecipientRequest);

		}

		private static void PopulateSpecialInstructionCodes(XmlDocument orderRequest)
		{
			if(verbose)
			{
				Console.WriteLine("METHOD:  PopulateSpecialInstructionCodes");
			}
			XmlNode specialInstructionCodesRoot = orderRequest.GetElementsByTagName("SpecialInstructionCodes")[0];

			// 1st SI
			specialInstructionCodesRoot.ChildNodes[0].InnerText = "ESI-O-0002";

			// 2nd SI
			XmlNode importNode = specialInstructionCodesRoot.OwnerDocument.ImportNode(specialInstructionCodesRoot.ChildNodes[0], true);
			importNode.InnerText = "ESI-O-1202";
			specialInstructionCodesRoot.AppendChild(importNode);
		}

		private static void AddOptions(XmlDocument orderRequest)
		{
			XmlNode node = orderRequest.CreateNode(XmlNodeType.Element, "Options", nsESLOrder.NamespaceName);
			XmlNode nodeAdditionalProcessingHours = orderRequest.CreateElement("AdditionalProcessingHours", "http://schemas.mimeo.com/EnterpriseServices/2008/09/OrderService");
			nodeAdditionalProcessingHours.InnerText = "24";
			node.AppendChild(nodeAdditionalProcessingHours);
			orderRequest.DocumentElement.AppendChild(node);
		}

		private static void AddPackagingSlip(XmlDocument orderRequest)
		{
			XmlNode node = orderRequest.CreateNode(XmlNodeType.Element, "PackagingSlip", nsESLOrder.NamespaceName);

			XmlNode nodeSalutationType = orderRequest.CreateElement("SalutationType", nsESLOrder.NamespaceName);
			nodeSalutationType.InnerText = "None";
			node.AppendChild(nodeSalutationType);

			XmlNode nodeMemo = orderRequest.CreateElement("Memo", nsESLOrder.NamespaceName);
			nodeMemo.InnerText = "Professor Smith";
			node.AppendChild(nodeMemo);

			orderRequest.DocumentElement.AppendChild(node);
		}

		private static XmlDocument GetShippingOptions(XmlDocument doc)
		{
			if(verbose)
			{
				Console.WriteLine("------ INPUT to GetShipping OPtions -----\r\n{0}\r\n-----------------------------------", XElement.Parse(doc.InnerXml).ToString());
			}
			Uri ordersEndpoint = new Uri(server + "orders/GetShippingOptions");
			return HttpWebPost(doc, ordersEndpoint);
		}

		public static string findShippingId(XmlDocument doc, string shippingMethodName)
		{
			XDocument shippingDoc = XDocument.Parse(doc.InnerXml);
			string retShipId = "-1";

			retShipId = (from file in shippingDoc.Descendants(nsESLOrder + "ShippingMethodDetail")
						 where file.Element(nsESLOrder + "Name").Value == shippingMethodName
						 select file.Element(nsESLOrder + "Id").Value).FirstOrDefault();

			return retShipId;
		}

		private static void PlaceOrder(XmlDocument doc)
		{
			Uri ordersEndpoint = new Uri(server + "orders/PlaceOrder");
			HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(ordersEndpoint);
			webrequest.Headers.Add(HttpRequestHeader.Authorization, authorizationData);
			webrequest.Method = "POST";
			// Set the ContentType property of the WebRequest.
			webrequest.ContentType = "application/xml";

			if(verbose)
			{
				Console.WriteLine("ENDPOINT: {0}", ordersEndpoint);
			}

			using(StringWriter sw = new StringWriter())
			using(XmlTextWriter xtw = new XmlTextWriter(sw))
			{
				doc.WriteTo(xtw);

				byte[] byteArray = Encoding.UTF8.GetBytes(sw.ToString());
				webrequest.ContentLength = byteArray.Length;

				Stream dataStream = webrequest.GetRequestStream();
				// Write the data to the request stream.
				dataStream.Write(byteArray, 0, byteArray.Length);
				// Close the Stream object.
				dataStream.Close();

				WebResponse response = Program.GetWebResponseWithFaultException(webrequest);
				Stream s = response.GetResponseStream();
				doc.Load(s);
				dataStream.Close();
				response.Close();
			}
		}

		private static XmlDocument GetQuote(XmlDocument doc)
		{
			Uri ordersEndpoint = new Uri(server + "orders/GetQuote");
			return HttpWebPost(doc, ordersEndpoint);
		}



		private static XmlDocument getOrderInfo(string OrderFriendlyId, string action)
		{
			XmlDocument actionResult = new XmlDocument();
			string callAction = string.Format("/{0}/{1}", OrderFriendlyId, action);
			Uri storageEndpoint = new Uri(server + orderService + callAction);
			actionResult = callEndPoint(storageEndpoint);

			return actionResult;
		}

		#endregion
		
		#region Helper

		private static void HttpWebGet(XmlDocument doc, Uri ordersEndpoint)
		{

			UTF8Encoding encoding = new UTF8Encoding();

			HttpWebRequest objRequest;
			HttpWebResponse objResponse;
			StreamReader srResponse;

			if(verbose)
			{
				Console.WriteLine("ENDPOINT: {0}", ordersEndpoint);
			}

			// Initialize request object  
			objRequest = (HttpWebRequest)WebRequest.Create(ordersEndpoint);
			objRequest.Headers.Add(HttpRequestHeader.Authorization, authorizationData);
			objRequest.Method = "GET";
			objRequest.AllowWriteStreamBuffering = true;

			// Get response
			objResponse = (HttpWebResponse)objRequest.GetResponse();
			srResponse = new StreamReader(objResponse.GetResponseStream(), Encoding.ASCII);
			string xmlOut = srResponse.ReadToEnd();
			srResponse.Close();

			if(xmlOut != null && xmlOut.Length > 0)
			{
				doc.LoadXml(xmlOut);
			}
		}
		private static XmlDocument HttpWebPost(XmlDocument doc, Uri ordersEndpoint)
		{
			HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(ordersEndpoint);
			webrequest.Headers.Add(HttpRequestHeader.Authorization, authorizationData);
			webrequest.Method = "POST";
			// Set the ContentType property of the WebRequest.
			webrequest.ContentType = "application/xml";

			if(verbose)
			{
				Console.WriteLine("ENDPOINT: {0}", ordersEndpoint);
			}

			XmlDocument result = new XmlDocument();
			result.XmlResolver = null;

			using(StringWriter sw = new StringWriter())
			using(XmlTextWriter xtw = new XmlTextWriter(sw))
			{
				doc.WriteTo(xtw);

				byte[] byteArray = Encoding.UTF8.GetBytes(sw.ToString());
				webrequest.ContentLength = byteArray.Length;

				Stream dataStream = webrequest.GetRequestStream();
				// Write the data to the request stream.
				dataStream.Write(byteArray, 0, byteArray.Length);
				// Close the Stream object.
				dataStream.Close();

				WebResponse response = Program.GetWebResponseWithFaultException(webrequest);
				Stream s = response.GetResponseStream();
				result.Load(s);

				dataStream.Close();
				response.Close();
			}

			return result;
		}
		public static XmlDocument callEndPoint(Uri uri)
		{
			try
			{
				XmlDocument doc = new XmlDocument();
				HttpWebGet(doc, uri);

				if(verbose && doc != null && doc.InnerXml != null && doc.InnerXml.Length > 0)
				{
					Console.WriteLine("-----------------------------------\r\n{0}\r\n-----------------------------------", XElement.Parse(doc.InnerXml));
				}
				else
				{
					Console.WriteLine("-----------------------------------\r\n{0}\r\n-----------------------------------", "Document Empty");
				}

				return doc;
			}
			catch(WebException we)
			{
				throw;
			}
			catch(Exception)
			{
				throw;
			}
		}
		private static WebResponse GetWebResponseWithFaultException(HttpWebRequest httpWebRequest)
		{
			WebResponse response = null;

			try
			{
				response = httpWebRequest.GetResponse();
			}
			catch(WebException we)
			{
				if(we.Status == WebExceptionStatus.ProtocolError)
				{
					using(Stream stream = we.Response.GetResponseStream())
					{
						XmlDocument doc = new XmlDocument();
						doc.XmlResolver = null;
						doc.Load(stream);
						throw new FaultException(doc.InnerText); 
					}
				}
				throw;
			}
			return response;
		}

		#endregion
	}

}
