<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="error.aspx.cs" Inherits="Assette.Web.ReportsGallery.error" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Assette - Gallery</title>
    <%/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////%>
    <%--StyleSheet Reference--%>
    <link href="Styles/Other/StyleSheet_O_Error.css?v=33" rel="stylesheet" type="text/css" />
    <%/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////%>
</head>
<body>
    <form id="form1" runat="server">
    <div class="e1">
        <div id="assetteimage" class="e2" title="Assette © 2013">
            <img src="Images/AssetteLogo.png" width="299" height="113" alt="Assette" />
        </div>
        <div class="e3">
            An error occurred while performing your request. Sorry for any convenience. Please
            go to <a href="default.aspx" style="outline: none;">Home</a> page.</div>
        <div style="display: none">
            <asp:Label ID="lblError" runat="server" Text=""></asp:Label></div>
    </div>
    </form>
</body>
</html>
