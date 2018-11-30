<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="edit-report-objects.aspx.cs"
    Inherits="Assette.Web.ReportsGallery.Admin.edit_report_objects" MasterPageFile="~/Admin/AdminMasterPage.master" %>

<asp:Content ID="content_editsamples" ContentPlaceHolderID="Content" runat="Server">
    <%/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////%>
    <%--StyleSheet Reference--%>
    <link href="../Styles/Admin/StyleSheet_A_EditReportObjects.css" rel="stylesheet"
        type="text/css" />
    <link href="../Scripts/jQuery/jquery-ui/themes/base/jquery.ui.all.css" rel="stylesheet"
        type="text/css" />
    <link href="../Scripts/jQuery/JQGrid/ui.jqgrid.css" rel="stylesheet" type="text/css" />
    <%--Script Reference--%>
    <script src="../Scripts/js/Admin/Script_A_EditReportObjects.js" type="text/javascript"></script>
    <%--Script Reference jqgrid--%>
    <script src="../Scripts/jQuery/JQGrid/grid.locale-en.js" type="text/javascript"></script>
    <script src="../Scripts/jQuery/JQGrid/jquery.jqGrid.min.js" type="text/javascript"></script>
    <script src="../Scripts/jQuery/jquery.json-2.2.js" type="text/javascript"></script>
    <%--icon btn--%>
    <link href="../Scripts/jQuery/FileUploader/icon-btn.css" rel="stylesheet" type="text/css" />
    <%--Message bar--%>
    <link href="../Scripts/jQuery/MessageBar/jquery.messagebar.singleline.css" rel="stylesheet"
        type="text/css" />
    <script src="../Scripts/jQuery/MessageBar/jquery.messagebar.singleline.js" type="text/javascript"></script>
    <%/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////%>
    <div class="r1">
        <div style="display: inline; float: right;">
            <div id="triggerMockUp" class="btn btn-primary" style="margin-bottom: 10px; outline: none;">
                <i class="icon-upload icon-white"></i>&nbsp;&nbsp;Mockup Objects</div>
            <div id="triggerCategory" class="btn btn-primary" style="margin-bottom: 10px; outline: none;">
                <i class="icon-upload icon-white"></i>&nbsp;&nbsp;Category Order</div>
        </div>
        <div style="display: none; float: left;">
            <asp:Button ID="btnExport" runat="server" Text="Create - Client Reporting Object Library"
                OnClick="btnExport_Click" /></div>
        <br />
        <br />
        <table id="gridReportObjects">
        </table>
        <div id="pagerReportObjects">
        </div>
        <div id="message_bar" style="border-radius: 4px; -moz-border-radius: 4px;">
        </div>
    </div>
</asp:Content>
