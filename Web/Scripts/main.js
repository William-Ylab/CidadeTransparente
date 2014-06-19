var LOAD_GIF_IMAGE = "../Design/Images/loading.gif"
var LOADING_HTML = "<div class='loading_view'><div class='overlay'>&nbsp;</div><div class='wrapper'><img src='" + LOAD_GIF_IMAGE +
    "'/><br/><span id='loadingtext'>Carregando...</span></div></div>";

var DATATABLE_SETTINGS = {
    bDestroy: true,
    bRetrieve: true,
    sDom: "<'pull-right'l>t<'row-fluid'<'span12'fp>>",
    sPaginationType: "bootstrap",
    oLanguage: {
        "sProcessing": "",
        "sLengthMenu": "Mostrar _MENU_ registro(s)",
        "sZeroRecords": "Nenhum registro encontrado",
        "sInfo": "_START_ até _END_ de _TOTAL_",
        "sInfoEmpty": "0 até 0",
        "sInfoFiltered": "(filtrado de _MAX_)",
        "sInfoPostFix": "",
        "sSearch": "Pesquisar: ",
        "sUrl": "",
        "oPaginate": {
            "sFirst": "Primeira",
            "sPrevious": "Anterior",
            "sNext": "Próxima",
            "sLast": "Última"
        }
    }
};

function showAlert(message, alertType) {
    showAlert(message, alertType, 2000);
}

function showAlert(message, alertType, fadeMs) {

    if (fadeMs == undefined)
        fadeMs = 2000;

    $("#master_alert").removeClass("alert-success").removeClass("alert-error").removeClass("alert-info");

    switch (alertType) {
        case "success":
            $("#master_alert").addClass("alert-success");
            break;
        case "error":
            $("#master_alert").addClass("alert-error");
            break;
        case "info":
        case "warning":
            $("#master_alert").addClass("alert-info");
            break;
        default:
    }

    $("#master_alert span").html(message);
    $("#master_alert").fadeIn('slow');

    setTimeout('hideAlert()', fadeMs);
}

function hideAlert() {
    $("#master_alert").fadeOut();
}

function setLoadingToDiv(obj) {
    removeLoadingToDiv(obj);
    $(obj).append(LOADING_HTML);
}

function setLoadingToDiv(obj, text) {

    if (text == "" || text == undefined) {
        text = "Aguarde...";
    }

    removeLoadingToDiv(obj);
    $(obj).append(LOADING_HTML.replace("Carregando...", text));
}

function removeLoadingToDiv(obj) {
    $(obj).find(".loading_view").each(function () {
        var obj = this;
        $(this).fadeOut(function () { $(this).remove() });

    });
}

function getValueByQueryString(param) {
    var querystr = new Array();
    loc = window.location.search.substr(1).split('&');

    if ((loc != '') && (loc != null)) {
        for (var icnt = 0; icnt < loc.length; icnt++) {
            var q = loc[icnt].split('=');
            querystr[q[0]] = q[1];
        }
        return decodeURIComponent(querystr[param]);
    } else {
        return (null);
    }
}

function getValueByQueryStringOnUrlEncode(param) {
    var querystr = new Array();
    loc = decodeURIComponent(window.location.search.substr(1)).split('&');

    if ((loc != '') && (loc != null)) {
        for (var icnt = 0; icnt < loc.length; icnt++) {
            var q = loc[icnt].split('=');
            querystr[q[0]] = q[1];
        }
        return querystr[param];
    } else {
        return (null);
    }
}

function removeAccents(strAccentsVar) {
    if (strAccentsVar == null)
        return null;

    var strAccents = strAccentsVar.split('');
    var strAccentsOut = new Array();
    var strAccentsLen = strAccents.length;
    var accents = 'ÀÁÂÃÄÅàáâãäåÒÓÔÕÕÖØòóôõöøÈÉÊËèéêëðÇçÐÌÍÎÏìíîïÙÚÛÜùúûüÑñŠšŸÿýŽž';
    var accentsOut = "AAAAAAaaaaaaOOOOOOOooooooEEEEeeeeeCcDIIIIiiiiUUUUuuuuNnSsYyyZz";
    for (var y = 0; y < strAccentsLen; y++) {
        if (accents.indexOf(strAccents[y]) != -1) {
            strAccentsOut[y] = accentsOut.substr(accents.indexOf(strAccents[y]), 1);
        } else
            strAccentsOut[y] = strAccents[y];
    }
    strAccentsOut = strAccentsOut.join('');
    return strAccentsOut;
}

function convertStringToFloat(value) {
    var ret = 0;
    try {
        if (typeof value === 'number') {
            ret = parseFloat(value);
        } else {

            var arr = "";
            if (value.indexOf('.') > -1 && value.indexOf(',') > -1) {
                arr = value.replace(".", "").replace(",", "."); //1.222,00 -> 1222.00
            } else if (value.indexOf(',') > -1) {
                arr = value.replace(",", "."); //1222,00 -> 1222.0
            } else {
                arr = value;
            }
            ret = parseFloat(arr);
        }
    } catch (err) {
        alert(err.message + " - " + Object.prototype.toString.call(value));
    }

    ret = Math.round((ret) * 100) / 100;

    return ret;
}

function htmlEncode(value) {
    //create a in-memory div, set it's inner text(which jQuery automatically encodes)
    //then grab the encoded contents back out.  The div never exists on the page.
    return $('<div/>').text(value).html();
}

function htmlDecode(value) {
    return $('<div/>').html(value).text();
}

function dataTableBuild(instance) {

    if (instance != null) {
        var tables = $.fn.DataTable.fnTables(true);

        if (tables.length > 0) {
            for (var i in tables) {
                if ($(tables[i]).attr('id') == (instance).attr('id')) {
                    $(tables[i]).dataTable().fnDestroy();
                }
            }
        }
    }

    return instance.dataTable();


}

function dataTableDraw(instance) {
    $(instance).fnClearTable();
    $(instance).fnDestroy();
}