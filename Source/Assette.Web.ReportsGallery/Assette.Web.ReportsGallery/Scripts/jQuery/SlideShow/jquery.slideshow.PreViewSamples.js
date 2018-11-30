var currentPosition;
var slideWidth;
var slides;
var numberOfSlides;
var noOfReports;

// show slider
function ShowSlideShowDialogForSamplesPreView(objectId, indexId, sliderContent, reportCount, reportIds) {
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
    startSliderRows.push("<div id='slideshowSamples'>");
    startSliderRows.push("<div id='slidesContainerSamples'>");

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
    slides = $('.slideSamples');
    numberOfSlides = slides.length;

    // Remove scrollbar in JS
    $('#slidesContainerSamples').css('overflow', 'hidden');

    // Wrap all .slides with #slideInner div
    slides.wrapAll('<div id="slideInner"></div>')

    // Float left to display horizontally, readjust .slides width
    .css({ 'float': 'left', 'width': slideWidth });

    // Set #slideInner width equal to total width of all slides
    $('#slideInner').css('width', slideWidth * numberOfSlides);

    // Insert controls in the DOM
    $('#slideshowSamples')
    .before('<span title="Previous Example" class="controlSamples" id="leftControl">left</span>')
    .after('<span title="Next Example" class="controlSamples" id="rightControl">right</span>');

    // Hide left arrow control on first load
    ManageControlsPosition(currentPosition);

    // setting dimensions on dialog controls

    //slideshowSamples
    $('#slideshowSamples').css('width', dimensionsObject.sliderWidth);
    $('#slideshowSamples').css('height', dimensionsObject.sliderHeight);

    //slidesContainerSamples
    $('#slideshowSamples #slidesContainerSamples').css('width', dimensionsObject.sliderWidth);
    $('#slideshowSamples #slidesContainerSamples').css('height', dimensionsObject.sliderHeight);

    //controlSamples
    $('.controlSamples').css('width', dimensionsObject.navigationControlWidth);
    $('.controlSamples').css('height', dimensionsObject.sliderHeight);

    //lefterSamples
    $('.lefterSamples').css('width', dimensionsObject.sliderLeftWidth);
    $('.lefterSamples').css('height', dimensionsObject.sliderLeftHeight);

    //centerSamples
    $('.centerSamples').css('width', dimensionsObject.sliderCenterWidth);
    $('.centerSamples').css('height', dimensionsObject.sliderLeftHeight);

    //righterSamples
    $('.righterSamples').css('width', dimensionsObject.sliderRightWidth);
    $('.righterSamples').css('height', dimensionsObject.sliderLeftHeight);

    //topSamples
    $('.topSamples').css('width', dimensionsObject.sliderRightTopWidth);
    $('.topSamples').css('height', dimensionsObject.sliderRightTopHeight);

    //bellowSamples
    $('.bellowSamples').css('width', dimensionsObject.sliderRightBellowWidth);
    $('.bellowSamples').css('height', dimensionsObject.sliderRightBellowHeight);

    //descriptionSamples
    $('.descriptionSamples').css('width', dimensionsObject.sliderRightDescriptionWidth);
    $('.descriptionSamples').css('height', dimensionsObject.sliderRightDescriptionHeight);

    // Create event listeners for .controls clicks
    $('.controlSamples').bind('click', function () {
        //debugger;

        // Determine new position
        currentPosition = ($(this).attr('id') == 'rightControl') ? currentPosition + 1 : currentPosition - 1;

        // get clicked object's id
        objectId = reportIds[currentPosition - 1];

        // get clicked object's index id
        indexId = currentPosition;

        // get sample reports
        GetSampleReportPages(objectId, indexId);

        // get report object
        GetSampleReportDetails(objectId, indexId);

        // setting div border
        $('.slideViewer').parent().css({ 'border': '1px solid #000000', 'padding-right': '5px', 'padding-bottom': '5px' });

    });

    // image slide show
    $("div#my-folio-of-works").slideViewerPro({
        thumbs: 5,
        autoslide: false,
        asTimer: 3500,
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

}
