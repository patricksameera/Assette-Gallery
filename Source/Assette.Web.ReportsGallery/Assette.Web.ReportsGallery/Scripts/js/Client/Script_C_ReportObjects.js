///////////////////////////////////////////////////////
//********** Assette Gallery       **********//
//********** Assette © 2013                **********//
//////////////////////////////////////////////////////

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
var tableType;
var selectedObjectTableType;
var queryReportTableTypes = [];
var selectedUniqueObjectId;

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
    //debugger;

    // get delay time from web config
    var timeDelay = $("[id$=hdnDelayTime]").val();

    // ajax loading image
    $("#divLoading").fadeIn('medium');
    $("#divLoading").delay(timeDelay).fadeOut('medium');
    $(".r14").text("Loading...");
    $("#divLoading").css("left", "50%");
    $("#divLoading").css("top", "50%");

    // get row count
    GetRowCount();

    // window resize
    //WindowReSize();

    $(window).resize(function () {

        // window resize event
        //WindowReSize();

        // tool bar re-size
        WindowWidthReSize();
    });

    // water mark
    $("#Search").Watermark("SEARCH");

    // jquery select box - set the initial place holder
    $("#Category").selectbox('attach');
    $("#ClientType").selectbox('attach');

    // set selected tab link   
    $('#samples').css('background-color', '#EFEFEF');
    //$('#objects').css('background-color', '#C1D82F');
    $('#templates').css('background-color', '#EFEFEF');
    $('#objects').css('background-color', '#0397D6');
    $('#objects').css('color', '#ffffff');

    // set selected link
    $('#btnAll').css('color', '#C1D82F');
    $('#btnTables').css('color', '#7a7a7a');
    $('#btnCharts').css('color', '#7a7a7a');

    // default filter
    $("#hdnFilterType").val(objectTypeNone);

    /*
    // loading image functionality    
    $("#divLoading").ajaxStart(OnAjaxStart);
    $("#divLoading").ajaxError(OnAjaxError);
    $("#divLoading").ajaxSuccess(OnAjaxSuccess);
    $("#divLoading").ajaxStop(OnAjaxStop);
    $("#divLoading").ajaxComplete(OnAjaxComplete);
    */

    // fill dropdowns - bind values
    GetPAObjeCtategories();
    GetPAObjectClientTypes();

    // set object type
    $("#hdnFilterType").val(objectTypeNone);
    objectType = $("#hdnFilterType").val();

    // get pa-objects
    GetAllPAObjects();

    // scrolling load
    $(window).scroll(function () {
        //debugger;

        if ($(window).scrollTop() == $(document).height() - $(window).height()) {
            //debugger;

            //get the pa-object div count
            var count = $(".r7").length;

            var x = count;
            var y = 5;
            x %= y;

            if ($("#hdnScroll").val() == "1" && x == 0) {

                //debugger;

                // ajax loading image
                $("#divLoading").hide();
                $("#divLoading").fadeIn('medium');
                $("#divLoading").delay(100).fadeOut('medium');
                $(".r14").text("Loading...");
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
            var count = $(".r7").length;

            var x = count;
            var y = 5;
            x %= y;

            if ($("#hdnScroll").val() == "1" && x == 0) {

                //debugger;

                // ajax loading image
                $("#divLoading").hide();
                $("#divLoading").fadeIn('medium');
                $("#divLoading").delay(100).fadeOut('medium');
                $(".r14").text("Loading...");
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

        // get clicked object's table type
        tableType = $(this).attr("TableType");

        // get object details on slider
        GetPAObjectDetails(selectedObjectId, selectedObjectIndexId, tableType);

    });

    // re-set filter
    //$('#btnAll').live("click", function (e) {
    $("#btnAll").click(function () {
        //debugger;

        // set selected link
        $('#btnAll').css('color', '#C1D82F');
        $('#btnTables').css('color', '#7a7a7a');
        $('#btnCharts').css('color', '#7a7a7a');
        $('#btnBoth').css('color', '#7a7a7a');

        // default filter
        $("#hdnFilterType").val(objectTypeNone);

        // re-set fields /////////////////////////////

        // clear search field
        $("#Search").val('');
        $("#Search").Watermark("SEARCH");

        // set text in jquery-select

        // category dropdown
        var dropDownSbId = $("#Category").attr("sb");
        var $SelectedHref = $("#sbSelector_" + dropDownSbId);

        var dropdown = document.getElementById("Category");
        var value = dropdown.options[0].text;

        $SelectedHref.text(value);

        // client type dropdown
        dropDownSbId = $("#ClientType").attr("sb");
        $SelectedHref = $("#sbSelector_" + dropDownSbId);

        dropdown = document.getElementById("ClientType");
        value = dropdown.options[0].text;

        $SelectedHref.text(value);

        // set value in html-select
        $("#Category").val(0);
        $("#ClientType").val(0);

        if ($("#Category").val() > 0) {
            $("#divBothItems").css('visibility', 'visible')
            $('#btnAll').css('color', '#7a7a7a');
            $("#btnBoth").css('color', '#C1D82F');
        }
        else if ($("#Category").val() == 0) {
            $("#divBothItems").css('visibility', 'hidden')
            if ($("#btnBoth").css('color') == 'rgb(193, 216, 47)') {
                $('#btnAll').css('color', '#C1D82F');
                $("#btnBoth").css('color', '#7a7a7a');
            }
        }

        //////////////////////////////////////////////

        pageIndex = 1;

        // filter data
        FilterData();
    });

    // filter by table
    //$('#btnTables').live("click", function (e) {
    $("#btnTables").click(function () {
        //debugger;

        // set selected link
        $('#btnAll').css('color', '#7a7a7a');
        $('#btnTables').css('color', '#C1D82F');
        $('#btnCharts').css('color', '#7a7a7a');
        $('#btnBoth').css('color', '#7a7a7a');

        // set filter
        $("#hdnFilterType").val(objectTypeTable);

        //////////////////////////////////////////////

        pageIndex = 1;

        // filter data
        FilterData();
    });

    // filter by charts
    //$('#btnCharts').live("click", function (e) {
    $("#btnCharts").click(function () {
        //debugger;

        // set selected link
        $('#btnAll').css('color', '#7a7a7a');
        $('#btnTables').css('color', '#7a7a7a');
        $('#btnCharts').css('color', '#C1D82F');
        $('#btnBoth').css('color', '#7a7a7a');

        // set filter
        $("#hdnFilterType").val(objectTypeChart);

        //////////////////////////////////////////////

        pageIndex = 1;

        // filter data
        FilterData();
    });

    //ADD: Binuka Ranasinghe (25/2/2013)
    // filter by both table andcharts
    //$('#btnCharts').live("click", function (e) {
    $("#btnBoth").click(function () {
        //debugger;

        // set selected link
        $('#btnAll').css('color', '#7a7a7a');
        $('#btnTables').css('color', '#7a7a7a');
        $('#btnCharts').css('color', '#7a7a7a');
        $('#btnBoth').css('color', '#C1D82F');

        // set filter
        $("#hdnFilterType").val(objectTypeNone);

        //////////////////////////////////////////////

        pageIndex = 1;

        // filter data
        FilterData();
    });


    // add to my gallery button click
    $('.AddToMyGallery').live("click", function (e) {
        //debugger;

        // ajax loading image
        $('#divLoading').css('position', 'fixed');
        $("#divLoading").fadeIn('medium');
        $("#divLoading").delay(100).fadeOut('medium');
        $(".r14").text("Adding...");
        $("#divLoading").css("left", "50%");
        $("#divLoading").css("top", "50%");

        // stop postback
        e.preventDefault();

        // get clicked object's id
        selectedObjectId = $(this).attr("objectId");

        // get clicked object's table type
        selectedObjectTableType = $(this).attr("TableType");

        // get clicked object's unique id
        selectedUniqueObjectId = $(this).attr("uniqueObjectId");

        // add to my gallery
        AddToMyGallery(selectedObjectId, selectedObjectTableType);

        // positioning message bar
        var windowHeight = $(window).height();
        var windowWidth = $(window).width();
        $('#message_bar').css('position', 'absolute');
        $("#message_bar").css("left", windowWidth / 2 - 73);
        $("#message_bar").css("top", 70);

        // place message bar
        //WindowReSize();

        // setting the image - on library
        $('#imgAddRemove' + selectedUniqueObjectId).attr({ src: 'Images/AddedRemoveFromGallery.png', id: 'imgAddRemove' + selectedUniqueObjectId, width: '24', height: '16', border: '0', style: 'float: right; padding-right: 5px; padding-top: 5px;' });
        $('#imgAddRemove' + selectedUniqueObjectId).parent().attr({ 'class': 'RemoveFromMyGallery', title: 'Added to My Gallery. Click to remove.' });

        // updating json data
        EditJsonValue("ID", selectedUniqueObjectId, "AddedToMyGallery", "1");
    });

    // add to my gallery button click
    $('.AddToMyGalleryOnDialog').live("click", function (e) {
        //debugger;

        // ajax loading image
        $('#divLoading').css('position', 'fixed');
        $("#divLoading").fadeIn('medium');
        $("#divLoading").delay(100).fadeOut('medium');
        $(".r14").text("Adding...");
        $("#divLoading").css("left", "50%");
        $("#divLoading").css("top", "50%");
        $('#divLoading').css('z-index', 3000);

        // stop postback
        e.preventDefault();

        // get clicked object's id
        selectedObjectId = $(this).attr("objectId");

        // get clicked object's table type
        selectedObjectTableType = $(this).attr("TableType");

        // get clicked object's unique id
        selectedUniqueObjectId = $(this).attr("uniqueObjectId");

        // add to my gallery
        AddToMyGallery(selectedObjectId, selectedObjectTableType);

        // positioning message bar
        var windowHeight = $(window).height();
        var windowWidth = $(window).width();
        $('#message_bar').css('position', 'absolute');
        $("#message_bar").css("left", windowWidth / 2 - 173);
        $("#message_bar").css("top", -10);
        $('#message_bar').css('z-index', 3000);

        // setting the image - on dialog
        $('#imgAddRemoveOnDialog' + selectedUniqueObjectId).attr({ src: 'Images/RemoveFromMyGallery.png', id: 'imgAddRemoveOnDialog' + selectedUniqueObjectId, width: '102', height: '30', border: '0', style: 'float: right; padding-right: 2px; padding-top: 6px;' });
        $('#imgAddRemoveOnDialog' + selectedUniqueObjectId).parent().attr({ 'class': 'RemoveFromMyGalleryOnDialog', title: 'Added to My Gallery. Click to remove.' });

        // setting the image - on library
        $('#imgAddRemove' + selectedUniqueObjectId).attr({ src: 'Images/AddedRemoveFromGallery.png', id: 'imgAddRemove' + selectedUniqueObjectId, width: '24', height: '16', border: '0', style: 'float: right; padding-right: 5px; padding-top: 5px;' });
        $('#imgAddRemove' + selectedUniqueObjectId).parent().attr({ 'class': 'RemoveFromMyGallery', title: 'Added to My Gallery. Click to remove.' });

        // updating json data
        EditJsonValue("ID", selectedUniqueObjectId, "AddedToMyGallery", "1");

    });

    // remove from  my gallery button click
    $('.RemoveFromMyGallery').live("click", function (e) {
        //debugger;

        // ajax loading image
        $('#divLoading').css('position', 'fixed');
        $("#divLoading").fadeIn('medium');
        $("#divLoading").delay(100).fadeOut('medium');
        $(".r14").text("Removing...");
        $("#divLoading").css("left", "50%");
        $("#divLoading").css("top", "50%");

        // stop postback
        e.preventDefault();

        // get clicked object's id
        selectedObjectId = $(this).attr("objectId");

        // get clicked object's table type
        selectedObjectTableType = $(this).attr("TableType");

        // get clicked object's unique id
        selectedUniqueObjectId = $(this).attr("uniqueObjectId");

        // remove from gallery
        RemoveFromMyGallery(selectedObjectId, selectedObjectTableType);

        // positioning message bar
        var windowHeight = $(window).height();
        var windowWidth = $(window).width();
        $('#message_bar').css('position', 'absolute');
        $("#message_bar").css("left", windowWidth / 2 - 73);
        $("#message_bar").css("top", 70);

        // place message bar
        //WindowReSize();

        // setting the image - on library
        $('#imgAddRemove' + selectedUniqueObjectId).attr({ 'src': 'Images/AddToLibrary.jpg', 'id': 'imgAddRemove' + selectedUniqueObjectId, 'width': '28', 'height': '26', 'border': '0', 'style': 'float: right; padding-right: 2px;' });
        $('#imgAddRemove' + selectedUniqueObjectId).parent().attr({ 'class': 'AddToMyGallery', title: 'Add to My Gallery' });

        // updating json data
        EditJsonValue("ID", selectedUniqueObjectId, "AddedToMyGallery", "0");
    });

    // remove from my gallery button click
    $('.RemoveFromMyGalleryOnDialog').live("click", function (e) {
        //debugger;

        // ajax loading image
        $('#divLoading').css('position', 'fixed');
        $("#divLoading").fadeIn('medium');
        $("#divLoading").delay(100).fadeOut('medium');
        $(".r14").text("Removing...");
        $("#divLoading").css("left", "50%");
        $("#divLoading").css("top", "50%");
        $('#divLoading').css('z-index', 3000);

        // stop postback
        e.preventDefault();

        // get clicked object's id
        selectedObjectId = $(this).attr("objectId");

        // get clicked object's table type
        selectedObjectTableType = $(this).attr("TableType");

        // get clicked object's unique id
        selectedUniqueObjectId = $(this).attr("uniqueObjectId");

        // remove from gallery
        RemoveFromMyGallery(selectedObjectId, selectedObjectTableType);

        // positioning message bar
        var windowHeight = $(window).height();
        var windowWidth = $(window).width();
        $('#message_bar').css('position', 'absolute');
        $("#message_bar").css("left", windowWidth / 2 - 173);
        $("#message_bar").css("top", -10);
        $('#message_bar').css('z-index', 3000);

        // setting the image - on library
        $('#imgAddRemove' + selectedUniqueObjectId).attr({ 'src': 'Images/AddToLibrary.jpg', 'id': 'imgAddRemove' + selectedUniqueObjectId, 'width': '28', 'height': '26', 'border': '0', 'style': 'float: right; padding-right: 2px;' });
        $('#imgAddRemove' + selectedUniqueObjectId).parent().attr({ 'class': 'AddToMyGallery', title: 'Add to My Gallery' });

        // setting the image - on dialog
        $('#imgAddRemoveOnDialog' + selectedUniqueObjectId).attr({ 'src': 'Images/AddToGallery.jpg', 'id': 'imgAddRemoveOnDialog' + selectedUniqueObjectId, 'width': '102', 'height': '30', 'alt': 'Add To My Gallery', 'border': '0', 'style': 'float: right; padding-right: 2px; padding-top: 6px;' });
        $('#imgAddRemoveOnDialog' + selectedUniqueObjectId).parent().attr({ 'class': 'AddToMyGalleryOnDialog', title: 'Add to My Gallery' });

        // updating json data
        EditJsonValue("ID", selectedUniqueObjectId, "AddedToMyGallery", "0");
    });

})

// get pa-object details /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function GetAllPAObjects() {
    //debugger;

    $.ajax({
        type: "POST",
        url: "report-objects.aspx/GetAllPAObjects",
        data: "",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: true,
        success: OnGetAllPAObjectsSuccess,
        error: OnGetAllPAObjectsError
    });
}

function OnGetAllPAObjectsError(request, status, error) {
    //debugger;

    // browser loggin
    fireBugConsoleLog(request, status, error);

    // show error to user
    showErrorToUser("");

    // log4net logging
    var errorText = "OnGetAllPAObjectsError() jQuery Exception: response text= " + request.responseText + ", status= " + status + ", error= " + error;
    ClientSideLogging(errorText);
}

function OnGetAllPAObjectsSuccess(response) {
    //debugger;

    // re-set
    var allReportIds = [];

    // assigning
    allObjectsJsonData = $.parseJSON(response.d);


    // get all report id's
    $.each(allObjectsJsonData, function () {
        //debugger;

        // add all report id's to array
        allReportIds.push(this.ID);

        // add query report id's to array - only on initial load, after that through filter
        queryReportIds.push(this.ID);

        // add query report table types
        queryReportTableTypes.push(this.ObjectTableType);

    });

    // get report count
    var count = allReportIds.length;

    // show data
    ShowDataOnATable(allObjectsJsonData);
}

// filter data /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function FilterData() {
    //debugger;

    //example
    /*
    "ID":64,
    "Name":"[Group]",
    "ClientType":"Easy",
    "CategoryId":1,
    "Category":"Asset Allocation",
    "TypeId":2,
    "Type":"Chart",
    "AddedToMyGallery":"1"
    */

    // re-set
    idx = 0;
    queryReportIds = [];
    queryReportTableTypes = [];

    // get data
    var categoryId = $("#Category").val();
    var clientTypeId = $("#ClientType").val();
    var searchText = $("#Search").val();

    if (categoryId == null) categoryId = 0;
    if (clientTypeId == null) clientTypeId = 0;
    if (searchText == "SEARCH") searchText = "";

    var data = allObjectsJsonData;
    var rowCount = data.length;

    // filter by Type ///////////////////////////////////////////////////////////////////////////////////////////////////////

    objectType = $("#hdnFilterType").val();

    if (objectType == "1") {
        data = jQuery.grep(data, function (element, index) {
            return element.TypeId == "1"; // table
        });
    }
    else if (objectType == "2") {
        data = jQuery.grep(data, function (element, index) {
            return element.TypeId == "2"; // chart
        });
    }
    else {
        // 0: all
    }

    rowCount = data.length;

    // filter by ClientType ///////////////////////////////////////////////////////////////////////////////////////////////////////
    if (clientTypeId != "0" && clientTypeId != "Standard") {
        data = jQuery.grep(data, function (element, index) {
            return element.ClientType.toLowerCase() == clientTypeId.toLowerCase();
        });
    }

    rowCount = data.length;

    // filter by Category ///////////////////////////////////////////////////////////////////////////////////////////////////////

    if (categoryId != "0") {
        data = jQuery.grep(data, function (element, index) {
            return element.CategoryId == categoryId;
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

        // add query report table types to array
        queryReportTableTypes.push(this.ObjectTableType);

    });

    // get report count
    var count = queryReportIds.length;

    // pass data to table for showing ///////////////////////////////////////////////////////////////////////////////////////////////////////
    ShowDataOnATable(data);
}

// show data on a table /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function ShowDataOnATable(data) {
    //debugger;

    // re-set
    var galleryRows = [];
    sliderContent = "";
    idx = 0;

    // clear pdf-export fields
    var commaSeperatedPaObjectIds = '';
    var commaSeperatedhdnMockupObjectIds = '';
    $("#hdnPaObjectIds").val('');
    $("#hdnMockupObjectIds").val('');

    var loopBreakRecordCount = parseInt(recordRowCount) * parseInt(pageIndex) * 5; // 5: no of records per row

    // clear contents
    $("#divObjects").empty();
    $("#divTopLink").empty();

    // get paged data
    $.each(data, function () {
        //debugger;

        // get date
        var jsonDate = new Date(parseInt(this.ModifiedDate.substr(6)));
        var d = jsonDate.getDate();
        var m = jsonDate.getMonth() + 1;
        var y = jsonDate.getFullYear();
        var hh = jsonDate.getHours();
        var mm = jsonDate.getMinutes();
        var ss = jsonDate.getSeconds();
        var ModifiedDate = y + "" + (m <= 9 ? '0' + m : m) + "" + (d <= 9 ? '0' + d : d) + "" + (hh <= 9 ? '0' + hh : hh) + "" + (mm <= 9 ? '0' + mm : mm) + "" + (ss <= 9 ? '0' + ss : ss);

        // increment the index
        idx++;
        //uniqueIndex++;

        // gallery
        galleryRows.push("<div class='r7'>");
        galleryRows.push("<div class='r9' title='Click to view report object'>");

        //galleryRows.push("<a href='#' class='ViewLibrary' Index='" + idx + "' Id='" + this.ID + "'><img class='urlImageSmall' src='ImageHandler.ashx?id=" + this.ID + "&type=r_t&date=" + ModifiedDate + "&uniqueindex=" + uniqueIndex + "' width='140' height='90'  border='0' style='float: left; padding:2px;' /></a>");

        if (this.ObjectTableType == 'p') {
            galleryRows.push("<a href='#' class='ViewLibrary' TableType='" + this.ObjectTableType + "' Index='" + idx + "' Id='" + this.ID + "'><img class='urlImageSmall' src='ImageHandler.ashx?id=" + this.ID + "&type=r_t&date=" + ModifiedDate + "&uniqueindex=" + uniqueIndex + "' width='140' height='90'  border='0' style='float: left;padding:2px;' /></a>");

            // add report id's to var field
            if (commaSeperatedPaObjectIds == '') {
                commaSeperatedPaObjectIds = this.ID;
            }
            else {
                commaSeperatedPaObjectIds = commaSeperatedPaObjectIds + "," + this.ID;
            }
        }
        else {
            galleryRows.push("<a href='#' class='ViewLibrary' TableType='" + this.ObjectTableType + "' Index='" + idx + "' Id='" + this.ID + "'><img class='urlImageSmall' src='ImageHandler.ashx?id=" + this.ID + "&type=m_t&date=" + ModifiedDate + "&uniqueindex=" + uniqueIndex + "' width='140' height='90'  border='0' style='float: left;padding:2px;' /></a>");

            // add report id's to var field
            if (commaSeperatedhdnMockupObjectIds == '') {
                commaSeperatedhdnMockupObjectIds = this.ID;
            }
            else {
                commaSeperatedhdnMockupObjectIds = commaSeperatedhdnMockupObjectIds + "," + this.ID;
            }
        }

        galleryRows.push("</div>");
        galleryRows.push("<div class='r10'>");
        galleryRows.push("<b>" + this.Name + "</b>");
        galleryRows.push("<br />");
        galleryRows.push(this.Category);
        galleryRows.push("<br />");
        //galleryRows.push(this.ID + "-" + this.ClientType + "-" + this.Type);
        galleryRows.push("</div>");
        galleryRows.push("<div class='r11'>");
        //galleryRows.push("<a title='View Settings' href='#' class='ViewSettings'><img src='Images/Settings.jpg' width='25' height='21' alt='View Settings' border='0' style='float: left; padding-left: 2px; padding-top: 2px;' /></a>");

        var uniqueObjectId = this.ID + "_" + this.ObjectTableType;

        if (this.AddedToMyGallery == '0') {
            galleryRows.push("<a uniqueObjectId='" + uniqueObjectId + "' objectId='" + this.ID + "' TableType='" + this.ObjectTableType + "' title='Add to my gallery' href='#' class='AddToMyGallery'><img id='imgAddRemove" + uniqueObjectId + "' src='Images/AddToLibrary.jpg' width='28' height='26' alt='Add To My Gallery' border='0' style='float: right; padding-right: 2px;' /></a>");
        }
        else {
            galleryRows.push("<a uniqueObjectId='" + uniqueObjectId + "' objectId='" + this.ID + "' TableType='" + this.ObjectTableType + "' title='Added to my gallery. Click to remove' href='#' class='RemoveFromMyGallery'><img id='imgAddRemove" + uniqueObjectId + "' src='Images/AddedRemoveFromGallery.png' width='24' height='16' alt='Add To My Gallery' border='0' style='float: right; padding-right: 5px; padding-top: 5px;' /></a>");
        }

        galleryRows.push("</div>");
        galleryRows.push("<div class='r12'>");
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

    // setting var field to hidden field
    $("[id$=hdnPaObjectIds]").val(commaSeperatedPaObjectIds);
    $("[id$=hdnMockupObjectIds]").val(commaSeperatedhdnMockupObjectIds);

    if (idx < loopBreakRecordCount) {
        $("#hdnScroll").val("0"); // no more records for loading through scrolling
    }

    if (idx == 0) {
        galleryRows.push("<div class='r17'>");
        galleryRows.push("No matching data found.");
        galleryRows.push("</div>");

        $("#divObjects").html(galleryRows.join(""));
    }
    else {

        $("#divObjects").append(galleryRows.join(""));
    }

    // resize for window width
    WindowWidthReSize();
}

// loading screen functionality /////////////////////////////////////////////////////////////////////////////////////////////////////////////

function OnAjaxStart() {
    //debugger;

    $("#divLoading").css("left", "50%");
    $("#divLoading").css("top", "50%");

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

// get pa-object types details /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function GetPAObjectClientTypes() {
    //debugger;

    $.ajax({
        type: "POST",
        url: "report-objects.aspx/GetPAObjectClientTypes",
        data: "",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: true,
        success: OnGetPAObjectClientTypesSuccess,
        error: OnGetPAObjectClientTypesError
    });
}

function OnGetPAObjectClientTypesError(request, status, error) {
    //debugger;

    // browser loggin
    fireBugConsoleLog(request, status, error);

    // show error to user
    showErrorToUser("");

    // log4net logging
    var errorText = "OnGetPAObjectClientTypesError() jQuery Exception: response text= " + request.responseText + ", status= " + status + ", error= " + error;
    ClientSideLogging(errorText);
}

function OnGetPAObjectClientTypesSuccess(response) {
    //debugger;

    var dropDown = $("#ClientType");
    bindDataToDropDown(response, dropDown, "PRODUCT - ALL");
}

// get pa-object categories details /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function GetPAObjeCtategories() {
    //debugger;

    $.ajax({
        type: "POST",
        url: "report-objects.aspx/GetPAObjeCtategories",
        data: "",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: true,
        success: OnGetPAObjeCtategoriesSuccess,
        error: OnGetPAObjeCtategoriesError
    });
}

function OnGetPAObjeCtategoriesError(request, status, error) {
    //debugger;

    // browser loggin
    fireBugConsoleLog(request, status, error);

    // show error to user
    showErrorToUser("");

    // log4net logging
    var errorText = "OnGetPAObjeCtategoriesError() jQuery Exception: response text= " + request.responseText + ", status= " + status + ", error= " + error;
    ClientSideLogging(errorText);
}

function OnGetPAObjeCtategoriesSuccess(response) {
    //debugger;

    var dropDown = $("#Category");

    bindDataToDropDown(response, dropDown, "CATEGORY - ALL");
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
            if (this.ToolTip != null) {

                dropdown.append('<option value=' + this.ID + ' title="' + this.ToolTip + '">' + this.Type + '</option>');
            }
            else {
                dropdown.append('<option value=' + this.ID + '>' + this.Type + '</option>');
            }

        });

        // jquery select box - set
        //dropdown.selectbox();

        // jquery select box - set
        //        dropdown.selectbox({
        //            onChange: function (val, inst) {
        //                debugger;
        //                pageIndex = 1;
        //            }
        //        });

        $('#Category').selectbox({
            onChange: function (val, inst) {
                //debugger;

                // set selected link
                //  $('#btnAll').css('color', '#999999');//MOD:Binuka Ranasinghe (25/2/2013)

                //MOD:Binuka Ranasinghe (25/2/2013)
                if ($("#Category").val() > 0) {

                    $("#divBothItems").css('visibility', 'visible');

                    if ($('#btnAll').css('color') == 'rgb(193, 216, 47)') {

                        $('#btnAll').css('color', '#7a7a7a');
                        $("#btnBoth").css('color', '#C1D82F');
                    }

                }

                else if ($("#Category").val() == 0) {
                    $("#divBothItems").css('visibility', 'hidden')
                    if ($("#btnBoth").css('color') == 'rgb(193, 216, 47)') {
                        $('#btnAll').css('color', '#C1D82F');
                        $("#btnBoth").css('color', '#7a7a7a');
                    }

                }

                //
                //////////////////////////////////////////////

                pageIndex = 1;

                // filter data
                FilterData();
            }
        });

        $('#ClientType').selectbox({
            onChange: function (val, inst) {
                //debugger;

                // set selected link
                // $('#btnAll').css('color', '#999999');  //MOD:Binuka Ranasinghe (25/2/2013)

                //////////////////////////////////////////////

                pageIndex = 1;

                // filter data
                FilterData();
            }
        });
    }
}

// get report object details /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function GetPAObjectDetails(objectId, indexId, tableType) {
    //debugger;

    selectedObjectIndexId = indexId;
    selectedObjectId = objectId;
    selectedObjectTableType = tableType;

    var sliderRows = [];

    // get slider content
    var data = jQuery.grep(allObjectsJsonData, function (element, index) {
        return element.ID == objectId && element.ObjectTableType == tableType;
    });

    //debugger;

    // setting modal dialog image width
    var dimensionsObject = GetDialogDimensionsForReportObjects();

    sliderImageWidth = dimensionsObject.sliderImageWidth;
    sliderImageHeight = dimensionsObject.sliderImageHeight;

    //debugger;

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
        sliderRows.push("<div id='lefter' class='lefter'>");

        //sliderRows.push("<img class='urlImageLarge' src='ImageHandler.ashx?id=" + this.ID + "&type=r_i&date=" + ModifiedDate + "&uniqueindex=" + uniqueIndex + "' border='1' style='float: left; padding: 10px;' width='" + sliderImageWidth + "' height='" + sliderImageHeight + "' border='0'  />");

        sliderRows.push("<div class='LoadingDiv'  style='width:" + (sliderImageWidth + 10) + "px; height:" + (sliderImageHeight + 10) + "px;'>");

        if (this.ObjectTableType == 'p') {
            sliderRows.push("<img onload='SetImageSize()' class='urlImageLarge' src='ImageHandler.ashx?id=" + this.ID + "&type=r_i&date=" + ModifiedDate + "&uniqueindex=" + uniqueIndex + "' style='float: left; padding: 5px;opacity: 0;' border='0'  />");
        }
        else {
            sliderRows.push("<img onload='SetImageSize()' class='urlImageLarge' src='ImageHandler.ashx?id=" + this.ID + "&type=m_i&date=" + ModifiedDate + "&uniqueindex=" + uniqueIndex + "' style='float: left; padding: 5px;opacity: 0;' border='0'  />");
        }

        /*
        if (this.ObjectTableType == 'p') {
        //sliderRows.push("<div id='urlImageLargeTempDiv' style='visibility: hidden;'><img class='urlImageLargeTemp' src='ImageHandler.ashx?id=" + this.ID + "&type=r_i&date=" + ModifiedDate + "&uniqueindex=" + uniqueIndex + "' /></div>");
        sliderRows.push("<img class='urlImageLarge' src='ImageHandler.ashx?id=" + this.ID + "&type=r_i&date=" + ModifiedDate + "&uniqueindex=" + uniqueIndex + " border='0' style='float: left; padding: 10px;' border='0'  />");
        }
        else {
        //sliderRows.push("<div id='urlImageLargeTempDiv' style='visibility: hidden;'><img class='urlImageLargeTemp' src='ImageHandler.ashx?id=" + this.ID + "&type=m_i&date=" + ModifiedDate + "&uniqueindex=" + uniqueIndex + "' /></div>");
        sliderRows.push("<img class='urlImageLarge' src='ImageHandler.ashx?id=" + this.ID + "&type=m_i&date=" + ModifiedDate + "&uniqueindex=" + uniqueIndex + "' border='0' style='float: left; padding: 10px;' border='0'  />");
        }*/

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
        sliderRows.push("<div class='description' title='Description: " + this.Description + "'>");
        sliderRows.push(this.Description);
        sliderRows.push("</div>");
        sliderRows.push("</div>");
        sliderRows.push("<div class='bellow'>");
        //sliderRows.push("<a title='View Settings' href='#' class='SliderViewSettings'><img src='Images/ViewSettings.jpg' width='81' height='31' alt='View Settings' border='0' style='float: left; padding-left: 2px; padding-top: 5px;' /></a>");

        var uniqueObjectId = this.ID + "_" + this.ObjectTableType;

        if (this.AddedToMyGallery == "0") {
            sliderRows.push("<a uniqueObjectId='" + uniqueObjectId + "' objectId='" + this.ID + "' TableType='" + this.ObjectTableType + "' title='Add to my gallery' href='#' class='AddToMyGalleryOnDialog'><img id='imgAddRemoveOnDialog" + uniqueObjectId + "' src='Images/AddToGallery.jpg' width='102' height='30' alt='Add To My Gallery' border='0' style='float: right; padding-right: 2px; padding-top: 6px;' /></a>");
        }
        else {
            sliderRows.push("<a uniqueObjectId='" + uniqueObjectId + "' objectId='" + this.ID + "' TableType='" + this.ObjectTableType + "' title='Added to my gallery. Click to remove' href='#' class='RemoveFromMyGalleryOnDialog'><img id='imgAddRemoveOnDialog" + uniqueObjectId + "' src='Images/RemoveFromMyGallery.png' width='102' height='30' alt='Click to remove from my gallery' border='0' style='float: right; padding-right: 2px; padding-top: 6px;' /></a>");
        }

        if (this.ClientType != 'Easy') {
            sliderRows.push("</br></br></br>");
            sliderRows.push("<div style='border: 0px solid #000000; margin-top: 0px; float: right;'>");

            sliderRows.push("<div style='float: left;'>");
            sliderRows.push("<img src='Images/Easy.jpg' style='margin-top: .0em; margin-right: .3em;' />");
            sliderRows.push("</div>");

            sliderRows.push("<div class='clienttype' style='float: left; padding-top: 3px;'>");
            sliderRows.push("Not available in Easy Editions");
            sliderRows.push("</div>");

            sliderRows.push("</div>");
        }

        sliderRows.push("</div>");
        sliderRows.push("</div>");
        sliderRows.push("</div>");

    });

    sliderContent = sliderRows.join("");

    ShowSlideShowDialog(selectedObjectId, selectedObjectIndexId, sliderContent, queryReportIds.length, queryReportIds, queryReportTableTypes);

    EllipsisToolTipOnSlider();

}

// add object to my gallery /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function AddToMyGallery(objectId, tableType) {
    //debugger;

    $.ajax({
        type: "POST",
        url: "report-objects.aspx/AddToMyGallery",
        data: JSON.stringify({ id: objectId, type: tableType }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: true,
        success: OnAddToMyGallerySuccess,
        error: OnAddToMyGalleryError
    });

}

function OnAddToMyGalleryError(request, status, error) {
    //debugger;

    // browser loggin
    fireBugConsoleLog(request, status, error);

    // show error to user
    showErrorToUser("");

    // log4net logging
    var errorText = "OnAddToMyGalleryError() jQuery Exception: response text= " + request.responseText + ", status= " + status + ", error= " + error;
    ClientSideLogging(errorText);
}

function OnAddToMyGallerySuccess(response) {
    //debugger;

    var rtnVal = response.d;

    // hide message bar
    $('#message_bar').hide();

    if (data == "-1") // error
    {
        // show error to user
        showErrorToUser("");
    }
    else if (rtnVal == "-2") {
        //debugger;

        /*
        var message = "Report object was already added to My Gallery.";

        // show message bar
        $('#message_bar').displayMessage({
        message: message,
        background: '#111111',
        color: '#FFFFFF',
        speed: 'medium',
        skin: 'orange',
        //position: 'fixed',
        position: 'absolute',
        autohide: true
        });
        */
    }
    else {
        //debugger;

        var message = "Report object was successfully added to My Gallery.";

        // show message bar
        $('#message_bar').displayMessage({
            message: message,
            background: '#111111',
            color: '#FFFFFF',
            speed: 'medium',
            skin: 'green',
            position: 'fixed',
            autohide: true
        });
    }
}

// remove from my gallery /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function RemoveFromMyGallery(objectId, tableType) {
    //debugger;

    $.ajax({
        type: "POST",
        url: "report-objects.aspx/RemoveFromMyGallery",
        data: JSON.stringify({ id: objectId, type: tableType }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: true,
        success: OnRemoveFromMyGallerySuccess,
        error: OnRemoveFromMyGalleryError
    });
}

function OnRemoveFromMyGalleryError(request, status, error) {
    //debugger;

    // browser loggin
    fireBugConsoleLog(request, status, error);

    // show error to user
    showErrorToUser("");

    // log4net logging
    var errorText = "OnRemoveFromMyGalleryError() jQuery Exception: response text= " + request.responseText + ", status= " + status + ", error= " + error;
    ClientSideLogging(errorText);
}

function OnRemoveFromMyGallerySuccess(response, e) {
    //debugger;

    var rtnVal = response.d;

    // hide message bar
    $('#message_bar').hide();

    if (data == "-1") // error
    {
        // show error to user
        showErrorToUser("");
    }
    else {

        var message = "Report object was successfully removed from My Gallery.";

        // show message bar
        $('#message_bar').displayMessage({
            message: message,
            background: '#111111',
            color: '#FFFFFF',
            speed: 'medium',
            skin: 'green',
            position: 'fixed',
            autohide: true
        });
    }
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
    // right hand side tool tip
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

// edit json data /////////////////////////////////////////////////////////////////////////////////////////////////////////////

function EditJsonValue(uniqueFieldName, uniqueFieldValue, fieldToChange, newValue) {
    //debugger;

    for (var k = 0; k < allObjectsJsonData.length; ++k) {

        var valueCheck = allObjectsJsonData[k][uniqueFieldName] + '_' + allObjectsJsonData[k]['ObjectTableType'];

        if (uniqueFieldValue == valueCheck) {
            //debugger;

            allObjectsJsonData[k][fieldToChange] = newValue;
        }
    }
    //debugger;
}

// window resize event /////////////////////////////////////////////////////////////////////////////////////////////////////////////

function WindowReSize() {
    //debugger;

    // positioning message bar
    var windowWidth = $(window).width();
    var windowHeight = $(window).height();

    $("#message_bar").css("left", windowWidth / 2 - 72);
    $("#message_bar").css("top", "100");

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

    // r1
    $('.r1').css({ 'font-size': 13 });
    $('.r1').css({ 'padding-right': 4 });
    $('.r1').css({ 'padding-left': 10 });

    // r16
    $('.r16').css({ 'font-size': 13 });
    $('.r16').css({ 'padding-right': 2 });
    $('.r16').css({ 'padding-left': 2 });

    // r4
    $('.r4').css({ 'width': 140 });
    $('.r4').css({ 'font-size': 12 });
    $('.r4').css({ 'padding-left': 10 });

    // r19
    $('.r19').css({ 'width': 41 });
    $('.r19').css({ 'padding-left': 10 });

    // r18
    $('.r18').css({ 'padding-left': 5 });

    // r3
    $('.r3').css({ 'padding-left': 15 });

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

        // r1
        classValue = parseInt($('.r1').css('font-size'));
        newClassValue = parseInt(classValue * percentage);
        $('.r1').css({ 'font-size': newClassValue });

        classValue = parseInt($('.r1').css('padding-right'));
        newClassValue = parseInt(classValue * percentage);
        $('.r1').css({ 'padding-right': newClassValue });

        classValue = parseInt($('.r1').css('padding-left'));
        newClassValue = parseInt(classValue * percentage);
        $('.r1').css({ 'padding-left': newClassValue });

        // r16
        classValue = parseInt($('.r16').css('font-size'));
        newClassValue = parseInt(classValue * percentage);
        $('.r16').css({ 'font-size': newClassValue });

        classValue = parseInt($('.r16').css('padding-right'));
        newClassValue = parseInt(classValue * percentage);
        $('.r16').css({ 'padding-right': newClassValue });

        classValue = parseInt($('.r16').css('padding-left'));
        newClassValue = parseInt(classValue * percentage);
        $('.r16').css({ 'padding-left': newClassValue });

        // r19
        classValue = parseInt($('.r19').css('width'));
        newClassValue = parseInt(classValue * percentage);
        $('.r19').css({ 'width': newClassValue });

        classValue = parseInt($('.r19').css('padding-left'));
        newClassValue = parseInt(classValue * percentage);
        $('.r19').css({ 'padding-left': newClassValue });

        // r4
        classValue = parseInt($('.r4').css('width'));
        newClassValue = parseInt(classValue * percentage);
        $('.r4').css({ 'width': newClassValue });

        classValue = parseInt($('.r4').css('padding-left'));
        newClassValue = parseInt(classValue * percentage);
        $('.r4').css({ 'padding-left': newClassValue });

        classValue = parseInt($('.r14').css('font-size'));
        newClassValue = parseInt(classValue * percentage);
        $('.r4').css({ 'font-size': newClassValue });

        // r18
        classValue = parseInt($('.r18').css('padding-left'));
        newClassValue = parseInt(classValue * percentage);
        $('.r18').css({ 'padding-left': newClassValue });

        // r3
        classValue = parseInt($('.r3').css('padding-left'));
        newClassValue = parseInt(classValue * percentage);
        $('.r3').css({ 'padding-left': newClassValue });
    }
}

// export loading /////////////////////////////////////////////////////////////////////////////////////////////////////////////

function ExportLoading() {
    //debugger;

    if ($("[id$=hdnPaObjectIds]").val() == "" && $("[id$=hdnMockupObjectIds]").val() == "") {

        return false;
    }
    else {
        // get delay time from web config
        var timeDelay = $("[id$=hdnDelayTime]").val();

        // ajax loading image
        $("#divLoading").fadeIn('medium');
        $("#divLoading").delay(timeDelay).fadeOut('medium');
        $(".r14").text("Exporting...");
        $("#divLoading").css("left", "50%");
        $("#divLoading").css("top", "50%");

        return true;
    }
}