/*!
* slideViewerPro 1.5
* Examples and documentation at: 
* http://www.gcmingati.net/wordpress/wp-content/lab/jquery/svwt/
* 2009-2011 Gian Carlo Mingati
* Version: 1.5.0 (02-AUG-2011)
* Dual licensed under the MIT and GPL licenses:
* http://www.opensource.org/licenses/mit-license.php
* http://www.gnu.org/licenses/gpl.html
* Requires:
* jQuery v1.6 or later
* Option:
* jQuery Timers plugin | plugins.jquery.com/project/timers (for autoslide mode)
* 
*/
jQuery.extend(jQuery.easing, // from the jquery.easing plugin
{
easeInOutExpo: function (x, t, b, c, d) {
    if (t == 0) return b;
    if (t == d) return b + c;
    if ((t /= d / 2) < 1) return c / 2 * Math.pow(2, 10 * (t - 1)) + b;
    return c / 2 * (-Math.pow(2, -10 * --t) + 2) + b;
}
});
jQuery(function () {
    jQuery("div.svwp").prepend("<img src='images/ImageSliderLoader.gif' class='ldrgif' alt='loading...'/ >"); //change with YOUR loader image path   
});
var j = 0;
jQuery.fn.slideViewerPro = function (settings) {
    settings = jQuery.extend({
        galBorderWidth: 6,
        thumbsTopMargin: 3,
        thumbsRightMargin: 3,
        thumbsBorderWidth: 3,
        buttonsWidth: 20,
        galBorderColor: "#ff0000",
        thumbsBorderColor: "#d8d8d8",
        thumbsActiveBorderColor: "#ff0000",
        buttonsTextColor: "#ff0000",
        thumbsBorderOpacity: 1.0, // could be 0, 0.1 up to 1.0
        thumbsActiveBorderOpacity: 1.0, // could be 0, 0.1 up to 1.0
        easeTime: 750,
        asTimer: 4000,
        thumbs: 4,
        thumbsPercentReduction: 12,
        thumbsVis: true,
        easeFunc: "easeInOutExpo",
        leftButtonInner: "-", //could be an image "<img src='images/larw.gif' />" or an escaped char as "&larr";
        rightButtonInner: "+", //could be an image or an escaped char as "&rarr";
        autoslide: false,
        typo: false,
        typoFullOpacity: 0.9,
        shuffle: false
    }, settings);

    return this.each(function () {
        function shuffle(a) {
            var i = a.size();
            while (--i) {
                var j = Math.floor(Math.random() * (i));
                var tmp = a.slice(i, i + 1);
                a.slice(j, j + 1).insertAfter(tmp);
            }
        }
        var container = jQuery(this);
        (!settings.shuffle) ? null : shuffle(container.find("li"));
        container.find("img.ldrgif").remove();
        container.removeClass("svwp").addClass("slideViewer");
        container.attr("id", "svwp" + j);
        /**
        var pictWidth = container.find("img").width(); 
        Reads the width of the first (and only) IMG height. 
        That's why all images in the gal must have the same width/height
        */
        var pictWidth = container.find("img").width();
        var pictHeight = container.find("img").height();
        var pictEls = container.find("li").size();
        (pictEls >= settings.thumbs) ? null : settings.thumbs = pictEls;
        var slideViewerWidth = pictWidth * pictEls;
        var thumbsWidth = Math.round(pictWidth * settings.thumbsPercentReduction / 100);
        var thumbsHeight = Math.round(pictHeight * settings.thumbsPercentReduction / 100);
        /*var thumbsWidth = 50;
        var thumbsHeight = 50;*/
        var pos = 0;
        var posThumbList = 0;
        var r_enabled = true;
        var l_enabled = true;
        var leftButtonFirst = "<img src='Images/SliderLeftArrowFirst.jpg' />";
        var rightButtonLast = "<img src='Images/SliderRightArrowLast.jpg' />";

        container.find("ul").css("width", slideViewerWidth)
        .wrap(jQuery("<div style='width:" + pictWidth + "px; overflow: hidden; position: relative; top: 0; left: 0'>"));
        container.css("width", pictWidth);
        container.css("height", pictHeight);
        container.each(function (i) {
            if (settings.typo) {
                jQuery(this).find("img").each(function (z) {
                    jQuery(this).after("<span class='typo' style='position: absolute; width:" + (pictWidth - 12) + "px; margin: 0 0 0 -" + pictWidth + "px'>" + jQuery(this).attr("alt") + "<\/span>");
                });
            }

            jQuery(this).after("<div class='thumbSlider' id='thumbSlider" + j + "'><ul><\/ul><\/div>");

            jQuery(this).next().after("<a href='#' class='left' id='leftEnd" + j + "'><span title='View first page'>" + leftButtonFirst + "</span><\/a><a href='#' class='left' id='left" + j + "'><span title='View previous (page 1 of " + pictEls + ")'>" + settings.leftButtonInner + "</span><\/a><a href='#' class='right' id='right" + j + "'><span title='View next (page 2 of " + pictEls + ")'>" + settings.rightButtonInner + "<\/span><\/a><a href='#' class='right' id='rightEnd" + j + "'><span title='View last page'>" + rightButtonLast + "<\/span><\/a>");

            jQuery(this).find("li").each(function (n) {
                jQuery("div#thumbSlider" + j + " ul").append("<li><a title='Page " + jQuery(this).find("img").attr("alt") + " of " + pictEls + "' href='#'><img width='" + thumbsWidth + "' height='" + thumbsHeight + "' src='" + jQuery(this).find("img").attr("src") + "' /><p class='tmbrdr'>&nbsp;<\/p><\/a><\/li>");
            });


            // thumbnail click //////////////////////////////////////////////////////////////////////////////////////////

            jQuery("div#thumbSlider" + j + " a").each(function (z) {
                jQuery(this).bind("click", function () {
                    //debugger;

                    // setting position
                    pos = z;

                    // setting selected thumbnail
                    jQuery(this).find("p.tmbrdr").css({ borderColor: settings.thumbsActiveBorderColor, opacity: settings.thumbsActiveBorderOpacity });
                    jQuery(this).parent().parent().find("p.tmbrdr").not(jQuery(this).find("p.tmbrdr")).css({ borderColor: settings.thumbsBorderColor, opacity: settings.thumbsBorderOpacity });

                    // setting image
                    var cnt = -(pictWidth * z);
                    (cnt != container.find("ul").css("left").replace(/px/, "")) ? container.find("span.typo").animate({ "opacity": 0 }, 250) : null;
                    container.find("ul").animate({ left: cnt }, settings.easeTime, settings.easeFunc, function () { container.find("span.typo").animate({ "opacity": settings.typoFullOpacity }, 250) });

                    // setting next/previous button visibility
                    authorityMixing();

                    return false;
                });
            });


            // navigation button functionality ///////////////////////////////////////////////////////////////////////////

            var jQuerybtl = jQuery("a#left" + j);
            var jQuerybtr = jQuery("a#right" + j);

            var jQuerybtlEnd = jQuery("a#leftEnd" + j);
            var jQuerybtrEnd = jQuery("a#rightEnd" + j);


            // right button click /////////////////////////////////////////////////////////////////////////////////////////

            jQuerybtr.bind("click", function () {
                //debugger;

                if (pos == pictEls - 1) {

                    r_enabled = false;
                    l_enabled = true;

                    jQuerybtr.addClass("r_dis");
                }
                else {

                    // setting position
                    pos = pos + 1;

                    // setting image
                    var cnt = -(pictWidth * pos);
                    (cnt != container.find("ul").css("left").replace(/px/, "")) ? container.find("span.typo").animate({ "opacity": 0 }, 250) : null;
                    container.find("ul").animate({ left: cnt }, settings.easeTime, settings.easeFunc, function () { container.find("span.typo").animate({ "opacity": settings.typoFullOpacity }, 250) });

                    // setting thumbnail
                    var count_thumbs_page_list = 0;
                    var count_thumbs = parseInt(pos / settings.thumbs);
                    var count_thumbs_remainder = (pos % settings.thumbs);

                    if (count_thumbs == 0) {
                        count_thumbs_page = 0;
                    }
                    else {
                        count_thumbs_page = count_thumbs;
                    }

                    // setting the page
                    if (r_enabled && count_thumbs_page > 0) {
                        //debugger;

                        count_thumbs_page_list = count_thumbs_page * settings.thumbs;

                        if ((count_thumbs_page + 1) * settings.thumbs > pictEls) {
                            count_thumbs_page_list = pictEls - settings.thumbs;
                        }

                        r_enabled = false;

                        jQuery(this).prev().prev().prev().find("ul:not(:animated)").animate({ left: -(thumbsWidth + settings.thumbsRightMargin) * count_thumbs_page_list });
                    }

                    // setting next/previous button visibility
                    authorityMixing();

                    // setting navigation button text
                    settingNavigationButtonText();

                    // setting selected thumbnail
                    var index = j - 1;
                    var $thumbSlider = jQuery("div#thumbSlider" + index);

                    // setting selected thumbnail
                    $thumbSlider.find("p.tmbrdr").eq(pos).css({ borderColor: settings.thumbsActiveBorderColor, opacity: settings.thumbsActiveBorderOpacity });
                    $thumbSlider.find("p.tmbrdr").eq(pos - 1).css({ borderColor: settings.thumbsBorderColor, opacity: settings.thumbsBorderOpacity });

                    return false;

                }
            });


            // left button click /////////////////////////////////////////////////////////////////////////////////////////

            jQuerybtl.bind("click", function () {
                //debugger;

                if (pos == 0) {

                    r_enabled = true;
                    l_enabled = false;

                    jQuerybtl.addClass("l_dis");
                }
                else {

                    // setting position
                    pos = pos - 1;

                    // setting image
                    var cnt = -(pictWidth * pos);
                    (cnt != container.find("ul").css("left").replace(/px/, "")) ? container.find("span.typo").animate({ "opacity": 0 }, 250) : null;
                    container.find("ul").animate({ left: cnt }, settings.easeTime, settings.easeFunc, function () { container.find("span.typo").animate({ "opacity": settings.typoFullOpacity }, 250) });

                    // setting thumbnail
                    var count_thumbs_page_list = 0;
                    var count_thumbs = parseInt(pos / settings.thumbs);
                    var count_thumbs_remainder = (pos % settings.thumbs);

                    if (count_thumbs == 0) {
                        count_thumbs_page = 0;
                    }
                    else {
                        count_thumbs_page = count_thumbs;
                    }

                    // setting the page
                    if (r_enabled) {
                        //debugger;

                        count_thumbs_page_list = count_thumbs_page * settings.thumbs;

                        if ((count_thumbs_page + 1) * settings.thumbs > pictEls) {
                            count_thumbs_page_list = pictEls - settings.thumbs;
                        }

                        r_enabled = false;

                        jQuery(this).prev().prev().find("ul:not(:animated)").animate({ left: -(thumbsWidth + settings.thumbsRightMargin) * count_thumbs_page_list });
                    }

                    // setting next/previous button visibility
                    authorityMixing();

                    // setting navigation button text
                    settingNavigationButtonText();

                    // setting selected thumbnail
                    var index = j - 1;
                    var $thumbSlider = jQuery("div#thumbSlider" + index);

                    // setting selected thumbnail
                    $thumbSlider.find("p.tmbrdr").eq(pos).css({ borderColor: settings.thumbsActiveBorderColor, opacity: settings.thumbsActiveBorderOpacity });
                    $thumbSlider.find("p.tmbrdr").eq(pos + 1).css({ borderColor: settings.thumbsBorderColor, opacity: settings.thumbsBorderOpacity });

                    return false;
                }
            });


            // first button click /////////////////////////////////////////////////////////////////////////////////////////

            jQuerybtlEnd.bind("click", function () {
                //debugger;

                // setting position
                pos = 0;

                // setting next/previous button visibility
                authorityMixing();

                // setting image
                var cnt = -(pictWidth * pos);
                (cnt != container.find("ul").css("left").replace(/px/, "")) ? container.find("span.typo").animate({ "opacity": 0 }, 250) : null;
                container.find("ul").animate({ left: cnt }, settings.easeTime, settings.easeFunc, function () { container.find("span.typo").animate({ "opacity": settings.typoFullOpacity }, 250) });

                // setting thumbnail
                var count_thumbs_page_list = 0;
                var count_thumbs = parseInt(pos / settings.thumbs);
                var count_thumbs_remainder = (pos % settings.thumbs);

                if (count_thumbs == 0) {
                    count_thumbs_page = 0;
                }
                else {
                    count_thumbs_page = count_thumbs;
                }

                jQuery(this).prev().find("ul:not(:animated)").animate({ left: -(thumbsWidth + settings.thumbsRightMargin) * count_thumbs_page_list });

                // setting selected thumbnail
                var index = j - 1;
                var $thumbSlider = jQuery("div#thumbSlider" + index);

                $thumbSlider.find("p.tmbrdr").each(function (z) {
                    $thumbSlider.find("p.tmbrdr").eq(z).css({ borderColor: settings.thumbsBorderColor, opacity: settings.thumbsBorderOpacity });
                });

                $thumbSlider.find("p.tmbrdr").eq(pos).css({ borderColor: settings.thumbsActiveBorderColor, opacity: settings.thumbsActiveBorderOpacity });

                // setting navigation button text
                settingNavigationButtonText();

                return false;
            });


            // last button click /////////////////////////////////////////////////////////////////////////////////////////

            jQuerybtrEnd.bind("click", function () {
                //debugger;

                // setting position
                pos = pictEls - 1;

                // setting next/previous button visibility
                authorityMixing();

                // setting image
                var cnt = -(pictWidth * pos);
                (cnt != container.find("ul").css("left").replace(/px/, "")) ? container.find("span.typo").animate({ "opacity": 0 }, 250) : null;
                container.find("ul").animate({ left: cnt }, settings.easeTime, settings.easeFunc, function () { container.find("span.typo").animate({ "opacity": settings.typoFullOpacity }, 250) });

                // setting thumbnail
                var count_thumbs_page_list = 0;
                var count_thumbs = parseInt(pos / settings.thumbs);
                var count_thumbs_remainder = (pos % settings.thumbs);

                if (count_thumbs == 0) {
                    count_thumbs_page = 0;
                }
                else {
                    count_thumbs_page = count_thumbs;
                }

                // setting the page
                count_thumbs_page_list = count_thumbs_page * settings.thumbs;

                if ((count_thumbs_page + 1) * settings.thumbs > pictEls) {
                    count_thumbs_page_list = pictEls - settings.thumbs;
                }

                jQuery(this).prev().prev().prev().prev().find("ul:not(:animated)").animate({ left: -(thumbsWidth + settings.thumbsRightMargin) * count_thumbs_page_list });

                // setting selected thumbnail
                var index = j - 1;
                var $thumbSlider = jQuery("div#thumbSlider" + index);

                $thumbSlider.find("p.tmbrdr").each(function (z) {
                    $thumbSlider.find("p.tmbrdr").eq(z).css({ borderColor: settings.thumbsBorderColor, opacity: settings.thumbsBorderOpacity });
                });

                $thumbSlider.find("p.tmbrdr").eq(pos).css({ borderColor: settings.thumbsActiveBorderColor, opacity: settings.thumbsActiveBorderOpacity });

                // setting navigation button text
                settingNavigationButtonText();

                return false;
            });


            // setting button visibility /////////////////////////////////////////////////////////////////////////////////

            function authorityMixing() {
                //debugger;

                if (pos == 0) {
                    r_enabled = true;
                    l_enabled = false;

                    jQuerybtl.addClass("l_dis");
                    jQuerybtlEnd.addClass("r_dis");

                    jQuerybtr.removeClass("r_dis");
                    jQuerybtrEnd.removeClass("r_dis")
                }
                else if (pos == pictEls - 1) {
                    r_enabled = false;
                    l_enabled = true;

                    jQuerybtl.removeClass("l_dis");
                    jQuerybtlEnd.removeClass("r_dis");

                    jQuerybtr.addClass("r_dis");
                    jQuerybtrEnd.addClass("r_dis")
                }
                else {
                    r_enabled = true;
                    l_enabled = true;

                    jQuerybtl.removeClass("l_dis");
                    jQuerybtlEnd.removeClass("r_dis");

                    jQuerybtr.removeClass("r_dis");
                    jQuerybtrEnd.removeClass("r_dis")
                }
            }

            function settingNavigationButtonText() {
                //debugger;

                var slidePosition = parseInt(pos + 1);

                jQuerybtl.find("img").attr('title', 'View previous (page ' + parseInt(slidePosition - 1) + ' of ' + pictEls + ')');
                jQuerybtr.find("img").attr('title', 'View next (page ' + parseInt(slidePosition + 1) + ' of ' + pictEls + ')');
            }

            //CSS ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //debugger;

            var tBorder = settings.thumbsBorderWidth;
            var contBorder = settings.galBorderWidth

            jQuery(".slideViewer a img").css({ border: "0" });
            if (settings.typo) {
                jQuery(this).find("span.typo").each(function (z) {
                    jQuery(this).css({ marginTop: (pictHeight - jQuery(this).innerHeight()), opacity: settings.typoFullOpacity });
                });
            }
            jQuery("div#svwp" + j).css({ border: settings.galBorderWidth + "px solid " + settings.galBorderColor });

            jQuery("div#thumbSlider" + j).css({ position: "relative", left: contBorder, top: settings.thumbsTopMargin + "px", width: settings.thumbs * thumbsWidth + ((settings.thumbsRightMargin * settings.thumbs) - settings.thumbsRightMargin), height: thumbsHeight, textAlign: "center", overflow: "hidden", margin: "0 auto" });
            jQuery("div#thumbSlider" + j + " ul").css({ width: (thumbsWidth * pictEls) + settings.thumbsRightMargin * pictEls, position: "relative", left: "0", top: "0" });
            jQuery("div#thumbSlider" + j + " ul li").css({ marginRight: settings.thumbsRightMargin });

            jQuery("div#thumbSlider" + j).find("p.tmbrdr").css({ width: (thumbsWidth - tBorder * 2) + "px", height: (thumbsHeight - tBorder * 2) + "px", top: -(thumbsHeight) + "px", border: settings.thumbsBorderWidth + "px solid " + settings.thumbsBorderColor, opacity: settings.thumbsBorderOpacity });
            jQuery("div#thumbSlider" + j + " a:first p.tmbrdr").css({ borderColor: settings.thumbsActiveBorderColor, opacity: settings.thumbsActiveBorderOpacity });

            var rbttLeftMargin = (pictWidth / 2) + (jQuery("div#thumbSlider" + j).width() / 2) + settings.thumbsRightMargin + contBorder;
            var lbttLeftMargin = (pictWidth / 2) - (jQuery("div#thumbSlider" + j).width() / 2) - (settings.buttonsWidth + settings.thumbsRightMargin) + contBorder;

            // setting img margin - left/right
            var innerLeftImg = jQuerybtl.find("img");
            var innerRightImg = jQuerybtr.find("img");
            if (innerLeftImg.length != 0 && innerRightImg.length != 0) {
                jQuery(innerLeftImg).load(function () {
                    jQuery(this).css({ margin: Math.round((thumbsHeight / 2) - (jQuery(this).height() / 2)) + "px 0 0 0" });
                });
                jQuery(innerRightImg).load(function () {
                    jQuery(this).css({ margin: Math.round((thumbsHeight / 2) - (jQuery(this).height() / 2)) + "px 0 0 0" });
                });
            }

            // setting img margin - first/last
            var jQuerybtlEndImg = jQuerybtlEnd.find("img");
            var jQuerybtrEndImg = jQuerybtrEnd.find("img");
            if (jQuerybtlEndImg.length != 0 && jQuerybtrEndImg.length != 0) {
                jQuery(jQuerybtlEndImg).load(function () {
                    jQuery(this).css({ margin: Math.round((thumbsHeight / 2) - (jQuery(this).height() / 2)) + "px 0 0 0" });
                });
                jQuery(jQuerybtrEndImg).load(function () {
                    jQuery(this).css({ margin: Math.round((thumbsHeight / 2) - (jQuery(this).height() / 2)) + "px 0 0 0" });
                });
            }

            // next/previous button styles
            jQuery("a#left" + j).css({ display: "block", textAlign: "center", width: settings.buttonsWidth + "px", height: thumbsHeight + "px", margin: -(thumbsHeight - settings.thumbsTopMargin) + "px 0 0 " + lbttLeftMargin + "px", textDecoration: "none", lineHeight: thumbsHeight + "px", color: settings.buttonsTextColor });
            jQuery("a#right" + j).css({ display: "block", textAlign: "center", width: settings.buttonsWidth + "px", height: thumbsHeight + "px", margin: -(thumbsHeight) + "px 0 0 " + rbttLeftMargin + "px", textDecoration: "none", lineHeight: thumbsHeight + "px", color: settings.buttonsTextColor });

            // end/last button styles
            jQuery("a#leftEnd" + j).css({ display: "block", textAlign: "center", width: settings.buttonsWidth + "px", height: thumbsHeight + "px", margin: -(thumbsHeight - settings.thumbsTopMargin) + "px 0 0 " + parseInt(lbttLeftMargin - 30) + "px", textDecoration: "none", lineHeight: thumbsHeight + "px", color: settings.buttonsTextColor });
            jQuery("a#rightEnd" + j).css({ display: "block", textAlign: "center", width: settings.buttonsWidth + "px", height: thumbsHeight + "px", margin: -(thumbsHeight) + "px 0 0 " + parseInt(rbttLeftMargin + 30) + "px", textDecoration: "none", lineHeight: thumbsHeight + "px", color: settings.buttonsTextColor });

            // positioning left-first button
            var position_left = jQuery("a#left" + j).position();
            var position_leftEnd = jQuery("a#leftEnd" + j).position();
            var difference = position_left.top - position_leftEnd.top;

            // setting margin-top of left href
            var margin_top = parseInt(jQuery("a#left" + j).css("margin-top"));
            var new_margin = margin_top - difference;
            jQuery("a#leftEnd" + j).css('marginTop', new_margin + 'px');
            margin_top = jQuery("a#leftEnd" + j).css("margin-top");

            // setting margin-top of left img
            var jQuerybtlEndImg = jQuerybtlEnd.find("img");
            if (jQuerybtlEndImg.length != 0) {
                jQuery(jQuerybtlEndImg).load(function () {
                    jQuery(this).css({ margin: Math.round((thumbsHeight / 2) - (jQuery(this).height() / 2)) + difference + "px 0 0 0" });
                });
            }

            // setting navigation button visibility
            authorityMixing();

            if (settings.autoslide) {
                var i = 1;

                jQuery("div#thumbSlider" + j).everyTime(settings.asTimer, "asld", function () {
                    jQuery(this).find("a").eq(i).trigger("click");
                    if (i == 0) {
                        pos = 0;
                        l_enabled = false;
                        jQuery("div#thumbSlider" + j).find("ul:not(:animated)").animate({ left: -(thumbsWidth + settings.thumbsRightMargin) * pos }, 500, settings.easeFunc, function () { authorityMixing(); });
                    }
                    else l_enabled = true;

                    (i % settings.thumbs == 0) ? jQuery(this).next().next().trigger("click") : null;
                    (i < pictEls - 1) ? i++ : i = 0;
                });

                //stops autoslidemode	
                jQuery("a#right" + j).bind("mouseup", function () {
                    jQuery(this).prev().prev().stopTime("asld");
                });
                jQuery("a#left" + j).bind("mouseup", function () {
                    jQuery(this).prev().stopTime("asld");
                });
                jQuery("div#thumbSlider" + j + " a").bind("mouseup", function () {
                    jQuery(this).parent().parent().parent().stopTime("asld");
                });
            }

            var uiDisplay = (settings.thumbsVis) ? "block" : "none";

            //jQuery("div#thumbSlider" + j + ", a#left" + j + ", a#right" + j).wrapAll("<div style='width:" + pictWidth + "px; display: " + uiDisplay + "' id='ui" + j + "'><\/div>");

            jQuery("div#thumbSlider" + j + ", a#left" + j + ", a#right" + j + ", a#leftEnd" + j + ", a#rightEnd" + j).wrapAll("<div style='width:" + pictWidth + "px; display: " + uiDisplay + "' id='ui" + j + "'><\/div>");

            jQuery("div#svwp" + j + ", div#ui" + j).wrapAll("<div style='width:" + pictWidth + "px'><\/div>");
        });

        (jQuery("div#thumbSlider" + j).width() + (settings.buttonsWidth * 2) >= pictWidth) ? alert("ALERT: THE THUMBNAILS SLIDER IS TOO WIDE! \nthumbsPercentReduction and/or buttonsWidth needs to be scaled down!") : null;

        j++;

    });
};