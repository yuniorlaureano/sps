var table = [];

$(document).ready(function () {

    getPivotReport();

    $('#btnExportReport').click(function (e) {
        e.preventDefault();
        exportBillingReport();
    });

    $('#ddlCanvassEdition').change(function () {

        getPivotReport();
    });

    $('#weekRange').change(function () {

        getPivotReport();
    });

})


function getPivotReport() {

    var weekRange = $('#weekRange').val().split('-');

    var $el = showLoading($("#demo-panel-ref"));

    $.get($('#tbLogReportPanel').data("url"), { "canvEdition": $('#ddlCanvassEdition').val(), "startWeek": weekRange[0].trim(), "endWeek": weekRange[1].trim() }, function (result) {

        if (!$.fn.DataTable.isDataTable('#tbLogReportPanel')) {

            table = $('#tbLogReportPanel').DataTable({
                data: result,
                "columns": [
                         { "data": "CANV_WEEK" },
                          { "data": "CANV_CODE" },
                          { "data": "PROD_GRP_CODE" },
                          { "data": "PROD_CO" },
                          { "data": "PROD_CI" },
                           { "data": "TOT_COM" },
                          { "data": "RENEW_CI" },
                          { "data": "INCREASE_CI" },
                          { "data": "DECREASE_CI" },
                          { "data": "NP_CI" },
                          { "data": "OP_CI" },
                          { "data": "OTC_CI" },
                          { "data": "CTRL_LOSS" },
                          { "data": "UNCTRL_LOSS" },
                          { "data": "NP_CNT" },
                          { "data": "OP_CNT" },
                           { "data": "OTC_NP_CNT" },
                          { "data": "CTRL_CNT" }
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

function exportBillingReport() {

    var weekRange = $('#weekRange').val().split('-');
    
    var $el = showLoading($("#demo-panel-ref"));

    $.ajax({
        url: $('#btnExportReport').data("url"),
        type: 'post',
        dataType: 'json',
        contentType: "application/json",

          xhr: function () {
            var xhr = new window.XMLHttpRequest();
            xhr.upload.addEventListener("progress", function (evt) {
                if (evt.lengthComputable) {
                    var percentComplete = evt.loaded / evt.total * 100;
                    console.log("percentComplete = " + percentComplete);
                } else {
                    console.log("lengthComputable evaluated to false;")
                }
            }, false);

            xhr.addEventListener("progress", function (evt) {
                if (evt.lengthComputable) {
                    var percentComplete = evt.loaded / evt.total * 100;
                    console.log("percentComplete = " + percentComplete);
                } else {
                    console.log("lengthComputable evaluated to false;")
                }
            }, false);

            return xhr;
        },
        success: function (data) {
            data.file.map(function (file) {
                window.open(file);
                hideLoading($el);
            });
        },
        error: function (ex) {
            hideLoading($el);
        },
        data: JSON.stringify({ "canvEdition": $('#ddlCanvassEdition').val(), "startWeek": weekRange[0].trim(), "endWeek": weekRange[1].trim() })


    });
}