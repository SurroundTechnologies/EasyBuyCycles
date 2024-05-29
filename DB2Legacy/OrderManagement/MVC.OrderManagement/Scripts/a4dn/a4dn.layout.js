// ***********************************************************************
// Assembly         : MVC.SmartAdminLOB
// Author           : Derek Maciak
// Created          : 03-11-2016
//
// Last Modified By : Derek Maciak
// Last Modified On : 03-10-2016
// ***********************************************************************
// <copyright file="a4dn.layout.js" company="">
//     Copyright ©  2014
// </copyright>
// <summary></summary>
// ***********************************************************************

// TODO: re-enable this and root out all of the issues it causes, particular use of undeclared/global variables: "use strict";

var _ContentMainClass = ".a4dn-splitter-pane-1";
var _ContentPreviewClass = ".a4dn-splitter-pane-2";
var _ModuleIdPrefix = "a4dn-module-";

var _NextSplitterType = "a";

var _IsPhone = false;
var _LastWidth = 0;
var _LastHeight = 0;

var _DataTablesIgnoreAjaxRequest = false;
var _DataTablesExcelExportAjaxRequest = false;
var _DataTablesExcelExportAll = false;

a4dn.core.mvc.ap_LoadedModules = [];    // Clear loaded modules cache each time a4dn.layout.js is loaded and executed

// Prevent the browser from backing out of the application
// https://stackoverflow.com/a/34337617/73475
if (window.history) {
    history.pushState(null, document.title, location.href);
    window.addEventListener('popstate', function (event) {
        history.pushState(null, document.title, location.href);
    });
}

// Setup Default ajax methods
a4dn.core.mvc.am_ajax_defaultOnErrorResponse = function (data, textStatus, jqXHR, $feedbackElement) {
    // Distinguish between error and informational messages
    if (data.output && data.output.messages) {
        if ($feedbackElement !== undefined && $feedbackElement !== null) {
            if (data.markup !== null && data.markup.length) $feedbackElement.html(data.markup);
        }
        else {
            console.trace();
            console.error(data);
        }
        a4dn.core.mvc.am_ajax_NotifyMessages(data.output.messages, "Error");
    }
    else {
        if ($feedbackElement !== undefined && $feedbackElement !== null) {
            if (data.markup !== null && data.markup.length) $feedbackElement.html(data.markup);   // Remove?
            else if (data.output !== null && data.output.length) $feedbackElement.html(data.output);
        }
        else {
            console.trace();
            console.error(data);
        }
        a4dn.core.mvc.am_Notification("smallBox", "Error", data.message);
    }
};

a4dn.core.mvc.am_ajax_defaultOnAjaxError = function (jqXHR, textStatus, errorThrown, $feedbackElement) {
    console.error(jqXHR, $feedbackElement, textStatus, errorThrown);
    console.trace();
    if (jqXHR.responseText) {
        console.error(jqXHR.responseText);
    }
    a4dn.core.mvc.am_Notification("smallBox", "Error", textStatus + ": " + errorThrown);
};

a4dn.core.mvc.am_ajax_defaultOnMessageOnlyResponse = function (data, textStatus, jqXHR, $feedbackElement) {
    // Distinguish between success and informational messages
    if (data.output && data.output.messages) {
        a4dn.core.mvc.am_ajax_NotifyMessages(data.output.messages, "Success");
    }
    else {
        a4dn.core.mvc.am_Notification("extraSmallBox", "Success", data.message);
    }
};

a4dn.core.mvc.am_ajax_NotifyMessages = function (messages, defaultType) {
    messages.forEach(function (msg) {
        switch (msg.type) {
            case "Informational":
                a4dn.core.mvc.am_Notification("extraSmallBox", "Information", msg.text);
                break;

            case "Error":
                a4dn.core.mvc.am_Notification("extraSmallBox", "Error", msg.text);
                break;

            case "Validation":
                a4dn.core.mvc.am_Notification("extraSmallBox", "ValidationError", msg.text);
                break;

            case "Success":
                a4dn.core.mvc.am_Notification("extraSmallBox", "Success", msg.text);
                break;


            default:
                a4dn.core.mvc.am_Notification("extraSmallBox", defaultType, msg.text);
        }
    });
};


// AJAX progress bar

var _ActiveAJAXRequests = [];

a4dn.core.mvc.am_ShowAjaxProgresBar = function () {
    $('.js-ajax-progress-bars .progress').show();
};

a4dn.core.mvc.am_HideAjaxProgressBar = function () {
    $('.js-ajax-progress-bars .progress').hide();
};

a4dn.core.mvc.am_UpdateAjaxProgressBar = function () {
    let $bar = $('.js-ajax-progress-bars .progress .progress-bar'),
        width = _ActiveAJAXRequests.length === 0 ? 0 : 100 / _ActiveAJAXRequests.length;
    $bar.css({ width: width + "%" }).attr('aria-valuenow', width);
};

$(document).ajaxStart(function () {
    a4dn.core.mvc.am_ShowAjaxProgresBar();
});

$(document).ajaxSend(function (e, jqXHR, options) {
    _ActiveAJAXRequests.push(options.url);
    a4dn.core.mvc.am_UpdateAjaxProgressBar();
});

//$(document).ajaxError(function (e, jqXHR, options, thrownError) {
//    console.debug("AJAX: Error", e, jqXHR, options, thrownError);
//});

//$(document).ajaxSuccess(function (e, jqXHR, options, data) {
//    console.debug("AJAX: Success", e, jqXHR, options, data);
//});

$(document).ajaxComplete(function (e, jqXHR, options) {
    _ActiveAJAXRequests = _ActiveAJAXRequests.filter(url => url !== options.url);
    a4dn.core.mvc.am_UpdateAjaxProgressBar();
});

$(document).ajaxStop(function () {
    a4dn.core.mvc.am_HideAjaxProgressBar();
});

// Convert a function into a debounced function that will only execute after {wait}ms, no matter how often it's called.
// See https://davidwalsh.name/javascript-debounce-function
a4dn.core.mvc.am_Debounce = function (func, wait, immediate) {
    var timeout;
    return function () {
        var context = this, args = arguments;
        var later = function () {
            timeout = null;
            if (!immediate) func.apply(context, args);
        };
        var callNow = immediate && !timeout;
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
        if (callNow) func.apply(context, args);
    };
};

a4dn.core.mvc.am_LoadModuleJavascript = function (srcUrl, useAsync, useCache) {
    // If srcUrl hasn't been loaded before, use jQuery to load it.
    // Setting useCache = true will prevent jQuery from adding a cache-busting _{timestamp} to the url

    if (a4dn.core.mvc.ap_LoadedModules.indexOf(srcUrl) < 0) {
        $.ajax({
            url: srcUrl,
            dataType: 'script',
            success: function () {
                a4dn.core.mvc.ap_LoadedModules.push(srcUrl);
            },
            async: useAsync,
            cache: useCache
        });
    }
};

a4dn.core.mvc.am_htmlEscape = function (textString) {
    // Uses jQuery .text and .html to escape html-special characters in textString into html escaped characters
    return $('<div>').text(textString).html();
};

a4dn.core.mvc.am_InitializeNavigator = function () {
    $("#hide-menu").click(function (e) {
        e.preventDefault();

        if ($("#a4dn-body").hasClass("hidden-menu")) {
            $("#a4dn-main-content").addClass("a4dn-hidden-menu");
        } else {
            $("#a4dn-main-content").removeClass("a4dn-hidden-menu");
        }
        a4dn.core.mvc.am_FullScreenToggle();
    });

    $(".a4dn-nav-modules, .a4dn-tiles").on("click", "a", function (e) {
        e.preventDefault();
        var $this = $(this);

        if ($this.hasClass('a4dn-module-link')) {
            a4dn.core.mvc.am_NavModuleClick($this);
        }
        if ($this.hasClass('a4dn-favorites-link')) {
            a4dn.core.mvc.am_ToggleModuleFavorite($this);
        }
    });

    $("#a4dn-main-content").on("click", ".a4dn-module-link", function (e) {
        e.preventDefault();

        let $link = $(this),
            modNo = $link.data('a4dn-module-number'),
            params = $link.data('a4dn-params'),
            $navLink = $(".a4dn-nav .a4dn-module-link[data-a4dn-modnum='" + modNo + "']");

        if ($navLink.length > 0) {
            a4dn.core.mvc.am_NavModuleClick($navLink.eq(0), { openInNewTab: true, searchParameters: params });
        }
    });

    $("#a4dn-main-module-explorer-tabs, #a4dn-main-module-explorer-dropdown").on("click", ".closeTab", function (e) {
        e.preventDefault();

        var $this = $(this);

        // There are multiple elements which has .closeTab icon so close the tab whose close icon is clicked
        var tabContentId = $this.closest('a').attr("href");

        // If tab contains a module detail - check if record is dirty
        var $container = $(tabContentId);
        var $mod = $container.find('.a4dn-module-detail').first();
        if ($mod.length) {
            var guid = $mod.data("a4dn-guid");
            var $form = a4dn.core.mvc.am_Get$Form(guid);
            if ($form.hasClass("a4dn-dirty") || $form.find(".a4dn-dirty").length) {
                // Record is dirty - prompt user to save
                a4dn.core.mvc.am_SmartMessageBox_SaveChanges(guid, tabContentId);
                return;
            }
        }

        a4dn.core.mvc.am_CloseModuleExplorerTab(tabContentId);
    });

    $("#a4dn-main-module-explorer-tabs, #a4dn-main-module-explorer-dropdown").on("click", ".pinTab", function (e) {
        e.preventDefault();

        var $this = $(this);

        //there are multiple elements which has .closeTab icon so close the tab whose close icon is clicked
        var tabContentId = $this.closest('a').attr("href");

        var $tab = $('#a4dn-main-module-explorer-tabs').find('a[href$=' + tabContentId + ']').find('.pinTab') // tab
        var $dd = $('#a4dn-main-module-explorer-dropdown').find('a[href$=' + tabContentId + ']').find('.pinTab'); // dropdown

        var pinned = $this.closest("li").data("a4dn-pinned");

        if (pinned == "true") {
            $tab.addClass("fa-rotate-90");
            $tab.removeClass("a4dn-pinned");
            $tab.closest("li").data("a4dn-pinned", "false");

            $dd.addClass("fa-rotate-90");
            $dd.removeClass("a4dn-pinned");
            $dd.closest("li").data("a4dn-pinned", "false");
        } else {
            $tab.removeClass("fa-rotate-90");
            $tab.addClass("a4dn-pinned");
            $tab.closest("li").data("a4dn-pinned", "true");

            $dd.removeClass("fa-rotate-90");
            $dd.addClass("a4dn-pinned");
            $dd.closest("li").data("a4dn-pinned", "true");
        }

        // move as first li
        var $tabli = $('#a4dn-main-module-explorer-tabs').find('a[href$=' + tabContentId + ']').closest('li');
        $tabli.parent().prepend($tabli);
        var $ddli = $('#a4dn-main-module-explorer-dropdown').find('a[href$=' + tabContentId + ']').closest('li');
        $ddli.parent().prepend($ddli);
    });

    $("#a4dn-main-module-explorer-dropdown").on("click", "a", function (e) {
        var $this = $(this);

        $("#a4dn-main-module-explorer-dropdown-text").html('<span class="a4dn-tab-image">' + $this.find('.a4dn-tab-image').html() + '</span> <span class="a4dn-tab-title">' + $this.find('.a4dn-tab-title').html() + '</span>');

        var href = $this.attr('href');

        $('#a4dn-main-module-explorer-tabs').find('a[href$=' + href + ']').tab('show'); // Select first tab

        $("#a4dn-main-module-explorer-dropdown li").removeClass("active");
        $this.closest('li').addClass("active");
    });

    $("#a4dn-main-module-explorer-tabs").on("click", "a", function (e) {
        $('.tooltip').remove();
    });

    $("#a4dn-new-explorer-tab").on("click", "a", function (e) {
        e.preventDefault();
        e.stopPropagation();
        $('.tooltip').remove();

        var $this = $(this);
        if ($this.hasClass('a4dn-new-explorer-tab-link')) {
            a4dn.core.mvc.am_NavModuleClick($this);
        }
    });


    var $body = $('#a4dn-body');
    if ($body.data("a4dn-show-new-tab-on-startup") == true || $body.data("a4dn-show-new-tab-on-startup") == "True") {

        // Show New Tab on Startup
        $("#a4dn-new-explorer-tab a.a4dn-new-explorer-tab-link").click();
    }
    else {

        // Set First Module Open
        $('.a4dn-nav-modules a').first().click();
        $('.4dn-nav li').first().addClass('open');
        $('ul.a4dn-nav-modules').first().css("display", "block");
    }


    $body.on("mousedown", function (e) {
        if ($(e.target).hasClass('a4dn-view')) { return; }

        $('div.ad4n-dropdown-menu-attach-to-body.open > button.dropdown-toggle').click();
    });
};

a4dn.core.mvc.am_InitializeModuleRecentAndFavorites = function () {
    $(".a4dn-nav-favorites-modules, .a4dn-nav-recent-modules").on("click", "a", function (e) {
        e.preventDefault();

        a4dn.core.mvc.am_OnNavigatorFavoriteRecentModuleClick($(this));
    });

    $(".a4dn-nav-favorites-selector").on("click", function (e) {
        e.preventDefault();

        var $container = $(".a4dn-nav-favorites-modules.dropdown-menu");

        if ($container.data("a4dn-dataloaded") == false || $container.data("a4dn-dataloaded") == "False") {
            var url = $container.data("a4dn-view-href");

            a4dn.core.mvc.am_ajax(url, $container, {
                onSuccess: function (data) {
                    //console.log($container, data);
                    $container.html($(data.markup));
                    $container.data("a4dn-dataloaded", true);
                }
            });
        }
    });

    $(".a4dn-nav-recent-selector").on("click", function (e) {
        e.preventDefault();

        var $container = $(".a4dn-nav-recent-modules.dropdown-menu");

        if ($container.data("a4dn-dataloaded") == false || $container.data("a4dn-dataloaded") == "False") {
            var url = $container.data("a4dn-view-href");

            a4dn.core.mvc.am_ajax(url, $container, {
                onSuccess: function (data) {
                    //console.log($container, data);
                    $container.html($(data.markup));
                    $container.data("a4dn-dataloaded", true);
                }
            });
        }
    });
}

a4dn.core.mvc.am_InitializeNewTabRecentAndFavorites = function (guid) {
    $("#a4dn-module-explorer-new-tab-" + guid).on("click", "a", function (e) {
        e.preventDefault();

        a4dn.core.mvc.am_OnNavigatorFavoriteRecentModuleClick($(this));
    });
}

a4dn.core.mvc.am_OnNavigatorFavoriteRecentModuleClick = function ($this, removePrompt) {
    var $container = $this;
    if ($container.hasClass('a4dn-remove')) {
        var url = $container.data('a4dn-remove-href');

        if ($container.hasClass('a4dn-remove-all-btn') && (typeof removePrompt === "undefined" || removePrompt == true)) {
            var title = "";
            if ($container.hasClass('a4dn-remove-all-favorites')) {
                title = $('#a4dn-main-content').data('a4dn-lang-notification-removefav-title'); //"Are you sure you want to remove all favorites?"
            }
            if ($container.hasClass('a4dn-remove-all-recent')) {
                title = $('#a4dn-main-content').data('a4dn-lang-notification-removerec-title'); // "Are you sure you want to remove all recently used?"
            }

            var noBtn = $('#a4dn-main-content').data('a4dn-lang-no'); //"No"
            var yesBtn = $('#a4dn-main-content').data('a4dn-lang-yes'); //"Yes"

            $.SmartMessageBox({
                title: $('#a4dn-main-content').data('a4dn-lang-notification-removeall-title'), //'Confirm Remove All?'
                content: title,
                buttons: '[' + noBtn + '][' + yesBtn + ']'
            }, function (ButtonPressed) {
                if (ButtonPressed === yesBtn) {
                    // Call again without prompt
                    a4dn.core.mvc.am_OnNavigatorFavoriteRecentModuleClick($this, false);
                }
            });
            return;
        }

        a4dn.core.mvc.am_ajax(url, $container, {
            type: "POST",
            onMessageOnlyResponse: function (data, textStatus, jqXHR) {
                //console.log($container, data);

                if ($container.hasClass('a4dn-remove-favorite') || $container.hasClass('a4dn-remove-all-favorites')) {
                    // Mark Favorites modules to be reloaded
                    $(".a4dn-nav-favorites-modules").data("a4dn-dataloaded", false);

                    if ($container.hasClass('a4dn-remove-favorite')) {
                        var moduleNumber = $container.data('a4dn-module-number');
                        var appNumber = $container.data('a4dn-app-number');
                        $(".a4dn-favorites-link").filter('[data-a4dn-app-number="' + appNumber + '"]' + '[data-a4dn-module-number="' + moduleNumber + '"]').removeClass("a4dn-isfavorite");
                    }
                    else {
                        $(".a4dn-favorites-link").removeClass("a4dn-isfavorite");
                    }
                }
                if ($container.hasClass('a4dn-remove-recent') || $container.hasClass('a4dn-remove-all-recent')) {
                    // Mark Favorites modules to be reloaded
                    $(".a4dn-nav-recent-modules").data("a4dn-dataloaded", false);
                }

                if (data.message != "") {
                    a4dn.core.mvc.am_Notification("extraSmallBox", "Success", data.message);
                }

                a4dn.core.mvc.am_ReloadModuleExplorerNewTabs();
            }
        });
    }
    else {
        // Module Clicked
        a4dn.core.mvc.am_NavModuleClick($container);
    }
}

a4dn.core.mvc.OpenWindows = {};

a4dn.core.mvc.am_NavModuleClick = function ($this, options) {
    $("#a4dn-main-content").removeClass("a4dn-hidden");

    options = options || {};

    let modNum = $this.data("a4dn-modnum"),
        modNam = $this.data("a4dn-modnam"),
        url = $this.attr("href"),
        target = $this.attr('target');

    if (target === 'x-a4dn-inline') {
        // FIXME: Open a module tab and display url inside it using an iframe
    }
    else if (target === "_blank") {
        window.open(url, target);
        return;
    }
    else if (target !== undefined) {
        var openWindow = a4dn.core.mvc.OpenWindows[target];
        if (openWindow === null || openWindow === undefined || openWindow.closed) {
            a4dn.core.mvc.OpenWindows[target] = window.open(url, target);
        }
        else {
            openWindow.focus(); // Browsers are inconsistent about allowing this. Not sure if there is a workaround.
        }
        return;
    }

    if (url === "javascript:void(0);") {
        url = $this.data("a4dn-href");
    }

    if (options.searchParameters) {
        url += '&pSearchData=' + options.searchParameters;
    }

    var openOnlyOne = $this.data("a4dn-allow-only-one");
    if (openOnlyOne == true || openOnlyOne === "True") {
        var $modExpTab = $("#a4dn-main-module-explorer-tab-content .a4dn-module").filter('[data-a4dn-mod-number="' + modNum + '"]').first();
        var tabpaneId = $modExpTab.closest('.a4dn-module-explorer-tab-pane').attr('id');

        tabpaneId = "#" + tabpaneId;
        var $tab = $('#a4dn-main-module-explorer-tabs > li > a').filter('[href="' + tabpaneId + '"]').first();
        if ($tab.length) {
            // Tab Open - just Activate
            $tab.tab('show');

            var $itm = $("#a4dn-main-module-explorer-dropdown").find('span:contains("' + title + '")').first();
            $itm.trigger("click");

            return;
        }
    }

    var tabContentContainerID = "m-" + modNum + "-" + a4dn.core.mvc.am_GenerateGuid();

    // If New Tab - Don't show in existing Tab
    if (!$this.hasClass('a4dn-new-explorer-tab-link')) {
        var openInNewTab = $this.data("a4dn-open-in-new-tab");

        if (options.openInNewTab || (typeof openInNewTab !== "undefined" && (openInNewTab === "True" || openInNewTab == true))) {
            // Do not close Tab - Open in New Tab
        }
        else {
            // Check if Current Tab is pinned

            var pinned = $("#a4dn-main-module-explorer-tabs li.active").data("a4dn-pinned");
            if (pinned != true && pinned != "true") {
                //there are multiple elements which has .closeTab icon so close the tab whose close icon is clicked
                var tabContentId = $("#a4dn-main-module-explorer-tabs li.active a").attr("href");
                $("#a4dn-main-module-explorer-tabs li.active").remove(); //remove li of tab

                $('#a4dn-main-module-explorer-dropdown').find('a[href$=' + tabContentId + ']').closest('li').remove(); //remove li of dropdown

                $(tabContentId).remove(); //remove respective tab content
            }
        }
    }
    var image = $this.parent().find("img.a4dn-nav-image").first().attr("src");

    if (typeof image === "undefined") {
        image = $('#a4dn-body').data('content-href') + "/img/a4dn/Tab_small.png";
    }

    var imageElement = '<img src="' + image + '" />';

    $(".a4dn-main-module-loading").removeClass("hidden");

    $("#a4dn-main-module-explorer-tabs li").removeClass("active")
    $("#a4dn-main-module-explorer-dropdown li").removeClass("active")

    $("#a4dn-main-module-explorer-tab-content").append('<div class="a4dn-module-explorer-tab-pane a4dn-tab-pane tab-pane active" id="' + tabContentContainerID + '"> </div>');

    // Locate New Tab
    var $newTabli = $('#a4dn-main-module-explorer-tabs').find('li.a4dn-new-explorer-tab');

    if ($this.hasClass('a4dn-new-explorer-tab-link')) {
        // Add Tab before new Tab with pin visible and not pinned
        $('<li data-a4dn-pinned="false"> <a href="#' + tabContentContainerID + '" data-toggle="tab"><span class="a4dn-tab-image">' + imageElement + '</span><span class="a4dn-tab-title">' + modNam + '</span> <span class="a4dn-tab-controls"></i> <i class="fa fa-times closeTab a4dn-closetab-only" data-original-title="' + $('#a4dn-main-content').data('a4dn-lang-closetab') + '" rel="tooltip" data-placement="bottom"></i></span></a></li>').insertBefore($newTabli);
    }
    else {
        // Add Tab before new Tab with pin visible and not pinned
        $('<li data-a4dn-pinned="false"> <a href="#' + tabContentContainerID + '" data-toggle="tab"><span class="a4dn-tab-image">' + imageElement + '</span><span class="a4dn-tab-title">' + modNam + '</span> <span class="a4dn-tab-controls"> <i class="fa fa-thumb-tack pinTab fa-rotate-90" data-original-title="' + $('#a4dn-main-content').data('a4dn-lang-togglepin') + '" rel="tooltip" data-placement="bottom"></i> <i class="fa fa-times closeTab" data-original-title="' + $('#a4dn-main-content').data('a4dn-lang-closetab') + '" rel="tooltip" data-placement="bottom"></i></span></a></li>').insertBefore($newTabli);
    }

    // Add to Drop Down
    $("#a4dn-main-module-explorer-dropdown").append('<li  data-a4dn-pinned="false" class="active"> <a href="#' + tabContentContainerID + '" data-toggle="tab"><span class="a4dn-tab-image">' + imageElement + '</span> <span class="a4dn-tab-title">' + modNam + '</span>  <span class="a4dn-tab-controls"> <i class="fa fa-thumb-tack pinTab fa-rotate-90" data-original-title="' + $('#a4dn-main-content').data('a4dn-lang-togglepin') + '" rel="tooltip" data-placement="bottom"></i>|<i class="fa fa-times closeTab" data-original-title="' + $('#a4dn-main-content').data('a4dn-lang-closetab') + '" rel="tooltip" data-placement="bottom"></i></span></a></li>');

    $("#a4dn-main-module-explorer-dropdown-text").html('<span class="a4dn-tab-image">' + imageElement + '</span> <span class="a4dn-tab-title">' + modNam + '</span>');

    $('#a4dn-main-module-explorer-tabs a[href="#' + tabContentContainerID + '"]').tab('show');

    var $container = $("#" + tabContentContainerID);

    a4dn.core.mvc.am_ajax(url, $container, {
        onSuccess: function (data) {
            $(".a4dn-main-module-loading").addClass("hidden");
            $container.html($(data.markup));

            // Mark Recent modules to be reloaded
            $(".a4dn-nav-recent-modules").data("a4dn-dataloaded", false);

            if (!$this.hasClass('a4dn-new-explorer-tab-link')) {
                // Only Reload if not a new tab
                a4dn.core.mvc.am_ReloadModuleExplorerNewTabs();
            }
        },
        onErrorResponse: function (data, textStatus, jqXHR, $feedbackElement) {
            $(".a4dn-main-module-loading").addClass("hidden");
            a4dn.core.mvc.am_ajax_defaultOnErrorResponse(data, textStatus, jqXHR, $feedbackElement);
        },
        beforeSend: function (jqXHR, ajaxSettings) {
            $(".a4dn-main-module-loading").removeClass("hidden");
        },
        complete: function (jqXHR, textStatus) {
            $(".a4dn-main-module-loading").addClass("hidden");
        },
    });

    if (window.matchMedia('(max-width: 768px)').matches) {
        // hide Menu
        $('#hide-menu a').trigger('click');
    }
}

a4dn.core.mvc.am_CloseModuleExplorerTab = function (tabContentId) {

    var timeout = 0;

    // Close newlook Sesssion - Need to wait on destroying tab
    var $newlookiframe = $(tabContentId).find('.a4dn-newlook-iframe');
    if ($newlookiframe.length) {
        $newlookiframe.trigger("closeSession");
        timeout = 1000;
    }

    setTimeout(function () {
        $('#a4dn-main-module-explorer-tabs').find('a[href$=' + tabContentId + ']').closest('li').hide();// Hide first so it appears to remove instantly

        $('#a4dn-main-module-explorer-tabs').find('a[href$=' + tabContentId + ']').closest('li').remove(); //remove li of tab
        $('#a4dn-main-module-explorer-dropdown').find('a[href$=' + tabContentId + ']').closest('li').remove(); //remove li of dropdown

        // Get module guids from tab and cleanup search explorers
        let moduleGuids = $(tabContentId).find(".a4dn-module-explorer[data-a4dn-guid]").map(function () { return $(this).data('a4dn-guid'); }).get();
        $.each(moduleGuids, function (i, guid) {
            a4dn.core.mvc.am_CleanupModalSearchTemplateFromStorage(guid);
        });


        // Get First Module Explorer - Release Lock?
        let firstModule = $(tabContentId).find(".a4dn-module").first();
        if (firstModule.length && firstModule.data('a4dn-type') === 'detail') {

            let trackUserSessions = $('#a4dn-main-content').data('a4dn-userjoblog-track-user-sessions');
            let recordLockingEnabled = $('#a4dn-main-content').data('a4dn-userjoblog-record-locking-enabled');
            let commandID = firstModule.data('a4dn-commandid');
            let uniqueName = firstModule.data('a4dn-unique-name');
            let uniqueKey = firstModule.data('a4dn-unique-key');
            let recordMode = firstModule.data('a4dn-record-mode');

            // If record locking is on and module explorer is an open detail (not in Display mode), then release lock;
            // Don't unlock if detail is open in display/read-only mode, because that will release someone else's lock
            if (trackUserSessions === "True" && recordLockingEnabled === "True" && recordMode !== "Display") {
                a4dn.core.mvc.am_ReleaseRecordLock({ uniqueName: uniqueName, uniqueKey: uniqueKey, commandID: commandID });
            }
        }

        //remove respective tab content
        $(tabContentId).remove();

        var modHtml = "";

        var $lastAnchorBeforeNewTab = $('#a4dn-main-module-explorer-tabs li:last').prev().find('a');
        if ($lastAnchorBeforeNewTab.length) {
            var href = $lastAnchorBeforeNewTab.attr("href");

            $("#a4dn-main-module-explorer-dropdown li").removeClass("active");
            $('#a4dn-main-module-explorer-dropdown').find('a[href$=' + href + ']').closest('li').addClass("active");

            modHtml = $lastAnchorBeforeNewTab.find('.a4dn-tab-image').clone().html() + $lastAnchorBeforeNewTab.find('.a4dn-tab-title').clone().html();

            modHtml = '<span class="a4dn-tab-image">' + $lastAnchorBeforeNewTab.find('.a4dn-tab-image').html() + '</span> <span class="a4dn-tab-title">' + $lastAnchorBeforeNewTab.find('.a4dn-tab-title').html() + '</span>';
        }

        setTimeout(function () { $("#a4dn-main-module-explorer-dropdown-text").html(modHtml); }, 100);

        if ($('#a4dn-main-module-explorer-tabs li:last').prev().find('a').length > 0) {
            $('#a4dn-main-module-explorer-tabs li:last').prev().find('a').tab('show'); // Select last tab
        } else {
            // Show New Tab
            $("#a4dn-new-explorer-tab a.a4dn-new-explorer-tab-link").click();
        }
    }, timeout);
};

a4dn.core.mvc.am_InitializeWindow = function () {
    a4dn.core.mvc.am_WindowResize();

    a4dn.core.mvc.am_SetupWindowEvents();

    a4dn.core.mvc.am_DefineCustomValidationRules();

    // Set initial height of Module Explorer tab content
    $("#a4dn-main-module-explorer-tab-content").css("height", $("#a4dn-main-content").height());

    //$("#a4dn-main-content-widget .jarviswidget-fullscreen-btn").click(function () {
    //    a4dn.core.mvc.am_FullScreenToggle();
    //});

    $("[data-action='toggleCompactUI']").on('click', function (e) {
        e.preventDefault();
        let $body = $("body");

        if ($body.is(".a4dn-compact-ui")) {
            $body.removeClass('a4dn-compact-ui');
        }
        else {
            $body.addClass('a4dn-compact-ui');
        }

        $(window).trigger('resize');
    });

    a4dn.core.mvc.am_StartUserJobLogHeartBeat();
    a4dn.core.mvc.am_HideAjaxProgressBar();
};

a4dn.core.mvc.am_DefineCustomValidationRules = function () {
    if ($.validator) {
        // Tell unobtrusive about a4dn_propertychanged, so it will manage data attributes and validator
        $.validator.unobtrusive.adapters.add('a4dn_propertychanged', [], function (options) {
            options.rules['a4dn_propertychanged'] = options.params;
        });

        // Tell validator how to check a4dn_propertychanged-isinerror flag
        $.validator.addMethod('a4dn_propertychanged', function (value, element, params) {
            var $element = $(element),
                isInError = $element.data("a4dn_propertychanged-isinerror");
            // return true if element is NOT in error: flag is false or undefined
            return isInError != true || isInError == undefined;
        });
    }
}

a4dn.core.mvc.am_SetupWindowEvents = function () {
    //$(window).on('beforeunload', function (e) {
    //    // Close newlook Session
    //    $(this).find('.a4dn-newlook-iframe').trigger("closeSession");
    //});

    // hold onto the drop down menu
    var $dropdownMenu;

    // and when you show it, move it to the body
    $(window).on('show.bs.dropdown', function (e) {
        if ($(e.target).hasClass('ad4n-dropdown-menu-attach-to-body')) {
            // grab the menu
            $dropdownMenu = $(e.target).find('.dropdown-menu');

            // detach it and append it to the body
            $('body').append($dropdownMenu.detach());

            // grab the new offset position
            var eOffset = $(e.target).offset();

            if ($(e.target).hasClass('dropup')) {
                $dropdownMenu.css({
                    'display': 'block',
                    'top': eOffset.top + $(e.target).outerHeight() - $dropdownMenu.outerHeight(true),
                    'left': eOffset.left
                });
            }

            else if ($(e.target).hasClass('dropdown')) {
                $dropdownMenu.css({
                    'display': 'block',
                    'top': eOffset.top + $(e.target).outerHeight(),
                    'left': eOffset.left
                });
            }
        }
    });

    // and when you hide it, reattach the drop down, and hide it normally
    $(window).on('hide.bs.dropdown', function (e) {
        if ($(e.target).hasClass('ad4n-dropdown-menu-attach-to-body')) {
            $(e.target).append($dropdownMenu.detach());
            $dropdownMenu.hide();
        }
    });

    $(document).keydown(function (e) {
        a4dn.core.mvc.am_KeyDown(this, e);
    });

    $(window).resize(function () {
        a4dn.core.mvc.am_WindowResize();
    });

    $(window).on('show.bs.collapse', function (e) {
        var $panelBodyContainer = $(e.target),
            $headingLink = $panelBodyContainer.prev().find("[data-toggle='collapse']"),
            $accordion = $panelBodyContainer.parents(".a4dn-accordion");

        if ($accordion.length == 0)
            return;

        $headingLink.removeClass("collapsed").attr("aria-expanded", true);
    });
    $(window).on('hide.bs.collapse', function (e) {
        var $panelBodyContainer = $(e.target),
            $headingLink = $panelBodyContainer.prev().find("[data-toggle='collapse']"),
            $accordion = $panelBodyContainer.parents(".a4dn-accordion");

        if ($accordion.length == 0)
            return;

        $headingLink.addClass("collapsed").attr("aria-expanded", false);
    });
};

a4dn.core.mvc.am_WindowResize = function () {
    let windowHeight = window.innerHeight,
        windowWidth = window.innerWidth;

    if (window.innerHeight === _LastHeight && window.innerWidth === _LastWidth) {
        return;
    }

    _LastHeight = windowHeight;
    _LastWidth = windowWidth;

    let $divBody = $('#a4dn-main-content');

    if ($divBody.length > 0) {
        let height = windowHeight - $('#header').outerHeight(true) - $('#footer').outerHeight(true);
        $divBody.css("height", height);
    }

    waitForFinalEvent(function () {
        if (window.matchMedia('(max-width: 768px)').matches) {
            // Phone

            _IsPhone = true;

            // remove Module Explorer
            $('.a4dn-module-explorer-tab-pane').each(function () {
                let $pane = $(this),
                    $mod = $pane.find('.a4dn-module').first(),
                    guid = $mod.data("a4dn-guid"),
                    $toolbarCommandId = $("#a4dn-toolbar-commands-" + guid);

                if ($mod.data('a4dn-type') === "search-explorer") {
                    a4dn.core.mvc.am_HidePreviewPane(guid);
                }

                // Hide Preview
                $toolbarCommandId.find(".a4dn-command" + "[data-a4dn-command-id='PREVIEW']").each(function () {
                    $(this).closest('li').addClass("hidden");
                });
            });
        }
        else if (_IsPhone) {
            _IsPhone = false;

            $('.a4dn-module-explorer-tab-pane').each(function () {
                let $pane = $(this),
                    $mod = $pane.find('.a4dn-module').first(),
                    guid = $mod.data("a4dn-guid"),
                    $toolbarCommandId = $("#a4dn-toolbar-commands-" + guid);

                // Show Preview button
                $toolbarCommandId.find(".a4dn-command.a4dn-button" + "[data-a4dn-command-id='PREVIEW']").each(function () {
                    $(this).closest('li').removeClass("hidden");
                });
            });
        }
    }, 100, _LastWidth);
};

a4dn.core.mvc.am_KeyDown = function (document, e) {
    if (e.which == 114 && e.shiftKey == true && e.ctrlKey == true) {
        // ctrl+shift+F3 - close active tab
        $('#a4dn-main-module-explorer-tabs li.active > a .closeTab').trigger('click');
    }

    if (e.which == 113 && e.shiftKey == true && e.ctrlKey == true) {
        // ctrl+shift+F2 - pin active tab
        $('#a4dn-main-module-explorer-tabs li.active > a .pinTab').trigger('click');
    }

    var $table = $(document).find("table.a4dn-focus").first();

    if ($table.length) {
        var $tbody = $table.find('tbody').first();

        if ($tbody.find('.highlight').length) {
            if (e.which == 40) {//down arrow
                e.preventDefault();
                a4dn.core.mvc.am_SelectNextRow($table, $tbody);
            }

            else if (e.which == 38) {//up arrow
                e.preventDefault();

                a4dn.core.mvc.am_SelectPreviousRow($table, $tbody);
            }

            else if (e.which == 13) {//enter
                // Don't intercept Enter key inside open x-editable controls
                if ($(e.target).parents(".editable-input").length > 0)
                    return;

                //here is your code for double click
                var $moduleExplorer = $table.closest(".a4dn-module-explorer");
                var guid = $moduleExplorer.data('a4dn-guid');
                a4dn.core.mvc.am_TriggerCommandClick(guid, "OPEN")
            }
        }
    }
};

a4dn.core.mvc.am_FullScreenToggle = function () {
    $('.tooltip').remove();

    $('#a4dn-main-module-explorer-tab-content').find('.a4dn-module-explorer-tab-pane').each(function () {
        var moduleExplorerId = $(this).first('.splitter').attr("id");

        waitForFinalEvent(function () {
            $jq("#" + moduleExplorerId).trigger("resize");
        }, 100, moduleExplorerId);
    });

    if ($('#jarviswidget-fullscreen-mode').length) {
    }
    else {
        setTimeout(function () { $('#a4dn-main-module-explorer-tab-content').css("height", $('#a4dn-main-module-explorer-tab-content').height() - 7); }, 5000);
    }
};

a4dn.core.mvc.am_FullScreenToggleWidget = function (widgetID) {
    var $splitter = $(widgetID).closest('.splitter');
    var moduleExplorerId = $splitter.attr("id");

    var guid = $moduleExplorerId.closest('.a4dn-module-explorer').data('a4dn-guid');

    $('.tooltip').remove();

    waitForFinalEvent(function () {
        $jq("#" + moduleExplorerId).trigger("resize");
        a4dn.core.mvc.am_ResizeGrid(guid);
    }, 100, moduleExplorerId);
};

a4dn.core.mvc.am_SetupModuleExplorer = function (options) {
    var guid = options.guid;
    //var parentGuid = options.parentGuid;
    //var splitterDirection = options.splitterDirection;
    var previewSize = options.previewSize;
    //var maxInitializeSplitterCount = options.maxInitializeSplitterCount;
    var commandCallBackNamespace = options.commandCallBackNamespace;

    var $module = a4dn.core.mvc.am_Get$Module(guid);
    var parentGuid = $module.data("a4dn-parent-guid");
    if (parentGuid === "") { parentGuid = undefined; }

    var splitterDirection = $module.data("a4dn-splitter-direction");
    if (splitterDirection === "") { splitterDirection = "Horizontal"; }

    var maxInitializeSplitterCount = $module.data("a4dn-max-init-splitter-cnt");
    if (maxInitializeSplitterCount === "") { maxInitializeSplitterCount = 1; }

    var enablePreview = $module.data("a4dn-enable-preview");
    if (enablePreview === "" || enablePreview === "True") { enablePreview = true; } else { enablePreview = false; }

    var previewInitVisible = $module.data("a4dn-preview-init-visible");
    if (previewInitVisible === "" || previewInitVisible === "True") { previewInitVisible = true; } else { previewInitVisible = false; }

    // Hide Preview container
    $("#a4dn-module-" + guid + " .a4dn-module-preview").first().addClass("hidden");

    pageSetUp();

    a4dn.core.mvc.am_InitializeJarvisWidgets("a4dn-widget-section-" + guid);

    a4dn.core.mvc.am_InitializeModuleExplorerToolbar(guid, commandCallBackNamespace);

    if (enablePreview) {
        if (previewInitVisible) {
            var showPrev = a4dn.core.mvc.am_InitializePreviewPane(guid, maxInitializeSplitterCount)
            if (!showPrev) {
                previewSize = 0;
            }
        }
        else {
            a4dn.core.mvc.am_SetupModuleExplorerContentPreviewClickEvents(guid);
            previewSize = 0;
        }
    }
    else {
        previewSize = 0;
    }

    a4dn.core.mvc.am_InitializeSplitter(guid, splitterDirection, previewSize);

    a4dn.core.mvc.am_SetupModuleExplorerEvents(guid, parentGuid);

    if (commandCallBackNamespace && typeof commandCallBackNamespace.afterPageLoad == 'function') {
        commandCallBackNamespace.afterPageLoad(guid);
    }

    // show Preview container
    $("#a4dn-module-" + guid + " .a4dn-module-preview").first().removeClass("hidden");

    if (options.showExplorerBarOnStartup === true || options.showExplorerBarOnStartup === "True") {
        // Show Explorer Bar
        var $search = $("#a4dn-toolbar-commands-" + guid + " .a4dn-command").filter('[data-a4dn-command-id="SEARCH"]').first();
        $search.click();
    }
};

a4dn.core.mvc.am_SetupModuleDetail = function (options) {
    var guid = options.guid;
    //  var splitterDirection = options.splitterDirection;
    var previewSize = options.previewSize;
    //  var maxInitializeSplitterCount = options.maxInitializeSplitterCount;
    //  var recordMode = options.recordMode;
    var commandCallBackNamespace = options.commandCallBackNamespace;

    var $module = a4dn.core.mvc.am_Get$Module(guid);

    $module.data('a4dn-commandcallbacknamespace', commandCallBackNamespace);

    var parentGuid = $module.data("a4dn-parent-guid");
    if (parentGuid === "") { parentGuid = undefined; }

    var splitterDirection = $module.data("a4dn-splitter-direction");
    if (splitterDirection === "") { splitterDirection = "Vertical"; }

    var maxInitializeSplitterCount = $module.data("a4dn-max-init-splitter-cnt");
    if (maxInitializeSplitterCount === "") { maxInitializeSplitterCount = 1; }

    var enablePreview = $module.data("a4dn-enable-preview");
    if (enablePreview === "" || enablePreview === "True") { enablePreview = true; } else { enablePreview = false; }

    var previewInitVisible = $module.data("a4dn-preview-init-visible");
    if (previewInitVisible === "" || previewInitVisible === "True") { previewInitVisible = true; } else { previewInitVisible = false; }

    var recordMode = $module.data("a4dn-record-mode");
    if (recordMode === "") { recordMode = "Open" };

    // Hide Preview container
    $("#a4dn-module" + guid + " .a4dn-module-preview").first().addClass("hidden");

    pageSetUp();

    a4dn.core.mvc.am_InitializeJarvisWidgets("a4dn-widget-section-" + guid);

    let $form = a4dn.core.mvc.am_Get$Form(guid);
    a4dn.core.mvc.am_InitializeDropdownControls($form);
    a4dn.core.mvc.am_InitializeDateTimePickers($form);

    if (recordMode != "Preview") {

        a4dn.core.mvc.am_InitializeDetailToolbar(guid, commandCallBackNamespace);

        a4dn.core.mvc.am_SetCommandStateFromRecordMode(guid, recordMode);

        // update Module Explorer Tab title and Tooltip
        var title = $('#a4dn-module-' + guid).data("a4dn-title");
        $('#a4dn-main-module-explorer-tabs li.active > a').attr("data-original-title", title);
        $('#a4dn-main-module-explorer-tabs li.active > a').find(".a4dn-tab-title").html(title);
        $('#a4dn-main-module-explorer-dropdown li.active > a').find(".a4dn-tab-title").html(title);
        $('#a4dn-main-module-explorer-dropdown-text').find(".a4dn-tab-title").html(title);

        // DEV NOTE: <select> elements fire 'input' events when the selection changes, and 'change' events when the control loses focus.
        // However, only some newer browsers support it. https://developer.mozilla.org/en-US/docs/Web/Events/input#Browser_compatibility
        // We don't want to process the change multiple times, so we have to include <select> in the 'change' events.

        // Detect Change for input and textarea
        $form.on('input', 'input,textarea', function () {
            a4dn.core.mvc.am_InputControlChanged(guid, $(this));
        });

        // Detect Change for fields that don't/won't fire input events: checkboxes and radios, datepickers, timepickers
        $form.on('change', "select,[type=checkbox],[type=radio],input.hasDatepicker,input.bootstrap-timepicker", function () {
            a4dn.core.mvc.am_InputControlChanged(guid, $(this));
        });

        $form.on('dp.change', ".datetimepicker", function () {  // https://github.com/Eonasdan/bootstrap-datetimepicker/blob/master/docs/Events.md
            a4dn.core.mvc.am_InputControlChanged(guid, $(this));
        });

        $('.bootstrap-timepicker').timepicker({
            template: false,
            defaultTime: false,
        });

        if ($form.data("a4dn-isvalid") === false || $form.data("a4dn-isvalid") === "False" || $form.data("a4dn-title-Mode") === "Copy") {
            // Form has server validation rules - make as dirty - Also mark a copy as dirty
            $form.addClass("a4dn-dirty");
            a4dn.core.mvc.am_SetSaveCommandsEnabled(guid);

            a4dn.core.mvc.am_FormNotificationValidationErrors($form);
        }
        else {
            a4dn.core.mvc.am_SetSaveCommandsDisabled(guid);
        }

        // Validate input, select, and textarea fields on focusout
        $form.on('focusout', 'input,select,textarea', function () {
            a4dn.core.mvc.am_InputControlLostFocus(guid, $(this));

            // Delay so that popup controls such as date pickers have time to set the value.
            var $this = $(this);
            setTimeout(function () { $this.valid(); }, 500);
        });
    }

    if (enablePreview) {
        if (previewInitVisible) {
            var showPrev = a4dn.core.mvc.am_InitializePreviewPane(guid, maxInitializeSplitterCount);
            if (!showPrev) {
                previewSize = 0;
            }
        }
        else {
            a4dn.core.mvc.am_SetupModuleExplorerContentPreviewClickEvents(guid);
            previewSize = 0;
        }
    }
    else {
        previewSize = 0;
    }

    a4dn.core.mvc.am_InitializeSplitter(guid, splitterDirection, previewSize);

    $form.trigger("itemSelected", [a4dn.core.mvc.am_GetDetailBroadcastKeys("OnSearch", $form), $form.data("a4dn-sub-title")]);
    $form.trigger("itemSelectedCount", [a4dn.core.mvc.am_GetDetailBroadcastKeys("OnSearch", $form)]);

    if (window.matchMedia('(max-width: 768px)').matches) {
        var moduleId = _ModuleIdPrefix + guid;

        // force vertical splitter for small screens
        $jq("#" + moduleId).triggerHandler('destroy');
        a4dn.core.mvc.am_MakeHorizontalSplitter(guid);

        a4dn.core.mvc.am_HidePreviewPane(guid);
    }

    if ($module.data('a4dn-commandid') != "PREVIEW") {
        // call module detail afterPageLoad callback
        // Preview is handled in the am_FetchDetailData
        if (typeof commandCallBackNamespace.afterPageLoad == 'function') {
            commandCallBackNamespace.afterPageLoad(guid, $module.data('a4dn-commandid'));
        }
    }

    // show Preview container
    $("#a4dn-module-" + guid + " .a4dn-module-preview").first().removeClass("hidden");
};

a4dn.core.mvc.am_SetupExceptionError = function (options) {
    var guid = options.guid;

    // Hide Preview container
    $("#a4dn-module" + guid + " .a4dn-module-preview").first().addClass("hidden");

    a4dn.core.mvc.am_InitializePreviewPane(guid, 0)

    a4dn.core.mvc.am_InitializeSplitter(guid, 'h', 0);

    if (window.matchMedia('(max-width: 768px)').matches) {
        var moduleId = _ModuleIdPrefix + guid;

        // force vertical splitter for small screens
        $jq("#" + moduleId).triggerHandler('destroy');
        a4dn.core.mvc.am_MakeHorizontalSplitter(guid);

        a4dn.core.mvc.am_HidePreviewPane(guid);
    }

    // show Preview container
    $("#a4dn-module-" + guid + " .a4dn-module-preview").first().removeClass("hidden");
};

a4dn.core.mvc.am_SetupModuleExplorerEvents = function (guid, parentGuid) {
    var $module = a4dn.core.mvc.am_Get$Module(guid);
    var $table = a4dn.core.mvc.am_Get$Table(guid);
    $module.on("itemSelected", $table, function (event, parentKeys, parentTitle) {
        event.stopPropagation();

        var $submoduletabs = $("#a4dn-submoduletabs-" + guid),
            $detail = $submoduletabs.find("[data-a4dn-type='Detail']");

        // Load Detail Preview
        $detail.data("a4dn-dataloaded", "false");

        if ($detail.closest('li').hasClass('active')) {
            $detail.trigger('click');
        }
    });

    var $parentModule = a4dn.core.mvc.am_Get$Module(parentGuid);
    if ($parentModule.length) {
        if ($parentModule.data("a4dn-type") !== "detail") {
            var $parentTable = a4dn.core.mvc.am_Get$Table(parentGuid);

            $parentModule.on("itemSelected", $parentTable, function (event, parentKeys, parentTitle) {
                event.stopPropagation();

                a4dn.core.mvc.am_SetModuleExplorerTitle(guid, parentTitle);

                // Clear Search because of new parent keys
                var $module = a4dn.core.mvc.am_Get$Module(guid);
                $module.removeData("a4dn-search");

                a4dn.core.mvc.am_TryReloadDataTable(guid);
            });
        }
    }

    $("#a4dn-widget-id-" + guid + " .jarviswidget-fullscreen-btn").click(function (e) {
        e.preventDefault();

        a4dn.core.mvc.am_FullScreenToggleWidget("#wid-id-" + guid);
    });

    $("#a4dn-widget-id-" + guid).on('resize', function (e) {
        a4dn.core.mvc.am_AdjustTableColumns(guid);
    });

    $("#a4dn-favorites-link-" + guid).on('click', function (e) {
        e.preventDefault();
        e.stopPropagation();
        a4dn.core.mvc.am_ToggleModuleFavorite($(this));
    });
};

a4dn.core.mvc.am_ToggleModuleFavorite = function ($this) {
    $('.tooltip').remove();

    var $container = $this;

    var moduleNumber = $container.data('a4dn-module-number');
    var appNumber = $container.data('a4dn-app-number');

    var url = "";

    if ($container.hasClass("a4dn-isfavorite")) {
        url = $container.data("a4dn-remove-href");
    }
    else {
        url = $container.data("a4dn-add-href");
    }

    a4dn.core.mvc.am_ajax(url, $container, {
        type: "POST",
        onMessageOnlyResponse: function (data, textStatus, jqXHR) {
            //console.log($container, data);

            if ($container.hasClass("a4dn-isfavorite")) {
                $(".a4dn-favorites-link").filter('[data-a4dn-app-number="' + appNumber + '"]' + '[data-a4dn-module-number="' + moduleNumber + '"]').removeClass("a4dn-isfavorite");
            }
            else {
                $(".a4dn-favorites-link").filter('[data-a4dn-app-number="' + appNumber + '"]' + '[data-a4dn-module-number="' + moduleNumber + '"]').addClass("a4dn-isfavorite");
            }

            // Mark Favorites modules to be reloaded
            $(".a4dn-nav-favorites-modules").data("a4dn-dataloaded", false);

            a4dn.core.mvc.am_ReloadModuleExplorerNewTabs();

            if (data.message != "") {
                a4dn.core.mvc.am_Notification("extraSmallBox", "Success", data.message);
            }
        }
    });
}

a4dn.core.mvc.am_ReloadModuleExplorerNewTabs = function () {
    //Refresh New Tabs
    $('.a4dn-module-explorer-new-tab').each(function () {
        var $container = $(this);

        var url = $container.data('a4dn-view-href');

        a4dn.core.mvc.am_ajax(url, null, {
            onSuccess: function (data) {
                //console.log($container, data);

                $container.html($(data.markup));
            }
        });
    });
}

a4dn.core.mvc.am_SetupModuleExplorerContentPreviewClickEvents = function (guid) {
    $("#preview-wid-id-" + guid + " .jarviswidget-fullscreen-btn").click(function (e) {
        e.preventDefault();

        a4dn.core.mvc.am_FullScreenToggle();
    });

    $("#a4dn-submodule-dropdown-" + guid).on("click", "a", function (e) {
        e.preventDefault();

        var $this = $(this);

        let moduleId = _ModuleIdPrefix + guid,
            $jqModule = $jq("#" + moduleId);

        var dataHeight = $module.height();

        var href = $this.attr('href');

        if (href == "#Detail") {
            $("#" + moduleId).find('.a4dn-module-detail-content').removeClass('hidden');

            $jqModule.children('.a4dn-splitter-pane-1').first().css("height", dataHeight);
            $jqModule.children('.a4dn-splitter-pane-2').first().css("height", 0);
            $jqModule.trigger("resize");
        }
        else {
            // Set Preview Height to full height
            $jqModule.children('.a4dn-splitter-pane-1').first().css("height", 0);
            $jqModule.children('.a4dn-splitter-pane-2').first().css("height", dataHeight);
            $jqModule.trigger("resize");

            // Set detail hidden
            $("#" + moduleId).find('.a4dn-module-detail-content').addClass('hidden');

            $("#a4dn-submoduletabs-" + guid).find('a[href$=' + href + ']').trigger('click'); // Select tab

            $('#a4dn-preview-widget-grid-' + guid).removeClass("hidden");
        }

        $("#a4dn-submodule-dropdown-text-" + guid).html($this.find('.a4dn-tab-info').html());

        $("#a4dn-submodule-dropdown-" + guid + " li").removeClass("active");
        $this.closest('li').addClass("active");
    });

    for (var i = 0; i < 5; i++) {
        // Add handling to multiple sub module tabs ID - this is needed to support multiple preview tabs
        let idIncrement = i;
        if (i === 0) {
            idIncrement = "";
        }
        if ($("#a4dn-submoduletabs-" + guid + idIncrement).length > 0) {
            $("#a4dn-submoduletabs-" + guid + idIncrement).on("click", "a", function (e) {
                e.preventDefault();

                var $this = $(this);

                var moduleId = _ModuleIdPrefix + guid;
                var modType = $("#" + moduleId).data('a4dn-type');
                var moduleNumber = $this.data("a4dn-modulenumber");
                var url = $this.data("a4dn-html-view-href");

                var parentGuid = $this.data("a4dn-parent-guid");
                if (parentGuid != "") {
                    url = url + a4dn.core.mvc.am_GetSearchParentKeys(parentGuid);
                }

                var vw = $this.data("a4dn-view-name");
                if (typeof vw !== "undefined") {
                    url = url + "&pVwNm=" + $this.data("a4dn-view-name");
                }

                var href = $this.attr("href");
                var $container = $(href);

                if ($this.data("a4dn-type") == "Detail") {
                    if ($this.data("a4dn-viewloaded") == "true") {
                        if ($this.data("a4dn-dataloaded") == "false") {
                            // load data for selected record
                            var $modDetail = $container.find('.a4dn-module-detail').first();
                            var detailGuid = $modDetail.data('a4dn-guid');
                            var moduleName = $modDetail.data('a4dn-mod-name');
                            var titleMode = $modDetail.data('a4dn-title-mode');
                            var fetchHref = $this.data("a4dn-fetch-href") + "&pModNm=" + moduleName + "&pTitleMode=" + titleMode;

                            a4dn.core.mvc.am_FetchDetailDataForPreview(guid, detailGuid, fetchHref);

                            $this.data("a4dn-dataloaded", "true");
                            return;
                        }

                        return;
                    }

                    // Get Detail Preview HTML for selected record
                    var $table = a4dn.core.mvc.am_Get$Table(guid);
                    var $row = a4dn.core.mvc.am_GetFocusRow($table);
                    if ($row.length == 0) {
                        return;
                    }
                    var table = $table.DataTable();
                    url = url + "&pUNm=" + table.cell($row, "A4DN_UniqueName:name").data() + "&pUKy=" + table.cell($row, "A4DN_UniqueKey:name").data();

                    // Set Default Height of Tab Content
                    $('#a4dn-submoduletabcontent-' + guid).css("height", $('#a4dn-submoduletabcontent-' + guid).closest('.a4dn-splitter-pane-2').height() - $('#a4dn-submoduletabs-' + guid).height());
                    $('#a4dn-submoduletabcontent-' + guid).find('.a4dn-sub-module-loading').first().removeClass('hidden');
                }
                else {
                    // Explorer Data
                    if ($this.data("a4dn-viewloaded") == "true") {
                        if ($this.data("a4dn-dataloaded") == "false") {
                            var $moduleExplorer = $(href).find('.a4dn-module-explorer').filter('[data-a4dn-mod-number="' + moduleNumber + '"]').first();

                            a4dn.core.mvc.am_ReloadDataTable({ guid: $moduleExplorer.data('a4dn-guid') });
                            $this.data("a4dn-dataloaded", "true");
                        }
                        return;
                    }

                    // Set Default Height of Tab Content
                    $('#a4dn-submoduletabcontent-' + guid).css("height", $('#a4dn-submoduletabcontent-' + guid).closest('.a4dn-splitter-pane-2').height() - $('#a4dn-submoduletabs-' + guid).height());

                }

                a4dn.core.mvc.am_ajax(url, $container, {
                    onSuccess: function (data) {
                        $container.html($(data.markup));

                        let $form = $container.find(".a4dn-form");
                        a4dn.core.mvc.am_InitializeDropdownControls($form);
                        a4dn.core.mvc.am_InitializeDateTimePickers($form);
                        $this.trigger("a4dn-submodule-pane-initialized");
                    },
                    beforeSend: function (jqXHR, ajaxSettings) {
                        $('#a4dn-submoduletabcontent-' + guid).find('.a4dn-sub-module-loading').first().removeClass('hidden');

                    },
                    complete: function (jqXHR, textStatus) {
                        var moduleExplorerId = _ModuleIdPrefix + guid;

                        // Resize Content
                        $jq("#" + moduleExplorerId).trigger("resize");
                        a4dn.core.mvc.am_ResizeGrid(guid);
                        $('#a4dn-submoduletabcontent-' + guid).find('.a4dn-sub-module-loading').first().addClass('hidden');

                        // Resize Content again with delay just in case
                        waitForFinalEvent(function () {
                            $jq("#" + moduleExplorerId).trigger("resize");
                            a4dn.core.mvc.am_ResizeGrid(guid);
                        }, 500, guid);
                    },
                });

                $this.data("a4dn-viewloaded", "true");
                $this.data("a4dn-dataloaded", "true");
            });
        }
    }
};

a4dn.core.mvc.am_SetModuleExplorerTitle = function (guid, parentTitle) {
    var moduleId = _ModuleIdPrefix + guid;
    var modType = $("#" + moduleId).data('a4dn-type');

    if (typeof modType === "undefined") {
        return;
    }

    //title
    var $widget = $("#a4dn-widget-id-" + guid);
    var $title = $widget.find('.a4dn-module-Exp-title').first();
    var modName = $title.data('a4dn-module-Name');

    if ($title.length) {
        if (modType === "search-explorer") {
            $title.html(modName + " " + parentTitle);
        }
        //else {
        //    $title.html(modName + " " + $form.data("a4dn-sub-title"));
        //}
    }
};

a4dn.core.mvc.am_LoadModuleExplorerContentPreviewFirstTab = function (guid) {

    for (var i = 0; i < 5; i++) {
        // Add handling to multiple sub module tabs ID - this is needed to support multiple preview tabs
        let idIncrement = i;
        if (i === 0) {
            idIncrement = "";
        }

        $("#a4dn-submoduletabs-" + guid + idIncrement + " li").removeClass("active");
        $("#a4dn-submoduletabs-" + guid + idIncrement + " a:first").tab('show'); // Select first tab
        $("#a4dn-submoduletabs-" + guid + idIncrement + " a:first").trigger('click');
    }

};

a4dn.core.mvc.am_InitializeSplitter = function (guid, splitterDirection, previewSize) {
    //Determine if the splitter should show and splitter direction
    var moduleId = _ModuleIdPrefix + guid,
        $module = $('#' + moduleId);

    if (splitterDirection == "Alternate") {
        // Alternate
        splitterDirection = _NextSplitterType;
    }

    if (splitterDirection == "Horizontal") {
        // Make Horizontal Splitter
        a4dn.core.mvc.am_MakeHorizontalSplitter(guid, previewSize);

        _NextSplitterType = "Vertical";
    }
    else {
        // Make Vertical Splitter
        a4dn.core.mvc.am_MakeVerticalSplitter(guid, previewSize);

        _NextSplitterType = "Horizontal";
    }

    waitForFinalEvent(function () {

        a4dn.core.mvc.am_ResizeGrid(guid);
    }, 500, guid);

    let $moduleChildMain = $module.children(_ContentMainClass).first(),
        $moduleChildPreview = $module.children(_ContentPreviewClass).first();

    $moduleChildMain.resize(function () {
        a4dn.core.mvc.am_ResizeGrid(guid, function () {
            //find closest splitter child
            $moduleChildPreview.find('.a4dn-module').each(function () {
                var pGuid = $(this).data("a4dn-guid");
                a4dn.core.mvc.am_ResizeGrid(pGuid);
            });
        });
    });
};

a4dn.core.mvc.resizeFlags = {};

a4dn.core.mvc.am_ResizeGrid = function (guid, then) {
    if (a4dn.core.mvc.resizeFlags[guid] === true) {
        return;
    }

    a4dn.core.mvc.resizeFlags[guid] = true;

    let moduleId = _ModuleIdPrefix + guid,
        $moduleDiv = $("#" + moduleId),
        $splitterContentMain = $("#" + moduleId).children(_ContentMainClass).first(),
        $splitterContentPreview = $("#" + moduleId).children(_ContentPreviewClass).first(),
        margin1 = $splitterContentMain.children(".a4dn-module-content").first().outerHeight(true)
            - $splitterContentMain.children(".a4dn-module-content").first().height(),
        contentHeight1 = $splitterContentMain.outerHeight(true)
            - margin1,
        margin2 = $splitterContentPreview.children(".a4dn-module-preview").first().outerHeight(true)
            - $splitterContentPreview.children(".a4dn-module-preview").first().height(),
        contentHeight2 = $splitterContentPreview.outerHeight(true)
            - margin2,
        $parentTabContent = $moduleDiv.closest('.tab-content').first(),
        $moduleContentHeader = $('#a4dn-module-content-header-' + guid),
        parentTabContentHeight = $moduleDiv.height();

    if ($moduleContentHeader.length) {
        parentTabContentHeight += $moduleContentHeader.outerHeight(true) + 10;
    }

    let dataHeight, $dataElement;
    let a4dnWidgetHeaderHeight = $splitterContentMain.find('.a4dn-widget-header').first().outerHeight(true),
        a4dnToolbarHeight = $splitterContentMain.find('.a4dn-toolbar').first().outerHeight(true),
        dataTablesScrollHeadHeight = $splitterContentMain.find('.dataTables_scrollHead').first().outerHeight(true),
        a4dnToolbarFooterHeight = $splitterContentMain.find('.a4dn-toolbar-footer').first().outerHeight(true),
        a4dnPanelHeight = $splitterContentMain.find('.a4dn-panel').first().outerHeight(true);

    // Chrome will bounce between 44 and 45 px on consecutive calls to am_ResizeGrid, so force the footer
    // to be at least 45px if it is not 0px.
    if (a4dnToolbarFooterHeight > 0 && a4dnToolbarFooterHeight < 45) {
        a4dnToolbarFooterHeight = 45;
    }

    switch ($moduleDiv.data('a4dn-type')) {
        case "search-explorer":
            $jq("#" + moduleId).trigger("resize");
            dataHeight = contentHeight1 - a4dnWidgetHeaderHeight - a4dnToolbarHeight - dataTablesScrollHeadHeight - a4dnToolbarFooterHeight - a4dnPanelHeight;
            $dataElement = $splitterContentMain.find(".dataTables_scrollBody").first();
            break;

        case "detail":
            dataHeight = contentHeight1 - a4dnWidgetHeaderHeight - a4dnToolbarHeight - 33;
            $dataElement = $splitterContentMain.find(".a4dn-scroll-body").first();
            break;

        case "exception":
        case "custom-explorer":
        case "web-explorer":
            dataHeight = contentHeight1;
            $dataElement = $splitterContentMain.find(".a4dn-scroll-body").first();
            break;
    }

    window.requestAnimationFrame(function () {
        // Set Main Grid
        $splitterContentMain.children(".a4dn-module-content").first().css("height", contentHeight1);
        $splitterContentPreview.children(".a4dn-module-preview").first().css("height", contentHeight2);

        // Set Data Element
        if (dataHeight !== undefined && $dataElement !== undefined && $dataElement.length > 0) {
            $dataElement.css("height", dataHeight);
        }

        // Set parent tab-content
        $parentTabContent.css("height", parentTabContentHeight);

        // Run post-resize callback if provided
        if (then) {
            then();
        }

        delete a4dn.core.mvc.resizeFlags[guid];
    });
};

a4dn.core.mvc.am_InitializePreviewPane = function (guid, maxInitializeSplitterCount) {
    var showPrev = false;

    var moduleId = _ModuleIdPrefix + guid;

    var $modExpTab = $("#" + moduleId).closest(".a4dn-module-explorer-tab-pane");

    if ($('#a4dn-preview-widget-grid-' + guid).length) {
        // has Preview

        if (window.matchMedia('(max-width: 768px)').matches) {
            // No Preview on small devices
            maxInitializeSplitterCount = 0;
            $('#a4dn-preview-unavailable-' + guid).addClass("hidden");
            $('#a4dn-preview-widget-grid-' + guid).removeClass("hidden");
            a4dn.core.mvc.am_SetupModuleExplorerContentPreviewClickEvents(guid);
            return false;
        }

        showPrev = false;

        var numSplitterItems = $modExpTab.find('.splitter').length;

        a4dn.core.mvc.am_SetupModuleExplorerContentPreviewClickEvents(guid);

        if (numSplitterItems <= maxInitializeSplitterCount) {
            // Has preview and within splitter count
            showPrev = true;
            a4dn.core.mvc.am_LoadModuleExplorerContentPreviewFirstTab(guid);
        }
        else {
            showPrev = false;
        }

        $('#a4dn-preview-unavailable-' + guid).addClass("hidden");
        $('#a4dn-preview-widget-grid-' + guid).removeClass("hidden");
    }
    else {
        // no preview
        $('#a4dn-preview-unavailable-' + guid).addClass("hidden");
    }

    if (showPrev) {
        var $toolbarCommands = $("#" + moduleId + " .a4dn-toolbar-commands").first();

        $toolbarCommands.find(".a4dn-command" + "[data-a4dn-command-id='PREVIEW']").each(function () {
            var $this = $(this);

            $this.data("a4dn-checked-state", "true");
            $this.removeClass("btn-default");
            $this.addClass("btn-primary");
        });
    }

    return showPrev;
}

a4dn.core.mvc.am_SetPreviewCommandCheckState = function (guid) {
    var moduleId = _ModuleIdPrefix + guid;

    var $preview = $('#a4dn-preview-widget-grid-' + guid);

    if ($preview.length && $preview.is(":visible")) {
        var $toolbarCommands = $("#" + moduleId + " .a4dn-toolbar-commands").first();

        $toolbarCommands.find(".a4dn-command" + "[data-a4dn-command-id='PREVIEW']").each(function () {
            var $this = $(this);

            $this.data("a4dn-checked-state", "true");
            $this.removeClass("btn-default");
            $this.addClass("btn-primary");
        });
    }
}

a4dn.core.mvc.am_ShowPreviewPane = function (guid) {
    var moduleId = _ModuleIdPrefix + guid;

    var $toolbarCommands = $("#" + moduleId + " .a4dn-toolbar-commands").first();

    $toolbarCommands.find(".a4dn-command" + "[data-a4dn-command-id='PREVIEW']").each(function () {
        var $this = $(this);
        $this.data("a4dn-checked-state", "true");
        $this.removeClass("btn-default");
        $this.addClass("btn-primary");
    });

    var splitterType = $("#" + moduleId).data("a4dn-splitter-type");

    if (splitterType == "Horizontal") {
        $jq("#" + moduleId).triggerHandler('destroy');

        a4dn.core.mvc.am_InitializeSplitter(guid, "Horizontal");
    }
    else {
        $jq("#" + moduleId).triggerHandler('destroy');

        a4dn.core.mvc.am_InitializeSplitter(guid, "Vertical");
    }

    a4dn.core.mvc.am_LoadModuleExplorerContentPreviewFirstTab(guid);
}

a4dn.core.mvc.am_HidePreviewPane = function (guid) {
    var moduleId = _ModuleIdPrefix + guid;

    var $toolbarCommands = $("#" + moduleId + " .a4dn-toolbar-commands").first();

    $toolbarCommands.find(".a4dn-command" + "[data-a4dn-command-id='PREVIEW']").each(function () {
        var $this = $(this);
        $this.data("a4dn-checked-state", "false");
        $this.addClass("btn-default");
        $this.removeClass("btn-primary");
    });

    var splitterType = $("#" + moduleId).data("a4dn-splitter-type");

    $("#" + moduleId + " .splitter-bar").first().addClass("hidden");

    // remove Module Explorer
    $("#" + moduleId + " " + _ContentPreviewClass).find('.a4dn-module').each(function () {
        var $this = $(this);

        $("#a4dn-submoduletabs-" + guid).find('a').data("a4dn-viewloaded", "false");
        $("#a4dn-submoduletabs-" + guid).find('a').data("a4dn-dataloaded", "false");

        $("#" + moduleId).off("itemSelected");

        a4dn.core.mvc.am_CleanupModalSearchTemplateFromStorage($this.data('a4dn-guid'));

        $this.remove();
    });

    $('#a4dn-preview-widget-grid-' + guid).addClass('hidden');

    a4dn.core.mvc.am_SplitterDock(guid);
}

a4dn.core.mvc.am_MakeVerticalSplitter = function (guid, sizeRightWeight) {
    let moduleId = _ModuleIdPrefix + guid,
        $jqModule = $jq("#" + moduleId),
        $module = $("#" + moduleId);

    var anchorToWindow = $module.data("a4dn-anchortowindow");

    if (anchorToWindow == "False") { anchorToWindow = false; }
    else { anchorToWindow = true; }

    $module.data("a4dn-splitter-type", "Vertical");

    $jqModule.splitter({
        type: "v",
        outline: true,
        sizeRight: true,
        //minLeft: 20,
        //minRight: 20,
        anchorToWindow: anchorToWindow,
        resizeToWidth: true,
        dock: "right",
        dockSpeed: 0,
        // cookie: "docksplitter",
        //dockKey: 'Z',	// Alt-Shift-Z in FF/IE
        //accessKey: 'I'	// Alt-Shift-I in FF/IE
    });

    if (sizeRightWeight > 0) {
        $module.find(".splitter-bar").first().show();
    }

    // add Buttons and events
    // ==================
    $module.find(".splitter-bar").first().append(' <div class="split-bar-vertical-button" unselectable="on"></div>');
    $module.find(".splitter-bar").first().append(' <div class="split-bar-vertical-rotate" unselectable="on"><i class="fa fa-caret-up fa-rotate-270"></i></div>');

    $module.find("> .splitter-bar .split-bar-vertical-button").mousedown(function (e) {
        e.stopPropagation();
    })

    $module.find("> .splitter-bar .split-bar-vertical-button").click(function (e) {
        e.stopPropagation();

        a4dn.core.mvc.am_ToggleSplitterDock(guid);
    })

    $module.find("> .splitter-bar .split-bar-vertical-rotate").click(function (e) {
        e.stopPropagation();

        $jqModule.triggerHandler('destroy');
        a4dn.core.mvc.am_InitializeSplitter(guid, "Horizontal");
    })
    // ==================

    var splitterContentWidth = 0;
    var splitterPreviewWidth = 0;

    if (typeof sizeRightWeight === "undefined") {
        sizeRightWeight = .5;
    }

    splitterPreviewWidth = $module.width() * sizeRightWeight;
    splitterContentWidth = $module.width() - splitterPreviewWidth;

    if (splitterPreviewWidth > 0) {
        $module.find(".splitter-bar").first().show();
    }

    $jqModule.children(".a4dn-splitter-pane-1").first().css("width", splitterContentWidth);
    $jqModule.children(".a4dn-splitter-pane-2").first().css("width", splitterPreviewWidth);
    $jqModule.trigger('resize');
}

a4dn.core.mvc.am_MakeHorizontalSplitter = function (guid, sizeBottomWeight) {
    let moduleId = _ModuleIdPrefix + guid,
        $jqModule = $jq("#" + moduleId),
        $module = $("#" + moduleId);

    $module.data("a4dn-splitter-type", "Horizontal");

    var anchorToWindow = $module.data("a4dn-anchortowindow");

    if (anchorToWindow == "False") { anchorToWindow = false; }
    else { anchorToWindow = true; }

    // Create Splitter
    $jqModule.splitter({
        type: "h",
        outline: true,
        sizeBottom: true,
        anchorToWindow: anchorToWindow,
        resizeToWidth: true,
        // minTop: 200,
        //minBottom: 100,
        dock: "bottom",
        dockSpeed: 0,
        // cookie: "docksplitter",
        // dockKey: 'Z',	// Alt-Shift-Z in FF/IE
        // accessKey: 'I'	// Alt-Shift-I in FF/IE
    });

    // add Buttons and events
    // ==================
    $module.find(".splitter-bar").first().append(' <div class="split-bar-horiontal-button" unselectable="on"></div>');
    $module.find(".splitter-bar").first().append(' <div class="split-bar-horiontal-rotate" unselectable="on"><i class="fa fa-caret-up"></i></div>');

    $module.find("> .splitter-bar .split-bar-horiontal-button").mousedown(function (e) {
        e.stopPropagation();
    })

    $module.find("> .splitter-bar .split-bar-horiontal-button").click(function (e) {
        e.stopPropagation();
        a4dn.core.mvc.am_ToggleSplitterDock(guid)
    })

    $module.find("> .splitter-bar .split-bar-horiontal-rotate").click(function (e) {
        e.stopPropagation();
        $jqModule.triggerHandler('destroy');
        a4dn.core.mvc.am_InitializeSplitter(guid, "Vertical");
    })
    // ==================

    var splitterContentHeight = 0;
    var splitterPreviewHeight = 0;

    if (typeof sizeBottomWeight === "undefined") {
        sizeBottomWeight = .5;
    }

    splitterPreviewHeight = Math.round($module.height() * sizeBottomWeight);
    splitterContentHeight = Math.round($module.height() - splitterPreviewHeight);

    if (splitterPreviewHeight > 0) {
        $module.find(".splitter-bar").first().show();
    }

    $jqModule.children(".a4dn-splitter-pane-1").first().css("height", splitterContentHeight);
    $jqModule.children(".a4dn-splitter-pane-2").first().css("height", splitterPreviewHeight);
    $jqModule.trigger('resize');
}

a4dn.core.mvc.am_ToggleSplitterDock = function (guid) {
    var moduleId = _ModuleIdPrefix + guid;

    var docked = $("#" + moduleId).data("a4dn-docked");
    if (docked == "true") {
        a4dn.core.mvc.am_SplitterUnDock(guid);
    }
    else {
        a4dn.core.mvc.am_SplitterDock(guid);
    }
}

a4dn.core.mvc.am_SplitterDock = function (guid) {
    var moduleId = _ModuleIdPrefix + guid;

    var splitterType = $("#" + moduleId).data("a4dn-splitter-type");
    if (splitterType == "Horizontal") {
        $("#" + moduleId + " > .splitter-bar .split-bar-horiontal-button").first().addClass('invert');
        //remember content preview size height for undock
        $("#" + moduleId).data("a4dn-last-undocked-size", $jq("#" + moduleId + " .a4dn-splitter-pane-2").first().height());
    }
    else {
        $("#" + moduleId + " > .splitter-bar .split-bar-vertical-button").first().addClass('invert');
        //remember content preview size height for undock
        $("#" + moduleId).data("a4dn-last-undocked-size", $jq("#" + moduleId + " .a4dn-splitter-pane-2").first().width());
    }

    $jq("#" + moduleId).triggerHandler('dock');
    $("#" + moduleId).data("a4dn-docked", "true");
}

a4dn.core.mvc.am_SplitterUnDock = function (guid) {
    let moduleId = _ModuleIdPrefix + guid,
        $jqModule = $jq("#" + moduleId),
        $module = $("#" + moduleId);

    var splitterType = $module.data("a4dn-splitter-type");
    if (splitterType == "Horizontal") {
        $module.find("> .splitter-bar .split-bar-horiontal-button").first().removeClass('invert');

        // get last preview height
        var splitterPreviewHeight = $module.data("a4dn-last-undocked-size");
        var splitterContentHeight = $module.height() - splitterPreviewHeight;

        $jqModule.children(".a4dn-splitter-pane-1").first().css("height", splitterContentHeight);
        $jqModule.children(".a4dn-splitter-pane-2").first().css("height", splitterPreviewHeight);
        $jqModule.trigger('resize');
    }
    else {
        $module.find("> .splitter-bar .split-bar-vertical-button").first().removeClass('invert');

        // get last preview width
        var splitterPreviewWidth = $module.data("a4dn-last-undocked-size");
        var splitterContentWidth = $module.width() - splitterPreviewWidth;

        $jqModule.children(".a4dn-splitter-pane-1").first().css("width", splitterContentWidth);
        $jqModule.children(".a4dn-splitter-pane-2").first().css("width", splitterPreviewWidth);
        $jqModule.trigger('resize');
    }

    //  $jqModule.triggerHandler('undock');

    $module.data("a4dn-docked", "false");
}

a4dn.core.mvc.am_MoveModuleExplorerToolbar = function (guid) {
    var toolbarCommandId = "a4dn-toolbar-commands-" + guid;
    var moduleExplorerId = _ModuleIdPrefix + guid;

    $("#" + moduleExplorerId + " .a4dn-toolbar-tray").first().append($("#" + toolbarCommandId));
}

a4dn.core.mvc.am_InitializeModuleExplorerToolbar = function (guid, commandCallBackNamespace) {
    var toolbarCommandId = "a4dn-toolbar-commands-" + guid;
    var moduleExplorerId = _ModuleIdPrefix + guid;

    $("#" + moduleExplorerId + " .a4dn-toolbar-tray").first().append($("#" + toolbarCommandId));

    // wire up click event
    $("#" + toolbarCommandId + " .a4dn-command").click(function (e, perCallCallbacks) {
        e.preventDefault();

        var $this = $(this);

        $('.tooltip').hide();

        var commandID = $this.data("a4dn-command-id");
        var guid = $this.closest(".a4dn-module-explorer").data('a4dn-guid');
        a4dn.core.mvc.am_HandelModuleExplorerCommand(guid, commandID, $this, {
            beforeProcessCommand: commandCallBackNamespace.beforeProcessCommand,
            afterProcessCommand: commandCallBackNamespace.afterProcessCommand,
            perCallCallbacks: perCallCallbacks
        });
    });

    a4dn.core.mvc.am_OnToolbarResize(guid);

    $("#" + moduleExplorerId).on('resize', a4dn.core.mvc.am_Debounce(function () {
        a4dn.core.mvc.am_OnToolbarResize(guid);
    }, 100));

    a4dn.core.mvc.am_SetPreviewCommandCheckState(guid);
}

a4dn.core.mvc.am_SetFocusFirstInput = function (parent) {
    $(parent).find('input, textarea, select')
        .not('input[type=hidden],input[type=button],input[type=submit],input[type=reset],input[type=image],button')
        .filter(':enabled:visible:first')
        .focus();
}

a4dn.core.mvc.am_GetModuleImageSource = function ($module) {
    return $module.length > 0 ? $module.data('a4dn-image-src') : undefined;
};

a4dn.core.mvc.am_GetModuleName = function ($module) {
    return $module.length > 0 ? $module.data('a4dn-mod-name') : undefined;
};

a4dn.core.mvc.am_GetModuleNumber = function ($module) {
    return $module.length > 0 ? $module.data('a4dn-mod-number') : undefined;
};

a4dn.core.mvc.am_GetApplicationNumber = function ($module) {
    return $module.length > 0 ? $module.data('a4dn-app-number') : undefined;
};

a4dn.core.mvc.am_GetCommandCallbackNamespace = function (guid) {
    var $module = a4dn.core.mvc.am_Get$Module(guid);
    if ($module.length == 0)
        return;

    var callBackNamespace = $module.data('a4dn-commandcallbacknamespace');
    if (!callBackNamespace)
        return;

    if (typeof callBackNamespace == "string")
        callBackNamespace = window[callBackNamespace];

    return callBackNamespace;
};

a4dn.core.mvc.am_InitializeDropdownControls = function ($containers, propertyName) {
    //console.trace("am_InitializeDropdownControls: " + propertyName);

    // Don't initialize hidden dropdowns; we have to ignore any that have a parent .a4dn-field-control-group with the hidden class
    let $dropdowns = $containers.find(".a4dn-field-control-group:not(.hidden) [data-a4dn-initialize='ab_dropdown']");

    if (propertyName)
        $dropdowns = $dropdowns.filter(function (i, elem) { return $(this).attr('name') == propertyName });

    $dropdowns.each(function () {
        let $dropdown = $(this),
            name = $dropdown.attr("name");

        // console.debug("$dropdown for " + name, $dropdown);

        // $dropdown.removeData("AB_Dropdown");
        if (!$dropdown.data("AB_Dropdown")) {
            $dropdown.AB_Dropdown();
        }
        else {
            //console.warn("Dropdown " + name + " already initialized", $dropdown);
            //console.trace();
        }
    });
}

a4dn.core.mvc.am_InitializeDateTimePickers = function ($containers) {
    $containers.find(".datetimepicker").each(function () {
        var $input = $(this),
            format = $input.data("format");
        // TODO: other data- attributes we want to pass as options to datetimepicker(). See https://github.com/Eonasdan/bootstrap-datetimepicker/blob/master/docs/Options.md

        if (!$input.data("datetimepicker")) {
            $input.datetimepicker({
                format: format,
                date: $input.val(),
                useCurrent: false
            });
            $input.on("dp.show", function (e) {
                $input.addClass("a4dn-readonly").prop('readonly', true);
            });
            $input.on("dp.hide", function (e) {
                $input.removeClass("a4dn-readonly").prop('readonly', false);
            });
        }
    });
}

a4dn.core.mvc.am_MoveModalSearchTemplateToStorage = function (guid) {
    let $label = $("#a4dn-search-modal-label-" + guid),
        $body = $("#a4dn-search-modal-body-" + guid),
        $storage = $("#a4dn-search-modal-storage");

    if ($label.parents("#a4dn-search-modal-storage").length === 0) {
        $storage.append($label);
    }

    if ($body.parents("#a4dn-search-modal-storage").length === 0) {
        $storage.append($body);
    }
}

a4dn.core.mvc.am_CleanupModalSearchTemplateFromStorage = function (guid) {
    let $label = $("#a4dn-search-modal-label-" + guid),
        $body = $("#a4dn-search-modal-body-" + guid),
        $storage = $("#a4dn-search-modal-storage");

    if ($label.parents("#a4dn-search-modal-storage").length === 1) {
        $label.remove();
    }

    if ($body.parents("#a4dn-search-modal-storage").length === 1) {
        $body.remove();
    }
}

a4dn.core.mvc.am_ShowModalSearch = function (guid) {
    // console.debug("am_ShowModalSearch(" + guid + ")");

    let $modal = $("#a4dn-search-modal"),
        $modalLabel = $("#a4dn-search-modal-label"),
        $modalBody = $("#a4dn-search-modal-body"),
        labelSelector = "#a4dn-search-modal-label-" + guid,
        bodySelector = "#a4dn-search-modal-body-" + guid;

    $modalLabel.html($(labelSelector).clone(true, true).html());
    $modalBody.html($(bodySelector).clone(true, true).html());

    // Hide .a4dn-dropdown-btn on search explorers
    $modalBody.find('button.a4dn-command.a4dn-dropdown-btn').hide();

    let $form = $modalBody.find(".a4dn-search-form");
    a4dn.core.mvc.am_InitializeDropdownControls($form);
    a4dn.core.mvc.am_InitializeDateTimePickers($form);

    // TODO: Look at the code below and divide it into things that only need to run once when the search
    // explorer for a module is first loaded, and things that need to run each time the search explorer
    // for a module is displayed. We shouldn't have to reattach event handlers every time we open
    // the modal; the handlers should be attached to the outer boilerplate, and figure out which module
    // to work with when they are executed. $(".a4dn-search-form").data("a4dn-guid") could help with
    // that; it contains the guid that was passed into am_ShowModalSearch().
    // 
    // If anything can be executed just once, and it's not module-specific, it should be moved
    // to the window initialization function.


    // Populate Search Form with parent Keys
    a4dn.core.mvc.am_PopulateSearchFormWithParentKeys(guid);

    let callBackNamespace = a4dn.core.mvc.am_GetCommandCallbackNamespace(guid);

    if (callBackNamespace && typeof callBackNamespace.beforeShowModalSearch == 'function') {
        callBackNamespace.beforeShowModalSearch($modal, guid);
    }

    // To support search dialogs that use tabs with multiple forms, selectors below use form:visible to
    // get the form that's currently active.

    // Set Focus - Move this to modal form show
    setTimeout(function () { a4dn.core.mvc.am_SetFocusFirstInput('#a4dn-search-modal-body form:visible') }, 1000);

    var $module = a4dn.core.mvc.am_Get$Module(guid);
    var $table = a4dn.core.mvc.am_Get$Table(guid);
    var table = $table.DataTable();

    // Add existing Search Data
    // TODO: support multiple forms with their own search data
    var searchData = $module.data("a4dn-search");
    if (typeof searchData !== "undefined") {
        a4dn.core.mvc.am_Deserialize(searchData, $modal);
    }

    // TODO: restore previously-set filterKeys?

    $('.bootstrap-timepicker').timepicker({
        template: false,
        defaultTime: false,
    });

    $('#a4dn-search-model-search-btn').off();
    $('#a4dn-search-model-search-btn').on('click', function (e) {
        e.stopPropagation();

        let searchData = $('#a4dn-search-modal-body form:visible').serialize(); // Grab before hiding form 

        $modal.modal('hide');

        a4dn.core.mvc.am_Search({ guid: guid, searchData: searchData });
    });

    $('#a4dn-search-model-reset-btn').off();
    $('#a4dn-search-model-reset-btn').on('click', function (e) {
        e.stopPropagation();
        $modalBody.html($('#a4dn-search-modal-body-' + guid).clone().html());

        if (callBackNamespace && typeof callBackNamespace.beforeShowModalSearch == 'function') {
            callBackNamespace.beforeShowModalSearch($modal, guid);
        }

        // Populate Search Form with parent Keys
        a4dn.core.mvc.am_PopulateSearchFormWithParentKeys(guid);

        // Hide .a4dn-dropdown-btn on search explorers
        $modalBody.find('button.a4dn-command.a4dn-dropdown-btn').hide();

        // Re-initialize dropdowns
        let $form = $modalBody.find(".a4dn-search-form");
        a4dn.core.mvc.am_InitializeDropdownControls($form);
        a4dn.core.mvc.am_InitializeDateTimePickers($form);

        let $module = a4dn.core.mvc.am_Get$Module(guid);
        $module.data("a4dn-search");

        if (callBackNamespace && typeof callBackNamespace.afterShowModalSearch == 'function') {
            callBackNamespace.afterShowModalSearch($modal, guid);
        }
    });

    $modal.off();
    $modal.keydown(function (e) {
        e.stopPropagation();

        if (e.which == 13) {//enter
            $('#a4dn-search-model-search-btn').trigger('click');
        }
    });

    $modal.on("hidden.bs.modal", function () {
        $('#a4dn-search-modal-search-btn').off('click'); // remove handler
        $modal.off('keydown'); // remove handler
    });

    $modal.find('.a4dn-clear-input-button').on("click", function () {
        var inputName = $(this).data('a4dn-clear-input-name')
        $modal.find("[name='" + inputName + "']").val("").trigger('change');
    });

    if (callBackNamespace && typeof callBackNamespace.afterShowModalSearch == 'function') {
        callBackNamespace.afterShowModalSearch($modal, guid);
    }
}

a4dn.core.mvc.am_CreateOrActivateModuleExplorerDetailTab = function (options) {
    var applicationNumber = options.applicationNumber;
    var moduleNumber = options.moduleNumber;
    var uniqueName = options.uniqueName;
    var uniqueKey = options.uniqueKey;
    var commandID = options.commandID;
    var moduleTitle = options.moduleTitle;

    var $tab = a4dn.core.mvc.am_Get$ModuleExplorerDetailTab(moduleNumber, uniqueKey);

    if ($tab.length) {
        // Tab Open - just Activate
        $tab.tab('show');

        var $itm = $("#a4dn-main-module-explorer-dropdown").find('span:contains("' + moduleTitle + '")').first();
        $itm.trigger("click");
    }
    else {

        var trackUserSessions = $('#a4dn-main-content').data('a4dn-userjoblog-track-user-sessions');
        var recordLockingEnabled = $('#a4dn-main-content').data('a4dn-userjoblog-record-locking-enabled');

        options.AddHistory = true;

        if (commandID === "OPEN" && trackUserSessions === "True" && recordLockingEnabled === "True") {
            a4dn.core.mvc.am_CreateRecordLock(options);
        }
        else {

            // Create Module Explorer Detail Tab
            a4dn.core.mvc.am_CreateModuleExplorerDetailTab(options);
        }
    }
}

a4dn.core.mvc.am_CreateModuleExplorerDetailTab = function (options) {
    var url = options.url;
    var moduleTitle = a4dn.core.mvc.am_htmlEscape(options.moduleTitle);
    var moduleImageSource = options.moduleImageSource;

    var tabContentContainerID = "mdtl-" + a4dn.core.mvc.am_GenerateGuid();
    var imageElement = '<img src="' + moduleImageSource + '" />';

    // var modNam = moduleName + " Detail";

    $(".a4dn-main-module-loading").removeClass("hidden");

    $("#a4dn-main-module-explorer-tabs li").removeClass("active");
    $("#a4dn-main-module-explorer-dropdown li").removeClass("active")

    $("#a4dn-main-module-explorer-tab-content").append('<div class="a4dn-module-explorer-tab-pane tab-pane active" id="' + tabContentContainerID + '"> </div>');

    // Locate New Tab
    var $newTabli = $('#a4dn-main-module-explorer-tabs').find('li.a4dn-new-explorer-tab');

    // Add Tab before new Tab
    $('<li  data-a4dn-pinned="true"> <a  rel="tooltip" data-placement="bottom" href="#' + tabContentContainerID + '" data-toggle="tab"><span class="a4dn-tab-image">' + imageElement + '</span> <span class="a4dn-tab-title">' + moduleTitle + '</span>  <span class="a4dn-tab-controls">   <i class="fa fa-thumb-tack pinTab a4dn-pinned" data-original-title="' + $('#a4dn-main-content').data('a4dn-lang-togglepin') + '" rel="tooltip" data-placement="bottom"></i> <i class="fa fa-times closeTab" data-original-title="' + $('#a4dn-main-content').data('a4dn-lang-closetab') + '" rel="tooltip" data-placement="bottom"></i></span></a></li>').insertBefore($newTabli);

    // Add to Drop Down
    $("#a4dn-main-module-explorer-dropdown").append('<li  data-a4dn-pinned="true" class="active"> <a href="#' + tabContentContainerID + '" data-toggle="tab"><span class="a4dn-tab-image">' + imageElement + '</span> <span class="a4dn-tab-title">' + moduleTitle + '</span>  <span class="a4dn-tab-controls"> <i class="fa fa-thumb-tack pinTab fa-rotate-90" data-original-title="' + $('#a4dn-main-content').data('a4dn-lang-togglepin') + '" rel="tooltip" data-placement="bottom"></i>|<i class="fa fa-times closeTab" data-original-title="' + $('#a4dn-main-content').data('a4dn-lang-closetab') + '" rel="tooltip" data-placement="bottom"></i></span></a></li>');

    $("#a4dn-main-module-explorer-dropdown-text").html('<span class="a4dn-tab-image">' + imageElement + '</span> <span class="a4dn-tab-title">' + moduleTitle + '</span>');

    $('#a4dn-main-module-explorer-tabs a[href="#' + tabContentContainerID + '"]').tab('show');

    var $container = $("#" + tabContentContainerID);

    a4dn.core.mvc.am_ajax(url, $container, {
        onSuccess: function (data) {
            $(".a4dn-main-module-loading").addClass("hidden");

            //$container.removeClass('loading');

            $container.html($(data.markup));

            let $form = $container.find('.a4dn-form');
            a4dn.core.mvc.am_InitializeDropdownControls($form);
            a4dn.core.mvc.am_InitializeDateTimePickers($form);

            if ($.validator) {
                $.validator.setDefaults({
                    // validate hidden fields
                    ignore: [],
                });

                $.validator.unobtrusive.parse($form.find("form"));  // parse the actual <form> element
            }

            if (options.AddHistory) {
                //Get Updated Unique Name and Title
                var $module = $container.find('.a4dn-module-detail').first();
                options.uniqueName = $module.data('a4dn-unique-name');
                options.moduleTitle = $module.data('a4dn-title');
                a4dn.core.mvc.am_AddHistoryShortcut(options);
            }
        },
        onErrorResponse: function (data, textStatus, jqXHR, $feedbackElement) {
            console.error($feedbackElement, data);
            $(".a4dn-main-module-loading").addClass("hidden");

            if (data.markup === null) {
                // no Exception - issue message and close tab
                if (data.message.toUpperCase().includes("RECORD NOT FOUND")) {
                    // Override Record Not Found Message
                    data.message = 'Record "' + moduleTitle + '" was Not Found. If this error continues to occur, please sign off and back on.';
                }

                a4dn.core.mvc.am_CloseModuleExplorerTab(tabContentContainerID);
            }
            else {
                $container.html(data.markup);
            }

            a4dn.core.mvc.am_Notification("smallBox", "Error", data.message);
        },
        beforeSend: function (jqXHR, ajaxSettings) {
            $(".a4dn-main-module-loading").removeClass("hidden");
        },
        complete: function (jqXHR, textStatus) {
            $(".a4dn-main-module-loading").addClass("hidden");
        },
    });
}

a4dn.core.mvc.am_AddHistoryShortcut = function (options) {
    var applicationNumber = options.applicationNumber;
    var moduleNumber = options.moduleNumber;
    var uniqueKey = options.uniqueKey;
    var uniqueName = options.uniqueName;
    var moduleTitle = options.moduleTitle;

    var url = $('#a4dn-shortcuts-dropdown .a4dn-history-options').data('a4dn-add-href');
    var $container = $('#a4dn-shortcuts-dropdown');

    a4dn.core.mvc.am_ajax(url, $container, {
        type: "POST",
        data: [{ name: "ApplicationNumber", value: applicationNumber },
        { name: "ModuleNumber", value: moduleNumber },
        { name: "FileObjectName", value: uniqueName },
        { name: "FileObjectKey", value: uniqueKey },
        { name: "Title", value: moduleTitle }],
        onSuccess: function (data) {
            if ($container.data('a4dn-loaded') === true) {
                // Remove old items
                $container.find('.a4dn-history-content .a4dn-detail-link').filter('[data-a4dn-mod-number="' + moduleNumber + '"]').filter('[data-a4dn-unique-key="' + uniqueKey + '"]').closest('li.a4dn-tree-list-item').remove();

                var radioChecked = $container.find(".a4dn-history-options input:checked").data('a4dn-groupby');
                switch (radioChecked) {
                    case "ByDate":
                    case "ByModule":
                        var key = "";
                        if (radioChecked === "ByDate") {
                            key = data.output.LastVisitedDate;
                        }
                        else {
                            key = moduleNumber;
                        }

                        var $ulItem = $container.find('.a4dn-history-content ul.a4dn-tree-list').filter('[data-a4dn-key="' + key + '"]');
                        if ($ulItem.length && $ulItem.data('a4dn-loaded') === true) {
                            //If Date group exists, then add it
                            $ulItem.prepend($(data.markup));

                            // Hide if node is collapsed
                            if ($ulItem.data('a4dn-expanded') === false) {
                                $ulItem.find('li.a4dn-tree-list-item').hide('fast');
                            }
                        }
                        else {
                            // trigger reload to get new Data group
                            $container.find('.a4dn-history-options').show();
                            radioChecked = $container.find(".a4dn-history-options input:checked").trigger("click");
                        }

                        break;
                    case "ByMostRecent":

                        $ulItem = $container.find('.a4dn-history-content ul.a4dn-tree-single-list')
                        $ulItem.prepend($(data.markup));

                        break;
                    default:
                }
            }
        }
    });
};

a4dn.core.mvc.am_AddHotListShortcut = function (options) {
    var applicationNumber = options.applicationNumber;
    var moduleNumber = options.moduleNumber;
    var uniqueKey = options.uniqueKey;
    var uniqueName = options.uniqueName;
    var moduleTitle = options.moduleTitle;

    var url = $('#a4dn-shortcuts-dropdown .a4dn-hotlist-options').data('a4dn-add-href')
    var $container = $('#a4dn-shortcuts-dropdown');

    a4dn.core.mvc.am_ajax(url, $container, {
        type: "POST",
        data: [{ name: "ApplicationNumber", value: applicationNumber },
               { name: "ModuleNumber", value: moduleNumber },
               { name: "FileObjectName", value: uniqueName },
               { name: "FileObjectKey", value: uniqueKey },
               { name: "Title", value: moduleTitle }],
        onSuccess: function (data) {
            //console.log($container, data);

            if (data.message != "") {
                a4dn.core.mvc.am_Notification("extraSmallBox", "Success", data.message);
            }
            if ($container.data('a4dn-loaded') == true) {
                var $ulItem = $container.find('.a4dn-hotlist-content ul.a4dn-tree-single-list')
                $ulItem.prepend($(data.markup));
            }
        },
        onErrorResponse: function (data, textStatus, jqXHR, $feedbackElement) {
            if (data.resultCode == "VE") {
                a4dn.core.mvc.am_Notification("extraSmallBox", "Information", data.message);
            }
            else {
                a4dn.core.mvc.am_ajax_defaultOnErrorResponse(data, textStatus, jqXHR, $feedbackElement);
            }
        },
    });
}

a4dn.core.mvc.am_AddFolderRecordShortcut = function (options) {
    var isFavorite = options.isFavorite;
    var parentFolderID = options.parentFolderID;
    var parentFolderName = options.parentFolderName;
    var applicationNumber = options.applicationNumber;
    var moduleNumber = options.moduleNumber;
    var uniqueKey = options.uniqueKey;
    var uniqueName = options.uniqueName;
    var moduleTitle = options.moduleTitle;

    if (typeof isFavorite === "undefined" || typeof parentFolderID === "undefined") {
        $('#a4dn-select-folder-modal .a4dn-folders-data').remove();

        setTimeout(function () { $('#a4dn-select-folder-modal .a4dn-folders-options input:checked').trigger("click"); }, 500);

        $('#a4dn-select-folder-modal').data('a4dn-options', options);
        //prompt User to get isFavorite and ParentFolderID
        $('#a4dn-select-folder-modal').modal('show');

        return;
    }

    var url = $('#a4dn-shortcuts-dropdown .a4dn-folders-options').data('a4dn-add-href')
    var $container = $('#a4dn-shortcuts-dropdown');

    a4dn.core.mvc.am_ajax(url, $container, {
        type: "POST",
        data: [
               { name: "isFavorite", value: isFavorite },
               { name: "ParentFolderID", value: parentFolderID },
               { name: "ParentFolderName", value: parentFolderName },
               { name: "ApplicationNumber", value: applicationNumber },
               { name: "ModuleNumber", value: moduleNumber },
               { name: "FileObjectName", value: uniqueName },
               { name: "FileObjectKey", value: uniqueKey },
               { name: "Title", value: moduleTitle }],
        onSuccess: function (data) {
            //console.log($container, data);

            if (data.message != "") {
                a4dn.core.mvc.am_Notification("extraSmallBox", "Success", data.message);
            }
            if ($container.data('a4dn-loaded') == true) {
                var $ulItem = $container.find('.a4dn-folders-content ul.a4dn-tree-list').filter('[data-a4dn-key="' + data.output.ParentFolderID + '"]')
                if ($ulItem.length && $ulItem.data('a4dn-loaded') == true) {
                    $ulItem.append(data.markup);
                }
            }
        },
        onErrorResponse: function (data, textStatus, jqXHR, $feedbackElement) {
            if (data.resultCode == "VE") {
                a4dn.core.mvc.am_Notification("extraSmallBox", "Information", data.message);
            }
            else {
                a4dn.core.mvc.am_ajax_defaultOnErrorResponse(data, textStatus, jqXHR, $feedbackElement);
            }
        },
    });
}

a4dn.core.mvc.am_InitializeDetailToolbar = function (guid, commandCallBackNamespace) {
    var toolbarCommandId = "a4dn-toolbar-commands-" + guid;
    var moduleId = _ModuleIdPrefix + guid;

    // wire up click event
    $("#" + toolbarCommandId + " .a4dn-command").click(function (e) {
        e.preventDefault();

        var $this = $(this);

        $('.tooltip').hide();

        var commandID = $this.data("a4dn-command-id");
        var guid = $this.closest(".a4dn-module-detail").data('a4dn-guid');

        a4dn.core.mvc.am_HandelModuleDetailCommand(guid, commandID, $this,
            {
                beforeProcessCommand: commandCallBackNamespace.beforeProcessCommand,
                afterProcessCommand: commandCallBackNamespace.afterProcessCommand,
            });
    });

    a4dn.core.mvc.am_OnToolbarResize(guid);

    $("#" + moduleId).resize(function () {
        waitForFinalEvent(function () {
            a4dn.core.mvc.am_OnToolbarResize(guid);
        }, 100, toolbarCommandId);
    });
}

a4dn.core.mvc.am_Get$ModuleExplorerDetailTab = function (moduleNumber, uniqueKey) {
    var $modExpDtlTab = $("#a4dn-main-module-explorer-tab-content .a4dn-module-detail").not('.a4dn-module-detail-preview').filter('[data-a4dn-mod-number="' + moduleNumber + '"]').filter('[data-a4dn-unique-key="' + uniqueKey + '"]').first();

    var tabpaneId = $modExpDtlTab.closest('.a4dn-module-explorer-tab-pane').attr('id');

    tabpaneId = "#" + tabpaneId;
    var $tab = $('#a4dn-main-module-explorer-tabs > li > a').filter('[href="' + tabpaneId + '"]').first();

    return $tab;
}

a4dn.core.mvc.am_IsModuleExplorerDetailTabOpen = function (moduleNumber, uniqueKey) {
    var $tab = a4dn.core.mvc.am_Get$ModuleExplorerTab(moduleNumber, uniqueKey);
    if ($tab.length) {
        return true;
    }
    return false;
}

a4dn.core.mvc.am_DetailAjaxPostRequest = function (guid, url, successCommandIDs) {
    var $a4dnForm = a4dn.core.mvc.am_Get$Form(guid);
    var $form = $a4dnForm.find(' > form');

    var $container = $('.a4dn-module-explorer-tab-pane.active');

    if ($form.length) {
        // Make sure all dirty flags are set on controls with original-value
        $form.find("[data-a4dn-original-value]").each(function () {
            let $control = $(this);
            a4dn.core.mvc.am_SetControlIsDirtyFlag($control, $form);
        });

        // Checkboxes and radio buttons that aren't checked will not be included in $form.serialize().
        // But we need to get their name=False values submitted when they're dirty, so the server
        // knows to update them. am_InputControlChanged alters their value attribute to match their
        // checked state, so we can fake-out jQuery here by setting their property to checked.
        // Both onSuccess and onError replace the form markup, so we don't have to worry about
        // resetting the properties to match the attributes.

        $form.find("[type=checkbox].a4dn-dirty[value='False']").each(function () {
            $(this).prop("checked", true);
        });

        let dirtyFields = [];
        $form.find(".a4dn-dirty").each(function () {
            let $control = $(this),
                originalVal = undefined;

            if ($control.is("[type=checkbox]")) {
                originalVal = /TRUE/i.test($control.data('a4dn-original-value')) ? "True" : "False";
            }
            else if ($control.is("[type=radio]")) {
                if ($control.is(":checked")) {
                    originalVal = $control.data('a4dn-original-value');
                }
                else {
                    // Only report the original value for the checked radio buttons. The unchecked
                    // ones are marked dirty too, but they map to the same field as the checked
                    // radio button so they don't matter.
                    return;
                }
            }
            else {
                originalVal = $control.data('a4dn-original-value');
            }

            dirtyFields.push({ name: $control.attr('name'), value: originalVal });
        });

        let $dirtyFields = $form.find('[name="pDirtyFields"]');

        if ($dirtyFields.length == 0) {
            $form.append("<input type='hidden' name='pDirtyFields' value=''/>");
            $dirtyFields = $form.find('[name="pDirtyFields"]');
        }

        $dirtyFields.val(JSON.stringify(dirtyFields));

        a4dn.core.mvc.am_ajax(url, $container, {
            data: $form.serialize(),
            type: $form.attr('method'),
            onSuccess: function (data) {
                // TODO: Distinguish between Success and informational messages

                try {
                    if (data.message != "") {
                        a4dn.core.mvc.am_Notification("extraSmallBox", "Success", data.message);
                    }

                    $container.html($(data.markup));

                    let $form = $container.find('form');

                    guid = ($form.is('.a4dn-form') ? $form : $form.closest(".a4dn-form"))
                        .data('a4dn-guid');

                    let $module = a4dn.core.mvc.am_Get$Module(guid),
                        commandCallBackNamespace = $module.data('a4dn-commandcallbacknamespace'),
                        commandId = $module.data('a4dn-commandid');

                    if ($.validator) {
                        $.validator.setDefaults({
                            // validate hidden fields
                            ignore: [],
                        });
                        $.validator.unobtrusive.parse($form);
                    }

                    if ($container.find('.a4dn-detail-form').data('a4dn-isvalid') == "False") {
                        // Validation Error - Return
                        return;
                    }

                    // Call afterPageLoad to try to reset business logic
                    if (typeof commandCallBackNamespace.afterPageLoad == 'function') {
                        commandCallBackNamespace.afterPageLoad(guid, commandId);
                    }

                    var $mod = $container.find('.a4dn-module-detail').first();
                    a4dn.core.mvc.am_DetailSaveProcessSuccessCommands($mod, successCommandIDs);
                }
                catch (error) {
                    a4dn.core.mvc.am_Notification("smallBox", "Error", error);
                }
            },
            beforeSend: function (jqXHR, ajaxSettings) {
                $("#" + _ModuleIdPrefix + guid + " .a4dn-detail-loading").first().removeClass("hidden");
            },
            complete: function (jqXHR, textStatus) {
                $("#" + _ModuleIdPrefix + guid + " .a4dn-detail-loading").first().addClass("hidden");
            },
        });
    }
};

a4dn.core.mvc.am_DetailSaveProcessSuccessCommands = function ($mod, successCommandIDs) {
    for (i = 0; i < successCommandIDs.length; i++) {
        commandID = successCommandIDs[i];
        let $activeTab = $('#a4dn-main-module-explorer-tabs li.active > a');

        switch (commandID) {
            case "SAVE AND CLOSE":
                $('#a4dn-main-module-explorer-tabs li.active > a .closeTab').trigger("click");
                break;

            case "SAVE AND NEW":
                a4dn.core.mvc.am_TriggerCommandClick($mod.data('a4dn-guid'), "NEW");
                $activeTab.find('.closeTab').trigger("click");  // FIXME: This breaks any custom code for Detail NEW, because the tab closes before the custom code can prompt and re-trigger the command
                break;

            case "SAVE AND COPY":
                a4dn.core.mvc.am_TriggerCommandClick($mod.data('a4dn-guid'), "COPY");
                $activeTab.find('.closeTab').trigger("click");  // FIXME: This breaks any custom code for Detail COPY, because the tab closes before the custom code can prompt and re-trigger the command
                break;

            case "REFRESHTABLE":

                a4dn.core.mvc.am_RefreshTableRowAfterSave($mod.data('a4dn-mod-number'), $mod.data('a4dn-unique-name'), $mod.data('a4dn-unique-key'));
                break;

            case "UPDATETABLE":

                a4dn.core.mvc.am_UpdateTableRowAfterSave($mod.data('a4dn-mod-number'), $mod.data('a4dn-unique-name'), $mod.data('a4dn-unique-key'));
                break;

            case "ADDHISTORY":
                a4dn.core.mvc.am_AddHistoryShortcut({ applicationNumber: $mod.data('a4dn-app-number'), moduleNumber: $mod.data('a4dn-mod-number'), uniqueKey: $mod.data('a4dn-unique-key'), uniqueName: $mod.data('a4dn-unique-name'), moduleTitle: $mod.data('a4dn-title') });

                break;
        }
    }
}

a4dn.core.mvc.am_DetailAjaxGetRequest = function (guid, url, options) {
    var $a4dnForm = $('#a4dn-form-' + guid);

    var $container = $('.a4dn-module-explorer-tab-pane.active');

    a4dn.core.mvc.am_ajax(url, $container, {
        onSuccess: function (data) {
            //console.log($container, data);

            $container.html($(data.markup));
            let $form = $container.find(".a4dn-form");
            a4dn.core.mvc.am_InitializeDropdownControls($form);
            a4dn.core.mvc.am_InitializeDateTimePickers($form);

            if (typeof options.onSuccess == 'function') {
                var $mod = $container.find('.a4dn-module-detail').first();
                options.onSuccess($mod);
            }
        },
        onErrorResponse: function (data, textStatus, jqXHR, $feedbackElement) {
            $("#" + _ModuleIdPrefix + guid + " .a4dn-detail-loading").first().addClass("hidden");
            a4dn.core.mvc.am_ajax_defaultOnErrorResponse(data, textStatus, jqXHR, $feedbackElement);
        },
        beforeSend: function (jqXHR, ajaxSettings) {
            $("#" + _ModuleIdPrefix + guid + " .a4dn-detail-loading").first().removeClass("hidden");
        },
        complete: function (jqXHR, textStatus) {
            $("#" + _ModuleIdPrefix + guid + " .a4dn-detail-loading").first().addClass("hidden");
        },
    });
};

a4dn.core.mvc.am_DetailAjaxDeleteRequest = function (guid, url, successCallback, errorCallback) {
    var $container = a4dn.core.mvc.am_Get$Module(guid);

    a4dn.core.mvc.am_ajax(url, $container, {
        type: "POST",
        onErrorResponse: function (data, textStatus, jqXHR, $feedbackElement) {
            if (errorCallback !== undefined) {
                errorCallback(data, textStatus, jqXHR, $feedbackElement);
            }
            else {
                a4dn.core.mvc.am_ajax_defaultOnErrorResponse(data, textStatus, jqXHR, $feedbackElement);
            }
        },
        onMessageOnlyResponse: function (data, textStatus, jqXHR) {
            //console.log($container, data);
            a4dn.core.mvc.am_RefreshTableAfterDelete($container.data('a4dn-mod-number'));

            var $tab = a4dn.core.mvc.am_Get$ModuleExplorerDetailTab($container.data('a4dn-mod-number'), data.output.UniqueKey);
            if ($tab.length) {
                a4dn.core.mvc.am_CloseModuleExplorerTab($tab.attr("href"));
            }

            if (successCallback !== undefined) {
                successCallback(data, textStatus, jqXHR);
            }
            else {
                if (data.message != "") {
                    a4dn.core.mvc.am_Notification("extraSmallBox", "Success", data.message);
                }
            }

        }
    });
}

a4dn.core.mvc.am_DetailAjaxPropertyChangedPostRequest = function (guid, propertyChangedName, isDirty) {
    let $a4dnForm = a4dn.core.mvc.am_Get$Form(guid),
        $form = $a4dnForm.find(' > form'),
        $propertyControl = $form.find("[name='" + propertyChangedName + "']");

    var url = $a4dnForm.data('a4dn-property-changed-href') + "&propertyName=" + encodeURIComponent(propertyChangedName) + "&isDirty=" + encodeURIComponent(isDirty);

    var $container = $('.a4dn-module-explorer-tab-pane.active');

    if ($form.length) {

        // Originally coded as an onSuccess handler, but since no markup is returned and message is sometimes returned,
        // am_ajax will send response to onMessageOnlyResponse if there is a message, and onSuccess if there is no message.

        let handleResponse = function (data) {
            if (data.message != "") {
                a4dn.core.mvc.am_Notification("extraSmallBox", "Success", data.message);
            }

            a4dn.core.mvc.am_UpdateFormPropertyModel($container, data.output.ap_ChangedPropertyModelDictionary);

            a4dn.core.mvc.am_PopulateForm($container, data.output.ap_ChangedData);

            a4dn.core.mvc.am_UpdateCommandStates(guid, data.output.ap_CommandStates);

            var $module = a4dn.core.mvc.am_Get$Module(guid);
            a4dn.core.mvc.am_SetCommandStateFromRecordMode(guid, $module.data("a4dn-record-mode"));
            a4dn.core.mvc.am_SetSaveCommandsBasedOnChange(guid);
        };

        a4dn.core.mvc.am_ajax(url, $propertyControl, {
            data: $form.serialize(),
            type: $form.attr('method'),
            onMessageOnlyResponse: handleResponse,
            onSuccess: handleResponse
        });
    }
};

a4dn.core.mvc.am_UpdateFormPropertyModel = function ($container, changedPropertyModelDict) {
    var $form = $container.find('form');

    // Reset a4dn_propertychanged error states, because we won't get flags if there is no property changed error.
    $form.find("[data-val-a4dn_propertychanged]").each(function () {
        var $prop = $(this);
        $prop.data("a4dn_propertychanged-isinerror", false);
        $prop.data("msg-a4dn_propertychanged", "");
    });

    for (var modelPropName in changedPropertyModelDict) {

        var properties = changedPropertyModelDict[modelPropName],
            $prop = $form.find("[name='" + modelPropName + "']");

        for (var i = 0; i < properties.length; i++) {
            var propName = "";
            var propValue = "";

            for (var propArray in properties[i]) {
                if (propArray == "ap_PropertyName") { propName = properties[i][propArray]; }
                if (propArray == "ap_PropertyValue") { propValue = properties[i][propArray]; }

                if (propName !== "" && propValue !== "") {

                    switch (propName) {
                        case "ap_IsReadOnly":
                            if (propValue == true) {
                                $prop.addClass("readonly");
                                $prop.addClass("a4dn-readonly");
                                $prop.prop('readonly', true);
                            }
                            else if (propValue == false) {
                                $prop.removeClass("readonly");
                                $prop.removeClass("a4dn-readonly");
                                $prop.prop('readonly', false);
                            }

                            break;
                        case "ap_IsEnabled":
                            if (propValue == true) {
                                $prop.removeClass("disabled");
                                $prop.removeClass("a4dn-disabled");
                                $prop.prop('disabled', false);
                                $prop.closest("label").removeClass("state-disabled");
                            }
                            else if (propValue == false) {
                                $prop.addClass("disabled");
                                $prop.addClass("a4dn-disabled");
                                $prop.prop('disabled', true);
                                $prop.closest("label").addClass("state-disabled");
                            }
                            break;
                        case "ap_IsVisible":
                            if (propValue == true) {
                                $prop.closest(".a4dn-field-control-group").removeClass("hidden");
                            }
                            else if (propValue == false) {
                                $prop.closest(".a4dn-field-control-group").addClass("hidden");
                            }
                            break;
                        case "ap_IsInError":
                            // Set a flag to indicate there is a property changed error; the a4dn_propertychanged validator rule will pick it up
                            $prop.data("a4dn_propertychanged-isinerror", propValue);
                            break;
                        case "ap_ErrorMessage":
                            // Set a custom error message for the property changed error; the a4dn_propertychanged validator rule will pick it up
                            $prop.data("msg-a4dn_propertychanged", propValue);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
};

a4dn.core.mvc.am_OnToolbarResize = function (guid) {
    let toolbarCommandId = "a4dn-toolbar-commands-" + guid,
        moduleExplorerId = _ModuleIdPrefix + guid,
        $toolbarCommands = $('#' + toolbarCommandId),
        $moduleExplorer = $('#' + moduleExplorerId);

    window.requestAnimationFrame(function () {
        var filterWidth = 0;
        if ($moduleExplorer.data("a4dn-type") == "search-explorer") {
            // Footer Toolbar
            var toolbarFooterCommandId = "a4dn-toolbar-footer-commands-" + guid;

            if (window.matchMedia('(max-width: 480px)').matches) {
                // Phone Size
                $moduleExplorer.find(".a4dn-toolbar-footer-tray.a4dn-phone-show").first().prepend($("#" + toolbarFooterCommandId));
            }
            else {
                $moduleExplorer.find(".a4dn-toolbar-footer-tray.a4dn-phone-hidden").first().prepend($("#" + toolbarFooterCommandId));
            }

            $toolbarCommands.find(".a4dn-toolbar-overflow-dropdown-menu").first().removeClass("a4dn-command-collapse");

            if (window.matchMedia('(max-width: 480px)').matches) {
                // Phone Size

                // Show overflow button
                $toolbarCommands.find(".a4dn-toolbar-overflow-dropdown-menu").first().addClass("a4dn-command-collapse");

                $toolbarCommands.find(".a4dn-toolbar-overflow-btn-group").first().removeClass("hidden");
                $toolbarCommands.find(".a4dn-commands-label").first().removeClass("hidden");

                $toolbarCommands.find(".a4dn-overflow-btn").each(function () {
                    var $closestli = $(this).closest('li');

                    if (!$closestli.hasClass('a4dn-state-hidden')) {
                        $closestli.removeClass("hidden");
                    }
                });

                $toolbarCommands.find(".a4dn-btn").each(function () {
                    $(this).addClass("hidden");
                });

                return;
            }
            else {
                filterWidth = $moduleExplorer.find(".a4dn-toolbar .a4dn-toolbar-tray .a4dn-toolbar-filter").first().outerWidth(true);

                // All others
                $toolbarCommands.find(".a4dn-commands-label").first().addClass("hidden");
            }
        }

        var availableWidth = $moduleExplorer.find(".a4dn-toolbar .a4dn-toolbar-tray").first().outerWidth(true) - filterWidth;
        var commandBarWidth = $toolbarCommands.outerWidth(true);

        if (commandBarWidth > availableWidth) {
            // Show overflow button
            $toolbarCommands.find(".a4dn-toolbar-overflow-btn-group").first().removeClass("hidden");

            for (var i = 0; i < 10; i++) {
                availableWidth = $moduleExplorer.find(".a4dn-toolbar .a4dn-toolbar-tray").first().outerWidth(true) - filterWidth;
                commandBarWidth = $toolbarCommands.outerWidth(true);

                if (commandBarWidth > availableWidth) {
                    // Hide in toolbar
                    var $btn = $toolbarCommands.find(".a4dn-btn:not(.hidden):not(.a4dn-state-hidden)").last();
                    $btn.addClass("hidden");

                    //show in overflow
                    var $overflowbtn = $toolbarCommands.find(".a4dn-overflow-btn[data-a4dn-command-id='" + $btn.data("a4dn-command-id") + "']").first();

                    $overflowbtn.closest('li').removeClass("hidden");
                }
            }
        }
        else {
            for (var i = 0; i < 10; i++) {
                availableWidth = $moduleExplorer.find(".a4dn-toolbar .a4dn-toolbar-tray").first().outerWidth(true) - filterWidth;
                commandBarWidth = $toolbarCommands.outerWidth(true);

                if (commandBarWidth < availableWidth) {
                    // Show in toolbar
                    var $btn = $toolbarCommands.find(".a4dn-btn.hidden:not(.a4dn-state-hidden)").first();
                    $btn.removeClass("hidden");

                    var newWidth = commandBarWidth + $btn.outerWidth(true);

                    if (newWidth > availableWidth) {
                        // Adding new command will not fit
                        $btn.addClass("hidden");
                        break;
                    }

                    //hide in overflow
                    var $overflowbtn = $toolbarCommands.find(".a4dn-overflow-btn[data-a4dn-command-id='" + $btn.data("a4dn-command-id") + "']").first();
                    $overflowbtn.closest('li').addClass("hidden");
                }
            }
        }

        if ($toolbarCommands.find(".a4dn-overflow-btn").closest('li:not(.hidden, .a4dn-state-hidden)').length > 0) {
            // Show overflow button
            $toolbarCommands.find(".a4dn-toolbar-overflow-btn-group").first().removeClass("hidden");
        }
        else {
            // Hide overflow button
            $toolbarCommands.find(".a4dn-toolbar-overflow-btn-group").first().addClass("hidden");
        }
    });
}

var waitForFinalEvent = (function () {
    var timers = {};
    return function (callback, ms, uniqueId) {
        if (!uniqueId) {
            uniqueId = "Don't call this twice without a uniqueId";
        }
        if (timers[uniqueId]) {
            clearTimeout(timers[uniqueId]);
        }
        timers[uniqueId] = setTimeout(callback, ms);
    };
})();

a4dn.core.mvc.am_GetBroadcastKeys = function (keyType, $table, $row) {
    var dataKeyType = "";
    switch (keyType) {
        case "OnSearch":
            dataKeyType = "a4dn-on-search-broadcast-keys";
            break;

        case "OnPreload":
            dataKeyType = "a4dn-on-preload-broadcast-keys";
            break;
        default:
    }
    if ($row != undefined && $row.length > 0) {
        var table = $table.DataTable();

        // Get Parent Keys
        var kyprop = $table.data(dataKeyType);
        var res = kyprop.split(",");
        var parentKeys = {};

        for (var i = 0; i < res.length; i++) {
            var val = table.cell($row, res[i].split(":")[0] + ":name").data();
            if (typeof val !== "undefined") {
                parentKeys[res[i].split(":")[1]] = val;
            }
        }

//        console.debug("am_GetBroadcastKeys(" + keyType + ")", $row);
//        console.dir(parentKeys);
        return JSON.stringify(parentKeys);
    }
};

a4dn.core.mvc.am_GetDetailBroadcastKeys = function (keyType, $form) {
    var dataKeyType = "";
    switch (keyType) {
        case "OnSearch":
            dataKeyType = "a4dn-on-search-broadcast-keys";
            break;

        case "OnPreload":
            dataKeyType = "a4dn-on-preload-broadcast-keys";
            break;
        default:
    }

    if ($form.length) {
        // Set Parent Keys
        var kyprop = $form.data(dataKeyType);
        var res = kyprop.split(",");
        var parentKeys = {};
        for (var i = 0; i < res.length; i++) {
            var val = $form.find("[name=" + res[i].split(":")[0] + "]").val();

            // This should never be needed; any element with a [name] is going to return a value from .val()
            //if (typeof val == "undefined") {
            //    val = $form.find("[name=" + res[i].split(":")[0] + "]").text();
            //}

            if (typeof val !== "undefined") {
                parentKeys[res[i].split(":")[1]] = val;
            }
        }
//        console.debug("am_GetDetailBroadcastKeys(" + keyType + ")", $form);
//        console.dir(parentKeys);

        return JSON.stringify(parentKeys);
    }
};

a4dn.core.mvc.am_SelectRow = function ($table, $tbody, $row, delay, flag) {
    var $moduleExplorer = $table.closest(".a4dn-module-explorer");
    var guid = $moduleExplorer.data('a4dn-guid');
    var table = $table.DataTable();

    var selectionState = "";
    if (table.data().length == 0) {
        selectionState = "Zero";
    }
    else {
        selectionState = "One";
    }

    // Call selection Changed on Server
    if ($table.data("a4dn-selectionchangedajaxtrigger") == "OnSelectionChanged") {
        a4dn.core.mvc.am_DataGridAjaxSelectionChangedPostRequest(guid, $table, $row, selectionState);
    }
    if ($table.data("a4dn-propertychangedajaxtrigger") == "OnSelectionChangedDelayedWait") {
        waitForFinalEvent(function () {
            a4dn.core.mvc.am_DataGridAjaxSelectionChangedPostRequest(guid, $table, $row, selectionState);
        }, 500, guid);
    }

    if (table.data().length == 0) {
        a4dn.core.mvc.am_ZeroSelectedCommandState(guid);
        return;
    }

    if ($row.length) {
        $tbody.children().removeClass("highlight");
        $row.addClass('highlight');
        a4dn.core.mvc.am_OneSelectedCommandState(guid);

        if (typeof delay === "undefined") {
            delay = 0; // no delay wait
        }

        waitForFinalEvent(function () {
            var table = $table.DataTable();

            if (!flag) {
                $table.trigger("itemSelected", [a4dn.core.mvc.am_GetBroadcastKeys("OnSearch", $table, $row), table.cell($row, "A4DN_SubTitle:name").data(), $row]);
                $table.trigger("itemSelectedCount", [a4dn.core.mvc.am_GetBroadcastKeys("OnSearch", $table, $row)]);
            }
        }, delay, $table.attr('id'));
    }
};

a4dn.core.mvc.am_SelectNextRow = function ($table, $tbody) {
    var $currentRow = $tbody.find('.highlight');

    var $container = $table.closest('.dataTables_scrollBody');
    $container.scrollTop($container.scrollTop() + $currentRow.height());

    var $next = $currentRow.next();

    var selectRowDelay = $table.data("a4dn-select-row-delay");
    if (typeof selectRowDelay === "undefined") {
        selectRowDelay = 1000;
    }
    a4dn.core.mvc.am_SelectRow($table, $tbody, $next, selectRowDelay);

    return $next;
}

a4dn.core.mvc.am_SelectPreviousRow = function ($table, $tbody) {
    var $currentRow = $tbody.find('.highlight');

    var $container = $table.closest('.dataTables_scrollBody');
    $container.scrollTop($container.scrollTop() - $currentRow.height());

    var $prev = $currentRow.prev();

    var selectRowDelay = $table.data("a4dn-select-row-delay");
    if (typeof selectRowDelay === "undefined") {
        selectRowDelay = 1000;
    }

    a4dn.core.mvc.am_SelectRow($table, $tbody, $prev, selectRowDelay);

    return $prev;
}

a4dn.core.mvc.am_SelectFirstRow = function (guid) {
    var $table = $('#a4dn-table-' + guid);
    var $tbody = $table.find('tbody').first();
    var $first = $tbody.find('tr').first();
    a4dn.core.mvc.am_SelectRow($table, $tbody, $first);

    return $first;
}

a4dn.core.mvc.am_SetTableFocus = function ($table) {
    if ($table.length) {
        $('.a4dn-focus').removeClass("a4dn-focus");
        $table.addClass("a4dn-focus")
    }
}

a4dn.core.mvc.am_RemoveAllTableFocus = function () {
    $('.a4dn-focus').removeClass("a4dn-focus");
}

a4dn.core.mvc.am_GetFocusRow = function ($table) {
    if ($table.length) {
        // Table
        var $tbody = $table.find("tbody");
        var $row = $tbody.find('.highlight');
    }

    return $row;
}

a4dn.core.mvc.am_Get$Table = function (guid) {
    return $("#a4dn-table-" + guid);
}

a4dn.core.mvc.am_Get$Form = function (guid) {
    return $("#a4dn-form-" + guid);
}

a4dn.core.mvc.am_Get$Module = function (guid) {
    return $("#a4dn-module-" + guid);
}

a4dn.core.mvc.am_Get$NewlookiFrame = function (guid) {
    return $("#a4dn-newlook-iframe-" + guid);
}

a4dn.core.mvc.am_UpdateRecordCountOnSubModuleTabAfterRefresh = function (guid, moduleNumber, recordcount) {
    var $mod = $('#a4dn-submoduletabs-' + guid + ' li a').filter('[data-a4dn-modulenumber="' + moduleNumber + '"]');

    if ($mod.length) {
        var $container = $mod.find('span.a4dn-record-count').html(recordcount);
    }
}

a4dn.core.mvc.am_SetRecordCountsOnSubModules = function (submoduleTabs, parentKeys, componentLocation) {
    // The browser will only allow ~6 of them at once, and will block any other requests until the queue
    // clears. This makes the GUI unresponsive. Loading them one at a time isn't be ideal, but it should
    // be better than trying more than 6 at a time.

    if (submoduleTabs.length === 0)
        return;

    if (submoduleTabs.length < 3) {
        submoduleTabs.forEach(function (tab) {
            a4dn.core.mvc.am_SetRecordCountOnSubModuleTab(tab.guid, tab.moduleNumber, parentKeys, componentLocation);
        });
    }
    else {
        let firstTab = submoduleTabs.shift();
        a4dn.core.mvc.am_SetRecordCountOnSubModuleTab(firstTab.guid, firstTab.moduleNumber, parentKeys, componentLocation, submoduleTabs);
    }
}

a4dn.core.mvc.am_SetRecordCountOnSubModuleTab = function (guid, moduleNumber, parentKeys, componentLocation, restOfTheTabs) {
    if ($('#a4dn-preview-widget-grid-' + guid).hasClass('hidden')) {
        // Preview is hidden
        return;
    }

    // Get Last Submodule. This covers the scenario of a Module that refers back to itself
    var $mod = $('#a4dn-submoduletabs-' + guid + ' li a').filter('[data-a4dn-modulenumber="' + moduleNumber + '"]').last();

    if ($mod.closest('li').hasClass('active')) {
        // Does get counts for the active tab as that is returned from the ajax call

        return;
    }

    if (typeof $mod.data('a4dn-count-href') === "undefined") {
        return;
    }

    var url = $mod.data('a4dn-count-href') + "&pPKeys=" + encodeURIComponent(parentKeys);

    var $container = $mod.find('span.a4dn-record-count');

    $container.html("?");

    if (parentKeys == "{}") {
        $container.html(0);

        return;
    }

    var $module = a4dn.core.mvc.am_Get$Module(guid);
    if (!$module.length) {
        $module = a4dn.core.mvc.am_Get$Form(guid);
    }
    // Do not get counts for sub modules if the data a4dn-ignore-item-selected-submodule-count is false
    if ($module.length && ($module.data("a4dn-ignore-item-selected-submodule-count") === "True" || $module.data("a4dn-ignore-item-selected-submodule-count") === true)) {
        if (restOfTheTabs && restOfTheTabs.length > 0) {
            let nextTab = restOfTheTabs.shift();
            setTimeout(function () {
                a4dn.core.mvc.am_SetRecordCountOnSubModuleTab(nextTab.guid, nextTab.moduleNumber, parentKeys, componentLocation, restOfTheTabs);
            }, 0);
        }
        return;
    }

    a4dn.core.mvc.am_ajax(url, $container, {
        onSuccess: function (data) {
            $container.html(data.output.RecordCount);

            // Update Drop Down List for phone
            var href = $container.closest('a').attr('href');
            $('.a4dn-submodule-dropdown').find('a[href$=' + href + ']').find('span.a4dn-record-count').html(data.output.RecordCount);

            // If restOfTheTabs is passed and contains tabs, schedule the loading of the next tab in the list and pass the remainder of the list
            if (restOfTheTabs && restOfTheTabs.length > 0) {
                let nextTab = restOfTheTabs.shift();
                setTimeout(function () {
                    a4dn.core.mvc.am_SetRecordCountOnSubModuleTab(nextTab.guid, nextTab.moduleNumber, parentKeys, componentLocation, restOfTheTabs);
                }, 0);
            }
        }
    });
}

a4dn.core.mvc.am_GenerateGuid = function () {
    // FIXME: This function should at least be made RFC4122 compliant, for version 4 random GUIDs.
    // It could also be a lot faster.
    // See https://stackoverflow.com/questions/105034/create-guid-uuid-in-javascript

    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
        s4() + '-' + s4() + s4() + s4();
}

a4dn.core.mvc.am_UpdateTableRowAfterSave = function (moduleNumber, uniqueName, uniqueKey) {
    // Find a4dn-module-explorer with Module Number
    var $moduleExplorer = $('.a4dn-module-explorer').filter('[data-a4dn-mod-number="' + moduleNumber + '"]').first();
    if ($moduleExplorer.length) {
        var guid = $moduleExplorer.data('a4dn-guid');
        var href = $('#a4dn-table-' + guid).data('a4dn-fetch-href');

        var url = href + "&pUNm=" + uniqueName + "&pUKy=" + uniqueKey + "&pRetMarkup=false" + "&pRetTblData=true";

        var $container = $moduleExplorer;

        a4dn.core.mvc.am_ajax(url, $container, {
            onSuccess: function (data) {
                // Find all Tables that have this record and replace

                $('.a4dn-module-explorer').filter('[data-a4dn-mod-number="' + moduleNumber + '"]').each(function () {
                    var guid = $(this).data('a4dn-guid');
                    var $table = $('#a4dn-table-' + guid);
                    var table = $table.DataTable();

                    // Find tr with unique Key
                    table.rows().every(function (rowIdx, tableLoop, rowLoop) {
                        if (table.cell($(this.node()), "A4DN_UniqueKey:name").data() == uniqueKey) {
                            // Remember Scroll Position
                            var $scrollBody = $('#a4dn-table-' + guid + '_wrapper .dataTables_scrollBody');
                            var scroll = $scrollBody.scrollTop();

                            a4dn.core.mvc.am_UpdateRow(guid, this, data.output[0]);

                            // Fire selected to update sub modules if currently selected
                            if ($(this.node()).hasClass("highlight")) {
                                var $row = $(this.node());
                                var $tbody = $table.find('tbody').first();
                                a4dn.core.mvc.am_SelectRow($table, $tbody, $row)
                            }

                            // Reset Scroll Position
                            $scrollBody.scrollTop(scroll);
                        }
                    });

                    a4dn.core.mvc.am_AdjustTableColumns(guid);
                });
            }
        });
    }
}

a4dn.core.mvc.am_RefreshTableRowAfterSave = function (moduleNumber, uniqueName, uniqueKey) {
    // Find all Tables that have this record and replace

    $('.a4dn-module-explorer').filter('[data-a4dn-mod-number="' + moduleNumber + '"]').each(function () {
        var guid = $(this).data('a4dn-guid');
        var $table = a4dn.core.mvc.am_Get$Table(guid);
        var table = $table.DataTable();

        var $row = a4dn.core.mvc.am_GetFocusRow($table);
        var currentUniqueKey = table.cell($row, "A4DN_UniqueKey:name").data();

        a4dn.core.mvc.am_ReloadDataTable({ guid: guid, selectedItemPriority1: uniqueKey, selectedItemPriority2: currentUniqueKey });
    });
}

a4dn.core.mvc.am_RefreshTableAfterDelete = function (moduleNumber) {
    // Find all Tables that have this record and replace

    $('.a4dn-module-explorer').filter('[data-a4dn-mod-number="' + moduleNumber + '"]').each(function () {
        var guid = $(this).data('a4dn-guid');
        var $table = a4dn.core.mvc.am_Get$Table(guid);
        var table = $table.DataTable();
        var $tbody = $table.find('tbody').first();
        a4dn.core.mvc.am_SelectNextRow($table, $tbody)

        var $row = a4dn.core.mvc.am_GetFocusRow($table);
        var currentUniqueKey = table.cell($row, "A4DN_UniqueKey:name").data();

        a4dn.core.mvc.am_ReloadDataTable({ guid: guid, selectedItemPriority1: currentUniqueKey });
    });
}

a4dn.core.mvc.am_TryReloadDataTable = function (guid) {
    var moduleId = _ModuleIdPrefix + guid;

    var $tabPane = $("#" + moduleId).closest('.a4dn-tab-pane');
    var moduleNumber = $tabPane.data('a4dn-modulenumber');
    var tab = $tabPane.data('a4dn-tab');

    var $subModuleTabAnchor = $('#' + tab).find('a').filter('[data-a4dn-modulenumber="' + moduleNumber + '"]');

    if ($tabPane.hasClass('active')) {
        a4dn.core.mvc.am_ReloadDataTable({ guid: guid });

        $subModuleTabAnchor.data("a4dn-dataloaded", "true");
    }
    else {
        $subModuleTabAnchor.data("a4dn-dataloaded", "false");
    }
};

a4dn.core.mvc.am_Deserialize = function (query, $container) {
    // remove leading question mark if its there
    if (query.slice(0, 1) === '?') {
        query = query.slice(1);
    }

    if (query !== '') {
        let pairs = query.split('&');
        for (var i = 0; i < pairs.length; i += 1) {
            // Have to replace spaces encoded as + with %20 encoding, because decodeURIComponent only handles the latter
            let keyValuePair = pairs[i].replace(/\+/g, "%20").split('=');

            let key = decodeURIComponent(keyValuePair[0]);
            let value = (keyValuePair.length > 1) ? decodeURIComponent(keyValuePair[1]) : undefined;

            let $input = $container.find("[name='" + key + "']"); // <input>, <select>, <textarea>

            a4dn.core.mvc.am_SetControlsValue($input, value, false);
        }
    }
};

a4dn.core.mvc.am_QueryHasSetValues = function (query) {
    var pairs, i, keyValuePair, key, value;
    // remove leading question mark if its there
    if (query.slice(0, 1) === '?') {
        query = query.slice(1);
    }
    if (query !== '') {
        pairs = query.split('&');
        for (i = 0; i < pairs.length; i += 1) {
            keyValuePair = pairs[i].split('=');
            key = decodeURIComponent(keyValuePair[0]);
            value = (keyValuePair.length > 1) ? decodeURIComponent(keyValuePair[1]) : undefined;

            if (value.length > 0) {
                //value found
                return true;
            }
        }
    }
    return false;
}

a4dn.core.mvc.am_SwitchView = function (guid, viewName, href, commandCallbackNamespace) {
    var moduleId = _ModuleIdPrefix + guid;
    var $container = $('#' + moduleId).find('.a4dn-datatable-container').first();

    var $module = a4dn.core.mvc.am_Get$Module(guid);
    var pVwRefExt = $module.data('a4dn-view-ref-ext');
    var pRole = $module.data('a4dn-component-role');

    var url = href + "&pVwNm=" + viewName + "&pVwRefExt=" + pVwRefExt + "&pGuid=" + guid + "&pTblDfrLoad=false" + "&pRole=" + pRole;

    a4dn.core.mvc.am_ajax(url, $container, {
        onSuccess: function (data) {
            //console.log($container, data);

            a4dn.core.mvc.am_DestroyDataTable(guid);
            $container.empty();

            $container.append($(data.markup));

            //a4dn.core.mvc.am_InitializeModuleExplorerToolbar(guid, commandCallbackNamespace);
        }
    });
};

a4dn.core.mvc.am_FetchDetailData = function ($module, options) {
    var url = $module.data("a4dn-fetch-href");

    a4dn.core.mvc.am_ajax(url, null, {
        onSuccess: function (data) {
            var jsonObj = data.output === undefined ? {} : JSON.parse(data.output);

            if (typeof options.onSuccess == 'function') {
                options.onSuccess(jsonObj);
            }
        }
    });
};

a4dn.core.mvc.am_FetchDetailDataForPreview = function (tableGuid, detailGuid, href) {
    var $table = a4dn.core.mvc.am_Get$Table(tableGuid);
    var $row = a4dn.core.mvc.am_GetFocusRow($table);
    if ($row.length == 0) {
        return;
    }
    var table = $table.DataTable();
    var url = href + "&pUNm=" + table.cell($row, "A4DN_UniqueName:name").data() + "&pUKy=" + table.cell($row, "A4DN_UniqueKey:name").data(),
        overrideUrl = undefined;

    var $container = $('#a4dn-module-' + detailGuid);

    let commandCallBackNamespace = $container.data('a4dn-commandcallbacknamespace');

    if (commandCallBackNamespace && typeof commandCallBackNamespace.beforePreview === 'function') {
        let ret = commandCallBackNamespace.beforePreview(tableGuid, table, $row, url, $container);

        if (typeof ret === 'object') {
            overrideUrl = ret.overrideUrl;
            if (ret.handled) { return; }
        }
        else if (typeof ret === 'boolean' && ret) { return; }
    }

    if (overrideUrl !== undefined) {
        url = overrideUrl;
    }

    // start Loading
    $('#a4dn-module-' + detailGuid + ' .a4dn-loading').first().removeClass("hidden");

    a4dn.core.mvc.am_ajax(url, $container, {
        onSuccess: function (data) {
            var jsonObj = data.output === undefined ? {} : JSON.parse(data.output);

            let uniqueKey = jsonObj["ap_UniqueKey"];    // Comes from json serialization of a DataEntity, so will never be encoded here
            if (uniqueKey !== undefined) {
                uniqueKey = a4dn_Base64.am_ToBase64String(uniqueKey);
            }

            a4dn.core.mvc.am_PopulateForm($container, jsonObj);

            // Set Title, Unique name and Key
            $container.data("a4dn-unique-name", jsonObj["ap_UniqueName"]);
            $container.data("a4dn-unique-key", uniqueKey);
            $container.data("a4dn-title", jsonObj["ap_Title"]);

            if (commandCallBackNamespace && typeof commandCallBackNamespace.afterPageLoad === 'function') {
                commandCallBackNamespace.afterPageLoad(detailGuid, $container.data('a4dn-commandid'));
            }
            if (!commandCallBackNamespace) {
                console.warn("No a4dn-commandcallbacknamespace in ", $container);
            }

            // end Loading
            $('#a4dn-module-' + detailGuid + ' .a4dn-loading').first().addClass("hidden");
        }
    });
};

a4dn.core.mvc.am_PopulateForm = function ($form, data) {
    //console.log("PopulateForm, All form data: " + JSON.stringify(data));

    $.each(data, function (key, value)   // all json fields ordered by name
    {
        //console.log("Data Element: " + key + " value: " + value );

        $form.find('[ data-a4dn-bind-prop=' + key + ']').each(function () {
            var target = $(this).data('a4dn-bind-target');

            if (target.startsWith('data-')) {
                // Update both data() and data- attribute, so both JS and CSS can use the new value
                $(this).data(target.replace("data-", ""), value);
                $(this).attr(target, value);
            }
            else {
                $(this).attr(target, value);
            }
        });

        var $ctrls = $form.find("[name='" + key + "']");  //all form elements for a name. Multiple checkboxes can have the same name, but different values

        // Append all form elements with a data-a4dn-received-tag-names attribute that contains key
        let $receiveCtrls = $form.find("[data-a4dn-received-tag-names]").filter(function (index) {
            let $this = $(this),
                tagNames = $(this).data('a4dn-received-tag-names');
            return tagNames && tagNames !== "" && tagNames.indexOf(key) >= 0;
        });

        if ($receiveCtrls.length) {
            $ctrls = $ctrls.add($receiveCtrls);
        }

        if ($ctrls.length)
            a4dn.core.mvc.am_SetControlsValue($ctrls, value, true);
    });
};

a4dn.core.mvc.am_SetControlsValue = function ($ctrls, value, doTrigger) {
    if ($ctrls.is('select')) //special form types
    {
        $ctrls.val(value);

        $ctrls.find("option").each(function () {
            let $opt = $(this),
                val = $opt.attr("value"),
                isSelected = value === null || /^\s*$/.test(value)
                    ? /^\s*$/.test(val)
                    : /^(TRUE|FALSE)$/i.test(value)     // Case-insensitive match for tri-state 'checkboxes'
                        ? val.toUpperCase() === value.toUpperCase()
                        : val === value;

            $opt.prop("selected", isSelected);

            if ($ctrls.is(".a4dn-readonly"))
                $opt.prop("disabled", !isSelected);
        });
    }
    else if ($ctrls.is('textarea')) {
        $ctrls.val(value);
        if (doTrigger)
            $ctrls.trigger("valueUpdated");
    }
    else if (typeof $ctrls.attr("type") === "undefined") {
        $ctrls.html(value); // not input field
    }
    else {
        switch ($ctrls.attr("type"))   //input type
        {
            case "text":
            case "date":
            case "time":
            case "email":
            case "tel":
            case "number":
            case "hidden":
                $ctrls.val(value);
                if (doTrigger)
                    $ctrls.trigger("valueUpdated");
                break;

            case "radio":
                if ($ctrls.length >= 1) {
                    //console.log("$ctrls.length: " + $ctrls.length + " value.length: " + value.length);
                    $.each($ctrls, function (index) {  // every individual element
                        var elemValue = $(this).attr("value");
                        var elemValueInData = singleVal = value;
                        if (elemValue === value) {
                            $(this).prop('checked', true);
                        }
                        else {
                            $(this).prop('checked', false);
                        }
                    });
                }
                break;

            case "checkbox":
                if ($ctrls.length > 1) {
                    //console.log("$ctrls.length: " + $ctrls.length + " value.length: " + value.length);
                    // TODO: this loop is working with strings instead of bools; probably won't work right
                    $.each($ctrls, function (index) // every individual element
                    {
                        var elemValue = $(this).attr("value");
                        var elemValueInData = undefined;
                        var singleVal;
                        for (var i = 0; i < value.length; i++) {
                            singleVal = value[i];
                            //console.log("singleVal : " + singleVal + " value[i][1]" + value[i][1]);
                            if (singleVal === elemValue) { elemValueInData = singleVal; }
                        }

                        if (elemValueInData) {
                            //console.log("TRUE elemValue: " + elemValue + " value: " + value);
                            $(this).prop('checked', true);
                            //$(this).prop('value', true);
                        }
                        else {
                            //console.log("FALSE elemValue: " + elemValue + " value: " + value);
                            $(this).prop('checked', false);
                            //$(this).prop('value', false);
                        }
                    });
                }
                else if ($ctrls.length === 1) {
                    $ctrl = $ctrls;
                    var boolValue = /TRUE/i.test(value);
                    // console.log("Single checkbox:", $ctrl, "String value:", value, "Bool value: ", boolValue);
                    if (boolValue) { $ctrl.prop('checked', true); }
                    else { $ctrl.prop('checked', false); }
                }
                break;
        }
    }
};

a4dn.core.mvc.am_Notification = function (notificationStyle, notificationType, message, options) {
    var title;
    var messageButtons;
    var color;
    var icon;
    var timeout;
    var number;

    if (typeof options !== "undefined") {
        title = options.title;
        messageButtons = options.messageButtons;
        color = options.color;
        icon = options.icon;
        timeout = options.timeout;
        number = options.number;
    }

    if (typeof title === "undefined") {
        switch (notificationType) {
            case "ValidationError":
                title = $('#a4dn-main-content').data('a4dn-lang-notification-validation-error') //"Validation Error - Record Not Saved."
                break;
            case "Error":
                title = $('#a4dn-main-content').data('a4dn-lang-notification-error') //"Oops, an ERROR was reported!"
                break;
            case "Success":
                title = $('#a4dn-main-content').data('a4dn-lang-notification-success') //"Success"
                break;
            case "Information":
                title = $('#a4dn-main-content').data('a4dn-lang-notification-info') //"Information"
                break;
            case "Warning":
                title = $('#a4dn-main-content').data('a4dn-lang-notification-warning') //"Warning"
                break;
        }
    }

    if (typeof messageButtons === "undefined") {
        switch (notificationType) {
            case "ValidationError":
            case "Error":
                messageButtons = "<p class='text-align-right'><a href='javascript:void(0);' class='btn btn-danger btn-sm'>" + $('#a4dn-main-content').data('a4dn-lang-notification-dismiss') + "</a></p>"
                break;
            case "Success":

                break;
            case "Information":

                break;
            case "Warning":

                break;
        }
    }

    if (typeof color === "undefined") {
        switch (notificationType) {
            case "ValidationError":
            case "Error":
                color = "#C46A69";
                break;
            case "Success":
                color = "#659265";
                break;
            case "Information":
                color = "#3276b1";
                break;
            case "Warning":
                color = "#C46A69";
                break;
        }
    }

    if (typeof icon === "undefined") {
        switch (notificationType) {
            case "ValidationError":
            case "Error":
                icon = "fa fa-frown-o shake animated";
                break;
            case "Success":
                icon = "fa fa-check fadeInRight animated";
                break;
            case "Information":
                icon = "fa fa-comment fadeInRight animated";
                break;
            case "Warning":
                icon = "fa fa-warning shake animated";
                break;
        }
    }

    if (typeof timeout === "undefined") {
        switch (notificationType) {
            case "ValidationError":
            case "Error":
                break;
            case "Success":
                timeout = 4000;
                break;
            case "Information":
                timeout = 4000;
                break;
            case "Warning":
                timeout = 4000;
                break;
        }
    }

    var content = message;
    if (typeof messageButtons !== "undefined") {
        content = message + messageButtons;
    }
    content = content.replace(/(\n|; )/g, "<br/>");

    switch (notificationStyle) {
        case "smallBox":

            $.smallBox({
                title: title,
                content: content,
                color: color,
                icon: icon,
                timeout: timeout,
                number: number,
            });
            break;
        case "extraSmallBox":
            $.smallBox({
                title: title,
                content: content,
                color: color,
                iconSmall: icon,
                timeout: timeout,
                number: number,
            });
            break;
        case "bigbox":
            $.bigBox({
                title: title,
                content: content,
                color: color,
                icon: icon,
                timeout: timeout,
                number: number,
            });
            break;
    }
};

a4dn.core.mvc.am_SmartMessageBox_SaveChanges = function (guid, tabContentId) {
    var cancelBtn = $('#a4dn-main-content').data('a4dn-lang-cancel'); //"Cancel"
    var noBtn = $('#a4dn-main-content').data('a4dn-lang-no'); //"No"
    var yesBtn = $('#a4dn-main-content').data('a4dn-lang-yes'); //"Yes"

    $.SmartMessageBox({
        title: $('#a4dn-main-content').data('a4dn-lang-notification-save-title'), //'Save Changes?'
        content: $('#a4dn-main-content').data('a4dn-lang-notification-save-content'), //'Do you want to save your changes before you close the tab?'
        buttons: '[' + cancelBtn + '][' + noBtn + '][' + yesBtn + ']'
    }, function (ButtonPressed) {
        if (ButtonPressed === yesBtn) {
            // Save and Close Tab
            a4dn.core.mvc.am_TriggerCommandClick(guid, "SAVE AND CLOSE");
        }
        if (ButtonPressed === noBtn) {
            // Close Tab without saving
            a4dn.core.mvc.am_CloseModuleExplorerTab(tabContentId);
        }
    });
};

a4dn.core.mvc.am_SmartMessageBox_ConfirmDelete = function (guid, url, recordTitle, ifYesCallback, htitle) {
    if (!htitle) {
        htitle = $('#a4dn-main-content').data('a4dn-lang-notification-delete-content');
        htitle = htitle.replace("{0}", recordTitle);
    }

    var noBtn = $('#a4dn-main-content').data('a4dn-lang-no'); //"No"
    var yesBtn = $('#a4dn-main-content').data('a4dn-lang-yes'); //"Yes"

    $.SmartMessageBox({
        title: $('#a4dn-main-content').data('a4dn-lang-notification-delete-title'), //'Confirm Delete?'
        content: htitle, //'Are you sure you want to delete {0}?'
        buttons: '[' + noBtn + '][' + yesBtn + ']'
    }, function (ButtonPressed) {
        if (ButtonPressed === yesBtn) {
            ifYesCallback(guid, url);
        }
    });
};

a4dn.core.mvc.am_NewCommandState = function (guid) {
    $('#a4dn-toolbar-commands-' + guid).find('.a4dn-command-state').each(function () {
        a4dn.core.mvc.am_SetCommandState($(this), $(this).data('a4dn-new-state'))
    });
};

a4dn.core.mvc.am_OpenCommandState = function (guid) {
    $('#a4dn-toolbar-commands-' + guid).find('.a4dn-command-state').each(function () {
        a4dn.core.mvc.am_SetCommandState($(this), $(this).data('a4dn-open-state'))
    });
};

a4dn.core.mvc.am_DisplayCommandState = function (guid) {
    $('#a4dn-toolbar-commands-' + guid).find('.a4dn-command-state').each(function () {
        a4dn.core.mvc.am_SetCommandState($(this), $(this).data('a4dn-display-state'))
    });
};

a4dn.core.mvc.am_ZeroSelectedCommandState = function (guid) {
    $('#a4dn-toolbar-commands-' + guid).find('.a4dn-command-state').each(function () {
        a4dn.core.mvc.am_SetCommandState($(this), $(this).data('a4dn-zero-state'))
    });
};

a4dn.core.mvc.am_OneSelectedCommandState = function (guid) {
    $('#a4dn-toolbar-commands-' + guid).find('.a4dn-command-state').each(function () {
        a4dn.core.mvc.am_SetCommandState($(this), $(this).data('a4dn-one-state'))
    });
};

a4dn.core.mvc.am_MultipleSelectedCommandState = function (guid) {
    $('#a4dn-toolbar-commands-' + guid).find('.a4dn-command-state').each(function () {
        a4dn.core.mvc.am_SetCommandState($(this), $(this).data('a4dn-multiple-state'))
    });
};

a4dn.core.mvc.am_SetCommandState = function ($command, commandState) {
    switch (commandState) {
        case "E":
        case "V":
            // Enabled and Visible
            if ($command.hasClass('a4dn-state-hidden')) {
                $command.removeClass("hidden");
                $command.removeClass("a4dn-state-hidden");
            }

            $command.removeClass("disabled");

            if ($command.is('button')) {
                $command.prop('disabled', false);
            }
            else {
                $command.parent().removeClass("disabled");
            }

            break;
        case "D":
            // Disabled and Visible
            if ($command.hasClass('a4dn-state-hidden')) {
                $command.removeClass("hidden");
                $command.removeClass("a4dn-state-hidden");
            }

            $command.addClass("disabled");

            if ($command.is('button')) {
                $command.prop('disabled', true);
            }
            else {
                $command.parent().addClass("disabled");
            }

            break;
        case "H":
            // Hidden and Disabled
            $command.addClass("hidden");
            $command.addClass("a4dn-state-hidden");

            $command.addClass("disabled");

            if ($command.is('button')) {
                $command.prop('disabled', true);
            }
            else {
                $command.parent().addClass("disabled");
            }

            break;
    }
};

a4dn.core.mvc.am_UpdateCommandStates = function (guid, commandStates) {
    for (var i = 0; i < commandStates.length; i++) {
        var $toolbarCommandId = $("#a4dn-toolbar-commands-" + guid);
        $toolbarCommandId.find(".a4dn-command" + "[data-a4dn-command-id='" + commandStates[i].ap_CommandID + "']").each(function () {
            if (commandStates[i].ap_IsVisibile == true) {
                $(this).data("a4dn-new-state", "V");
                $(this).data("a4dn-open-state", "V");
                $(this).data("a4dn-display-state", "V");
                $(this).data("a4dn-zero-state", "V");
                $(this).data("a4dn-one-state", "V");
                $(this).data("a4dn-multiple-state", "V");
            }

            if (commandStates[i].ap_IsEnabled == false) {
                $(this).data("a4dn-new-state", "D");
                $(this).data("a4dn-open-state", "D");
                $(this).data("a4dn-display-state", "D");
                $(this).data("a4dn-zero-state", "D");
                $(this).data("a4dn-one-state", "D");
                $(this).data("a4dn-multiple-state", "D");
            }

            if (commandStates[i].ap_IsVisibile == false) {
                $(this).data("a4dn-new-state", "H");
                $(this).data("a4dn-open-state", "H");
                $(this).data("a4dn-display-state", "H");
                $(this).data("a4dn-zero-state", "H");
                $(this).data("a4dn-one-state", "H");
                $(this).data("a4dn-multiple-state", "H");
            }

            if (commandStates[i].ap_IsChecked == true) {
            }
            else if (commandStates[i].ap_IsChecked == false) {
            }
        });
    }
};

a4dn.core.mvc.am_SetCommandStateFromRecordMode = function (guid, recordMode) {
    switch (recordMode) {
        case "Open":
            a4dn.core.mvc.am_OpenCommandState(guid);
            break;
        case "New":
            a4dn.core.mvc.am_NewCommandState(guid);
            break;
        case "Display":
            a4dn.core.mvc.am_DisplayCommandState(guid);
            break;
    }
};

a4dn.core.mvc.am_InputControlChanged = function (guid, $control, triggerEvent, triggerArgs) {
    if (guid !== undefined) {
        let $form = a4dn.core.mvc.am_Get$Form(guid);

        a4dn.core.mvc.am_SetControlIsDirtyFlag($control, $form);

        a4dn.core.mvc.am_SetSaveCommandsBasedOnChange(guid);

        // Call Property Changed on Server
        if ($form.data("a4dn-propertychangedajaxtrigger") == "OnInputControlChanged") {
            a4dn.core.mvc.am_DetailAjaxPropertyChangedPostRequest(guid, $control.attr("name"), $control.is(".a4dn-dirty"));
        }
        if ($form.data("a4dn-propertychangedajaxtrigger") == "OnInputControlChangedDelayedWait") {
            waitForFinalEvent(function () {
                a4dn.core.mvc.am_DetailAjaxPropertyChangedPostRequest(guid, $control.attr("name"), $control.is(".a4dn-dirty"));
            }, 500, guid);
        }
    }

    if (triggerEvent) {
        // Trigger a specific event; used for dropdown controls
        if (triggerArgs) {
            $control.trigger(triggerEvent, triggerArgs);
        }
        else {
            $control.trigger(triggerEvent);
        }
    }
    else {
        // Trigger general PropChanged method on AB_FormModel
        $control.trigger("propertychanged.ab_formmodel");
    }
};

a4dn.core.mvc.am_SetControlIsDirtyFlag = function ($control, $form) {
    let val, originalVal;

    if ($control.is("[type=checkbox]")) {
        val = $control.prop("checked") === true;
        originalVal = /TRUE/i.test($control.data('a4dn-original-value'));
        // Update value attribute to match property, so that $form.serialize works even when the checkbox is cleared
        $control.attr("value", val ? "True" : "False");
    }
    else if ($control.is("[type=radio]")) {
        let $checked = $control.is(":checked") ? $control : $control.closest("form").find("[name='" + $control.attr("name") + "']:checked");
        val = $checked.val();
        originalVal = $checked.data('a4dn-original-value');
    }
    else if ($control.is(".datetimepicker")) {
        val = moment($control.val());
        originalVal = moment($control.data('a4dn-original-value'));
    }
    else if ($control.is("select")) {
        val = $control.val();
        if (val === "[null]" || val === "[blank]" || val === null || val === undefined) {
            val = "";
        }
        originalVal = $control.data('a4dn-original-value');
    }
    else {
        val = $control.val();
        originalVal = $control.data('a4dn-original-value');
    }

    //console.debug("[" + $control.length + "] am_InputControlChanged: " + $control.attr("name") + ", val: " + val + ", originalVal: " + originalVal);

    if ($control.hasClass("a4dn-field-control")) {
        // $form.find() is used to handle controls like radio buttons, where multiple
        // elements have the same name attribute
        // If val has an isSame method, it's a moment object and we can use isSame to compare to originalVal.
        // Otherwise, compare as strings.
        if ((val !== undefined && val !== null && typeof val.isSame === "function" && val.isSame(originalVal)) || val == originalVal) {
            $form.find("[name='" + $control.attr("name") + "']").removeClass('a4dn-dirty');
        }
        else {
            $form.find("[name='" + $control.attr("name") + "']").addClass('a4dn-dirty');
        }
    }
    else {
        // not Accelerator Field Control
        $control.addClass('a4dn-dirty');
    }
};

a4dn.core.mvc.am_InputControlLostFocus = function (guid, $control) {
    var $form = a4dn.core.mvc.am_Get$Form(guid);
    // Call Property Changed on Server
    if ($form.data("a4dn-propertychangedajaxtrigger") == "OnInputControlLostFocus") {
        a4dn.core.mvc.am_DetailAjaxPropertyChangedPostRequest(guid, $control.attr("name"), $control.is(".a4dn-dirty"));
    }
}

a4dn.core.mvc.am_SetSaveCommandsBasedOnChange = function (guid) {
    var $form = a4dn.core.mvc.am_Get$Form(guid);

    if ($form.hasClass("a4dn-dirty") || $form.find(".a4dn-dirty").length) {
        // Form is dirty
        a4dn.core.mvc.am_SetSaveCommandsEnabled(guid);
    }
    else {
        a4dn.core.mvc.am_SetSaveCommandsDisabled(guid);
    }
}

a4dn.core.mvc.am_SetSaveCommandsDisabled = function (guid) {
    $('#a4dn-toolbar-commands-' + guid).find('.a4dn-command').each(function () {
        var commandID = $(this).data('a4dn-command-id');

        if (commandID.includes("SAVE")) {
            if ($(this).hasClass('a4dn-overflow-btn')) {
                // Overflow Btn - Set classes on closest li
                let $command = $(this).closest('li.a4dn-command-state');
                $command.addClass("disabled").prop("disabled", true);
            }
            else {
                $(this).addClass("disabled").prop("disabled", true);
            }
        }
    });
};

a4dn.core.mvc.am_SetSaveCommandsEnabled = function (guid) {
    $('#a4dn-toolbar-commands-' + guid).find('.a4dn-command').each(function () {
        var commandID = $(this).data('a4dn-command-id');

        if (commandID.includes("SAVE")) {
            if ($(this).hasClass('a4dn-overflow-btn')) {
                // Overflow Btn - Set classes on closest li
                let $command = $(this).closest('li.a4dn-command-state');
                $command.removeClass("disabled").prop("disabled", false);
            }
            else {
                $(this).removeClass("disabled").prop("disabled", false);
            }
        }
    });
};

a4dn.core.mvc.am_HandelModuleDetailCommand = function (guid, commandID, $command, options) {
    var opts = $.extend({
        beforeProcessCommand: function (guid, commandID, $command) {
            return false;
        },
        afterProcessCommand: function (guid, commandID, $command) {
        },
    }, options || {});

    let $module = a4dn.core.mvc.am_Get$Module(guid),
        $form = a4dn.core.mvc.am_Get$Form(guid),
        url, uniqueName, uniqueKey, title,
        overrideUrl = undefined,
        successCallback = undefined,
        errorCallback = undefined;

    if (typeof opts.beforeProcessCommand == 'function') {

        var ret = opts.beforeProcessCommand(guid, commandID, $command);
        if (typeof ret == 'object') {
            commandID = ret.commandID === undefined ? commandID : ret.commandID;
            overrideUrl = ret.overrideUrl;
            successCallback = ret.successCallback;
            errorCallback = ret.errorCallback;
            // TODO: make use of successCallback and errorCallback in more than just the DELETE commands

            if (ret.handled) { return; }
        }
        if (typeof ret == 'boolean' && ret) { return; }
    }

    if (overrideUrl !== undefined) {
        url = overrideUrl;
    }
    else if (commandID === "DELETE" || commandID === "PERMDELETE") {
        url = $form.data('a4dn-delete-href');
    }
    else if (commandID === "SAVE AND CLOSE" || commandID === "SAVE" || commandID === "SAVE AND NEW" || commandID === "SAVE AND COPY")
    {
        if ($form.data("a4dn-record-mode") === "New") {
            url = $form.data('a4dn-insert-href');
        }
        else {
            url = $form.data('a4dn-update-href');
        }
    }
    else {
        url = $form.data('a4dn-dtl-view-href');
    }


    switch (commandID) {
        case "CLOSE_EDIT":
            $('#a4dn-main-module-explorer-tabs li.active > a .closeTab').trigger("click");
            break;

        case "NEWFOR":
        case "NEW":

            commandID = "NEWFOR";
            url = url + "&pCmdID=" + commandID + a4dn.core.mvc.am_GetNewForPropagationParameters($module) + "&pNewlookSrc=" + $form.data('a4dn-newlook-src');

            // Create Module Explorer Detail Tab
            a4dn.core.mvc.am_CreateModuleExplorerDetailTab({ url: url, moduleTitle: a4dn.core.mvc.am_GetModuleName($module) + " Detail", moduleImageSource: a4dn.core.mvc.am_GetModuleImageSource($module) });

            break;

        case "COPYFOR":
        case "COPY":

            commandID = "COPYFOR";
            url = url + "&pUNm=" + $module.data('a4dn-unique-name') + "&pUKy=" + $module.data('a4dn-unique-key') + "&pCmdID=" + commandID + a4dn.core.mvc.am_GetNewForPropagationParameters($module) + "&pNewlookSrc=" + $form.data('a4dn-newlook-src');

            // Create Module Explorer Detail Tab
            a4dn.core.mvc.am_CreateModuleExplorerDetailTab({ url: url, moduleTitle: a4dn.core.mvc.am_GetModuleName($module) + " Detail", moduleImageSource: a4dn.core.mvc.am_GetModuleImageSource($module) });

            break;

        case "SAVE AND CLOSE":
        case "SAVE":
        case "SAVE AND NEW":
        case "SAVE AND COPY":

            //Remove Drop Down Menus - This will cause errors on post
            $form.find('.a4dn-dropdown-menu').each(function () {
                var $this = $(this);
                $this.empty();
                $this.data('a4dn-view-loaded', false);
            });

            if ($form.find('form').valid()) {
                if ($form.data("a4dn-record-mode") == "New") {
                    url = url + "&pCmdID=" + commandID + a4dn.core.mvc.am_GetNewForPropagationParameters($module) + "&pNewlookSrc=" + $form.data('a4dn-newlook-src');
                    a4dn.core.mvc.am_DetailAjaxPostRequest(guid, url, [commandID, "REFRESHTABLE", "ADDHISTORY"]);
                }
                else {
                    url = url + "&pCmdID=" + commandID + a4dn.core.mvc.am_GetNewForPropagationParameters($module) + "&pNewlookSrc=" + $form.data('a4dn-newlook-src');

                    if ($form.data("a4dn-refresh-table-on-update") == "True" || $form.data("a4dn-refresh-table-on-update") == true) {

                        a4dn.core.mvc.am_DetailAjaxPostRequest(guid, url, [commandID, "REFRESHTABLE", "ADDHISTORY"]);
                    }
                    else {
                        a4dn.core.mvc.am_DetailAjaxPostRequest(guid, url, [commandID, "UPDATETABLE", "ADDHISTORY"]);
                    }
                }
            }
            else {
                // Show Error on correct fields
                $form.find('.a4dn-val-error-display-rule').each(function () {
                    var $this = $(this);
                    var fieldShowError = $this.data("a4dn-show-error-for");
                    var checkFields = $this.data("a4dn-when-in-error");
                    // checkFields is an array - need to test with multiple keys
                    $.each(checkFields, function (index, val) {
                        if (!$form.find("[name=" + val + "]").valid()) {
                            var errorArray = {};
                            errorArray[fieldShowError] = $this.data("a4dn-error-message");
                            $form.find('form').validate().showErrors(errorArray);
                        }
                    });
                });

                a4dn.core.mvc.am_FormNotificationValidationErrors($form);
            }
            break;

        case "NEWLOOK_SAVE":
            if ($module.length === 0) {
                return;
            }

            if ($form.length) {
                $form.removeClass("a4dn-dirty");

                if (typeof options.initialCommand !== "undefined") {
                    if (options.initialCommand == "SAVE") {
                        a4dn.core.mvc.am_HandelModuleDetailCommand(guid, "REFRESH", null, {
                            onSuccess: function ($newModule) {
                                if ($form.data("a4dn-record-mode") == "New") {
                                    // Save Message
                                    a4dn.core.mvc.am_Notification("extraSmallBox", "Success", 'The record "' + $newModule.data('a4dn-title') + '" was inserted successfully.');

                                    a4dn.core.mvc.am_DetailSaveProcessSuccessCommands($newModule, [options.initialCommand, "REFRESHTABLE", "ADDHISTORY"]);
                                }
                                else {
                                    // Save Message
                                    a4dn.core.mvc.am_Notification("extraSmallBox", "Success", 'The record "' + $newModule.data('a4dn-title') + '" was updated successfully.');

                                    a4dn.core.mvc.am_DetailSaveProcessSuccessCommands($newModule, [options.initialCommand, "UPDATETABLE", "ADDHISTORY"]);
                                }
                            }
                        });

                        return true; // Handled
                    }

                    else {
                        a4dn.core.mvc.am_FetchDetailData($module, {
                            onSuccess: function (data) {
                                if ($form.data("a4dn-record-mode") == "New") {
                                    // Add History Set Title, Unique name and Key
                                    a4dn.core.mvc.am_AddHistoryShortcut({ applicationNumber: $module.data('a4dn-app-number'), moduleNumber: $module.data('a4dn-mod-number'), uniqueKey: a4dn_Base64.am_ToBase64String(data["ap_UniqueKey"]), uniqueName: data["ap_UniqueName"], moduleTitle: data["ap_Title"] });

                                    // Save Message
                                    a4dn.core.mvc.am_Notification("extraSmallBox", "Success", 'The record "' + data["ap_Title"] + '" was inserted successfully.');

                                    a4dn.core.mvc.am_DetailSaveProcessSuccessCommands($module, [options.initialCommand, "REFRESHTABLE"]);
                                }
                                else {
                                    // Add History Set Title, Unique name and Key
                                    a4dn.core.mvc.am_AddHistoryShortcut({ applicationNumber: $module.data('a4dn-app-number'), moduleNumber: $module.data('a4dn-mod-number'), uniqueKey: a4dn_Base64.am_ToBase64String(data["ap_UniqueKey"]), uniqueName: data["ap_UniqueName"], moduleTitle: data["ap_Title"] });

                                    // Save Message
                                    a4dn.core.mvc.am_Notification("extraSmallBox", "Success", 'The record "' + data["ap_Title"] + '" was updated successfully.');

                                    a4dn.core.mvc.am_DetailSaveProcessSuccessCommands($module, [options.initialCommand, "UPDATETABLE"]);
                                }
                            }
                        });
                    }
                }
            }
            break;

        case "REFRESH":

            if ($form.data("a4dn-isreadonly") == "True") {
                commandID = "DISPLAY";
            }
            else {
                commandID = "OPEN";
            }

            url = url + "&pCmdID=" + commandID + a4dn.core.mvc.am_GetNewForPropagationParameters($module) + "&pNewlookSrc=" + $form.data('a4dn-newlook-src');

            a4dn.core.mvc.am_DetailAjaxGetRequest(guid, url, options);

            break;

        case "DELETE":
        case "PERMDELETE":

            url = url + "&pCmdID=" + commandID + "&pNewlookSrc=" + $form.data('a4dn-newlook-src');

            a4dn.core.mvc.am_SmartMessageBox_ConfirmDelete(guid, url, $module.data('a4dn-title'), function (guid, url) {
                //Remove Drop Down Menus - This will cause errors on post
                $form.find('.a4dn-dropdown-menu').each(function () {
                    var $this = $(this);
                    $this.empty();
                    $this.data('a4dn-view-loaded', false);
                });

                // Delete
                a4dn.core.mvc.am_DetailAjaxDeleteRequest(guid, url, successCallback, errorCallback);
            });

            break;

        case "ADDTOHOTLIST":

            uniqueName = $module.data('a4dn-unique-name'),
                uniqueKey = $module.data('a4dn-unique-key'),
                title = $module.data('a4dn-title');

            a4dn.core.mvc.am_AddHotListShortcut
                ({
                    applicationNumber: $module.data('a4dn-app-number'),
                    moduleNumber: $module.data('a4dn-mod-number'),
                    uniqueKey: uniqueKey,
                    uniqueName: uniqueName,
                    moduleTitle: title
                });

            break;

        case "ADDTOFOLDER":

            uniqueName = $module.data('a4dn-unique-name');
            uniqueKey = $module.data('a4dn-unique-key');
            title = $module.data('a4dn-title');

            a4dn.core.mvc.am_AddFolderRecordShortcut
                ({
                    applicationNumber: $module.data('a4dn-app-number'),
                    moduleNumber: $module.data('a4dn-mod-number'),
                    uniqueKey: uniqueKey,
                    uniqueName: uniqueName,
                    moduleTitle: title
                });

            break;
    }

    if (typeof opts.afterProcessCommand == 'function') {
        opts.afterProcessCommand(guid, commandID, $command);
    }
};

a4dn.core.mvc.am_GetNewForPropagationParameters = function ($module) {
    let parentKeys = $module.data('a4dn-newfor-parent-keys'),
        parentTitle = $module.data('a4dn-newfor-parent-title');
    return (parentKeys ? "&pPKeys=" + encodeURIComponent(JSON.stringify(parentKeys)) : "") + (parentTitle ? "&pPTitle=" + encodeURIComponent(parentTitle) : "");
}



a4dn.core.mvc.am_GetSearchParentKeys = function (parentGuid) {
    var parentKeys = "";
    var $parentModule = a4dn.core.mvc.am_Get$Module(parentGuid);

    // Need to get OnPreload Keys instead of Search keys so that names are populated for dropdowns
    if ($parentModule.data('a4dn-type') == "detail") {
        var $parentForm = a4dn.core.mvc.am_Get$Form(parentGuid);
        parentKeys = a4dn.core.mvc.am_GetDetailBroadcastKeys("OnPreload", $parentForm)
    }
    else {
        var $parentTable = a4dn.core.mvc.am_Get$Table(parentGuid);
        var parentTable = $parentTable.DataTable();
        var $parentRow = a4dn.core.mvc.am_GetFocusRow($parentTable);
        parentKeys = a4dn.core.mvc.am_GetBroadcastKeys("OnPreload", $parentTable, $parentRow);
    }

    return (parentKeys ? "&pPKeys=" + encodeURIComponent(parentKeys) : "");
}

a4dn.core.mvc.am_PopulateSearchFormWithParentKeys = function (guid) {
    var $module = a4dn.core.mvc.am_Get$Module(guid);
    var parentGuid = $module.data('a4dn-parent-guid');
    if (parentGuid != "") {

        var $parentModule = a4dn.core.mvc.am_Get$Module(parentGuid);

        // Need to get OnPreload Keys instead of Search keys so that names are populated for dropdowns
        if ($parentModule.data('a4dn-type') == "detail") {
            var $parentForm = a4dn.core.mvc.am_Get$Form(parentGuid);
            parentKeys = a4dn.core.mvc.am_GetDetailBroadcastKeys("OnPreload", $parentForm)
        }
        else {
            var $parentTable = a4dn.core.mvc.am_Get$Table(parentGuid);
            var parentTable = $parentTable.DataTable();
            var $parentRow = a4dn.core.mvc.am_GetFocusRow($parentTable);
            parentKeys = a4dn.core.mvc.am_GetBroadcastKeys("OnPreload", $parentTable, $parentRow);
        }

        var $container = $('#a4dn-search-modal-body');
        a4dn.core.mvc.am_PopulateForm($container, parentKeys === undefined ? {} : JSON.parse(parentKeys));
    }

}


a4dn.core.mvc.am_GetParentKeysAndTitleParameters = function ($module) {
    var parentKeys = "";
    var parentTitle = "";
    var parentGuid = $module.data('a4dn-parent-guid');
    var $parentModule = a4dn.core.mvc.am_Get$Module(parentGuid);
    var parentModuleName = a4dn.core.mvc.am_GetModuleName($parentModule),
        componentLocation = $module.data('a4dn-component-location');

    if (componentLocation === "Detail") {
        var $parentForm = a4dn.core.mvc.am_Get$Form(parentGuid);
        parentKeys = a4dn.core.mvc.am_GetDetailBroadcastKeys("OnPreload", $parentForm);
        parentTitle = $parentModule.data('a4dn-title');
    }
    else {
        var $parentTable = a4dn.core.mvc.am_Get$Table(parentGuid);
        var parentTable = $parentTable.DataTable();
        var $parentRow = a4dn.core.mvc.am_GetFocusRow($parentTable);

        parentKeys = a4dn.core.mvc.am_GetBroadcastKeys("OnPreload", $parentTable, $parentRow);
        parentTitle = parentTable.cell($parentRow, "A4DN_Title:name").data();
    }

    if (parentTitle)
        parentTitle = parentTitle.replace("{ModuleName}", parentModuleName);

    return (parentKeys ? "&pPKeys=" + encodeURIComponent(parentKeys) : "") + (parentTitle ? "&pPTitle=" + encodeURIComponent(parentTitle) : "");
};

a4dn.core.mvc.am_GetParentKeysParameter = function ($module) {
    let parentKeys = "";
    let parentGuid = $module.data('a4dn-parent-guid');
    let componentLocation = $module.data('a4dn-component-location');

    if (componentLocation === "Detail") {
        let $parentForm = a4dn.core.mvc.am_Get$Form(parentGuid);
        parentKeys = a4dn.core.mvc.am_GetDetailBroadcastKeys("OnPreload", $parentForm);
    }
    else {
        let $parentTable = a4dn.core.mvc.am_Get$Table(parentGuid);
        let $parentRow = a4dn.core.mvc.am_GetFocusRow($parentTable);
        parentKeys = a4dn.core.mvc.am_GetBroadcastKeys("OnPreload", $parentTable, $parentRow);
    }

    return (parentKeys ? "&pPKeys=" + encodeURIComponent(parentKeys) : "");
};

a4dn.core.mvc.am_HandelModuleExplorerCommand = function (guid, commandID, $command, options) {
    var opts = $.extend({
        beforeProcessCommand: function (guid, commandID, $command, table, $row, $selectedRows) {
            return false;
        },
        afterProcessCommand: function (guid, commandID, $command) {
        },
        perCallCallbacks: undefined
    }, options || {});

    // For multi-row selection, one viewcolumn should be defined for a boolean property to display a checkbox.
    // The checkbox must include data-a4dn-row-select="True" to be included in the $selectedRows collection.
    //  
    //      '<input type="checkbox"  name="IsSelected" '+ ((data == true) ? 'checked' : '') +' data-a4dn-row-select="True" />'
    //
    // The default commands, including calling am_ProcessRequest, do not currently use these row selections,
    // but custom commands implemented in the beforeProcessCommand callback function can use them.

    // Alternate approach - include data-a4dn-process-request-command-id:
    //
    //      '<input type="checkbox"  name="IsSelected" '+ ((data == true) ? 'checked' : '') +' data-a4dn-process-request-command-id="SELECTCBX"/>'
    //
    // When the checkbox is selected/cleared, the module's beforeDataGridAjaxProcessRequestPost 
    // callback will be called with the specified commandID, row, and record data. This can be
    // used for the primary processing, or it can be used to enable/disable toolbar buttons for other commands.
    //
    // POReceivingLabelDetailExplorerComandCallBackFunctions = {
    //     beforeProcessCommand: function (guid, commandID, $command) { ... },
    //     afterProcessCommand: function (guid, commandID, $command) { ... },
    //  
    //     beforeDataGridAjaxProcessRequestPost: function (guid, $table, $row, commandID, data) {
    //         switch (commandID) {
    //             case "SELECTCBX":
    //                 console.log("record selected", commandID, $row, data);
    //                 let $toolbar = $("#a4dn-toolbar-commands-" + guid);
    //                 ReceivingTransfersBusinessRules.setCommandDisabled($toolbar, "CreateTransfer", $table.find("[name='IsSelected']:checked").length === 0);
    //                 return true;
    //         }
    //     }
    // };

    let $module = a4dn.core.mvc.am_Get$Module(guid),
        $table = a4dn.core.mvc.am_Get$Table(guid),
        table = $table.DataTable(),
        $row = a4dn.core.mvc.am_GetFocusRow($table),
        $selectedRows = $table.find("[data-a4dn-row-select='True']:checked").map(function () { return this.closest('tr'); }),
        overrideUrl = undefined,
        url,
        successCallback = undefined,
        errorCallback = undefined;

    if (typeof opts.beforeProcessCommand == 'function') {

        var ret = opts.beforeProcessCommand(guid, commandID, $command, table, $row, $selectedRows);
        if (typeof ret == 'object') {
            commandID = ret.commandID === undefined ? commandID : ret.commandID;
            overrideUrl = ret.overrideUrl;
            successCallback = ret.successCallback;
            errorCallback = ret.errorCallback;
            // TODO: make use of successCallback and errorCallback in more than just the DELETE commands

            if (ret.handled) { return; }
        }
        if (typeof ret == 'boolean' && ret) { return; }
    }


    if (overrideUrl !== undefined) {
        url = overrideUrl;
    } else if (commandID === "DELETE" || commandID === "PERMDELETE") {
        url = $table.data('a4dn-delete-href');
    }
    else {
        url = $table.data('a4dn-dtl-view-href');
    }

    switch (commandID) {
        case "NEW":
        case "COPY":
            var zeroState = $command.data('a4dn-zero-state');
            if (zeroState === "E" || zeroState === "V") {
                url = url + "&pCmdID=" + commandID;

                // Create Module Explorer Detail Tab
                a4dn.core.mvc.am_CreateModuleExplorerDetailTab({ url: url, moduleTitle: a4dn.core.mvc.am_GetModuleName($module) + " Detail", moduleImageSource: a4dn.core.mvc.am_GetModuleImageSource($module) });
            }
            else {

                $table.find('tr.highlight').each(function () {
                    var $this = $(this);

                    var uniqueKey = table.cell($this, "A4DN_UniqueKey:name").data();
                    var $module = a4dn.core.mvc.am_Get$Module(guid);

                    var moduleName = a4dn.core.mvc.am_GetModuleName(a4dn.core.mvc.am_Get$Module(guid));
                    var title = moduleName + " Detail";

                    // Create Module Explorer Detail Tab
                    url = url + "&pUKy=" + uniqueKey + "&pCmdID=" + commandID;
                    a4dn.core.mvc.am_CreateModuleExplorerDetailTab({ url: url, moduleTitle: title, moduleImageSource: a4dn.core.mvc.am_GetModuleImageSource($module) });
                });
            }
            break;

        case "NEWFOR":

            url = url + "&pCmdID=" + commandID + a4dn.core.mvc.am_GetParentKeysAndTitleParameters($module);

            if (options.perCallCallbacks && options.perCallCallbacks["NEWFOR"]) {
                url = options.perCallCallbacks.NEWFOR(url);
            }

            // Create Module Explorer Detail Tab
            a4dn.core.mvc.am_CreateModuleExplorerDetailTab({ url: url, moduleTitle: a4dn.core.mvc.am_GetModuleName($module) + " Detail", moduleImageSource: a4dn.core.mvc.am_GetModuleImageSource($module) });

            break;

        case "COPYFOR":

            $table.find('tr.highlight').each(function () {
                var $this = $(this);

                var uniqueKey = table.cell($this, "A4DN_UniqueKey:name").data();
                var $module = a4dn.core.mvc.am_Get$Module(guid);

                var moduleName = a4dn.core.mvc.am_GetModuleName(a4dn.core.mvc.am_Get$Module(guid));
                var title = moduleName + " Detail";

                // Create Module Explorer Detail Tab
                url = url + "&pUKy=" + uniqueKey + "&pCmdID=" + commandID + a4dn.core.mvc.am_GetParentKeysAndTitleParameters($module);
                a4dn.core.mvc.am_CreateModuleExplorerDetailTab({ url: url, moduleTitle: title, moduleImageSource: a4dn.core.mvc.am_GetModuleImageSource($module) });
            });

            break;

        case "DELETE":
        case "PERMDELETE":

            url = url + "&pUKy=" + table.cell($row, "A4DN_UniqueKey:name").data() + "&pCmdID=" + commandID;
            var moduleName = a4dn.core.mvc.am_GetModuleName(a4dn.core.mvc.am_Get$Module(guid));
            var title = table.cell($row, "A4DN_Title:name").data();
            title = title.replace("{ModuleName}", moduleName);

            a4dn.core.mvc.am_SmartMessageBox_ConfirmDelete(guid, url, title, function (guid, url) {
                // Delete
                a4dn.core.mvc.am_DetailAjaxDeleteRequest(guid, url, successCallback, errorCallback);
            });

            break;

        case "REFRESH":
            a4dn.core.mvc.am_ReloadDataTable({ guid: guid });

            break;

        case "EXCELEXPORT":
            _DataTablesExcelExportAjaxRequest = true;
            _DataTablesExcelExportAll = false;
            a4dn.core.mvc.am_ReloadDataTable({ guid: guid });

            break;

        case "EXCELEXPORT_ALL":
            _DataTablesExcelExportAjaxRequest = true;
            _DataTablesExcelExportAll = true;
            a4dn.core.mvc.am_ReloadDataTable({ guid: guid });

            break;

        case "SEARCH":

            a4dn.core.mvc.am_ShowModalSearch(guid);

            break;

        case "PREVIEW":

            var moduleExplorerId = _ModuleIdPrefix + guid;

            if ($command.data("a4dn-checked-state") == "true") {
                // hide Preview
                a4dn.core.mvc.am_HidePreviewPane(guid);

                waitForFinalEvent(function () {
                    $jq("#" + moduleExplorerId).trigger("resize");
                    a4dn.core.mvc.am_ResizeGrid(guid);
                }, 500, guid);
            }
            else {
                // show Preview
                a4dn.core.mvc.am_ShowPreviewPane(guid);

                waitForFinalEvent(function () {
                    $jq("#" + moduleExplorerId).trigger("resize");
                    a4dn.core.mvc.am_ResizeGrid(guid);
                }, 500, guid);

                //Trigger Count
                $table.trigger("itemSelectedCount", [a4dn.core.mvc.am_GetBroadcastKeys("OnSearch", $table, $row)]);
            }

            break;

        case "OPEN":
        case "DISPLAY":

            $table.find('tr.highlight').each(function () {
                var $this = $(this);

                var uniqueKey = table.cell($this, "A4DN_UniqueKey:name").data();
                var uniqueName = table.cell($this, "A4DN_UniqueName:name").data();
                var $module = a4dn.core.mvc.am_Get$Module(guid);
                var title = table.cell($this, "A4DN_Title:name").data()
                title = title.replace("{ModuleName}", a4dn.core.mvc.am_GetModuleName(a4dn.core.mvc.am_Get$Module(guid)));

                // Create Module Explorer Detail Tab
                url = url + "&pUKy=" + uniqueKey + "&pCmdID=" + commandID + a4dn.core.mvc.am_GetParentKeysParameter($module);
                a4dn.core.mvc.am_CreateOrActivateModuleExplorerDetailTab({ applicationNumber: a4dn.core.mvc.am_GetApplicationNumber($module), moduleNumber: a4dn.core.mvc.am_GetModuleNumber($module), uniqueName: uniqueName, uniqueKey: uniqueKey, commandID: commandID, url: url, moduleTitle: title, moduleImageSource: a4dn.core.mvc.am_GetModuleImageSource($module) });
            });

            break;

        case "ADDTOHOTLIST":

            $table.find('tr.highlight').each(function () {
                var $this = $(this);

                var uniqueName = table.cell($this, "A4DN_UniqueName:name").data();
                var uniqueKey = table.cell($this, "A4DN_UniqueKey:name").data();

                var title = table.cell($this, "A4DN_Title:name").data();
                title = title.replace("{ModuleName}", a4dn.core.mvc.am_GetModuleName(a4dn.core.mvc.am_Get$Module(guid)));

                a4dn.core.mvc.am_AddHotListShortcut({
                    applicationNumber: $module.data('a4dn-app-number'),
                    moduleNumber: $module.data('a4dn-mod-number'),
                    uniqueKey: uniqueKey,
                    uniqueName: uniqueName,
                    moduleTitle: title
                });
            });

            break;

        case "ADDTOFOLDER":

            $table.find('tr.highlight').each(function () {
                var $this = $(this);

                var uniqueName = table.cell($this, "A4DN_UniqueName:name").data();
                var uniqueKey = table.cell($this, "A4DN_UniqueKey:name").data();

                var title = table.cell($this, "A4DN_Title:name").data()
                title = title.replace("{ModuleName}", a4dn.core.mvc.am_GetModuleName(a4dn.core.mvc.am_Get$Module(guid)));

                a4dn.core.mvc.am_AddFolderRecordShortcut({
                    applicationNumber: $module.data('a4dn-app-number'),
                    moduleNumber: $module.data('a4dn-mod-number'),
                    uniqueKey: uniqueKey,
                    uniqueName: uniqueName,
                    moduleTitle: title
                });
            });

            break;

        default:

            $table.find('tr.highlight').each(function () {
                var $this = $(this);
                // Default - Call Process Request
                a4dn.core.mvc.am_DataGridAjaxProcessRequestPostRequest(guid, $table, $this, commandID);
            });

        //var zeroState = $command.data('a4dn-zero-state');
        //if (zeroState == "E" || zeroState == "V") {
        //    var $table = a4dn.core.mvc.am_Get$Table(guid);
        //    url = url + "&pCmdID=" + commandID;
        //    var $module = a4dn.core.mvc.am_Get$Module(guid);

        //    // Create Module Explorer Detail Tab
        //    a4dn.core.mvc.am_CreateModuleExplorerDetailTab({ url: url, moduleTitle: a4dn.core.mvc.am_GetModuleName($module) + " Detail", moduleImageSource: a4dn.core.mvc.am_GetModuleImageSource($module) })
        //}
        //else {
        //    var $table = a4dn.core.mvc.am_Get$Table(guid);
        //    var table = $table.DataTable();

        //    $table.find('tr.highlight').each(function () {
        //        var $this = $(this);

        //        var uniqueKey = table.cell($this, "A4DN_UniqueKey:name").data();
        //        var $module = a4dn.core.mvc.am_Get$Module(guid);

        //        var moduleName = a4dn.core.mvc.am_GetModuleName(a4dn.core.mvc.am_Get$Module(guid));
        //        var title = moduleName + " Detail";

        //        // Create Module Explorer Detail Tab
        //        url = $table.data('a4dn-dtl-view-href') + "&pUKy=" + uniqueKey + "&pCmdID=" + commandID;
        //        a4dn.core.mvc.am_CreateModuleExplorerDetailTab({ url: url, moduleTitle: title, moduleImageSource: a4dn.core.mvc.am_GetModuleImageSource($module) })
        //    });
        //}
    }

    if (typeof opts.afterProcessCommand == 'function') {
        opts.afterProcessCommand(guid, commandID, $command);
    }
};

a4dn.core.mvc.am_TriggerCommandClick = function (guid, commandID, perCallCallbacks) {
    var toolbarCommandId = "a4dn-toolbar-commands-" + guid;
    var $commandBtn = $("#" + toolbarCommandId + " .a4dn-command" + "[data-a4dn-command-id='" + commandID + "']").first();
    $commandBtn.trigger('click', [perCallCallbacks]);
}

a4dn.core.mvc.am_DataGridAjaxSelectionChangedPostRequest = function (guid, $table, $row, selectionState) {
    var url = $table.data('a4dn-selection-changed-href') + "&selectionState=" + selectionState;

    var $container = $table;

    var data = "";

    if ($row.length) {
        var table = $table.DataTable();
        data = table.row($row).data()
    }

    let commandCallBackNamespace = a4dn.core.mvc.am_GetCommandCallbackNamespace(guid),
        overrideUrl = undefined;

    if (commandCallBackNamespace !== undefined && typeof commandCallBackNamespace.beforeDataGridAjaxSelectionChangedPostRequest === 'function') {
        let result = commandCallBackNamespace.beforeDataGridAjaxSelectionChangedPostRequest (guid, $table, $row, selectionState, data);
        if (typeof result == 'object') {
            overrideUrl = result.overrideUrl;
            if (result.handled) { return; }
        }
        if (typeof result == 'boolean' && result) { return; }
    }

    if (overrideUrl !== undefined)
        url = overrideUrl;

    a4dn.core.mvc.am_ajax(url, null, {
        data: data,
        type: "post",
        onSuccess: function (data) {
            //console.log($container, data);

            if (data.message != "") {
                a4dn.core.mvc.am_Notification("extraSmallBox", "Success", data.message);
            }

            //a4dn.core.mvc.am_UpdateFormPropertyModel($container, data.output.ap_ChangedPropertyModelDictionary);

            //a4dn.core.mvc.am_PopulateForm($container, data.output.ap_ChangedData);

            a4dn.core.mvc.am_UpdateCommandStates(guid, data.output.ap_CommandStates);

            switch (selectionState) {
                case "Zero":
                    a4dn.core.mvc.am_ZeroSelectedCommandState(guid);
                    break;
                case "One":
                    a4dn.core.mvc.am_OneSelectedCommandState(guid);
                    break;
                case "Multiple":
                    a4dn.core.mvc.am_MultipleSelectedCommandState(guid);
                    break;
            }
        }
    });
};

a4dn.core.mvc.am_DataGridAjaxProcessRequestPostRequest = function (guid, $table, $row, commandID) {
    var url = $table.data('a4dn-process-request-href') + "&commandID=" + commandID;
    var $container = $table;

    var data = "";

    if ($row.length) {
        var table = $table.DataTable();
        data = table.row($row).data();
    }

    let commandCallBackNamespace = a4dn.core.mvc.am_GetCommandCallbackNamespace(guid),
        overrideUrl = undefined;

    if (commandCallBackNamespace !== undefined && typeof commandCallBackNamespace.beforeDataGridAjaxProcessRequestPost === 'function') {
        let result = commandCallBackNamespace.beforeDataGridAjaxProcessRequestPost(guid, $table, $row, commandID, data);
        if (typeof result == 'object') {
            commandID = result.commandID;
            overrideUrl = result.overrideUrl;
            if (result.handled) { return; }
        }
        if (typeof result == 'boolean' && result) { return; }
    }

    if (overrideUrl !== undefined)
        url = overrideUrl;

    a4dn.core.mvc.am_ajax(url, null, {
        data: data,
        type: "post",
        onMessageOnlyResponse: function (data) {
            if (data.message !== "") {
                a4dn.core.mvc.am_Notification("extraSmallBox", "Success", data.message);
            }
        },
        onSuccess: function (data) {
            if (data.message !== "") {
                a4dn.core.mvc.am_Notification("extraSmallBox", "Success", data.message);
            }
        },
        beforeSend: function (jqXHR, ajaxSettings) {

            // start Loading
            $("#a4dn-module-" + guid + " .a4dn-loading").first().removeClass("hidden");
            $table.addClass("loading");
        },
        complete: function (jqXHR, textStatus) {

            // End Loading
            $("#a4dn-module-" + guid + " .a4dn-loading").first().addClass("hidden");
            $table.removeClass("loading");

            // Refresh Table
            a4dn.core.mvc.am_ReloadDataTable({ guid: guid });
        }
    });
};

a4dn.core.mvc.am_LoadModuleHTMLView = function (url, guid) {
    var $container = $("#" + guid).children('.a4dn-module-container').first();

    url = url + "&pPGuid=" + guid;

    a4dn.core.mvc.am_ajax(url, $container, {
        onSuccess: function (data) {
            //console.log($container, data);

            $container.html($(data.markup));

            var $module = $container.find('.a4dn-module-explorer').first();

            $module.css("height", $container.outerHeight(true));
        }
    });

    $container.data('a4dn-view-loaded', true);
}

a4dn.core.mvc.am_InitializeShortcuts = function () {
    $("#a4dn-shortcuts").click(function (a) {
        var $shortcutsDropDown = $("#a4dn-shortcuts-dropdown");

        var $this = $(this);
        $shortcutsDropDown.is(":visible") ? ($shortcutsDropDown.fadeOut(150),
            $this.removeClass("active")) : ($shortcutsDropDown.fadeIn(150)
            );

        if ($shortcutsDropDown.data('a4dn-loaded') == false) {
            $shortcutsDropDown.data('a4dn-loaded', true);

            // Hide Options and content
            $shortcutsDropDown.find('.a4dn-shortcuts-options .a4dn-options').hide();
            $shortcutsDropDown.find('.a4dn-shortcuts-content .a4dn-content').hide();

            // First Time in - Trigger Checked Button
            var radioChecked = $shortcutsDropDown.find(".a4dn-shortcuts-radio input:checked").data('a4dn-shortcut');
            switch (radioChecked) {
                case "a4dn-history":

                    $shortcutsDropDown.find('.a4dn-shortcuts-options .a4dn-history-options').show();

                    // Resize Content
                    $("#a4dn-shortcuts-dropdown .a4dn-shortcuts-content").css("height", $("#a4dn-shortcuts-dropdown").height() - $("#a4dn-shortcuts-dropdown .a4dn-shortcuts-radio").outerHeight(true) - $("#a4dn-shortcuts-dropdown .a4dn-shortcuts-options").outerHeight(true) - $("#a4dn-shortcuts-dropdown .a4dn-shortcuts-footer").outerHeight(true) - 14);

                    setTimeout(function () { $shortcutsDropDown.find('.a4dn-shortcuts-options .a4dn-history-options input:checked').trigger("click"); }, 100)

                    break;
                case "a4dn-folders":
                    break;
                case "a4dn-hotlist":
                    break;
                default:
            }
        }

        a.preventDefault()
    })

    $(document).mouseup(function (a) {
        if ($("#a4dn-folder-options-menu a").is(a.target)) {
            e.stopPropagation();
            e.preventDefault();
            return;
        }

        $(".a4dn-shortcuts-dropdown").is(a.target) || 0 !== $(".a4dn-shortcuts-dropdown").has(a.target).length || ($(".a4dn-shortcuts-dropdown").fadeOut(150), $(".a4dn-shortcuts-dropdown").prev().removeClass("active"))
    })

    $("#a4dn-shortcuts-dropdown .a4dn-shortcuts-radio > label").click(function () {
        var $this = $(this);
        var $radio = $this.find("[name = 'a4dn-shortcuts']")
        var $shortcutsDropDown = $("#a4dn-shortcuts-dropdown");

        if ($radio.data('a4dn-shortcut') == "a4dn-folders") {
            $shortcutsDropDown.find('.a4dn-shortcuts-footer .a4dn-folders-new-btn').removeClass('hidden');
            $shortcutsDropDown.find('.a4dn-shortcuts-footer .a4dn-shortcuts-remove-all').addClass('hidden');
        }
        else {
            $shortcutsDropDown.find('.a4dn-shortcuts-footer .a4dn-folders-new-btn').addClass('hidden');
            $shortcutsDropDown.find('.a4dn-shortcuts-footer .a4dn-shortcuts-remove-all').removeClass('hidden');
        }

        var url = $shortcutsDropDown.find('.a4dn-shortcuts-options .' + $radio.data('a4dn-shortcut') + '-options').data('a4dn-get-href');

        // Hide Options and content
        $shortcutsDropDown.find('.a4dn-shortcuts-options .a4dn-options').hide();
        $shortcutsDropDown.find('.a4dn-shortcuts-content .a4dn-content').hide();
        $shortcutsDropDown.find('.a4dn-shortcuts-content .a4dn-no-items').remove();

        $shortcutsDropDown.find('.a4dn-shortcuts-options .' + $radio.data('a4dn-shortcut') + '-options').show();
        $("#a4dn-shortcuts-dropdown .a4dn-shortcuts-content").css("height", $("#a4dn-shortcuts-dropdown").height() - $("#a4dn-shortcuts-dropdown .a4dn-shortcuts-radio").outerHeight(true) - $("#a4dn-shortcuts-dropdown .a4dn-shortcuts-options").outerHeight(true) - $("#a4dn-shortcuts-dropdown .a4dn-shortcuts-footer").outerHeight(true) - 14);

        $shortcutsDropDown.find('.a4dn-shortcuts-content .' + $radio.data('a4dn-shortcut') + '-content').show();

        if ($shortcutsDropDown.find('.a4dn-shortcuts-options .' + $radio.data('a4dn-shortcut') + '-options').data('a4dn-loaded') == false) {
            // not loaded - trigger checked radio
            $shortcutsDropDown.find('.a4dn-shortcuts-options .' + $radio.data('a4dn-shortcut') + '-options input:checked').trigger("click");
        }
        else {
            setTimeout(function () { a4dn.core.mvc.am_CheckShortcutsHasItems($shortcutsDropDown); }, 500);
        }
    });

    $("#a4dn-shortcuts-dropdown .a4dn-history-options [name='a4dn-history-groupby']").click(function () {
        var $shortcutsDropDown = $("#a4dn-shortcuts-dropdown");

        var $this = $(this);

        var $options = $this.closest('.a4dn-history-options');
        var url = $options.data('a4dn-get-href');
        url = url + "?&groupBy=" + $(this).data('a4dn-groupby');

        var $container = $('#a4dn-shortcuts-dropdown .a4dn-shortcuts-content .a4dn-history-content').first();

        a4dn.core.mvc.am_ajax(url, $container, {
            onSuccess: function (data) {
                //console.log($container, data);

                $container.html($(data.markup));
            },
            beforeSend: function (jqXHR, ajaxSettings) {
                $shortcutsDropDown.find('.a4dn-shortcuts-loading').first().removeClass('hidden');
                $shortcutsDropDown.find('.a4dn-shortcuts-content .a4dn-no-items').remove();
            },
            complete: function (jqXHR, textStatus) {
                $shortcutsDropDown.find('.a4dn-shortcuts-loading').first().addClass('hidden');

                $options.data('a4dn-loaded', true);

                a4dn.core.mvc.am_CheckShortcutsHasItems($shortcutsDropDown);
            },
        });
    });

    $("#a4dn-shortcuts-dropdown .a4dn-folders-options [name='a4dn-folders-type']").click(function () {
        var $shortcutsDropDown = $("#a4dn-shortcuts-dropdown");

        var $this = $(this);
        var $options = $this.closest('.a4dn-folders-options');

        var url = $options.data('a4dn-get-href');

        url = url + "?&isFavorite=" + $(this).data('a4dn-isfavorite') + "&parentFolderID=" + $(this).data('a4dn-parent-id');

        var $container = $('#a4dn-shortcuts-dropdown .a4dn-shortcuts-content .a4dn-folders-content').first();

        a4dn.core.mvc.am_ajax(url, $container, {
            onSuccess: function (data) {
                //console.log($container, data);

                $container.html($(data.markup));
            },
            beforeSend: function (jqXHR, ajaxSettings) {
                $shortcutsDropDown.find('.a4dn-shortcuts-loading').first().removeClass('hidden');
                $shortcutsDropDown.find('.a4dn-shortcuts-content .a4dn-no-items').remove();
            },
            complete: function (jqXHR, textStatus) {
                $shortcutsDropDown.find('.a4dn-shortcuts-loading').first().addClass('hidden');

                $options.data('a4dn-loaded', true);

                a4dn.core.mvc.am_CheckShortcutsHasItems($shortcutsDropDown);
            },
        });
    });

    $("#a4dn-select-folder-modal-body .a4dn-folders-options [name='a4dn-folders-type']").click(function () {
        var $this = $(this);
        var $modalForm = $('#a4dn-select-folder-modal');
        var $modalBody = $('#a4dn-select-folder-modal-body');

        var $options = $this.closest('.a4dn-folders-options');

        var url = $options.data('a4dn-get-href');

        url = url + "?&isFavorite=" + $(this).data('a4dn-isfavorite') + "&parentFolderID=" + $(this).data('a4dn-parent-id') + "&folderType=FOLDER";

        var $container = $('#a4dn-select-folder-modal-body .a4dn-shortcuts-content .a4dn-folders-content').first();

        a4dn.core.mvc.am_ajax(url, $container, {
            onSuccess: function (data) {
                //console.log($container, data);

                $container.html($(data.markup));
            },
            beforeSend: function (jqXHR, ajaxSettings) {
                $modalForm.find('.a4dn-shortcuts-loading').first().removeClass('hidden');
            },
            complete: function (jqXHR, textStatus) {
                $modalForm.find('.a4dn-shortcuts-loading').first().addClass('hidden');

                var radioChecked = "a4dn-folders";

                if ($modalBody.find('.' + radioChecked + '-content .a4dn-tree-list-top-folder').length > 0) {
                    // Has Data
                    $modalBody.find('.' + radioChecked + '-content').show();
                    $modalBody.find('.a4dn-shortcuts-content .a4dn-no-items').remove();
                }
                else {
                    // No Data

                    $modalBody.find('.' + radioChecked + '-content').hide();

                    $modalBody.find('.a4dn-shortcuts-content .a4dn-no-items').remove();

                    var noItemtext = $modalBody.find('.a4dn-shortcuts-options .' + radioChecked + '-options input:checked').data('a4dn-nr-msg');
                    $modalBody.find('.a4dn-shortcuts-content').append('<div class="a4dn-no-items a4dn-center">' + noItemtext + '</div>');
                }
            },
        });
    });

    $("#a4dn-shortcuts-dropdown .a4dn-hotlist-options [name='a4dn-hotlist-type']").click(function () {
        var $shortcutsDropDown = $("#a4dn-shortcuts-dropdown");

        var $this = $(this);
        var $options = $this.closest('.a4dn-hotlist-options');
        var url = $options.data('a4dn-get-href');

        var $container = $('#a4dn-shortcuts-dropdown .a4dn-shortcuts-content .a4dn-hotlist-content').first();

        a4dn.core.mvc.am_ajax(url, $container, {
            onSuccess: function (data) {
                //console.log($container, data);

                $container.html($(data.markup));
            },
            beforeSend: function (jqXHR, ajaxSettings) {
                $shortcutsDropDown.find('.a4dn-shortcuts-loading').first().removeClass('hidden');
                $shortcutsDropDown.find('.a4dn-shortcuts-content .a4dn-no-items').remove();
            },
            complete: function (jqXHR, textStatus) {
                $shortcutsDropDown.find('.a4dn-shortcuts-loading').first().addClass('hidden');

                $options.data('a4dn-loaded', true);

                a4dn.core.mvc.am_CheckShortcutsHasItems($shortcutsDropDown);
            },
        });
    });

    $("#a4dn-shortcuts-dropdown .a4dn-shortcuts-refresh").click(function (e) {
        e.stopPropagation();
        var $shortcutsDropDown = $("#a4dn-shortcuts-dropdown");
        var radioChecked = $shortcutsDropDown.find(".a4dn-shortcuts-radio input:checked").data('a4dn-shortcut');

        $shortcutsDropDown.find("." + radioChecked + "-options input:checked").trigger("click");
    });

    $("#a4dn-shortcuts-dropdown .a4dn-shortcuts-remove-all").click(function (e) {
        e.stopPropagation();
        var $shortcutsDropDown = $("#a4dn-shortcuts-dropdown");
        var radioChecked = $shortcutsDropDown.find(".a4dn-shortcuts-radio input:checked").data('a4dn-shortcut');

        var noBtn = $('#a4dn-main-content').data('a4dn-lang-no'); //"No"
        var yesBtn = $('#a4dn-main-content').data('a4dn-lang-yes'); //"Yes"

        $.SmartMessageBox({
            title: $('#a4dn-main-content').data('a4dn-lang-notification-removeall-title'), //'Confirm Remove All?'
            content: $shortcutsDropDown.find('.a4dn-shortcuts-options .' + radioChecked + '-options input:checked').data('a4dn-rmv-all-msg'),
            buttons: '[' + noBtn + '][' + yesBtn + ']'
        }, function (ButtonPressed) {
            if (ButtonPressed === yesBtn) {
                a4dn.core.mvc.am_RemoveShortcut($shortcutsDropDown.find("." + radioChecked + "-options"));
            }
        });
    });

    $("#a4dn-shortcuts-dropdown .a4dn-folders-new-btn").click(function (e) {
        e.stopPropagation();
        e.preventDefault();

        a4dn.core.mvc.am_AddFolderShortcut($("#a4dn-shortcuts-dropdown .a4dn-folders-content .a4dn-folders-data"));
    });

    a4dn.core.mvc.am_CheckShortcutsHasItems = function ($this) {
        var radioChecked = $this.find(".a4dn-shortcuts-radio input:checked").data('a4dn-shortcut');

        if ($this.find('.' + radioChecked + '-content ul.a4dn-tree-list').length > 0 || $this.find('.' + radioChecked + '-content li.a4dn-tree-list-item').length > 0) {
            // Has Data
            $this.find('.' + radioChecked + '-content').show();

            $this.find('.a4dn-shortcuts-remove-all').removeClass("disabled");
            $this.find('.a4dn-shortcuts-remove-all').prop('disabled', false);
            $this.find('.a4dn-shortcuts-content .a4dn-no-items').remove();
        }
        else {
            // No Data

            $this.find('.' + radioChecked + '-content').hide();

            $this.find('.a4dn-shortcuts-remove-all').addClass("disabled");
            $this.find('.a4dn-shortcuts-remove-all').prop('disabled', true);

            $this.find('.a4dn-shortcuts-content .a4dn-no-items').remove();

            var noItemtext = $this.find('.a4dn-shortcuts-options .' + radioChecked + '-options input:checked').data('a4dn-nr-msg');

            $this.find('.a4dn-shortcuts-content').append('<div class="a4dn-no-items a4dn-center">' + noItemtext + '</div>');
        }

        // Resize Content
        $this.find(".a4dn-shortcuts-content").css("height", $this.height() - $this.find(".a4dn-shortcuts-radio").outerHeight(true) - $this.find(".a4dn-shortcuts-options").outerHeight(true) - $this.find(".a4dn-shortcuts-footer").outerHeight(true) - 14);
    }

    $("#a4dn-shortcuts-dropdown").resize(function () {
        $("#a4dn-shortcuts-dropdown .a4dn-shortcuts-content").css("height", $("#a4dn-shortcuts-dropdown").height() - $("#a4dn-shortcuts-dropdown .a4dn-shortcuts-radio").outerHeight(true) - $("#a4dn-shortcuts-dropdown .a4dn-shortcuts-options").outerHeight(true) - $("#a4dn-shortcuts-dropdown .a4dn-shortcuts-footer").outerHeight(true) - 14);
    });
}

a4dn.core.mvc.am_InitializeShortcutData = function () {
    pageSetUp();

    $('.a4dn-tree > ul').attr('role', 'tree').find('ul').attr('role', 'group');

    $('.a4dn-tree').find('.a4dn-folder-span > a.a4dn-remove-group').off();
    $('.a4dn-tree').find('.a4dn-folder-span > a.a4dn-remove-group').on('click', function (e) {
        e.stopPropagation();

        var $this = $(this);
        if ($this.hasClass('a4dn-remove-group')) {
            a4dn.core.mvc.am_RemoveShortcut($this);
        }
    });

    $('.a4dn-tree').find('.a4dn-folder-span > a.a4dn-folder-options-btn').off();
    $('.a4dn-tree').find('.a4dn-folder-span > a.a4dn-folder-options-btn').on('click', function (e) {
        e.stopPropagation();

        var $this = $(this);
        var $folderOptionsMenu = $('#a4dn-folder-options-menu');

        var eOffset = $(e.target).offset();

        $folderOptionsMenu.css({
            'top': eOffset.top + $this.outerHeight(true),
            'left': eOffset.left - $folderOptionsMenu.width()
        });

        //Expand Folder
        var $folderSpan = $this.closest('.a4dn-folder-span');
        if ($folderSpan.next('ul').data('a4dn-expanded') == false) {
            $folderSpan.trigger('click');
        }

        $folderOptionsMenu.off();
        $folderOptionsMenu.on('mouseleave', function (e) {
            $folderOptionsMenu.hide();
        });

        $folderOptionsMenu.on('click', 'a', function (e) {
            var $athis = $(this);
            e.preventDefault();
            e.stopPropagation();
            $folderOptionsMenu.hide();

            if ($athis.hasClass('a4dn-folder-add')) {
                a4dn.core.mvc.am_AddFolderShortcut($this);
            }
            if ($athis.hasClass('a4dn-folder-rename')) {
                a4dn.core.mvc.am_RenameFolder($this);
            }
            if ($athis.hasClass('a4dn-folder-remove')) {
                a4dn.core.mvc.am_RemoveShortcut($this);
            }
        });

        $folderOptionsMenu.slideToggle("fast");
    });

    $('.a4dn-tree').find('li:has(ul)').find(' > span').off();
    $('.a4dn-tree').find('li:has(ul)').addClass('parent_li').attr('role', 'treeitem').find(' > span').attr('title', 'Expand this branch').on('click', function (e) {
        e.preventDefault();
        //  e.stopPropagation();

        var $this = $(this);

        var $container = $this.next('ul');

        if ($container.data('a4dn-loaded') == false) {
            var url = $container.data('a4dn-href');

            a4dn.core.mvc.am_ajax(url, $container, {
                onSuccess: function (data) {
                    //console.log($container, data);

                    $container.html($(data.markup));

                    $container.data('a4dn-loaded', true);
                    $container.data('a4dn-expanded', true);
                    $this.attr('title', 'Collapse this branch').find(' > i').first().removeClass().addClass('fa fa-lg fa-minus-circle');

                    $this.next('ul').data('a4dn-expanded', true);
                },
                beforeSend: function (jqXHR, ajaxSettings) {
                    $('#a4dn-shortcuts-dropdown').find('.a4dn-shortcuts-loading').first().removeClass('hidden');
                },
                complete: function (jqXHR, textStatus) {
                    $('#a4dn-shortcuts-dropdown').find('.a4dn-shortcuts-loading').first().addClass('hidden');

                    a4dn.core.mvc.am_InitializeShortcutData();

                    $this.find(' > i').each(function () {
                        if ($(this).hasClass('fa-folder')) {
                            $(this).removeClass('fa-folder').addClass('fa-folder-open');
                        }
                    });
                },
            });
        }
        else {
            a4dn.core.mvc.am_ExpandCollapseTreeNode($this);
        }
    });

    $('.a4dn-tree').off();
    $(".a4dn-tree").on("click", "a", function (e) {
        e.preventDefault();
        e.stopPropagation();

        var $this = $(this);

        if ($this.hasClass('a4dn-folder-select-btn')) {
            $('#a4dn-select-folder-modal').modal('hide');

            var options = $('#a4dn-select-folder-modal').data('a4dn-options');
            options.isFavorite = $('#a4dn-select-folder-modal .a4dn-folders-options input:checked').data('a4dn-isfavorite');
            options.parentFolderID = $(this).closest('.a4dn-folder-span').data('a4dn-folder-id');
            options.parentFolderName = $(this).closest('.a4dn-folder-span').find('.a4dn-folder-name').text();
            a4dn.core.mvc.am_AddFolderRecordShortcut(options);
            return;
        }

        if ($this.hasClass('a4dn-remove-item')) {
            a4dn.core.mvc.am_RemoveShortcut($this);
        }
        else {
            var applicationNumber = $this.data('a4dn-app-number');
            var moduleNumber = $this.data('a4dn-mod-number');
            var uniqueName = $this.data('a4dn-unique-name');
            var uniqueKey = $this.data('a4dn-unique-key');
            var commandID = $this.data('a4dn-command-id');
            var detailViewURL = $this.attr('href') + "&pCmdID=" + commandID + "&pUKy=" + uniqueKey;
            var moduleTitle = $this.text();
            var moduleImageSource = $this.prev('.a4dn-mod-image').attr('src');

            $("#a4dn-shortcuts-dropdown").fadeOut(150), $("#a4dn-shortcuts-dropdown").prev().removeClass("active");

            a4dn.core.mvc.am_CreateOrActivateModuleExplorerDetailTab({ applicationNumber: applicationNumber, moduleNumber: moduleNumber, uniqueName: uniqueName, uniqueKey: uniqueKey, commandID: commandID, url: detailViewURL, moduleTitle: moduleTitle, moduleImageSource: moduleImageSource });
        }
    });
}

a4dn.core.mvc.am_ExpandCollapseTreeNode = function ($this) {
    var $children = $this.next('ul').find('> li.a4dn-tree-list-item');
    var $childrenfolder = $this.next('ul').find('> li.a4dn-tree-list-folder');

    if ($children.is(':visible') || $childrenfolder.is(':visible')) {
        $children.hide('fast');
        $childrenfolder.hide('fast');
        $this.attr('title', 'Expand this branch').find(' > i').first().removeClass().addClass('fa fa-lg fa-plus-circle');

        $this.find(' > i').each(function () {
            if ($(this).hasClass('fa-folder-open')) {
                $(this).removeClass('fa-folder-open').addClass('fa-folder');
            }
        });

        $this.next('ul').data('a4dn-expanded', false);
    } else {
        $children.show('fast');
        $childrenfolder.show('fast');
        $this.attr('title', 'Collapse this branch').find(' > i').first().removeClass().addClass('fa fa-lg fa-minus-circle');

        $this.find(' > i').each(function () {
            if ($(this).hasClass('fa-folder')) {
                $(this).removeClass('fa-folder').addClass('fa-folder-open');
            }
        });
        $this.next('ul').data('a4dn-expanded', true);
    }
}

a4dn.core.mvc.am_RemoveShortcut = function ($this) {
    var url = $this.data('a4dn-remove-href');

    a4dn.core.mvc.am_ajax(url, null, {
        type: "POST",
        onMessageOnlyResponse: function (data, textStatus, jqXHR) {
            //console.log(data);

            if ($this.hasClass('a4dn-remove-item')) {
                var $ul = $this.closest('ul.a4dn-tree-list');
                var $span = $ul.prev('span');
                $this.closest('li.a4dn-tree-list-item').remove();

                if ($ul.find('li.a4dn-tree-list-item').length == 0) {
                    // remove span - no more items
                    $span.remove();
                    $ul.remove();
                }
            }
            if ($this.hasClass('a4dn-remove-group')) {
                var $span = $this.closest('span')
                $span.next('ul.a4dn-tree-list').remove();
                $span.remove();
            }

            if ($this.hasClass('a4dn-folder-options-btn')) {
                var $removeSpan = $this.closest('.a4dn-folder-span').first();
                $removeSpan.next('ul').remove();
                $removeSpan.remove();
            }

            if ($this.hasClass('a4dn-options')) {
                $('#a4dn-shortcuts-dropdown .a4dn-shortcuts-content .a4dn-data').remove();
            }

            a4dn.core.mvc.am_CheckShortcutsHasItems($('#a4dn-shortcuts-dropdown'));

            if (data.message != "") {
                a4dn.core.mvc.am_Notification("extraSmallBox", "Success", data.message);
            }
        }
    });
}

a4dn.core.mvc.am_AddFolderShortcut = function ($this) {
    var url = $this.data('a4dn-add-href');

    a4dn.core.mvc.am_ajax(url, null, {
        type: "POST",
        onSuccess: function (data) {
            //console.log(data);

            var $folderSpan = $this.closest('.a4dn-folder-span');

            if ($this.hasClass('a4dn-folders-data')) {
                $this.find(".a4dn-tree-list-top-folder").append(data.markup);
                // Rename Folder
                var $newSpan = $this.find(".a4dn-tree-list-top-folder").find('.a4dn-folder-span').filter('[data-a4dn-folder-id="' + data.output.FolderID + '"]');
                var $optionsbtn = $newSpan.find('> a.a4dn-folder-options-btn');
                a4dn.core.mvc.am_RenameFolder($optionsbtn);
            }
            else {
                var $container = $folderSpan.next('ul').find('> li.a4dn-tree-list-folder').last();
                if ($container.length) {
                    $container.after(data.markup);
                }
                else {
                    $container = $folderSpan.next('ul');
                    $container.prepend(data.markup);
                }
                // Rename Folder
                var $newSpan = $folderSpan.next('ul').find('.a4dn-folder-span').filter('[data-a4dn-folder-id="' + data.output.FolderID + '"]');
                var $optionsbtn = $newSpan.find('> a.a4dn-folder-options-btn');
                a4dn.core.mvc.am_RenameFolder($optionsbtn);
            }

            a4dn.core.mvc.am_InitializeShortcutData();
            a4dn.core.mvc.am_CheckShortcutsHasItems($('#a4dn-shortcuts-dropdown'));

            if (data.message != "") {
                a4dn.core.mvc.am_Notification("extraSmallBox", "Success", data.message);
            }
        }
    });
}

a4dn.core.mvc.am_RenameFolder = function ($this) {
    var $folderName = $this.closest('.a4dn-folder-span').find('.a4dn-folder-name');
    var $folderRename = $this.closest('.a4dn-folder-span').find('.a4dn-folder-rename');
    $folderName.addClass('hidden');
    $folderRename.removeClass('hidden');

    setTimeout(function () {
        $folderRename.focus();
        $folderRename.select();
    }, 500);

    $folderRename.off();
    $folderRename.on('focusout', function (e) {
        var url = $this.data('a4dn-rename-href') + "&folderName=" + $folderRename.val();

        a4dn.core.mvc.am_ajax(url, null, {
            type: "POST",
            onSuccess: function (data) {
                //console.log(data);

                $folderName.text($folderRename.val());

                if (data.message != "") {
                    a4dn.core.mvc.am_Notification("extraSmallBox", "Success", data.message);
                }
            },
            complete: function (jqXHR, textStatus) {
                $folderName.removeClass('hidden');
                $folderRename.addClass('hidden');
            },
        });
    });
}

a4dn.core.mvc.am_SetupAccountSettings = function ($this) {
    // Manage Security Question Dropdown
    $(".a4dn-accountsettings").on("change", "#security-question-suggestions", function (e) {
        var $select = $(this),
            $input = $($select.data("for")),
            value = $select.val();
        // Copy the select's value to the input control
        $input.val(value);

        if (value === "") {
            $select.addClass("no-selection");
        }
        else {
            $select.removeClass("no-selection");
        }
    });

    // Initialize Unobtrusive Validation and submit handlers
    $(".a4dn-accountsettings form").each(function () {
        var $form = $(this);

        // Try to attach validation
        $.validator.unobtrusive.parse($form);

        if ($form.data("validator")) {
            $form.data("validator").settings.submitHandler = function (form) { return submitForm($form); }
        }
        else {
            $form.on("submit", function (e) { e.preventDefault(); return submitForm($form); });
        }
    });

    function submitForm($form) {
        if ($form.length == 0)
            return false;

        a4dn.core.mvc.am_ajax($form.attr("action"), $form, {
            type: $form.attr("method"),
            data: $form.serialize(),
            onMessageOnlyResponse: function (data) {
                a4dn.core.mvc.am_Notification("smallBox", "Success", data.message);
            }
        });
    }
}

a4dn.core.mvc.am_FormNotificationValidationErrors = function ($form) {
    var msg = "";

    $form.find('span.field-validation-error').each(function () {
        msg = msg + "&#8226; " + $(this).text() + "<br>"
    });

    if (msg != "") {
        a4dn.core.mvc.am_Notification("smallBox", "ValidationError", msg);
    }
}


// Preserve linebreaks in textarea when calling $.val(). See http://api.jquery.com/val/
$.valHooks.textarea = {
    get: function (elem) {
        return elem.value.replace(/\r?\n/g, "\r\n");
    }
};



a4dn.core.mvc.am_StartUserJobLogHeartBeat = function () {
    const trackUserSessions = $('#a4dn-main-content').data('a4dn-userjoblog-track-user-sessions');

    if (trackUserSessions === true || trackUserSessions === "True") {
        const url = $('#a4dn-main-content').data('a4dn-userjoblog-update-href'),
            activityInterval = $('#a4dn-main-content').data('a4dn-userjoblog-activity-interval');

        if (url === undefined) return;

        const updateUserJobLog = function () {
            a4dn.core.mvc.am_ajax(url, null, {
                type: "POST",
                onSuccess: function (data) {
                    if (data.message !== "") {
                        a4dn.core.mvc.am_Notification("extraSmallBox", "Success", data.message);
                    }
                    setTimeout(updateUserJobLog, activityInterval);
                }
            });
        };

        setTimeout(updateUserJobLog, activityInterval);
    }
};

a4dn.core.mvc.am_CreateRecordLock = function (options) {
    const url = $('#a4dn-main-content').data('a4dn-userjoblog-create-record-lock-href');

    if (url === undefined) return;

    a4dn.core.mvc.am_ajax(url, null, {
        type: "POST",
        data: [{ name: 'uniqueName', value: options.uniqueName }, { name: 'uniqueKey', value: options.uniqueKey }],
        onSuccess: function (data) {
            if (data.message !== "") {
                a4dn.core.mvc.am_Notification("extraSmallBox", "Success", data.message);
            }

            if (data.output.LockedByOtherUser === true || data.output.LockedByOtherUser === "True") {

                // Check to see if tab is already open - this could happen if the user double clicks very quickly on the same record. If the Tab is already open, then just activate it. 
                // Need to wait a second to make sure first request will return before subsequent requests
                setTimeout(function () {
                        var $tab = a4dn.core.mvc.am_Get$ModuleExplorerDetailTab(options.moduleNumber, options.uniqueKey);

                        if ($tab.length) {
                            // Tab Open - just Activate
                            $tab.tab('show');

                            var $itm = $("#a4dn-main-module-explorer-dropdown")
                                .find('span:contains("' + options.moduleTitle + '")').first();
                            $itm.trigger("click");

                        } else {

                            a4dn.core.mvc.am_SmartMessageBox_RecordLockedByOtherUser(data.output, options);
                            }

                        }, 1000);
            } else {
                // Create Module Explorer Detail Tab
                a4dn.core.mvc.am_CreateModuleExplorerDetailTab(options);
            }
        }
    });
};

a4dn.core.mvc.am_SmartMessageBox_RecordLockedByOtherUser = function (output, options) {
    const cancelBtn = output.cancelBtnText, //"Cancel"
          openForReadOnlyBtn = output.openForReadOnlyBtnText, //"Open for Read Only"
          clearLockBtn = output.clearLockBtnText; //"Clear Lock"

    $.SmartMessageBox({
        title: output.lockedTitle, //'Record Locked by another User'
        content: output.lockedMessage, //'This record is locked by User {0} on {1} at {2}. Do you want to open for read only?'
        buttons: '[' + cancelBtn + '][' + openForReadOnlyBtn + ']'
        //buttons: '[' + cancelBtn + '][' + clearLockBtn + '][' + openForReadOnlyBtn + ']'
    }, function (ButtonPressed) {
        if (ButtonPressed === openForReadOnlyBtn) {
            // Replace OPEN with Display
            options.commandID = "DISPLAY";
            options.url = options.url.replace("pCmdID=OPEN", "pCmdID=DISPLAY");
            // Create Module Explorer Detail Tab
            a4dn.core.mvc.am_CreateModuleExplorerDetailTab(options);
        }
        //if (ButtonPressed === clearLockBtn) {
        //    return;
        //}
        if (ButtonPressed === cancelBtn) {
            return;
        }
    });
};

a4dn.core.mvc.am_ReleaseRecordLock = function (options) {
    const url = $('#a4dn-main-content').data('a4dn-userjoblog-release-record-lock-href');

    if (url === undefined) return;

    a4dn.core.mvc.am_ajax(url, null, {
        type: "POST",
        data: [{ name: 'uniqueName', value: options.uniqueName }, { name: 'uniqueKey', value: options.uniqueKey }],
        onSuccess: function (data) {
            if (data.message !== "") {
                a4dn.core.mvc.am_Notification("extraSmallBox", "Success", data.message);
            }
        }
    });
};
