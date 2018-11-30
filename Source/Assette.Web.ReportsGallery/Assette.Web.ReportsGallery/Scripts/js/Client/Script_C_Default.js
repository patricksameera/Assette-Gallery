///////////////////////////////////////////////////////
//********** Assette Gallery       **********//
//********** Assette © 2013                **********//
///////////////////////////////////////////////////////

//debugger;

// document ready //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


// jquery document ready function
$(document).ready(function () {
    //debugger;

    // set dimensions
    WindowResizeFunction();

    // window resize event
    $(window).resize(function () {

        WindowResizeFunction();
    });

    // register button click
    $('#lnkRegister').click(function () {
        //debugger;

        location.href = "login.aspx";
    });
})

function WindowResizeFunction() {
    //debugger;

    // resizing description divs
    var $divs = $(".r5");

    $divs.css("height", "100%");

    var height = $divs[0].clientHeight;

    if (parseInt($divs[1].clientHeight) > parseInt(height)) {
        height = $divs[1].clientHeight;
    }

    if (parseInt($divs[2].clientHeight) > parseInt(height)) {
        height = $divs[2].clientHeight;
    }

    $(".r5").height(height);

    // positioning message bar
    var position = $("#AssetteImage").offset();
    var windowWidth = $(window).width();

    if (position != null) {

        $("#message_bar").css("left", windowWidth / 2 - 213);
        $("#message_bar").css("top", position.top + 97);
    }
}