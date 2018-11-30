<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Assette.Web.ReportsGallery.index" %>

<!DOCTYPE html>
<html>
<head>
    <title>Examples of pitch books and client reports by investment management firms | Assette
    </title>
    <meta name="keywords" content="pitch books, sales presentations, investment management">
    <%--StyleSheet Reference--%>
    <link href="Styles/Client/StyleSheet_C_Default.css?v=33" rel="stylesheet" type="text/css" />
    <%--Script Reference - json--%>
    <script src="Scripts/jQuery/json2.js" type="text/javascript"></script>
    <script src="Scripts/jQuery/jquery-1.7.2.js" type="text/javascript"></script>
    <%--Script Reference - page--%>
    <script src="Scripts/js/Client/Script_C_Default.js" type="text/javascript"></script>
    <script src="Scripts/js/Client/Script_C_Base.js" type="text/javascript"></script>
    <%--Media query StyleSheet Reference--%>
    <link rel="stylesheet" media="screen and (min-width: 768px) and (max-width: 1024px) and (orientation : landscape)"
        href="Styles/Client/iPad/Landscape/StyleSheet_C_ClientWelcome_768-1024-Landscape.css" />
    <link rel="stylesheet" media="screen and (min-width: 768px) and (max-width: 1024px) and (orientation : portrait) "
        href="Styles/Client/iPad/Portrait/StyleSheet_C_ClientWelcome_768-1024-Portrait.css" />
    <%--Script Reference for image slider--%>
    <script src="Scripts/jQuery/ImageSlider/jquery.imageslider.js" type="text/javascript"></script>
    <link href="Scripts/jQuery/ImageSlider/jquery.imageslider.css" rel="stylesheet" type="text/css" />
    <%--Script Reference for PreView--%>
    <script src="Scripts/js/Client/Script_C_PreViewSamples.js" type="text/javascript"></script>
    <script src="Scripts/js/Client/Script_C_PreViewTemplateDesigns.js" type="text/javascript"></script>
    <script src="Scripts/js/Client/Script_C_PreViewReportObjects.js" type="text/javascript"></script>
    <%--Script Reference for slideshow--%>
    <link href="Scripts/jQuery/SlideShow/jquery.slideshow.PreViewSamples.css" rel="stylesheet"
        type="text/css" />
    <link href="Scripts/jQuery/SlideShow/jquery.slideshow.PreViewTemplateDesigns.css"
        rel="stylesheet" type="text/css" />
    <link href="Scripts/jQuery/SlideShow/jquery.slideshow.PreViewReportObjects.css" rel="stylesheet"
        type="text/css" />
    <script src="Scripts/jQuery/SlideShow/jquery.slideshow.PreViewReportObjects.js" type="text/javascript"></script>
    <script src="Scripts/jQuery/SlideShow/jquery.slideshow.PreViewSamples.js" type="text/javascript"></script>
    <script src="Scripts/jQuery/SlideShow/jquery.slideshow.PreViewTemplateDesigns.js"
        type="text/javascript"></script>
    <%--Script Reference for slideshow / modal dialog--%>
    <link href="Scripts/jQuery/jquery-ui/themes/base/jquery.ui.all.css" rel="stylesheet"
        type="text/css" />
    <script src="Scripts/jQuery/jquery-ui/ui/jquery.ui.core.js" type="text/javascript"></script>
    <script src="Scripts/jQuery/jquery-ui/ui/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="Scripts/jQuery/jquery-ui/ui/jquery.ui.mouse.js" type="text/javascript"></script>
    <script src="Scripts/jQuery/jquery-ui/ui/jquery.ui.draggable.js" type="text/javascript"></script>
    <script src="Scripts/jQuery/jquery-ui/ui/jquery.ui.position.js" type="text/javascript"></script>
    <script src="Scripts/jQuery/jquery-ui/ui/jquery.ui.resizable.js" type="text/javascript"></script>
    <script src="Scripts/jQuery/jquery-ui/ui/jquery.ui.dialog.js" type="text/javascript"></script>
    <%--Script Reference for message bar--%>
    <script src="Scripts/jQuery/MessageBar/jquery.messagebar.singleline.js" type="text/javascript"></script>
    <link href="Scripts/jQuery/MessageBar/jquery.messagebar.singleline.css" rel="stylesheet"
        type="text/css" />
    <%--Script Reference for dot dot dot--%>
    <script src="Scripts/jQuery/Dotdotdot/jquery.dotdotdot.js" type="text/javascript"></script>
    <%--Script Reference for tool tip--%>
    <script src="Scripts/jQuery/ToopTip/jquery.tooltip.js" type="text/javascript"></script>
    <link href="Scripts/jQuery/ToopTip/jquery.tooltip.css" rel="stylesheet" type="text/css" />
    <%--icon btn--%>
    <link href="Scripts/jQuery/Icons/icon-btn.css" rel="stylesheet" type="text/css" />
    <%/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////%>
    <asp:Literal ID="ltlHeader" runat="server" />
</head>
<body>
    <form id="Form1" class="Form1" runat="server">
    <div id="divLoading" class="r24">
        <span style="vertical-align: middle;">
            <img src='Images/Loading.gif' alt="" /></span> <span class="r25">Loading ...</span>
    </div>
    <div class="r0">
        <div title="Go to Assette Home">
            <a id="A1" href="../" class="r37">Assette Home</a>
        </div>
        <div class="r1">
            <div class="r2" title="Assette © 2013">
                <a href="default.aspx" style="outline: none;">
                    <img id="AssetteImage" src="Images/AssetteLogo.png" alt="Assette" class="r9" /></a>
            </div>
        </div>
        <div class="r6">
            <%--<span class="r7">This gallery showcases examples of how leading investment</span>
            <br />
            <span class="r33">management firms use Assette</span>--%>
            <span class="r7">This gallery showcases examples of how leading investment management
                firms use Assette</span>
            <p class="r8">
                An overview of what you can browse is below. You will be requested to register by
                filling out a simple form prior to viewing the gallery.</p>
        </div>
        <div class="r3" title="Go to examples of our work">
            <ul>
                <li><a href="samples.aspx" class="r4">Examples of Our Work</a></li>
                <li><a href="samples.aspx" class="r5">View sales pitch books, client presentations and
                    quarterly reports created using Assette.</a> </li>
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
        <div class="r10">
            <div class="r11" id="lnkRegister" title="Click to register and view gallery">
                REGISTER AND VIEW GALLERY
            </div>
        </div>
        <div class="r12">
            <div class="r13">
                Want to take a peek before registering? Click on an area below.
            </div>
        </div>
        <div class="r14">
            <div class="r3">
                <div class="r22">
                    <div class="r23" title="Click to view examples" id="SamplesPreView" runat="server">
                    </div>
                    <div class="r19">
                        Examples of Our Work
                    </div>
                </div>
            </div>
            <div class="r3">
                <div class="r22">
                    <div class="r23" title="Click to view report objects" id="ReportObjectsPreView" runat="server">
                    </div>
                    <div class="r19">
                        Report Layouts
                    </div>
                </div>
            </div>
            <div class="r3">
                <div class="r22">
                    <div class="r23" title="Click to view designs" id="DesignsPreView" runat="server">
                    </div>
                    <div class="r19">
                        Designs
                    </div>
                </div>
            </div>
        </div>
        <div class="r18">
            <div class="r34">
                <br />
                <hr class="r35" />
                Assette © 2013
                <%--Assette © 2013 | Go back to <a href="/" class="r36">www.assette.com</a>--%>
                <br />
                <br />
                &nbsp;
            </div>
        </div>
        <div id="dialog-modal">
        </div>
        <div id="message_bar" style="border-radius: 4px; -moz-border-radius: 4px;">
        </div>
        <input id="hdnFireBugConsoleLogging" name="hdnFireBugConsoleLogging" type="hidden" />
        <input id="hdnDelayTime" name="hdnDelayTime" type="hidden" runat="server" />
    </div>
    </form>
    <!-- Start of Async HubSpot Analytics Code -->
    <script type="text/javascript">
        (function (d, s, i, r) {
            if (d.getElementById(i)) { return; }
            var n = d.createElement(s), e = d.getElementsByTagName(s)[0];
            n.id = i; n.src = '//js.hubspot.com/analytics/' + (Math.ceil(new Date() / r) * r) + '/279299.js';
            e.parentNode.insertBefore(n, e);
        })(document, "script", "hs-analytics", 300000);
    </script>
    <!-- End of Async HubSpot Analytics Code -->
    <!-- google analytics -->
    <script type="text/javascript">
        var _gaq = _gaq || [];
        _gaq.push(['_setAccount', 'UA-32165472-1']);
        _gaq.push(['_setDomainName', 'assette.com']);
        _gaq.push(['_trackPageview']);

        (function () {
            var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
            ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
            var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
        })();
    </script>
    <!-- google analytics -->
</body>
</html>
