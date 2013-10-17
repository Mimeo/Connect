<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" CodeBehind="GetQuote.aspx.cs" Inherits="ApiWebApp.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Test Mimeo Connect API</title>
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js" ></script>
</head>

    <script type="text/javascript">

        $(document).ready(function () {

            $("#<%=rButtonList.ClientID%>").change(function () {
                var rbvalue = $("input[name='<%=rButtonList.UniqueID%>']:radio:checked").val();

                if (rbvalue == "sandbox") {
                    $('#txtDocumentId').val('a4cfb527-8462-490f-a77a-44e45a53c9d8');
                    $('#txtDocName').val('Sandbox Test');
                } else if (rbvalue == "production") {
                    $('#txtDocumentId').val('12e317d9-5470-4778-9f50-9ab10a88ab0b');
                    $('#txtDocName').val('Production Test');
                } else {
                    alert("this is default click");
                }
            });

});

</script>

<body>
	<h1>My Mimeo Connect API Test Harness</h1>
	<form id="form1" runat="server">
		<asp:Table runat="server">
			<asp:TableRow>
				<asp:TableCell VerticalAlign="Top">
					<p>
					<asp:Label ID="Label11" runat="server" Text="Login User/Password:"></asp:Label>
					<asp:TextBox ID="txtUser" runat="server" Height="24px" Width="150px">jmoncada@mimeo.com</asp:TextBox>&nbsp;/&nbsp;
					<asp:TextBox ID="txtPassword" runat="server" Height="24px" Width="100px">testing!</asp:TextBox>
					
					</p>
                        <div id="rblDiv ">
                        <asp:RadioButtonList id="rButtonList" runat="server"       
                            RepeatColumns = "2"
                            RepeatDirection="Vertical"
                            RepeatLayout="Table" >
                            <asp:ListItem value="sandbox" Selected="True">Sandbox</asp:ListItem>
                            <asp:ListItem value="production">Production</asp:ListItem>
                        </asp:RadioButtonList></div>
                        
                
					<asp:Label ID="Label1" runat="server" Text="Document Id:"></asp:Label>
					<asp:TextBox ID="txtDocumentId" runat="server" Height="24px" Width="250px">a4cfb527-8462-490f-a77a-44e45a53c9d8</asp:TextBox>
					<p>
						<asp:Label ID="Label2" runat="server" Text="Document Name:"></asp:Label>
						<asp:TextBox ID="txtDocName" runat="server" Height="24px" Width="250px">Test </asp:TextBox>
					</p>
					<p>
						<b>Address:</b>
						<asp:Table ID="Table1" runat="server">
							<asp:TableRow>
								<asp:TableCell>
									<asp:Label ID="Label4" runat="server" Text="First Name"></asp:Label>
								</asp:TableCell>
								<asp:TableCell>
									<asp:TextBox ID="txtFName" runat="server" Height="24px" Width="250px">Will </asp:TextBox>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow>
								<asp:TableCell>
									<asp:Label ID="Label3" runat="server" Text="Last Name"></asp:Label>
								</asp:TableCell>
								<asp:TableCell>
									<asp:TextBox ID="txtLName" runat="server" Height="24px" Width="250px">Smith </asp:TextBox>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow>
								<asp:TableCell>
									<asp:Label ID="Label5" runat="server" Text="Street"></asp:Label>
								</asp:TableCell>
								<asp:TableCell>
									<asp:TextBox ID="txtStreet" runat="server" Height="24px" Width="250px">460 Park Ave S.</asp:TextBox>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow>
								<asp:TableCell>
									<asp:Label ID="Label6" runat="server" Text="City"></asp:Label>
								</asp:TableCell>
								<asp:TableCell>
									<asp:TextBox ID="txtCity" runat="server" Height="24px" Width="250px">New York</asp:TextBox>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow>
								<asp:TableCell>
									<asp:Label ID="Label7" runat="server" Text="State"></asp:Label>
								</asp:TableCell>
								<asp:TableCell>
									<asp:TextBox ID="txtState" runat="server" Height="24px" Width="250px">NY</asp:TextBox>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow>
								<asp:TableCell>
									<asp:Label ID="Label8" runat="server" Text="Zip"></asp:Label>
								</asp:TableCell>
								<asp:TableCell>
									<asp:TextBox ID="txtZip" runat="server" Height="24px" Width="250px">10016</asp:TextBox>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow>
								<asp:TableCell>
									<asp:Label ID="Label9" runat="server" Text="Country"></asp:Label>
								</asp:TableCell>
								<asp:TableCell>
									<asp:TextBox ID="txtCountry" runat="server" Height="24px" Width="250px">US</asp:TextBox>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow>
								<asp:TableCell>
									<asp:Label ID="Label10" runat="server" Text="Phone"></asp:Label>
								</asp:TableCell>
								<asp:TableCell>
									<asp:TextBox ID="txtPhone" runat="server" Height="24px" Width="250px">212-333-4444</asp:TextBox>
								</asp:TableCell>
							</asp:TableRow>
						</asp:Table>
					</p>
					<p>
						<asp:DropDownList ID="ddlShipOptions" runat="server">
						</asp:DropDownList>
						<asp:Button ID="btnGetShippingOps" runat="server" OnClick="btnGetShippingOps_Click" Text="Get Shipping Options" />
					</p>
					<asp:Button ID="bntClear" CausesValidation="false" runat="server" OnClick="Clear_Click" Text="Clear Window" />&nbsp;&nbsp;&nbsp;
		<asp:Button ID="btnGetQuote" runat="server" OnClick="GetQuote_Click" Text="Get Quote" />&nbsp;&nbsp;&nbsp;
		<asp:Button ID="btnPlaceOrder" runat="server" OnClick="PlaceOrder_Click" Text="Place Order" />


				</asp:TableCell>
				<asp:TableCell VerticalAlign="Top">
					<p style="width: 888px">
						<asp:TextBox ID="txtOutput" runat="server" Height="800px" Width="800px" TextMode="MultiLine" Style="margin-top: 0px"></asp:TextBox>
					</p>
				</asp:TableCell>
			</asp:TableRow>
		</asp:Table>


	</form>
</body>
</html>
