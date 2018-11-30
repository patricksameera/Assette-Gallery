var currentPosition;
var slideWidth;
var slides;
var numberOfSlides;
var noOfReports;
var sliderImageWidth;
var sliderImageHeight;

// show slider
function ShowSlideShowDialogReportObjectsForPreView(objectId, indexId, sliderContent, reportCount, reportIds, reportTableTypes) {
    //debugger;

    // get modal dialog diemensions
    var dimensionsObject = GetDialogDimensionsForReportObjects();

    // by default - dont show the dialog
    $("#dialog-modal").dialog({
        autoOpen: false,
        width: dimensionsObject.dialogWidth,
        height: dimensionsObject.dialogHeight,
        modal: true,
        show: 'fade',
        hide: 'fade',
        resizable: false
    });

    // report count
    noOfReports = reportCount;

    var completeSliderContent;
    var startSliderRows = [];
    var endSliderRows = [];

    // slider - start tags
    startSliderRows.push("<div id='slideshow'>");
    startSliderRows.push("<div id='slidesContainer'>");

    // slider - end tags
    endSliderRows.push("<div>");
    endSliderRows.push("<div>");

    // make div content string
    completeSliderContent = startSliderRows.join("") + sliderContent + endSliderRows.join("");

    // clear dialog content
    $("#dialog-modal").empty();

    // remove title bar
    $("#dialog-modal").dialog({
        open: function () {
            $(this).parents(".ui-dialog:first").find(".ui-dialog-titlebar").removeClass("ui-widget-header");
        }
    });

    // positioning
    $('#dialog-modal').dialog('option', 'position', 'center');

    // open dialog
    $("#dialog-modal").dialog("open");

    // setting modal dialog background color
    var overlay = $(".ui-widget-overlay");
    overlay.css("background", "#000").css("opacity", "0.5");

    // set dialog's content
    $("#dialog-modal").append(completeSliderContent);

    // slider properties
    currentPosition = parseInt(indexId);
    //slideWidth = 1200;
    slides = $('.slide');
    numberOfSlides = slides.length;

    // Remove scrollbar in JS
    $('#slidesContainer').css('overflow', 'hidden');

    // Wrap all .slides with #slideInner div
    slides.wrapAll('<div id="slideInner"></div>')

    // Float left to display horizontally, readjust .slides width
    .css({ 'float': 'left', 'width': slideWidth });

    // Set #slideInner width equal to total width of all slides
    $('#slideInner').css('width', slideWidth * numberOfSlides);

    // Insert controls in the DOM
    $('#slideshow')
    .before('<span title="Previous Report Object" class="control" id="leftControl">left</span>')
    .after('<span title="Next Report Object" class="control" id="rightControl">right</span>');

    // Hide left arrow control on first load
    ManageControlsPosition(currentPosition);

    // setting dimensions on dialog controls

    //slideshow
    $('#slideshow').css('width', dimensionsObject.sliderWidth);
    $('#slideshow').css('height', dimensionsObject.sliderHeight);

    //slidesContainer
    $('#slideshow #slidesContainer').css('width', dimensionsObject.sliderWidth);
    $('#slideshow #slidesContainer').css('height', dimensionsObject.sliderHeight);

    //control
    $('.control').css('width', dimensionsObject.navigationControlWidth);
    $('.control').css('height', dimensionsObject.sliderHeight);

    //lefter
    $('.lefter').css('width', dimensionsObject.sliderLeftWidth);
    $('.lefter').css('height', dimensionsObject.sliderLeftHeight);

    //center
    $('.center').css('width', dimensionsObject.sliderCenterWidth);
    $('.center').css('height', dimensionsObject.sliderLeftHeight);

    //righter
    $('.righter').css('width', dimensionsObject.sliderRightWidth);
    $('.righter').css('height', dimensionsObject.sliderLeftHeight);

    //top
    //$('.top').css('width', dimensionsObject.sliderRightTopWidth);
    $('.top').css('width', dimensionsObject.sliderRightWidth);
    $('.top').css('height', dimensionsObject.sliderRightTopHeight);

    //bellow
    $('.bellow').css('width', dimensionsObject.sliderRightBellowWidth);
    $('.bellow').css('height', dimensionsObject.sliderRightBellowHeight);

    //description
    $('.description').css('width', dimensionsObject.sliderRightDescriptionWidth);
    $('.description').css('height', dimensionsObject.sliderRightDescriptionHeight);

    // Create event listeners for .controls clicks
    $('.control').bind('click', function () {
        //debugger;

        // Determine new position
        currentPosition = ($(this).attr('id') == 'rightControl') ? currentPosition + 1 : currentPosition - 1;

        // get clicked object's id
        objectId = reportIds[currentPosition - 1];

        // get clicked object's table type
        tableType = reportTableTypes[currentPosition - 1];

        // get clicked object's index id
        indexId = currentPosition;

        // get report object
        GetPAObjectDetails(objectId, indexId, tableType);

    });

}

//// setting image size
//function SetImageSize() {
//    //debugger;

//    // setting modal dialog image width
//    var dimensionsObject = GetDialogDimensionsForReportObjects();

//    sliderImageWidth = dimensionsObject.sliderImageWidth;
//    sliderImageHeight = dimensionsObject.sliderImageHeight;

//    // get image size
//    var imageWidth = $(".urlImageLarge").width();
//    var imageHeight = $(".urlImageLarge").height();

//    var padding = 10;

//    // set image size
//    if (imageWidth == 800) {

//        $(".urlImageLarge").attr({ border: 1 }).height(sliderImageHeight).width(sliderImageWidth);
//    }
//    else {
//        //debugger;

//        if (imageWidth > sliderImageWidth || imageHeight > sliderImageHeight) {
//            var t = 1;

//            var p = imageWidth / sliderImageWidth;
//            var q = imageHeight / sliderImageHeight;

//            if (p > q) { t = p; } else { t = q }

//            var newImageWidth = parseInt(imageWidth / t);
//            var newImageHeight = parseInt(imageHeight / t);

//            var xAddition = parseInt(sliderImageWidth - newImageWidth);
//            var yAddition = parseInt(sliderImageHeight - newImageHeight);

//            $(".urlImageLarge").attr({ 'border': 1 }).height(newImageHeight).width(newImageWidth);

//            $(".urlImageLarge").css({
//                "padding-left": (xAddition + padding) / 2,
//                "padding-right": (xAddition + padding) / 2,
//                "padding-top": (yAddition + padding) / 2,
//                "padding-bottom": (yAddition + padding) / 2
//            });
//        }
//        else {
//            var widthDifference = parseInt(sliderImageWidth - imageWidth);
//            var heightDifference = parseInt(sliderImageHeight - imageHeight);

//            $(".urlImageLarge").attr({ 'border': 1 }).height(imageHeight).width(imageWidth);

//            $(".urlImageLarge").css({
//                "padding-left": (widthDifference + padding) / 2,
//                "padding-right": (widthDifference + padding) / 2,
//                "padding-top": (heightDifference + padding) / 2,
//                "padding-bottom": (heightDifference + padding) / 2
//            });
//        }
//    }

//    $(".urlImageLarge").css({ 'opacity': 1 });
//    $(".LoadingDiv").css({ 'border': 'none', 'background': 'none' });
//}