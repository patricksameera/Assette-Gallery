///////////////////////////////////////////////////////
//********** Assette Gallery       **********//
//********** Assette © 2013                **********//
///////////////////////////////////////////////////////

//debugger;

//samples
//https://github.com/valums/file-uploader
//http://svn.openstreetmap.org/sites/free-map.org.uk/otv/js/fileuploader.js?p=24624
//http://stackoverflow.com/questions/5349326/need-advice-on-ajax-fileupload

// document ready //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// jquery document ready function
$(document).ready(function () {
    //debugger;

    // window resize event
    WindowReSize();

    // window resize event
    $(window).resize(function () {

        // window resize event
        WindowReSize();
    });

    // go back button click
    $('#goBack').live("click", function (e) {
        //debugger;

        location.href = "edit-report-object-mockups.aspx";
    });

    // set selected tab link   
    $('#editsamplereports').css('background-color', '#999999');
    $('#edittemplatedesigns').css('background-color', '#999999');
    $('#clients').css('background-color', '#999999');
    $('#editreportobjects').css('background-color', '#999999');
    $('#editreportobjectmockups').css('background-color', '#C1D82F');
    $('#editobjectcategoryorder').css('background-color', '#999999');
    $('#editheadtagcontent').css('background-color', '#999999');

    // collapsible
    $("#fieldset1").collapse();
    $("#fieldset2").collapse();

    // uploader
    var uploader = new qq.FileUploader({
        element: document.getElementById('file-uploader-demo'),
        dragText: 'Drop files here to upload',
        action: "MockupObjectUploadHandler.aspx",
        uploadButtonText: '&nbsp;<i class="icon-plus icon-white"></i>&nbsp;Select A File',
        debug: false,
        //uploadButtonText: "Upload File ...",
        multiple: true,
        autoUpload: false,
        allowedExtensions: ['ppt', 'pptx'],
        sizeLimit: 2147483647,
        //params: { name: $('#txtName').val(), shortdescription: $('#txtLongDescription').val(), longdescription: $('#txtShortDescription').val(), product: $("#Product").val(), firm: $("#Firm").val() },
        onSubmit: uploaderOnSubmit,
        onProgress: uploaderOnProgress,
        onComplete: uploaderOnComplete,
        onCancel: uploaderOnCancel,
        onError: uploaderOnError

    });

    // upload button click
    $('#triggerUpload').live("click", function (e) {
        //debugger;

        uploader.uploadStoredFiles();
    });

    function uploaderOnSubmit(id, fileName) { }
    function uploaderOnProgress(id, fileName, loaded, total) { }
    function uploaderOnCancel(id, fileName) { }
    function uploaderOnComplete(id, fileName, responseJSON) {
        //debugger;

        ClearFields();
    }
    function uploaderOnError(id, fileName, xhr) {
        //debugger;

        // show error to user
        showErrorToUser("");

        WindowReSize();

        // browser logging
        if (typeof console != "undefined") {

            console.log("id: " + id);
            console.log("fileName: " + fileName);
            console.log("xhr:" + xhr);
        }
    }

})

// window resize event /////////////////////////////////////////////////////////////////////////////////////////////////////////////

function WindowReSize() {
    //debugger;

    // positioning log-in image
    var position = $("#triggerUpload").offset();
    var windowWidth = $(window).width();

    $("#message_bar").css("left", windowWidth / 2 - 112);
    $("#message_bar").css("top", position.top + 85);
}
