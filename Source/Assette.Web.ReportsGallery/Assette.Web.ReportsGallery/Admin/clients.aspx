<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="clients.aspx.cs" Inherits="Assette.Web.ReportsGallery.Admin.clients"
    MasterPageFile="~/Admin/AdminMasterPage.master" %>

<asp:Content ID="content_clients" ContentPlaceHolderID="Content" runat="Server">
    <%/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////%>
    <%--StyleSheet Reference--%>
    <link href="../Styles/Admin/StyleSheet_A_Clients.css" rel="stylesheet" type="text/css" />
    <link href="../Scripts/jQuery/jquery-ui/themes/base/jquery.ui.all.css" rel="stylesheet"
        type="text/css" />
    <link href="../Scripts/jQuery/JQGrid/ui.jqgrid.css" rel="stylesheet" type="text/css" />
    <%--Script Reference--%>
    <script src="../Scripts/js/Admin/Script_A_Clients.js" type="text/javascript"></script>
    <%--Script Reference jqgrid--%>
    <script src="../Scripts/jQuery/JQGrid/grid.locale-en.js" type="text/javascript"></script>
    <script src="../Scripts/jQuery/JQGrid/jquery.jqGrid.min.js" type="text/javascript"></script>
    <script src="../Scripts/jQuery/jquery.json-2.2.js" type="text/javascript"></script>
    <%--Message bar--%>
    <link href="../Scripts/jQuery/MessageBar/jquery.messagebar.singleline.css" rel="stylesheet"
        type="text/css" />
    <script src="../Scripts/jQuery/MessageBar/jquery.messagebar.singleline.js" type="text/javascript"></script>
    <%--Clip board--%>
    <script src="../Scripts/jQuery/ClipBoard/jquery.zeroclipboard.js" type="text/javascript"></script>
    <script src="../Scripts/jQuery/ClipBoard/jquery.zclip.js" type="text/javascript"></script>
    <%--icon btn--%>
    <link href="../Scripts/jQuery/FileUploader/icon-btn.css" rel="stylesheet" type="text/css" />
    <%/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////%>
    <div class="c1">
        <div id="triggerCreateClient" class="btn btn-primary" style="margin-bottom: 10px;
            outline: none;">
            <i class="icon-upload icon-white"></i>&nbsp;&nbsp;Create A Prospect</div>
        <table id="gridClients">
        </table>
        <div id="pagerClients">
        </div>
        <div id="message_bar" style="border-radius: 4px; -moz-border-radius: 4px;">
        </div>
        <div style="display: none">
            <div id='copy'>
                Test</div>
            <button id='copy-button'>
                copy</button></div>
        <input id="hdnUrl" name="hdnUrl" type="hidden" runat="server" />
    </div>
</asp:Content>
