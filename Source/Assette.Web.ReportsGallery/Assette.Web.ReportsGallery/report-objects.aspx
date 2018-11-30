<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="report-objects.aspx.cs"
    Inherits="Assette.Web.ReportsGallery.reportobjects" MasterPageFile="~/ClientMasterPage.master"
    Title="Report Layouts | Assette Gallery" %>

<asp:Content runat="server" ID="HeaderContent" ContentPlaceHolderID="HeaderContentPlaceHolder">
    <%--Script Reference selectbox--%>
    <link href="Scripts/jQuery/SelectBox/jquery.selectbox.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jQuery/SelectBox/jquery.selectbox.js" type="text/javascript"></script>
    <%--Script Reference watermark--%>
    <script src="Scripts/jQuery/WaterMark/jquery.watermark.js" type="text/javascript"></script>
    <%--Script Reference for slideshow--%>
    <link href="Scripts/jQuery/jquery-ui/themes/base/jquery.ui.all.css" rel="stylesheet"
        type="text/css" />
    <script src="Scripts/jQuery/jquery-ui/ui/jquery.ui.core.js" type="text/javascript"></script>
    <script src="Scripts/jQuery/jquery-ui/ui/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="Scripts/jQuery/jquery-ui/ui/jquery.ui.mouse.js" type="text/javascript"></script>
    <script src="Scripts/jQuery/jquery-ui/ui/jquery.ui.draggable.js" type="text/javascript"></script>
    <script src="Scripts/jQuery/jquery-ui/ui/jquery.ui.position.js" type="text/javascript"></script>
    <script src="Scripts/jQuery/jquery-ui/ui/jquery.ui.resizable.js" type="text/javascript"></script>
    <script src="Scripts/jQuery/jquery-ui/ui/jquery.ui.dialog.js" type="text/javascript"></script>
    <link href="Scripts/jQuery/SlideShow/jquery.slideshow.ReportObjects.css" rel="stylesheet"
        type="text/css" />
    <script src="Scripts/jQuery/SlideShow/jquery.slideshow.ReportObjects.js" type="text/javascript"></script>
    <%--Script Reference for mouse wheel--%>
    <script src="Scripts/jQuery/MouseWheel/jquery.mousewheel.js" type="text/javascript"></script>
    <%--Script Reference for dot dot dot--%>
    <script src="Scripts/jQuery/Dotdotdot/jquery.dotdotdot.js" type="text/javascript"></script>
    <%--Script Reference for tool tip--%>
    <script src="Scripts/jQuery/ToopTip/jquery.tooltip.js" type="text/javascript"></script>
    <link href="Scripts/jQuery/ToopTip/jquery.tooltip.css" rel="stylesheet" type="text/css" />
    <%--Message bar--%>
    <link href="Scripts/jQuery/MessageBar/jquery.messagebar.singleline.css" rel="stylesheet"
        type="text/css" />
    <script src="Scripts/jQuery/MessageBar/jquery.messagebar.singleline.js" type="text/javascript"></script>
    <%--positioning--%>
    <script src="Scripts/jQuery/jquery-ui/ui/jquery.ui.position.js" type="text/javascript"></script>
    <%/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////%>
    <%--StyleSheet Reference--%>
    <link href="Styles/Client/StyleSheet_C_ReportObjects.css?v=33" rel="stylesheet" type="text/css" />
    <%--Script Reference--%>
    <script src="Scripts/js/Client/Script_C_ReportObjects.js" type="text/javascript"></script>
    <%--icon btn--%>
    <link href="Scripts/jQuery/Icons/icon-btn.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="content_reportobjects" ContentPlaceHolderID="Content" runat="Server">
    <%/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////%>
    <div id="divLoading" class="r13">
        <span style="vertical-align: middle;">
            <img src='Images/Loading.gif' alt="" /></span><span class="r14">Loading ...</span>
    </div>
    <div class="r0">
        <div class="r1">
            VIEW :</div>
        <div title="Re-set filters">
            <input id="btnAll" type="button" value="All Objects" class="r16" />
        </div>
        <div class="r2">
            |</div>
        <div title="Filter by object type - charts">
            <input id="btnCharts" type="button" value="Charts" class="r16" />
        </div>
        <div title="Filter by object type - tables">
            <div class="r2">
                |</div>
            <input id="btnTables" type="button" value="Tables" class="r16" />
        </div>
        <div id="divBothItems" style="visibility: hidden">
            <div class="r2">
                |</div>
            <div title="Filter by object type - tables and charts">
                <input id="btnBoth" type="button" value="Both" class="r16" />
            </div>
        </div>
        <div style="float: right;">
            <div class="r18" title="Filter by category">
                <select id="Category" name="category">
                </select>
            </div>
            <div class="r3" title="Filter by product">
                <select id="ClientType" name="clienttype">
                </select>
            </div>
            <div class="r3" title="Filter by text">
                <input type="text" id="Search" class="r4" />
            </div>
            <div title="Refresh search" style="float: right">
                <a href="javascript:void(0);" id="btnSearch" style="outline: none;">
                    <img alt="Refresh Search" src="Images/Refresh.jpg" class="r19" /></a></div>
        </div>
        <div style="display: none">
            <asp:LinkButton ID="lnkCreatePdf" runat="server" OnClick="lnkCreatePdf_Click" CssClass="r22"
                Style="outline: none; padding-left: 10px;" OnLoad="lnkCreatePdf_Load">Export to PDF</asp:LinkButton></div>
    </div>
    <div id="divObjects" class="r5">
    </div>
    <div id="divTopLink" class="r5">
    </div>
    <div id="dialog-modal">
    </div>
    <div id="message_bar" style="border-radius: 4px; -moz-border-radius: 4px;">
    </div>
    <%--<div style="float: right;">
        <a href="#top">
            <img width="73" height="20" border="0" src="Images/top.gif">
        </a>
    </div>--%>
    <input id="hdnFilterType" name="hdnFilterType" type="hidden" />
    <input id="hdnScroll" name="hdnScroll" type="hidden" />
    <input id="hdnDelayTime" name="hdnDelayTime" type="hidden" runat="server" />
    <input id="hdnPaObjectIds" name="hdnReportIds" type="hidden" runat="server" />
    <input id="hdnMockupObjectIds" name="hdnMockupObjectIds" type="hidden" runat="server" />
</asp:Content>
