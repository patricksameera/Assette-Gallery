///////////////////////////////////////////////////////
//********** Assette Gallery       **********//
//********** Assette © 2013                **********//
///////////////////////////////////////////////////////

// document ready //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// jquery document ready function
$(document).ready(function () {

    //debugger;

    // set tag content
    GetTagContent();

    // loading image functionality
    $("#divLoading").ajaxStart(OnAjaxStart);
    $("#divLoading").ajaxError(OnAjaxError);
    $("#divLoading").ajaxSuccess(OnAjaxSuccess);
    $("#divLoading").ajaxStop(OnAjaxStop);
    $("#divLoading").ajaxComplete(OnAjaxComplete);

    // create user button click
    $('#btnCreate').click(function () {

        // edit tag content
        EditTagContent();

        // set tag content
        GetTagContent();
    });

    // key press - enter
    $(document).keypress(function (e) {
        if (e.which == 13) {

            //EditTagContent();
        }
    });

    // set selected tab link  
    $('#clients').css('background-color', '#999999');
    $('#editsamplereports').css('background-color', '#999999');
    $('#edittemplatedesigns').css('background-color', '#999999');
    $('#editreportobjects').css('background-color', '#999999');
    $('#editobjectcategoryorder').css('background-color', '#999999');
    $('#editheadtagcontent').css('background-color', '#C1D82F');

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

function EditTagContent() {
    //debugger;

    var retValue = Validate();

    if (retValue == true) {

        AddTagContent();
    }
    else {
        //debugger;

        var message = "Please correct the following errors, then resubmit: </br>";

        for (i = 0; i < retValue.length; i++) {
            message += "* " + retValue[i];
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
    var tagContent = $('#txtContent').val();

    var retValue = true;
    var errorRows = [];

    if (tagContent == '') {
        errorRows.push("Tag content is blank");
        $('#txtContent').css({ 'border': '1px solid #FFCCCC' })
        retValue = false;
    }
    else {
        $('#txtContent').css({ 'border': '1px solid #000000' })
    }

    if (retValue != true) {
        retValue = errorRows;
    }

    return retValue;
}

// create new user /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function AddTagContent() {
    //debugger;

    // get values
    var tagContent = $('#txtContent').val();

    $.ajax({
        type: "POST",
        url: "edit-head-tag.aspx/AddTagContent",
        data: JSON.stringify({ tagContent: tagContent }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OnAddTagContentSuccess,
        error: OnAddTagContentError
    });
}

function OnAddTagContentError(request, status, error) {
    //debugger;

    // browser loggin
    fireBugConsoleLog(request, status, error);

    // show error to user
    showErrorToUser("");

    // log4net logging
    var errorText = "OnAddTagContentError() jQuery Exception: response text= " + request.responseText + ", status= " + status + ", error= " + error;
    ClientSideLogging(errorText);
}

function OnAddTagContentSuccess(response) {
    //debugger;

    if (response.d == "1") {

        var message = "Tag content successfully updated.";

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

// get firm types details /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function GetTagContent() {
    //debugger;

    $.ajax({
        type: "POST",
        url: "edit-head-tag.aspx/GetTagContent",
        data: "",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: true,
        success: OnGetTagContentSuccess,
        error: OnGetTagContentsError
    });
}

function OnGetTagContentsError(request, status, error) {
    //debugger;

    // browser loggin
    fireBugConsoleLog(request, status, error);

    // show error to user
    showErrorToUser("");

    // log4net logging
    var errorText = "OnGetTagContentError() jQuery Exception: response text= " + request.responseText + ", status= " + status + ", error= " + error;
    ClientSideLogging(errorText);
}

function OnGetTagContentSuccess(response) {
    //debugger;

    // assigning
    var data = $.parseJSON(response.d);

    // get all report id's
    $.each(data, function () {
        //debugger;

        $("#txtContent").val(this.ParamValue);

    });
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



