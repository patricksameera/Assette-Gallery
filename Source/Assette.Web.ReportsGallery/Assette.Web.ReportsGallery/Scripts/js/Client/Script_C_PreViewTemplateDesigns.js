///////////////////////////////////////////////////////
//********** Assette Gallery       **********//
//********** Assette © 2013                **********//
///////////////////////////////////////////////////////

//debugger;

var idxTemplateDesign = 0;
var sliderContentTemplateDesigns;
var selectedObjectIdTemplateDesigns;
var selectedObjectIndexIdTemplateDesigns;
var reportIdsTemplateDesigns = [];
var imageHandlerIndexTemplateDesigns = 0;
var allTemplateDesignsJsonData;
var uniqueIndex = 0;

// document ready //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// jquery document ready function
$(document).ready(function (e) {
    //debugger;

    /*
    // loading image functionality
    $("#divLoading").ajaxStart(OnAjaxStartForTemplateDesign);
    $("#divLoading").ajaxError(OnAjaxErrorForTemplateDesign);
    $("#divLoading").ajaxSuccess(OnAjaxSuccessForTemplateDesign);
    $("#divLoading").ajaxStop(OnAjaxStopForTemplateDesign);
    $("#divLoading").ajaxComplete(OnAjaxCompleteForTemplateDesign);
    */

    // get template designs
    GetAllTemplateDesignsForPreView();

    // register button click
    $('#lnkRegisterSlide').live("click", function (e) {
        //debugger;

        location.href = "login.aspx";
    });

    // view library button click
    $('.ViewLibraryTemplates').live("click", function (e) {
        //debugger;

        // stop postback
        e.preventDefault();

        // get clicked object's id
        selectedObjectIdTemplate = $(this).attr("Id");

        // get clicked object's index id
        selectedObjectIndexIdTemplate = $(this).attr("Index");

        // get design templates
        GetTemplateDesignPages(selectedObjectIdTemplate, selectedObjectIndexIdTemplate);

        // get report object
        GetTemplateDesignDetails(selectedObjectIdTemplate, selectedObjectIndexIdTemplate);

        // setting div border
        $('.slideViewer').parent().css({ 'border': '1px solid #000000', 'padding-right': '5px', 'padding-bottom': '5px' });
    });

})

// get sample reports /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function GetAllTemplateDesignsForPreView() {
    //debugger;

    $.ajax({
        type: "POST",
        url: "default.aspx/GetAllTemplateDesignsForPreView",
        data: "",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OnGetTemplateDesignsForPreViewSuccess,
        error: OnGetTemplateDesignsForPreViewError
    });
}

function OnGetTemplateDesignsForPreViewError(request, status, error) {
    //debugger;

    // browser loggin
    fireBugConsoleLog(request, status, error);

    // show error to user
    showErrorToUser("");

    // log4net logging
    var errorText = "OnGetTemplateDesignsForPreViewError() jQuery Exception: response text= " + request.responseText + ", status= " + status + ", error= " + error;
    ClientSideLogging(errorText);
}

function OnGetTemplateDesignsForPreViewSuccess(response) {
    //debugger;

    // re-set
    reportIdsTemplateDesigns = [];

    // assigning
    allTemplateDesignsJsonData = $.parseJSON(response.d);

    // get all report id's
    $.each(allTemplateDesignsJsonData, function () {
        //debugger;

        // add all report id's to array
        reportIdsTemplateDesigns.push(this.ID);
    });

    // get report count
    var count = reportIdsTemplateDesigns.length;

    // show data
    ShowAllTemplateDesignPreViewDataOnATable(allTemplateDesignsJsonData);
}

// show data on a table /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function ShowAllTemplateDesignPreViewDataOnATable(data) {
    //debugger;

    var galleryRows = [];
    sliderContentTemplateDesigns = "";

    // register link slider - add 0 report array
    reportIdsTemplateDesigns.push(0);

    // register link slider - increment by another 1
    idxTemplateDesign++;

    $("#DesignsPreView").append(galleryRows.join(""));
}

// loading screen functionality /////////////////////////////////////////////////////////////////////////////////////////////////////////////

function OnAjaxStartForTemplateDesign() {
    //debugger;

    var width = $(window).width();
    var height = $(window).height();

    $("#divLoading").css("left", width / 2 - 68);
    $("#divLoading").css("top", height - 250);

    $("#divLoading").fadeIn('fast');
}

function OnAjaxErrorForTemplateDesign() {
    //debugger;

}

function OnAjaxSuccessForTemplateDesign() {
    //debugger;

    $("#divLoading").delay(500).fadeOut('slow');
}

function OnAjaxStopForTemplateDesign() {
    //debugger;

    $("#divLoading").delay(500).fadeOut('slow');
}

function OnAjaxCompleteForTemplateDesign() {
    //debugger;

    $("#divLoading").delay(500).fadeOut('slow');
}

// get sample report pages /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function GetTemplateDesignPages(objectId, indexId) {
    //debugger;

    // of object id 0  means, register link slide
    if (objectId != 0) {

        selectedObjectIndexIdTemplate = indexId;
        selectedObjectIdTemplate = objectId;

        // get slider content
        var data = jQuery.grep(allTemplateDesignsJsonData, function (element, index) {
            return element.ID == selectedObjectIdTemplate;
        });

        var imageSliderRows = [];
        var recordId;
        var noOfPages;
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

        for (var i = 0; i < noOfPages; i++) {
            //debugger;

            var newRecordId = parseInt(recordId);
            var newPageId = parseInt(i) + 1;

            imageSliderRows.push("<li>");

            //imageSliderRows.push("<img class='urlImageLarge' index='" + newPageId + "' src='ImageHandler.ashx?id=" + newRecordId + "&type=t_i&date=" + UploadedDate + "' width='800' height='480' border='0' />");
            imageSliderRows.push("<img alt='" + newPageId + "' class='urlImageLarge' index='" + newPageId + "' src='ImageHandler.ashx?id=" + newRecordId + "&type=t_i&date=" + UploadedDate + "&uniqueindex=" + uniqueIndex + "&page=" + newPageId + "' border='0' width='" + dimensionsObject.sliderImageWidth + "' height='" + dimensionsObject.sliderImageHeight + "' border='1'  />");

            imageSliderRows.push("</li>");

        }

        imageSliderRows.push("</ul>");
        imageSliderRows.push("</div>");
        imageSliderRows.push("</div>");

        imagesliderContentTemplate = imageSliderRows.join("");
    }
    else {

        selectedObjectIndexIdTemplate = indexId;
        selectedObjectIdTemplate = objectId;
    }

}

// get sample report details /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function GetTemplateDesignDetails(objectId, indexId) {
    //debugger;

    // of object id 0  means, register link slide
    if (objectId != 0) {

        selectedObjectIndexIdTemplateDesigns = indexId;
        selectedObjectIdTemplateDesigns = objectId;

        var sliderRows = [];

        // get slider content
        var data = jQuery.grep(allTemplateDesignsJsonData, function (element, index) {
            return element.ID == selectedObjectIdTemplateDesigns;
        });

        $.each(data, function () {
            //debugger;

            // slider
            sliderRows.push("<div class='slide' id='" + this.ID + "'>");

            sliderRows.push("<div id='imageslider' class='lefterTemplates'>");
            sliderRows.push(imagesliderContentTemplate);
            sliderRows.push("</div>");

            sliderRows.push("<div class='centerTemplates'>");
            sliderRows.push("</div>");

            sliderRows.push("<div class='rightTemplates'>");

            sliderRows.push("<div class='topTemplates'>");

            sliderRows.push("<div class='maintitleTemplates'>");
            sliderRows.push(this.Name);
            sliderRows.push("</div>");

            //sliderRows.push("<div class='descriptionTemplates'>");
            sliderRows.push("<div class='descriptionTemplates' title='Description: " + this.LongDescription + "'>");
            sliderRows.push(this.LongDescription);
            sliderRows.push("</div>");

            sliderRows.push("</div>");

            sliderRows.push("<div class='bellowTemplates'>");
            sliderRows.push("</div>");

            sliderRows.push("</div>");

            sliderRows.push("</div>");

        });

        sliderContentTemplate = sliderRows.join("");

        ShowSlideShowDialogForTemplateDesignPreView(selectedObjectIdTemplate, selectedObjectIndexIdTemplate, sliderContentTemplate, reportIdsTemplateDesigns.length, reportIdsTemplateDesigns);

        EllipsisToolTipOnSliderTemplates();

    }
    else {
        //debugger;

        var sliderRows = [];

        // slider
        sliderRows.push("<div class='register'>");

        sliderRows.push("<div class='r26'>");

        sliderRows.push("<div class='r27'>");
        sliderRows.push("Register and see more designs that make your client communications beautiful and contemporary. Click below, it’s simple to register!");
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

        sliderContentTemplate = sliderRows.join("");

        ShowSlideShowDialogForTemplateDesignPreView(selectedObjectIdTemplate, selectedObjectIndexIdTemplate, sliderContentTemplate, reportIdsTemplateDesigns.length, reportIdsTemplateDesigns);
    }

}

// ellipsis/tool tip on slider /////////////////////////////////////////////////////////////////////////////////////////////////////////////

function EllipsisToolTipOnSliderTemplates() {
    //debugger;

    // ellipsis
    $('.descriptionTemplates:not(.tooltip-temp)').dotdotdot({
        ellipsis: ' ..........'
    });

    // tool tip - append
    var ellipsisShortDescriptionElements = $('.descriptionTemplates:contains(" ..........")');

    for (var i = 0; i < ellipsisShortDescriptionElements.length; i++) {

        var shortDescription = ellipsisShortDescriptionElements[i].innerHTML;
        var longDescription = ellipsisShortDescriptionElements[i].title;

        // vtipRight - right side
        shortDescription = shortDescription.replace('..........', '...<div title="' + longDescription + '" class="ui-icon ui-icon-newwin vtipRight" style="float: right; margin-right: 8px; margin-top: 7px; border: 0px solid black;"></div>');

        ellipsisShortDescriptionElements[i].innerHTML = shortDescription;
    }

    // add new class to make it unique
    $('.descriptionTemplates').addClass('tooltip-temp');

    // remove title
    $('.descriptionTemplates').removeAttr("title");

    // call tool tip functionality
    vtipRight();
}







