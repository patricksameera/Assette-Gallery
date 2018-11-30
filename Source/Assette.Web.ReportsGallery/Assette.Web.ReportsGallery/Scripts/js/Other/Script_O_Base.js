///////////////////////////////////////////////////////
//********** Assette Gallery       **********//
//********** Assette © 2013                **********//
///////////////////////////////////////////////////////

// client side logging /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function ClientSideLogging(message) {

    $.ajax({
        type: "POST",
        url: "login.aspx/ClientSideLogging",
        data: JSON.stringify({ message: message }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OnClientSideLoggingSuccess,
        error: OnClientSideLoggingError
    });
}

function OnClientSideLoggingError(request, status, error) {
    //debugger;

    // browser loggin
    fireBugConsoleLog(request, status, error);
}

function OnClientSideLoggingSuccess(response) {
    //debugger;
}

// firebug console log /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function fireBugConsoleLog(request, status, error) {
    if (typeof console != "undefined") {
        console.log(request);
        console.log("responseText: " + request.responseText);
        console.log("status: " + status);
        console.log("error: " + error);
    }
}

// show error to user /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function showErrorToUser(errorMessage) {
    var message;

    if (errorMessage != "") {
        message = "An Unexpected Error Has Occurred." + "</br>" + errorMessage;
    }
    else {
        message = "An Unexpected Error Has Occurred.";
    }

    // hide loading image
    $("#divLoading").css("display", "none");

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

// email validation ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function isValidEmailAddress(emailAddress) {
    //    var pattern = new RegExp(/^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i);
    //    return pattern.test(emailAddress);

    var pattern = new RegExp(/^[+a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/i);
    return pattern.test(emailAddress);
};

// get query string value ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function getQueryStringParameterByName(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.search);
    if (results == null)
        return "";
    else
        return decodeURIComponent(results[1].replace(/\+/g, " "));
}