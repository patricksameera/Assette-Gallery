///////////////////////////////////////////////////////
//********** Assette Gallery               **********//
//********** Assette © 2013                **********//
//********** Developed by Sameera Jayalath **********//
///////////////////////////////////////////////////////

//debugger;

// for testing purpose - clear cookie manually
//var cookieName = 'cookierptglry';
//document.cookie = cookieName + '=; expires=Thu, 01 Jan 1970 00:00:01 GMT;'; 

// email validation
var emailExists = "1";

// document ready //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// jquery document ready function
$(document).ready(function () {
    //debugger;

    // window resize event
    WindowReSize();

    // window resize event
    $(window).resize(function () {

        // window resize event
        WindowReSize();
    });

    // text box focus
    $('.a26').focus(function () {
        //debugger;
        $(this).css({ 'border': '1px solid #66cc00' })
    });

    // text box blur
    $('.a26').blur(function () {
        //debugger;
        //$(this).css({ 'border-left': '1px solid #E5E5E5', 'border-right': '1px solid #E5E5E5', 'border-top': '0px solid #F2F2F2', 'border-bottom': '0px solid #F2F2F2' });
        $(this).css({ 'border-left': '1px solid #F2F2F2', 'border-right': '1px solid #F2F2F2', 'border-top': '0px solid #F2F2F2', 'border-bottom': '0px solid #F2F2F2' });
    });

    // text box focus
    $('.a27').focus(function () {
        //debugger;
        $(this).css({ 'border': '1px solid #66cc00' })
    });

    // text box blur
    $('.a27').blur(function () {
        //debugger;
        /*$(this).css({ 'border-left': '1px solid #E5E5E5', 'border-right': '1px solid #E5E5E5', 'border-top': '1px solid #F2F2F2', 'border-bottom': '1px solid #F2F2F2' })*/
        /*$(this).css({ 'border-left': '1px solid #E5E5E5', 'border-right': '1px solid #E5E5E5', 'border-top': '1px solid #ffffff', 'border-bottom': '1px solid #ffffff' });*/
        $(this).css({ 'border-left': '1px solid #ffffff', 'border-right': '1px solid #ffffff', 'border-top': '1px solid #ffffff', 'border-bottom': '1px solid #ffffff' })
    });

    $('.selectInput').focus(function () {
        //debugger;

        $(this).css('border', '1px solid #66cc00');
    });

    $('.selectInput').blur(function () {
        //debugger;

        $(this).css('border', '1px solid #EBEBEB');
    });

    // key press - enter
    $(document).keypress(function (e) {
        if (e.which == 13) {
            //debugger;

            if ($('#EmailLogin').is(":focus")) {
                CheckEmailLogin();
            }
            else {
                UserRegistration();
            }
        }
    });

    // register button click
    $('#btnRegister').click(function () {

        UserRegistration();
    });

    // check email existence
    /*
    $("#EmailRegister").blur(function () {

    CheckEmailExistence();
    });*/

    // copy email text to register-email
    $("#EmailRegister").focus(function () {
        //debugger;

        CopyEmailFieldContent();

    });

    // login button click
    $('#btnLogin').click(function () {

        CheckEmailLogin();
    });

    // loading image functionality
    $("#divLoading").ajaxStart(OnAjaxStart);
    $("#divLoading").ajaxError(OnAjaxError);
    $("#divLoading").ajaxSuccess(OnAjaxSuccess);
    $("#divLoading").ajaxStop(OnAjaxStop);
    $("#divLoading").ajaxComplete(OnAjaxComplete);

    // water mark
    $("#FirstName").Watermark("FIRST NAME *");
    $("#LastName").Watermark("LAST NAME *");
    $("#Title").Watermark("TITLE *");
    $("#EmailRegister").Watermark("EMAIL *");
    $("#FirmName").Watermark("FIRM NAME *");
    $("#EmailLogin").Watermark("EMAIL *");

    // focus
    //$("#FirstName").focus();

    // select box
    //$("#drpJobFunction").selectbox('attach');
    //$("#drpFirmAum").selectbox('attach');
    //$("#drpFirmType").selectbox('attach');
})

function UserRegistration() {
    //debugger;

    var retValue = Validate();

    if (retValue == true) {
        CreateNewUser();
    }
    else {
        //debugger;

        var message = "Please correct the following errors, then resubmit: </br>";

        for (i = 0; i < retValue.length; i++) {
            message += "* " + retValue[i] + "</br>";
            //message += "<span class='ui-icon ui-icon-alert ui-state-error ui-corner-all' style='margin-right: 0.3em; float: left;'/>" + retValue[i] + "</br>";
            //message += "<span class='a29' style='margin-right: 0.3em; float: left;'/>" + retValue[i] + "</br>";
        }

        // show message bar
        $('#message_bar').displayMessage({
            message: message,
            background: '#111111',
            color: '#FFFFFF',
            speed: 'slow',
            skin: 'red',
            position: 'fixed',
            autohide: false
        });
    }
}

// form validation //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function Validate() {
    //debugger;

    // get values
    var firstName = $.trim($('#FirstName').val());
    var lastName = $.trim($('#LastName').val());
    var title = $.trim($('#Title').val());
    var emailRegister = $.trim($('#EmailRegister').val());
    var firmName = $.trim($('#FirmName').val());
    var jobFunction = $.trim($('#drpJobFunction').val());
    var firmAum = $.trim($('#drpFirmAum').val());
    var firmType = $.trim($('#drpFirmType').val());

    // set initial values - watermark
    if (firstName == "FIRST NAME *") firstName = "";
    if (lastName == "LAST NAME *") lastName = "";
    if (title == "TITLE *") title = "";
    if (emailRegister == "EMAIL *") emailRegister = "";
    if (firmName == "FIRM NAME *") firmName = "";
    if (jobFunction == "0") jobFunction = "0";
    if (firmAum == "0") firmAum = "0";
    if (firmType == "0") firmType = "0";

    var retValue = true;
    var errorRows = [];

    $('.a27').css({ 'border-left': '1px solid #E5E5E5', 'border-right': '1px solid #E5E5E5', 'border-top': '1px solid #F2F2F2', 'border-bottom': '1px solid #F2F2F2' })

    if (firstName == '') {
        errorRows.push("First Name field is blank");
        $('#FirstName').css({ 'border': '1px solid #FFCCCC' })
        retValue = false;
    }
    else {
        $('#FirstName').css({ 'border-left': '1px solid #E5E5E5', 'border-right': '1px solid #E5E5E5', 'border-top': '0px solid #F2F2F2', 'border-bottom': '0px solid #F2F2F2' })
    }

    if (lastName == '') {
        errorRows.push("Last Name field is blank");
        $('#LastName').css({ 'border': '1px solid #FFCCCC' })
        retValue = false;
    }
    else {
        $('#LastName').css({ 'border-left': '1px solid #E5E5E5', 'border-right': '1px solid #E5E5E5', 'border-top': '0px solid #F2F2F2', 'border-bottom': '0px solid #F2F2F2' })
    }

    if (title == '') {
        errorRows.push("Title field is blank");
        $('#Title').css({ 'border': '1px solid #FFCCCC' })
        retValue = false;
    }
    else {
        $('#Title').css({ 'border-left': '1px solid #E5E5E5', 'border-right': '1px solid #E5E5E5', 'border-top': '0px solid #F2F2F2', 'border-bottom': '0px solid #F2F2F2' })
    }

    if (emailRegister == '') {
        errorRows.push("Email field is blank");
        $('#EmailRegister').css({ 'border': '1px solid #FFCCCC' })
        retValue = false;
    }
    else if (!isValidEmailAddress(emailRegister)) {
        errorRows.push("Email is in wrong format");
        $('#EmailRegister').css({ 'border': '1px solid #FFCCCC' })
        retValue = false;
    }
    else {
        // email validation - removed
        /*
        if (emailExists == "1") {
        errorRows.push("Email address already exists");
        $('#EmailRegister').css({ 'border': '1px solid #FFCCCC' })
        retValue = false;
        }
        else {
        $('#EmailRegister').css({ 'border-left': '1px solid #E5E5E5', 'border-right': '1px solid #E5E5E5', 'border-top': '0px solid #F2F2F2', 'border-bottom': '0px solid #F2F2F2' })
        }*/
    }

    if (firmName == '') {
        errorRows.push("Firm name field is blank");
        $('#FirmName').css({ 'border': '1px solid #FFCCCC' })
        retValue = false;
    }
    else {
        $('#FirmName').css({ 'border-left': '1px solid #E5E5E5', 'border-right': '1px solid #E5E5E5', 'border-top': '0px solid #F2F2F2', 'border-bottom': '0px solid #F2F2F2' })
    }

    if (jobFunction == '0') {
        errorRows.push("Please select job function");
        $('#drpJobFunction').css({ 'border': '1px solid #FFCCCC' })
        retValue = false;
    }
    else {
        $('#drpJobFunction').css({ 'border-left': '1px solid #E5E5E5', 'border-right': '1px solid #E5E5E5', 'border-top': '0px solid #F2F2F2', 'border-bottom': '0px solid #F2F2F2' })
    }

    if (firmAum == '0') {
        errorRows.push("Please select firm aum");
        $('#drpFirmAum').css({ 'border': '1px solid #FFCCCC' })
        retValue = false;
    }
    else {
        $('#drpFirmAum').css({ 'border-left': '1px solid #E5E5E5', 'border-right': '1px solid #E5E5E5', 'border-top': '0px solid #F2F2F2', 'border-bottom': '0px solid #F2F2F2' })
    }

    if (firmType == '0') {
        errorRows.push("Please select firm type");
        $('#drpFirmType').css({ 'border': '1px solid #FFCCCC' })
        retValue = false;
    }
    else {
        $('#drpFirmType').css({ 'border-left': '1px solid #E5E5E5', 'border-right': '1px solid #E5E5E5', 'border-top': '0px solid #F2F2F2', 'border-bottom': '0px solid #F2F2F2' })
    }



    if (retValue != true) {
        retValue = errorRows;
    }

    return retValue;
}

// create new user /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function CreateNewUser() {

    // get values
    var firstName = $.trim($('#FirstName').val());
    var lastName = $.trim($('#LastName').val());
    var title = $.trim($('#Title').val());
    var emailRegister = $.trim($('#EmailRegister').val());
    var firmName = $.trim($('#FirmName').val());
    var jobFunction = $.trim($('#drpJobFunction').val());
    var firmAum = $.trim($('#drpFirmAum').val());
    var firmType = $.trim($('#drpFirmType').val());

    $.ajax({
        type: "POST",
        url: "login.aspx/CreateNewUser",
        data: JSON.stringify({ firstName: firstName, lastName: lastName, jobTitle: jobFunction, email: emailRegister, firmName: firmName, title: title, frimAum: firmAum, firmType: firmType }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OnCreateNewUserSuccess,
        error: OnCreateNewUserError
    });
}

function OnCreateNewUserError(request, status, error) {
    //debugger;

    // browser loggin
    fireBugConsoleLog(request, status, error);

    // show error to user
    showErrorToUser("");

    // log4net logging
    var errorText = "OnCreateNewUserError() jQuery Exception: response text= " + request.responseText + ", status= " + status + ", error= " + error;
    ClientSideLogging(errorText);
}

function OnCreateNewUserSuccess(response) {
    //debugger;

    if (response.d == "1") {

        // transfer
        location.href = "client-welcome.aspx";
    }
    else if (response.d == "0") {
        //alert('Record didn't get created.');
    }
    else {
        return false;
    }
}

// check email existence ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function CheckEmailExistence() {
    //debugger;

    // get values
    var emailRegister = $('#EmailRegister').val();

    if (emailRegister != "") {
        $.ajax({
            type: "POST",
            url: "login.aspx/CheckEmailExistence",
            data: "{'email': '" + emailRegister + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnCheckEmailExistenceSuccess,
            error: OnCheckEmailExistenceError
        });
    }
}

function OnCheckEmailExistenceError(request, status, error) {
    //debugger

    // browser loggin
    fireBugConsoleLog(request, status, error);

    // show error to user
    showErrorToUser("");

    // log4net logging
    var errorText = "OnCheckEmailExistenceError() jQuery Exception: response text= " + request.responseText + ", status= " + status + ", error= " + error;
    ClientSideLogging(errorText);
}

function OnCheckEmailExistenceSuccess(response) {
    //debugger;

    if (response.d == "1") {
        $("#divLoading").css("display", "none");

        //alert("Email exists.");
        emailExists = "1";

        //debugger;
        var message = "Error :&nbsp; Email exists";

        // show message bar
        $('#message_bar').displayMessage({
            message: message,
            background: '#111111',
            color: '#FFFFFF',
            speed: 'slow',
            skin: 'red',
            position: 'fixed',
            autohide: true
        });

        return false;
    }
    else {
        emailExists = "0";

        return true;
    }
}

// check email login ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

function CheckEmailLogin() {
    //debugger;

    // get values
    var emailLogin = $('#EmailLogin').val();

    $.ajax({
        type: "POST",
        url: "login.aspx/CheckEmailExistence",
        data: "{'email': '" + emailLogin + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OnCheckCheckEmailLoginSuccess,
        error: OnCheckCheckEmailLoginError
    });

}

function OnCheckCheckEmailLoginError(request, status, error) {
    //debugger;

    // browser loggin
    fireBugConsoleLog(request, status, error);

    // show error to user
    showErrorToUser("");

    // log4net logging
    var errorText = "OnCheckCheckEmailLoginError() jQuery Exception: response text= " + request.responseText + ", status= " + status + ", error= " + error;
    ClientSideLogging(errorText);
}

function OnCheckCheckEmailLoginSuccess(response) {
    //debugger;

    // get page
    var page = $('#hdnPage').val();

    if (response.d == "1") {

        // transfer
        location.href = page;
    }
    else {

        $("#divLoading").fadeOut('fast');

        $('.a26').css({ 'border-left': '1px solid #E5E5E5', 'border-right': '1px solid #E5E5E5', 'border-top': '0px solid #F2F2F2', 'border-bottom': '0px solid #F2F2F2' })
        $('#EmailLogin').css({ 'border': '1px solid #FFCCCC' })

        //debugger;
        var message = "Error:</br>We did not find the email address on the registered list.  Please verify that the address you entered is correct.  If the address is correct, that means you have not registered. Please fill in the information requested on the above-left to register.";

        // show message bar
        $('#message_bar').displayMessage({
            message: message,
            background: '#111111',
            color: '#FFFFFF',
            speed: 'slow',
            skin: 'red',
            position: 'fixed',
            autohide: false
        });

        return false;
    }
}

// loading screen functionality /////////////////////////////////////////////////////////////////////////////////////////////////////////////

function OnAjaxStart() {
    //debugger;

    // positioning log-in image
    var position = $("#assetteimage").offset();

    $("#divLoading").css("left", position.left + 75);
    $("#divLoading").css("top", position.top + 600);

    $("#divLoading").fadeIn('slow');
}

function OnAjaxError() {
    //debugger;
}

function OnAjaxSuccess() {
    //debugger;

    $("#divLoading").fadeOut('slow');
}

function OnAjaxStop() {
    //debugger;

    $("#divLoading").fadeOut('slow');
}

function OnAjaxComplete() {
    //debugger;

    $("#divLoading").fadeOut('slow');
}

// window resize event /////////////////////////////////////////////////////////////////////////////////////////////////////////////

function WindowReSize() {
    //debugger;

    // positioning log-in image
    var position = $("#assetteimage").offset();

    $("#loginimage").css("left", position.left + 60);
    $("#loginimage").css("top", position.top + 210);

    // message bar
    $("#message_bar").css("left", position.left - 50);
    $("#message_bar").css("top", position.top + 410);
}

// copy email content /////////////////////////////////////////////////////////////////////////////////////////////////////////////

function CopyEmailFieldContent() {
    //debugger;

    var emailLogin = $('#EmailLogin').val();

    if (emailLogin != "EMAIL *") {

        // copy email from EmailRegister to EmailLogin
        $('#EmailRegister').val(emailLogin);

        // reset EmailLogin field
        $('#EmailLogin').val('');
        $("#EmailLogin").Watermark("EMAIL");

        $('#EmailLogin').css({ 'border-left': '1px solid #E5E5E5', 'border-right': '1px solid #E5E5E5', 'border-top': '1px solid #F2F2F2', 'border-bottom': '1px solid #F2F2F2' })

        // hide message bar
        $('#message_bar').fadeOut('fast');
    }
}






