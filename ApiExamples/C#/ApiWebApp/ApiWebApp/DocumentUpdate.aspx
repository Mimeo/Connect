<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" CodeBehind="DocumentUpdate.aspx.cs" Inherits="ApiWebApp.WebForm4" %>

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
            <asp:TextBox ID="txtUser" runat="server" Height="24px" Width="150px">jmoncada@mimeo.com</asp:TextBox>&nbsp;/&nbsp;
					<asp:TextBox ID="txtPassword" runat="server" Height="24px" Width="100px">testing!</asp:TextBox>

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
            <asp:Label ID="Label2" runat="server" Text="File:"></asp:Label>
            <asp:TextBox ID="txtFile" runat="server" Height="24px" Width="150px"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            
            <asp:Label ID="Label3" runat="server" Text="(Use:  file path for upload, file name for remove)"></asp:Label>
        </p>

        <p> <asp:Button ID="btnUploadPDF" runat="server" Text="Upload Print File" OnClick="btnUploadPrintFile_Click" />&nbsp;&nbsp;
            <asp:Button ID="btnRemovePrintFile" runat="server" Text="Remove Print File" OnClick="btnRemovePrintFile_Click" />&nbsp;&nbsp;
            <asp:Button ID="btnRemoveDocument" runat="server" Text="Remove Document" OnClick="btnRemoveDocument_Click" Width="131px" />
        </p>

         <p>
             Document Id:<asp:TextBox ID="txtUpdateDocumentId" runat="server" Height="24px" Width="150px"></asp:TextBox>&nbsp;
             File Id:<asp:TextBox ID="txtUpdateFileId" runat="server" Height="24px" Width="150px"></asp:TextBox>&nbsp;
             Template Id:<asp:TextBox ID="txtUpdateTemplateId" runat="server" Height="24px" Width="150px"></asp:TextBox> &nbsp;&nbsp;
           </p>
        <p>
            
              <asp:Button ID="btnGetDocument" runat="server" Text="Get Document" OnClick="btnGetDocument_Click" />
              <asp:Button ID="btnUpdateDocument" runat="server" Text="Update Document" OnClick="btnUpdateDocument_Click" />

              <asp:Button ID="btnCreateDocument" runat="server" Text="Create Document" OnClick="btnCreateDocument_Click" />

        </p>
        <p>
            <asp:TextBox ID="txtOutput" runat="server" Height="800px" Width="800px" TextMode="MultiLine" Style="margin-top: 0px"></asp:TextBox>
        </p>
    </form>
</body>
</html>
