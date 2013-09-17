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
	public partial class WebForm2 : System.Web.UI.Page
	{
		public static XNamespace nsESLOrder = "http://schemas.mimeo.com/EnterpriseServices/2008/09/OrderService";
		public static RestApi mimeoApi = RestApi.GetInstance;

		protected void Page_Load(object sender, EventArgs e)
		{
          

		}

        protected void btnConvertDate_Click(object sender, EventArgs e)
        {
            this.txtOutDate.Text = this.txtInDate.Text;

            DateTime convertedDate = DateTime.SpecifyKind(DateTime.Parse(this.txtInDate.Text), DateTimeKind.Utc);
            this.txtOutDate.Text = convertedDate.ToLocalTime().ToString();
            //DateTime newDate = convertedDate.ToLocalTime()
            //string varTest = string.Format(
            //  CultureInfo.CurrentCulture,
            //  "localTime = {0}, localTime.Kind = {1}",
            //  newDate,
            //  newDate.Kind);
            //this.txtOutDate.Text = varTest;

        }

		#region Actions

		#endregion


		#region Workers


		#endregion


	}
}