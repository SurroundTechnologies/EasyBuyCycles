a4dn.core.mvc.am_InitializeDataTable = function (options) {
    var callBackNamespace = options.callBackNamespace;

    let getCallbackInfo = function (funcName) {
        return options.ajax.replace("/Search", "") + " Module #" + options.moduleNumber + " " + funcName + ": ";
    };

    if (typeof callBackNamespace == 'object' && typeof callBackNamespace.setOptions == 'function') {
        try {
            callBackNamespace.setOptions(options);
        }
        catch (error) {
            a4dn.core.mvc.am_Notification("smallBox", "Error", "Exception in " + getCallbackInfo("setOptions") + error);
            console.error("setOptions", error, options);
        }
    }

    var guid = options.guid;
    var parentGuid = options.parentGuid;
    var tableId = options.tableId;
    var systemNumber = options.systemNumber;
    var applicationNumber = options.applicationNumber;
    var moduleNumber = options.moduleNumber;
    var createdRowCallBackFunction = options.createdRowCallBackFunction;
    var createdCellCallBackFunction = options.createdCellCallBackFunction;
    var renderedCellCallBackFunction = options.renderedCellCallBackFunction;
    var columns = options.columns;
    var order = options.order;
    var ajax = options.ajax;
    var lengthMenu = options.options;
    var pageLength = options.pageLength;
    var deferLoading = options.deferLoading;
    var sDom = options.sDom;
    var autoWidth = options.autoWidth;
    var languageURL = options.languageURL;
    var viewName = options.viewName;
    var filterKeys = options.filterKeys;
    var initialSearchData = options.searchData;

    var columnDefs = [{
        "targets": '_all',
        "createdCell": function (td, cellData, rowData, row, col) {
            if (typeof createdCellCallBackFunction == 'function') {
                createdCellCallBackFunction(td, cellData, rowData, row, col);
            }

            if (typeof callBackNamespace == 'object' && typeof callBackNamespace.createdCell == 'function') {
                callBackNamespace.createdCell(guid, td, cellData, rowData, row, col);
            }
        },
        "render": function (data, type, full, meta) {
            if (typeof renderedCellCallBackFunction == 'function') {
                data = renderedCellCallBackFunction(data, type, full, meta);
            }

            if (typeof callBackNamespace == 'object' && typeof callBackNamespace.renderedCell == 'function') {
                data = callBackNamespace.renderedCell(data, type, full, meta);
            }

            return data;
        }
    }];
    if (options.columnDefs !== undefined)
        columnDefs = columnDefs.concat(options.columnDefs);

    if (typeof deferLoading === "undefined" || deferLoading == "False") {
        deferLoading = null;

        if (parentGuid.length > 0) {
            // If child Module and Parent has datatable with no data - then deferloading
            var $parentTable = $("#a4dn-table-" + parentGuid);
            if ($parentTable.length) {
                var parentTable = $("#a4dn-table-" + parentGuid).DataTable();

                if (parentTable.data().length === 0) {
                    deferLoading = 0;
                }
            }
        }
    }

    if (typeof lengthMenu === "undefined") {
        lengthMenu = [[25, 50, 100, 500, 1000, 2000, 5000, -1], [25, 50, 100, 500, 1000, 2000, 5000, "All"]];
        //lengthMenu = [[25, 50, 100, 500, 1000], [25, 50, 100, 500, 1000]];    // use the first inner array as the page length values and the second inner array as the displayed options
    }

    if (typeof sDom === "undefined") {
        sDom = "<' col-xs-12 a4dn-toolbar dt-toolbar '<'a4dn-toolbar-tray'<'a4dn-toolbar-filter pull-right'f>>>" +
                   "t" +
                     "<' col-xs-12 a4dn-toolbar-footer dt-toolbar-footer'<'a4dn-toolbar-footer-tray a4dn-phone-hidden col-xs-2 '><'col-xs-10 a4dn-phone-hidden' p l <'pull-right margin-right-10' i>>  '<'a4dn-toolbar-footer-tray a4dn-phone-show col-xs-3 '><'col-xs-9 a4dn-paging-controls a4dn-phone-show'p l><'col-xs-12 a4dn-phone-show' <'pull-right' i>> >";
    }

    if (typeof autoWidth === "undefined") {
        autoWidth = false;
    }

    var $table = $("#" + tableId);

    $("#" + tableId)
  .on("preXhr.dt", function (e, settings, data) {
      // Ajax Call started
      var $module = a4dn.core.mvc.am_Get$Module(guid);
      var $table = $("#a4dn-table-" + guid);
      var table = $table.DataTable();

      // For loading module explorers, search data will be attached to $module. 
      // But for Dropdown searches, it is attached to $table (in am_Search)
      var searchData = initialSearchData || $module.data("a4dn-search") || $table.data('a4dn-search');
      initialSearchData = undefined;    // Clear after first use
      var ajax = table.ajax.url();

      // Remove Any Parameters
      if (ajax.includes("?")) {
          ajax = ajax.substring(0, ajax.indexOf('?'));
          table.ajax.url(ajax);
      }

      if (typeof searchData !== "undefined") {
         
          if (!ajax.includes("?")) {
              ajax = ajax + "?";
          }

          ajax = ajax + "&" + searchData;

          // Add Search Data
          table.ajax.url(ajax);
      }

      if (_DataTablesExcelExportAjaxRequest) {
          if (!ajax.includes("?")) {
              ajax = ajax + "?";
          }

          var url = ajax + "&pRetExcelURL=true";

          if (_DataTablesExcelExportAll) {

              url = url + "&pExcelExportAll=true";
          }

          a4dn.core.mvc.am_ajax(url, null, {
              data: data,
              type: "post",
              onSuccess: function (data) {
                  window.open(data.output.ExcelUrl);
              },
              onErrorResponse: function (data) {
                  a4dn.core.mvc.am_Notification("smallBox", "Error", data.message);
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
              },
          });
      }
      else {
          // start Loading
          $("#a4dn-module-" + guid + " .a4dn-loading").first().removeClass("hidden");
          $table.addClass("loading");

          if (typeof callBackNamespace == 'object' && typeof callBackNamespace.ajaxCallStarted == 'function') {

              // Parse url parameters into an object whose key/value pairs are the url request parameter pairs
              // eg: &Company=1 in url becomes requestParams.Company === 1

              let requestParams = settings.ajax.url.substring(settings.ajax.url.indexOf('?'))
                  .replace(/^[?&]+/, '')
                  .split('&')
                  .map(function (kv) {
                      let pair = kv.split('=');
                      if (pair.length === 1) {
                          return [pair[0], ""];
                      }
                      else {
                          pair[1] = decodeURIComponent(pair[1]);
                          return [pair[0], pair[1]];
                      }
                  })
                  .reduce(function (p, c) { p[c[0]] = c[1]; return p; }, {});

              try {
                  callBackNamespace.ajaxCallStarted(guid, e, settings, data, requestParams);
              }
              catch (error) {
                  a4dn.core.mvc.am_Notification("smallBox", "Error", "Exception in " + getCallbackInfo("ajaxCallStarted") + error);
                  console.error("ajaxCallStarted", error, e, settings, data, requestParams);
              }
          }
      }
  });

    $("#" + tableId)
    .on("xhr.dt", function (e, settings, json, xhr) {
        // Ajax Call Completed
        if (json === undefined) {
            a4dn.core.mvc.am_Notification("smallBox", "Error", "Empty response from server trying to load data");
            return;
        }

        // Somewhere in Accelerator the result code is returned as 'returnCode' instead of 'resultCode', but it's inconsistent so check for both.
        const returnCode = json.hasOwnProperty('resultCode') ? json.resultCode : json.hasOwnProperty('returnCode') ? json.returnCode : null;

        if (typeof json !== 'object' || returnCode === null) {
            a4dn.core.mvc.am_Notification("smallBox", "Error", "Bad response from server trying to load data: " + (typeof json === 'object' ? JSON.stringify(json) : json));
            console.error(json);
            return;
        }

        if (returnCode !== "OK") {
            a4dn.core.mvc.am_Notification("smallBox", "Error", json.message || json.messages);  // DW: Not sure if messages ever gets populated
        }

        //console.log(e);
        //console.log(settings);
        //console.log(json);
        //console.log(xhr);

        // End Loading
        $("#a4dn-module-" + guid + " .a4dn-loading").first().addClass("hidden");
        $table.removeClass("loading");

        // Update Count after refresh
        var parentGuid = $table.data("a4dn-parent-guid");
        var $module = a4dn.core.mvc.am_Get$Module(guid)

        if (parentGuid != "" && $module.length) {
            a4dn.core.mvc.am_UpdateRecordCountOnSubModuleTabAfterRefresh(parentGuid, a4dn.core.mvc.am_GetModuleNumber($module), json.recordsTotal);
        }

        if (typeof callBackNamespace == 'object' && typeof callBackNamespace.ajaxCallCompleted == 'function') {
            try {
                callBackNamespace.ajaxCallCompleted(guid, e, settings, json, xhr);
            }
            catch (error) {
                a4dn.core.mvc.am_Notification("smallBox", "Error", "Exception in " + getCallbackInfo("ajaxCallCompleted") + error);
                console.error("ajaxCallCompleted", error, e, settings, json, xhr);
            }
        }
    });

    $("#" + tableId).on("draw.dt", function () {
        a4dn.core.mvc.am_DataTableCompletedDraw(guid);
    });

    var responsiveHelper_dt_basic = undefined;

    var breakpointDefinition = {
        tablet: 1024,
        phone: 480
    };

    $table.dataTable({
        "sDom": sDom,
        "responsive": true,
        "autoWidth": autoWidth,
        "pagingType": "full",
        "scrollX": true,
        "scrollY": '300px',
        "scrollCollapse": false,
        "select": true,
        "deferRender": true,
        "processing": true,
        "serverSide": true,
        "info": true,
        "deferLoading": deferLoading,

        "preDrawCallback": function () {
            if (_DataTablesIgnoreAjaxRequest) {
                _DataTablesIgnoreAjaxRequest = false;

                return false;
            }

            // Initialize the responsive datatables helper once.
            if (!responsiveHelper_dt_basic) {
                responsiveHelper_dt_basic = new ResponsiveDatatablesHelper($table, breakpointDefinition);
            }

            if (typeof callBackNamespace == 'object' && typeof callBackNamespace.preDrawCallback == 'function') {
                try {
                    callBackNamespace.preDrawCallback(guid);
                }
                catch (error) {
                    a4dn.core.mvc.am_Notification("smallBox", "Error", "Exception in " + getCallbackInfo("preDrawCallback") + error);
                    console.error("preDrawCallback", error);
                }
            }
        },
        "rowCallback": function (nRow) {
            responsiveHelper_dt_basic.createExpandIcon(nRow);

            // Nowrap on row
            $("td", nRow).attr("nowrap", "nowrap");

            if (typeof callBackNamespace == 'object' && typeof callBackNamespace.rowCallback == 'function') {
                try {
                    callBackNamespace.rowCallback(guid, nRow);
                }
                catch (error) {
                    a4dn.core.mvc.am_Notification("smallBox", "Error", "Exception in " + getCallbackInfo("rowCallback") + error);
                    console.error("rowCallback", error, nRow);
                }
            }
        },
        "drawCallback": function (oSettings) {
            responsiveHelper_dt_basic.respond();

            if (typeof callBackNamespace == 'object' && typeof callBackNamespace.drawCallback == 'function') {
                try {
                    callBackNamespace.drawCallback(guid, oSettings);
                }
                catch (error) {
                    a4dn.core.mvc.am_Notification("smallBox", "Error", "Exception in " + getCallbackInfo("drawCallback") + error);
                    console.error("drawCallback", error, oSettings);
                }
            }
        },
        "createdRow": function (row, data, dataIndex) {
            if (typeof createdRowCallBackFunction == 'function') {
                createdRowCallBackFunction(row, data, dataIndex, $table);
            }

            if (typeof callBackNamespace == 'object' && typeof callBackNamespace.createdRow == 'function') {
                try {
                    callBackNamespace.createdRow(guid, row, data, dataIndex, $table);
                }
                catch (error) {
                    a4dn.core.mvc.am_Notification("smallBox", "Error", "Exception in " + getCallbackInfo("createdRow") + error);
                    console.error("createdRow", error, row, data, dataIndex, $table);
                }
            }
        },
        "footerCallback": function (tfoot, data, start, end, display) {
            if (typeof callBackNamespace == 'object' && typeof callBackNamespace.footerCallback == 'function') {
                try {
                    callBackNamespace.footerCallback(guid, tfoot, data, start, end, display);
                }
                catch (error) {
                    a4dn.core.mvc.am_Notification("smallBox", "Error", "Exception in " + getCallbackInfo("footerCallback") + error);
                    console.error("footerCallback", error, tfoot, data, start, end, display);
                }
            }
        },

        "formatNumber": function (toFormat) {
            if (typeof callBackNamespace == 'object' && typeof callBackNamespace.formatNumber == 'function') {
                try {
                    return callBackNamespace.formatNumber(guid, toFormat);
                }
                catch (error) {
                    a4dn.core.mvc.am_Notification("smallBox", "Error", "Exception in " + getCallbackInfo("formatNumber") + error);
                    console.error("formatNumber", error, toFormat);
                }
            }
            return toFormat;
        },

        "headerCallback": function (thead, data, start, end, display) {
            if (typeof callBackNamespace == 'object' && typeof callBackNamespace.headerCallback == 'function') {
                try {
                    callBackNamespace.headerCallback(guid, thead, data, start, end, display);
                }
                catch (error) {
                    a4dn.core.mvc.am_Notification("smallBox", "Error", "Exception in " + getCallbackInfo("headerCallback") + error);
                    console.error("headerCallback", error, thead, data, start, end, display);
                }
            }
        },

        "infoCallback": function (settings, start, end, max, total, pre) {
            if (typeof callBackNamespace == 'object' && typeof callBackNamespace.infoCallback == 'function') {
                try {
                    return callBackNamespace.infoCallback(guid, settings, start, end, max, total, pre);
                }
                catch (error) {
                    a4dn.core.mvc.am_Notification("smallBox", "Error", "Exception in " + getCallbackInfo("infoCallback") + error);
                    console.error("infoCallback", error, settings, start, end, max, total, pre);
                }
            }

            return pre;
        },

        "initComplete": function (settings, json) {
            // Move Toolbar above datatable - The datatable may refresh if language URL is changed and toolbar is moved to the bottom
            a4dn.core.mvc.am_MoveModuleExplorerToolbar(guid);

            if (typeof callBackNamespace == 'object' && typeof callBackNamespace.initComplete == 'function') {
                try {
                    callBackNamespace.initComplete(guid, settings, json);
                }
                catch (error) {
                    a4dn.core.mvc.am_Notification("smallBox", "Error", "Exception in " + getCallbackInfo("initComplete") + error);
                    console.error("initComplete", error, settings, json);
                }
            }
        },
        "lengthMenu": lengthMenu,
        "pageLength": pageLength,

        "ajax": {
            "url": ajax,
            "type": "POST",
            "beforeSend": function (request) {
                if (_DataTablesExcelExportAjaxRequest) {
                    _DataTablesExcelExportAjaxRequest = false;
                    // Abort current Ajax Request so that Datatable does not reload
                    request.abort();
                }
            },
            "data": function (d) {
                var table = $table.DataTable();
                var info = table.page.info();

                var searchMethod = "InitialSearch";

                if (d.start != 0) {
                    var lastStart = $table.data("a4dn-laststart");
                    if ((d.start + d.length) >= info.recordsTotal) {
                        searchMethod = "LastSet";
                    }

                    else if (d.start > lastStart) {
                        searchMethod = "NextSet";
                        d.pUKy = table.cell(":last", "A4DN_UniqueKey:name").data();
                    }
                    else if (d.start < lastStart) {
                        searchMethod = "PreviousSet";
                        d.pUKy = table.cell(":first", "A4DN_UniqueKey:name").data();
                    }
                }

                $table.data("a4dn-laststart", d.start);
                $table.data("a4dn-lastdraw", d.draw);

                d.pSrchMth = searchMethod;
                d.pSysNo = systemNumber;
                d.pAppNo = applicationNumber;
                d.pModNo = moduleNumber;
                d.pVwNm = viewName || $('#a4dn-view-dropdown-' + guid + ' > li > a.selected').data('a4dn-view-name');
                d.pFilterKeys = filterKeys;

                var $module = a4dn.core.mvc.am_Get$Module(guid);
                d.pLoc = $module.data('a4dn-component-location');
                d.pVwRefExt = $module.data('a4dn-view-ref-ext');
               
                // If Table is a Sub Module - Has Parent Guid
                if ($table.data("a4dn-parent-guid") != "") {
                    // Get parent Keys
                    var $p_table = $("#a4dn-table-" + $table.data("a4dn-parent-guid"));

                    if ($p_table.length) {
                        // Table
                        var $p_tbody = $p_table.find("tbody");
                        var $p_row = $p_tbody.find('.highlight');
                        d.pPKeys = a4dn.core.mvc.am_GetBroadcastKeys("OnSearch", $p_table, $p_row);
                       
                        // Set Module Explorer title
                        var p_table = $p_table.DataTable();
                        a4dn.core.mvc.am_SetModuleExplorerTitle(guid, p_table.cell($p_row, "A4DN_SubTitle:name").data());
                    }
                    else {
                        //Form
                        var $form = $("#a4dn-form-" + $table.data("a4dn-parent-guid"));

                        if ($form.length == 0) {
                            //Could be presenter - look for guid
                            $form = $("#" + $table.data("a4dn-parent-guid"));
                        }

                        if ($form.length) {
                            d.pPKeys = a4dn.core.mvc.am_GetDetailBroadcastKeys("OnSearch", $form);
                            a4dn.core.mvc.am_SetModuleExplorerTitle(guid, $form.data("a4dn-sub-title"));
                        }
                    }
                }
            },
        },

        "columns": columns,
        "order": order,
        "columnDefs": columnDefs,

        //   "searchDelay": 5000,
        //   "stateSave": true,  //restore table state on page reload,

        "language": {
            url: languageURL,
            search: '<span class="input-group-addon"><i class="fa fa-filter"></i></span>',
            info: '<span class="txt-color-darken">_START_</span> to <span class="txt-color-darken">_END_</span> of <span class="text-primary">_TOTAL_</span>',
            infoFiltered: '(filtered from <span class="txt-color-darken">_MAX_</span>)',
            paginate: {
                first: '<i class="fa fa-chevron-left" aria-hidden="true"></i><i class="fa fa-chevron-left" aria-hidden="true"></i>',
                previous: '<i class="fa fa-chevron-left" aria-hidden="true"></i>',
                next: '<i class="fa fa-chevron-right" aria-hidden="true"></i>',
                last: '<i class="fa fa-chevron-right" aria-hidden="true"></i><i class="fa fa-chevron-right" aria-hidden="true"></i>'
            },
            lengthMenu: "_MENU_",
        },
    });

    // Add Filter placeholder text
    var $filter = $("#" + tableId + "_filter");
    $filter.find("input").attr("placeholder", "Filter Results");

    // AutoScroll
    //$("#" + tableID + '_wrapper .dataTables_scrollBody').scroll(function () {
    //   if ($(this)[0].scrollHeight - $(this).scrollTop() <= $(this).height()) {
    //       $("#" + tableID + "_next").trigger("click");

    //       $(this).scrollTop(0);

    //    }

    //});

    $table.find("tbody").off('click') // remove handler

    $table.find("tbody").on('click', 'tr', function (e) {
        // If click targets a hyperlink or a hyperlink's children, don't process it as a row selection
        let $target = $(e.target);
        if ($target.is("a") || $target.parents("a").length > 0)
            return;

        // Don't intercept clicks inside open x-editable controls
        if ($(e.target).parents(".editable-buttons").length > 0)
            return;

        e.stopPropagation();

        var $this = $(this);

        a4dn.core.mvc.am_SetTableFocus($table);

        if ($this.hasClass('clicked')) {
            $this.removeClass('clicked');

            //here is your code for double click
            let $moduleExplorer = $(this).closest(".a4dn-module-explorer"),
                guid = $moduleExplorer.data('a4dn-guid'),
                $toolbar = $("#a4dn-toolbar-commands-" + guid),
                defaultCommand = $toolbar.data('a4dn-dbl-click-default'),
                $cmdBtn;

            if (defaultCommand) {
                $cmdBtn = $toolbar.find(".a4dn-command[data-a4dn-command-id='" + defaultCommand + "']");
            }

            if (!$cmdBtn || !$cmdBtn.length > 0) {
                $cmdBtn = $toolbar.find(".a4dn-command[data-a4dn-command-id='OPEN']");
            }

            if (!$cmdBtn || !$cmdBtn.length > 0) {
                $cmdBtn = $toolbar.find(".a4dn-command[data-a4dn-command-id='DISPLAY']");
            }

            if ($cmdBtn && $cmdBtn.length > 0) {
                $cmdBtn.eq(0).trigger('click');
            }
        } else {
            $this.addClass('clicked');

            if (!$this.hasClass("highlight")) {
                // Select row if it is currently not focused
                var $tbody = $("#" + tableId + " tbody");
                a4dn.core.mvc.am_SelectRow($table, $tbody, $(this));
            }
            // Accept double-click within 500ms
            setTimeout(function () {
                $('.clicked').removeClass('clicked');
            }, 500);
        }
    });

    $table.find("tbody").on('click', 'input[type="checkbox"]', function (e) {

        var $this = $(this);
        var $row = $this.closest('tr');

        var table = $table.DataTable();
        data = table.row($row).data();
        data[$this.attr("name")] = $this.prop("checked");
        //console.log(data);

        var commandID = $this.data("a4dn-process-request-command-id");

        if (commandID != null && commandID != "")  {
            
            a4dn.core.mvc.am_DataGridAjaxProcessRequestPostRequest(guid, $table, $row, commandID);
        }
       
        
        // Prevent click event from propagating to parent
        e.stopPropagation();
    });

};

a4dn.core.mvc.am_ReloadDataTable = function (options) {
    let guid = options.guid,
        selectedItemPriority1 = options.selectedItemPriority1,
        selectedItemPriority2 = options.selectedItemPriority2,
        $table = a4dn.core.mvc.am_Get$Table(guid),
        table = $table.DataTable(),
        $focusRow = a4dn.core.mvc.am_GetFocusRow($table),
        scrollTop = 0;

    if (selectedItemPriority2 === undefined && $focusRow.length) {
        selectedItemPriority2 = table.cell($focusRow, "A4DN_UniqueKey:name").data();
        let $scrollable = $focusRow.parents(".dataTables_scrollBody");
        if ($scrollable.length) {
            scrollTop = $scrollable.scrollTop();
        }
    }

    $table.removeData('a4dn-sel-item-priority-1');
    $table.removeData('a4dn-sel-item-priority-2');
    $table.removeData('a4dn-sel-item-scrolltop');

    if (typeof selectedItemPriority1 !== "undefined") {
        $table.data('a4dn-sel-item-priority-1', selectedItemPriority1);
    }
    if (typeof selectedItemPriority2 !== "undefined") {
        $table.data('a4dn-sel-item-priority-2', selectedItemPriority2);
        $table.data('a4dn-sel-item-scrolltop', scrollTop);
    }

    table.ajax.reload();
};

a4dn.core.mvc.am_Search = function (options) {
    var guid = options.guid;
    var searchData = options.searchData;
    var setFirstRowFocus = options.setFirstRowFocus;
    var selectRowDelay = options.selectRowDelay;

    if (typeof setFirstRowFocus === "undefined") {
        setFirstRowFocus = true;
    }

    var $table = a4dn.core.mvc.am_Get$Table(guid);
    var $module = a4dn.core.mvc.am_Get$Module(guid);

    $module.data("a4dn-search", searchData);
    $table.data("a4dn-search", searchData);
    $table.data("a4dn-set-first-row-focus", setFirstRowFocus);
    $table.data("a4dn-select-row-delay", selectRowDelay);

    var table = $table.DataTable();

    table.ajax.reload();
}

a4dn.core.mvc.am_DataTableCompletedDraw = function (guid) {
    var $module = a4dn.core.mvc.am_Get$Module(guid);
    var $table = a4dn.core.mvc.am_Get$Table(guid);
    var table = $table.DataTable();

    if (table.data().length == 0) {
        $table.trigger("zeroRecordsSelected");
        a4dn.core.mvc.am_ZeroSelectedCommandState(guid);
    }
    else {
        let selPriority1 = $table.data('a4dn-sel-item-priority-1'),
            selPriority2 = $table.data('a4dn-sel-item-priority-2'),
            scrollTop = $table.data('a4dn-sel-item-scrolltop'),
            setFirstRowFocus = $table.data("a4dn-set-first-row-focus");

        var selRow = false;

        if (typeof selPriority1 !== "undefined") {
            selRow = a4dn.core.mvc.am_TrySelectRowWithUniqueKey(guid, selPriority1, scrollTop);
        }
        else if (typeof selPriority2 !== "undefined") {
            if (!selRow) {
                selRow = a4dn.core.mvc.am_TrySelectRowWithUniqueKey(guid, selPriority2, scrollTop);
            }
        }

        if (!selRow && (setFirstRowFocus != false && setFirstRowFocus != "False")) {
            // Default - Select first row
            a4dn.core.mvc.am_SelectFirstRow(guid);

            //if ($table.data("a4dn-lastdraw") == 1) {
            //    setTimeout(function () { a4dn.core.mvc.am_SelectFirstRow(guid); }, 1000);

            //    //Trigger Count - This is needed because on first load the counts are not showing
            //    var $table = a4dn.core.mvc.am_Get$Table(guid);
            //    var $row = a4dn.core.mvc.am_GetFocusRow($table);
            //    $table.trigger("itemSelectedCount", [a4dn.core.mvc.am_GetBroadcastKeys("OnSearch", $table, $row)]);
            //}
        }
    }

    var searchData = $module.data("a4dn-search");

    if (typeof searchData !== "undefined") {
        if (a4dn.core.mvc.am_QueryHasSetValues(searchData)) {
            // Search Applied

            $("#a4dn-table-" + guid + "_wrapper .dataTables_info").append('<a class="a4dn-text-search-clear btn btn-primary btn-xs margin-left-5 margin-bottom-5"><i class="fa fa-times margin-right-5" aria-hidden="true"></i>Clear Search</a>');

            $("#a4dn-table-" + guid + "_wrapper .dataTables_info a.a4dn-text-search-clear").click(function (e) {
                e.preventDefault();
                $module.removeData("a4dn-search");
                $table.removeData("a4dn-search");
                // Remove Everthing after the ?
                var s = table.ajax.url();
                s = s.substring(0, s.indexOf('?'));
                table.ajax.url(s);

                a4dn.core.mvc.am_Search({ guid: guid });
            });
        }
    }

};

a4dn.core.mvc.am_DestroyDataTable = function (guid) {
    var $table = $("#a4dn-table-" + guid);
    var table = $table.DataTable();

    table.destroy();
};

a4dn.core.mvc.am_UpdateRow = function (guid, row, data) {
    var $row = $(row.node());
    var $td = $row.find('td').first();
    var responsiveExpander = false;
    if ($row.find('td > span.responsiveExpander').length) {
        responsiveExpander = true;
    }

    row.data(data);
    _DataTablesIgnoreAjaxRequest = true;
    row.invalidate().draw();

    if (responsiveExpander) {
        $td.prepend('<span class="responsiveExpander"></span>');
    }
};

a4dn.core.mvc.am_TrySelectRowWithUniqueKey = function (guid, uniqueKey, scrollTop) {
    var $table = $("#a4dn-table-" + guid);
    var table = $table.DataTable();

    var found = false,
        $row, $tbody;

    // Find tr with unique Key
    table.rows().every(function (rowIdx, tableLoop, rowLoop) {
        if (table.cell($(this.node()), "A4DN_UniqueKey:name").data() == uniqueKey) {
            $row = $(this.node());
            $tbody = $table.find('tbody').first();
            found = true;
            return true;
        }
    });

    if (found) {
        setTimeout(function () {
            a4dn.core.mvc.am_SelectRow($table, $tbody, $row, 0, true);
            a4dn.core.mvc.am_ScrollRowIntoView(guid, $row, scrollTop);
        }, 0);
    }

    return found;
};

a4dn.core.mvc.am_ScrollRowIntoView = function (guid, $row, scrollTop) {
    let $table = a4dn.core.mvc.am_Get$Table(guid),
        rowpos = scrollTop === undefined || scrollTop === 0 ? $row.position().top : scrollTop;

    $('#a4dn-table-' + guid + '_wrapper .dataTables_scrollBody').scrollTop(rowpos);
}

a4dn.core.mvc.am_AdjustTableColumns = function (guid) {
    var table = $("#a4dn-table-" + guid).DataTable();

    // Remember Scroll Position
    var $scrollBody = $('#a4dn-table-' + guid + '_wrapper .dataTables_scrollBody');
    var scroll = $scrollBody.scrollTop();

    table.columns.adjust();

    // Reset Scroll Position
    $scrollBody.scrollTop(scroll);
};

a4dn.core.mvc.am_AddCellClass = function (columnName, className, table, $row) {
    let column = table.column(columnName + ':name'),
        idx = column.index('visible'),
        $td = idx !== null ? $row.find('td').eq(idx) : undefined;

    if ($td) {
        $td.addClass(className);
    }
};

a4dn.core.mvc.am_OverrideCellContent = function (columnName, value, table, $row) {
    let column = table.column(columnName + ':name'),
        idx = column.index('visible'),
        $td = idx !== null ? $row.find('td').eq(idx) : undefined;

    if ($td) {
        if (typeof value === 'function') {
            value($td);
        }
        else {
            $td.html(value);
        }
    }
};

// Call this method from createdRow callback:
//
// createdRow: function (guid, row, data, dataIndex, $table) {
//     a4dn.core.mvc.am_ReplaceBooleanWithCheckbox('Status', $table.DataTable(), $(row), data.Status, true);
// }
//
a4dn.core.mvc.am_ReplaceBooleanWithCheckbox = function (columnName, table, $row, isChecked, isReadOnly) {
    let readonlyClass = isReadOnly ? 'readonly a4dn-readonly' : '',
        readonlyAttr = isReadOnly ? 'disabled readonly' : '',
        checkedAttr = isChecked ? 'checked' : '';

    // TODO: checkbox variant that fires a ProcessRequest command with current checkbox state

    a4dn.core.mvc.am_OverrideCellContent(
        columnName,
        $('<div><div class="smart-form"><label class="checkbox text-center ' + readonlyClass
            + '"><input type="checkbox" class="a4dn-field-control check-box' + readonlyClass
            + '" name="' + columnName
            + '" ' + readonlyAttr
            + ' ' + checkedAttr
            + ' /><i></i></div></div>'),
        table,
        $row
    );
};


// See https://vitalets.github.io/x-editable/docs.html
// columnName: used to find cell in row, passed as 'name' parameter to url
// title: used for title in edit dialog
// dataType: can be text, textarea, select, date, checklist; see docs.html
// url: callback url for update; passed params name, pk, value
//
// Example:
//
//    createdRow: function (guid, row, data, dataIndex, $table) {
//        let $module = a4dn.core.mvc.am_Get$Module(guid),
//            url = $module.data("a4dn-module-controller-base-url").replace("/Index", "/UpdateAttribute");
//
//        a4dn.core.mvc.am_MakeCellXEditable('Attribute', 'Edit Attribute', 'textarea', url, data, $table, $(row));
//    }
//
// C# handler:
//
//    [AB_AjaxOnly]
//    [HttpPost]
//    public JsonResult UpdateAttribute(string name, string pk, string value)
//    {
//        var fetchRetArgs = this.FetchEntityWithEncodedUniqueKey<ItemAttributesVM, ItemAttributesEntity>(ap_ViewModel as ItemAttributesVM, pk);
//        if (!fetchRetArgs.ap_IsSuccess)
//            return this.AB_JsonErrorResult(string.Join("\n", fetchRetArgs.am_MessageList()));
//            
//        var entity = fetchRetArgs.ap_OutputEntity as ItemAttributesEntity;
//        if (entity == null)
//            return this.AB_JsonErrorResult("Attribute record not found");
//
//        entity.Attribute = value;
//
//        var updRetArgs = ap_ViewModel.am_Update(new AB_UpdateInputArgs<ItemAttributesEntity>(entity));
//        if (!updRetArgs.ap_IsSuccess)
//            return this.AB_JsonErrorResult(string.Join("\n", updRetArgs.am_MessageList()));
//
//        // Success
//        return this.AB_JsonResult();
//    }
a4dn.core.mvc.am_MakeCellXEditable = function (columnName, title, dataType, url, rowData, $table, $row, options) {
    let table = $table.DataTable(),
        opt = $.extend({
            mode: 'inline',
            params: undefined,
            savenochange: false,
            onSuccess: undefined,
            value: undefined,
            tpl: undefined,
            container: undefined
        }, options);

    a4dn.core.mvc.am_OverrideCellContent(columnName, function ($td) {
        let markup = $td.html();
        $td.html("<a href='#' data-name='" + columnName + "'"
            + " data-type='" + dataType + "'"
            + " data-pk='" + rowData.A4DN_UniqueKey + "'"
            + " data-url='" + url + "'"
            + " data-title='" + title + "'"
            + " data-inputclass='form-control'"
            + ">" + markup + "</a>");
    }, table, $row);

    $row.find("[data-name='" + columnName + "']").editable({
        mode: opt.mode,
        params: opt.params,
        savenochange: opt.savenochange,
        value: opt.value,
        tpl: opt.tpl,
        container: opt.container,
        success: function (data, newValue) {
            if (data.resultCode === "ER") {
                return data.message;
            }
            if (opt.onSuccess) {
                return opt.onSuccess(data, newValue);
            }
        }
    }).on('hidden', function (e, reason) {
        table.columns.adjust();
    });
};

