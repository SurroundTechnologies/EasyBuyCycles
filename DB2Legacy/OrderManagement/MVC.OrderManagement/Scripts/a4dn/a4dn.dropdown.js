// AB_Dropdown: a4dn.dropdown-4.2.js
//
// Plugin for Dropdown Controls
// Depends on jquery.encapsulatedPlugin.js

/*

SYNOPSYS

In view templates (eg: Detail, ExpBarSearch):

@Html.am_DropDownField(ProductEntity.SizeUnitMeasureCodeProperty, new UnitMeasureDropDown { ap_SelectedEntity = new UnitMeasureEntity { UnitMeasureCode = Model.SizeUnitMeasureCode }}, showLabel: true, labelPosition: AB_FieldLabelPosition.Top)

In a document ready handler:

$(".dropdownControlContainer").AB_Dropdown();

The AB_Dropdown() plugin expects to work with the markup created by
am_DropDownField(), which creates a root element with the class
dropdownControlContainer. The document ready handlers in a4dn-4.2.js
and a4dn.mobile.js contain the code to attach the plugin to dropdowns.
If markup containing dropdown controls is loaded dynamically with an
AJAX request, it is neceesary to attach AB_Dropdown() to those controls
after the markup is appended to the DOM in the AJAX success handler.

OPTIONS

The AB_Dropdown() method does not take any arguments.

METHODS

Public methods can be called on the api object, which is stored in the
"AB_Dropdown" data for the element the plugin is attached to.

var api = $("#somelement").data("AB_Dropdown");
var result = api.PublicMethodName(...);

... TODO: document public methods ...

EVENTS

All event handlers are executed in the context of the element
that matches the jQuery selector used in the initial call to
AB_Dropdown(). The 'this' variable is set to that element.

All event handlers will get a first argument api, which is a
handle to the AB_Dropdown api object, which would be used to call
any public methods.

... TODO: document events fired by AB_Dropdown, including any additional
arguments included in the event call ...

*/

(function ($) {
    "use strict";

    var AB_Dropdown = function (element, options) {
        //--------------------------------------------------------------------
        // Defaults/config
        var $element = $(element),
            $config = $element.siblings(".a4dn-ab_dropdown-configuration");

        if ($config.length > 1) {
            console.error("multiple configs found", $config);
        }

        // $config is a hidden element set up in _A4DN_Dropdown with module-specific and dropdown-control-specific settings
        // The call to AB_Dropdown() can provide overrides in options if needed.

        // console.debug("Attaching AB_Dropdown", $config.data());

        var settings = $.extend({
            url: $config.data('mod_exp_html_view_url'),
            detailViewURL: $config.data('detail_html_view_url'),
            displayMember: $config.data('display_member'),
            searchMember: $config.data('search_member'),
            keyMappings: $config.data('key_mappings'),
            additionalMappings: $config.data('additional_mappings'),
            moduleTitle: $config.data('module_title'),
            moduleImageSource: $config.data('module_image_source'),
            applicationNumber: $config.data('application_number'),
            moduleNumber: $config.data('module_number'),
            filterKeys: $config.data('filter_keys')
            //searchQuery: {},
            //filterProperties: [],
        }, options || {});

        //--------------------------------------------------------------------
        // Private variables

        var $module = $element.closest('.a4dn-module'),
            moduleGuid = $module.data('a4dn-guid'),
            $dropDownMenu,
            $dropDownButton,
            $dropDownSearchButton,
            $dropDownNewButton,
            $dropDownOpenButton,
            currentSelectedUniqueKey,
            showSearchAfterLoad = false,
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
        // $("#myElement").AB_Dropdown(...);
        //
        // var api = $("#myElement").data("AB_Dropdown");
        // api.MyPublicMethod(...);
        //
        //this.SetValue = function () {
        //    // TODO
        //};
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

        // Sets additional properties that will be used to create the query that populates the dropdown.
        // filterKeys is an array of objects with 'name' and 'value' properties. Eg: [ { name: "keyProp1", value: "keyVal1"}, { name: "keyProp2", value: "keyVal2" } ]
        this.setFilterKeys = function (filterKeys, clearCurrentValue, openDropdown) {
            //console.debug("setFilterKeys on " + $element.attr("name"), filterKeys);
            if (!filterKeys) {
                $element.removeData("a4dn-filterkeys");
            }
            else {
                $element.data("a4dn-filterkeys", $.param(filterKeys));
            }
            $dropDownMenu.data('a4dn-view-loaded', false);   // Force reload when opened

            if (clearCurrentValue)
                $element.val("");

            if (openDropdown)
                $element.triggerHandler("input");
        };

        this.enableDropdown = function () {
            $element.removeClass("readonly").removeClass("a4dn-readonly").prop("disabled", false);
            $element.parents("label.input").removeClass("state-disabled");
            $element.siblings(".btn").each(function () { $(this).prop("disabled", false); });
        };

        this.disableDropdown = function () {
            $element.addClass("readonly a4dn-readonly").prop("disabled", true);
            $element.parents("label.input").addClass("state-disabled");
            $element.siblings(".btn").each(function () { $(this).prop("disabled", true); });
        };

        this.isEnabled = function () {
            return $element.prop("disabled") === false;
        };

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

        function addDropDownMenu() {
            $element.after('<ul class="dropdown-menu a4dn-dropdown-menu" style="max-height:400px;" data-a4dn-view-loaded="false"/>');
            return $element.parent().find('ul.a4dn-dropdown-menu');
        }

        function getDropDownButton() {
            return $element.parent().find('button.a4dn-dropdown-toggle-btn');
        }

        function getSearchButton() {
            return $element.parent().find('button.a4dn-dropdown-search-btn');
        }

        function getNewButton() {
            return $element.parent().find('button.a4dn-dropdown-new-btn');
        }

        function getOpenButton() {
            return $element.parent().find('button.a4dn-dropdown-open-btn');
        }

        function addAutoCompleteOff() {
            $element.attr('autocomplete', 'off');
        }

        function setTextboxState(state) {
            $element.removeClass('a4dn-state--initial')
                .removeClass('a4dn-state--updated')
                .removeClass('a4dn-state--selected');
            $element.addClass('a4dn-state--' + state);
        }

        function parseDisplayMember(displayMember) {
            if (displayMember.indexOf("{") < 0) {
                return [{ placeholder: displayMember, names: [displayMember] }];
            }

            let placeholders = displayMember.match(/{.+?}/g),
                tokens = [];

            for (var i = 0; i < placeholders.length; i++) {
                let names = placeholders[i].replace('{', '').replace('}', '').split('|');
                tokens.push({ placeholder: placeholders[i], names: names });
            }

            return tokens;
        }

        function getDisplayValue(getValue) {
            if (settings.displayMember.indexOf("{") < 0) {
                return getValue(settings.displayMember);
            }

            let tokens = parseDisplayMember(settings.displayMember),
                newVal = settings.displayMember,
                numBlankTokens = 0;

            for (let i = 0; i < tokens.length; i++) {
                let placeholder = tokens[i].placeholder;

                for (let j = 0; j < tokens[i].names.length; j++) {
                    let val = getValue(tokens[i].names[j]);

                    if (val !== undefined) {
                        newVal = newVal.replace(placeholder, val);
                        if (val === "")
                            numBlankTokens++;
                        break;
                    }
                }
            }

            if (numBlankTokens === tokens.length)
                return "";

            return newVal;
        }


        function getDisplayValueFromTable(table, $row) {
            return getDisplayValue(function (name) {
                return table.cell($row, name + ":name").data();
            });
        }

        function getDisplayValueFromForm() {
            let $form = $element.closest("form");

            return getDisplayValue(function (name) {
                return $form.find("[name='" + name + "']").val();
            });
        }

        function loadDropDownHTMLView($container, callback) {
            /////Customer/GetModuleExplorerHtmlView

            // Flag as loaded right away, so that subsequent opens won't try to reload before the first load completes
            $container.data('a4dn-view-loaded', true);

            // Clear existing html view
            $container.html("");

            var url = settings.url;

            let filterKeys = $element.data("a4dn-filterkeys");
            if (filterKeys) {
                url = url.replace(/&?pFilterKeys=[^&]*/, "");

                if (url.indexOf("?") < 0)
                    url += "?";
                else
                    url += "&";

                url += "pFilterKeys=" + encodeURIComponent(filterKeys);
            }

            a4dn.core.mvc.am_ajax(url, $container, {
                onSuccess: function (data) {
                    $container.html($(data.markup));
                    $container.find('.a4dn-toolbar').remove();
                    $container.find('.a4dn-table-header').hide();

                    // TODO: Add a blank row to the top of the results so user can clear selection

                    loadDropDownTableEvents();

                    if (showSearchAfterLoad) {
                        showSearch();
                    }

                    if (callback) {
                        callback();
                    }
                },
                onErrorResponse: function (data) {
                    console.error($container, data);

                    if (data.markup)
                        $container.html(data.markup);

                    a4dn.core.mvc.am_Notification("smallBox", "Error", data.message);

                    // Set a4dn-view-loaded to false so that the user can try again
                    $container.data('a4dn-view-loaded', false);
                },
                beforeSend: function (jqXHR, ajaxSettings) {
                    $element.addClass('ui-autocomplete-loading');
                },
                complete: function (jqXHR, textStatus) {
                    $element.removeClass('ui-autocomplete-loading');
                },
            });
        }

        function loadDropDownTableEvents() {
            var $table = $dropDownMenu.find('.a4dn-table');
            var table = $table.DataTable();
            var $form = a4dn.core.mvc.am_Get$Form(moduleGuid);
            var tableGuid = $table.data('a4dn-guid');

            if ($form.length == 0)
                $form = $element.closest("form");

            // If only 1 row in Datable, then automatically select it
            $table.on('draw.dt', function () {
                if (table.data().count() === 1) {
                    $table.find("tbody tr:first").trigger("click");
                }
            });

            // Wire Up Events
            $table.find("tbody").on('click', 'tr', function (e) {
                var $this = $(this);

                //Display Member
                $element.val(getDisplayValueFromTable(table, $this));
                setTextboxState('selected');

                //Keys
                for (let i = 0; i < settings.keyMappings.length; i++) {
                    let columnSource = settings.keyMappings[i].Source,
                        columnValue = table.cell($this, columnSource + ":name").data(),
                        targetName = settings.keyMappings[i].Target,
                        $target = $form.find("[name='" + targetName + "']");
                    
                    // FIXME: $target can contain multiple elements if the form contains dropdowns which are loaded with hidden search forms that have an element named targetName
                    // The filter method is intended to remove any elements that are inside of a hidden a4dn-search-modal-body
                    //      .filter(function (index) { return $(this).parents(".hidden") == 0; });

                    $target.val(columnValue);
                    if ($target.data('a4dn-original-value') != columnValue) {
                        $target.addClass('a4dn-dirty');
                    }
                    else {
                        $target.removeClass('a4dn-dirty');
                    }
                }

                // Additional mappings
                for (let i = 0; i < settings.additionalMappings.length; i++) {
                    let columnSource = settings.additionalMappings[i].Source,
                        columnValue = table.cell($this, columnSource + ":name").data(),
                        targetName = settings.additionalMappings[i].Target,
                        $target = $form.find("[name='" + targetName + "']");
                    
                    // FIXME: $target can contain multiple elements if the form contains dropdowns which are loaded with hidden search forms that have an element named targetName
                    // The filter method is intended to remove any elements that are inside of a hidden a4dn-search-modal-body
                    //      .filter(function (index) { return $(this).parents(".hidden") == 0; });

                    $target.val(columnValue);
                    if ($target.data('a4dn-original-value') != columnValue) {
                        $target.addClass('a4dn-dirty');
                    }
                    else {
                        $target.removeClass('a4dn-dirty');
                    }
                }

                createUniqueKey();

                a4dn.core.mvc.am_InputControlChanged(moduleGuid, $element, "itemChanged", [$this]);

                hideDropdownMenu();
            });

            $form.on("itemSelected", '#a4dn-table-' + tableGuid, function (event, parentKeys, parentTitle, $row) {
                if (!$row.hasClass('highlight')) {
                    return;
                }

                // Only update the display member on selected row
                $element.val(getDisplayValueFromTable(table, $row));
                setTextboxState('selected');
            });

            $(document).keydown(function (e) {
                var $tbody = $table.find("tbody");

                if ($dropDownMenu.css('display') == 'none') {
                    a4dn.core.mvc.am_RemoveAllTableFocus();
                    $tbody.find('.highlight').removeClass('highlight');
                    return;
                }

                if (e.which == 40) {//down arrow
                    e.stopPropagation();

                    if ($tbody.find('.highlight').length == 0) {
                        $dropDownMenu.data('a4dn-stored-value', $element.val())

                        //no rows selected - set first row selected
                        a4dn.core.mvc.am_SetTableFocus($table);
                        a4dn.core.mvc.am_SelectFirstRow(tableGuid);
                        return;
                    }

                    a4dn.core.mvc.am_SelectNextRow($table, $tbody);
                }

                else if (e.which == 38) {//up arrow
                    e.stopPropagation();

                    if ($tbody.find('tr:first').hasClass('highlight')) {
                        a4dn.core.mvc.am_RemoveAllTableFocus();
                        $tbody.find('tr:first').removeClass('highlight');

                        // in a settimeout so that the selection start is at the end of the text
                        setTimeout(function () { $element.val($dropDownMenu.data('a4dn-stored-value')); }, 10);

                        return;
                    }

                    a4dn.core.mvc.am_SelectPreviousRow($table, $tbody);

                } else if (e.which == 13) {//enter
                    e.stopPropagation();
                    var $row = a4dn.core.mvc.am_GetFocusRow($table);
                    $row.trigger('click');

                } else if (e.which == 27) {//Escape
                    hideDropdownMenu();

                } else if (e.which == 9) {//Tab
                    hideDropdownMenu();
                }

            });
        }

        function createUniqueKey() {
            var $form = $element.closest("form");

            currentSelectedUniqueKey = "";

            for (var i = 0; i < settings.keyMappings.length; i++) {
                var $keyTarget = $form.find("[name='" + settings.keyMappings[i].Target + "']"),
                    uniqueValues = $keyTarget.map(function () { return $(this).val(); }).get().filter(function (v, i, self) { return self.indexOf(v) === i; }),
                    keyTargetVal = uniqueValues.length === 0 ? undefined : uniqueValues[0];

                if (keyTargetVal === undefined || keyTargetVal === null) {
                    console.warn("Using empty string for [name='" + settings.keyMappings[i].Target + "'] because val() returned " + keyTargetVal, $element, $keyTarget);
                    keyTargetVal = "";
                }

                if ($keyTarget.length === 0) {
                    console.error("Missing dropdown key target [name='" + settings.keyMappings[i].Target + "'] for " + $element.attr("name"), $form, $element, $keyTarget);
                }
                if (uniqueValues.length > 1) {
                    console.error("Too many (" + $keyTarget.length + ") dropdown key targets [name='" + settings.keyMappings[i].Target + "'] for " + $element.attr("name"), $form, $element, $keyTarget);
                }

                // Create Unique key using padding from max length - encode for transport
                currentSelectedUniqueKey = currentSelectedUniqueKey + keyTargetVal.padEnd($keyTarget.data('val-length-max'), ' ');
            }

            if (currentSelectedUniqueKey.trim() === "") {
                $dropDownOpenButton.prop('disabled', true);
            }
            else {
                $dropDownOpenButton.prop('disabled', false);
            }

            //encode for transport
            currentSelectedUniqueKey = a4dn_Base64.am_ToBase64String(currentSelectedUniqueKey);
        }

        function showSearch() {
            var $table = $dropDownMenu.find('.a4dn-table').first();
            var guid = $table.data('a4dn-guid');

            showSearchAfterLoad = false;

            a4dn.core.mvc.am_ShowModalSearch(guid);
        }

        //this._closeClickHandler = function (e) {
        //    let isInside = $.contains($element.get(0), e.target);
        //    if (!isInside) 
        //        hideDropdownMenu();
        //};

        function showDropdownMenu() {
            $dropDownMenu.show();
            //$(document).on('click', $element._closeClickHandler);
        }

        function hideDropdownMenu() {
            $dropDownMenu.hide();
            //$(document).off('click', $element._closeClickHandler);
        }

        //--------------------------------------------------------------------
        // Plugin Setup Code

        // HINT: Any code that the plugin needs to run the first time it is
        // attached to an element can go here. If the plugin user tries to
        // attach the plugin to the same element a second time, the setup code
        // will not run again. They will get a reference to the same plugin
        // object.

        this.setup = function () {

            addAutoCompleteOff();

            $dropDownNewButton = getNewButton();
            $dropDownOpenButton = getOpenButton();
            $dropDownSearchButton = getSearchButton();
            $dropDownButton = getDropDownButton();

            $dropDownMenu = addDropDownMenu();

            createUniqueKey();

            if (options) {
                this.setFilterKeys(options.filterKeys);
            }

            // Only for templated display member; non-templated must be pre-populated with correct value
            if (settings.displayMember.indexOf('{') >= 0) 
                $element.val(getDisplayValueFromForm());

            setTextboxState('initial');

            $element.on("valueUpdated", function (e) {
                // triggered when value updated via ajax
                createUniqueKey();
                setTextboxState('updated');
            });

            // Don't perform search unless 100ms have passed since the last search-invoking event
            let dbSearch = a4dn.core.mvc.am_Debounce(a4dn.core.mvc.am_Search, 500);
            let loadTable = a4dn.core.mvc.am_Debounce(loadDropDownHTMLView, 500);

            $element.on("input", function (e, dontopen) {
                setTextboxState('updated');

                let $form = $element.closest('form');

                // Reset validation error which is coming from parent keys
                $element.valid();

                // Set DropDown Menu Width
                $dropDownMenu.css("min-width", $element.outerWidth(true));

                let val = encodeURIComponent(settings.searchMember) + "=" + encodeURIComponent($element.val());

                if (encodeURIComponent($element.val()) === "") {

                    // Drop down value is blank - Clear out keyMappings and additionalMappings values, unless they're read-only
                    for (let i = 0; i < settings.keyMappings.length; i++) {
                        let $target = $form.find("[name='" + settings.keyMappings[i].Target + "']");

                        if (!$target.prop('readonly')) {

                            $target.val("");

                            if ($target.data('a4dn-original-value') != "") {
                                $target.addClass('a4dn-dirty');
                            }
                            else {
                                $target.removeClass('a4dn-dirty');
                            }
                        }
                    }

                    for (let i = 0; i < settings.additionalMappings.length; i++) {
                        let $target = $form.find("[name='" + settings.additionalMappings[i].Target + "']");

                        if (!$target.prop('readonly')) {
                            $target.val("");

                            if ($target.data('a4dn-original-value') != "") {
                                $target.addClass('a4dn-dirty');
                            }
                            else {
                                $target.removeClass('a4dn-dirty');
                            }
                        }
                    }

                    // Drop down value is blank - Hide Menu
                    dontopen = true;
                    hideDropdownMenu();
                    setTextboxState('initial');

                    // Trigger the events that happen after the user clicks on a row in the dropdown
                    setTimeout(function () { a4dn.core.mvc.am_InputControlChanged(moduleGuid, $element, "itemChanged", [$this]); }, 0);
                }
                else {

                    let doSearch = function () {
                        var $table = $dropDownMenu.find('.a4dn-table').first();
                        var guid = $table.data('a4dn-guid');

                        // TODO: This binds onto a search entity in the controller, which prevents searching across multiple properties.
                        // It would be a nice enhancement to allow settings.searchMember to be templated like settings.displayMember, 
                        // and to do an OR search across properties.
                        dbSearch({ guid: guid, searchData: val, setFirstRowFocus: false, selectRowDelay: 0 });
                    };

                    // Load HTML if not already Loaded
                    // FIXME: there's a strange toggling of this flag when typing into searchbox, causing every other character to reload the full results
                    if ($dropDownMenu.data('a4dn-view-loaded') == false) {
                        loadTable($dropDownMenu, doSearch);
                    }
                    else {
                        doSearch();
                    }
                }
                // Blank out mapping targets if target is not the dropdown input
                for (let i = 0; i < settings.keyMappings.length; i++) {
                    let $target = $form.find("[name='" + settings.keyMappings[i].Target + "']");

                    if ($element == $target) {
                        $target.val("");
                    }
                }

                for (let i = 0; i < settings.additionalMappings.length; i++) {
                    let $target = $form.find("[name='" + settings.additionalMappings[i].Target + "']");

                    if ($element == $target) {
                        $target.val("");
                    }
                }

                currentSelectedUniqueKey = "";
                $dropDownOpenButton.prop('disabled', true);

                if (dontopen !== true)
                    showDropdownMenu();
            });

            $element.on("filteredSearch", function (e, searchData) {
                setTextboxState('updated');

                // Mostly copied from 'input' handler above, except for the searchData
                $element.valid();
                $dropDownMenu.css("min-width", $element.outerWidth(true));

                loadTable($dropDownMenu, function () {
                    let $table = $dropDownMenu.find('.a4dn-table').first(),
                        guid = $table.data('a4dn-guid');

                    dbSearch({ guid: guid, searchData: searchData, setFirstRowFocus: false, selectRowDelay: 0 });

                    var $form = a4dn.core.mvc.am_Get$Form(moduleGuid);

                    for (let i = 0; i < settings.keyMappings.length; i++) {
                        let $target = $form.find("[name='" + settings.keyMappings[i].Target + "']");
                        if ($element == $target) {
                            $target.val("");
                        }
                    }

                    for (let i = 0; i < settings.additionalMappings.length; i++) {
                        let $target = $form.find("[name='" + settings.additionalMappings[i].Target + "']");
                        if ($element == $target) {
                            $target.val("");
                        }
                    }

                    currentSelectedUniqueKey = "";
                    $dropDownOpenButton.prop('disabled', true);

                    showDropdownMenu();
                });
            });


            $dropDownButton.on("click", function (e) {

                if ($dropDownMenu.css('display') != 'none') {
                    hideDropdownMenu();
                    $element.focus();

                    return;
                }

                // Reset validation error which is coming from parent keys
                $element.valid();

                // Set DropDown Menu Width
                $dropDownMenu.css("min-width", $element.outerWidth(true));

                if ($dropDownMenu.data('a4dn-view-loaded') == false) {
                    loadTable($dropDownMenu);
                }

                showDropdownMenu();
            });

            $dropDownNewButton.on("click", function (e) {

                var cmdID = $dropDownNewButton.data("a4dn-command-id");
                // Open Module Explorer Tab in New
                a4dn.core.mvc.am_CreateModuleExplorerDetailTab({ url: settings.detailViewURL + "&pCmdID=" + cmdID, moduleTitle: settings.moduleTitle, moduleImageSource: settings.moduleImageSource });

            });

            $dropDownOpenButton.on("click", function (e) {

                var cmdID = $dropDownOpenButton.data("a4dn-command-id");
                // Open Module Explorer Tab in Open

                a4dn.core.mvc.am_CreateOrActivateModuleExplorerDetailTab({ applicationNumber: settings.applicationNumber, moduleNumber: settings.moduleNumber, uniqueKey: currentSelectedUniqueKey, url: settings.detailViewURL + "&pCmdID=" + cmdID + "&pUKy=" + currentSelectedUniqueKey, moduleTitle: settings.moduleTitle, moduleImageSource: settings.moduleImageSource });

            });

            $dropDownSearchButton.on("click", function (e) {

                $dropDownButton.trigger("click");

                if ($dropDownMenu.data('a4dn-view-loaded') == true) {

                    showSearch();
                }
                else {
                    showSearchAfterLoad = true;
                }
            });
        };
    };

    // Expose plugin
    $.fn.AB_Dropdown = function (options) {
        return $.fn.encapsulatedPlugin('AB_Dropdown', AB_Dropdown, this, options);
    };
})(jQuery);