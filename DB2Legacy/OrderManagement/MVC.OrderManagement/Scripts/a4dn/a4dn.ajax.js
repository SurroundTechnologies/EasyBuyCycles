// a4dn.ajax.js

// Wrapper for $.ajax with built-in error handling and support for responses creating using AB_JsonResponse()
// and its partners.
//
// Arguments:
// - url: The url to send the request too
// - $feedbackElement: a jQuery object pointing to an element which will have a "loading" class applied during the request. (Optional)
// - options: an object containing callback functions and other options
// 
// Options (Commonly used):
// - type: HTTP Method; defaults to GET
// - data: data to upload; typically either $("form").serialize() or new FormData($("form")[0])
// - cache: set to true to append _{timestamp} to GET/HEAD requests to prevent cached responses
// - onSuccess(data, $feedbackElement): function called when request is successful. data is the json object created by AB_JsonResponse().
// - onProgress(loaded, total, e, $feedbackElement): function called with upload/download progress events. loaded and total will be set
//      to -1 if the browser can't compute the data transfer size. Won't be called for browsers that don't support XHR2.
// - processData: set to false when uploading files using FormData()
// - contentType: set to false when uploading files using FormData(). (Don't try to use multipart encoding type.)
//
// Options (rarely used): 
// - dataType: response type; defaults to "json"
// - loadingClass: defaults to "loading"; can be changed to a different class name
// - onSessionError(jqXHR, textStatus, errorThrown, $feedbackElement): called when session expired
//      - default: reload page to get redirected to login page
// - onAjaxCancelled(jqXHR, textStatus, errorThrown, $feedbackElement): called when user/browser cancels ajax request
//      - default: ignore error
// - onAjaxError(jqXHR, textStatus, errorThrown, $feedbackElement): called when there is an http error
//      - default: show alert box with textStatus and errorThrown
// - onErrorResponse(data, textStatus, jqXHR, $feedbackElement): called when data.resultCode != "OK"
//      - default: show alert box with data.message
// - onMessageOnlyResponse(data, textStatus, jqXHR, $feedbackElement): called when data.resultCode == "OK", data.message has a value, and data.markup doesn't
//      - default: show alert box with data.message
// - beforeSend(jqXHR, ajaxSettings): sets up class on $feedbackElement
// - complete(jqXHR, textStatus): cleans up class on $feedbackElement
(function ($) {
    "use strict";

    a4dn.core.mvc.am_ajax_defaultOnSuccess = function (data, $feedbackElement) { };
    a4dn.core.mvc.am_ajax_defaultOnMessageOnlyResponse = function (data, textStatus, jqXHR, $feedbackElement) { alert(data.message); }
    a4dn.core.mvc.am_ajax_defaultOnProgress = function (loaded, total, e, $feedbackElement) { };

    a4dn.core.mvc.am_ajax_defaultOnSessionError = function (jqXHR, textStatus, errorThrown, $feedbackElement) { console.error(jqXHR, $feedbackElement, textStatus, errorThrown); window.location.reload(); };  // Reload the page to get the redirect again as a page refresh
    a4dn.core.mvc.am_ajax_defaultOnAjaxError = function (jqXHR, textStatus, errorThrown, $feedbackElement) { console.error(jqXHR, $feedbackElement, textStatus, errorThrown); alert(textStatus + ": " + errorThrown); };
    a4dn.core.mvc.am_ajax_defaultOnErrorResponse = function (data, textStatus, jqXHR, $feedbackElement) { console.error(jqXHR, $feedbackElement, textStatus, data); alert(data.message); };
    a4dn.core.mvc.am_ajax_defaultOnAjaxCancelled = function (jqXHR, textStatus, errorThrown, $feedbackElement) { console.info($feedbackElement, textStatus, errorThrown); };   // Ignore the error

    a4dn.core.mvc.am_ajax = function (url, $feedbackElement, options) {
        var opts = $.extend({
            loadingClass: "loading",
            onSuccess: a4dn.core.mvc.am_ajax_defaultOnSuccess,
            onProgress: a4dn.core.mvc.am_ajax_defaultOnProgress,
            onSessionError: a4dn.core.mvc.am_ajax_defaultOnSessionError,
            onAjaxCancelled: a4dn.core.mvc.am_ajax_defaultOnAjaxCancelled,
            onAjaxError: a4dn.core.mvc.am_ajax_defaultOnAjaxError,
            onErrorResponse: a4dn.core.mvc.am_ajax_defaultOnErrorResponse,
            onMessageOnlyResponse: a4dn.core.mvc.am_ajax_defaultOnMessageOnlyResponse,
            beforeSend: function (jqXHR, ajaxSettings) {
                if ($feedbackElement !== undefined && $feedbackElement !== null) {
                    let currentOps = $feedbackElement.data('a4dn-am_ajax_ops-in-progress') || {};
                    if (currentOps[opts.loadingClass] === undefined) {
                        currentOps[opts.loadingClass] = 0;
                    }

                    currentOps[opts.loadingClass] = currentOps[opts.loadingClass] + 1;
                    $feedbackElement.data('a4dn-am_ajax_ops-in-progress', currentOps);
                    $feedbackElement.addClass(opts.loadingClass);
                }
            },
            complete: function (jqXHR, textStatus) {
                if ($feedbackElement !== undefined && $feedbackElement !== null) {
                    let currentOps = $feedbackElement.data('a4dn-am_ajax_ops-in-progress') || {};
                    if (currentOps[opts.loadingClass] === undefined) {
                        currentOps[opts.loadingClass] = 0;
                    }

                    currentOps[opts.loadingClass] = currentOps[opts.loadingClass] > 0 ? currentOps[opts.loadingClass] - 1 : 0;
                    $feedbackElement.data('a4dn-am_ajax_ops-in-progress', currentOps);

                    if (currentOps[opts.loadingClass] <= 0) {
                        $feedbackElement.removeClass(opts.loadingClass);
                    }
                }
            },

            type: "GET",
            dataType: "json",
            cache: false,
            processData: true,
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            async: true,
            data: {}

        }, options || {});

        $.ajax({
            url: url,
            data: opts.data,
            type: opts.type,
            dataType: opts.dataType,
            cache: opts.cache,
            processData: opts.processData,
            contentType: opts.contentType,
            async: opts.async,
            xhr: function () {
                var myXhr = $.ajaxSettings.xhr();
                if (myXhr.upload) {
                    myXhr.upload.addEventListener('progress', function (e) {
                        if (e.lengthComputable)
                            opts.onProgress(e.loaded, e.total, e, $feedbackElement);
                        else
                            opts.onProgress(-1, -1, e, $feedbackElement);
                    }, false);
                }
                return myXhr;
            },
            beforeSend: function (jqXHR, ajaxSettings) {
                opts.beforeSend(jqXHR, ajaxSettings);
            },
            complete: function (jqXHR, textStatus) {
                opts.complete(jqXHR, textStatus);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                // Check for requests cancelled by user action
                if (jqXHR.readyState < 4) {
                    opts.onAjaxCancelled(jqXHR, textStatus, errorThrown, $feedbackElement);
                    return;
                }

                // Check if the response was an authentication redirect to the Login page
                var contentType = jqXHR.getResponseHeader('Content-Type');
                if (jqXHR.status == 200 && typeof contentType == 'string' && contentType.toLowerCase().indexOf('text/html') >= 0) {
                    opts.onSessionError(jqXHR, textStatus, errorThrown, $feedbackElement);
                    return;
                }
                else {
                    opts.onAjaxError(jqXHR, textStatus, errorThrown, $feedbackElement);
                    return;
                }
            },
            success: function (data, textStatus, jqXHR) {
                if (data.resultCode != "OK") {
                    opts.onErrorResponse(data, textStatus, jqXHR, $feedbackElement);
                }
                else if (data.message.length > 0 && (data.markup == null || data.markup == "")) {
                    opts.onMessageOnlyResponse(data, textStatus, jqXHR, $feedbackElement);
                }
                else {
                    opts.onSuccess(data, $feedbackElement);
                }
            }
        });
    }
})(jQuery);