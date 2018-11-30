<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="edit-head-tag.aspx.cs"
    Inherits="Assette.Web.ReportsGallery.Admin.edit_head_tag" MasterPageFile="~/Admin/AdminMasterPage.master" %>

<asp:Content ID="content_editheadtag" ContentPlaceHolderID="Content" runat="Server">
    <%/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////%>
    <%--StyleSheet Reference--%>
    <link href="../Styles/Admin/StyleSheet_A_EditHeadTag.css" rel="stylesheet" type="text/css" />
    <%--Script Reference--%>
    <script src="../Scripts/js/Admin/Script_A_EditHeadTag.js" type="text/javascript"></script>
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
        <fieldset id="fieldset1" class="collapsible">
            <legend style="cursor: pointer">&nbsp;Edit Head Tag Content&nbsp;</legend>
            <div>
                <div class="s2">
                    Content:</div>
                <div class="s3">
                    <textarea id='txtContent' class="s1" tabindex='0' autofocus='autofocus'></textarea>
                </div>
                <div class="s7" title="click to edit tag content">
                    <input id="btnCreate" type="button" value="Save" class="s8" />
                </div>
            </div>
        </fieldset>
    </div>
    <div id="message_bar" style="border-radius: 4px; -moz-border-radius: 4px;">
    </div>
    <div id="divLoading" class="s9">
        <span style="vertical-align: middle;">
            <img src='../Images/Loading.gif' alt="Image not found." /></span> <span class="s10">
                Processing ...</span>
    </div>
</asp:Content>
