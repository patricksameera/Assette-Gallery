///////////////////////////////////////////////////////
//********** Assette Gallery       **********//
//********** Assette © 2013                **********//
///////////////////////////////////////////////////////

//debugger;

var idxReportObjects = 0;
var sliderContentReportObjects;
var selectedObjectIdReportObjects;
var selectedObjectIndexIdReportObjects;
var allPaObjectReportIds = [];
var allPAObjectsJsonData;
var uniqueIndex = 0;
var sliderImageWidth;
var sliderImageHeight;
var tableType;
var queryReportTableTypes = [];

// document ready //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// jquery document ready function
$(document).ready(function (e) {
    //debugger;

    // get delay time from web config
    var timeDelay = $("[id$=hdnDelayTime]").val();

    // ajax loading image
    $("#divLoading").fadeIn('medium');
    $("#divLoading").delay(timeDelay).fadeOut('medium');
    $(".r25").text("Loading...");
    $("#divLoading").css("left", "46%");
    $("#divLoading").css("top", "50%");

    var p = $("#ReportObjectsPreView");
    if (p != null) {
        var position = p.position();
        $("#divLoading").css("top", position.top);
    }

    /*
    // loading image functionality
    $("#divLoading").ajaxStart(OnAjaxStartForReportObjects);
    $("#divLoading").ajaxError(OnAjaxErrorForReportObjects);
    $("#divLoading").ajaxSuccess(OnAjaxSuccessForReportObjects);
    $("#divLoading").ajaxStop(OnAjaxStopForReportObjects);
    $("#divLoading").ajaxComplete(OnAjaxCompleteForReportObjects);
    */

    // get pa-objects
    GetAllPAObjectsForPreView();

    // register button click
    $('#lnkRegisterSlide').live("click", function (e) {
        //debugger;

        location.href = "login.aspx";
    });

    // view library button click
    $('.ViewLibraryReportObject').live("click", function (e) {
        //debugger;

        // stop postback
        e.preventDefault();

        // get clicked object's id
        selectedObjectIdReportObjects = $(this).attr("Id");

        // get clicked object's index id
        selectedObjectIndexIdReportObjects = $(this).attr("Index");

        // get clicked object's table type
        tableType = $(this).attr("TableType");

        // get report object
        GetPAObjectDetails(selectedObjectIdReportObjects, selectedObjectIndexIdReportObjects, tableType);
    });

})

// get pa-object /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function GetAllPAObjectsForPreView() {
    //debugger;

    $.ajax({
        type: "POST",
        url: "default.aspx/GetAllPAObjectsForPreView",
        data: "",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OnGetAllPAObjectsForPreViewSuccess,
        error: OnGetAllPAObjectsForPreViewError
    });
}

function OnGetAllPAObjectsForPreViewError(request, status, error) {
    //debugger;

    // browser loggin
    fireBugConsoleLog(request, status, error);

    // show error to user
    showErrorToUser("");

    // log4net logging
    var errorText = "OnGetAllPAObjectsForPreViewError() jQuery Exception: response text= " + request.responseText + ", status= " + status + ", error= " + error;
    ClientSideLogging(errorText);
}

function OnGetAllPAObjectsForPreViewSuccess(response) {
    //debugger;

    // re-set
    allPaObjectReportIds = [];


    // assigning
    allPAObjectsJsonData = $.parseJSON(response.d);

    // get all report id's
    $.each(allPAObjectsJsonData, function () {
        //debugger;

        // add all report id's to array
        allPaObjectReportIds.push(this.ID);

        // add query report table types
        queryReportTableTypes.push(this.ObjectTableType);
    });

    // get report count
    var count = allPaObjectReportIds.length;

    // show data
    ShowPAObjectPreViewDataOnATable(allPAObjectsJsonData);
}

// loading screen functionality /////////////////////////////////////////////////////////////////////////////////////////////////////////////

function OnAjaxStartForReportObjects() {
    //debugger;

    var width = $(window).width();
    var height = $(window).height();

    $("#divLoading").css("left", width / 2 - 68);
    $("#divLoading").css("top", height - 250);

    $("#divLoading").fadeIn('fast');
}

function OnAjaxErrorForReportObjects() {
    //debugger;

}

function OnAjaxSuccessForReportObjects() {
    //debugger;

    $("#divLoading").delay(500).fadeOut('slow');
}

function OnAjaxStopForReportObjects() {
    //debugger;

    $("#divLoading").delay(500).fadeOut('slow');
}

function OnAjaxCompleteForReportObjects() {
    //debugger;

    $("#divLoading").delay(500).fadeOut('slow');
}

// show data on a table /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function ShowPAObjectPreViewDataOnATable(data) {
    //debugger;

    var galleryRows = [];
    sliderContentReportObjects = "";

    // register link slider - add 0 report array
    allPaObjectReportIds.push(0);

    // register link slider - increment by another 1
    idxReportObjects++;

    $("#ReportObjectsPreView").append(galleryRows.join(""));
}

// loading screen functionality /////////////////////////////////////////////////////////////////////////////////////////////////////////////

function OnAjaxStart() {
    //debugger;

    var width = $(window).width();
    var height = $(window).height();

    $("#divLoading").css("left", width / 2 - 68);
    $("#divLoading").css("top", height - 250);

    $("#divLoading").fadeIn('fast');
}

function OnAjaxError() {
    //debugger;
}

function OnAjaxSuccess() {
    //debugger;

    $("#divLoading").delay(500).fadeOut('slow');
}

function OnAjaxStop() {
    //debugger;

    $("#divLoading").delay(500).fadeOut('slow');
}

function OnAjaxComplete() {
    //debugger;

    $("#divLoading").delay(500).fadeOut('slow');
}

// get report object details /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function GetPAObjectDetails(objectId, indexId, tableType) {
    //debugger;

    // of object id 0  means, register link slide
    if (objectId != 0) {

        selectedObjectIndexIdReportObjects = indexId;
        selectedObjectIdReportObjects = objectId;

        var sliderRows = [];

        // get slider content
        var data = jQuery.grep(allPAObjectsJsonData, function (element, index) {
            return element.ID == selectedObjectIdReportObjects && element.ObjectTableType == tableType;
        });

        //debugger;

        // setting modal dialog image width
        var dimensionsObject = GetDialogDimensionsForReportObjects();

        sliderImageWidth = dimensionsObject.sliderImageWidth;
        sliderImageHeight = dimensionsObject.sliderImageHeight;

        $.each(data, function () {
            //debugger;

            // increment
            //uniqueIndex++;

            // get date
            var jsonDate = new Date(parseInt(this.ModifiedDate.substr(6)));
            var d = jsonDate.getDate();
            var m = jsonDate.getMonth() + 1;
            var y = jsonDate.getFullYear();
            var hh = jsonDate.getHours();
            var mm = jsonDate.getMinutes();
            var ss = jsonDate.getSeconds();
            var ModifiedDate = y + "" + (m <= 9 ? '0' + m : m) + "" + (d <= 9 ? '0' + d : d) + "" + (hh <= 9 ? '0' + hh : hh) + "" + (mm <= 9 ? '0' + mm : mm) + "" + (ss <= 9 ? '0' + ss : ss);

            // slider
            sliderRows.push("<div class='slide' id='" + this.ID + "'>");
            sliderRows.push("<div class='lefter'>");

            //sliderRows.push("<img class='urlImageLarge' src='ImageHandler.ashx?id=" + this.ID + "&type=r_i&date=" + ModifiedDate + "' width=800' height='540'  border='1' style='float: left; padding-left: 2px; padding-top: 5px;' />");
            //sliderRows.push("<img class='urlImageLarge' src='ImageHandler.ashx?id=" + this.ID + "&type=r_i&date=" + ModifiedDate + "&uniqueindex=" + uniqueIndex + "' border='1' style='float: left; padding: 2px;' width='" + sliderImageWidth + "' height='" + sliderImageHeight + "' border='0'  />");

            /*
            if (this.ObjectTableType == 'p') {
            sliderRows.push("<img class='urlImageLarge' src='ImageHandler.ashx?id=" + this.ID + "&type=r_i&date=" + ModifiedDate + "&uniqueindex=" + uniqueIndex + "' style='float: left; padding: 0px;' width='" + sliderImageWidth + "' height='" + sliderImageHeight + "' border='1'  />");
            }
            else {
            sliderRows.push("<img class='urlImageLarge' src='ImageHandler.ashx?id=" + this.ID + "&type=m_i&date=" + ModifiedDate + "&uniqueindex=" + uniqueIndex + "' style='float: left; padding: 0px;' width='" + sliderImageWidth + "' height='" + sliderImageHeight + "' border='1'  />");
            }
            */

            sliderRows.push("<div class='LoadingDiv'  style='width:" + (sliderImageWidth + 10) + "px; height:" + (sliderImageHeight + 10) + "px;'>");

            if (this.ObjectTableType == 'p') {
                sliderRows.push("<img onload='SetImageSize()' class='urlImageLarge' src='ImageHandler.ashx?id=" + this.ID + "&type=r_i&date=" + ModifiedDate + "&uniqueindex=" + uniqueIndex + "' style='float: left; padding: 5px; opacity:0;' border='0'  />");
            }
            else {
                sliderRows.push("<img onload='SetImageSize()' class='urlImageLarge' src='ImageHandler.ashx?id=" + this.ID + "&type=m_i&date=" + ModifiedDate + "&uniqueindex=" + uniqueIndex + "' style='float: left; padding: 5px; opacity:0;' border='0'  />");
            }

            sliderRows.push("</div>");
            sliderRows.push("</div>");
            sliderRows.push("<div class='center'>");
            sliderRows.push("</div>");
            sliderRows.push("<div class='righter'>");

            sliderRows.push("<div class='top'>");

            sliderRows.push("<div class='maintitle'>");
            sliderRows.push(this.Name);
            sliderRows.push("</div>");

            sliderRows.push("<div class='subtitle'>");
            sliderRows.push(this.Category);
            sliderRows.push("</div>");

            //sliderRows.push("<div class='description'>");
            sliderRows.push("<div class='description' title='Description: " + this.Description + "'>");
            sliderRows.push(this.Description);
            sliderRows.push("</div>");

            sliderRows.push("</div>");

            sliderRows.push("<div class='bellow'>");

//            if (this.ClientType != 'Easy') {

//                sliderRows.push("<div style='border: 0px solid #000000; margin-top: 0px; float: right;'>");

//                sliderRows.push("<div style='float: left;'>");
//                sliderRows.push("<img src='Images/Easy.jpg' style='margin-top: .5em; margin-right: .3em;' />");
//                sliderRows.push("</div>");

//                sliderRows.push("<div class='clienttype' style='float: left; padding-top: 3px;'>");
//                sliderRows.push("Not available in Easy Editions");
//                sliderRows.push("</div>");

//                sliderRows.push("</div>");
//            }

            sliderRows.push("</div>");

            sliderRows.push("</div>");
            sliderRows.push("</div>");

        });

        sliderContentReportObjects = sliderRows.join("");

        ShowSlideShowDialogReportObjectsForPreView(selectedObjectIdReportObjects, selectedObjectIndexIdReportObjects, sliderContentReportObjects, allPaObjectReportIds.length, allPaObjectReportIds, queryReportTableTypes);

        EllipsisToolTipOnSlider();
    }
    else {

        selectedObjectIndexIdReportObjects = indexId;
        selectedObjectIdReportObjects = objectId;

        var sliderRows = [];

        // slider
        sliderRows.push("<div class='register'>");

        sliderRows.push("<div class='r26'>");

        sliderRows.push("<div class='r27'>");
        sliderRows.push("Register and see more report layouts that let you easily communicate the value of active management. Click below, it’s simple to register!");
        sliderRows.push("</div>");

        sliderRows.push("<div class='r28'>");
        sliderRows.push("<div class='r29' id='lnkRegisterSlide' title='click to Register and View Gallery'>");
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

        sliderContentReportObjects = sliderRows.join("");

        ShowSlideShowDialogReportObjectsForPreView(selectedObjectIdReportObjects, selectedObjectIndexIdReportObjects, sliderContentReportObjects, allPaObjectReportIds.length, allPaObjectReportIds, queryReportTableTypes);
    }
}

function OnGetPAObjectDetailsError(request, status, error) {
    //debugger;

    // browser loggin
    fireBugConsoleLog(request, status, error);

    // show error to user
    showErrorToUser("");

    // log4net logging
    var errorText = "OnGetPAObjectDetailsError() jQuery Exception: response text= " + request.responseText + ", status= " + status + ", error= " + error;
    ClientSideLogging(errorText);
}

// ellipsis/tool tip on slider /////////////////////////////////////////////////////////////////////////////////////////////////////////////

function EllipsisToolTipOnSlider() {
    //debugger;

    // ellipsis
    $('.description:not(.tooltip-temp)').dotdotdot({
        ellipsis: ' ..........'
    });

    // tool tip - append
    var ellipsisShortDescriptionElements = $('.description:contains(" ..........")');

    for (var i = 0; i < ellipsisShortDescriptionElements.length; i++) {

        var shortDescription = ellipsisShortDescriptionElements[i].innerHTML;
        var longDescription = ellipsisShortDescriptionElements[i].title;

        // vtipRight - right side
        shortDescription = shortDescription.replace('..........', '...<div title="' + longDescription + '" class="ui-icon ui-icon-newwin vtipRight" style="float: right; margin-right: 8px; margin-top: 7px; border: 0px solid black;"></div>');

        ellipsisShortDescriptionElements[i].innerHTML = shortDescription;
    }

    // add new class to make it unique
    $('.description').addClass('tooltip-temp');

    // remove title
    $('.description').removeAttr("title");

    // call tool tip functionality
    vtipRight();
}


