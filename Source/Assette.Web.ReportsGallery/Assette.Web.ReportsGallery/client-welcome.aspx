<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="client-welcome.aspx.cs"
    Inherits="Assette.Web.ReportsGallery.client_welcome" MasterPageFile="~/ClientMasterPage.Master"
    Title="Home | Assette Gallery" %>

<asp:Content ID="content_clientwelcome" ContentPlaceHolderID="Content" runat="Server">
    <%/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////%>
    <%--StyleSheet Reference--%>
    <link href="Styles/Client/StyleSheet_C_ClientWelcome.css?v=33" rel="stylesheet" type="text/css" />
    <%--Script Reference--%>
    <script src="Scripts/js/Client/Script_C_ClientWelcome.js" type="text/javascript"></script>
    <%--Media query StyleSheet Reference--%>
    <link rel="stylesheet" media="screen and (min-width: 768px) and (max-width: 1024px) and (orientation : landscape)"
        href="Styles/Client/iPad/Landscape/StyleSheet_C_ClientWelcome_768-1024-Landscape.css" />
    <link rel="stylesheet" media="screen and (min-width: 768px) and (max-width: 1024px) and (orientation : portrait) "
        href="Styles/Client/iPad/Portrait/StyleSheet_C_ClientWelcome_768-1024-Portrait.css" />
    <%/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////%>
    <div class="r0">
        <span class="r1">Welcome to the Assette Gallery</span>
        <br />
        <br />
        <span class="r2">Click on a link on the left to get started.</span>
        <br />
        <span class="r2">Below is an overview of areas portrayed in the gallery.</span>
        <br />
        <br />
    </div>
    <div class="r3" title="Go to examples of our work">
        <ul>
            <li><a href="samples.aspx" class="r4">Examples of Our Work</a></li>
            <li><a href="samples.aspx" class="r5">View sales pitch books, client presentations and
                quarterly reports created using Assette.</a></li>
        </ul>
    </div>
    <div class="r3" title="Go to report objects">
        <ul>
            <li><a href="report-objects.aspx" class="r4">Report Layouts</a></li>
            <li><a href="report-objects.aspx" class="r5">See the extensive library of chart and
                table layouts, covering all angles needed to effectively communicate results to
                your clients. You simply place these charts and tables, along with your insights,
                on templates, and Assette automatically provides your data to fill in the content.</a></li>
        </ul>
    </div>
    <div class="r3" title="Go to designs">
        <ul>
            <li><a href="template-designs.aspx" class="r4">Designs</a></li>
            <li><a href="template-designs.aspx" class="r5">View the different template designs that
                are provided with Assette. You can use the designs as is, modify them as you see
                fit or come up with your own designs. The choice is up to you.</a></li>
        </ul>
    </div>
</asp:Content>
