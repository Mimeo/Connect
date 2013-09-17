<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" CodeBehind="GetLibrary.aspx.cs" Inherits="ApiWebApp.WebForm3" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Get Library Calls</title>
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
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
    <form id="form1" runat="server">
        <h1>My Mimeo Connect API Test Harness</h1>

        <p>
            <asp:Label ID="Label11" runat="server" Text="Login User/Password:"></asp:Label>
            <asp:TextBox ID="txtUser" runat="server" Height="24px" Width="150px">user@mimeo.com</asp:TextBox>&nbsp;/&nbsp;
					<asp:TextBox ID="txtPassword" runat="server" Height="24px" Width="100px">password</asp:TextBox>

        </p>
        <div id="rblDiv ">
            <asp:RadioButtonList ID="rButtonList" runat="server"
                RepeatColumns="2"
                RepeatDirection="Vertical"
                RepeatLayout="Table">
                <asp:ListItem Value="sandbox" Selected="True">Sandbox</asp:ListItem>
                <asp:ListItem Value="production">Production</asp:ListItem>
            </asp:RadioButtonList>
        </div>

        <p>
            <asp:Label ID="Label1" runat="server" Text="Folder:"></asp:Label>
            <asp:TextBox ID="txtDocumentFolder" runat="server" Height="24px" Width="150px"></asp:TextBox>&nbsp;
            <asp:Button ID="btnGetDocuments" runat="server" Text="GetDocuments" OnClick="btnGetDocuments_Click" />
        </p>

          <p>
            <asp:Label ID="Label2" runat="server" Text="Folder:"></asp:Label>
            <asp:TextBox ID="txtFileFolder" runat="server" Height="24px" Width="150px"></asp:TextBox>&nbsp;
            <asp:Button ID="bntGetFiles" runat="server" Text="GetFiles" OnClick="btnGetFiles_Click" Width="131px" />
        </p>

        <p>
            <asp:Label ID="Label3" runat="server" Text="Folder Id:"></asp:Label>
            <asp:TextBox ID="txtStoreItemFolder" runat="server" Height="24px" Width="150px"></asp:TextBox>&nbsp;
            <asp:Label ID="Label4" runat="server" Text="Page:"></asp:Label>
            <asp:TextBox ID="txtstoreItemPage" runat="server" Height="24px" Width="150px"></asp:TextBox>&nbsp;
            <asp:Button ID="FindStoreItemFolder" runat="server" Text="FindStoreItem" OnClick="btnFindStoreItem_Click" Width="131px" />
        </p>

        <p>
            <asp:TextBox ID="txtOutput" runat="server" Height="800px" Width="800px" TextMode="MultiLine" Style="margin-top: 0px"></asp:TextBox>
        </p>
    </form>
</body>
</html>
