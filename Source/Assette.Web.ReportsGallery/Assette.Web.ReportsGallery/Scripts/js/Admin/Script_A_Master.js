///////////////////////////////////////////////////////
//********** Assette Gallery       **********//
//********** Assette © 2013                **********//
///////////////////////////////////////////////////////

//debugger;

// document ready //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// jquery document ready function
$(document).ready(function () {

    // client gallery button click
    $('#lnkClientGallery').live("click", function (e) {
        //debugger;

        window.open("../default.aspx", "newWindow", "");
    });

    // client gallery button click
    $('#lnkClientGalleryNewWindow').live("click", function (e) {
        //debugger;

        window.open("../default.aspx", "newWindow", "");
    });
})


