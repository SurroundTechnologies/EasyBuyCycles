//var parentURL = "http://localhost:56110";
//var parentURL = "http://localhost:50691";
//var parentURL = "http://services.surroundtech.com";
var parentURL = "";
var callBackCommand = "";

$(function () {
    postMessage("SessionLoaded");

    $(document).ajaxComplete(function () {
        if (typeof callBackCommand !== "undefined" && callBackCommand != "") {
            postMessage(callBackCommand + "_Completed");
        }
        callBackCommand = "";
    });
})

function postMessage(commandID) {
    var $TopScreen = $('#TopScreen');
    var $FunctionKeyPanel = $('#FunctionKeyPanel');
    var $StatusBarText = $('#StatusBarText');

    parent.postMessage({ commandID: commandID, href: window.location.href, formname: $TopScreen.data("form-name"), formtitle: $TopScreen.data("title"), formid: $TopScreen.data("formid"), functionkeys: $FunctionKeyPanel.html(), StatusBarText: $StatusBarText.text() }, parentURL);

    // Detect Change for input
    $("input.look-entryfield").on('input', function () {
        parent.postMessage({ commandID: "ControlChanged" }, parentURL);
    });
}

function receiveMessage(e) {
    // Do we trust the sender of this message?
    if (e.origin !== parentURL)
        return;

    switch (e.data.commandID) {
        case "AddEmbeddedClass":
            $('html').addClass("a4dn-newlook-embedded-session");
            break;

        case "SendKey":
            callBackCommand = e.data.commandID;
            var form = document.forms["SessionForm"]; form.Action.value = "{" + e.data.key + "}"; $(form).submit();
            break;

        case "SetOption":
            callBackCommand = e.data.commandID;
            $('tr').filter('[data-row="' + e.data.row + '"]').find(' > td > input').first().val(e.data.Option);
            var form = document.forms["SessionForm"]; form.Action.value = "{Enter}"; $(form).submit();
            break;

        case "CloseSession":
            $("#CloseSessionButton").click();
            break;

        case "RestartSessionButton":
            $("#RestartSessionButton").click();
            break;
    }
}
window.addEventListener("message", receiveMessage, false);