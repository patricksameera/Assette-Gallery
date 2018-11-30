///////////////////////////////////////////////////////
//********** Assette Gallery       **********//
//********** Assette © 2013                **********//
///////////////////////////////////////////////////////

//debugger;

var idx = 0;
var sliderContent;
var imageSliderContent;
var selectedPAObjectId;
var selectedPAObjectIndexId;
var galleryContentId;
var reportIds = [];
var galleryReportIds = [];
var objectTypeNone = 0;
var objectTypeTable = 1;
var objectTypeChart = 2;
var objectType;
var galleryObjectNotes;
var uniqueIndex = 0;
var tableType;
var reportIdTableTypes = [];

// document ready //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// jquery document ready function
$(document).ready(function (e) {
    //debugger;

    // window events

    // window resize event
    WindowReSize();

    // window resize event
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
    $('#editsamplereports').css('background-color', '#999999');
    $('#edittemplatedesigns').css('background-color', '#999999');
    $('#clients').css('background-color', '#C1D82F');
    $('#editreportobjects').css('background-color', '#999999');
    $('#editobjectcategoryorder').css('background-color', '#999999');

    // loading image functionality
    $("#divLoading").ajaxStart(OnAjaxStart);
    $("#divLoading").ajaxError(OnAjaxError);
    $("#divLoading").ajaxSuccess(OnAjaxSuccess);
    $("#divLoading").ajaxStop(OnAjaxStop);
    $("#divLoading").ajaxComplete(OnAjaxComplete);

    // fill dropdowns - bind values
    //GetPAObjectClientTypes();
    //GetPAObjeCtategories();

    // initial page index for scroll loading
    var pageIndex = 1;

    // set object type
    objectType = objectTypeNone;

    // get pa-objects
    GetMyGalleryPAObjects(pageIndex, objectType);

    // scrolling load
    $(window).scroll(function () {
        if ($(window).scrollTop() == $(document).height() - $(window).height()) {
            //debugger;

            // get the pa-object div count
            var count = $(".r7").length;

            var x = count;
            var y = 5;
            x %= y;

            if (x == 0) {
                pageIndex++;

                // set object type
                objectType = objectTypeNone;

                GetMyGalleryPAObjects(pageIndex, objectType);
            }
        }
    });

    // search button click
    $('#btnSearch').live("click", function (e) {
        //debugger;

        // stop postback
        e.preventDefault();

        // clear slider contents
        sliderRows = [];

        // clear index
        idx = 0;

        // clear contents
        $("#divGalleryObjects").empty();

        pageIndex = 1;

        // set object type
        objectType = objectTypeNone;

        // get objects
        GetMyGalleryPAObjects(pageIndex, objectType);
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

        galleryContentId = $(this).attr("GalleryContentId");

        // get clicked object's table type
        tableType = $(this).attr("TableType");

        // get notes
        GetNotes(galleryContentId);

        setTimeout(function () {
            //debugger;

            // get report object
            GetMyGalleryPAObjectDetails(pAObjectId, indexId, galleryContentId, tableType);
        }, 1000);

    });

    // re-set filter
    $('#btnAll').live("click", function (e) {
        //debugger;

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

        //////////////////////////////////////////////

        // stop postback
        e.preventDefault();

        // clear index
        idx = 0;

        // clear report ids
        reportIds = [];
        galleryReportIds = [];
        reportIdTableTypes = [];

        // clear contents
        $("#divGalleryObjects").empty();

        pageIndex = 1;

        // set object type
        objectType = objectTypeNone;

        // get objects
        GetMyGalleryPAObjects(pageIndex, objectType);
    });

    // filter by table
    $('#btnTables').live("click", function (e) {
        //debugger;

        // stop postback
        e.preventDefault();

        // clear index
        idx = 0;

        // clear report ids
        reportIds = [];
        galleryReportIds = [];
        reportIdTableTypes = [];

        // clear contents
        $("#divGalleryObjects").empty();

        pageIndex = 1;

        // set object type
        objectType = objectTypeTable;

        // get objects
        GetMyGalleryPAObjects(pageIndex, objectType);
    });

    // filter by charts
    $('#btnCharts').live("click", function (e) {
        //debugger;

        // stop postback
        e.preventDefault();

        // clear index
        idx = 0;

        // clear report ids
        reportIds = [];
        galleryReportIds = [];
        reportIdTableTypes = [];

        // clear contents
        $("#divGalleryObjects").empty();

        pageIndex = 1;

        // set object type
        objectType = objectTypeChart;

        // get objects
        GetMyGalleryPAObjects(pageIndex, objectType);
    });

    // remove from gallery button click
    $('.RemoveFromGallery').live("click", function (e) {
        //debugger;

        // stop postback
        e.preventDefault();

        // get clicked object's id
        galleryContentId = $(this).attr("GalleryContentId");

        // remove from gallery
        RemoveFromMyGallery(galleryContentId);
    });

    // add notes button click
    $('#btnNotes').live("click", function (e) {
        //debugger;

        // stop postback
        e.preventDefault();

        // get clicked object's id
        galleryContentId = $(this).attr("GalleryContentId");

        var note = $("#txtNote").val();

        if (note != '') {

            // add note
            AddNotes(galleryContentId);

            // get notes
            //GetNotesAfterAddingOrRemoving(galleryContentId);

            setTimeout(function () {
                //debugger;
                // get notes
                GetNotesAfterAddingOrRemoving(galleryContentId);
            }, 1000);

        }
    });

    // remove note
    $('.notedelete-disabled').live("click", function (e) {
        //debugger;

        // stop postback
        e.preventDefault();

        // get clicked object's id
        noteId = $(this).attr("noteId");

        // remove note
        RemoveNotes(noteId, $(this));

        // get notes
        //GetNotesAfterAddingOrRemoving(galleryContentId);

        setTimeout(function () {
            //debugger;
            // get notes
            GetNotesAfterAddingOrRemoving(galleryContentId);
        }, 1000);

    });

    // go back button click
    $('#goBack').live("click", function (e) {
        //debugger;

        location.href = "clients.aspx";
    });

})

// get pa-object details /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function GetMyGalleryPAObjects(pageIndex, typeId) {
    //debugger;
    var categoryId = $("#Category").val();
    var clientType = $("#ClientType").val();
    var searchText = $("#Search").val();

    var userId = getQueryStringParameterByName("userid");

    if (categoryId == null) categoryId = 0;
    if (clientType == null) clientType = "0";
    if (searchText == "SEARCH") searchText = "";

    var typeId = parseInt(typeId);

    $.ajax({
        type: "POST",
        url: "../Admin/client-gallery.aspx/GetMyGalleryPAObjects",
        data: JSON.stringify({ pageIndex: pageIndex, categoryId: categoryId, clientType: clientType, searchText: searchText, type: typeId, userId: userId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OnGetMyGalleryPAObjectsSuccess,
        error: OnGetMyGalleryPAObjectsError
    });

}

function OnGetMyGalleryPAObjectsError(request, status, error) {
    //debugger;

    // browser loggin
    fireBugConsoleLog(request, status, error);

    // show error to user
    showErrorToUser("");

    // log4net logging
    var errorText = "OnGetMyGalleryPAObjectsError() jQuery Exception: response text= " + request.responseText + ", status= " + status + ", error= " + error;
    ClientSideLogging(errorText);
}

function OnGetMyGalleryPAObjectsSuccess(response) {
    //debugger;

    showOnATable(response);
}

// show data on a table /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function showOnATable(response) {
    //debugger;

    var galleryRows = [];
    sliderContent = "";

    //debugger;
    data = $.parseJSON(response.d);

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

        // add report id's to array
        reportIds.push(this.ID);
        galleryReportIds.push(this.GalleryContentId);
        reportIdTableTypes.push(this.ObjectTableType);

        // gallery
        galleryRows.push("<div class='r7'>");
        galleryRows.push("<div class='r9'>");

        //galleryRows.push("<a href='#' class='ViewLibrary' GalleryContentId='" + this.GalleryContentId + "' TableType='" + this.ObjectTableType + "' Index='" + idx + "' Id='" + this.ID + "'><img class='urlImageSmall' src='ImageHandler.ashx?id=" + this.ID + "&type=r_t&date=" + ModifiedDate + "&uniqueindex=" + uniqueIndex + "' width='140' height='90'  border='0' style='float: left;' /></a>");
        //galleryRows.push("<img src='../Client/ImageHandler.ashx?id=" + this.ID + "&type=r_t' width='140' height='90'  border='0' style='float: left;' />");


        if (this.ObjectTableType == 'p') {
            galleryRows.push("<a href='#' class='ViewLibrary' GalleryContentId='" + this.GalleryContentId + "' TableType='" + this.ObjectTableType + "' Index='" + idx + "' Id='" + this.ID + "'><img class='urlImageSmall' src='ImageHandler.ashx?id=" + this.ID + "&type=r_t&date=" + ModifiedDate + "&uniqueindex=" + uniqueIndex + "' width='140' height='90'  border='0' style='float: left;' /></a>");
        }
        else {
            galleryRows.push("<a href='#' class='ViewLibrary' GalleryContentId='" + this.GalleryContentId + "' TableType='" + this.ObjectTableType + "' Index='" + idx + "' Id='" + this.ID + "'><img class='urlImageSmall' src='ImageHandler.ashx?id=" + this.ID + "&type=m_t&date=" + ModifiedDate + "&uniqueindex=" + uniqueIndex + "' width='140' height='90'  border='0' style='float: left;' /></a>");
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
        galleryRows.push("</div>");
        galleryRows.push("<div class='r12'>");
        galleryRows.push("</div>");
        galleryRows.push("</div>");

    });

    if (idx == 0) {
        galleryRows.push("<div class='r17'>");
        galleryRows.push("No objects found in Client Gallery.");
        galleryRows.push("</div>");

        $("#divGalleryObjects").append(galleryRows.join(""));
    }
    else {
        $("#divGalleryObjects").append(galleryRows.join(""));
    }
}

// loading screen functionality /////////////////////////////////////////////////////////////////////////////////////////////////////////////

function OnAjaxStart() {
    //debugger;

    $("#divLoading").fadeIn('slow');
}

function OnFailure(response) {
    //debugger;

}

function OnError(response) {
    //debugger;

    var errorText = response.responseText;
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
        url: "../my-gallery.aspx/GetPAObjectClientTypes",
        data: "",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
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
        url: "../my-gallery.aspx/GetPAObjeCtategories",
        data: "",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
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

    // clear dropdown
    dropdown.empty();

    // detach dropdown
    dropdown.selectbox('detach');

    dropdown.append('<option value=0>' + defaultText + '</option>');

    $.each(data, function () {

        dropdown.append('<option value=' + this.ID + '>' + this.Type + '</option>');

    });

    // jquery select box - set
    dropdown.selectbox();
}

// get report object details /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function GetMyGalleryPAObjectDetails(objectId, indexId, galleryContentId, tableType) {
    //debugger;

    selectedPAObjectIndexId = indexId;
    selectedPAObjectId = objectId;
    selectedPAObjectTableType = tableType;

    $.ajax({
        type: "POST",
        url: "../my-gallery.aspx/GetMyGalleryPAObjectDetails",
        data: JSON.stringify({ id: galleryContentId, type: tableType }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OnGetMyGalleryPAObjectDetailsSuccess,
        error: OnGetMyGalleryPAObjectDetailsError
    });
}

function OnGetMyGalleryPAObjectDetailsError(request, status, error) {
    //debugger;

    // browser loggin
    fireBugConsoleLog(request, status, error);

    // show error to user
    showErrorToUser("");

    // log4net logging
    var errorText = "OnGetMyGalleryPAObjectDetailsError() jQuery Exception: response text= " + request.responseText + ", status= " + status + ", error= " + error;
    ClientSideLogging(errorText);
}

function OnGetMyGalleryPAObjectDetailsSuccess(response) {
    //debugger;

    var sliderRows = [];

    data = $.parseJSON(response.d);

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

        // slider
        sliderRows.push("<div class='slide' id='" + this.ID + "'>");
        //sliderRows.push(this.ID);
        sliderRows.push("<div class='left' title='click to view report object'>");

        //sliderRows.push("<img class='urlImageSmall' src='ImageHandler.ashx?id=" + this.ID + "&type=r_i&date=" + ModifiedDate + "&uniqueindex=" + uniqueIndex + "' width='390' height='340' border='1' style='float: left; padding-left: 2px; padding-top: 5px;' />");

        if (this.ObjectTableType == 'p') {
            sliderRows.push("<img class='urlImageSmall' src='ImageHandler.ashx?id=" + this.ID + "&type=r_i&date=" + ModifiedDate + "&uniqueindex=" + uniqueIndex + "' width='390' height='340' border='1' style='float: left; padding-left: 2px; padding-top: 5px;' />")
        }
        else {
            sliderRows.push("<img class='urlImageSmall' src='ImageHandler.ashx?id=" + this.ID + "&type=m_i&date=" + ModifiedDate + "&uniqueindex=" + uniqueIndex + "' width='390' height='340' border='1' style='float: left; padding-left: 2px; padding-top: 5px;' />")
        }

        sliderRows.push("</div>");
        sliderRows.push("<div class='centre'>");
        sliderRows.push("</div>");
        sliderRows.push("<div class='right'>");
        sliderRows.push("<div class='top'>");
        sliderRows.push("<div class='maintitle'>");
        sliderRows.push(this.Name);
        sliderRows.push("</div>");
        sliderRows.push("<div class='subtitle'>");
        sliderRows.push(this.Category);
        sliderRows.push("</div>");
        sliderRows.push("<div class='description'>");
        sliderRows.push(this.Description);
        sliderRows.push("</div>");
        sliderRows.push("</div>");
        sliderRows.push("<div class='bellow'>");
        //sliderRows.push("<a title='View Settings' href='#' class='SliderViewSettings'><img src='Images/ViewSettings.jpg' width='81' height='31' alt='View Settings' border='0' style='float: left; padding-left: 2px; padding-top: 5px;' /></a>");
        sliderRows.push("<a style='pointer-events: none;cursor: default;' galleryContentId='" + this.GalleryContentId + "' title='Click to remove from my gallery - Disabled' href='#' class='RemoveFromGallery-disabled'><img src='../Images/RemoveFromGallery.jpg' width='59' height='31' alt='click to remove from my gallery' border='0' style='float: right; padding-right: 0px; padding-top: 1px;' /></a>");
        sliderRows.push("</div>");
        sliderRows.push("<div class='notes'>");
        sliderRows.push("<div class='notetools'>");
        //sliderRows.push("<a title='View Settings' href='#' class='SliderViewSettings'><img src='Images/ViewSettings.jpg' width='81' height='31' alt='View Settings' border='0' style='float: left; padding-left: 2px; padding-top: 5px;' /></a>");
        sliderRows.push("<span class='notestext'>NOTES</span>");
        sliderRows.push("<img src='../Images/AddNotesToGallery.jpg' width='51' height='27' alt='click to add notes' border='0' style='float: right; padding-right: 3px; padding-top: 10px;' />");
        sliderRows.push("</div>");
        sliderRows.push("<div class='scrollnotes' id='scrollbar'>");
        sliderRows.push(galleryObjectNotes);
        sliderRows.push("</div>");
        //        sliderRows.push("<div class='addnotes' title='type notes here...'>");
        //        sliderRows.push("<textarea id='txtNote' maxlength='1000'></textarea>");
        //        sliderRows.push("<div class='notespostdiv' title='click to add notes'>");
        //        sliderRows.push("<input galleryContentId='" + this.GalleryContentId + "' id='btnNotes' type='button' value='POST' class='postnotes' />");
        //        sliderRows.push("</div>");
        //        //sliderRows.push("<a galleryContentId='" + this.GalleryContentId + "' title='click to add notes' href='#' class='AddNotes'><img src='Images/AddNotesToGallery.jpg' width='51' height='27' alt='click to add notes' border='0' style='float: right; padding-left: 2px; padding-top: 5px;' /></a>");
        //        sliderRows.push("</div>");
        sliderRows.push("</div>");
        sliderRows.push("</div>");
        sliderRows.push("</div>");

    });

    sliderContent = sliderRows.join("");

    var noOfReports = idx;

    ShowSlideShowDialog(selectedPAObjectId, selectedPAObjectIndexId, sliderContent, noOfReports, reportIds, galleryReportIds, reportIdTableTypes);

}

// remove from my gallery /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function RemoveFromMyGallery(objectId) {
    //debugger;

    $.ajax({
        type: "POST",
        url: "../my-gallery.aspx/RemoveFromMyGallery",
        data: JSON.stringify({ id: objectId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
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

    // open dialog
    $("#dialog-modal").dialog("close");

    // clear slider contents
    sliderRows = [];

    // clear index
    idx = 0;

    // clear contents
    $("#divGalleryObjects").empty();

    pageIndex = 1;

    // set object type
    objectType = objectTypeNone;

    GetMyGalleryPAObjects(pageIndex, objectType);
}

// add notes /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function AddNotes(objectId) {
    //debugger;

    var note = $("#txtNote").val();

    $.ajax({
        type: "POST",
        url: "../my-gallery.aspx/AddNotes",
        data: JSON.stringify({ id: objectId, note: note }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
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
        url: "../my-gallery.aspx/RemoveNotes",
        data: JSON.stringify({ id: objectId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
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
        url: "../my-gallery.aspx/GetNotes",
        data: JSON.stringify({ id: objectId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OnGetNotesSuccess,
        error: OnGetNotesError
    });

}


function OnGetNotesError(response) {
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

    $.each(data, function () {
        //debugger;

        rows.push("<div>");

        rows.push("<div class='note'>");
        rows.push(this.Note);
        rows.push("</div>");

        rows.push("<div noteId='" + this.ID + "' title='Click to delete note - Disabled' class='notedelete'>");
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

// get notes after adding /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function GetNotesAfterAddingOrRemoving(objectId) {
    //debugger;

    $.ajax({
        type: "POST",
        url: "../my-gallery.aspx/GetNotes",
        data: JSON.stringify({ id: objectId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OnGetNotesAfterAddingOrRemovingSuccess,
        error: OnGetNotesAfterAddingOrRemovingError
    });

}

function OnGetNotesAfterAddingOrRemovingError(request, status, error) {
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
}

// window resize event /////////////////////////////////////////////////////////////////////////////////////////////////////////////

function WindowReSize() {
    //debugger;

    // positioning log-in image
    var position = $("#goBack").offset();
    var windowWidth = $(window).width();

    $("#message_bar").css("left", windowWidth / 2 - 70);
    $("#message_bar").css("top", position.top - 10);
}

 
