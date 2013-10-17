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
using System.Globalization;

using Mimeo.MimeoConnect;

namespace ApiWebApp
{
	public partial class WebForm3 : System.Web.UI.Page
	{
		public static XNamespace nsESLOrder = "http://schemas.mimeo.com/EnterpriseServices/2008/09/OrderService";
		public static RestApi mimeoApi = RestApi.GetInstance;

		protected void Page_Load(object sender, EventArgs e)
		{
            bool sandbox = (this.rButtonList.SelectedValue == "sandbox") ? true : false;
            mimeoApi.Initialize(this.txtUser.Text, this.txtPassword.Text, sandbox);

		}

		#region Actions

        protected void btnGetDocuments_Click(object sender, EventArgs e)
        {
            this.txtOutput.Text = string.Empty;

            string folder = (String.IsNullOrEmpty(this.txtDocumentFolder.Text))
                ? ""
                : "/" + this.txtDocumentFolder.Text;
            XDocument apiResult = mimeoApi.GetFolderInfo(folder);

            this.txtOutput.Text = apiResult.ToString();
        }

        protected void btnGetFiles_Click(object sender, EventArgs e)
        {
            this.txtOutput.Text = string.Empty;

            string folder = (String.IsNullOrEmpty(this.txtFileFolder.Text))
                ? ""
                : "/" + this.txtFileFolder.Text;
            XDocument apiResult = mimeoApi.GetPrintFiles(folder);

            this.txtOutput.Text = apiResult.ToString();
        }

        protected void btnFindStoreItem_Click(object sender, EventArgs e)
        {
            this.txtOutput.Text = string.Empty;

            string xmlRequest =
            "<StoreItemSearchCriteria xmlns=\"http://schemas.mimeo.com/EnterpriseServices/2008/09/StorageService\">" +
            "<PageInfo xmlns=\"http://schemas.mimeo.com/EnterpriseServices/2008/09/Common/Search\"><PageSize>10</PageSize><PageNumber>" + this.txtstoreItemPage.Text + "</PageNumber></PageInfo>" +
            "<FolderId>" + this.txtStoreItemFolder.Text + "</FolderId>" +
            "<Type>Document</Type>" +
            "</StoreItemSearchCriteria>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlRequest);

            XmlDocument apiResult = mimeoApi.FindStoreItems(doc);

            this.txtOutput.Text = XElement.Parse(apiResult.InnerXml).ToString();
        }

		#endregion


		#region Workers


		#endregion


	}
}