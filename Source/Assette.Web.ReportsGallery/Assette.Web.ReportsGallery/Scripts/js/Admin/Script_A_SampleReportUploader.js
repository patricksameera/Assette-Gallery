///////////////////////////////////////////////////////
//********** Assette Gallery       **********//
//********** Assette © 2013                **********//
///////////////////////////////////////////////////////

//debugger;

//samples
//https://github.com/valums/file-uploader
//http://svn.openstreetmap.org/sites/free-map.org.uk/otv/js/fileuploader.js?p=24624
//http://stackoverflow.com/questions/5349326/need-advice-on-ajax-fileupload
//https://github.com/valums/file-uploader#qqfineuploader---setting-up-full-upload-widget

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

    // go back button click
    $('#goBack').live("click", function (e) {
        //debugger;

        location.href = "edit-sample-reports.aspx";
    });

    // set selected tab link   
    $('#editsamplereports').css('background-color', '#C1D82F');
    $('#edittemplatedesigns').css('background-color', '#999999');
    $('#clients').css('background-color', '#999999');
    $('#editreportobjects').css('background-color', '#999999');
    $('#editreportobjectmockups').css('background-color', '#999999');
    $('#editobjectcategoryorder').css('background-color', '#999999');
    $('#editheadtagcontent').css('background-color', '#999999');

    // collapsible
    $("#fieldset1").collapse();
    $("#fieldset2").collapse();
    $("#fieldset3").collapse();

    // uploader
    var uploader = new qq.FileUploader({
        element: document.getElementById('file-uploader-demo'),
        dragText: 'Drop files here to upload',
        action: "SampleReportUploadHandler.aspx",
        uploadButtonText: '&nbsp;<i class="icon-plus icon-white"></i>&nbsp;Select A File',
        //uploadButtonText: "Upload File ...",
        debug: false,
        multiple: true,
        autoUpload: false,
        allowedExtensions: ['ppt', 'pptx'],
        sizeLimit: 2147483647, // maximum
        //params: { name: $('#txtName').val(), shortdescription: $('#txtLongDescription').val(), longdescription: $('#txtShortDescription').val(), product: $("#Product").val(), firm: $("#Firm").val() },
        onSubmit: uploaderOnSubmit,
        onProgress: uploaderOnProgress,
        onComplete: uploaderOnComplete,
        onCancel: uploaderOnCancel,
        onError: uploaderOnError

    });

    // upload button click
    $('#triggerUpload').live("click", function (e) {
        //debugger;

        var rtnVal = false;

        rtnVal = UploadValidation();

        if (rtnVal == true) {
            uploader.setParams({
                name: $.trim($('#txtName').val()),
                shortdescription: $.trim($('#txtShortDescription').val()),
                longdescription: $.trim($('#txtLongDescription').val()),
                product: $("#Product").val(),
                firm: $("#Firm").val(),
                preview: $('#chkPreview').attr('checked') ? 1 : 0
            });

            uploader.uploadStoredFiles();

        }
    });

    function uploaderOnSubmit(id, fileName) { }
    function uploaderOnProgress(id, fileName, loaded, total) { }
    function uploaderOnCancel(id, fileName) { }
    function uploaderOnComplete(id, fileName, responseJSON) {
        //debugger;

        ClearFields();
    }
    function uploaderOnError(id, fileName, xhr) {
        //debugger;

        // show error to user
        showErrorToUser("");

        WindowReSize();

        // browser logging
        if (typeof console != "undefined") {

            console.log("id: " + id);
            console.log("fileName: " + fileName);
            console.log("xhr:" + xhr);
        }
    }

    // jquery select box - set the initial place holder
    $("#Product").selectbox('attach');
    $("#Firm").selectbox('attach');

    // fill dropdowns - bind values
    GetFirmTypes();
    GetProductTypes();

})

// get firm types details /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function GetFirmTypes() {
    //debugger;

    $.ajax({
        type: "POST",
        url: "../Admin/sample-report-uploader.aspx/GetFirmTypes",
        data: "",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OnGetFirmTypesSuccess,
        error: OnGetFirmTypesError
    });
}

function OnGetFirmTypesError(request, status, error) {
    //debugger;

    // browser loggin
    fireBugConsoleLog(request, status, error);

    // show error to user
    showErrorToUser("");

    WindowReSize();

    // log4net logging
    var errorText = "OnGetFirmTypesError() jQuery Exception: response text= " + request.responseText + ", status= " + status + ", error= " + error;
    ClientSideLogging(errorText);
}

function OnGetFirmTypesSuccess(response) {
    //debugger;

    var dropDown = $("#Firm");

    bindDataToDropDown(response, dropDown, "FIRM");
}

// get product types details /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function GetProductTypes() {
    //debugger;

    $.ajax({
        type: "POST",
        url: "../Admin/sample-report-uploader.aspx/GetProductTypes",
        data: "",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OnGetProductTypesSuccess,
        error: OnGetProductTypesError
    });
}

function OnGetProductTypesError(request, status, error) {
    //debugger;

    // browser loggin
    fireBugConsoleLog(request, status, error);

    // show error to user
    showErrorToUser("");

    WindowReSize();

    // log4net logging
    var errorText = "OnGetProductTypesError() jQuery Exception: response text= " + request.responseText + ", status= " + status + ", error= " + error;
    ClientSideLogging(errorText);
}

function OnGetProductTypesSuccess(response) {
    //debugger;

    var dropDown = $("#Product");

    bindDataToDropDown(response, dropDown, "PRODUCT");
}

// bind data to dropdown /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function bindDataToDropDown(response, dropdown, defaultText) {
    //debugger;

    data = $.parseJSON(response.d);

    // clear dropdown
    dropdown.empty();

    // detach dropdown
    dropdown.selectbox('detach');

    dropdown.append('<option value=0>' + defaultText + '</option>');

    $.each(data, function () {

        dropdown.append('<option value=' + this.ID + '>' + this.Name + '</option>');

    });

    // jquery select box - set
    dropdown.selectbox();
}

// upload validation /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function UploadValidation() {
    //debugger;

    var retValue = Validate();

    if (retValue == true) {
        // hide message bar
        $('#message_bar').hide();

        return true;
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

        WindowReSize();

        return false;
    }
}

function Validate() {
    //debugger;

    // get values
    var name = $.trim($('#txtName').val());
    var shortDescription = $.trim($('#txtShortDescription').val());
    var longDescription = $.trim($('#txtLongDescription').val());
    var firm = $("#Firm").val();
    var product = $("#Product").val();

    var retValue = true;
    var errorRows = [];

    //$('.a27').css({ 'border-left': '1px solid #E5E5E5', 'border-right': '1px solid #E5E5E5', 'border-top': '1px solid #F2F2F2', 'border-bottom': '1px solid #F2F2F2' })

    if (name == '') {
        errorRows.push("Name field is blank");
        $('#txtName').css({ 'border': '1px solid #FFCCCC' })
        retValue = false;
    }
    else {
        $('#txtName').css({ 'border': '1px solid #000000' })
    }

    if (shortDescription == '') {
        errorRows.push("Short Description field is blank");
        $('#txtShortDescription').css({ 'border': '1px solid #FFCCCC' })
        retValue = false;
    }
    else {
        $('#txtShortDescription').css({ 'border': '1px solid #000000' })
    }

    if (longDescription == '') {
        errorRows.push("Long Description field is blank");
        $('#txtLongDescription').css({ 'border': '1px solid #FFCCCC' })
        retValue = false;
    }
    else {
        $('#txtLongDescription').css({ 'border': '1px solid #000000' })
    }

    if (product == '0') {
        errorRows.push("Select product type");
        //$('#LastName').css({ 'border': '1px solid #FFCCCC' })
        retValue = false;
    }
    else {
        //$('#LastName').css({ 'border-left': '1px solid #E5E5E5', 'border-right': '1px solid #E5E5E5', 'border-top': '0px solid #F2F2F2', 'border-bottom': '0px solid #F2F2F2' })
    }

    if (firm == '0') {
        errorRows.push("Select firm type");
        //$('#Firm').css({ 'border': '1px solid #FFCCCC' })
        retValue = false;
    }
    else {
        //$('#Firm').css({ 'border': '1px solid #000000' })
    }

    if (retValue != true) {
        retValue = errorRows;
    }

    return retValue;
}

// clear fields /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function ClearFields() {
    //debugger;

    $('#txtName').val('');
    $('#txtShortDescription').val('');
    $('#txtLongDescription').val('');

    // set text in jquery-select

    // firm dropdown
    var dropDownSbId = $("#Firm").attr("sb");
    var $SelectedHref = $("#sbSelector_" + dropDownSbId);

    var dropdown = document.getElementById("Firm");
    var value = dropdown.options[0].text;

    $SelectedHref.text(value);

    // product dropdown
    dropDownSbId = $("#Product").attr("sb");
    $SelectedHref = $("#sbSelector_" + dropDownSbId);

    dropdown = document.getElementById("Product");
    value = dropdown.options[0].text;

    $SelectedHref.text(value);

    // set value in html-select
    $("#Firm").val(0);
    $("#Product").val(0);
}

// window resize event /////////////////////////////////////////////////////////////////////////////////////////////////////////////

function WindowReSize() {
    //debugger;

    // positioning log-in image
    var position = $("#triggerUpload").offset();
    var windowWidth = $(window).width();

    $("#message_bar").css("left", windowWidth / 2 - 112);
    $("#message_bar").css("top", position.top + 85);
}
