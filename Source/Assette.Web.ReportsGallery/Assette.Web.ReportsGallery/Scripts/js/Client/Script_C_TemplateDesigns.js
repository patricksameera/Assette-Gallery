///////////////////////////////////////////////////////
//********** Assette Gallery       **********//
//********** Assette © 2013                **********//
///////////////////////////////////////////////////////

//debugger;


var idx = 0;
var sliderContent;
var imageSliderContent;
var selectedObjectId;
var selectedObjectIndexId;
var objectTypeNone = 0;
var objectTypeTable = 1;
var objectTypeChart = 2;
var objectType;
var pageIndex = 1;
var recordRowCount = 0;
var allReportIds = [];
var queryReportIds = [];
var allObjectsJsonData;
var uniqueIndex = 0;

// body on load //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//$("body").on({
//    ajaxStart: function () {
//        $(this).addClass("loading");
//    },
//    ajaxStop: function () {
//        $(this).removeClass("loading");
//    }
//});

//$("body").on({
//    ajaxStart: function () {
//        $("#divPageLoading").fadeIn('fast');
//    },
//    ajaxStop: function () {
//        $("#divPageLoading").fadeOut('slow');
//    }
//});

// document ready //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// jquery document ready function
$(document).ready(function (e) {

    // get delay time from web config
    var timeDelay = $("[id$=hdnDelayTime]").val();

    // ajax loading image
    $("#divLoading").fadeIn('medium');
    $("#divLoading").delay(timeDelay).fadeOut('medium');
    $(".t14").text("Loading...");
    $("#divLoading").css("left", "50%");
    $("#divLoading").css("top", "50%");

    // get row count
    GetRowCount();

    // window resize
    WindowReSize();

    $(window).resize(function () {

        // window resize event
        WindowReSize();

        // tool bar re-size
        WindowWidthReSize();
    });

    // water mark
    $("#Search").Watermark("SEARCH");

    // set selected link
    $('#btnAll').css('color', '#C1D82F');

    // jquery select box - set the initial place holder
    $("#Firm").selectbox('attach');
    $("#Product").selectbox('attach');

    // set selected tab link   
    $('#samples').css('background-color', '#EFEFEF');
    $('#objects').css('background-color', '#EFEFEF');
    //$('#templates').css('background-color', '#C1D82F');
    $('#templates').css('background-color', '#0397D6');
    $('#templates').css('color', '#ffffff');

    /*
    // loading image functionality
    $("#divLoading").ajaxStart(OnAjaxStart);
    $("#divLoading").ajaxError(OnAjaxError);
    $("#divLoading").ajaxSuccess(OnAjaxSuccess);
    $("#divLoading").ajaxStop(OnAjaxStop);
    $("#divLoading").ajaxComplete(OnAjaxComplete);
    */

    // fill dropdowns - bind values
    GetFirmTypes();
    GetProductTypes();

    // get template designs
    GetAllTemplateDesigns();

    // scrolling load
    $(window).scroll(function () {

        if ($(window).scrollTop() == $(document).height() - $(window).height()) {
            //debugger;

            // get the pa-object div count
            var count = $(".t7").length;

            var x = count;
            var y = 5;
            x %= y;

            if ($("#hdnScroll").val() == "1" && x == 0) {

                // ajax loading image
                $("#divLoading").hide();
                $("#divLoading").fadeIn('medium');
                $("#divLoading").delay(200).fadeOut('medium');
                $(".t14").text("Loading...");
                $("#divLoading").css("left", "50%");
                $("#divLoading").css("top", "50%");

                //////////////////////////////////////////////

                pageIndex++;

                $("#divTopLink").empty();

                // filter data
                FilterData();
            }
            else {

                if ($("body").height() > $(window).height()) {

                    $("#divTopLink").empty();

                    var galleryRowsLink = [];

                    galleryRowsLink.push("<div style='width: 900px; border: 0px solid #000000; height: 30px;'>");
                    galleryRowsLink.push("<a style='float: right; padding-right: 53px; padding-top: 10px; outline: none;' href='#top'><img width='73' height='20' border='0' src='Images/top.gif'></a>");
                    galleryRowsLink.push("</div>");

                    $("#divTopLink").append(galleryRowsLink.join(""));
                }
            }
        }
    });

    // mouse wheel load
    $(window).bind('mousewheel', function (event, delta, deltaX, deltaY) {
        //debugger;

        //delta == "-1" mouse wheel - up
        //delta == "1" mouse wheel - down

        if ($(window).scrollTop() == $(document).height() - $(window).height() && delta == "-1") {
            //debugger;

            // get the pa-object div count
            var count = $(".t7").length;

            var x = count;
            var y = 5;
            x %= y;

            if ($("#hdnScroll").val() == "1" && x == 0) {

                // ajax loading image
                $("#divLoading").hide();
                $("#divLoading").fadeIn('medium');
                $("#divLoading").delay(200).fadeOut('medium');
                $(".t14").text("Loading...");
                $("#divLoading").css("left", "50%");
                $("#divLoading").css("top", "50%");

                //////////////////////////////////////////////

                pageIndex++;

                $("#divTopLink").empty();

                // filter data
                FilterData();
            }
            else {

                if ($("body").height() > $(window).height()) {

                    $("#divTopLink").empty();

                    var galleryRowsLink = [];

                    galleryRowsLink.push("<div style='width: 900px; border: 0px solid #000000; height: 30px;'>");
                    galleryRowsLink.push("<a style='float: right; padding-right: 53px; padding-top: 10px; outline: none;' href='#top'><img width='73' height='20' border='0' src='Images/top.gif'></a>");
                    galleryRowsLink.push("</div>");

                    $("#divTopLink").append(galleryRowsLink.join(""));
                }
            }
        }
    });

    // key press - enter
    $(document).keypress(function (e) {
        if (e.which == 13) {
            //debugger;

            // stop postback
            e.preventDefault();

            // set selected link
            $('#btnAll').css('color', '#7a7a7a');

            //////////////////////////////////////////////

            pageIndex = 1;

            // filter data
            FilterData();
        }
    });

    // search button click
    $('#btnSearch').live("click", function (e) {
        //debugger;

        // set selected link
        $('#btnAll').css('color', '#7a7a7a');

        //////////////////////////////////////////////

        pageIndex = 1;

        // filter data
        FilterData();
    });

    // view library button click
    $('.ViewLibrary').live("click", function (e) {
        //debugger;

        // stop postback
        e.preventDefault();

        // get clicked object's id
        selectedObjectId = $(this).attr("Id");

        // get clicked object's index id
        selectedObjectIndexId = $(this).attr("Index");

        // get design template pages
        GetTemplateDesignPages(selectedObjectId, selectedObjectIndexId);

        // get design template details
        GetTemplateDesignDetails(selectedObjectId, selectedObjectIndexId);

        // setting div border
        $('.slideViewer').parent().css({ 'border': '1px solid #000000', 'padding-right': '5px', 'padding-bottom': '5px' });

    });

    // re-set filter
    $('#btnAll').live("click", function (e) {
        //debugger;

        // re-set fields /////////////////////////////

        // clear search field
        $("#Search").val('');
        $("#Search").Watermark("SEARCH");

        // set selected link
        $('#btnAll').css('color', '#C1D82F');

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

        //dropdown = document.getElementById("Product");
        //value = dropdown.options[0].text;

        $SelectedHref.text(value);

        // set value in html-select
        $("#Firm").val(0);
        //$("#Product").val(0);

        //////////////////////////////////////////////

        pageIndex = 1;

        // filter data
        FilterData();
    });

})

// get sample reports /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function GetAllTemplateDesigns() {
    //debugger;

    $.ajax({
        type: "POST",
        url: "template-designs.aspx/GetAllTemplateDesigns",
        data: "",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: true,
        success: OnGetAllTemplateDesignsSuccess,
        error: OnGetAllTemplateDesignsError
    });
}

function OnGetAllTemplateDesignsError(request, status, error) {
    //debugger;

    // browser loggin
    fireBugConsoleLog(request, status, error);

    // show error to user
    showErrorToUser("");

    // log4net logging
    var errorText = "OnGetAllTemplateDesignsError() jQuery Exception: response text= " + request.responseText + ", status= " + status + ", error= " + error;
    ClientSideLogging(errorText);
}

function OnGetAllTemplateDesignsSuccess(response) {
    //debugger;

    // assigning
    allObjectsJsonData = $.parseJSON(response.d);

    // get all report id's
    $.each(allObjectsJsonData, function () {
        //debugger;

        // add all report id's to array
        allReportIds.push(this.ID);

        // add query report id's to array - only on initial load, after that through filter
        queryReportIds.push(this.ID);
    });

    var count = allReportIds.length;

    // show data
    ShowDataOnATable(allObjectsJsonData);
}

// filter data /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function FilterData() {
    //debugger;

    //example
    /*
    "ID":149,
    "Name":"sample report 2",
    "ShortDescription":"short 2",
    "LongDescription":"long 2",
    "FirmTypeId":2,
    "FirmName":"Wealth Management",
    "ProductTypeId":1,
    "ProductName":"Standard Edition"
    */

    // re-set
    idx = 0;
    queryReportIds = [];

    // get data
    var firmId = $("#Firm").val();
    var productId = $("#Product").val();
    var searchText = $("#Search").val();

    if (firmId == null) firmId = 0;
    if (productId == null) productId = 0;
    if (searchText == "SEARCH") searchText = "";

    var data = allObjectsJsonData;
    var rowCount = data.length;

    /// filter by FirmType ///////////////////////////////////////////////////////////////////////////////////////////////////////

    if (firmId != "0") {
        data = jQuery.grep(data, function (element, index) {
            return element.FirmTypeId == firmId;
        });
    }

    rowCount = data.length;

    // filter by Product ///////////////////////////////////////////////////////////////////////////////////////////////////////

    if (productId != "0") {
        data = jQuery.grep(data, function (element, index) {
            return element.ProductTypeId == productId;
        });
    }

    rowCount = data.length;

    // filter by text ///////////////////////////////////////////////////////////////////////////////////////////////////////

    if (searchText != "") {
        data = jQuery.grep(data, function (element, index) {
            return element.Name.toLowerCase().indexOf(searchText.toLowerCase()) != -1;
        });
    }

    rowCount = data.length;

    // get query report id's ///////////////////////////////////////////////////////////////////////////////////////////////////////
    $.each(data, function () {
        //debugger;

        // add query report id's to array
        queryReportIds.push(this.ID);
    });

    var count = queryReportIds.length;

    // pass data to table for showing ///////////////////////////////////////////////////////////////////////////////////////////////////////
    ShowDataOnATable(data);
}

// show data on a table /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function ShowDataOnATable(data) {
    //debugger;

    // re-set
    var galleryRows = [];
    idx = 0;

    var loopBreakRecordCount = parseInt(recordRowCount) * parseInt(pageIndex) * 5; // 5: no of records per row

    // clear contents
    $("#divTemplateDesigns").empty();
    $("#divTopLink").empty();

    $.each(data, function () {
        //debugger;

        // get date
        var jsonDate = new Date(parseInt(this.UploadedDate.substr(6)));
        var d = jsonDate.getDate();
        var m = jsonDate.getMonth() + 1;
        var y = jsonDate.getFullYear();
        var hh = jsonDate.getHours();
        var mm = jsonDate.getMinutes();
        var ss = jsonDate.getSeconds();
        var UploadedDate = y + "" + (m <= 9 ? '0' + m : m) + "" + (d <= 9 ? '0' + d : d) + "" + (hh <= 9 ? '0' + hh : hh) + "" + (mm <= 9 ? '0' + mm : mm) + "" + (ss <= 9 ? '0' + ss : ss);

        // increment the index
        idx++;
        //uniqueIndex++;

        // gallery
        galleryRows.push("<div class='t7'>");
        galleryRows.push("<div class='t9' title='Click to view design'>");
        galleryRows.push("<a href='#' class='ViewLibrary' Index='" + idx + "' Id='" + this.ID + "'><img class='urlImageSmall' src='ImageHandler.ashx?id=" + this.ID + "&type=t_t&date=" + UploadedDate + "&uniqueindex=" + uniqueIndex + "' width='140' height='90'  border='0' style='float: left;padding:2px;' /></a>");
        galleryRows.push("</div>");
        galleryRows.push("<div class='t10' title='Description: " + this.ShortDescription + "'>");
        galleryRows.push("<b>" + this.Name + "</b>");
        galleryRows.push("<br />");
        //galleryRows.push(this.ShortDescription);
        galleryRows.push("<br />");
        //galleryRows.push(this.ID + "-" + this.FirmName + "-" + this.ProductName);
        galleryRows.push("</div>");
        galleryRows.push("<div class='t12'>");
        galleryRows.push("</div>");
        galleryRows.push("</div>");

        if (idx == loopBreakRecordCount) {

            if (idx == queryReportIds.length) {

                $("#hdnScroll").val("0"); // no more records for loading through scrolling
            }
            else {

                $("#hdnScroll").val("1"); // there are more records for loading through scrolling
            }

            return false;
        }
    });

    if (idx < loopBreakRecordCount) {
        $("#hdnScroll").val("0"); // no more records for loading through scrolling
    }

    if (idx == 0) {
        galleryRows.push("<div class='t17'>");
        galleryRows.push("No matching data found.");
        galleryRows.push("</div>");

        $("#divTemplateDesigns").html(galleryRows.join(""));
    }
    else {
        $("#divTemplateDesigns").append(galleryRows.join(""));
    }

    // resize for window width
    WindowWidthReSize();

    // call ellipsis and tool tip functionality
    EllipsisToolTip();
}

// loading screen functionality /////////////////////////////////////////////////////////////////////////////////////////////////////////////

function OnAjaxStart() {
    //debugger;

    $("#divLoading").css("left", "50%");
    $("#divLoading").css("top", "50%");

    $("#divLoading").fadeIn('slow');
}

function OnFailure(response) {
    //debugger;

}

function OnError(response) {
    //debugger;

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

function GetFirmTypes() {
    //debugger;

    $.ajax({
        type: "POST",
        url: "template-designs.aspx/GetFirmTypes",
        data: "",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: true,
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

    // log4net logging
    var errorText = "OnGetFirmTypesError() jQuery Exception: response text= " + request.responseText + ", status= " + status + ", error= " + error;
    ClientSideLogging(errorText);
}

function OnGetFirmTypesSuccess(response) {
    //debugger;

    var dropDown = $("#Firm");

    bindDataToDropDown(response, dropDown, "CLIENT TYPE - ALL");
}

// get product types details /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function GetProductTypes() {
    //debugger;

    $.ajax({
        type: "POST",
        url: "template-designs.aspx/GetProductTypes",
        data: "",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: true,
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

    // log4net logging
    var errorText = "OnGetProductTypesError() jQuery Exception: response text= " + request.responseText + ", status= " + status + ", error= " + error;
    ClientSideLogging(errorText);
}

function OnGetProductTypesSuccess(response) {
    //debugger;

    var dropDown = $("#Product");

    bindDataToDropDown(response, dropDown, "PRODUCT - ALL");
}

// bind data to dropdown /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function bindDataToDropDown(response, dropdown, defaultText) {
    //debugger;

    data = $.parseJSON(response.d);

    if (data == "-1") // error
    {
        // show error to user
        showErrorToUser("");
    }
    else {

        // clear dropdown
        dropdown.empty();

        // detach dropdown
        dropdown.selectbox('detach');

        dropdown.append('<option value=0>' + defaultText + '</option>');

        $.each(data, function () {

            dropdown.append('<option value=' + this.ID + '>' + this.Name + '</option>');

        });

        // jquery select box - set
        //dropdown.selectbox();

        // jquery select box - set
        /*
        dropdown.selectbox({
        onChange: function (val, inst) {
        pageIndex = 1;
        }
        });*/

        $('#Firm').selectbox({
            onChange: function (val, inst) {
                //debugger;

                // set selected link
                $('#btnAll').css('color', '#7a7a7a');

                //////////////////////////////////////////////

                pageIndex = 1;

                // filter data
                FilterData();
            }
        });

        $('#Product').selectbox({
            onChange: function (val, inst) {
                //debugger;

                // set selected link
                $('#btnAll').css('color', '#7a7a7a');

                //////////////////////////////////////////////

                pageIndex = 1;

                // filter data
                FilterData();
            }
        });
    }
}

// get sample report pages /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function GetTemplateDesignPages(objectId, indexId) {
    //debugger;

    selectedObjectId = objectId;
    selectedObjectIndexId = indexId;

    // get slider content
    var data = jQuery.grep(allObjectsJsonData, function (element, index) {
        return element.ID == objectId;
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

    // image slider
    for (var i = 0; i < noOfPages; i++) {
        //debugger;

        // increment
        //uniqueIndex++;

        var newRecordId = parseInt(recordId);
        var newPageId = parseInt(i) + 1;

        imageSliderRows.push("<li>");

        //imageSliderRows.push("<img class='urlImageLarge' alt='" + newPageId + "' id=" + newRecordId + " index='" + newPageId + "' src='ImageHandler.ashx?id=" + newRecordId + "&type=t_i&date=" + UploadedDate + "' width='770' height='480' border='0' />");

        imageSliderRows.push("<img class='urlImageLarge' alt='" + newPageId + "' id=" + newRecordId + " index='" + newPageId + "' src='ImageHandler.ashx?id=" + newRecordId + "&type=t_i&date=" + UploadedDate + "&uniqueindex=" + uniqueIndex + "&page=" + newPageId + "' border='0' width='" + dimensionsObject.sliderImageWidth + "' height='" + dimensionsObject.sliderImageHeight + "' border='1'  />");

        imageSliderRows.push("</li>");

    }

    imageSliderRows.push("</ul>");
    imageSliderRows.push("</div>");
    imageSliderRows.push("</div>");

    imageSliderContent = imageSliderRows.join("");
}

// get sample report details /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function GetTemplateDesignDetails(objectId, indexId) {
    //debugger;

    selectedObjectIndexId = indexId;
    selectedObjectId = objectId;

    var sliderRows = [];

    // get slider content
    var data = jQuery.grep(allObjectsJsonData, function (element, index) {
        return element.ID == objectId;
    });

    $.each(data, function () {
        //debugger;

        var ModifiedDate = 0;

        // slider
        sliderRows.push("<div class='slide' id='" + this.ID + "'>");
        //sliderRows.push(this.ID);
        sliderRows.push("<div id='imageslider' class='lefter'>");
        //sliderRows.push("<img class='urlImageSmall' src='ImageHandler.ashx?id=" + this.ID + "&type=s_i' width='390' height='340'  border='0' style='float: left; padding-left: 2px; padding-top: 5px;' />");
        sliderRows.push(imageSliderContent);
        sliderRows.push("</div>");
        sliderRows.push("<div class='center'>");
        sliderRows.push("</div>");
        sliderRows.push("<div class='righter'>");
        sliderRows.push("<div class='top'>");
        sliderRows.push("<div class='maintitle'>");
        sliderRows.push(this.Name);
        sliderRows.push("</div>");
        sliderRows.push("<div class='description' title='Description: " + this.LongDescription + "'>");
        //sliderRows.push("<div class='description'>");
        sliderRows.push(this.LongDescription);
        sliderRows.push("</div>");
        sliderRows.push("</div>");
        sliderRows.push("<div class='bellow'>");
        //sliderRows.push("<a title='View Settings' href='#' class='SliderViewSettings'><img src='Images/ViewSettings.jpg' width='81' height='31' alt='View Settings' border='0' style='float: left; padding-left: 2px; padding-top: 5px;' /></a>");
        //sliderRows.push("<a title='add to My Gallery' href='#' class='SliderAddToGallery'><img src='Images/AddToGallery.jpg' width='102' height='30' alt='Add To My Gallery' border='0' style='float: right; padding-right: 2px; padding-top: 6px;' /></a>");
        sliderRows.push("</div>");
        sliderRows.push("</div>");
        sliderRows.push("</div>");

    });

    sliderContent = sliderRows.join("");

    ShowSlideShowDialog(selectedObjectId, selectedObjectIndexId, sliderContent, queryReportIds.length, queryReportIds);

    EllipsisToolTipOnSlider();
}

// ellipsis/tool tip /////////////////////////////////////////////////////////////////////////////////////////////////////////////

function EllipsisToolTip() {

    // ellipsis
    $('.t10:not(.tooltip-temp)').dotdotdot({
        ellipsis: ' ..........'
    });

    //$('.t10').find(":contains('...'):").css({ "font-style": "italic", "font-weight": "bolder" });
    //$('.t10:contains("...")').removeAttr("title")

    // tool tip - append
    var ellipsisShortDescriptionElements = $('.t10:contains("..........")');

    for (var i = 0; i < ellipsisShortDescriptionElements.length; i++) {

        var shortDescription = ellipsisShortDescriptionElements[i].innerHTML;
        var longDescription = ellipsisShortDescriptionElements[i].title;

        shortDescription = shortDescription.replace('..........', '...<div title="' + longDescription + '" class="ui-icon ui-icon-newwin vtip" style="float: right; margin-right: 8px; border: 0px solid black;"></div>');

        ellipsisShortDescriptionElements[i].innerHTML = shortDescription;
    }

    // add new class to make it unique
    $('.t10').addClass('tooltip-temp');

    // remove rest of the titles from the divs
    $('.t10').removeAttr("title");

    // call tool tip functionality
    vtip();
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

// get row count /////////////////////////////////////////////////////////////////////////////////////////////////////////////

function GetRowCount() {
    //debugger;

    var topHeight = 113;
    var elementHeight = 210;

    var windowtHeight = $(window).height();
    var documentHeight = $(document).height();
    var bellowHeight = windowtHeight - topHeight;

    recordRowCount = parseInt(bellowHeight / elementHeight) + 1;
}

// window resize event /////////////////////////////////////////////////////////////////////////////////////////////////////////////

function WindowReSize() {
    //debugger;

    // positioning log-in image
    var position = $("#btnSearch").offset();
    var windowWidth = $(window).width();

    $("#message_bar").css("left", windowWidth / 2 - 83);
    $("#message_bar").css("top", position.top - 70);

}

// window width re-sizing /////////////////////////////////////////////////////////////////////////////////////////////////////////////

function WindowWidthReSize() {
    //debugger;

    // tool bar re-size
    var m4Width = $('.m4').width();
    var m4Height = $('.m4').height();

    var percentage;

    // set default values //////////////////////////////////////////////////
    // sbHolder
    $('.sbHolder').css({ 'width': 170 });
    $('.sbSelector').css({ 'width': 170 });
    $('.sbHolder').css({ 'font-size': 12 });

    // t18
    $('.t18').css({ 'width': 185 });

    // t16
    $('.t16').css({ 'font-size': 13 });
    $('.t16').css({ 'padding-right': 4 });
    $('.t16').css({ 'padding-left': 0 });

    // t3
    $('.t3').css({ 'padding-left': 30 });

    // t4
    $('.t4').css({ 'width': 160 });
    $('.t4').css({ 'font-size': 12 });
    $('.t4').css({ 'padding-left': 10 });

    // t19
    $('.t19').css({ 'padding-left': 10 });
    $('.t19').css({ 'width': 41 });

    // re-set values to the percentage /////////////////////////////////////
    if (m4Width < 900) {

        percentage = m4Width / 900;

        var classValue;
        var newClassValue;

        // sbHolder
        classValue = parseInt($('.sbHolder').css('width'));
        newClassValue = parseInt(classValue * percentage);
        $('.sbHolder').css({ 'width': newClassValue });
        $('.sbSelector').css({ 'width': newClassValue });

        classValue = parseInt($('.sbHolder').css('font-size'));
        newClassValue = parseInt(classValue * percentage);
        $('.sbHolder').css({ 'font-size': newClassValue });

        // t18
        classValue = parseInt($('.t18').css('width'));
        newClassValue = parseInt(classValue * percentage);
        $('.t18').css({ 'width': newClassValue });

        // t16
        classValue = parseInt($('.t16').css('font-size'));
        newClassValue = parseInt(classValue * percentage);
        $('.t16').css({ 'font-size': newClassValue });

        classValue = parseInt($('.t16').css('padding-right'));
        newClassValue = parseInt(classValue * percentage);
        $('.t16').css({ 'padding-right': newClassValue });

        classValue = parseInt($('.t16').css('padding-left'));
        newClassValue = parseInt(classValue * percentage);
        $('.t16').css({ 'padding-left': newClassValue });

        // t3
        classValue = parseInt($('.t3').css('padding-left'));
        newClassValue = parseInt(classValue * percentage);
        $('.t3').css({ 'padding-left': newClassValue });

        // t4
        classValue = parseInt($('.t4').css('width'));
        newClassValue = parseInt(classValue * percentage);
        $('.t4').css({ 'width': newClassValue });

        classValue = parseInt($('.t4').css('padding-left'));
        newClassValue = parseInt(classValue * percentage);
        $('.t4').css({ 'padding-left': newClassValue });

        classValue = parseInt($('.t4').css('font-size'));
        newClassValue = parseInt(classValue * percentage);
        $('.t4').css({ 'font-size': newClassValue });

        // t19
        classValue = parseInt($('.t19').css('padding-left'));
        newClassValue = parseInt(classValue * percentage);
        $('.t19').css({ 'padding-left': newClassValue / 2 });

        classValue = parseInt($('.t19').css('width'));
        newClassValue = parseInt(classValue * percentage);
        $('.t19').css({ 'width': newClassValue });
    }
}

