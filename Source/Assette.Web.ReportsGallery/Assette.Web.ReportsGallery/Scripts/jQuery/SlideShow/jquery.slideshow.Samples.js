var currentPosition;
var slideWidth;
var slides;
var numberOfSlides;
var noOfReports;

// show slider
function ShowSlideShowDialog(objectId, indexId, sliderContent, reportCount, reportIds) {
    //debugger;

    // get modal dialog diemensions
    var dimensionsObject = GetDialogDimensionsForSamplesAndDesigns();

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
    .before('<span title="Previous Example" class="control" id="leftControl">left</span>')
    .after('<span title="Next Example" class="control" id="rightControl">right</span>');

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
    $('.top').css('width', dimensionsObject.sliderRightTopWidth);
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

        // get clicked object's index id
        indexId = currentPosition;

        // get sample reports
        GetSampleReportPages(objectId, indexId);

        GetSampleReportDetails(objectId, indexId);

        // setting div border
        $('.slideViewer').parent().css({ 'border': '1px solid #000000', 'padding-right': '5px', 'padding-bottom': '5px' });
    });

    // image slide show
    $("div#my-folio-of-works").slideViewerPro({
        thumbs: 5,
        autoslide: false,
        asTimer: 1000,
        galBorderWidth: 0,
        thumbsBorderOpacity: 1,
        buttonsWidth: 40,
        thumbsActiveBorderOpacity: 1,
        thumbsActiveBorderColor: "#C1D82F",
        thumbsBorderColor: "#d8d8d8",
        leftButtonInner: "<img src='Images/SliderLeftArrow.jpg' />",
        rightButtonInner: "<img src='Images/SliderRightArrow.jpg' />",
        shuffle: false,
        thumbsTopMargin: 6,
        thumbsRightMargin: 6,
        thumbsLeftMargin: 6,
        thumbsBorderWidth: 3,
        thumbsPercentReduction: 10
    });

    // image zoom
    $('.zoom').zoom({ on: 'click' });

    // fancybox
    //$(".fancybox").fancybox({
    //  autoSize   : true
    //});

}