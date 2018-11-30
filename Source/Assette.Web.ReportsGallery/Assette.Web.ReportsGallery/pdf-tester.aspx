<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pdf-tester.aspx.cs" Inherits="Assette.Web.ReportsGallery.pdf_tester" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btnCreatePdf" runat="server" Text="Create Pdf - itextsharp" OnClick="btnCreatePdf_Click" />
    </div>
    <div>
        <asp:Button ID="bttnCreatePdf" runat="server" Text="Create Pdf - pdfsharp" 
            onclick="bttnCreatePdf_Click" />
    </div>
    </form>
</body>
</html>
