///////////////////////////////////////////////////////
//********** Assette Gallery       **********//
//********** Assette © 2013                **********//
///////////////////////////////////////////////////////

//debugger;

// document ready //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

var productTypeValues = '';
var firmTypeValues = '';

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

        location.href = "sample-report-uploader.aspx";
    });

    // get drop down values
    GetProductTypes();
    GetFirmTypes();

    // set selected tab link   
    $('#editsamplereports').css('background-color', '#C1D82F');
    $('#edittemplatedesigns').css('background-color', '#999999');
    $('#clients').css('background-color', '#999999');
    $('#editreportobjects').css('background-color', '#999999');
    $('#editreportobjectmockups').css('background-color', '#999999');
    $('#editobjectcategoryorder').css('background-color', '#999999');
    $('#editheadtagcontent').css('background-color', '#999999');

    //jqgrid functionality
    var grid = jQuery("#gridSamples");

    grid.jqGrid({
        colNames: ['Id', 'Name', 'Short Description', 'Long Description', 'Firm Type Id', 'Firm Type', 'Product Type Id', 'Product Type', 'Preview', 'File', 'File'],
        colModel: [
                      { name: 'Id', width: 100, align: 'left', sortable: true, hidden: false, search: true, searchoptions: { sopt: ['eq'], size: 25 }, editable: false, key: true },
                      { name: 'Name', width: 150, align: 'left', sortable: false, hidden: false, search: true, searchoptions: { sopt: ['eq', 'cn'], size: 25 }, editable: true, editrules: { required: true }, editoptions: { size: 90, maxlength: 1000} },
                      { name: 'ShortDescription', width: 150, align: 'left', sortable: false, hidden: true, viewable: true, search: false, searchoptions: { sopt: ['eq', 'cn'], size: 25 }, editable: true, edittype: 'textarea', editrules: { edithidden:true, required: true }, editoptions: { rows: '3', cols: '100'} },
                      { name: 'LongDescription', width: 100, align: 'left', sortable: false, hidden: true, viewable: true, search: false, searchoptions: { sopt: ['eq', 'cn'], size: 25 }, editable: true, edittype: 'textarea', editrules: { edithidden:true, required: true }, editoptions: { rows: '3', cols: '100'} },
                      { name: 'FirmTypeId', width: 100, align: 'left', sortable: false, hidden: true, search: false },
                      { name: 'FirmType', width: 100, align: 'left', sortable: false, hidden: false, search: true, editable: true, edittype: 'select', editoptions: { value: firmTypeValues }, stype: 'select', searchoptions: { value: firmTypeValues, sopt: ['eq', 'ne']} },
                      { name: 'ProductTypeId', width: 100, align: 'left', sortable: false, hidden: true, search: false },
                      { name: 'ProductType', width: 100, align: 'left', sortable: false, hidden: false, search: true, editable: true, edittype: 'select', editoptions: { value: productTypeValues }, stype: 'select', searchoptions: { value: productTypeValues, sopt: ['eq', 'ne']} },
                      { name: 'PreRegisterPreView', width: 50, align: 'left', sortable: false, hidden: false, search: false, editable: true, edittype: 'checkbox', search: true, editoptions: { value: "True:False" }, stype: 'select', searchoptions: { value: "True:True;False:False", sopt: ['eq', 'ne']} },
                      { name: 'UploadFile', index: 'UploadFile', align: 'left', search: false, hidden: true, editable: true, edittype: 'file', editrules: { edithidden: true }, editoptions: { enctype: 'multipart/form-data' }, editoptions: { size: 70} },
                      { name: 'Id', width: 100, align: 'center', sortable: false, formatter: 'showlink', formatter: downloadFileFormatter, search: false }
                    ],
        rowNum: 15,
        rowList: [1, 15, 30, 50],
        pager: jQuery('#pagerSamples'),
        sortname: "Id",
        sortorder: "asc",
        caption: "Assette - Sample Report Details",
        cellEdit: false,
        width: 885,
        height: 270,
        gridview: true,
        shrinkToFit: true,
        ignoreCase: true,
        rownumbers: true,
        editurl: "../Admin/edit-sample-reports.aspx/UpdateSampleReport",
        viewrecords: true,
        recordtext: "View {0} - {1} of {2}",
        emptyrecords: "No records found.",
        loadtext: "Loading...",
        pgtext: "Page {0} of {1}",

        // get json data
        datatype: function (parameterData) {
            GetSampleReportData(parameterData);
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
    grid.jqGrid('navGrid', '#pagerSamples',
    // options//////////////////////////////////////////////////////////////////////////////////////////////
               {
               // properties
               edit: true,
               view: false,
               add: false,
               del: true,
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
               afterComplete: GetUploadedFile,
               onclickSubmit: EditSampleReportData
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
               onclickSubmit: DeleteSampleReportData
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
            GetSampleReportData(parameterData);
        }
        }).trigger('reloadGrid');
    }

    // show sample data row on grid /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    function GetSampleReportData(parameterData) {
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
        var pageCount = $("#sp_1_pagerSamples").text();

        // if entered page number larger than actual page count
        if (pageCount == "" || parseInt(parameters.page) > parseInt(pageCount)) {
            parameters.page = 1;
        }

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../Admin/edit-sample-reports.aspx/GetSamples",
            data: JSON.stringify(parameters),
            dataType: "json",
            success: OnGetSampleReportDataSuccess,
            error: OnGetSampleReportDataError
        });
    }

    function OnGetSampleReportDataError(request, status, error) {
        //debugger;

        // browser loggin
        fireBugConsoleLog(request, status, error);

        // show error to user
        showErrorToUser("");

        // log4net logging
        var errorText = "OnGetSampleReportDataError() jQuery Exception: response text= " + request.responseText + ", status= " + status + ", error= " + error;
        ClientSideLogging(errorText);
    }

    function OnGetSampleReportDataSuccess(response, textStatus) {
        //debugger;

        if (textStatus == "success") {
            var thegrid = $("#gridSamples")[0];

            //var jsonData = JSON.parse(response.d);

            var jsonData = EscapeJsonBreakCharacters(response.d);
            jsonData = JSON.parse(jsonData);

            thegrid.addJSONData(jsonData);
        }
    }

    // edit sample report data /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    function EditSampleReportData(parameters, postData) {
        //debugger;

        var parameters = new Object();

        parameters.Id = postData.gridSamples_id;
        parameters.name = postData.Name;
        parameters.shortDescription = postData.ShortDescription;
        parameters.longDescription = postData.LongDescription;
        parameters.firmId = postData.FirmType;
        parameters.productId = postData.ProductType;
        parameters.preRegisterPreView = postData.PreRegisterPreView;

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../Admin/edit-sample-reports.aspx/UpdateSampleReport",
            data: JSON.stringify(parameters),
            dataType: "json",
            success: OnEditSampleReportDataSuccess,
            error: OnEditSampleReportDataError
        });
    }

    function OnEditSampleReportDataError(request, status, error) {
        //debugger;

        // browser loggin
        fireBugConsoleLog(request, status, error);

        // show error to user
        showErrorToUser("");

        // log4net logging
        var errorText = "OnEditSampleReportDataError() jQuery Exception: response text= " + request.responseText + ", status= " + status + ", error= " + error;
        ClientSideLogging(errorText);
    }

    function OnEditSampleReportDataSuccess(response, textStatus) {
        //debugger;

        if (textStatus == "success") {
            grid.jqGrid().trigger('reloadGrid');
        }
    }

    // delete sample report data /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    function DeleteSampleReportData(parameters, postData) {
        //debugger;

        var parameters = new Object();

        parameters.Id = postData;

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../Admin/edit-sample-reports.aspx/DeleteSampleReport",
            data: JSON.stringify(parameters),
            dataType: "json",
            success: OnDeleteSampleReportDataSuccess,
            error: OnDeleteSampleReportDataError
        });
    }

    function OnDeleteSampleReportDataError(request, status, error) {
        //debugger;

        // browser loggin
        fireBugConsoleLog(request, status, error);

        // show error to user
        showErrorToUser("");

        // log4net logging
        var errorText = "OnDeleteSampleReportDataError() jQuery Exception: response text= " + request.responseText + ", status= " + status + ", error= " + error;
        ClientSideLogging(errorText);
    }

    function OnDeleteSampleReportDataSuccess(response, textStatus) {
        //debugger;

        if (textStatus == "success") {
            grid.jqGrid().trigger('reloadGrid');
        }
    }

    // get uploaded file data /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    function GetUploadedFile(parameters, postData) {
        //debugger;

        if ($("#UploadFile").val() != "") {

            var ext = $('#UploadFile').val().split('.').pop().toLowerCase();

            if ($.inArray(ext, ['ppt', 'pptx']) == -1) {
                alert('Invalid extension. Valid extinsion(s): ppt, pptx.');
            }
            else {
                ajaxFileUpload(postData.id);
            }
        }
    }

    function ajaxFileUpload(id) {
        //debugger;

        $.ajaxFileUpload
         (
             {
                 url: 'SampleReportEditHandler.aspx',
                 secureuri: false,
                 fileElementId: 'UploadFile',
                 data: { 'id': id },
                 beforeSend: function () {

                 },
                 success: function (data, status) {
                     if (typeof (data.error) != 'undefined') {
                         if (data.error != '') {
                             alert(data.error);
                         } else {
                             alert(data.msg);
                         }
                     }

                     if (data.success) {

                     }
                 },
                 error: function (data, status, e) {
                     //debugger;

                     alert(e);
                 }
             }
         )

        return false;
    }

    function OnGetUploadedFileError(request, status, error) {
        //debugger;

        // browser loggin
        fireBugConsoleLog(request, status, error);

        // show error to user
        showErrorToUser("");

        // log4net logging
        var errorText = "OnGetUploadedFileError() jQuery Exception: response text= " + request.responseText + ", status= " + status + ", error= " + error;
        ClientSideLogging(errorText);
    }

    function OnGetUploadedFileSuccess(response, textStatus) {
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


    // get product types details /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    function GetProductTypes() {
        //debugger;

        $.ajax({
            type: "POST",
            url: "../Admin/edit-sample-reports.aspx/GetProductTypes",
            data: "",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
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

    function OnGetProductTypesSuccess(data) {
        //debugger;

        var response = jQuery.parseJSON(data.d);

        var rtnValue = "";

        if (response && response.length) {
            for (var i = 0, l = response.length; i < l; i++) {
                var item = response[i];

                if (rtnValue == '') {
                    rtnValue = item.ID + ":" + item.Name;
                }
                else {
                    rtnValue += ";" + item.ID + ":" + item.Name;
                }
            }
        }

        productTypeValues = rtnValue;
    }

    // get firm types details /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    function GetFirmTypes() {
        //debugger;

        $.ajax({
            type: "POST",
            url: "../Admin/edit-sample-reports.aspx/GetFirmTypes",
            data: "",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
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

    function OnGetFirmTypesSuccess(data) {
        //debugger;

        var response = jQuery.parseJSON(data.d);

        var rtnValue = "";

        if (response && response.length) {
            for (var i = 0, l = response.length; i < l; i++) {
                var item = response[i];

                if (rtnValue == '') {
                    rtnValue = item.ID + ":" + item.Name;
                }
                else {
                    rtnValue += ";" + item.ID + ":" + item.Name;
                }
            }
        }

        firmTypeValues = rtnValue;
    }

    function downloadFileFormatter(cellvalue, options, rowObject) {
        //debugger;

        return "<a href='../Admin/FileDownloadHandler.aspx?id=" + rowObject[0] + "&type=s'><u>Download</u></a>";
    }

})

// window resize event /////////////////////////////////////////////////////////////////////////////////////////////////////////////

function WindowReSize() {
    //debugger;

    // positioning log-in image
    var position = $("#triggerUpload").offset();
    var windowWidth = $(window).width();

    $("#message_bar").css("left", windowWidth / 2 - 70);
    $("#message_bar").css("top", position.top - 10);
}