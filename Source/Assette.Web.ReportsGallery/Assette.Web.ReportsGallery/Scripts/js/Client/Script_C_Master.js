///////////////////////////////////////////////////////
//********** Assette Gallery       **********//
//********** Assette © 2013                **********//
///////////////////////////////////////////////////////

//debugger;

// document ready //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// jquery document ready function
$(document).ready(function () {
    //debugger;

    // window resize event
    $(window).resize(function () {

        // window resize event
        WindowReSizeMaster();
    });

    // share button click - open
    $('#btnShare').live("click", function (e) {
        //debugger;

        $("#divShare").slideDown('slow');

        // window resize event
        WindowReSizeMaster();
    });

    // email button click - open
    $('.shareThisEmail').live("click", function (e) {
        //debugger;

        // clear filed
        $("#EmailTo").val('');
        $("#EmailFrom").val('');
        $("#EmailMessage").val('');
        $("#emailValidation").hide();
        $("#divEmail").hide();

        // water mark
        $("#EmailTo").Watermark("TO: (Type addresses, each separated by a comma)");
        $("#EmailFrom").Watermark("FROM:");
        $("#EmailMessage").Watermark("Type message here ...");

        //var link = "<a href='" + $("[id$=hdnUrl]").val() + "'>" + $("[id$=hdnUrl]").val() + "</a>";
        var link = "<a href='" + document.URL + "'>" + document.URL + "</a>";

        var htmlMessage =
        'Hello,<br/><br/>' +
        'I was looking through the Gallery on the Assette website, and thought you would be interested in taking a look. Assette offers an easy and automated way to create effective client communications that go beyond the basics. Please click on the link below to view the gallery. (You will be asked to fill out a simple registration form, if you haven’t already registered).<br/><br/>' +
        //'Please click on the link below to view the gallery.  (You will be asked to fill out a simple registration form, if you haven’t already registered).<br/><br/>' +
        link +
        '<br/><br/>' +
        'Thanks,<br/><br/>' +
        $("[id$=hdnFirstName]").val();

        var plainMessage = htmlMessage.replace(/\<br\/\>/gi, '\n').replace(/(<([^>]+)>)/ig, "");

        $("#EmailFrom").val($("[id$=hdnEmail]").val());
        $("#EmailMessage").val(plainMessage);

        $("#divEmail").slideDown('slow');

        // window resize event
        WindowReSizeMaster();
    });

    // close button click - x
    $('.m19').live("click", function (e) {
        //debugger;

        $("#divShare").slideUp('slow');
        $("#divEmail").slideUp('slow');
    });

    // close button click - x
    $('.m23').live("click", function (e) {
        //debugger;

        $("#divEmail").slideUp('slow');
    });

    // send button click
    $('#btnSend').live("click", function (e) {
        //debugger;

        SendEmail();
    });

    // cancel button click
    $('#btnCancel').live("click", function (e) {
        //debugger;

        // clear filed
        $("#EmailTo").val('');
        $("#EmailFrom").val('');
        $("#EmailMessage").val('');

        $("#emailValidation").hide();

        // water mark
        $("#EmailTo").Watermark("TO: (Type addresses, each separated by a comma)");
        $("#EmailFrom").Watermark("FROM:");
        $("#EmailMessage").Watermark("Type message here ...");

        $("#divEmail").slideUp('slow');
    });

    // water mark
    $("#EmailTo").Watermark("TO: (Type addresses, each separated by a comma)");
    $("#EmailFrom").Watermark("FROM:");
    $("#EmailMessage").Watermark("Type message here ...");
})

// send email ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function SendEmail() {
    //debugger;

    // get values
    var emailTo = $.trim($('#EmailTo').val());
    var emailFrom = $.trim($('#EmailFrom').val());
    var emailMessage = $.trim($('#EmailMessage').val());

    // set initial values - watermark
    if (emailTo == "TO: (Type addresses, each separated by a comma)") emailTo = "";
    if (emailFrom == "FROM:") emailFrom = "";
    if (emailMessage == "Type message here ...") emailMessage = "";

    if (emailTo != "" && emailFrom != "" && emailMessage != "") {

        if (ValidateEmailAddress()) {

            $("#emailValidation").hide();

            $("#emailValidation").text('Sending email ...');
            $("#emailValidation").fadeIn('slow');

            $.ajax({
                type: "POST",
                url: "login.aspx/SendMail",
                data: JSON.stringify({ emailTo: emailTo, emailFrom: emailFrom, emailMessage: emailMessage }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSendEmailSuccess,
                error: OnSendEmailError
            });
        }
        else {
            $("#emailValidation").hide();

            $("#emailValidation").text('Incorrect email format.');
            $("#emailValidation").fadeIn('slow');
        }
    }
    else {

        var message;

        if (emailTo == "" && emailMessage == "") {
            message = 'Please fill all the fields.'
        }
        else if (emailTo == "") {
            message = 'Please enter email address.'
        }
        else if (emailMessage == "") {
            message = 'Please enter email message.'
        }

        $("#emailValidation").hide();
        $("#emailValidation").text(message);
        $("#emailValidation").fadeIn('slow');
    }
}

function OnSendEmailError(request, status, error) {
    //debugger

    // browser loggin
    fireBugConsoleLog(request, status, error);

    // show error to user
    showErrorToUser("");

    // log4net logging
    var errorText = "OnSendEmailError() jQuery Exception: response text= " + request.responseText + ", status= " + status + ", error= " + error;
    ClientSideLogging(errorText);
}

function OnSendEmailSuccess(response) {
    //debugger;

    if (response.d == "1") {

        $("#emailValidation").hide();

        $("#emailValidation").text('Email sent.');
        $("#emailValidation").fadeIn('slow');
    }
    else {

        $("#emailValidation").hide();

        $("#emailValidation").text('Error occured.');
        $("#emailValidation").fadeIn('slow');
    }
}

// email validation /////////////////////////////////////////////////////////////////////////////////////////////////////////////

function ValidateEmailAddress() {
    //debugger;

    var retValue = true;

    // get values
    var emailTo = $('#EmailTo').val().split(',');
    var emailFrom = $('#EmailFrom').val();

    emailTo.push(emailFrom);

    for (j = 0; j < emailTo.length; j++) {

        if (!isValidEmailAddress($.trim(emailTo[j]))) {
            retValue = false;
            break;
        }
    }

    return retValue;
}

// window resize event /////////////////////////////////////////////////////////////////////////////////////////////////////////////

function WindowReSizeMaster() {
    //debugger;

    var position = $("#btnShare").offset();

    //$("#divShare").css(position)

    // share
    $("#divShare").css("left", position.left - 152);
    $("#divShare").css("top", position.top + 40);

    // email
    $("#divEmail").css("left", position.left - 360);
    $("#divEmail").css("top", position.top + 95);
}

