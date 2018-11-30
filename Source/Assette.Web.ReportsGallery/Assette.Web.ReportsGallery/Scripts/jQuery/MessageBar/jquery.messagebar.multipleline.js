/*!
* jQuery Display Message Plugin
*
* Copyright 2010, Andrey Voev
* http://www.andreyvoev.com
*
* Date: Fri Dec 12 16:12 2010 -0800
*/

(function ($) {

    $.fn.displayMessage = function (options) {

        // Default configuration properties.
        var defaults = {
            message: 'Error message',
            background: '#111111',
            color: '#FFFFFF',
            speed: 'fast',
            skin: 'plain',
            position: 'relative', // relative, absolute, fixed
            autohide: false
        }

        var options = $.extend(defaults, options);
        $(this).slideUp('fast');
        $(this).removeClass().empty();

        var sticky = (options.sticky == false) ? 'relative' : 'absolute';

        $(this).addClass('messagebar-skin-' + options.skin + '_bar').css('position', options.position).css('background-color', options.background);

        var rows = [];

        rows.push('<div class="messagebar-skin-' + options.skin + '_inner">');

        rows.push('<div id="messagebar" class="messagebar-skin-' + options.skin + '_text">');
        rows.push('</div>');

        rows.push('<div id="close" class="messagebar-skin-' + options.skin + '_close">');
        rows.push('</div>');

        rows.push('</div>');

        var htmlContent = rows.join("");

        $(this).append(htmlContent).css('color', options.color);

        $(this).find('#messagebar').html(options.message);

        $(this).slideDown(options.speed, function () {

            if (options.autohide == true) {
                $(this).delay(1000).slideUp('slow');
            }

            $('#close').bind("click", function (event) {
                event.preventDefault();
                $(this).parent().hide();
            });

        });
    };
})(jQuery);
