<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Assette.Web.ReportsGallery.Admin.login" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
    <%/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////%>
    <%--StyleSheet Reference--%>
    <link href="../Styles/Admin/StyleSheet_A_Login.css" rel="stylesheet" type="text/css" />
    <%--Script Reference--%>
    <script src="../Scripts/jQuery/jquery-1.7.2.js" type="text/javascript"></script>
    <script src="../Scripts/jQuery/json2.js" type="text/javascript"></script>
    <script src="../Scripts/jQuery/jquery.json-2.2.js" type="text/javascript"></script>
    <script src="../Scripts/js/Admin/Script_A_Login.js" type="text/javascript"></script>
    <%--Water mark--%>
    <script src="../Scripts/jQuery/WaterMark/jquery.watermark.js" type="text/javascript"></script>
    <%--Message bar--%>
    <link href="../Scripts/jQuery/MessageBar/jquery.messagebar.singleline.css" rel="stylesheet"
        type="text/css" />
    <script src="../Scripts/jQuery/MessageBar/jquery.messagebar.singleline.js" type="text/javascript"></script>
    <script type="text/javascript">

        function showErrorMessage() {
            //debugger;

            var message = "Error :&nbsp; Invalid Username/Password";

            // show message bar
            $('#message_bar').displayMessage({
                message: message,
                background: '#111111',
                color: '#FFFFFF',
                speed: 'slow',
                skin: 'red',
                position: 'relative',
                autohide: false
            });

            return false;
        }
        
    </script>
    <%/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////%>
</head>
<body>
    <form id="form1" runat="server">
    <div class="l11">
        <div id="assetteimage" class="l12" title="Assette © 2013">
            <img src="../Images/AssetteLogo.png" width="299" height="113" alt="Assette" />
        </div>
    </div>
    <div class="l1">
        <div class="l2">
        </div>
        <div class="l3" title="Please enter username">
            <div>
                <input type="text" id="username" class="l4" maxlength="200" runat="server" /></div>
        </div>
        <div class="l5">
        </div>
        <div class="l3" title="Please enter password">
            <div>
                <input type="password" id="password" class="l4" maxlength="200" runat="server" /></div>
        </div>
        <div class="l6" title="Click to login">
            <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="l7" OnClick="btnLogin_Click" />
        </div>
    </div>
    <div id="message_bar" style="border-radius: 4px; -moz-border-radius: 4px;">
    </div>
    </form>
</body>
</html>
