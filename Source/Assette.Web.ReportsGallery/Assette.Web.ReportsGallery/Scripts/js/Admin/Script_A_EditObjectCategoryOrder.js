///////////////////////////////////////////////////////
//********** Assette Gallery       **********//
//********** Assette © 2013                **********//
///////////////////////////////////////////////////////

//debugger;

// document ready //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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

    // set selected tab link 
//    $('#editobjectcategoryorder').css('background-color', '#C1D82F');
//    $('#editreportobjects').css('background-color', '#999999');
//    $('#editsamplereports').css('background-color', '#999999');
//    $('#edittemplatedesigns').css('background-color', '#999999');
//    $('#clients').css('background-color', '#999999');
//    $('#editreportobjectmockups').css('background-color', '#999999');
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
    var grid = jQuery("#gridCategories");

    grid.jqGrid({
        colNames: ['ID', 'Category Id', 'Category', 'Sort Order', 'Is Visible'],
        colModel: [
            { name: 'Id', width: 50, align: 'left', sortable: true, hidden: true, editable: false, key: true, search: true, searchoptions: { sopt: ['eq'], size: 25} },
            { name: 'CategoryId', width: 30, align: 'left', sortable: false, hidden: false, editable: false, search: true, searchoptions: { sopt: ['eq'], size: 25} },
            { name: 'Category', width: 150, align: 'left', sortable: false, hidden: false, editable: false, search: true, searchoptions: { sopt: ['eq', 'cn'], size: 25 } },
            { name: 'SortOrder', width: 30, align: 'left', sortable: false, hidden: false, search: true, searchoptions: { sopt: ['eq'], size: 25 }, editable: true, editrules: { required: true, number: true }, editoptions: { size: 20, maxlength: 50} },
            { name: 'IsVisible', width: 30, align: 'left', sortable: false, hidden: false, search: true, stype: 'select', searchoptions: { value: "True:True;False:False", sopt: ['eq', 'ne']}, editable: true, edittype: 'checkbox', editoptions: { value: "True:False" } },
        ],
        rowNum: 15,
        rowList: [1, 15, 30, 50],
        pager: jQuery('#pagerCategories'),
        sortname: "Id",
        sortorder: "asc",
        caption: "Assette - Category Ordering",
        cellEdit: false,
        width: 885,
        height: 310,
        gridview: true,
        shrinkToFit: true,
        ignoreCase: true,
        rownumbers: true,
        editurl: "../Admin/edit-object-category-order.aspx/UpdateCategoryOrdering",
        viewrecords: true,
        recordtext: "View {0} - {1} of {2}",
        emptyrecords: "No records found.",
        loadtext: "Loading...",
        pgtext: "Page {0} of {1}",

        // get json data
        datatype: function (parameterData) {
            GetObjectCategoryData(parameterData);
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
    grid.jqGrid('navGrid', '#pagerCategories',
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
               height: '170',
               width: '400',

               // events
               onclickSubmit: EditObjectCategoryData
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
               closeAfterSubmit: true
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
            GetObjectCategoryData(parameterData);
        }
        }).trigger('reloadGrid');
    }

    // show report object data row on grid /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    function GetObjectCategoryData(parameterData) {
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
        var pageCount = $("#sp_1_pagerCategories").text();

        // if entered page number larger than actual page count
        if (pageCount == "" || parseInt(parameters.page) > parseInt(pageCount)) {
            parameters.page = 1;
        }

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../Admin/edit-object-category-order.aspx/GetPAObjectCategoryOrdering",
            data: JSON.stringify(parameters),
            dataType: "json",
            success: OnGetObjectCategoryDataSuccess,
            error: OnGetObjectCategoryDataError
        });
    }

    function OnGetObjectCategoryDataError(request, status, error) {
        //debugger;

        // browser loggin
        fireBugConsoleLog(request, status, error);

        // show error to user
        showErrorToUser("");

        // log4net logging
        var errorText = "OnGetObjectCategoryDataError() jQuery Exception: response text= " + request.responseText + ", status= " + status + ", error= " + error;
        ClientSideLogging(errorText);
    }

    function OnGetObjectCategoryDataSuccess(response, textStatus) {
        //debugger;

        if (textStatus == "success") {
            var thegrid = $("#gridCategories")[0];

            //var jsonData = JSON.parse(response.d);

            var jsonData = EscapeJsonBreakCharacters(response.d);
            jsonData = JSON.parse(jsonData);

            thegrid.addJSONData(jsonData);
        }
    }

    // edit sample report data /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    function EditObjectCategoryData(parameters, postData) {
        //debugger;

        var parameters = new Object();

        parameters.Id = postData.gridCategories_id;
        parameters.sortOrder = postData.SortOrder;
        parameters.isVisible = postData.IsVisible;

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../Admin/edit-object-category-order.aspx/UpdateCategoryOrdering",
            data: JSON.stringify(parameters),
            dataType: "json",
            success: OnEditObjectCategoryDataSuccess,
            error: OnEditObjectCategoryDataError
        });
    }

    function OnEditObjectCategoryDataError(request, status, error) {
        //debugger;

        // browser loggin
        fireBugConsoleLog(request, status, error);

        // show error to user
        showErrorToUser("");

        // log4net logging
        var errorText = "OnEditObjectCategoryDataError() jQuery Exception: response text= " + request.responseText + ", status= " + status + ", error= " + error;
        ClientSideLogging(errorText);
    }

    function OnEditObjectCategoryDataSuccess(response, textStatus) {
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
    var position = $("#gridCategories").offset();
    var windowWidth = $(window).width();

    $("#message_bar").css("left", windowWidth / 2 - 70);
    $("#message_bar").css("top", position.top - 10);
}