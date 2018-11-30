/**
Vertigo Tip by www.vertigo-project.com
Requires jQuery
*/

this.vtip = function () {
    this.xOffset = -10; // x distance from mouse
    this.yOffset = 10; // y distance from mouse       

    $(".vtip").unbind().hover(
        function (e) {
            //debugger;

            this.t = this.title;
            this.title = '';
            this.top = (e.pageY + yOffset);
            this.left = (e.pageX + xOffset);

            $('body').append('<p id="vtip"><img id="vtipArrow" />' + this.t + '</p>');

            $('p#vtip #vtipArrow').attr("src", 'Scripts/jQuery/ToopTip/tooltip_arrow.png');

            $('p#vtip').css("top", this.top + "px").css("left", this.left + "px").fadeIn("slow");
        },
        function () {
            this.title = this.t;
            $("p#vtip").fadeOut("slow").remove();
        }
    ).mousemove(
        function (e) {
            this.top = (e.pageY + yOffset);
            this.left = (e.pageX + xOffset);

            $("p#vtip").css("top", this.top + "px").css("left", this.left + "px");
        }
    );

};

this.vtipRight = function () {
    this.xOffset = -10; // x distance from mouse
    this.yOffset = 10; // y distance from mouse       

    $(".vtipRight").unbind().hover(
        function (e) {
            //debugger;

            this.t = this.title;
            this.title = '';

            this.top = (e.pageY + yOffset);
            this.left = (e.pageX + xOffset);

            $('body').append('<p id="vtipRight"><img id="vtipArrowRight" />' + this.t + '</p>');

            $('p#vtipRight #vtipArrowRight').attr("src", 'Scripts/jQuery/ToopTip/tooltip_arrow.png');

            $('p#vtipRight').css("top", this.top + "px").css("left", this.left + "px").fadeIn("slow");
        },
        function () {
            this.title = this.t;
            $("p#vtipRight").fadeOut("slow").remove();
        }
    ).mousemove(
        function (e) {
            this.top = (e.pageY + yOffset);
            this.left = (e.pageX + xOffset) - 400;

            $("p#vtipRight").css("top", this.top + "px").css("left", this.left + "px");
        }
    );

};

//jQuery(document).ready(function($){vtip();}) 