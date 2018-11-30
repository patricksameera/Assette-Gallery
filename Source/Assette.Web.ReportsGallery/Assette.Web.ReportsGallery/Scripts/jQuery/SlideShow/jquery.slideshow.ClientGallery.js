var currentPosition;
var slideWidth;
var slides;
var numberOfSlides;
var noOfReports;

$(document).ready(function () {

    // by default - dont show the dialog
    $("#dialog-modal").dialog({
        autoOpen: false,
        width: 800,
        height: 540,
        modal: true,
        show: 'fade',
        hide: 'fade',
        resizable: false
    });

});

// show slider
function ShowSlideShowDialog(objectId, indexId, sliderContent, reportCount, reportIds, galleryReportIds, galleryReportIdTableTypes) {
    //debugger;

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

    // open dialog
    $("#dialog-modal").dialog("open");

    // set dialog's content
    $("#dialog-modal").append(completeSliderContent);

    //notes scroll bar
    $("#scrollbar").mCustomScrollbar({
        scrollButtons: {
            enable: true
        }
    });

    // slider properties
    currentPosition = parseInt(indexId);
    slideWidth = 700;
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
    manageControls(currentPosition);

    // Create event listeners for .controls clicks
    $('.control').bind('click', function () {
        //debugger;

        // Determine new position
        currentPosition = ($(this).attr('id') == 'rightControl') ? currentPosition + 1 : currentPosition - 1;

        // get clicked object's id
        objectId = reportIds[currentPosition - 1];
        galleryReportId = galleryReportIds[currentPosition - 1];
        tableType = galleryReportIdTableTypes[currentPosition - 1];

        // get clicked object's index id
        indexId = currentPosition;

        // get notes
        GetNotes(galleryReportId);

        setTimeout(function () {
            //debugger;
            // get report object
            GetMyGalleryPAObjectDetails(objectId, indexId, galleryReportId, tableType);
        }, 1000);

        // get report object
        //GetMyGalleryPAObjectDetails(objectId, indexId, galleryReportId);

    });

}

// manageControls: Hides and Shows controls depending on currentPosition
function manageControls(position) {
    //debugger;

    // Hide left arrow if position is first slide
    if (position == 1) {
        $('#leftControl').hide()
    }
    else {
        $('#leftControl').show()
    }

    // Hide right arrow if position is last slide
    if (position == noOfReports) {
        $('#rightControl').hide()
    }
    else {
        $('#rightControl').show()
    }
}