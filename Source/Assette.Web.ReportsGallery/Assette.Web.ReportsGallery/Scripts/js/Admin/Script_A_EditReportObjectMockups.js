///////////////////////////////////////////////////////
//********** Assette Gallery       **********//
//********** Assette © 2013                **********//
///////////////////////////////////////////////////////

//debugger;

// document ready //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

/*
var categoryValues = '1:Asset Allocation;2:Advanced Analytics;3:Attribution;4:Characteristics;5:Gains/Losses;6:Holdings;7:Other;8:Portfolio Activity;9:Performance;10:Sector Allocation;11:Transactions;12:Account Name;13:Account Number;14:Sentences ;15:Dates';
var typeValues = '1:Table;2:Chart';
var clientTypeValues = 'Easy:Easy;Standard:Standard';
*/

var categoryValues = '';
var typeValues = '';
var clientTypeValues = '';

// jquery document ready function

$(document).ready(function () {
    //debugger;

    // window events

    // window resize event
    WindowReSize();

    // window resize event
    $(window).resize(function () {

        // window resize event
        WindowReSize();
    });

    // upload button click
    $('#triggerUpload').live("click", function (e) {
        //debugger;

        location.href = "mockup-object-uploader.aspx";
    });

    // get dropdown values
    GetPAObjectClientTypes();
    GetAllPAObjeCtategories();
    GetAllPAObjectTypes();

    // set selected tab link  
//    $('#editreportobjects').css('background-color', '#999999');
//    $('#editsamplereports').css('background-color', '#999999');
//    $('#edittemplatedesigns').css('background-color', '#999999');
//    $('#clients').css('background-color', '#999999');
//    $('#editreportobjectmockups').css('background-color', '#C1D82F');
//    $('#editobjectcategoryorder').css('background-color', '#999999');
    //    $('#editheadtagcontent').css('background-color', '#999999');

    // set selected tab link  
    $('#editreportobjects').css('background-color', '#C1D82F');
    $('#editsamplereports').css('background-color', '#999999');
    $('#edittemplatedesigns').css('background-color', '#999999');
    $('#clients').css('background-color', '#999999');
    $('#editreportobjectmockups').css('background-color', '#999999');
    $('#editobjectcategoryorder').css('background-color', '#999999');
    $('#editheadtagcontent').css('background-color', '#999999');

    //jqgrid functionality
    var grid = jQuery("#gridReportObjectMockups");

    grid.jqGrid({
        colNames: ['ID', 'Name', 'Description', 'Client Type', 'Category Id', 'Report Category', 'Type Id', 'Report Type', 'Preview'],
        colModel: [
            { name: 'ID', width: 30, align: 'left', sortable: true, hidden: false, editable: false, key: true, search: true, searchoptions: { sopt: ['eq'], size: 25} },
            { name: 'Name', width: 150, align: 'left', sortable: false, hidden: false, editable: false, search: true, searchoptions: { sopt: ['eq', 'cn'], size: 25 }, editable: true, editrules: { required: true }, editoptions: { size: 90, maxlength: 1000} },
            { name: 'Description', width: 150, align: 'left', sortable: false, hidden: false, editable: true, search: true, searchoptions: { sopt: ['eq', 'cn'], size: 25 }, edittype: 'textarea', editrules: { edithidden:true, required: true }, editoptions: { rows: '3', cols: '100'} },
            { name: 'ClientType', width: 100, align: 'left', sortable: false, hidden: false, search: true, editable: true, edittype: 'select', editoptions: { value: clientTypeValues }, stype: 'select', searchoptions: { value: clientTypeValues, sopt: ['eq', 'ne']} }, // dropdown values: async: false
            { name: 'CategoryId', width: 100, align: 'left', sortable: false, hidden: true, search: false, editable: false },
            { name: 'Category', width: 100, align: 'left', sortable: false, hidden: false, editable: true, edittype: 'select', editoptions: { value: categoryValues }, stype: 'select', searchoptions: { value: categoryValues, sopt: ['eq', 'ne']} }, // dropdown values: async: false
            { name: 'TypeId', width: 100, align: 'left', sortable: false, hidden: true, search: false },
            { name: 'Type', width: 100, align: 'left', sortable: false, hidden: false, editable: true, edittype: 'select', editoptions: { value: typeValues }, stype: 'select', searchoptions: { value: typeValues, sopt: ['eq', 'ne']} }, // dropdown values: async: false
            { name: 'PreRegisterPreView', width: 50, align: 'left', sortable: false, hidden: false, editable: true, edittype: 'checkbox', search: true, editoptions: { value: "True:False" }, stype: 'select', searchoptions: { value: "True:True;False:False", sopt: ['eq', 'ne']} }, // dropdown values: async: false
        ],
        rowNum: 15,
        rowList: [1, 15, 30, 50],
        pager: jQuery('#pagerReportObjectMockups'),
        sortname: "Id",
        sortorder: "asc",
        caption: "Assette - Report Object Mockups Details",
        cellEdit: false,
        width: 885,
        height: 270,
        gridview: true,
        shrinkToFit: true,
        ignoreCase: true,
        rownumbers: true,
        editurl: "../Admin/edit-report-object-mockups.aspx/UpdateReportObjectMockup",
        viewrecords: true,
        recordtext: "View {0} - {1} of {2}",
        emptyrecords: "No records found.",
        loadtext: "Loading...",
        pgtext: "Page {0} of {1}",

        // get json data
        datatype: function (parameterData) {
            GetReportObjectData(parameterData);
        },

        // events
        gridComplete: LoadComplete,

        // formatter
        formatter: {
            integer: { thousandsSeparator: " ", defaultValue: '0' },
            number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
            currency: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' },
            date: {
                dayNames: [
    "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat",
    "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"
    ],
                monthNames: [
    "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec",
    "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"
    ],
                AmPm: ["am", "pm", "AM", "PM"],
                S: function (j) { return j < 11 || j > 13 ? ['st', 'nd', 'rd', 'th'][Math.min((j - 1) % 10, 3)] : 'th' },
                srcformat: 'Y-m-d',
                newformat: 'd/m/Y',
                masks: {
                    ISO8601Long: "Y-m-d H:i:s",
                    ISO8601Short: "Y-m-d",
                    ShortDate: "n/j/Y",
                    LongDate: "l, F d, Y",
                    FullDateTime: "l, F d, Y g:i:s A",
                    MonthDay: "F d",
                    ShortTime: "g:i A",
                    LongTime: "g:i:s A",
                    SortableDateTime: "Y-m-d\\TH:i:s",
                    UniversalSortableDateTime: "Y-m-d H:i:sO",
                    YearMonth: "F, Y"
                },
                reformatAfterEdit: false
            },
            baseLinkUrl: '',
            showAction: '',
            target: '',
            checkbox: { disabled: true },
            idName: 'Id'
        }

    });

    //    grid.jqGrid('navGrid', '#pagerSamples',
    //               {}, // options
    //               {}, // edit options 
    //               {}, // add options
    //               {}, // delete options
    //               {}  // search options
    //               );

    // jqgrid options
    grid.jqGrid('navGrid', '#pagerReportObjectMockups',
    // options//////////////////////////////////////////////////////////////////////////////////////////////
               {
               // properties
               edit: true,
               view: false,
               add: false,
               del: false,
               search: true,
               refresh: true,

               // events
               beforeRefresh: RefreshGrid
           },
    // edit options/////////////////////////////////////////////////////////////////////////////////////////
               {
               // properties
               closeAfterEdit: true,
               closeOnEscape: true,
               closeAfterSubmit: true,
               viewPagerButtons: false,
               recreateForm: true,
               height: '370',
               width: '830',

               // events
               onclickSubmit: EditReportObjectData
           },
    // add options////////////////////////////////////////////////////////////////////////////////////////////  
               {
               // properties
               // events               
           },
    // delete options/////////////////////////////////////////////////////////////////////////////////////////
               {
               // properties
               closeAfterEdit: true,
               closeOnEscape: true,
               closeAfterSubmit: true,

               // events
               onclickSubmit: DeleteReportObjectMockup
           },
    // search options///////////////////////////////////////////////////////////////////////////////////////// 
               {
               // properties
               closeAfterSearch: true,
               closeAfterReset: true,
               closeOnEscape: true,
               multipleSearch: false
           }
               );

    // grid load complete function
    function LoadComplete(parameterData) {
        //debugger;
    }

    // re-load grid
    function RefreshGrid() {
        //debugger;

        grid.jqGrid('setGridParam', { datatype: function (parameterData) {
            GetReportObjectData(parameterData);
        }
        }).trigger('reloadGrid');
    }

    // show report object data row on grid /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    function GetReportObjectData(parameterData) {
        //debugger;

        var parameters = new Object();

        parameters.page = parameterData.page;
        parameters.rows = parameterData.rows;
        parameters.sidx = parameterData.sidx;
        parameters.sord = parameterData.sord;
        parameters.search = parameterData._search;

        if (parameters.search == true) {
            parameters.searchField = parameterData.searchField;
            parameters.searchString = parameterData.searchString;
            parameters.searchOperator = parameterData.searchOper;
        }
        else {
            parameters.searchField = "";
            parameters.searchString = "";
            parameters.searchOperator = "";
        }

        // get page count
        var pageCount = $("#sp_1_pagerReportObjectMockups").text();

        // if entered page number larger than actual page count
        if (pageCount == "" || parseInt(parameters.page) > parseInt(pageCount)) {
            parameters.page = 1;
        }

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../Admin/edit-report-object-mockups.aspx/GetReportObjectMockups",
            data: JSON.stringify(parameters),
            dataType: "json",
            success: OnGetReportObjectDataSuccess,
            error: OnGetReportObjectDataError
        });
    }

    function OnGetReportObjectDataError(request, status, error) {
        //debugger;

        // browser loggin
        fireBugConsoleLog(request, status, error);

        // show error to user
        showErrorToUser("");

        // log4net logging
        var errorText = "OnGetReportObjectDataError() jQuery Exception: response text= " + request.responseText + ", status= " + status + ", error= " + error;
        ClientSideLogging(errorText);
    }

    function OnGetReportObjectDataSuccess(response, textStatus) {
        //debugger;

        if (textStatus == "success") {
            var thegrid = $("#gridReportObjectMockups")[0];

            //var jsonData = JSON.parse(response.d);

            var jsonData = EscapeJsonBreakCharacters(response.d);
            jsonData = JSON.parse(jsonData);

            thegrid.addJSONData(jsonData);
        }
    }

    // edit sample report data /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    function EditReportObjectData(parameters, postData) {
        //debugger;

        var parameters = new Object();

        parameters.Id = postData.gridReportObjectMockups_id;
        parameters.name = postData.Name;
        parameters.description = postData.Description;
        parameters.clientType = postData.ClientType;
        parameters.category = postData.Category;
        parameters.type = postData.Type;
        parameters.preRegisterPreView = postData.PreRegisterPreView;

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../Admin/edit-report-object-mockups.aspx/UpdateReportObjectMockup",
            data: JSON.stringify(parameters),
            dataType: "json",
            success: OnEditReportObjectDataSuccess,
            error: OnEditReportObjectDataError
        });
    }

    function OnEditReportObjectDataError(request, status, error) {
        //debugger;

        // browser loggin
        fireBugConsoleLog(request, status, error);

        // show error to user
        showErrorToUser("");

        // log4net logging
        var errorText = "OnEditReportObjectDataError() jQuery Exception: response text= " + request.responseText + ", status= " + status + ", error= " + error;
        ClientSideLogging(errorText);
    }

    function OnEditReportObjectDataSuccess(response, textStatus) {
        //debugger;

        if (textStatus == "success") {
            grid.jqGrid().trigger('reloadGrid');
        }
    }

    // jqgrid json data example /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //{"total":5,"page":1,"records":47,"rows":[{"i":0,"cell":["9db67f26-92f7-4e90-93a9-5e0317265068","Sameera","Jayalath","sameera.jayalath@assette.com","Assette","192.168.0.150","11/2/2012 8:57:38 AM","0"]},{"i":1,"cell":["d371ba9d-b5bb-4229-bd51-a48f8cb3449f","...
    //{"total":5,"page":1,"records":47,"rows":[]}

    //var test_string = '{"total":4,"page":1,"records":33,"rows":[{"i":0,"cell":["108","Emmerman, Ross","Emmerman, Ross - Client Reporting Has Never Been This Easy. Emmerman, Ross - Client Reporting Has Never Been This Easy.","Quickly creating presentations and reports is easy with our client reporting software, built specifically for investment management firms. Save time, eliminate errors, and make reports look good. Our software accurately feeds data from multiple systems into your client and sales presentations, monthly and quarterly reports, and client portal.","1","Firm 1","1","Product 1"]}]}';
    //var jsonData = JSON.parse(test_string);


    // get pa-object types details /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    function GetPAObjectClientTypes() {
        //debugger;

        $.ajax({
            type: "POST",
            url: "edit-report-objects.aspx/GetPAObjectClientTypes",
            data: "",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
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

    function OnGetPAObjectClientTypesSuccess(data) {
        //debugger;

        var response = jQuery.parseJSON(data.d);

        var rtnValue = "";

        if (response && response.length) {
            for (var i = 0, l = response.length; i < l; i++) {
                var item = response[i];

                if (rtnValue == '') {
                    rtnValue = item.ID + ":" + item.Type;
                }
                else {
                    rtnValue += ";" + item.ID + ":" + item.Type;
                }
            }
        }

        //debugger;

        clientTypeValues = rtnValue;
    }

    // get pa-object categories details /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    function GetAllPAObjeCtategories() {
        //debugger;

        $.ajax({
            type: "POST",
            url: "edit-report-object-mockups.aspx/GetAllPAObjeCtategories",
            data: "",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: OnGetAllPAObjeCtategoriesSuccess,
            error: OnGetAllPAObjeCtategoriesError
        });

    }

    function OnGetAllPAObjeCtategoriesError(request, status, error) {
        //debugger;

        // browser loggin
        fireBugConsoleLog(request, status, error);

        // show error to user
        showErrorToUser("");

        // log4net logging
        var errorText = "OnGetAllPAObjeCtategoriesError() jQuery Exception: response text= " + request.responseText + ", status= " + status + ", error= " + error;
        ClientSideLogging(errorText);
    }

    function OnGetAllPAObjeCtategoriesSuccess(data) {
        //debugger;

        var response = jQuery.parseJSON(data.d);

        var rtnValue = "";

        if (response && response.length) {
            for (var i = 0, l = response.length; i < l; i++) {
                var item = response[i];

                if (rtnValue == '') {
                    rtnValue = item.ID + ":" + item.Type;
                }
                else {
                    rtnValue += ";" + item.ID + ":" + item.Type;
                }
            }
        }

        //debugger;

        categoryValues = rtnValue;
    }

    // get pa-object types details /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    function GetAllPAObjectTypes() {
        //debugger;

        $.ajax({
            type: "POST",
            url: "edit-report-object-mockups.aspx/GetAllPAObjectTypes",
            data: "",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: OnGetAllPAObjectTypesSuccess,
            error: OnGetAllPAObjectTypesError
        });
    }

    function OnGetAllPAObjectTypesError(request, status, error) {
        //debugger;

        // browser loggin
        fireBugConsoleLog(request, status, error);

        // show error to user
        showErrorToUser("");

        // log4net logging
        var errorText = "OnGetAllPAObjectTypesError() jQuery Exception: response text= " + request.responseText + ", status= " + status + ", error= " + error;
        ClientSideLogging(errorText);
    }

    function OnGetAllPAObjectTypesSuccess(data) {
        //debugger;

        var response = jQuery.parseJSON(data.d);

        var rtnValue = "";

        if (response && response.length) {
            for (var i = 0, l = response.length; i < l; i++) {
                var item = response[i];

                if (rtnValue == '') {
                    rtnValue = item.ID + ":" + item.Type;
                }
                else {
                    rtnValue += ";" + item.ID + ":" + item.Type;
                }
            }
        }

        //debugger;

        typeValues = rtnValue;
    }

    // delete report object mockup /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    function DeleteReportObjectMockup(parameters, postData) {
        //debugger;

        var parameters = new Object();

        parameters.Id = postData;

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../Admin/edit-report-object-mockups.aspx/DeleteReportObjectMockup",
            data: JSON.stringify(parameters),
            dataType: "json",
            success: OnDeleteReportObjectMockupSuccess,
            error: OnDeleteReportObjectMockupError
        });
    }

    function OnDeleteReportObjectMockupError(request, status, error) {
        //debugger;

        // browser loggin
        fireBugConsoleLog(request, status, error);

        // show error to user
        showErrorToUser("");

        // log4net logging
        var errorText = "OnDeleteReportObjectMockupError() jQuery Exception: response text= " + request.responseText + ", status= " + status + ", error= " + error;
        ClientSideLogging(errorText);
    }

    function OnDeleteReportObjectMockupSuccess(response, textStatus) {
        //debugger;

        if (textStatus == "success") {
            grid.jqGrid().trigger('reloadGrid');
        }
    }
})

// window resize event /////////////////////////////////////////////////////////////////////////////////////////////////////////////

function WindowReSize() {
    //debugger;

    // positioning log-in image
    var position = $("#gridReportObjectMockups").offset();
    var windowWidth = $(window).width();

    $("#message_bar").css("left", windowWidth / 2 - 70);
    $("#message_bar").css("top", position.top - 10);
}