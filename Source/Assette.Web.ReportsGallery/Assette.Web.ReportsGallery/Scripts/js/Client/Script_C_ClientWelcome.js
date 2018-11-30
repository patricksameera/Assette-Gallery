///////////////////////////////////////////////////////
//********** Assette Gallery       **********//
//********** Assette © 2013                **********//
///////////////////////////////////////////////////////

//debugger;

// document ready //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// jquery document ready function
$(document).ready(function () {

    //debugger;

    // set description div height
    SetDivHeight();

    // window resize event
    $(window).resize(function () {
        // window resize event
        SetDivHeight();
    });
})

function SetDivHeight() {
    //debugger;

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
}

