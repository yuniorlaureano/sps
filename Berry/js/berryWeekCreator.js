var table = [];

$(document).ready(function () {

    setCanvass();
    setCanvassEdition();

    $('#btnCreateWeek').click(function (e) {
        e.preventDefault();
        bootbox.confirm("Se crearan las semanas hasta la fecha seleccionada, ¿está seguro que desea realizar esta acción?", function (result) {
            if (result) {
                createWeek();
            }
        });
    });
 

})

//Set Info
function setCanvass() {

    showColorLoading();

    $.get($("#ddlCanvCode").data('url'), function (canvList) {

        $('#ddlCanvCode')
        .find('option')
        .remove()
        .end();

        $(canvList).each(function (index, value) {
            var $option = $("<option/>").attr("value", value.CANV_CODE).text(value.CANV_CODE);
            $('#ddlCanvCode').append($option);
        });

        $('#ddlCanvCode')
              .selectpicker('refresh');

        hideColorLoading();
    });
}

function setCanvassEdition() {

    showColorLoading();

    $.get($("#ddlCanvassEdition").data('url'), function (canvEditionList) {

        $('#ddlCanvassEdition')
        .find('option')
        .remove()
        .end();

        $(canvEditionList).each(function (index, value) {
            var $option = $("<option/>").attr("value", value.PROD_CYCLE).text(value.PROD_CYCLE);
            $('#ddlCanvassEdition').append($option);
        });

        $('#ddlCanvassEdition')
              .selectpicker('refresh');

        hideColorLoading();
    });
}

function getActiveWeek() {

    var weekRange = $('#weekRange').val().split('-');

    var $el = showLoading($("#logReportPanel-body"));

    $.get($('#tbLogReportPanel').data("url"), { "canvEdition": $('#ddlCanvassEdition').val(), "startWeek": weekRange[0].trim(), "endWeek": weekRange[1].trim() }, function (result) {

        if (!$.fn.DataTable.isDataTable('#tbLogReportPanel')) {

            table = $('#tbLogReportPanel').DataTable({
                data: result,
                "columns": [
                        { "data": "CANV_CODE" },
                        { "data": "CANV_EDITION" },
                        { "data": "CANV_WEEK" }
                ],
                "deferRender": true
            });
            relocatePag_Filt('#tbLogReportPanel');
        } else {
            $('#tbLogReportPanel').dataTable().fnClearTable();

            if (result.length > 0) {
                $('#tbLogReportPanel').dataTable().fnAddData(result);
            }
        }

        hideLoading($el);
    });
}

function createWeek() {

    showColorLoading();

    $.get($('#btnCreateWeek').data("url"), { "canvEdition": $('#ddlCanvassEdition').val(), "canvCode": $('#ddlCanvCode').val(), "endDate": $('#endWeek').val() }, function (result) {

        var type = result.split('-')[0].trim() == "Error" ? 'danger' : 'success';
       
        getTimerDialogMsg(type, result, 'berryCreateWeekPanel-body', 3000);
        
        hideColorLoading();
    }).fail(function (ex) {
        getTimerDialogMsg('danger', 'No se pudo realizar la transacción, favor contactar validar los datos ingresados', 'berryCreateWeekPanel-body', 3000);
    });
}