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
	public partial class WebForm5 : System.Web.UI.Page
	{
		public static XNamespace nsESLOrder = "http://schemas.mimeo.com/EnterpriseServices/2008/09/OrderService";
		public static RestApi mimeoApi = RestApi.GetInstance;

		protected void Page_Load(object sender, EventArgs e)
		{
            bool sandbox = (this.rButtonList.SelectedValue == "sandbox") ? true : false;
            mimeoApi.Initialize(this.txtUser.Text, this.txtPassword.Text, sandbox);

		}

		#region Actions

        protected void btnGetStatus_Click(object sender, EventArgs e)
        {
            this.txtOutput.Text = string.Empty;

            string friendlyId = (String.IsNullOrEmpty(this.txtFriendlyId.Text))
                ? ""
                : this.txtFriendlyId.Text;

            XDocument apiResult = mimeoApi.GetInfo(friendlyId, "status");

            this.txtOutput.Text = apiResult.ToString();
        }
		#endregion

        protected void btnGetTracking_Click(object sender, EventArgs e)
        {
            this.txtOutput.Text = string.Empty;

            string friendlyId = (String.IsNullOrEmpty(this.txtFriendlyId.Text))
                ? ""
                : this.txtFriendlyId.Text;

            XDocument apiResult = mimeoApi.GetInfo(friendlyId, "tracking");

            this.txtOutput.Text = apiResult.ToString();
        }

        protected void btnGetOrderInfo_Click(object sender, EventArgs e)
        {
            this.txtOutput.Text = string.Empty;

            string friendlyId = (String.IsNullOrEmpty(this.txtFriendlyId.Text))
                ? ""
                : this.txtFriendlyId.Text;

            XDocument apiResult = mimeoApi.GetInfo(friendlyId, "GetOrderHistory");

            this.txtOutput.Text = apiResult.ToString();
        }

      



		#region Workers


		#endregion


	}
}