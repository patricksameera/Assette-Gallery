///////////////////////////////////////////////////////
//********** Assette Gallery       **********//
//********** Assette © 2013                **********//
//********** Developed by Sameera Jayalath **********//
///////////////////////////////////////////////////////

//debugger;

var idxSamples = 0;
var sliderContentSamples;
var imageSliderContentSamples;
var selectedObjectIdSamples;
var selectedObjectIndexIdSamples;
var reportIdsSamples = [];
var imageHandlerIndexSamples = 0;
var allSamplesJsonData;
var uniqueindex = 0;

// document ready //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// jquery document ready function
$(document).ready(function (e) {
    //debugger;

    /*
    // loading image functionality
    $("#divLoading").ajaxStart(OnAjaxStartForSampleReportsPreView);
    $("#divLoading").ajaxError(OnAjaxErrorForSampleReportsPreView);
    $("#divLoading").ajaxSuccess(OnAjaxSuccessForSampleReportsPreView);
    $("#divLoading").ajaxStop(OnAjaxStopForSampleReportsPreView);
    $("#divLoading").ajaxComplete(OnAjaxCompleteForSampleReportsPreView);
    */

    // get sample reports
    GetAllSampleReportsForPreView();

    // register button click
    $('#lnkRegisterSlide').live("click", function (e) {
        //debugger;

        location.href = "login.aspx";
    });

    // view library button click
    $('.ViewLibrarySamples').live("click", function (e) {
        //debugger;

        // stop postback
        e.preventDefault();

        // get clicked object's id
        selectedObjectIdSamples = $(this).attr("Id");

        // get clicked object's index id
        selectedObjectIndexIdSamples = $(this).attr("Index");

        // get sample reports
        GetSampleReportPages(selectedObjectIdSamples, selectedObjectIndexIdSamples);

        // get report object
        GetSampleReportDetails(selectedObjectIdSamples, selectedObjectIndexIdSamples);

        // setting div border
        $('.slideViewer').parent().css({ 'border': '1px solid #000000', 'padding-right': '5px', 'padding-bottom': '5px' });
    });

})

// get sample reports /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function GetAllSampleReportsForPreView() {
    //debugger;

    $.ajax({
        type: "POST",
        url: "default.aspx/GetAllSampleReportsForPreView",
        data: "",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OnGetAllSampleReportsForPreViewSuccess,
        error: OnGetAllSampleReportsForPreViewError
    });

}

function OnGetAllSampleReportsForPreViewError(request, status, error) {
    //debugger;

    // browser loggin
    fireBugConsoleLog(request, status, error);

    // show error to user
    showErrorToUser("");

    // log4net logging
    var errorText = "OnGetAllSampleReportsForPreViewError() jQuery Exception: response text= " + request.responseText + ", status= " + status + ", error= " + error;
    ClientSideLogging(errorText);
}

function OnGetAllSampleReportsForPreViewSuccess(response) {
    //debugger;

    // re-set
    reportIdsSamples = [];

    // assigning
    allSamplesJsonData = $.parseJSON(response.d);

    // get all report id's
    $.each(allSamplesJsonData, function () {
        //debugger;

        // add all report id's to array
        reportIdsSamples.push(this.ID);
    });

    // get report count
    var count = reportIdsSamples.length;

    // show data
    ShowAllSamplesPreViewDataOnATable(allSamplesJsonData);
}

// show data on a table /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function ShowAllSamplesPreViewDataOnATable(data) {
    //debugger;

    var galleryRows = [];
    sliderContentSamples = "";

    $.each(data, function () {
        //debugger;

        // increment the index
        idxSamples++;

    });

    // register link slider - add 0 report array
    reportIdsSamples.push(0);

    // register link slider - increment by another 1
    idxSamples++;

    $("#SamplesPreView").append(galleryRows.join(""));
}

// loading screen functionality /////////////////////////////////////////////////////////////////////////////////////////////////////////////

function OnAjaxStartForSampleReportsPreView() {
    //debugger;

    var width = $(window).width();
    var height = $(window).height();

    $("#divLoading").css("left", width / 2 - 68);
    $("#divLoading").css("top", height - 250);

    $("#divLoading").fadeIn('fast');
}

function OnAjaxErrorForSampleReportsPreView() {
    //debugger;

}

function OnAjaxSuccessForSampleReportsPreView() {
    //debugger;

    $("#divLoading").delay(500).fadeOut('slow');
}

function OnAjaxStopForSampleReportsPreView() {
    //debugger;

    $("#divLoading").delay(500).fadeOut('slow');
}

function OnAjaxCompleteForSampleReportsPreView() {
    //debugger;

    $("#divLoading").delay(500).fadeOut('slow');
}

// get sample report pages /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function GetSampleReportPages(objectId, indexId) {
    //debugger;

    // of object id 0  means, register link slide
    if (objectId != 0) {

        selectedObjectIndexIdSamples = indexId;
        selectedObjectIdSamples = objectId;

        // get slider content
        var data = jQuery.grep(allSamplesJsonData, function (element, index) {
            return element.ID == objectId;
        });

        var imageSliderRows = [];
        var UploadedDate;

        $.each(data, function () {
            //debugger;

            recordId = this.ID;
            noOfPages = this.NoOfPages;

            // get date
            var jsonDate = new Date(parseInt(this.UploadedDate.substr(6)));
            var d = jsonDate.getDate();
            var m = jsonDate.getMonth() + 1;
            var y = jsonDate.getFullYear();
            var hh = jsonDate.getHours();
            var mm = jsonDate.getMinutes();
            var ss = jsonDate.getSeconds();
            UploadedDate = y + "" + (m <= 9 ? '0' + m : m) + "" + (d <= 9 ? '0' + d : d) + "" + (hh <= 9 ? '0' + hh : hh) + "" + (mm <= 9 ? '0' + mm : mm) + "" + (ss <= 9 ? '0' + ss : ss);

        });

        // setting modal dialog image width
        var dimensionsObject = GetDialogDimensionsForSamplesAndDesigns();

        // image slider
        imageSliderRows.push("<div>");
        imageSliderRows.push("<div id='my-folio-of-works' class='svwp'>");
        imageSliderRows.push("<ul>");

        // image slider
        for (var i = 0; i < noOfPages; i++) {
            //debugger;

            // increment
            //uniqueindex++;

            var newRecordId = parseInt(recordId);
            var newPageId = parseInt(i) + 1;

            imageSliderRows.push("<li>");

            // default
            //imageSliderRows.push("<img class='urlImageLarge' id=" + newRecordId + " index='" + newPageId + "' src='ImageHandler.ashx?id=" + newRecordId + "&type=s_i&date=" + UploadedDate + "' width='800' height='480' border='0' />");

            imageSliderRows.push("<img alt='" + newPageId + "' class='urlImageLarge' id=" + newRecordId + " index='" + newPageId + "' src='ImageHandler.ashx?id=" + newRecordId + "&type=s_i&date=" + UploadedDate + "&uniqueindex=" + uniqueIndex + "&page=" + newPageId + "' width='" + dimensionsObject.sliderImageWidth + "' height='" + dimensionsObject.sliderImageHeight + "' border='0'  />");

            imageSliderRows.push("</li>");
        }

        imageSliderRows.push("</ul>");
        imageSliderRows.push("</div>");
        imageSliderRows.push("</div>");

        imageSliderContentSamples = imageSliderRows.join("");
    }
    else {

        selectedObjectIndexIdSamples = indexId;
        selectedObjectIdSamples = objectId;
    }
}

// get sample report details /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function GetSampleReportDetails(objectId, indexId) {
    //debugger;

    // of object id 0  means, register link slide
    if (objectId != 0) {

        selectedObjectIndexIdSamples = indexId;
        selectedObjectIdSamples = objectId;

        var sliderRows = [];

        // get slider content
        var data = jQuery.grep(allSamplesJsonData, function (element, index) {
            return element.ID == selectedObjectIdSamples;
        });

        $.each(data, function () {
            //debugger;

            // slider
            sliderRows.push("<div class='slideSamples' id='" + this.ID + "'>");

            sliderRows.push("<div id='imageslider' class='lefterSamples'>");
            sliderRows.push(imageSliderContentSamples);
            sliderRows.push("</div>");

            sliderRows.push("<div class='centerSamples'>");
            sliderRows.push("</div>");

            sliderRows.push("<div class='righterSamples'>");

            sliderRows.push("<div class='topSamples'>");

            sliderRows.push("<div class='maintitleSamples'>");
            sliderRows.push(this.Name);
            sliderRows.push("</div>");

            //sliderRows.push("<div class='descriptionSamples'>");
            sliderRows.push("<div class='descriptionSamples' title='Description: " + this.LongDescription + "'>");
            sliderRows.push(this.LongDescription);
            sliderRows.push("</div>");

            sliderRows.push("</div>");

            sliderRows.push("<div class='bellowSamples'>");
            sliderRows.push("</div>");

            sliderRows.push("</div>");

            sliderRows.push("</div>");

        });

        sliderContentSamples = sliderRows.join("");

        ShowSlideShowDialogForSamplesPreView(selectedObjectIdSamples, selectedObjectIndexIdSamples, sliderContentSamples, reportIdsSamples.length, reportIdsSamples);

        EllipsisToolTipOnSliderSamples();
    }
    else {

        var sliderRows = [];

        // slider
        sliderRows.push("<div class='register'>");

        sliderRows.push("<div class='r26'>");

        sliderRows.push("<div class='r27'>");
        sliderRows.push("Register and see more examples of how other firms are delivering insights and communicating the value of active management. Click below, it’s simple to register!");
        sliderRows.push("</div>");

        sliderRows.push("<div class='r28'>");
        sliderRows.push("<div class='r29' id='lnkRegisterSlide' title='Click to Register and View Gallery'>");
        sliderRows.push("REGISTER AND VIEW GALLERY");
        sliderRows.push("</div>");
        sliderRows.push("</div>");

        sliderRows.push("<div class='r28'>")

        sliderRows.push("<div class='r30'>");
        sliderRows.push("With Assette:");
        sliderRows.push("</div>");

        sliderRows.push("<div class='r32'>");
        sliderRows.push("<span class='r31'>");
        sliderRows.push("&#10003;");
        sliderRows.push("</span>");
        sliderRows.push("Win clients and retain assets by sharing insights behind your investment results.");
        sliderRows.push("</div>");

        sliderRows.push("<div class='r32'>");
        sliderRows.push("<span class='r31'>");
        sliderRows.push("&#10003;");
        sliderRows.push("</span>");
        sliderRows.push("Free up time to actually serve clients by reducing report preparation time by 85%.");
        sliderRows.push("</div>");

        sliderRows.push("<div class='r32'>");
        sliderRows.push("<span class='r31'>");
        sliderRows.push("&#10003;");
        sliderRows.push("</span>");
        sliderRows.push("Dramatically reduce errors by eliminating manual data rekeying and manipulation.");
        sliderRows.push("</div>");

        sliderRows.push("<div class='r32'>");
        sliderRows.push("<br/><br/>We stand behind our products and services. We promise to delight you through onboarding and beyond with our 60-day unconditional money-back guarantee.");
        sliderRows.push("</div>");

        sliderRows.push("<div class='r32'>");
        sliderRows.push("<br/><br/>So, there is really no risk with Assette.");
        sliderRows.push("</div>");

        sliderRows.push("</div>");

        sliderRows.push("</div>");

        sliderRows.push("</div>");

        sliderContentSamples = sliderRows.join("");

        ShowSlideShowDialogForSamplesPreView(selectedObjectIdSamples, selectedObjectIndexIdSamples, sliderContentSamples, reportIdsSamples.length, reportIdsSamples);
    }
}

// ellipsis/tool tip on slider /////////////////////////////////////////////////////////////////////////////////////////////////////////////

function EllipsisToolTipOnSliderSamples() {
    //debugger;

    // ellipsis
    $('.descriptionSamples:not(.tooltip-temp)').dotdotdot({
        ellipsis: ' ..........'
    });

    // tool tip - append
    var ellipsisShortDescriptionElements = $('.descriptionSamples:contains(" ..........")');

    for (var i = 0; i < ellipsisShortDescriptionElements.length; i++) {

        var shortDescription = ellipsisShortDescriptionElements[i].innerHTML;
        var longDescription = ellipsisShortDescriptionElements[i].title;

        // vtipRight - right side
        shortDescription = shortDescription.replace('..........', '...<div title="' + longDescription + '" class="ui-icon ui-icon-newwin vtipRight" style="float: right; margin-right: 8px; margin-top: 7px; border: 0px solid black;"></div>');

        ellipsisShortDescriptionElements[i].innerHTML = shortDescription;
    }

    // add new class to make it unique
    $('.descriptionSamples').addClass('tooltip-temp');

    // remove title
    $('.descriptionSamples').removeAttr("title");

    // call tool tip functionality
    vtipRight();
}



