<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" CodeBehind="GetLocalDate.aspx.cs" Inherits="ApiWebApp.WebForm2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Convert UTC to Local Date</title>
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js" ></script>
</head>

    <script type="text/javascript">

        $(document).ready(function () {

            $("#FromUTC").click(function () {
               
            });

});

</script>

<body>
	<form id="form1" runat="server">
	<h1>Convert UTC to Local Datetime</h1>

        <asp:Label ID="Label2" runat="server" Text="UTC Date:"></asp:Label>
        <asp:TextBox ID="txtInDate" runat="server" Width="156px"></asp:TextBox>
        
                <asp:Button ID="btnConvertDate" runat="server" Text="Convert to Local" OnClick="btnConvertDate_Click" />

        <asp:TextBox ID="txtOutDate" runat="server" Width="238px"></asp:TextBox>

    </form>
	

</body>
</html>
