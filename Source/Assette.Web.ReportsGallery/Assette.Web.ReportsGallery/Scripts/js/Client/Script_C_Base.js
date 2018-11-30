///////////////////////////////////////////////////////
//********** Assette Gallery       **********//
//********** Assette © 2013                **********//
///////////////////////////////////////////////////////

var FireBugConsoleLogging = 0;

// for all browser logging
if (!window.console) window.console = {};
if (!window.console.log) window.console.log = function () { };

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
    //debugger;

    if (typeof console != "undefined") {

        console.log(request);
        console.log("responseText: " + request.responseText);
        console.log("status: " + status);
        console.log("error: " + error);
    }
}

// show error to user /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function showErrorToUser(errorMessage) {
    //debugger;

    var message;

    /*
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
    position: 'fixed', // relative, absolute, fixed
    autohide: false
    });*/
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
    if (results == null) {
        return "";
    }
    else {
        return decodeURIComponent(results[1].replace(/\+/g, " "));
    }
}

// if error on loading image ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

$(".urlImageSmall").error(function () {
    //debugger;
    $(this).unbind("error").attr("src", "/Images/NoImageFound.jpg");
});

$(".urlImageLarge").error(function () {
    //debugger;
    $(this).unbind("error").attr("src", "/Images/NoImageFound.jpg");
});

// json break characters ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function EscapeJsonBreakCharacters(json) {
    //debugger;

    /*
    Quotation mark (")
    Backslash (\)
    Backspace (b)
    Formfeed (f)
    Newline (n)
    Carriage return (r)
    Horizontal tab (t)
    Four-hexadecimal-digits (uhhhh)
    */

    var rtnJson;

    /*
    rtnJson = json.replace("\n", "\\n");
    rtnJson = rtnJson.replace('\b', '\\b"');
    rtnJson = rtnJson.replace('\f', '\\f"');
    rtnJson = rtnJson.replace('\r', '\\r"');
    rtnJson = rtnJson.replace('\t', '\\t"');
    */

    // replace backslash
    rtnJson = json.replace(/\\/g, "\\\\");

    // replace back space
    //rtnJson = rtnJson.replace(/\b/g, "\\b");

    // replace form feed
    rtnJson = rtnJson.replace(/\f/g, "\\f");

    // replace new line
    rtnJson = rtnJson.replace(/\n/g, "\\n");

    // replace - Carriage return (r)
    rtnJson = rtnJson.replace(/(\r\n|\r|\n)/g, "\\n");

    // replace tab
    rtnJson = rtnJson.replace(/\t/g, "\\t");

    return rtnJson;
}

// ManageControls: Hides and Shows controls depending on currentPosition
function ManageControlsPosition(position) {
    //debugger;

    // Hide left arrow if position is first slide
    if (position == 1) {
        $('#leftControl').hide()
    }
    else {
        $('#leftControl').show()
    }

    // Hide right arrow if position is last slide
    if (position == noOfReports) {
        $('#rightControl').hide()
    }
    else {
        $('#rightControl').show()
    }
}

// GetDialogDimensionsForSamplesAndDesigns: Getting dialog dimensions
function GetDialogDimensionsForSamplesAndDesigns() {
    //debugger;

    var dimensionsObject = new Object();

    // setting modal dialog width
    var windowWidth = $(window).width();

    // scroll bar : 17px

    if (windowWidth >= 1400) {

        //alert('windowWidth: ' + windowWidth + ' [>1400]');

        dimensionsObject.dialogWidth = 1300;
        dimensionsObject.dialogHeight = 640; //dimensionsObject.sliderImageHeight + 175
        dimensionsObject.navigationControlWidth = 40;
        dimensionsObject.difference = 10;
        dimensionsObject.sliderWidth = dimensionsObject.dialogWidth - 100;
        dimensionsObject.sliderHeight = dimensionsObject.dialogHeight - 80;
        dimensionsObject.sliderImageWidth = 690;
        dimensionsObject.sliderImageHeight = parseInt(dimensionsObject.sliderImageWidth * 0.675); // 465
        dimensionsObject.sliderLeftWidth = dimensionsObject.sliderImageWidth + dimensionsObject.difference;
        dimensionsObject.sliderCenterWidth = 30;
        dimensionsObject.sliderRightWidth = dimensionsObject.sliderWidth - dimensionsObject.sliderLeftWidth - dimensionsObject.sliderCenterWidth - dimensionsObject.difference;
        dimensionsObject.sliderLeftHeight = dimensionsObject.sliderHeight - dimensionsObject.difference;
        dimensionsObject.sliderRightTopWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightTopHeight = 100;
        dimensionsObject.sliderRightBellowWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightBellowHeight = 0;
        dimensionsObject.sliderRightDescriptionWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightDescriptionHeight = 500;
    }
    else if (windowWidth >= 1300 && windowWidth < 1400) {

        //alert('windowWidth: ' + windowWidth + ' [1300/1400]');

        dimensionsObject.dialogWidth = 1200;
        dimensionsObject.dialogHeight = 580;
        dimensionsObject.navigationControlWidth = 40;
        dimensionsObject.difference = 10;
        dimensionsObject.sliderWidth = dimensionsObject.dialogWidth - 100;
        dimensionsObject.sliderHeight = dimensionsObject.dialogHeight - 80;
        dimensionsObject.sliderImageWidth = 600;
        dimensionsObject.sliderImageHeight = parseInt(dimensionsObject.sliderImageWidth * 0.675); // 405
        dimensionsObject.sliderLeftWidth = dimensionsObject.sliderImageWidth + dimensionsObject.difference;
        dimensionsObject.sliderCenterWidth = 30;
        dimensionsObject.sliderRightWidth = dimensionsObject.sliderWidth - dimensionsObject.sliderLeftWidth - dimensionsObject.sliderCenterWidth - dimensionsObject.difference;
        dimensionsObject.sliderLeftHeight = dimensionsObject.sliderHeight - dimensionsObject.difference;
        dimensionsObject.sliderRightTopHeight = 100;
        dimensionsObject.sliderRightBellowWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightBellowHeight = 0;
        dimensionsObject.sliderRightDescriptionWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightDescriptionHeight = 440;

    }
    else if (windowWidth >= 1200 && windowWidth < 1300) {

        //alert('windowWidth: ' + windowWidth + ' [1200/1300]');

        dimensionsObject.dialogWidth = 1100;
        dimensionsObject.dialogHeight = 540;
        dimensionsObject.navigationControlWidth = 40;
        dimensionsObject.difference = 10;
        dimensionsObject.sliderWidth = dimensionsObject.dialogWidth - 100;
        dimensionsObject.sliderHeight = dimensionsObject.dialogHeight - 80;
        dimensionsObject.sliderImageWidth = 540;
        dimensionsObject.sliderImageHeight = parseInt(dimensionsObject.sliderImageWidth * 0.675); // 365
        dimensionsObject.sliderLeftWidth = dimensionsObject.sliderImageWidth + dimensionsObject.difference;
        dimensionsObject.sliderCenterWidth = 30;
        dimensionsObject.sliderRightWidth = dimensionsObject.sliderWidth - dimensionsObject.sliderLeftWidth - dimensionsObject.sliderCenterWidth - dimensionsObject.difference;
        dimensionsObject.sliderLeftHeight = dimensionsObject.sliderHeight - dimensionsObject.difference;
        dimensionsObject.sliderRightTopHeight = 100;
        dimensionsObject.sliderRightBellowWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightBellowHeight = 0;
        dimensionsObject.sliderRightDescriptionWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightDescriptionHeight = 400;
    }
    else if (windowWidth >= 1100 && windowWidth < 1200) {

        //alert('windowWidth: ' + windowWidth + ' [1100/1200]');

        dimensionsObject.dialogWidth = 1000;
        dimensionsObject.dialogHeight = 515;
        dimensionsObject.navigationControlWidth = 40;
        dimensionsObject.difference = 10;
        dimensionsObject.sliderWidth = dimensionsObject.dialogWidth - 100;
        dimensionsObject.sliderHeight = dimensionsObject.dialogHeight - 80;
        dimensionsObject.sliderImageWidth = 500;
        dimensionsObject.sliderImageHeight = parseInt(dimensionsObject.sliderImageWidth * 0.675); // 340
        dimensionsObject.sliderLeftWidth = dimensionsObject.sliderImageWidth + dimensionsObject.difference;
        dimensionsObject.sliderCenterWidth = 30;
        dimensionsObject.sliderRightWidth = dimensionsObject.sliderWidth - dimensionsObject.sliderLeftWidth - dimensionsObject.sliderCenterWidth - dimensionsObject.difference;
        dimensionsObject.sliderLeftHeight = dimensionsObject.sliderHeight - dimensionsObject.difference;
        dimensionsObject.sliderRightTopHeight = 100;
        dimensionsObject.sliderRightBellowWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightBellowHeight = 0;
        dimensionsObject.sliderRightDescriptionWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightDescriptionHeight = 375;
    }
    else if (windowWidth >= 1000 && windowWidth < 1100) {

        //alert('windowWidth: ' + windowWidth + ' [1000/1100]');

        dimensionsObject.dialogWidth = 900;
        dimensionsObject.dialogHeight = 485;
        dimensionsObject.navigationControlWidth = 40;
        dimensionsObject.difference = 10;
        dimensionsObject.sliderWidth = dimensionsObject.dialogWidth - 100;
        dimensionsObject.sliderHeight = dimensionsObject.dialogHeight - 80;
        dimensionsObject.sliderImageWidth = 460;
        dimensionsObject.sliderImageHeight = parseInt(dimensionsObject.sliderImageWidth * 0.675);
        dimensionsObject.sliderLeftWidth = dimensionsObject.sliderImageWidth + dimensionsObject.difference; // 310
        dimensionsObject.sliderCenterWidth = 30;
        dimensionsObject.sliderRightWidth = dimensionsObject.sliderWidth - dimensionsObject.sliderLeftWidth - dimensionsObject.sliderCenterWidth - dimensionsObject.difference;
        dimensionsObject.sliderLeftHeight = dimensionsObject.sliderHeight - dimensionsObject.difference;
        dimensionsObject.sliderRightTopHeight = 100;
        dimensionsObject.sliderRightBellowWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightBellowHeight = 0;
        dimensionsObject.sliderRightDescriptionWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightDescriptionHeight = 345;
    }
    else if (windowWidth >= 900 && windowWidth < 1000) {

        //alert('windowWidth: ' + windowWidth + ' [900/1000]');

        dimensionsObject.dialogWidth = 800;
        dimensionsObject.dialogHeight = 485;
        dimensionsObject.navigationControlWidth = 40;
        dimensionsObject.difference = 10;
        dimensionsObject.sliderWidth = dimensionsObject.dialogWidth - 100;
        dimensionsObject.sliderHeight = dimensionsObject.dialogHeight - 80;
        dimensionsObject.sliderImageWidth = 380;
        dimensionsObject.sliderImageHeight = parseInt(dimensionsObject.sliderImageWidth * 0.675); // 260
        dimensionsObject.sliderLeftWidth = dimensionsObject.sliderImageWidth + dimensionsObject.difference;
        dimensionsObject.sliderCenterWidth = 30;
        dimensionsObject.sliderRightWidth = dimensionsObject.sliderWidth - dimensionsObject.sliderLeftWidth - dimensionsObject.sliderCenterWidth - dimensionsObject.difference;
        dimensionsObject.sliderLeftHeight = dimensionsObject.sliderHeight - dimensionsObject.difference;
        dimensionsObject.sliderRightTopHeight = 100;
        dimensionsObject.sliderRightBellowWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightBellowHeight = 0;
        dimensionsObject.sliderRightDescriptionWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightDescriptionHeight = 345;
    }
    else if (windowWidth >= 800 && windowWidth < 900) {

        //alert('windowWidth: ' + windowWidth + ' [800/900]');

        dimensionsObject.dialogWidth = 700;
        dimensionsObject.dialogHeight = 485;
        dimensionsObject.navigationControlWidth = 40;
        dimensionsObject.difference = 10;
        dimensionsObject.sliderWidth = dimensionsObject.dialogWidth - 100;
        dimensionsObject.sliderHeight = dimensionsObject.dialogHeight - 80;
        dimensionsObject.sliderImageWidth = 320;
        dimensionsObject.sliderImageHeight = parseInt(dimensionsObject.sliderImageWidth * 0.675); // 215
        dimensionsObject.sliderLeftWidth = dimensionsObject.sliderImageWidth + dimensionsObject.difference;
        dimensionsObject.sliderCenterWidth = 30;
        dimensionsObject.sliderRightWidth = dimensionsObject.sliderWidth - dimensionsObject.sliderLeftWidth - dimensionsObject.sliderCenterWidth - dimensionsObject.difference;
        dimensionsObject.sliderLeftHeight = dimensionsObject.sliderHeight - dimensionsObject.difference;
        dimensionsObject.sliderRightTopHeight = 100;
        dimensionsObject.sliderRightBellowWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightBellowHeight = 0;
        dimensionsObject.sliderRightDescriptionWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightDescriptionHeight = 345;
    }
    else if (windowWidth >= 700 && windowWidth < 800) {

        //alert('windowWidth: ' + windowWidth + ' [700/800]');

        dimensionsObject.dialogWidth = 600;
        dimensionsObject.dialogHeight = 485;
        dimensionsObject.navigationControlWidth = 40;
        dimensionsObject.difference = 10;
        dimensionsObject.sliderWidth = dimensionsObject.dialogWidth - 100;
        dimensionsObject.sliderHeight = dimensionsObject.dialogHeight - 80;
        dimensionsObject.sliderImageWidth = 300;
        dimensionsObject.sliderImageHeight = parseInt(dimensionsObject.sliderImageWidth * 0.675); // 215
        dimensionsObject.sliderLeftWidth = dimensionsObject.sliderImageWidth + dimensionsObject.difference;
        dimensionsObject.sliderCenterWidth = 30;
        dimensionsObject.sliderRightWidth = dimensionsObject.sliderWidth - dimensionsObject.sliderLeftWidth - dimensionsObject.sliderCenterWidth - dimensionsObject.difference;
        dimensionsObject.sliderLeftHeight = dimensionsObject.sliderHeight - dimensionsObject.difference;
        dimensionsObject.sliderRightTopHeight = 100;
        dimensionsObject.sliderRightBellowWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightBellowHeight = 0;
        dimensionsObject.sliderRightDescriptionWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightDescriptionHeight = 345;
    }
    else {

        //alert('windowWidth: ' + windowWidth + ' other');

        dimensionsObject.dialogWidth = 600;
        dimensionsObject.dialogHeight = 485;
        dimensionsObject.navigationControlWidth = 40;
        dimensionsObject.difference = 10;
        dimensionsObject.sliderWidth = dimensionsObject.dialogWidth - 100;
        dimensionsObject.sliderHeight = dimensionsObject.dialogHeight - 80;
        dimensionsObject.sliderImageWidth = 300;
        dimensionsObject.sliderImageHeight = parseInt(dimensionsObject.sliderImageWidth * 0.675);
        dimensionsObject.sliderLeftWidth = dimensionsObject.sliderImageWidth + dimensionsObject.difference;
        dimensionsObject.sliderCenterWidth = 30;
        dimensionsObject.sliderRightWidth = dimensionsObject.sliderWidth - dimensionsObject.sliderLeftWidth - dimensionsObject.sliderCenterWidth - dimensionsObject.difference;
        dimensionsObject.sliderLeftHeight = dimensionsObject.sliderHeight - dimensionsObject.difference;
        dimensionsObject.sliderRightTopHeight = 100;
        dimensionsObject.sliderRightBellowWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightBellowHeight = 100;
        dimensionsObject.sliderRightDescriptionWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightDescriptionHeight = 345;
    }

    return dimensionsObject;
}


// GetDialogDimensionsForReportObjects: Getting dialog dimensions
function GetDialogDimensionsForReportObjects() {
    //debugger;

    var dimensionsObject = new Object();

    // setting modal dialog width
    var windowWidth = $(window).width();

    // scroll bar : 17px

    if (windowWidth >= 1400) {

        //alert('windowWidth: ' + windowWidth + ' [>1400]');

        dimensionsObject.dialogWidth = 1300;
        dimensionsObject.dialogHeight = 640; //dimensionsObject.sliderImageHeight + 175
        dimensionsObject.navigationControlWidth = 40;
        dimensionsObject.difference = 10;
        dimensionsObject.sliderWidth = dimensionsObject.dialogWidth - 100;
        dimensionsObject.sliderHeight = dimensionsObject.dialogHeight - 80;
        dimensionsObject.sliderImageWidth = 690;
        dimensionsObject.sliderImageHeight = parseInt(dimensionsObject.sliderImageWidth * 0.675); // 465
        dimensionsObject.sliderLeftWidth = dimensionsObject.sliderImageWidth + dimensionsObject.difference;
        dimensionsObject.sliderCenterWidth = 30;
        dimensionsObject.sliderRightWidth = dimensionsObject.sliderWidth - dimensionsObject.sliderLeftWidth - dimensionsObject.sliderCenterWidth - dimensionsObject.difference;
        dimensionsObject.sliderLeftHeight = dimensionsObject.sliderHeight - dimensionsObject.difference;
        dimensionsObject.sliderRightTopWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightTopHeight = 400;
        dimensionsObject.sliderRightBellowWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightBellowHeight = 50;
        dimensionsObject.sliderRightDescriptionWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightDescriptionHeight = 340;
    }
    else if (windowWidth >= 1300 && windowWidth < 1400) {

        //alert('windowWidth: ' + windowWidth + ' [1300/1400]');

        dimensionsObject.dialogWidth = 1200;
        dimensionsObject.dialogHeight = 580;
        dimensionsObject.navigationControlWidth = 40;
        dimensionsObject.difference = 10;
        dimensionsObject.sliderWidth = dimensionsObject.dialogWidth - 100;
        dimensionsObject.sliderHeight = dimensionsObject.dialogHeight - 80;
        dimensionsObject.sliderImageWidth = 600;
        dimensionsObject.sliderImageHeight = parseInt(dimensionsObject.sliderImageWidth * 0.675); // 405
        dimensionsObject.sliderLeftWidth = dimensionsObject.sliderImageWidth + dimensionsObject.difference;
        dimensionsObject.sliderCenterWidth = 30;
        dimensionsObject.sliderRightWidth = dimensionsObject.sliderWidth - dimensionsObject.sliderLeftWidth - dimensionsObject.sliderCenterWidth - dimensionsObject.difference;
        dimensionsObject.sliderLeftHeight = dimensionsObject.sliderHeight - dimensionsObject.difference;
        dimensionsObject.sliderRightTopHeight = 320;
        dimensionsObject.sliderRightBellowWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightBellowHeight = 50;
        dimensionsObject.sliderRightDescriptionWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightDescriptionHeight = 255;

    }
    else if (windowWidth >= 1200 && windowWidth < 1300) {

        //alert('windowWidth: ' + windowWidth + ' [1200/1300]');

        dimensionsObject.dialogWidth = 1100;
        dimensionsObject.dialogHeight = 540;
        dimensionsObject.navigationControlWidth = 40;
        dimensionsObject.difference = 10;
        dimensionsObject.sliderWidth = dimensionsObject.dialogWidth - 100;
        dimensionsObject.sliderHeight = dimensionsObject.dialogHeight - 80;
        dimensionsObject.sliderImageWidth = 540;
        dimensionsObject.sliderImageHeight = parseInt(dimensionsObject.sliderImageWidth * 0.675); // 365
        dimensionsObject.sliderLeftWidth = dimensionsObject.sliderImageWidth + dimensionsObject.difference;
        dimensionsObject.sliderCenterWidth = 30;
        dimensionsObject.sliderRightWidth = dimensionsObject.sliderWidth - dimensionsObject.sliderLeftWidth - dimensionsObject.sliderCenterWidth - dimensionsObject.difference;
        dimensionsObject.sliderLeftHeight = dimensionsObject.sliderHeight - dimensionsObject.difference;
        dimensionsObject.sliderRightTopHeight = 310;
        dimensionsObject.sliderRightBellowWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightBellowHeight = 50;
        dimensionsObject.sliderRightDescriptionWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightDescriptionHeight = 250;
    }
    else if (windowWidth >= 1100 && windowWidth < 1200) {

        //alert('windowWidth: ' + windowWidth + ' [1100/1200]');

        dimensionsObject.dialogWidth = 1000;
        dimensionsObject.dialogHeight = 515;
        dimensionsObject.navigationControlWidth = 40;
        dimensionsObject.difference = 10;
        dimensionsObject.sliderWidth = dimensionsObject.dialogWidth - 100;
        dimensionsObject.sliderHeight = dimensionsObject.dialogHeight - 80;
        dimensionsObject.sliderImageWidth = 500;
        dimensionsObject.sliderImageHeight = parseInt(dimensionsObject.sliderImageWidth * 0.675); // 340
        dimensionsObject.sliderLeftWidth = dimensionsObject.sliderImageWidth + dimensionsObject.difference;
        dimensionsObject.sliderCenterWidth = 30;
        dimensionsObject.sliderRightWidth = dimensionsObject.sliderWidth - dimensionsObject.sliderLeftWidth - dimensionsObject.sliderCenterWidth - dimensionsObject.difference;
        dimensionsObject.sliderLeftHeight = dimensionsObject.sliderHeight - dimensionsObject.difference;
        dimensionsObject.sliderRightTopHeight = 270;
        dimensionsObject.sliderRightBellowWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightBellowHeight = 50;
        dimensionsObject.sliderRightDescriptionWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightDescriptionHeight = 200;
    }
    else if (windowWidth >= 1000 && windowWidth < 1100) {

        //alert('windowWidth: ' + windowWidth + ' [1000/1100]');

        dimensionsObject.dialogWidth = 900;
        dimensionsObject.dialogHeight = 485;
        dimensionsObject.navigationControlWidth = 40;
        dimensionsObject.difference = 10;
        dimensionsObject.sliderWidth = dimensionsObject.dialogWidth - 100;
        dimensionsObject.sliderHeight = dimensionsObject.dialogHeight - 80;
        dimensionsObject.sliderImageWidth = 460;
        dimensionsObject.sliderImageHeight = parseInt(dimensionsObject.sliderImageWidth * 0.675);
        dimensionsObject.sliderLeftWidth = dimensionsObject.sliderImageWidth + dimensionsObject.difference; // 310
        dimensionsObject.sliderCenterWidth = 30;
        dimensionsObject.sliderRightWidth = dimensionsObject.sliderWidth - dimensionsObject.sliderLeftWidth - dimensionsObject.sliderCenterWidth - dimensionsObject.difference;
        dimensionsObject.sliderLeftHeight = dimensionsObject.sliderHeight - dimensionsObject.difference;
        dimensionsObject.sliderRightTopHeight = 270;
        dimensionsObject.sliderRightBellowWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightBellowHeight = 50;
        dimensionsObject.sliderRightDescriptionWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightDescriptionHeight = 200;
    }
    else if (windowWidth >= 900 && windowWidth < 1000) {

        //alert('windowWidth: ' + windowWidth + ' [900/1000]');

        dimensionsObject.dialogWidth = 800;
        dimensionsObject.dialogHeight = 485;
        dimensionsObject.navigationControlWidth = 40;
        dimensionsObject.difference = 10;
        dimensionsObject.sliderWidth = dimensionsObject.dialogWidth - 100;
        dimensionsObject.sliderHeight = dimensionsObject.dialogHeight - 80;
        dimensionsObject.sliderImageWidth = 380;
        dimensionsObject.sliderImageHeight = parseInt(dimensionsObject.sliderImageWidth * 0.675); // 260
        dimensionsObject.sliderLeftWidth = dimensionsObject.sliderImageWidth + dimensionsObject.difference;
        dimensionsObject.sliderCenterWidth = 30;
        dimensionsObject.sliderRightWidth = dimensionsObject.sliderWidth - dimensionsObject.sliderLeftWidth - dimensionsObject.sliderCenterWidth - dimensionsObject.difference;
        dimensionsObject.sliderLeftHeight = dimensionsObject.sliderHeight - dimensionsObject.difference;
        dimensionsObject.sliderRightTopHeight = 270;
        dimensionsObject.sliderRightBellowWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightBellowHeight = 50;
        dimensionsObject.sliderRightDescriptionWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightDescriptionHeight = 200;
    }
    else if (windowWidth >= 800 && windowWidth < 900) {

        //alert('windowWidth: ' + windowWidth + ' [800/900]');

        dimensionsObject.dialogWidth = 700;
        dimensionsObject.dialogHeight = 485;
        dimensionsObject.navigationControlWidth = 40;
        dimensionsObject.difference = 10;
        dimensionsObject.sliderWidth = dimensionsObject.dialogWidth - 100;
        dimensionsObject.sliderHeight = dimensionsObject.dialogHeight - 80;
        dimensionsObject.sliderImageWidth = 320;
        dimensionsObject.sliderImageHeight = parseInt(dimensionsObject.sliderImageWidth * 0.675); // 215
        dimensionsObject.sliderLeftWidth = dimensionsObject.sliderImageWidth + dimensionsObject.difference;
        dimensionsObject.sliderCenterWidth = 30;
        dimensionsObject.sliderRightWidth = dimensionsObject.sliderWidth - dimensionsObject.sliderLeftWidth - dimensionsObject.sliderCenterWidth - dimensionsObject.difference;
        dimensionsObject.sliderLeftHeight = dimensionsObject.sliderHeight - dimensionsObject.difference;
        dimensionsObject.sliderRightTopHeight = 270;
        dimensionsObject.sliderRightBellowWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightBellowHeight = 50;
        dimensionsObject.sliderRightDescriptionWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightDescriptionHeight = 200;
    }
    else if (windowWidth >= 700 && windowWidth < 800) {

        //alert('windowWidth: ' + windowWidth + ' [700/800]');

        dimensionsObject.dialogWidth = 600;
        dimensionsObject.dialogHeight = 485;
        dimensionsObject.navigationControlWidth = 40;
        dimensionsObject.difference = 10;
        dimensionsObject.sliderWidth = dimensionsObject.dialogWidth - 100;
        dimensionsObject.sliderHeight = dimensionsObject.dialogHeight - 80;
        dimensionsObject.sliderImageWidth = 300;
        dimensionsObject.sliderImageHeight = parseInt(dimensionsObject.sliderImageWidth * 0.675); // 215
        dimensionsObject.sliderLeftWidth = dimensionsObject.sliderImageWidth + dimensionsObject.difference;
        dimensionsObject.sliderCenterWidth = 30;
        dimensionsObject.sliderRightWidth = dimensionsObject.sliderWidth - dimensionsObject.sliderLeftWidth - dimensionsObject.sliderCenterWidth - dimensionsObject.difference;
        dimensionsObject.sliderLeftHeight = dimensionsObject.sliderHeight - dimensionsObject.difference;
        dimensionsObject.sliderRightTopHeight = 270;
        dimensionsObject.sliderRightBellowWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightBellowHeight = 50;
        dimensionsObject.sliderRightDescriptionWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightDescriptionHeight = 200;
    }
    else {

        //alert('windowWidth: ' + windowWidth + ' other');

        dimensionsObject.dialogWidth = 600;
        dimensionsObject.dialogHeight = 485;
        dimensionsObject.navigationControlWidth = 40;
        dimensionsObject.difference = 10;
        dimensionsObject.sliderWidth = dimensionsObject.dialogWidth - 100;
        dimensionsObject.sliderHeight = dimensionsObject.dialogHeight - 80;
        dimensionsObject.sliderImageWidth = 300;
        dimensionsObject.sliderImageHeight = parseInt(dimensionsObject.sliderImageWidth * 0.675);
        dimensionsObject.sliderLeftWidth = dimensionsObject.sliderImageWidth + dimensionsObject.difference;
        dimensionsObject.sliderCenterWidth = 30;
        dimensionsObject.sliderRightWidth = dimensionsObject.sliderWidth - dimensionsObject.sliderLeftWidth - dimensionsObject.sliderCenterWidth - dimensionsObject.difference;
        dimensionsObject.sliderLeftHeight = dimensionsObject.sliderHeight - dimensionsObject.difference;
        dimensionsObject.sliderRightTopHeight = 250;
        dimensionsObject.sliderRightBellowWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightBellowHeight = 20;
        dimensionsObject.sliderRightDescriptionWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightDescriptionHeight = 170;
    }

    //debugger;

    // test
    dimensionsObject.dialogWidth = 900;
    dimensionsObject.dialogHeight = 485;
    dimensionsObject.navigationControlWidth = 40;
    dimensionsObject.difference = 10;
    dimensionsObject.sliderWidth = dimensionsObject.dialogWidth - 100;
    dimensionsObject.sliderHeight = dimensionsObject.dialogHeight - 80;
    dimensionsObject.sliderImageWidth = 460;
    dimensionsObject.sliderImageHeight = parseInt(dimensionsObject.sliderImageWidth * 0.675);
    dimensionsObject.sliderLeftWidth = dimensionsObject.sliderImageWidth + dimensionsObject.difference; // 310
    dimensionsObject.sliderCenterWidth = 30;
    dimensionsObject.sliderRightWidth = dimensionsObject.sliderWidth - dimensionsObject.sliderLeftWidth - dimensionsObject.sliderCenterWidth - dimensionsObject.difference;
    dimensionsObject.sliderLeftHeight = dimensionsObject.sliderHeight - dimensionsObject.difference;
    dimensionsObject.sliderRightTopHeight = 270;
    dimensionsObject.sliderRightBellowWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
    dimensionsObject.sliderRightBellowHeight = 50;
    dimensionsObject.sliderRightDescriptionWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
    dimensionsObject.sliderRightDescriptionHeight = 200;

    return dimensionsObject;
}

// GetDialogDimensionsForMyGalleryReportObjects: Getting dialog dimensions
function GetDialogDimensionsForMyGalleryReportObjects() {
    //debugger;

    var dimensionsObject = new Object();

    // setting modal dialog width
    var windowWidth = $(window).width();

    // scroll bar : 17px

    if (windowWidth >= 1400) {

        //alert('windowWidth: ' + windowWidth + ' [>1400]');

        dimensionsObject.dialogWidth = 1300;
        dimensionsObject.dialogHeight = 640; //dimensionsObject.sliderImageHeight + 175
        dimensionsObject.navigationControlWidth = 40;
        dimensionsObject.difference = 10;
        dimensionsObject.sliderWidth = dimensionsObject.dialogWidth - 100;
        dimensionsObject.sliderHeight = dimensionsObject.dialogHeight - 80;
        dimensionsObject.sliderImageWidth = 690;
        dimensionsObject.sliderImageHeight = parseInt(dimensionsObject.sliderImageWidth * 0.675); // 465
        dimensionsObject.sliderLeftWidth = dimensionsObject.sliderImageWidth + dimensionsObject.difference;
        dimensionsObject.sliderCenterWidth = 30;
        dimensionsObject.sliderRightWidth = dimensionsObject.sliderWidth - dimensionsObject.sliderLeftWidth - dimensionsObject.sliderCenterWidth - dimensionsObject.difference;
        dimensionsObject.sliderLeftHeight = dimensionsObject.sliderHeight - dimensionsObject.difference;
        dimensionsObject.sliderRightTopWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightTopHeight = 270;
        dimensionsObject.sliderRightBellowWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightBellowHeight = 25;
        dimensionsObject.sliderRightDescriptionWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightDescriptionHeight = 220;
    }
    else if (windowWidth >= 1300 && windowWidth < 1400) {

        //alert('windowWidth: ' + windowWidth + ' [1300/1400]');

        dimensionsObject.dialogWidth = 1200;
        dimensionsObject.dialogHeight = 640;
        dimensionsObject.navigationControlWidth = 40;
        dimensionsObject.difference = 10;
        dimensionsObject.sliderWidth = dimensionsObject.dialogWidth - 100;
        dimensionsObject.sliderHeight = dimensionsObject.dialogHeight - 80;
        dimensionsObject.sliderImageWidth = 600;
        dimensionsObject.sliderImageHeight = parseInt(dimensionsObject.sliderImageWidth * 0.675); // 405
        dimensionsObject.sliderLeftWidth = dimensionsObject.sliderImageWidth + dimensionsObject.difference;
        dimensionsObject.sliderCenterWidth = 30;
        dimensionsObject.sliderRightWidth = dimensionsObject.sliderWidth - dimensionsObject.sliderLeftWidth - dimensionsObject.sliderCenterWidth - dimensionsObject.difference;
        dimensionsObject.sliderLeftHeight = dimensionsObject.sliderHeight - dimensionsObject.difference;
        dimensionsObject.sliderRightTopHeight = 270;
        dimensionsObject.sliderRightBellowWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightBellowHeight = 25;
        dimensionsObject.sliderRightDescriptionWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightDescriptionHeight = 220;

    }
    else if (windowWidth >= 1200 && windowWidth < 1300) {

        //alert('windowWidth: ' + windowWidth + ' [1200/1300]');

        dimensionsObject.dialogWidth = 1100;
        dimensionsObject.dialogHeight = 640;
        dimensionsObject.navigationControlWidth = 40;
        dimensionsObject.difference = 10;
        dimensionsObject.sliderWidth = dimensionsObject.dialogWidth - 100;
        dimensionsObject.sliderHeight = dimensionsObject.dialogHeight - 80;
        dimensionsObject.sliderImageWidth = 540;
        dimensionsObject.sliderImageHeight = parseInt(dimensionsObject.sliderImageWidth * 0.675); // 365
        dimensionsObject.sliderLeftWidth = dimensionsObject.sliderImageWidth + dimensionsObject.difference;
        dimensionsObject.sliderCenterWidth = 30;
        dimensionsObject.sliderRightWidth = dimensionsObject.sliderWidth - dimensionsObject.sliderLeftWidth - dimensionsObject.sliderCenterWidth - dimensionsObject.difference;
        dimensionsObject.sliderLeftHeight = dimensionsObject.sliderHeight - dimensionsObject.difference;
        dimensionsObject.sliderRightTopHeight = 270;
        dimensionsObject.sliderRightBellowWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightBellowHeight = 25;
        dimensionsObject.sliderRightDescriptionWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightDescriptionHeight = 220;
    }
    else if (windowWidth >= 1100 && windowWidth < 1200) {

        //alert('windowWidth: ' + windowWidth + ' [1100/1200]');

        dimensionsObject.dialogWidth = 1000;
        dimensionsObject.dialogHeight = 640;
        dimensionsObject.navigationControlWidth = 40;
        dimensionsObject.difference = 10;
        dimensionsObject.sliderWidth = dimensionsObject.dialogWidth - 100;
        dimensionsObject.sliderHeight = dimensionsObject.dialogHeight - 80;
        dimensionsObject.sliderImageWidth = 500;
        dimensionsObject.sliderImageHeight = parseInt(dimensionsObject.sliderImageWidth * 0.675); // 340
        dimensionsObject.sliderLeftWidth = dimensionsObject.sliderImageWidth + dimensionsObject.difference;
        dimensionsObject.sliderCenterWidth = 30;
        dimensionsObject.sliderRightWidth = dimensionsObject.sliderWidth - dimensionsObject.sliderLeftWidth - dimensionsObject.sliderCenterWidth - dimensionsObject.difference;
        dimensionsObject.sliderLeftHeight = dimensionsObject.sliderHeight - dimensionsObject.difference;
        dimensionsObject.sliderRightTopHeight = 270;
        dimensionsObject.sliderRightBellowWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightBellowHeight = 25;
        dimensionsObject.sliderRightDescriptionWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightDescriptionHeight = 200;
    }
    else if (windowWidth >= 1000 && windowWidth < 1100) {

        //alert('windowWidth: ' + windowWidth + ' [1000/1100]');

        dimensionsObject.dialogWidth = 900;
        dimensionsObject.dialogHeight = 640; //dimensionsObject.sliderImageHeight + 175
        dimensionsObject.navigationControlWidth = 40;
        dimensionsObject.difference = 10;
        dimensionsObject.sliderWidth = dimensionsObject.dialogWidth - 100;
        dimensionsObject.sliderHeight = dimensionsObject.dialogHeight - 80;
        dimensionsObject.sliderImageWidth = 460;
        dimensionsObject.sliderImageHeight = parseInt(dimensionsObject.sliderImageWidth * 0.675); // 465
        dimensionsObject.sliderLeftWidth = dimensionsObject.sliderImageWidth + dimensionsObject.difference;
        dimensionsObject.sliderCenterWidth = 30;
        dimensionsObject.sliderRightWidth = dimensionsObject.sliderWidth - dimensionsObject.sliderLeftWidth - dimensionsObject.sliderCenterWidth - dimensionsObject.difference;
        dimensionsObject.sliderLeftHeight = dimensionsObject.sliderHeight - dimensionsObject.difference;
        dimensionsObject.sliderRightTopWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightTopHeight = 270;
        dimensionsObject.sliderRightBellowWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightBellowHeight = 25;
        dimensionsObject.sliderRightDescriptionWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightDescriptionHeight = 200;
    }
    else if (windowWidth >= 900 && windowWidth < 1000) {

        //alert('windowWidth: ' + windowWidth + ' [900/1000]');

        dimensionsObject.dialogWidth = 800;
        dimensionsObject.dialogHeight = 640;
        dimensionsObject.navigationControlWidth = 40;
        dimensionsObject.difference = 10;
        dimensionsObject.sliderWidth = dimensionsObject.dialogWidth - 100;
        dimensionsObject.sliderHeight = dimensionsObject.dialogHeight - 80;
        dimensionsObject.sliderImageWidth = 380;
        dimensionsObject.sliderImageHeight = parseInt(dimensionsObject.sliderImageWidth * 0.675); // 260
        dimensionsObject.sliderLeftWidth = dimensionsObject.sliderImageWidth + dimensionsObject.difference;
        dimensionsObject.sliderCenterWidth = 30;
        dimensionsObject.sliderRightWidth = dimensionsObject.sliderWidth - dimensionsObject.sliderLeftWidth - dimensionsObject.sliderCenterWidth - dimensionsObject.difference;
        dimensionsObject.sliderLeftHeight = dimensionsObject.sliderHeight - dimensionsObject.difference;
        dimensionsObject.sliderRightTopHeight = 270;
        dimensionsObject.sliderRightBellowWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightBellowHeight = 25;
        dimensionsObject.sliderRightDescriptionWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightDescriptionHeight = 200;
    }
    else if (windowWidth >= 800 && windowWidth < 900) {

        //alert('windowWidth: ' + windowWidth + ' [800/900]');

        dimensionsObject.dialogWidth = 700;
        dimensionsObject.dialogHeight = 640;
        dimensionsObject.navigationControlWidth = 40;
        dimensionsObject.difference = 10;
        dimensionsObject.sliderWidth = dimensionsObject.dialogWidth - 100;
        dimensionsObject.sliderHeight = dimensionsObject.dialogHeight - 80;
        dimensionsObject.sliderImageWidth = 320;
        dimensionsObject.sliderImageHeight = parseInt(dimensionsObject.sliderImageWidth * 0.675); // 215
        dimensionsObject.sliderLeftWidth = dimensionsObject.sliderImageWidth + dimensionsObject.difference;
        dimensionsObject.sliderCenterWidth = 30;
        dimensionsObject.sliderRightWidth = dimensionsObject.sliderWidth - dimensionsObject.sliderLeftWidth - dimensionsObject.sliderCenterWidth - dimensionsObject.difference;
        dimensionsObject.sliderLeftHeight = dimensionsObject.sliderHeight - dimensionsObject.difference;
        dimensionsObject.sliderRightTopHeight = 240;
        dimensionsObject.sliderRightBellowWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightBellowHeight = 25;
        dimensionsObject.sliderRightDescriptionWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightDescriptionHeight = 150;
    }
    else if (windowWidth >= 700 && windowWidth < 800) {

        //alert('windowWidth: ' + windowWidth + ' [700/800]');

        dimensionsObject.dialogWidth = 600;
        dimensionsObject.dialogHeight = 640;
        dimensionsObject.navigationControlWidth = 40;
        dimensionsObject.difference = 10;
        dimensionsObject.sliderWidth = dimensionsObject.dialogWidth - 100;
        dimensionsObject.sliderHeight = dimensionsObject.dialogHeight - 80;
        dimensionsObject.sliderImageWidth = 300;
        dimensionsObject.sliderImageHeight = parseInt(dimensionsObject.sliderImageWidth * 0.675); // 215
        dimensionsObject.sliderLeftWidth = dimensionsObject.sliderImageWidth + dimensionsObject.difference;
        dimensionsObject.sliderCenterWidth = 30;
        dimensionsObject.sliderRightWidth = dimensionsObject.sliderWidth - dimensionsObject.sliderLeftWidth - dimensionsObject.sliderCenterWidth - dimensionsObject.difference;
        dimensionsObject.sliderLeftHeight = dimensionsObject.sliderHeight - dimensionsObject.difference;
        dimensionsObject.sliderRightTopHeight = 240;
        dimensionsObject.sliderRightBellowWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightBellowHeight = 25;
        dimensionsObject.sliderRightDescriptionWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightDescriptionHeight = 150;
    }
    else {

        //alert('windowWidth: ' + windowWidth + ' other');

        dimensionsObject.dialogWidth = 600;
        dimensionsObject.dialogHeight = 640;
        dimensionsObject.navigationControlWidth = 40;
        dimensionsObject.difference = 10;
        dimensionsObject.sliderWidth = dimensionsObject.dialogWidth - 100;
        dimensionsObject.sliderHeight = dimensionsObject.dialogHeight - 80;
        dimensionsObject.sliderImageWidth = 300;
        dimensionsObject.sliderImageHeight = parseInt(dimensionsObject.sliderImageWidth * 0.675);
        dimensionsObject.sliderLeftWidth = dimensionsObject.sliderImageWidth + dimensionsObject.difference;
        dimensionsObject.sliderCenterWidth = 30;
        dimensionsObject.sliderRightWidth = dimensionsObject.sliderWidth - dimensionsObject.sliderLeftWidth - dimensionsObject.sliderCenterWidth - dimensionsObject.difference;
        dimensionsObject.sliderLeftHeight = dimensionsObject.sliderHeight - dimensionsObject.difference;
        dimensionsObject.sliderRightTopHeight = 240;
        dimensionsObject.sliderRightBellowWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightBellowHeight = 25;
        dimensionsObject.sliderRightDescriptionWidth = dimensionsObject.sliderRightWidth - dimensionsObject.difference;
        dimensionsObject.sliderRightDescriptionHeight = 130;
    }

    return dimensionsObject;
}

// setting image size
function SetImageSize() {
    //debugger;

    // setting modal dialog image width
    var dimensionsObject = GetDialogDimensionsForReportObjects();

    sliderImageWidth = dimensionsObject.sliderImageWidth;
    sliderImageHeight = dimensionsObject.sliderImageHeight;

    // get image size
    var imageWidth = $(".urlImageLarge").width();
    var imageHeight = $(".urlImageLarge").height();

    var padding = 10;

    if (imageWidth > sliderImageWidth || imageHeight > sliderImageHeight) {
        var t = 1;

        var p = imageWidth / sliderImageWidth;
        var q = imageHeight / sliderImageHeight;

        if (p > q) { t = p; } else { t = q }

        var newImageWidth = parseInt(imageWidth / t);
        var newImageHeight = parseInt(imageHeight / t);

        var xAddition = parseInt(sliderImageWidth - newImageWidth);
        var yAddition = parseInt(sliderImageHeight - newImageHeight);

        $(".urlImageLarge").attr({ 'border': 1 }).height(newImageHeight).width(newImageWidth);

        $(".urlImageLarge").css({
            "padding-left": (xAddition + padding) / 2,
            "padding-right": (xAddition + padding) / 2,
            "padding-top": (yAddition + padding) / 2,
            "padding-bottom": (yAddition + padding) / 2
        });
    }
    else {
        var widthDifference = parseInt(sliderImageWidth - imageWidth);
        var heightDifference = parseInt(sliderImageHeight - imageHeight);

        $(".urlImageLarge").attr({ 'border': 1 }).height(imageHeight).width(imageWidth);

        $(".urlImageLarge").css({
            "padding-left": (widthDifference + padding) / 2,
            "padding-right": (widthDifference + padding) / 2,
            "padding-top": (heightDifference + padding) / 2,
            "padding-bottom": (heightDifference + padding) / 2
        });
    }

    $(".urlImageLarge").css({ 'opacity': 1 });
    $(".LoadingDiv").css({ 'border': 'none', 'background': 'none' });
}
