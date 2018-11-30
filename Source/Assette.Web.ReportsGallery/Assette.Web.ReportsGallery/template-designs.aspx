<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="template-designs.aspx.cs"
    Inherits="Assette.Web.ReportsGallery.templatedesigns" MasterPageFile="~/ClientMasterPage.master"
    Title="Designs | Assette Gallery" %>

<asp:Content ID="content_templatedesigns" ContentPlaceHolderID="Content" runat="Server">
    <%/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////%>
    <%--StyleSheet Reference--%>
    <link href="Styles/Client/StyleSheet_C_TemplateDesigns.css?v=33" rel="stylesheet" type="text/css" />
    <%--Script Reference--%>
    <script src="Scripts/js/Client/Script_C_TemplateDesigns.js" type="text/javascript"></script>
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
    <link href="Scripts/jQuery/SlideShow/jquery.slideshow.TemplateDesigns.css" rel="stylesheet"
        type="text/css" />
    <script src="Scripts/jQuery/SlideShow/jquery.slideshow.TemplateDesigns.js" type="text/javascript"></script>
    <%--Script Reference for image slider--%>
    <script src="Scripts/jQuery/ImageSlider/jquery.imageslider.js" type="text/javascript"></script>
    <link href="Scripts/jQuery/ImageSlider/jquery.imageslider.css" rel="stylesheet" type="text/css" />
    <%--Script Reference for dot dot dot--%>
    <script src="Scripts/jQuery/Dotdotdot/jquery.dotdotdot.js" type="text/javascript"></script>
    <%--Script Reference for tool tip--%>
    <script src="Scripts/jQuery/ToopTip/jquery.tooltip.js" type="text/javascript"></script>
    <link href="Scripts/jQuery/ToopTip/jquery.tooltip.css" rel="stylesheet" type="text/css" />
    <%--Script Reference for mouse wheel--%>
    <script src="Scripts/jQuery/MouseWheel/jquery.mousewheel.js" type="text/javascript"></script>
    <%--Script Reference for message bar--%>
    <link href="Scripts/jQuery/MessageBar/jquery.messagebar.singleline.css" rel="stylesheet"
        type="text/css" />
    <script src="Scripts/jQuery/MessageBar/jquery.messagebar.singleline.js" type="text/javascript"></script>
    <%--positioning--%>
    <script src="Scripts/jQuery/jquery-ui/ui/jquery.ui.position.js" type="text/javascript"></script>
    <%/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////%>
    <div id="divLoading" class="t13">
        <span style="vertical-align: middle;">
            <img src='Images/Loading.gif' alt="" /></span> <span class="t14">Loading ...</span>
    </div>
    <div class="t0">
        <div class="t1">
            VIEW :</div>
        <div class="t18" title="Re-set filters">
            <input id="btnAll" type="button" value="All Designs" class="t16" />
        </div>
        <div class="t3" title="filter by product" style="visibility: hidden">
            <select id="Product" name="product">
            </select>
        </div>
        <div style="float: right;">
            <div class="t3" title="Filter by client type">
                <select id="Firm" name="firm">
                </select>
            </div>
            <div class="t3" title="Filter by text">
                <input type="text" id="Search" class="t4" />
            </div>
            <div title="Refresh search" style="float: right">
                <a href="javascript:void(0);" id="btnSearch" style="outline: none;">
                    <img alt="Refresh Search" src="Images/Refresh.jpg" class="t19" /></a></div>
        </div>
    </div>
    <div id="divTemplateDesigns" class="t5">
    </div>
    <div id="divTopLink" class="t5">
    </div>
    <div id="dialog-modal">
    </div>
    <div id="message_bar" style="border-radius: 4px; -moz-border-radius: 4px;">
    </div>
    <input id="hdnScroll" type="hidden" />
    <input id="hdnDelayTime" name="hdnDelayTime" type="hidden" runat="server" />
</asp:Content>
