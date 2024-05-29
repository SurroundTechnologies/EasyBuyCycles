// Process data-replacewith attributes
// -----------------------------------
//
// This function is used to process elements that have data-replacewith attributes.
// It takes the value of the attribute as a jQuery selector or an element id, finds 
// the matching element within the page, and moves it to the location of the element
// that has the data-replacewith attribute.
//
// This is typically used for CMS static web content pages that have dynamically-inserted
// content, to indicate where the dynamic content should be located within the static 
// content.
//
// The flag data-replacewith-mode="inner" can be used when you only want to move the 
// children of the specified element, instead of the entire element. This is for situations
// where the markup structure won't allow an extra wrapper element to be inserted.
//
// Either way, after the function is run the div with data-replacewith will be gone from 
// the DOM, and if inner mode is used the container element will also be gone from the DOM.
//
//
// Usage - Markup:
//
// <div data-replacewith="#dynamic">Placeholder content; this entire div will be replaced with #dynamic.</div>
//
// <div data-replacewith="#dynamic" data-replacewith-mode="inner">This div will be replaced with #dynamic's children.</div>
//
//
// Usage - Process all attributes at page ready:
//
// $(function() { $.a4dn_process_replacewith(); });
//
//
// Usage - Process specific container after page load:
//
// onSuccess: function (data) {
//     $("#container").html($(data.markup));
//     $.a4dn_process_replacewith("#container");
// }
// 
(function ($) {
    "use strict";

    $.extend({
        a4dn_process_replacewith: function (selector) {
            if (selector) {
                selector += " [data-replacewith]";
            }
            else {
                selector = "[data-replacewith]";
            }

            // Searches for elements loaded from CMS system that need to relocate dynamically-generated sections
            $(selector).each(function () {
                var id = $(this).data('replacewith'),
                    $elem = /^[.#]/.test(id) ? $(id) : $("#" + id); // explicit id/class selector, or implicit id selector

                if ($elem.length == 0) { // Maybe id starts with an element name?
                    $elem = $(id);
                }

                if ($elem.length > 0) {
                    if ($elem.data("replacewith-mode") === "inner") {
                        $(this).replaceWith($elem.children());
                        $elem.remove();
                    }
                    else {
                        $(this).replaceWith($elem);
                    }
                }
            });
        }
    })
})(jQuery);

