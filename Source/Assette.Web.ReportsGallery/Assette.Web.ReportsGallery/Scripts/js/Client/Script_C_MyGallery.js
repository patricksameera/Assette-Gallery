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
var queryReportTableTypes = [];
var galleryContentIds = [];
var allObjectsJsonData;
var uniqueIndex = 0;
var tableType;
var selectedPAObjectTableType;
var pAObjectId;

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
    $(".s14").text("Loading...");
    $("#divLoading").css("left", "50%");
    $("#divLoading").css("top", "50%");

    // get row count
    GetRowCount();

    // window resize
    WindowReSize();

    $(window).resize(function () {

        // window resize event
        WindowReSize();
    });

    // water mark
    $("#Search").Watermark("SEARCH");

    // jquery select box - set the initial place holder
    $("#Category").selectbox('attach');
    $("#ClientType").selectbox('attach');

    // set selected tab link   
    $('#samples').css('background-color', '#EFEFEF');
    $('#objects').css('background-color', '#EFEFEF');
    $('#templates').css('background-color', '#EFEFEF');
    //$('#lnkMyGallery').css('background-color', '#0397D6');
    $('#lnkMyGallery').css('color', '#0397D6');

    /*
    // loading image functionality
    $("#divLoading").ajaxStart(OnAjaxStart);
    $("#divLoading").ajaxError(OnAjaxError);
    $("#divLoading").ajaxSuccess(OnAjaxSuccess);
    $("#divLoading").ajaxStop(OnAjaxStop);
    $("#divLoading").ajaxComplete(OnAjaxComplete);
    */

    // fill dropdowns - bind values
    //GetPAObjectClientTypes();
    //GetPAObjeCtategories();

    // set object type
    objectType = objectTypeNone;

    // get pa-objects
    GetAllMyGalleryPAObjects();

    // scrolling load
    $(window).scroll(function () {
        //debugger;

        if ($(window).scrollTop() == $(document).height() - $(window).height()) {
            //debugger;

            // get the pa-object div count
            var count = $(".r7").length;

            var x = count;
            var y = 5;
            x %= y;

            if ($("#hdnScroll").val() == "1" && x == 0) {

                // ajax loading image
                $("#divLoading").fadeIn('medium');
                $("#divLoading").delay(500).fadeOut('medium');
                $(".s14").text("Loading...");
                $("#divLoading").css("left", "50%");
                $("#divLoading").css("top", "50%");

                //////////////////////////////////////////////

                pageIndex++;

                // filter data
                FilterData();
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

                // ajax loading image
                $("#divLoading").fadeIn('medium');
                $("#divLoading").delay(500).fadeOut('medium');
                $(".s14").text("Loading...");
                $("#divLoading").css("left", "50%");
                $("#divLoading").css("top", "50%");

                //////////////////////////////////////////////

                pageIndex++;

                // filter data
                FilterData();
            }
        }
    });

    // search button click
    $('#btnSearch').live("click", function (e) {
        //debugger;

    });

    // re-set filter
    $('#btnAll').live("click", function (e) {
        //debugger;


    });

    // filter by table
    $('#btnTables').live("click", function (e) {
        //debugger;

    });

    // filter by charts
    $('#btnCharts').live("click", function (e) {
        //debugger;

    });

    // view library button click
    $('.ViewLibrary').live("click", function (e) {
        //debugger;

        // stop postback
        e.preventDefault();

        // get clicked object's id
        pAObjectId = $(this).attr("Id");

        // get clicked object's index id
        indexId = $(this).attr("Index");

        // get gallery content id
        galleryContentId = $(this).attr("GalleryContentId");

        // get clicked object's table type
        tableType = $(this).attr("TableType");

        // get notes
        GetNotes(galleryContentId);

        // get delay time from web config
        var timeDelay = $("[id$=hdnGetNotesDelayTime]").val();

        setTimeout(function () {
            //debugger;

            // get report object
            GetMyGalleryPAObjectDetails(pAObjectId, indexId, galleryContentId, tableType);

        }, timeDelay);

    });

    // remove from gallery button click
    $('.RemoveFromGallery').live("click", function (e) {
        //debugger;

        // stop postback
        e.preventDefault();

        // get clicked object's id
        galleryContentId = $(this).attr("GalleryContentId");

        // get clicked object's table type
        tableType = $(this).attr("TableType");

        // remove from gallery
        RemoveFromMyGallery(galleryContentId, tableType);
    });

    // add notes button click
    $('#btnNotes').live("click", function (e) {
        //debugger;

        // stop postback
        e.preventDefault();

        // show loading image
        $("#AddRemoveLoading").fadeIn('medium');
        $("#AddRemoveLoading").delay(1000).fadeOut('medium');

        // get clicked object's id
        galleryContentId = $(this).attr("GalleryContentId");

        var note = $.trim($("#txtNote").val());

        if (note != '') {

            // add note
            AddNotes(galleryContentId);

            // get delay time from web config
            var timeDelay = $("[id$=hdnGetNotesDelayTime]").val();

            setTimeout(function () {
                //debugger;

                // get notes
                GetNotesAfterAddingOrRemoving(galleryContentId);

                // note exists image
                $('#imgNoteAddRemove' + pAObjectId).attr({ src: 'Images/NoteExists.png', id: 'imgNoteAddRemove' + pAObjectId, width: '16', height: '19', border: '0', style: 'float: right; padding-right: 2px;' });

            }, timeDelay);
        }
    });

    // remove note
    $('.notedelete').live("click", function (e) {
        //debugger;

        // stop postback
        e.preventDefault();

        // show loading image
        $("#AddRemoveLoading").fadeIn('medium');
        $("#AddRemoveLoading").delay(1000).fadeOut('medium');

        // get clicked object's id
        noteId = $(this).attr("noteId");

        // remove note
        RemoveNotes(noteId, $(this));

        // get delay time from web config
        var timeDelay = $("[id$=hdnGetNotesDelayTime]").val();

        setTimeout(function () {
            //debugger;

            // get notes
            GetNotesAfterAddingOrRemoving(galleryContentId);

            // note exists image
            if ($(".note").length == 1) {
                $('#imgNoteAddRemove' + pAObjectId).attr({ src: 'Images/AddNotes.jpg', id: 'imgNoteAddRemove' + pAObjectId, width: '18', height: '22', border: '0', style: 'float: right; padding-right: 2px;' });
            }
            else {
                $('#imgNoteAddRemove' + pAObjectId).attr({ src: 'Images/NoteExists.png', id: 'imgNoteAddRemove' + pAObjectId, width: '16', height: '19', border: '0', style: 'float: right; padding-right: 2px;' });
            }
        }, timeDelay);

    });

    // invite to view gallery
    $('.InviteToViewGallery').live("click", function (e) {
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

        var link = "<a href='" + $("[id$=hdnUrl]").val() + "'>" + $("[id$=hdnUrl]").val() + "</a>";

        var htmlMessage =
        'Hello,<br/><br/>' +
        'I have been browsing the Assette Gallery, and have identified reports that will help us better communicate results to clients.<br/><br/>' +
        'Click on the link below to see the “My Gallery” area, which has the reports I noted.  (You will have to fill out a simple registration form, if you haven’t already registered.)<br/><br/>' +
        link +
        '<br/><br/>' +
        'Not only is Assette going to improve our client communications, but also will save us a lot of time and eliminate errors!<br/><br/>' +
        'Thanks,<br/>' +
        $("[id$=hdnFirstName]").val();

        var plainMessage = htmlMessage.replace(/\<br\/\>/gi, '\n').replace(/(<([^>]+)>)/ig, "");

        $("#EmailFrom").val($("[id$=hdnEmail]").val());
        $("#EmailMessage").val(plainMessage);

        $("#divEmail").slideDown('slow');

        // positioning div
        var position = $(".InviteToViewGallery").offset();

        // email
        $("#divEmail").css("left", position.left - 130);
        $("#divEmail").css("top", position.top + 45);
    });

})

// get pa-object details /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function GetAllMyGalleryPAObjects() {
    //debugger;

    $.ajax({
        type: "POST",
        url: "my-gallery.aspx/GetAllMyGalleryPAObjects",
        data: "",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: true,
        success: OnGetAllMyGalleryPAObjectsSuccess,
        error: OnGetAllMyGalleryPAObjectsError
    });
}

function OnGetAllMyGalleryPAObjectsError(request, status, error) {
    //debugger;

    // browser loggin
    fireBugConsoleLog(request, status, error);

    // show error to user
    showErrorToUser("");

    // log4net logging
    var errorText = "OnGetAllMyGalleryPAObjectsError() jQuery Exception: response text= " + request.responseText + ", status= " + status + ", error= " + error;
    ClientSideLogging(errorText);
}

function OnGetAllMyGalleryPAObjectsSuccess(response) {
    //debugger;

    // re-set
    allReportIds = [];
    queryReportIds = [];
    galleryContentIds = [];
    queryReportTableTypes = [];

    // assigning
    allObjectsJsonData = $.parseJSON(response.d);

    // get all report id's
    $.each(allObjectsJsonData, function () {
        //debugger;

        // add all report id's to array
        allReportIds.push(this.ID);

        // add query report id's to array - only on initial load, after that through filter
        queryReportIds.push(this.ID);

        // add gallery content report id's to array - only on initial load, after that through filter
        galleryContentIds.push(this.GalleryContentId);

        // add gallery content report table types
        queryReportTableTypes.push(this.ObjectTableType);
    });

    //debugger;

    // get count
    var count = allReportIds.length;

    // show data
    ShowDataOnATable(allObjectsJsonData);
}

// filter data /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function FilterData() {
    //debugger;

    //example
    /*
    "ID":50,
    "Name":"Target Allocation",
    "Description":"This object displays ",
    "ClientType":"Easy",
    "CategoryId":1,
    "Category":"Asset Allocation",
    "TypeId":2,
    "Type":"Chart",
    "GalleryContentId":362,
    "UserId":"37ae84db-8eae-4ec7-8574-b16dae88b931"
    */

    // re-set
    idx = 0;
    queryReportIds = [];
    galleryContentIds = [];
    queryReportTableTypes = [];

    var data = allObjectsJsonData;
    var rowCount = data.length;

    // filter data ///////////////////////////////////////////////////////////////////////////////////////////////////////

    // get query report id's ///////////////////////////////////////////////////////////////////////////////////////////////////////
    $.each(data, function () {
        //debugger;

        // add query report id's to array
        queryReportIds.push(this.ID);

        // add all gallery content id's to array
        galleryContentIds.push(this.GalleryContentId);

        // add gallery content report table types
        queryReportTableTypes.push(this.ObjectTableType);
    });

    // get count
    var count = queryReportIds.length;

    // get count
    count = galleryContentIds.length;

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

    var loopBreakRecordCount = parseInt(recordRowCount) * parseInt(pageIndex) * 5; // 5: no of records per row

    // clear contents
    $("#divGalleryObjects").empty();

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
        //galleryRows.push("<a href='#' class='ViewLibrary' GalleryContentId='" + this.GalleryContentId + "' TableType='" + this.ObjectTableType + "' Index='" + idx + "' Id='" + this.ID + "'><img class='urlImageLarge' src='ImageHandler.ashx?id=" + this.ID + "&type=r_t&date=" + ModifiedDate + "&uniqueindex=" + uniqueIndex + "' width='140' height='90'  border='0' style='float: left;padding:2px;' /></a>");

        if (this.ObjectTableType == 'p') {
            galleryRows.push("<a href='#' class='ViewLibrary' GalleryContentId='" + this.GalleryContentId + "' TableType='" + this.ObjectTableType + "' Index='" + idx + "' Id='" + this.ID + "'><img class='urlImageSmall' src='ImageHandler.ashx?id=" + this.ID + "&type=r_t&date=" + ModifiedDate + "&uniqueindex=" + uniqueIndex + "' width='140' height='90'  border='0' style='float: left;padding:2px;' /></a>");
        }
        else {
            galleryRows.push("<a href='#' class='ViewLibrary' GalleryContentId='" + this.GalleryContentId + "' TableType='" + this.ObjectTableType + "' Index='" + idx + "' Id='" + this.ID + "'><img class='urlImageSmall' src='ImageHandler.ashx?id=" + this.ID + "&type=m_t&date=" + ModifiedDate + "&uniqueindex=" + uniqueIndex + "' width='140' height='90'  border='0' style='float: left;padding:2px;' /></a>");
        }

        galleryRows.push("</div>");
        galleryRows.push("<div class='r10'>");
        galleryRows.push("<b>" + this.Name + "</b>");
        galleryRows.push("<br />");
        galleryRows.push(this.Category);
        galleryRows.push("<br />");
        //galleryRows.push(this.ID + "-" + this.GalleryContentId + "-" + this.ClientType + "-" + this.Type);
        galleryRows.push("</div>");
        galleryRows.push("<div class='r11'>");
        //galleryRows.push("<a title='View Settings' href='#' class='ViewSettings'><img src='Images/Settings.jpg' width='25' height='21' alt='No image found.' border='0' style='float: left; padding-left: 2px; padding-top: 2px;' /></a>");
        //galleryRows.push("<a title='click to add notes' href='#' class='ViewLibrary' GalleryContentId='" + this.GalleryContentId + "' Index='" + idx + "' Id='" + this.ID + "'><img src='Images/AddNotes.jpg' width='18' height='22' alt='click to add notes' border='0' style='float: right; padding-right: 2px;' /></a>");

        if (this.NoteCount == '0') {
            galleryRows.push("<a title='Click to add notes' href='#' class='ViewLibrary' GalleryContentId='" + this.GalleryContentId + "' TableType='" + this.ObjectTableType + "' Index='" + idx + "' Id='" + this.ID + "'><img id='imgNoteAddRemove" + this.ID + "' src='Images/AddNotes.jpg' width='18' height='22' alt='click to add notes' border='0' style='float: right; padding-right: 2px;' /></a>");
        }
        else {
            galleryRows.push("<a title='Contains notes. click to add notes' href='#' class='ViewLibrary' GalleryContentId='" + this.GalleryContentId + "' TableType='" + this.ObjectTableType + "' Index='" + idx + "' Id='" + this.ID + "'><img id='imgNoteAddRemove" + this.ID + "' src='Images/NoteExists.png' width='16' height='19' alt='click to add notes' border='0' style='float: right; padding-right: 2px;' /></a>");
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

    if (idx == 0) {
        galleryRows.push("<div class='r17'>");
        galleryRows.push("There are no items in My Gallery. <br />");
        galleryRows.push("</div>");
        galleryRows.push("<div class='r22'>");
        galleryRows.push("When browsing through the chart and table layouts, click on the ");
        //galleryRows.push("<span style='margin-top: 5px;'>");
        galleryRows.push("<img src='Images/AddToMyGalleryText.jpg' width='21' height='15' alt='Add To My Gallery' border='0' style='padding-right: 5px; padding-left: 5px; margin-top: 0px;' />");
        //galleryRows.push("</span>");
        galleryRows.push(" icon to add samples to the My Gallery. You can then review these at your convenience.");
        //galleryRows.push("<span style='color: #C1D82F;'> and even share your own gallery with colleagues.<span>");
        galleryRows.push("</div>");

        $("#divGalleryObjects").html(galleryRows.join(""));
    }
    else {
        $("#divGalleryObjects").append(galleryRows.join(""));
    }
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
        url: "my-gallery.aspx/GetPAObjectClientTypes",
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

    bindDataToDropDown(response, dropDown, "CLIENT TYPE - ALL");
}

// get pa-object categories details /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function GetPAObjeCtategories() {
    //debugger;

    $.ajax({
        type: "POST",
        url: "my-gallery.aspx/GetPAObjeCtategories",
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

    bindDataToDropDown(response, dropDown, "CATEGORY");
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

            dropdown.append('<option value=' + this.ID + '>' + this.Type + '</option>');

        });

        // jquery select box - set
        //dropdown.selectbox();

        // jquery select box - set
        dropdown.selectbox({
            onChange: function (val, inst) {
                pageIndex = 1;
            }
        });
    }
}

// get report object details /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function GetMyGalleryPAObjectDetails(objectId, indexId, galleryContentId, tableType) {
    //debugger;

    selectedPAObjectIndexId = indexId;
    selectedPAObjectId = objectId;
    selectedPAObjectTableType = tableType;

    var sliderRows = [];

    // get slider content
    var data = jQuery.grep(allObjectsJsonData, function (element, index) {
        return element.ID == objectId && element.ObjectTableType == selectedPAObjectTableType;
    });

    // setting modal dialog image width
    var dimensionsObject = GetDialogDimensionsForMyGalleryReportObjects();

    sliderImageWidth = dimensionsObject.sliderImageWidth;
    sliderImageHeight = dimensionsObject.sliderImageHeight;

    /*
    // setting modal dialog image width
    var windowWidth = $(window).width();

    //alert(windowWidth);

    var percentage = 540 / 800; // 786/531 : 0.67

    if (windowWidth >= 1600) {
    //alert('1600');
    sliderImageWidth = 770;
    sliderImageHeight = parseInt(parseFloat(sliderImageWidth) * parseFloat(percentage));
    }
    else if (windowWidth >= 1500 && windowWidth < 1600) {
    //alert('1500/1600');
    sliderImageWidth = 710;
    sliderImageHeight = parseInt(parseFloat(sliderImageWidth) * parseFloat(percentage));
    }
    else if (windowWidth >= 1400 && windowWidth < 1500) {
    //alert('1400/1500');
    sliderImageWidth = 660;
    sliderImageHeight = parseInt(parseFloat(sliderImageWidth) * parseFloat(percentage));
    }
    else if (windowWidth >= 1300 && windowWidth < 1400) {
    //alert('1300/1400');
    sliderImageWidth = 620;
    sliderImageHeight = parseInt(parseFloat(sliderImageWidth) * parseFloat(percentage));
    }
    else if (windowWidth >= 1200 && windowWidth < 1300) {
    //alert('1200/1300');
    sliderImageWidth = 560;
    sliderImageHeight = parseInt(parseFloat(sliderImageWidth) * parseFloat(percentage));
    }
    else if (windowWidth >= 1100 && windowWidth < 1200) {
    //alert('1100/1200');
    sliderImageWidth = 510;
    sliderImageHeight = parseInt(parseFloat(sliderImageWidth) * parseFloat(percentage));
    }
    else if (windowWidth >= 1000 && windowWidth < 1100) {
    //alert('1000/1100');
    sliderImageWidth = 450;
    sliderImageHeight = parseInt(parseFloat(sliderImageWidth) * parseFloat(percentage));
    }
    else if (windowWidth >= 900 && windowWidth < 1000) {
    //alert('900/1000');
    sliderImageWidth = 410;
    sliderImageHeight = parseInt(parseFloat(sliderImageWidth) * parseFloat(percentage));
    }
    else if (windowWidth >= 800 && windowWidth < 900) {
    //alert('800/900');
    sliderImageWidth = 370;
    sliderImageHeight = parseInt(parseFloat(sliderImageWidth) * parseFloat(percentage));
    }
    else if (windowWidth >= 700 && windowWidth < 800) {
    //alert('700/800');
    sliderImageWidth = 320;
    sliderImageHeight = parseInt(parseFloat(sliderImageWidth) * parseFloat(percentage));
    }
    else {
    //alert('other');
    sliderImageWidth = 320;
    sliderImageHeight = parseInt(parseFloat(sliderImageWidth) * parseFloat(percentage));
    }

    //alert(sliderImageWidth);
    //alert(sliderImageHeight);
    */

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
        //sliderRows.push(this.ID);
        sliderRows.push("<div class='lefter'>");

        //sliderRows.push("<img class='urlImageLarge' src='ImageHandler.ashx?id=" + this.ID + "&type=r_i' width='796' height='540'  border='1' style='float: left; padding-left: 2px; padding-right: 2px; padding-top: 5px;' />");

        //sliderRows.push("<img class='urlImageLarge' src='ImageHandler.ashx?id=" + this.ID + "&type=r_i&date=" + ModifiedDate + "&uniqueindex=" + uniqueIndex + "' border='1' style='float: left; padding: 10px;' width='" + sliderImageWidth + "' height='" + sliderImageHeight + "' border='0'  />");

        /*
        if (this.ObjectTableType == 'p') {
        sliderRows.push("<img class='urlImageLarge' src='ImageHandler.ashx?id=" + this.ID + "&type=r_i&date=" + ModifiedDate + "&uniqueindex=" + uniqueIndex + "' border='1' style='float: left; padding: 10px;opacity:0;' width='" + sliderImageWidth + "' height='" + sliderImageHeight + "' border='0'  />");
        }
        else {
        sliderRows.push("<img class='urlImageLarge' src='ImageHandler.ashx?id=" + this.ID + "&type=m_i&date=" + ModifiedDate + "&uniqueindex=" + uniqueIndex + "' border='1' style='float: left; padding: 10px;opacity:0;' width='" + sliderImageWidth + "' height='" + sliderImageHeight + "' border='0'  />");
        }
        */

        sliderRows.push("<div class='LoadingDiv' style='width:" + (sliderImageWidth + 10) + "px; height:" + (sliderImageHeight + 10) + "px;'>");

        if (this.ObjectTableType == 'p') {
            sliderRows.push("<img onload='SetImageSize()' class='urlImageLarge' src='ImageHandler.ashx?id=" + this.ID + "&type=r_i&date=" + ModifiedDate + "&uniqueindex=" + uniqueIndex + "' border='1' style='opacity:0;float: left; padding: 5px;' border='0'  />");
        }
        else {
            sliderRows.push("<img onload='SetImageSize()' class='urlImageLarge' src='ImageHandler.ashx?id=" + this.ID + "&type=m_i&date=" + ModifiedDate + "&uniqueindex=" + uniqueIndex + "' border='1' style='opacity:0;float: left; padding: 5px;' border='0'  />");
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
        //sliderRows.push("<a title='View Settings' href='#' class='SliderViewSettings'><img src='Images/ViewSettings.jpg' width='81' height='31' alt='View Settings' border='0' style='float: left; padding-left: 2px; padding-top: 5px;' /></a>");
        sliderRows.push("<a galleryContentId='" + this.GalleryContentId + "' TableType='" + this.ObjectTableType + "' title='Click to remove from my gallery' href='#' class='RemoveFromGallery'><img src='Images/RemoveFromGallery.jpg' width='59' height='31' alt='click to remove from my gallery' border='0' style='float: right; padding-right: 0px; padding-top: 1px;' /></a>");
        sliderRows.push("</div>");


        sliderRows.push("<div class='notes'>");
        sliderRows.push("<div class='notetools'>");
        //sliderRows.push("<a title='View Settings' href='#' class='SliderViewSettings'><img src='Images/ViewSettings.jpg' width='81' height='31' alt='View Settings' border='0' style='float: left; padding-left: 2px; padding-top: 5px;' /></a>");
        sliderRows.push("<span class='notestext'>NOTES</span>");
        //sliderRows.push("<img src='Images/AddNotesToGallery.jpg' width='51' height='27' alt='click to add notes' border='0' style='float: right; padding-right: 3px; padding-top: 10px;' />");
        sliderRows.push("</div>");
        sliderRows.push("<div class='scrollnotes' id='scrollbar'>");
        sliderRows.push(galleryObjectNotes);
        sliderRows.push("</div>");
        sliderRows.push("<div class='addnotes' title='Type notes here...'>");
        sliderRows.push("<textarea id='txtNote' tabindex='0' maxlength='1000' autofocus='autofocus'></textarea>");
        sliderRows.push("<div class='notespostdiv' title='Click to add notes'>");
        sliderRows.push("<input galleryContentId='" + this.GalleryContentId + "' id='btnNotes' type='button' value='POST' class='postnotes' />");

        sliderRows.push("<div id='AddRemoveLoading' style='display: none; float: left;'>");
        sliderRows.push("<span style='margin-left: 8px;'><img src='Images/Loading.gif' class='loading' /></span>");
        sliderRows.push("<div>");

        sliderRows.push("</div>");

        //sliderRows.push("<a galleryContentId='" + this.GalleryContentId + "' title='click to add notes' href='#' class='AddNotes'><img src='Images/AddNotesToGallery.jpg' width='51' height='27' alt='click to add notes' border='0' style='float: right; padding-left: 2px; padding-top: 5px;' /></a>");
        sliderRows.push("</div>");
        sliderRows.push("</div>");

        if (this.ClientType != 'Easy') {
            sliderRows.push("<div style='border: 0px solid #000000; margin-top: 15px; float: right;'>");

            sliderRows.push("<div style='float: left;'>");
            sliderRows.push("<img src='Images/Easy.jpg' style='margin-top: .7em; margin-right: .3em;' />");
            sliderRows.push("</div>");

            sliderRows.push("<div class='clienttype' style='float: left; padding-top: 3px;'>");
            sliderRows.push("Not available in Easy Editions");
            sliderRows.push("</div>");

            sliderRows.push("</div>");
        }

        sliderRows.push("</div>");
        sliderRows.push("</div>");

    });

    sliderContent = sliderRows.join("");

    //debugger;

    ShowSlideShowDialog(selectedPAObjectId, selectedPAObjectIndexId, sliderContent, queryReportIds.length, queryReportIds, galleryContentIds, queryReportTableTypes);

    EllipsisToolTipOnSlider();
}

// remove from my gallery /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function RemoveFromMyGallery(objectId, tableType) {
    //debugger;

    $.ajax({
        type: "POST",
        url: "my-gallery.aspx/RemoveFromMyGallery",
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

    if (data == "-1") // error
    {
        // show error to user
        showErrorToUser("");
    }
    else {

        pageIndex = 1;

        // open dialog
        $("#dialog-modal").dialog("close");

        // set object type
        objectType = objectTypeNone;

        // get my gallery objects
        GetAllMyGalleryPAObjects();
    }
}

// add notes /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function AddNotes(objectId) {
    //debugger;

    var note = $("#txtNote").val();

    $.ajax({
        type: "POST",
        url: "my-gallery.aspx/AddNotes",
        data: JSON.stringify({ id: objectId, note: note }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: true,
        success: OnAddNotesSuccess,
        error: OnAddNotesError
    });

}

function OnAddNotesError(request, status, error) {
    //debugger;

    // browser loggin
    fireBugConsoleLog(request, status, error);

    // show error to user
    showErrorToUser("");

    // log4net logging
    var errorText = "OnAddNotesError() jQuery Exception: response text= " + request.responseText + ", status= " + status + ", error= " + error;
    ClientSideLogging(errorText);
}

function OnAddNotesSuccess(response, e) {
    //debugger;

}

// remove notes /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function RemoveNotes(objectId) {
    //debugger;

    $.ajax({
        type: "POST",
        url: "my-gallery.aspx/RemoveNotes",
        data: JSON.stringify({ id: objectId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: true,
        success: OnAddNotesSuccess,
        error: OnAddNotesError
    });
}

function OnRemoveNotesError(request, status, error) {
    //debugger;

    // browser loggin
    fireBugConsoleLog(request, status, error);

    // show error to user
    showErrorToUser("");

    // log4net logging
    var errorText = "OnRemoveNotesError() jQuery Exception: response text= " + request.responseText + ", status= " + status + ", error= " + error;
    ClientSideLogging(errorText);
}

function OnRemoveNotesSuccess(response, e) {
    //debugger;
}

// get notes /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function GetNotes(objectId) {
    //debugger;

    $.ajax({
        type: "POST",
        url: "my-gallery.aspx/GetNotes",
        data: JSON.stringify({ id: objectId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: true,
        success: OnGetNotesSuccess,
        error: OnGetNotesError
    });
}

function OnGetNotesError(request, status, error) {
    //debugger;

    // browser loggin
    fireBugConsoleLog(request, status, error);

    // show error to user
    showErrorToUser("");

    // log4net logging
    var errorText = "OnGetNotesError() jQuery Exception: response text= " + request.responseText + ", status= " + status + ", error= " + error;
    ClientSideLogging(errorText);
}

function OnGetNotesSuccess(response, e) {
    //debugger;

    var rows = [];

    data = $.parseJSON(response.d);

    if (data == "-1") // error
    {
        // show error to user
        showErrorToUser("");
    }
    else {

        $.each(data, function () {
            //debugger;

            rows.push("<div>");

            rows.push("<div class='note'>");
            rows.push(this.Note);
            rows.push("</div>");

            rows.push("<div noteId='" + this.ID + "' title='Click to delete note' class='notedelete'>");
            rows.push("x");
            rows.push("</div>");

            rows.push("<div class='notespace'>");
            rows.push("</div>");

            rows.push("</div>");

        });

        galleryObjectNotes = rows.join("");

        // clear
        $("#txtNote").val('');
    }
}

// get notes after adding /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function GetNotesAfterAddingOrRemoving(objectId) {
    //debugger;

    $.ajax({
        type: "POST",
        url: "my-gallery.aspx/GetNotes",
        data: JSON.stringify({ id: objectId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: true,
        success: OnGetNotesAfterAddingOrRemovingSuccess,
        error: OnGetNotesAfterAddingOrRemovingError
    });

}

function OnGetNotesAfterAddingOrRemovingError(response) {
    //debugger;

    // browser loggin
    fireBugConsoleLog(request, status, error);

    // show error to user
    showErrorToUser("");

    // log4net logging
    var errorText = "OnGetNotesAfterAddingOrRemovingError() jQuery Exception: response text= " + request.responseText + ", status= " + status + ", error= " + error;
    ClientSideLogging(errorText);
}

function OnGetNotesAfterAddingOrRemovingSuccess(response, e) {
    //debugger;

    var rows = [];

    data = $.parseJSON(response.d);

    if (data == "-1") // error
    {
        // show error to user
        showErrorToUser("");
    }
    else {

        $.each(data, function () {
            //debugger;

            // slider

            rows.push("<div>");

            rows.push("<div class='note'>");
            rows.push(this.Note);
            rows.push("</div>");

            rows.push("<div noteId='" + this.ID + "' title='Click to delete note' class='notedelete'>");
            rows.push("x");
            rows.push("</div>");

            rows.push("<div class='notespace'>");
            rows.push("</div>");

            rows.push("</div>");

        });

        galleryObjectNotes = rows.join("");

        $('.scrollnotes').empty();

        $('.scrollnotes').append(rows.join(""));

        // clear
        $("#txtNote").val('');

        //notes scroll bar
        $("#scrollbar").mCustomScrollbar({
            scrollButtons: {
                enable: true
            }
        });

        /*
        //scroll bar resize
        ScrollBarReSize();*/
    }
}

// scroll bar resize event /////////////////////////////////////////////////////////////////////////////////////////////////////////////

function ScrollBarReSize() {
    //debugger;

    var scrollNotesHeight = $(".scrollnotes").height();

    var notesHeight = 0;
    var noteSpaceHeight = 3;

    var notesElements = $(".note");

    for (i = 0; i < notesElements.length; i++) {
        notesHeight += notesElements[i].offsetHeight + noteSpaceHeight;
    }
    if (scrollNotesHeight < notesHeight) {

        //$('.note').css("width", "320px");
        $('.note').css("width", "94%");
    }
    else {
        //$('.note').css("width", "350px");
        $('.note').css("width", "95%");
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

// window resize event /////////////////////////////////////////////////////////////////////////////////////////////////////////////

function WindowReSize() {
    //debugger;

    // positioning log-in image
    var position = $("#btnShare").offset();
    var windowWidth = $(window).width();

    $("#message_bar").css("left", windowWidth / 2 - 105);
    $("#message_bar").css("top", position.top + 70);
}

// get note count /////////////////////////////////////////////////////////////////////////////////////////////////////////////

function GetNoteCount() {
    //debugger;

    var noteCount = $(".note").length;

    // note doesn't exists image
    if (noteCount == 1) {

        $('#imgAddRemove' + selectedObjectId).attr({ src: 'Images/AddedRemoveFromGallery.png', id: 'imgAddRemove' + selectedObjectId, width: '24', height: '16', border: '0', style: 'float: right; padding-right: 5px; padding-top: 5px;' });
    }

    // note exists image
    if (noteCount == 0) {

        $('#imgAddRemove' + selectedObjectId).attr({ src: 'Images/AddedRemoveFromGallery.png', id: 'imgAddRemove' + selectedObjectId, width: '24', height: '16', border: '0', style: 'float: right; padding-right: 5px; padding-top: 5px;' });
    }
}