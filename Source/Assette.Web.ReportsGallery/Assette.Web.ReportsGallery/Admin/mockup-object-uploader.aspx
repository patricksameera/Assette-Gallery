<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mockup-object-uploader.aspx.cs"
    Inherits="Assette.Web.ReportsGallery.Admin.shell_object_uploader" MasterPageFile="~/Admin/AdminMasterPage.master" %>

<asp:Content ID="content_templatedesignuploader" ContentPlaceHolderID="Content" runat="Server">
    <%/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////%>
    <%--StyleSheet Reference--%>
    <link href="../Styles/Admin/StyleSheet_A_ShellReportUploader.css" rel="stylesheet"
        type="text/css" />
    <%--Script Reference selectbox--%>
    <link href="../Scripts/jQuery/SelectBox/jquery.selectbox.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jQuery/SelectBox/jquery.selectbox.js" type="text/javascript"></script>
    <%--Script Reference uploader--%>
    <link href="../Scripts/jQuery/FileUploader/jquery.fileuploader.css" rel="stylesheet"
        type="text/css" />
    <link href="../Scripts/jQuery/FileUploader/bootstrap.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/Admin/Script_A_ReportObjectMockupsUploader.js" type="text/javascript"></script>
    <script src="../Scripts/jQuery/FileUploader/jquery.fileuploader.js" type="text/javascript"></script>
    <%--Collapsible--%>
    <link href="../Scripts/jQuery/Collapsible/jquery.collapsible.css" rel="stylesheet"
        type="text/css" />
    <script src="../Scripts/jQuery/Collapsible/jquery.collapsible.js" type="text/javascript"></script>
    <%--Message bar--%>
    <script src="../Scripts/jQuery/MessageBar/jquery.messagebar.multipleline.js" type="text/javascript"></script>
    <link href="../Scripts/jQuery/MessageBar/jquery.messagebar.multipleline.css" rel="stylesheet"
        type="text/css" />
    <%/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////%>
    <div class="t0">
        <div id="goBack" class="btn btn-primary s7" style="margin-bottom: 10px; outline: none;">
            <i class="icon-hand-left icon-white"></i>&nbsp;&nbsp;Go Back</div>
        <fieldset id="fieldset1" class="collapsible">
            <legend style="cursor: pointer">&nbsp;Step 01: Select a file to upload&nbsp;</legend>
            <div id="file-uploader-demo" style="outline: none;">
                <noscript>
                    <p>
                        Please enable JavaScript to use file up-loader.</p>
                </noscript>
            </div>
        </fieldset>
        <fieldset id="fieldset2" class="collapsible">
            <legend style="cursor: pointer">&nbsp;Step 02: Upload the selected file&nbsp;</legend>
            <div id="triggerUpload" class="btn btn-primary" style="margin-top: 10px; outline: none;">
                <i class="icon-upload icon-white"></i>&nbsp;&nbsp;Upload Now</div>
        </fieldset>
        <div id="message_bar" style="border-radius: 4px; -moz-border-radius: 4px;">
        </div>
    </div>
</asp:Content>
