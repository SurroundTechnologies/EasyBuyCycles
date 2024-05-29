// AB_Newlook: a4dn.newlook.js
//
// Plugin for Dropdown Controls
// Depends on jquery.encapsulatedPlugin.js

/*

SYNOPSYS

In a document ready handler:

$(".a4dn-newlook-session").AB_Newlook();

The AB_Newlook() plugin expects to work with an iframe

OPTIONS

The AB_Newlook() method does not take any arguments.

METHODS

Public methods can be called on the api object, which is stored in the
"AB_Newlook" data for the element the plugin is attached to.

var api = $("#somelement").data("AB_Newlook");
var result = api.PublicMethodName(...);

... TODO: document public methods ...

EVENTS

All event handlers are executed in the context of the element
that matches the jQuery selector used in the initial call to
AB_Newlook(). The 'this' variable is set to that element.

*/

(function ($) {
    "use strict";

    var AB_Newlook = function (element, options) {
        //--------------------------------------------------------------------
        // Defaults

        var settings = $.extend({
            // HINT: Define default values for any options here
            url: "",
        }, options || {});

        //--------------------------------------------------------------------
        // Private variables

        var $element = $(element),
             iframeURL = "http://localhost",
             //iframeURL = "http://100.35.36.117",
             iframeSessionURL = "",
             iframeAPIUrl = "",
             iframeWindow = "",
             iframeFormName = "",
             iframeFormID = "",
             iframeFormTitle = "",
             showScreen = false,
             guid = "",
             sendKeyComplete = "",
             hideNavMsgOnNextSessionLoaded = false,
             api = this;

        //--------------------------------------------------------------------
        // Public methods and event handlers

        // HINT: public methods can be called on the api object, which will be
        // available to the code that uses the plugin.
        //
        // Example - Public method definition:
        // this.MyPublicMethod = function(...) { ... }
        //
        // Public method usage:
        //
        // $("#myElement").AB_Newlook(...);
        //
        // var api = $("#myElement").data("AB_Newlook");
        // api.MyPublicMethod(...);
        //
        this.SetValue = function () {
            // TODO
        }
        //
        // Event handlers can be triggered by the code that uses the plugin,
        // or by the browser if they're standard DOM events. See jQuery docs
        // for more info about the on() and trigger() methods.
        //
        // Example - Event Handler definition:
        //
        // $element.on("click", ".myClassSelector", function (e) {
        //     ...
        // });
        //
        // $element.on("mycustomevent", function (e) {
        //     ...
        // });
        //
        // Event Handler usage:
        // $("#myElement").trigger("mycustomevent");
        //

        // TODO: Implement public methods and event handlers

        //--------------------------------------------------------------------
        // Private methods

        // HINT: Define private methods here, which cannot be accessed by users
        // of the plugin. They can only be called from inside this startup
        // code and the methods, functions, and event handlers defined here.
        //
        // Example - Private method defintion:
        //
        // function myPrivateMethod(...) { ... }
        //

        function receiveMessage(e) {
            // Do we trust the sender of this message?
            if (e.origin !== iframeURL)
                return;

            if (e.data.commandID == "SessionLoaded") {
                if (!$element.data("a4dn-loaded")) {
                    $element.data("a4dn-loaded", true);

                    iframeWindow = e.source;
                    iframeSessionURL = e.data.href;
                    iframeAPIUrl = iframeSessionURL.replace('session', 'api/v1/');

                    // Update source with new encoded URL
                    var $form = a4dn.core.mvc.am_Get$Form(guid);
                    $form.data("a4dn-newlook-src", a4dn_Base64.am_ToBase64String(iframeSessionURL));
                }
            }

            if (iframeWindow != e.source) {
                // Only get messages for this iFrame
                return;
            }

            switch (e.data.commandID) {
                case "SessionLoaded":

                    iframeFormName = e.data.formname;
                    iframeFormID = e.data.formid;
                    iframeFormTitle = e.data.formtitle;

                    // Add Embedded class on HTML tag
                    iframeWindow.postMessage({ commandID: "AddEmbeddedClass" }, iframeURL);

                    if ($element.data('a4dn-stayonscreen') === "True" || hideNavMsgOnNextSessionLoaded) {
                        $element.data('a4dn-stayonscreen', false);
                        hideNavMsgOnNextSessionLoaded = false;

                        // This gives the CSS a chance to remove newlook shell
                        setTimeout(function () { $element.trigger("hideNavigatingMessage"); }, 500);
                    }

                    $element.trigger("SessionLoaded", e);

                    break;

                case "SendKey_Completed":

                    if (typeof sendKeyComplete == 'function') {
                        sendKeyComplete(e);
                    }

                    if (showScreen) {
                        hideNavigatingMessage();
                    }

                    break;

                case "SetOption_Completed":

                    if (showScreen) {
                        hideNavigatingMessage();
                    }

                    break;

                case "ControlChanged":

                    var $form = a4dn.core.mvc.am_Get$Form(guid);
                    $form.addClass("a4dn-dirty");
                    a4dn.core.mvc.am_SetSaveCommandsBasedOnChange(guid);

                    break;

                default:
            }
        }
        window.addEventListener("message", receiveMessage, false);

        function playTransaction(name, inputs, options) {
            $.when(

            setValue("renewTransactionName", name),
            setValue("renewTransactionInputs", inputs),
            setValue("renewMacroName", "renew_PlayTransaction"),
            setValue("renewReturnToMainMenuMacroName", "renew_ReturnToMainMenu.F3")

                 ).then(function () {
                     // All have been resolved (or rejected)
                     // Run Macro
                     runMacro("renew_RunMacro", options);

                     getValue("renewTransactionError", function (data) {
                         if (typeof data !== "undefined" && data != "") {
                             a4dn.core.mvc.am_Notification("smallBox", "Error", "PlayTransaction: " + data);
                         }
                     });
                 })
        }

        function setValue(item, value) {
            if (typeof item !== "undefined" && typeof value !== "undefined") {
                return ajax("app/SetValue", $element, {
                    data: JSON.stringify([item, value])
                });
            }
        }

        function getValue(item, options) {
            if (typeof item !== "undefined") {
                return ajax("app/GetValue", $element, {
                    data: JSON.stringify([item])
                });
            }
        }

        function sendKeys(keys, options) {
            if (typeof keys !== "undefined") {
                return ajax("System/SendKeys", $element, {
                    data: JSON.stringify(["", keys, options.waitResult]),
                });
            }
        }

        function runMacro(name, options) {
            if (typeof name !== "undefined") {
                $.when(

                  ajax("app/RunMacro", $element, {
                      data: JSON.stringify([name]),
                  })

               ).then(function () {
                   // All have been resolved (or rejected)

                   if (typeof options === "undefined" || typeof options.showScreen === "undefined" || options.showScreen == true) {
                       hideNavMsgOnNextSessionLoaded = true;
                   }

                   // Reload
                   $element.attr('src', iframeSessionURL);

                   // Check for errors
                   getValue("renewMacroError", function (data) {
                       if (data != "") {
                           a4dn.core.mvc.am_Notification("smallBox", "Error", data);
                       }
                   });
               })
            }
        }

        function ajax(urlMethod, $feedbackElement, options) {
            var opts = $.extend({
                loadingClass: "loading",
                beforeSend: function (jqXHR, ajaxSettings) {
                    if ($feedbackElement != undefined)
                        $feedbackElement.addClass(opts.loadingClass);
                },
                complete: function (jqXHR, textStatus) {
                    if ($feedbackElement != undefined)
                        $feedbackElement.removeClass(opts.loadingClass);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    hideNavigatingMessage();

                    if (typeof jqXHR !== "undefined" && jqXHR != "") {
                        a4dn.core.mvc.am_Notification("smallBox", "Error", urlMethod + ":" + jqXHR + textStatus + errorThrown);
                    }
                },
                success: function (result, textStatus, jqXHR) {
                },

                type: "POST",
                dataType: "json",
                cache: false,
                processData: true,
                contentType: "application/json",
                async: true,
                crossDomain: true,
                xhrFields: { withCredentials: true },
                data: {}
            }, options || {});

            return $.ajax({
                url: iframeAPIUrl + urlMethod,
                data: opts.data,
                type: opts.type,
                dataType: opts.dataType,
                cache: opts.cache,
                processData: opts.processData,
                contentType: opts.contentType,
                async: opts.async,
                crossDomain: opts.crossDomain,
                xhrFields: opts.xhrFields,
                beforeSend: function (jqXHR, ajaxSettings) {
                    opts.beforeSend(jqXHR, ajaxSettings);
                },
                complete: function (jqXHR, textStatus) {
                    opts.complete(jqXHR, textStatus);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    opts.error(jqXHR, textStatus, errorThrown);
                },
                success: function (result, textStatus, jqXHR) {
                    opts.success(result, textStatus, jqXHR);
                }
            });
        }

        function showNavigatingMessage() {
            $('#a4dn-newlook-iframe-navigating-' + guid).removeClass('hidden');
            $('#a4dn-newlook-iframe-' + guid).addClass('hidden');
        }

        function hideNavigatingMessage() {
            $('#a4dn-newlook-iframe-navigating-' + guid).addClass('hidden');
            $('#a4dn-newlook-iframe-' + guid).removeClass('hidden');
        }

        //--------------------------------------------------------------------
        // Plugin Setup Code

        // HINT: Any code that the plugin needs to run the first time it is
        // attached to an element can go here. If the plugin user tries to
        // attach the plugin to the same element a second time, the setup code
        // will not run again. They will get a reference to the same plugin
        // object.

        this.setup = function () {
            guid = $element.data('a4dn-guid');

            $element.on("playTransaction", function (e, name, inputs, options) {
                //Options.beforeSend - ajax beforeSend function
                //Options.complete - ajax complete function
                //Options.error - ajax error function
                //Options.success - ajax success function
                //options.showScreen (boolean - show screen after complete - default is true)

                showNavigatingMessage();
                playTransaction(name, inputs, options);
            });

            $element.on("sendKey", function (e, key, options) {
                //options.complete (Completed CallBack Function)
                //options.showScreen (boolean - show screen after complete - default is true)

                sendKeyComplete = options.complete;

                if (typeof options.showScreen === "undefined" || options.showScreen == true) {
                    showScreen = true;
                }
                else { showScreen = false; }

                iframeWindow.postMessage({ commandID: "SendKey", key: key }, iframeURL);
            });

            $element.on("runMacro", function (e, name, options) {
                //Options.beforeSend - ajax beforeSend function
                //Options.complete - ajax complete function
                //Options.error - ajax error function
                //Options.success - ajax success function
                //options.showScreen (boolean - show screen after complete - default is true)

                showNavigatingMessage();
                runMacro(name, options);
            });

            $element.on("sendKeys", function (e, keys, options) {
                //options.complete (Completed CallBack Function)
                //options.showScreen (boolean - show screen after complete - default is true)

                //sendKeyComplete = options.complete;
                options = options || {};
                options.waitResult = options.waitResult || 1;

                if (typeof options.showScreen === "undefined" || options.showScreen == true) {
                    showScreen = true;
                }
                else { showScreen = false; }

                sendKeys(keys, options);
            });

            $element.on("closeSession", function (e) {
                $element.height(0).width(0);
                iframeWindow.postMessage({ commandID: "CloseSession" }, iframeURL);
            });

            $element.on("restartSession", function (e) {
                iframeWindow.postMessage({ commandID: "RestartSession" }, iframeURL);
            });

            $element.on("refresh", function () {
                $element.attr('src', iframeSessionURL);
            });

            $element.on("showNavigatingMessage", function () {
                showNavigatingMessage();
            });

            $element.on("hideNavigatingMessage", function () {
                hideNavigatingMessage();
            });

            $element.on("postMessage", function (e, options) {
                //options.showScreen (boolean - show screen after complete - default is true)

                if (typeof options.showScreen === "undefined" || options.showScreen == true) {
                    showScreen = true;
                }
                else { showScreen = false; }

                iframeWindow.postMessage(options, iframeURL);
            });
        };
    };

    // Expose plugin
    $.fn.AB_Newlook = function (options) {
        return $.fn.encapsulatedPlugin('AB_Newlook', AB_Newlook, this, options);
    };
})(jQuery);