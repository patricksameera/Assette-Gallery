<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="edit-sample-reports.aspx.cs"
    Inherits="Assette.Web.ReportsGallery.Admin.edit_sample_reports" MasterPageFile="~/Admin/AdminMasterPage.master" %>

<asp:Content ID="content_editsamples" ContentPlaceHolderID="Content" runat="Server">
    <%/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////%>
    <%--StyleSheet Reference--%>
    <link href="../Styles/Admin/StyleSheet_A_EditSampleReports.css" rel="stylesheet"
        type="text/css" />
    <link href="../Scripts/jQuery/jquery-ui/themes/base/jquery.ui.all.css" rel="stylesheet"
        type="text/css" />
    <link href="../Scripts/jQuery/JQGrid/ui.jqgrid.css" rel="stylesheet" type="text/css" />
    <%--Script Reference--%>
    <script src="../Scripts/js/Admin/Script_A_EditSampleReports.js" type="text/javascript"></script>
    <%--Script Reference jqgrid--%>
    <script src="../Scripts/jQuery/JQGrid/grid.locale-en.js" type="text/javascript"></script>
    <script src="../Scripts/jQuery/JQGrid/jquery.jqGrid.min.js" type="text/javascript"></script>
    <script src="../Scripts/jQuery/jquery.json-2.2.js" type="text/javascript"></script>
    <%--icon btn--%>
    <link href="../Scripts/jQuery/FileUploader/icon-btn.css" rel="stylesheet" type="text/css" />
    <%--ajax file uploader--%>
    <script src="../Scripts/jQuery/AjaxFileUploader/ajaxfileupload.js" type="text/javascript"></script>
    <%--Message bar--%>
    <link href="../Scripts/jQuery/MessageBar/jquery.messagebar.singleline.css" rel="stylesheet"
        type="text/css" />
    <script src="../Scripts/jQuery/MessageBar/jquery.messagebar.singleline.js" type="text/javascript"></script>
    <%/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////%>
    <div class="c1">
        <div id="triggerUpload" class="btn btn-primary" style="margin-bottom: 10px; outline: none;">
            <i class="icon-upload icon-white"></i>&nbsp;&nbsp;Upload A File</div>
        <table id="gridSamples">
        </table>
        <div id="pagerSamples">
        </div>
        <div id="message_bar" style="border-radius: 4px; -moz-border-radius: 4px;">
        </div>
    </div>
</asp:Content>
