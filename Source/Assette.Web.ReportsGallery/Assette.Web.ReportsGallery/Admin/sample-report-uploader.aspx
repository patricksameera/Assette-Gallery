<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sample-report-uploader.aspx.cs"
    Inherits="Assette.Web.ReportsGallery.Admin.sample_report_uploader" MasterPageFile="~/Admin/AdminMasterPage.master" %>

<asp:Content ID="content_samplereportuploader" ContentPlaceHolderID="Content" runat="Server">
    <%/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////%>
    <%--StyleSheet Reference--%>
    <link href="../Styles/Admin/StyleSheet_A_SampleReportUploader.css" rel="stylesheet"
        type="text/css" />
    <%--Script Reference selectbox--%>
    <link href="../Scripts/jQuery/SelectBox/jquery.selectbox.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jQuery/SelectBox/jquery.selectbox.js" type="text/javascript"></script>
    <%--Script Reference uploader--%>
    <link href="../Scripts/jQuery/FileUploader/jquery.fileuploader.css" rel="stylesheet"
        type="text/css" />
    <link href="../Scripts/jQuery/FileUploader/bootstrap.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/Admin/Script_A_SampleReportUploader.js" type="text/javascript"></script>
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
    <div class="s0">
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
            <legend style="cursor: pointer">&nbsp;Step 02: Enter the details of the file to be uploaded&nbsp;</legend>
            <div>
                <div class="s2">
                    Name:</div>
                <div class="s3">
                    <input id="txtName" type="text" class="s1" /></div>
                <div class="s2">
                    Short Description:</div>
                <div class="s3">
                    <input id="txtShortDescription" type="text" class="s1" />
                </div>
                <div class="s5">
                    Long Description:</div>
                <div class="s6">
                    <textarea id="txtLongDescription" class="s4" rows="2" cols="40"></textarea>
                </div>
                <div class="s2">
                    Product Type:</div>
                <div class="s3">
                    <select id="Product" name="product">
                    </select></div>
                <div class="s2">
                    Firm Type:</div>
                <div class="s3">
                    <select id="Firm" name="firm">
                    </select></div>
                <div class="s2">
                    Preview:</div>
                <div class="s3">
                    <input id="chkPreview" type="checkbox" />
                </div>
            </div>
        </fieldset>
        <fieldset id="fieldset3" class="collapsible">
            <legend style="cursor: pointer">&nbsp;Step 03: Upload the selected file&nbsp;</legend>
            <div id="triggerUpload" class="btn btn-primary" style="margin-top: 10px; outline: none;">
                <i class="icon-upload icon-white"></i>&nbsp;&nbsp;Upload Now</div>
        </fieldset>
        <div id="message_bar" style="border-radius: 4px; -moz-border-radius: 4px;">
        </div>
    </div>
</asp:Content>
