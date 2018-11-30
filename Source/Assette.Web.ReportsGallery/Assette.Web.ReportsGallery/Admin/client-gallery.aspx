<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="client-gallery.aspx.cs"
    Inherits="Assette.Web.ReportsGallery.Admin.client_gallery" MasterPageFile="~/Admin/AdminMasterPage.master" %>

<asp:Content ID="content_clientgallery" ContentPlaceHolderID="Content" runat="Server">
    <%/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////%>
    <%--StyleSheet Reference--%>
    <link href="../Styles/Admin/StyleSheet_A_ClientGallery.css" rel="stylesheet" type="text/css" />
    <%--Script Reference--%>
    <script src="../Scripts/js/Admin/Script_A_ClientGallery.js" type="text/javascript"></script>
    <%--Script Reference for select box--%>
    <link href="../Scripts/jQuery/SelectBox/jquery.selectbox.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jQuery/SelectBox/jquery.selectbox.js" type="text/javascript"></script>
    <%--Script Reference for watermark--%>
    <script src="../Scripts/jQuery/WaterMark/jquery.watermark.js" type="text/javascript"></script>
    <%--Script Reference for slideshow--%>
    <link href="../Scripts/jQuery/jquery-ui/themes/base/jquery.ui.all.css" rel="stylesheet"
        type="text/css" />
    <script src="../Scripts/jQuery/jquery-ui/ui/jquery.ui.core.js" type="text/javascript"></script>
    <script src="../Scripts/jQuery/jquery-ui/ui/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="../Scripts/jQuery/jquery-ui/ui/jquery.ui.mouse.js" type="text/javascript"></script>
    <script src="../Scripts/jQuery/jquery-ui/ui/jquery.ui.draggable.js" type="text/javascript"></script>
    <script src="../Scripts/jQuery/jquery-ui/ui/jquery.ui.position.js" type="text/javascript"></script>
    <script src="../Scripts/jQuery/jquery-ui/ui/jquery.ui.resizable.js" type="text/javascript"></script>
    <script src="../Scripts/jQuery/jquery-ui/ui/jquery.ui.dialog.js" type="text/javascript"></script>
    <link href="../Scripts/jQuery/SlideShow/jquery.slideshow.ClientGallery.css" rel="stylesheet"
        type="text/css" />
    <script src="../Scripts/jQuery/SlideShow/jquery.slideshow.ClientGallery.js" type="text/javascript"></script>
    <%--Script Reference for scroll bar--%>
    <script src="../Scripts/jQuery/jquery-ui/ui/jquery-ui-1.8.21.custom.js" type="text/javascript"></script>
    <script src="../Scripts/jQuery/ScrollBar/jquery.mousewheel.min.js" type="text/javascript"></script>
    <script src="../Scripts/jQuery/ScrollBar/jquery.scrollbar.js" type="text/javascript"></script>
    <link href="../Scripts/jQuery/ScrollBar/jquery.scrollbar.css" rel="stylesheet" type="text/css" />
    <%--icon btn--%>
    <link href="../Scripts/jQuery/FileUploader/icon-btn.css" rel="stylesheet" type="text/css" />
    <%--Message bar--%>
    <link href="../Scripts/jQuery/MessageBar/jquery.messagebar.singleline.css" rel="stylesheet"
        type="text/css" />
    <script src="../Scripts/jQuery/MessageBar/jquery.messagebar.singleline.js" type="text/javascript"></script>
    <%/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////%>
    <div id="divLoading" class="r13">
        <span style="vertical-align: middle;">
            <img src='../Images/Loading.gif' alt="Image not found." /></span> <span class="r14">
                Loading ...</span>
    </div>
    <div class="r0" style="display: none">
        <div>
            <div class="r1">
                VIEW:</div>
            <div>
                <input id="btnAll" type="button" value="All" class="r16" />
            </div>
            <div class="r2">
                |</div>
            <div>
                <input id="btnTables" type="button" value="Tables" class="r16" />
            </div>
            <div class="r2">
                |</div>
            <div>
                <input id="btnCharts" type="button" value="Charts" class="r16" />
            </div>
            <div class="r3">
                <select id="Category" name="category">
                </select>
            </div>
            <div class="r3">
                <select id="ClientType" name="clienttype">
                </select>
            </div>
            <div class="r3">
                <input type="text" id="Search" class="r4" />
            </div>
            <div>
                <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="../Images/Refresh.jpg" CssClass="r5"
                    title="refresh search" /></div>
            <div class="r18">
                <a title='Invite To View Gallery' href='#' class='InviteToViewGallery'>
                    <img class="border" src='../Images/InviteToGallery.jpg' width='129' height='30' alt='Invite To Gallery'
                        style='float: left; padding-right: 0px;' /></a><a title='Print PDF OF All Objects'
                            href='#' class='PrintAllObjects'><img src='../Images/PrintAllObjects.jpg' class="border"
                                width='129' height='30' alt='Print All Objects' style='float: left; padding-left: 30px;' /></a>
            </div>
        </div>
    </div>
    <div id="goBack" class="btn btn-primary r19" style="margin-bottom: 5px; outline: none;">
        <i class="icon-hand-left icon-white"></i>&nbsp;&nbsp;Go Back</div>
    <div id="divGalleryObjects" class="r5">
    </div>
    <div id="dialog-modal">
    </div>
    <div id="message_bar" style="border-radius: 4px; -moz-border-radius: 4px;">
    </div>
</asp:Content>
