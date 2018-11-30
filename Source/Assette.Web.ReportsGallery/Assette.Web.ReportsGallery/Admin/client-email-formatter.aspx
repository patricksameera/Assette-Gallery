<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="client-email-formatter.aspx.cs"
    Inherits="Assette.Web.ReportsGallery.Admin.client_email_formatter" MasterPageFile="~/Admin/AdminMasterPage.master" %>

<asp:Content ID="content_client_email_formatter" ContentPlaceHolderID="Content" runat="Server">
    <%/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////%>
    <%--StyleSheet Reference--%>
    <link href="../Styles/Admin/StyleSheet_A_ClientEmailFormatter.css" rel="stylesheet"
        type="text/css" />
    <%--Script Reference--%>
    <script src="../Scripts/js/Admin/Script_A_ClientEmailFormatter.js" type="text/javascript"></script>
    <%--Message bar--%>
    <link href="../Scripts/jQuery/MessageBar/jquery.messagebar.singleline.css" rel="stylesheet"
        type="text/css" />
    <script src="../Scripts/jQuery/MessageBar/jquery.messagebar.singleline.js" type="text/javascript"></script>
    <%--icon btn--%>
    <link href="../Scripts/jQuery/FileUploader/icon-btn.css" rel="stylesheet" type="text/css" />
    <%--timy mce--%>
    <script src="../Scripts/jQuery/RichTextFormatter/tinymce/jscripts/tiny_mce/tiny_mce.js"
        type="text/javascript"></script>
    <%/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////%>
    <div class="c1">
        <textarea id="content" name="content" style="width: 100%"></textarea>
        <asp:Button ID="btnPublish" runat="server" Text="Publish" CssClass="button" Height="36px"
            Width="88px" ValidationGroup="postValid" OnClientClick="alert(tinyMCE.get('content').getContent());return false;"
            OnClick="btnPublish_Click" />
    </div>
</asp:Content>
