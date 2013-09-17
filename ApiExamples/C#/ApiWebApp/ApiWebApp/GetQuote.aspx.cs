using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using Mimeo.MimeoConnect;

namespace ApiWebApp
{
	public partial class WebForm1 : System.Web.UI.Page
	{
		public static XNamespace nsESLOrder = "http://schemas.mimeo.com/EnterpriseServices/2008/09/OrderService";
		public static RestApi mimeoApi = RestApi.GetInstance;

		protected void Page_Load(object sender, EventArgs e)
		{
            bool sandbox = (this.rButtonList.SelectedValue == "sandbox") ? true : false;
			mimeoApi.Initialize(this.txtUser.Text, this.txtPassword.Text, sandbox);

		}

		#region Actions

		protected void GetQuote_Click(object sender, EventArgs e)
		{
			this.txtOutput.Text = string.Empty;

			//XDocument docXml = DoUploadDocument();
			XDocument docXml = new XDocument();

			String orderFriendlyId = OrderDocument(docXml, false);

		}

		protected void btnGetShippingOps_Click(object sender, EventArgs e)
		{

			this.txtOutput.Text = string.Empty;

			var orderDoc = new XmlDocument();
			orderDoc.XmlResolver = null;
			setupOrder(orderDoc);

			var shippingDoc = new XmlDocument();
			shippingDoc.LoadXml(orderDoc.InnerXml);
			shippingDoc = mimeoApi.GetShippingOptions(shippingDoc);

			XDocument shippingOpts = XDocument.Parse(shippingDoc.InnerXml);

			var query = from xEle in shippingOpts.Descendants(nsESLOrder + "ShippingMethodDetail")
						select new
						{
							value = xEle.Element(nsESLOrder + "Id").Value,
							text = xEle.Element(nsESLOrder + "Name").Value
						};
			ddlShipOptions.DataValueField = "value";
			ddlShipOptions.DataTextField = "text";
			ddlShipOptions.DataSource = query;
			ddlShipOptions.DataBind();
			this.txtOutput.Text = XElement.Parse(shippingDoc.InnerXml).ToString();
		}

		protected void PlaceOrder_Click(object sender, EventArgs e)
		{
			this.txtOutput.Text = string.Empty;

			//XDocument docXml = DoUploadDocument();
			XDocument docXml = new XDocument();

			String orderFriendlyId = OrderDocument(docXml, true);
		}

		protected void Clear_Click(object sender, EventArgs e)
		{
			this.txtOutput.Text = "";
		}

		#endregion


		#region Workers

		private void setupOrder(XmlDocument orderDoc)
		{
			try
			{
				//Step 1- Get New Order Template
				mimeoApi.GetNewOrderRequest(orderDoc);

				// Step 2:  Add Document to Order
				List<Document> documents = new List<Document>(){
					new Document (){
						 id = Guid.Parse(this.txtDocumentId.Text),
						 Name = this.txtDocName.Text,
						 Quantity = int.Parse("10")
					}
				};
				mimeoApi.AddLineItems(orderDoc, documents);

				// Step 3:  Add Payment to Order
				mimeoApi.PopulatePaymentMethod(orderDoc);

				// Step 4:  Add Recipient
				List<Address> addresses = new List<Address>{
					new Address {
						firstName = this.txtFName.Text,
						lastName = this.txtLName.Text,
						street = this.txtStreet.Text,
						city= this.txtCity.Text,
						state = this.txtState.Text,
						country = this.txtCountry.Text,
						postalCode = this.txtZip.Text,
						telephone = this.txtPhone.Text
					}
				};
				mimeoApi.PopulateRecipients(orderDoc, addresses);

				// Step 5:  Add Delivery Options:  Additional Processing Time to 24hrs
				mimeoApi.SpecifyProcessingHours(orderDoc, 24);
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}

		}

		private string OrderDocument(XDocument docXml, bool placeOrder)
		{
			string OrderFriendlyId = "-1";
			try
			{
				var orderDoc = new XmlDocument();
				orderDoc.XmlResolver = null;
				setupOrder(orderDoc);

				// Select 2nd day delivery for Recipient 1
				mimeoApi.setShippingOption(orderDoc, ddlShipOptions.SelectedItem.Value, 0);

				// Step 7 - Get Quote
				

				if(placeOrder)
				{
					mimeoApi.PlaceOrder(orderDoc);
					this.txtOutput.Text = XElement.Parse(orderDoc.InnerXml).ToString();
				}
				else
				{
					XmlDocument orderQuoteDoc = mimeoApi.GetQuote(orderDoc);
					this.txtOutput.Text = XElement.Parse(orderQuoteDoc.InnerXml).ToString();
				}

			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}

			return OrderFriendlyId;
		}



		#endregion


	}
}