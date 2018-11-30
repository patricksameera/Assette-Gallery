<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Assette.Web.ReportsGallery.login" %>

<!DOCTYPE html>
<html>
<head>
    <title>Register for Gallery | Assette</title>
    <%/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////%>
    <%--StyleSheet Reference--%>
    <link href="Styles/Client/StyleSheet_C_Login.css?v=33" rel="stylesheet" type="text/css" />
    <%--Script Reference--%>
    <script src="Scripts/jQuery/json2.js" type="text/javascript"></script>
    <script src="Scripts/jQuery/jquery-1.7.2.js" type="text/javascript"></script>
    <script src="Scripts/jQuery/Cookie/jquery.cookie.js" type="text/javascript"></script>
    <script src="Scripts/js/Client/Script_C_Base.js" type="text/javascript"></script>
    <script src="Scripts/js/Client/Script_C_Login.js" type="text/javascript"></script>
    <script src="Scripts/jQuery/WaterMark/jquery.watermark.js" type="text/javascript"></script>
    <%--Message bar--%>
    <link href="Scripts/jQuery/MessageBar/jquery.messagebar.multipleline.css" rel="stylesheet"
        type="text/css" />
    <script src="Scripts/jQuery/MessageBar/jquery.messagebar.multipleline.js" type="text/javascript"></script>
    <%--Session--%>
    <script src="Scripts/jQuery/Session/jquery.session.js" type="text/javascript"></script>
    <%--SelectBox--%>
    <link href="Scripts/jQuery/SelectBoxLogin/jquery.selectbox.css" rel="stylesheet"
        type="text/css" />
    <script src="Scripts/jQuery/SelectBoxLogin/jquery.selectbox.js" type="text/javascript"></script>
    <%/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////%>
    <asp:Literal ID="ltlHeader" runat="server" />
</head>
<body>
    <form id="Form1" class="Form1" runat="server">
    <div title="Go to Assette Home">
        <a id="A1" href="../" class="a34">Assette Home</a>
    </div>
    <div id="loginimage" class="a23" style="top: 212px; left: 700px; position: absolute;">
        <div class="a30">
            REGISTER</div>
    </div>
    <div id="divLoading" class="a24">
        <span style="vertical-align: middle;">
            <img src='Images/Loading.gif' alt="" /></span> <span class="a25">Processing ...</span>
    </div>
    <div class="a1">
        <div class="a2">
            <div id="assetteimage" class="a4" title="Assette © 2013">
                <img src="Images/AssetteLogo.png" width="299" height="113" alt="Assette" />
            </div>
        </div>
        <div class="a3">
            <div class="a5">
                <div class="a6">
                    <div class="a7">
                        <div class="a9" title="Please enter first name">
                            <input type="text" id="FirstName" class="a26" maxlength="200" />
                        </div>
                        <div class="a11">
                        </div>
                        <div class="a9" title="Please enter last name">
                            <input type="text" id="LastName" class="a26" maxlength="200" />
                        </div>
                        <div class="a11">
                        </div>
                        <div class="a9" title="Please enter email">
                            <input type="text" id="EmailRegister" class="a26" maxlength="100" />
                        </div>
                        <div class="a11">
                        </div>
                        <div class="a9" title="Please enter title">
                            <input type="text" id="Title" class="a26" maxlength="100" />
                        </div>
                        <div class="a11">
                        </div>
                        <div class="a36" title="Please select job function">
                            <asp:DropDownList ID="drpJobFunction" runat="server" CssClass="selectInput">
                                <asp:ListItem Value="0" Selected="True">
                    SELECT JOB FUNCTION *
                                </asp:ListItem>
                                <asp:ListItem Value="Executive Management">
                    Executive Management
                                </asp:ListItem>
                                <asp:ListItem Value="Marketing, Sales, Client Service">
                    Marketing, Sales, Client Service
                                </asp:ListItem>
                                <asp:ListItem Value="Portfolio Management">
                    Portfolio Management
                                </asp:ListItem>
                                <asp:ListItem Value="Operations">
                    Operations
                                </asp:ListItem>
                                <asp:ListItem Value="Compliance">
                    Compliance
                                </asp:ListItem>
                                <asp:ListItem Value="IT">
                    IT
                                </asp:ListItem>
                                <asp:ListItem Value="Other">
                   Other
                                </asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="a11">
                        </div>
                        <div class="a9" title="Please enter name of the firm">
                            <input type="text" id="FirmName" class="a26" maxlength="200" />
                        </div>
                        <div class="a11">
                        </div>
                        <div class="a36" title="Please select firm aum">
                            <asp:DropDownList ID="drpFirmAum" runat="server" CssClass="selectInput">
                                <asp:ListItem Value="0" Selected="True">
                    SELECT FIRM AUM *
                                </asp:ListItem>
                                <asp:ListItem Value="Less than $500M">
                    Less than $500M
                                </asp:ListItem>
                                <asp:ListItem Value="$500M - $1B">
                    $500M - $1B
                                </asp:ListItem>
                                <asp:ListItem Value="$1B - $5B">
                    $1B - $5B
                                </asp:ListItem>
                                <asp:ListItem Value="$5B - $10B">
                    $5 - 10B
                                </asp:ListItem>
                                <asp:ListItem Value="$10B - $15B">
                    $10 - 15$
                                </asp:ListItem>
                                <asp:ListItem Value="More than $15B">
                    More than $15B
                                </asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="a11">
                        </div>
                        <div class="a36" title="Please select frim type">
                            <asp:DropDownList ID="drpFirmType" runat="server" CssClass="selectInput">
                                <asp:ListItem Value="0" Selected="True">
                    SELECT FIRM TYPE *
                                </asp:ListItem>
                                <asp:ListItem Value="Institutional Asset Manager">
                    Institutional Asset Manager
                                </asp:ListItem>
                                <asp:ListItem Value="Manager of Managers/OCIO">
                    Manager of Managers/OCIO
                                </asp:ListItem>
                                <asp:ListItem Value="Wealth Manager/Advisor">
                    Wealth Manager/Advisor
                                </asp:ListItem>
                                <asp:ListItem Value="Institutional Consultant">
                   Institutional Consultant
                                </asp:ListItem>
                                <asp:ListItem Value="Other">
                    Other
                                </asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="a15">
                            <div class="a17" title="Click to get register and view gallery">
                                <input id="btnRegister" type="button" value="View Gallery" class="a18" />
                            </div>
                        </div>
                    </div>
                    <div class="a8">
                        <div class="a13">
                            Already registered?
                        </div>
                        <div class="a19" title="Please enter email">
                            <div>
                                <input type="text" id="EmailLogin" class="a27" maxlength="200" /></div>
                        </div>
                        <div class="a21" title="Click to view gallery">
                            <input id="btnLogin" type="button" value="View Gallery" class="a22" />
                        </div>
                    </div>
                </div>
                <div class="a35">
                    * Required</div>
                <div id="message_bar" style="border-radius: 4px; -moz-border-radius: 4px;">
                </div>
            </div>
            <div class="a31">
                <br />
                <br />
                <br />
                <hr class="a32" />
                Assette © 2013
                <%-- Assette © 2013 | Go back to <a href="/" class="a33">www.assette.com--%></a>
            </div>
        </div>
        <div id="ReportsGalleryVersion" runat="server" style="display: none">
        </div>
        <input id="hdnPage" type="hidden" runat="server" />
        <input id="hdnUser" type="hidden" runat="server" />
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
