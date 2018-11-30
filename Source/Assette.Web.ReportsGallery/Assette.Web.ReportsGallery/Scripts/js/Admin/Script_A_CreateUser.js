///////////////////////////////////////////////////////
//********** Assette Gallery       **********//
//********** Assette © 2013                **********//
///////////////////////////////////////////////////////

// document ready //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// jquery document ready function
$(document).ready(function () {

    //debugger;

    // loading image functionality
    $("#divLoading").ajaxStart(OnAjaxStart);
    $("#divLoading").ajaxError(OnAjaxError);
    $("#divLoading").ajaxSuccess(OnAjaxSuccess);
    $("#divLoading").ajaxStop(OnAjaxStop);
    $("#divLoading").ajaxComplete(OnAjaxComplete);

    // create user button click
    $('#btnCreate').click(function () {

        UserRegistration();
    });

    // key press - enter
    $(document).keypress(function (e) {
        if (e.which == 13) {

            //UserRegistration();
        }
    });

    // go back button click
    $('#goBack').live("click", function (e) {
        //debugger;

        location.href = "clients.aspx";
    });

    // set selected tab link  
    $('#clients').css('background-color', '#C1D82F');
    $('#editsamplereports').css('background-color', '#999999');
    $('#edittemplatedesigns').css('background-color', '#999999');
    $('#editreportobjects').css('background-color', '#999999');
    $('#editobjectcategoryorder').css('background-color', '#999999');

    // window resize event
    WindowReSize();

    // window resize event
    $(window).resize(function () {

        // window resize event
        WindowReSize();
    });

    // collapsible
    $("#fieldset1").collapse();
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

        return false;
    }
}

// form validation //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function Validate() {
    //debugger;

    // get values
    var firstName = $('#FirstName').val();
    var lastName = $('#LastName').val();
    var jobTitle = $('#JobTitle').val();
    var emailRegister = $('#EmailRegister').val();
    var company = $('#Company').val();

    var retValue = true;
    var errorRows = [];

    if (firstName == '') {
        errorRows.push("First Name field is blank");
        $('#FirstName').css({ 'border': '1px solid #FFCCCC' })
        retValue = false;
    }
    else {
        $('#FirstName').css({ 'border': '1px solid #000000' })
    }

    if (lastName == '') {
        errorRows.push("Last Name field is blank");
        $('#LastName').css({ 'border': '1px solid #FFCCCC' })
        retValue = false;
    }
    else {
        $('#LastName').css({ 'border': '1px solid #000000' })
    }

    if (jobTitle == '') {
        errorRows.push("Job Title field is blank");
        $('#JobTitle').css({ 'border': '1px solid #FFCCCC' })
        retValue = false;
    }
    else {
        $('#JobTitle').css({ 'border': '1px solid #000000' })
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
        $('#EmailRegister').css({ 'border': '1px solid #000000' })
    }

    if (company == '') {
        errorRows.push("Company field is blank");
        $('#Company').css({ 'border': '1px solid #FFCCCC' })
        retValue = false;
    }
    else {
        $('#Company').css({ 'border': '1px solid #000000' })
    }

    if (retValue != true) {
        retValue = errorRows;
    }

    if (retValue != true) {
        retValue = errorRows;
    }

    return retValue;
}

// create new user /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function CreateNewUser() {
    //debugger;

    // get values
    var firstName = $('#FirstName').val();
    var lastName = $('#LastName').val();
    var jobTitle = $('#JobTitle').val();
    var emailRegister = $('#EmailRegister').val();
    var company = $('#Company').val();

    $.ajax({
        type: "POST",
        url: "create-user.aspx/CreateNewUser",
        data: JSON.stringify({ firstName: firstName, lastName: lastName, jobTitle: jobTitle, email: emailRegister, company: company }),
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

        var message = "Client successfully created.";

        // show message bar
        $('#message_bar').displayMessage({
            message: message,
            background: '#111111',
            color: '#FFFFFF',
            speed: 'slow',
            skin: 'green',
            position: 'fixed',
            autohide: true
        });

        // clear fields
        $('#FirstName').val('');
        $('#LastName').val('');
        $('#JobTitle').val('');
        $('#EmailRegister').val('');
        $('#Company').val('');
    }
    else {

        var message = "An Unexpected Error Has Occurred.";

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

    return false;
}

// loading screen functionality /////////////////////////////////////////////////////////////////////////////////////////////////////////////

function OnAjaxStart() {
    //debugger;

    var windowWidth = $(window).width();
    var windowHeight = $(window).height();

    $("#divLoading").css("left", windowWidth / 2 + 50);
    $("#divLoading").css("top", windowHeight / 2 + 70);

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
    var position = $("#btnCreate").offset();
    var windowWidth = $(window).width();

    $("#message_bar").css("left", windowWidth / 2 - 108);
    $("#message_bar").css("top", position.top + 70);
}

