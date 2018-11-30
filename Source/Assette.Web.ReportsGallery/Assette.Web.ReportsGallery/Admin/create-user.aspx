<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="create-user.aspx.cs" Inherits="Assette.Web.ReportsGallery.Admin.create_user"
    MasterPageFile="~/Admin/AdminMasterPage.master" %>

<asp:Content ID="content_createuser" ContentPlaceHolderID="Content" runat="Server">
    <%/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////%>
    <%--Script Reference--%>
    <script src="../Scripts/js/Admin/Script_A_CreateUser.js" type="text/javascript"></script>
    <%--StyleSheet Reference--%>
    <link href="../Styles/Admin/StyleSheet_A_CreateUser.css" rel="stylesheet" type="text/css" />
    <%--Collapsible--%>
    <link href="../Scripts/jQuery/Collapsible/jquery.collapsible.css" rel="stylesheet"
        type="text/css" />
    <script src="../Scripts/jQuery/Collapsible/jquery.collapsible.js" type="text/javascript"></script>
    <%--Message bar--%>
    <script src="../Scripts/jQuery/MessageBar/jquery.messagebar.multipleline.js" type="text/javascript"></script>
    <link href="../Scripts/jQuery/MessageBar/jquery.messagebar.multipleline.css" rel="stylesheet"
        type="text/css" />
    <%--icon buttons--%>
    <link href="../Scripts/jQuery/FileUploader/icon-btn.css" rel="stylesheet" type="text/css" />
    <%/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////%>
    <div class="s0">
        <div id="goBack" class="btn btn-primary s11" style="margin-bottom: 10px; outline: none;">
            <i class="icon-hand-left icon-white"></i>&nbsp;&nbsp;Go Back</div>
        <fieldset id="fieldset1" class="collapsible">
            <legend style="cursor: pointer">&nbsp;Create Prospect&nbsp;</legend>
            <div>
                <div class="s2">
                    First Name:</div>
                <div class="s3">
                    <input id="FirstName" type="text" class="s1" maxlength="200" /></div>
                <div class="s2">
                    Last Name:</div>
                <div class="s3">
                    <input id="LastName" type="text" class="s1" maxlength="200" />
                </div>
                <div class="s5">
                    Job Title:</div>
                <div class="s6">
                    <input id="JobTitle" type="text" class="s1" maxlength="200" />
                </div>
                <div class="s2">
                    Email:</div>
                <div class="s3">
                    <input id="EmailRegister" type="text" class="s1" maxlength="100" /></div>
                <div class="s2">
                    Company:</div>
                <div class="s3">
                    <input id="Company" type="text" class="s1" maxlength="200" /></div>
                <div class="s7" title="click to create client">
                    <input id="btnCreate" type="button" value="Create" class="s8" />
                </div>
            </div>
        </fieldset>
    </div>
    <div id="message_bar" style="border-radius: 4px; -moz-border-radius: 4px;">
    </div>
    <div id="divLoading" class="s9">
        <span style="vertical-align: middle;">
            <img src='../Images/Loading.gif' alt="Image not found." /></span> <span class="s10">Processing
                ...</span>
    </div>
</asp:Content>
