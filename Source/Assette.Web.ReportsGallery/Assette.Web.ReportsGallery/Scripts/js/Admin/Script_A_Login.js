///////////////////////////////////////////////////////
//********** Assette Gallery       **********//
//********** Assette © 2013                **********//
///////////////////////////////////////////////////////

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

    // water mark
    $("#username").Watermark("USERNAME");
    $("#password").Watermark("PASSWORD");

    // key press - enter
    $(document).keypress(function (e) {
        if (e.which == 13) {
            //debugger;

            document.getElementById('btnLogin').click();
        }
    });
})

// window resize event /////////////////////////////////////////////////////////////////////////////////////////////////////////////

function WindowReSize() {
    //debugger;

    // positioning log-in image
    var position = $("#btnLogin").offset();
    var windowWidth = $(window).width();

    // message bar
    $("#message_bar").css("left", windowWidth / 2 - 805);
    $("#message_bar").css("top", position.top - 370);
}



