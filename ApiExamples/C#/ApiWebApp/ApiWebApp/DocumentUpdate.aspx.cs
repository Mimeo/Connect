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
	public partial class WebForm4 : System.Web.UI.Page
	{
		public static XNamespace nsESLOrder = "http://schemas.mimeo.com/EnterpriseServices/2008/09/OrderService";
		public static RestApi mimeoApi = RestApi.GetInstance;

		protected void Page_Load(object sender, EventArgs e)
		{
            bool sandbox = (this.rButtonList.SelectedValue == "sandbox") ? true : false;
            mimeoApi.Initialize(this.txtUser.Text, this.txtPassword.Text, sandbox);

		}

		#region Actions

        protected void btnRemovePrintFile_Click(object sender, EventArgs e)
        {
            this.txtOutput.Text = string.Empty;

            string folder = (String.IsNullOrEmpty(this.txtDocumentFolder.Text))
                ? ""
                : this.txtDocumentFolder.Text;

            string file = (String.IsNullOrEmpty(this.txtFile.Text))
                ? ""
                : this.txtFile.Text;

            XDocument apiResult = mimeoApi.DeletePrintFile(folder, file);

            this.txtOutput.Text = apiResult.ToString();
        }

        protected void btnRemoveDocument_Click(object sender, EventArgs e)
        {
            this.txtOutput.Text = string.Empty;

            string folder = (String.IsNullOrEmpty(this.txtDocumentFolder.Text))
                ? ""
                : this.txtDocumentFolder.Text;

            string file = (String.IsNullOrEmpty(this.txtFile.Text))
                ? ""
                : this.txtFile.Text;

            XDocument apiResult = mimeoApi.DeleteDocument(folder, file);

            this.txtOutput.Text = apiResult.ToString();
        }


        protected void btnUpdateDocument_Click(object sender, EventArgs e)
        {
            this.txtOutput.Text = string.Empty;

            string DocumentId = (String.IsNullOrEmpty(this.txtUpdateDocumentId.Text))
                ? ""
                : this.txtUpdateDocumentId.Text;

            string fileId = (String.IsNullOrEmpty(this.txtUpdateFileId.Text))
                ? ""
                : this.txtUpdateFileId.Text;

            string templateId = (String.IsNullOrEmpty(this.txtUpdateTemplateId.Text))
                ? ""
                : this.txtUpdateTemplateId.Text;


            XDocument apiResult = mimeoApi.updateDocument(DocumentId, fileId, templateId);

            this.txtOutput.Text = apiResult.ToString();
        }


		#endregion

        protected void btnGetDocument_Click(object sender, EventArgs e)
        {
            this.txtOutput.Text = string.Empty;

            string DocumentId = (String.IsNullOrEmpty(this.txtUpdateDocumentId.Text))
                ? ""
                : this.txtUpdateDocumentId.Text;

            XDocument apiResult = mimeoApi.GetDocument(DocumentId);

            this.txtOutput.Text = apiResult.ToString();
        }



		#region Workers


		#endregion


	}
}