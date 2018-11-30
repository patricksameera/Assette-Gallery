///////////////////////////////////////////////////////
//********** Assette Gallery       **********//
//********** Assette © 2013                **********//
///////////////////////////////////////////////////////

//debugger;

// document ready //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// jquery document ready function
$(document).ready(function () {

    // window resize event
    WindowReSize();

    // rich text formatter
    RichTextFormatter();

})

// rich text formatter /////////////////////////////////////////////////////////////////////////////////////////////////////////////

function RichTextFormatter() {
    //debugger;

    var text = "<p>This is a test document." +
                "Some <span class='mceNonEditable' style=\"color: #ff0000;\">Portion</span> " +
                "of this document can't be changed.</p>\r\n<p>The area with red " +
                "<span class='mceNonEditable' style=\"color: #ff0000;\">background </span> " +
                "can't be <span class='mceNonEditable' style=\"color: #ff0000;\">removed</span>. " +
                "You can only <span class='mceNonEditable' style=\"color: #ff0000;\">change </span>" +
                "the area with black.</p>\r\n<p>&nbsp;</p>";

    $("textarea#content").val(text);

    tinyMCE.init({

        // General options
        mode: "textareas",
        theme: "advanced",
        plugins: "autolink,lists,spellchecker,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template",
        encoding: "xml",
        language: 'en',

        // Theme options
        theme_advanced_buttons1: "save,newdocument,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,|,styleselect,formatselect,fontselect,fontsizeselect",
        theme_advanced_buttons2: "cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,cleanup,help,code,|,insertdate,inserttime,preview,|,forecolor,backcolor",
        theme_advanced_buttons3: "tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,emotions,iespell,media,advhr,|,print,|,ltr,rtl,|,fullscreen",
        theme_advanced_buttons4: "insertlayer,moveforward,movebackward,absolute,|,styleprops,spellchecker,|,cite,abbr,acronym,del,ins,attribs,|,visualchars,nonbreaking,template,blockquote,pagebreak,|,insertfile,insertimage",
        theme_advanced_toolbar_location: "top",
        theme_advanced_toolbar_align: "left",
        theme_advanced_statusbar_location: "bottom",
        theme_advanced_resizing: true,

        // Skin options
        skin: "o2k7",
        skin_variant: "silver",

        // Example content CSS (should be your site CSS)
        content_css: "css/example.css",

        // Drop lists for link/image/media/template dialogs
        template_external_list_url: "js/template_list.js",
        external_link_list_url: "js/link_list.js",
        external_image_list_url: "js/image_list.js",
        media_external_list_url: "js/media_list.js",

        // Replace values for the template plugin
        template_replace_values: {
            username: "Some User",
            staffid: "991234"
        },

        style_formats: [
	        { title: 'Red text', inline: 'span', styles: { color: '#ff0000'} },
	        { title: 'Red header', block: 'h1', styles: { color: '#ff0000'} },
	        { title: 'Table styles' },
	        { title: 'Table Row', selector: 'tr', classes: 'tablerow' },
	        { title: 'Table Border', selector: 'table', styles: { border: '1px solid'} }
	    ]

    });

}

// window resize event /////////////////////////////////////////////////////////////////////////////////////////////////////////////

function WindowReSize() {
    //debugger;
    /*
    // positioning log-in image
    var position = $("#gridClients").offset();
    var windowWidth = $(window).width();

    $("#message_bar").css("left", windowWidth / 2 - 70);
    $("#message_bar").css("top", position.top - 97);*/
}