///////////////////////////////////////////////////////
//********** Assette Gallery       **********//
//********** Assette © 2013                **********//
///////////////////////////////////////////////////////

//debugger;

// document ready //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// jquery document ready function
$(document).ready(function () {

    // copy direct url copy button click
    $('#directUrlCopy').live("click", function (e) {
        //debugger;

        var href = $(this).attr('dirrectUrl');

        window.prompt("Copy to clipboard: press Ctrl+C, Enter", href);
    });

    // button click
    $('#triggerCreateClient').live("click", function (e) {
        //debugger;

        location.href = "create-user.aspx";
    });

    /*
    //$(this).zclip({
    $("#copy-button").zclip({
    //path: "http://www.steamdev.com/zclip/js/ZeroClipboard.swf",
    path: "../Script/jQuery/ClipBoard/ZeroClipboard.swf",
    copy: function () {
    return $("#copy").text();
    }
    });
    */

    // window events

    // window resize event
    WindowReSize();

    // window resize event
    $(window).resize(function () {

        // window resize event
        WindowReSize();
    });

    // set selected tab link   
    $('#clients').css('background-color', '#C1D82F');
    $('#samplereportsuploader').css('background-color', '#999999');
    $('#templatedesignuploader').css('background-color', '#999999');
    $('#editreportobjects').css('background-color', '#999999');
    $('#editreportobjectmockups').css('background-color', '#999999');
    $('#editobjectcategoryorder').css('background-color', '#999999');

    var grid = jQuery("#gridClients");

    grid.jqGrid({
        colNames: ['Id', 'First Name', 'Last Name', 'Job Title', 'Email', 'Company', 'Registered IP', 'Registered Date', 'Client Gallery', 'Direct URL'],
        colModel: [
                      { name: 'Id', width: 100, align: 'left', sortable: true, hidden: true, search: false },
                      { name: 'FirstName', width: 150, align: 'left', sortable: false, search: true, searchoptions: { sopt: ['eq', 'cn']} },
                      { name: 'LastName', width: 150, align: 'left', sortable: false, search: true, searchoptions: { sopt: ['eq', 'cn']} },
                      { name: 'JobTitle', width: 150, align: 'left', sortable: false, search: true, searchoptions: { sopt: ['eq', 'cn']} },
                      { name: 'Email', width: 100, align: 'left', sortable: false, search: true, searchoptions: { sopt: ['eq', 'cn']} },
                      { name: 'Company', width: 100, align: 'left', sortable: false, search: true, searchoptions: { sopt: ['eq', 'cn']} },
                      { name: 'RegisteredIP', width: 100, align: 'left', sortable: false, search: false },
                      { name: 'RegisteredDate', width: 100, align: 'left', sortable: true, sorttype: 'date', formatter: 'date', formatoptions: { srcformat: 'm/d/y', newformat: 'm/d/Y' }, search: false },
                      //{ name: 'RegisteredDate', width: 100, align: 'left', sortable: true, sorttype: 'date', search: false },
                      { name: 'ObjectCount', width: 100, align: 'center', sortable: false, formatter: 'showlink', formatter: objectCountFomatter, search: false },
                      { name: 'Id', width: 100, align: 'center', sortable: false, formatter: 'showlink', formatter: directUrlFomatter, search: false }
                    ],
        rowNum: 15,
        rowList: [1, 15, 30, 50],
        pager: jQuery('#pagerClients'),
        sortname: 'Id',
        sortorder: "desc",
        caption: "Assette - Prospect Details",
        cellEdit: false,
        width: 885,
        height: 310,
        gridview: true,
        shrinkToFit: true,
        ignoreCase: true,
        rownumbers: true,

        viewrecords: true,
        recordtext: "View {0} - {1} of {2}",
        emptyrecords: "No records found.",
        loadtext: "Loading...",
        pgtext: "Page {0} of {1}",

        gridComplete: LoadComplete,

        // get json data
        datatype: function (parameterData) {
            GetClientData(parameterData);
        },

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

    grid.jqGrid('navGrid', '#pagerClients',
               { edit: false, view: false, add: false, del: false, search: true, refresh: true,
                   beforeRefresh: function () {
                       grid.jqGrid('setGridParam', { datatype: function (parameterData) {
                           GetClientData(parameterData);
                       }
                       }).trigger('reloadGrid');
                   }
               },
               {},
               {},
               {},
               { closeAfterSearch: true, closeAfterReset: true, closeOnEscape: true, multipleSearch: false }
               );

    function GetClientData(parameterData) {
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
        var pageCount = $("#sp_1_pagerClients").text();

        // if entered page number larger than actual page count
        if (pageCount == "" || parseInt(parameters.page) > parseInt(pageCount)) {
            parameters.page = 1;
        }

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../Admin/Clients.aspx/GetClients",
            data: JSON.stringify(parameters),
            dataType: "json",
            success: OnGetClientDataSuccess,
            error: OnGetClientDataError
        });
    }

    function OnGetClientDataError(request, status, error) {
        //debugger;

        // browser loggin
        fireBugConsoleLog(request, status, error);

        // show error to user
        showErrorToUser("");

        WindowReSize();

        // log4net logging
        var errorText = "OnGetClientDataError() jQuery Exception: response text= " + request.responseText + ", status= " + status + ", error= " + error;
        ClientSideLogging(errorText);
    }

    function OnGetClientDataSuccess(response, textStatus) {
        //debugger;

        if (textStatus == "success") {
            var thegrid = $("#gridClients")[0];
            var jsonData = JSON.parse(response.d);
            thegrid.addJSONData(jsonData);
        }
    }

    function objectCountFomatter(cellvalue, options, rowObject) {
        //debugger;

        if (cellvalue == "0") {
            return "-";
        }
        else {
            return "<a href='../Admin/client-gallery.aspx?userid=" + rowObject[0] + "'><u>" + cellvalue + " Objects</u></a>";
        }
    }

    function directUrlFomatter(cellvalue, options, rowObject) {
        //debugger;

        var url = $("[id$=hdnUrl]").val();
        var dirrectUrl = url + "/client-welcome.aspx?userid=" + rowObject[0];

        return "<a id='directUrlCopy' dirrectUrl='" + dirrectUrl + "' href='javascript:void(0);'><u>Copy URL</u></a>";
    }

    function LoadComplete() {

    }

    // sample
    //{"total":5,"page":1,"records":47,"rows":[{"i":0,"cell":["9db67f26-92f7-4e90-93a9-5e0317265068","Sameera","Jayalath","sameera.jayalath@assette.com","Assette","192.168.0.150","11/2/2012 8:57:38 AM","0"]},{"i":1,"cell":["d371ba9d-b5bb-4229-bd51-a48f8cb3449f","...
    //{"total":5,"page":1,"records":47,"rows":[]}

})

// window resize event /////////////////////////////////////////////////////////////////////////////////////////////////////////////

function WindowReSize() {
    //debugger;

    // positioning log-in image
    var position = $("#gridClients").offset();
    var windowWidth = $(window).width();

    $("#message_bar").css("left", windowWidth / 2 - 70);
    $("#message_bar").css("top", position.top - 97);
}