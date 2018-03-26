//Compares SMS Dimmas
var tbSMSVsDimmas, tbDimmasVsSMS,
    tbSMSVsDimmasRej, tbStatusReport;

//UniverseCompares
var tbDimmasBerry, tbDimmasBerryAcum,
    tbDimmasNoBerry;

//childTable
var SMSVsDimmaDetail, dimmasVsSMSDetail,
    SMSVsDimmasRejDetail;


$(document).ready(function () {

    GetSMSVsDimmasReport();

    $("#dimmasVsSMS").click(function (e) {
        e.preventDefault();

        if (!$.fn.DataTable.isDataTable('#tbDimmasVsSMS')) {
            GetDimmasVsSMS();
        }
    })

    $("#SMSVsDimmasRej").click(function (e) {
        e.preventDefault();

        if (!$.fn.DataTable.isDataTable('#tbSMSVsDimmasRej')) {
            GetSMSVsDimmasRejReport();
        }
    })

    GetTotalDifCompares();
    getStatusCompareReport();


});

//#Compares SMS Dimmas
function GetSMSVsDimmasReport() {
    try {

        var $el = showLoading($("#SMSVsDimmasPanel"));

        $.get($('#tbSMSVsDimmas').data("url"), function (result) {

            if ($.fn.DataTable.isDataTable('#tbSMSVsDimmas')) {
                tbSMSVsDimmas.destroy();
            }

            tbSMSVsDimmas = $('#tbSMSVsDimmas').DataTable({
                "fnRowCallback": function (row, data, start, end, display) {

                    //Deshabilita estas columnas para que al dar click en las mismas no se abra el histórico en SMS
                    $('td:eq(0),td:eq(1),td:eq(24)', row).addClass("not-details-control ");

                    $("td input[type='checkbox']", row).change(function () {

                        if ($(this).is(':checked')) {
                            $(this).closest("tr").css('background-color', "#F5F5F5")
                        } else {
                            $(this).closest("tr").css('background-color', "#fff")
                        }

                    })
                }
            });

            tbSMSVsDimmas.clear().draw();

            relocatePag_Filt('#tbSMSVsDimmas');
            var rows = 1;
            $.each(result.Table, function (i, element) {
                tbSMSVsDimmas.row.add([
                    '<input type="checkbox" name="chkSD' + rows + '" data-move="1" data-id="' + element.ID + '"  data-sem="' + element.SEM_NUMERO + '" data-account="' + element.REP_ACCOUNT_ID + '" data-edition="' + element.CANV_EDITION + '" data-canv="' + element.CANV_CODE + '" data-seq="' + element.REP_SECUENCIA + '">',
                    '<input type="checkbox"  name="chkSD' + rows + '" data-move="2" data-id="' + element.ID + '"  data-sem="' + element.SEM_NUMERO + '" data-account="' + element.REP_ACCOUNT_ID + '" data-edition="' + element.CANV_EDITION + '" data-canv="' + element.CANV_CODE + '" data-seq="' + element.REP_SECUENCIA + '">',
                    element.SUBSCR_ID,
                    element.CANV_CODE,
                    element.CANV_EDITION,
                    formatDate(element.REP_SALES_DATE),
                    element.STF_CODIGO,
                    element.REP_UNIT,
                    element.TEAM_ID_DIMMAS,
                    formatNumber(element.CARGO_SMS),
                    formatNumber(element.VENTA_SMS),
                    element.PROGRAM_STATUS,
                    element.ACCOUNT_STATUS,
                    element.CLOSE_TYPE_SGV,
                      element.NO_DIGITADA,
                      element.DIF_EJECUTIVO,
                     element.DIF_TEAM,
                      element.DIF_CHARGE_IN,
                     element.DIF_CHARGE_OUT,
                    element.DIF_CLOSE_TYPE,
                    element.SMS_DIGITADOR,
                     element.LS,
                      element.AG,
                      setCompareStatusColor(element.STATUS),

                      ' <input class="form-control goog" type="text" id="inputWarning"  />'

                ]).draw().nodes().to$().addClass('details-control');

                rows++;
            });

            hideLoading($el);

            SMSVsDimmaDetail = result.Table1;

            checkLikeRadio();

            $("#tbSMSVsDimmas tbody").off("click");

            $('#tbSMSVsDimmas tbody').on('click', 'tr.details-control td:not(.not-details-control)', function () {
                var tr = $(this).parent();
                var row = tbSMSVsDimmas.row(tr);

                if (row.child.isShown()) {
                    // This row is already open - close it
                    row.child.hide();
                    tr.removeClass('shown');
                }
                else {
                    // Open this row
                    row.child(getChildTable(SMSVsDimmaDetail, row.data())).show();
                    tr.addClass('shown');
                    row.child().addClass('bg-gray-light');
                }
            });

            auditProcess("#btnProcesSvD", "#tbSMSVsDimmas", "SMSVsDimmas");

        }, "Json");

    } catch (ex) {
        getTimerDialogMsg('danger', 'No se pudo mostrar el reporte, detalle: ' + ex.message, 'MovementsPanel', 3000);
    }

}

function GetDimmasVsSMS() {
    try {
        var $el = showLoading($("#DimmasVsSMSPanel"));

        $.get($('#tbDimmasVsSMS').data("url"), function (result) {

            tbDimmasVsSMS = $('#tbDimmasVsSMS').DataTable();

            relocatePag_Filt('#tbDimmasVsSMS');

            $.each(result.Table, function (i, element) {
                tbDimmasVsSMS.row.add([
                    element.SUBSCR_ID,
                    element.CANV_CODE,
                    element.CANV_EDITION,
                    formatDate(element.SALES_DATE),
                    element.STF_CODIGO,
                    element.REP_UNIT,
                    element.TEAM_ID,
                    formatNumber(element.CARGO_SMS),
                    formatNumber(element.VENTA_SMS),
                    element.ACCOUNT_STATUS,
                    setCompareStatusColor(element.STATUS),
                     element.LAYOUT_SUST,
                      element.ARTE_AGENCIA
                ]).draw().nodes().to$().addClass('details-control');

            });

            dimmasVsSMSDetail = result.Table1;

            hideLoading($el);

            $("#tbDimmasVsSMS tbody").off("click");

            $('#tbDimmasVsSMS tbody').on('click', 'tr.details-control td:not(.not-details-control)', function () {
                var tr = $(this).parent();
                var row = tbDimmasVsSMS.row(tr);

                if (row.child.isShown()) {
                    // This row is already open - close it
                    row.child.hide();
                    tr.removeClass('shown');
                }
                else {
                    // Open this row
                    row.child(getChildTable(dimmasVsSMSDetail, row.data())).show();
                    tr.addClass('shown');
                    row.child().addClass('bg-gray-light');
                }
            });

        }, "Json");

    } catch (ex) {
        getTimerDialogMsg('danger', 'No se pudo mostrar el reporte, detalle: ' + ex.message, 'MovementsPanel', 3000);
    }

}

function GetSMSVsDimmasRejReport() {
    try {
        var $el = showLoading($("#SMSVsDimmasRejPanel"));

        $.get($('#tbSMSVsDimmasRej').data("url"), function (result) {


            if ($.fn.DataTable.isDataTable('#tbSMSVsDimmasRej')) {
                tbSMSVsDimmasRej.destroy();
            }

            tbSMSVsDimmasRej = $('#tbSMSVsDimmasRej').DataTable({
                "fnRowCallback": function (row, data, start, end, display) {
                    $('td:eq(0),td:eq(1),td:eq(18)', row).addClass("not-details-control ");
                }
            });
            tbSMSVsDimmasRej.clear().draw();
            relocatePag_Filt('#tbSMSVsDimmasRej');
            var rows = 1;
            $.each(result.Table, function (i, element) {
                tbSMSVsDimmasRej.row.add([
                      '<input type="checkbox" name="chkSDR' + rows + '" data-move="1" data-id="' + element.ID + '"  data-sem="' + element.SEM_NUMERO + '" data-account="' + element.REP_ACCOUNT_ID + '" data-edition="' + element.CANV_EDITION + '" data-canv="' + element.CANV_CODE + '" data-seq="' + element.REP_SECUENCIA + '">',
                    '<input type="checkbox"  name="chkSDR' + rows + '" data-move="2" data-id="' + element.ID + '"  data-sem="' + element.SEM_NUMERO + '" data-account="' + element.REP_ACCOUNT_ID + '" data-edition="' + element.CANV_EDITION + '" data-canv="' + element.CANV_CODE + '" data-seq="' + element.REP_SECUENCIA + '">',
                    element.REP_SUBSCR_ID,
                    element.CANV_CODE,
                    element.CANV_EDITION,
                      element.REP_SALES_DATE,
                    formatDate(element.DEV_FECHA_SUBIDA),
                    element.DEV_ESTATUS,
                    element.DEV_FECHA_APERTURA,
                    element.CARGO_SMS,
                    formatNumber(element.VENTA_SMS),
                    element.PROGRAM_STATUS,
                    element.NO_DIGITADA,
                      element.DIF_CHARGE_IN,
                      element.DIF_CHARGE_OUT,
                        element.UPDATED_BY,
                          element.VENTA_X95,
                            setCompareStatusColor(element.STATUS),
                    '<input class="not-details-control form-control goog" type="text" class="form-control goog" id="inputWarning" />'

                ]).draw().nodes().to$().addClass('details-control');

                rows++;
            });

            hideLoading($el);

            SMSVsDimmasRejDetail = result.Table1;

            checkLikeRadio();

            $("#tbSMSVsDimmasRej tbody").off("click");

            $('#tbSMSVsDimmasRej tbody').on('click', 'tr.details-control td:not(.not-details-control)', function () {
                var tr = $(this).parent();
                var row = tbSMSVsDimmasRej.row(tr);

                if (row.child.isShown()) {
                    // This row is already open - close it
                    row.child.hide();
                    tr.removeClass('shown');
                }
                else {
                    // Open this row
                    row.child(getChildTable(SMSVsDimmasRejDetail, row.data())).show();
                    tr.addClass('shown');
                    row.child().addClass('bg-gray-light');
                }
            });

            auditProcess("#btnProcesDvSR", "#tbSMSVsDimmasRej", "SMSVsDimmasRej");

        }, "Json");

    } catch (ex) {
        getTimerDialogMsg('danger', 'No se pudo mostrar el reporte, detalle: ' + ex.message, 'MovementsPanel', 3000);
    }

}
//END #Compares SMS Dimmas


//#UNIVERSE COMPARE
function GetDimmasBerryReport() {
    try {

        var $el = showLoading($("#dimmasBerryPanel"));

        $.get($('#tbDimmasBerry').data("url"), function (result) {

            if (!$.fn.DataTable.isDataTable('#tbDimmasBerry')) {
                tbDimmasBerry = $('#tbDimmasBerry').DataTable();
                relocatePag_Filt('#tbDimmasBerry');
            }
            tbDimmasBerry.clear().draw();

            $.each(result, function (i, element) {
                tbDimmasBerry.row.add([
                    element.CANV_CODE,
                    element.CANV_EDITION,
                    element.CANV_WEEK,
                    element.SUBSCR_ID,
                    element.MAIN_NAME,
                    element.PROD_GRP_CODE,
                      element.DIF_CO,
                      element.DIF_CI

                ]).draw();

            });

            hideLoading($el);

        }, "Json");

    } catch (ex) {
        getTimerDialogMsg('danger', 'No se pudo mostrar el reporte, detalle: ' + ex.message, 'MovementsPanel', 3000);
    }

}

function GetDimmasBerryAcumReport() {
    try {

        var $el = showLoading($("#dimmasBerryAcumPanel"));

        $.get($('#tbDimmasBerryAcum').data("url"), function (result) {

            if (!$.fn.DataTable.isDataTable('#tbDimmasBerryAcum')) {
                tbDimmasBerryAcum = $('#tbDimmasBerryAcum').DataTable();
                relocatePag_Filt('#tbDimmasBerryAcum');
            }
            tbDimmasBerryAcum.clear().draw();

            $.each(result, function (i, element) {
                tbDimmasBerryAcum.row.add([
                    element.CANV_CODE,
                    element.CANV_EDITION,
                    element.CANV_WEEK,
                    element.SUBSCR_ID,
                    element.MAIN_NAME,
                    element.PROD_GRP_CODE,
                      element.DIF_CO,
                      element.DIF_CI,
                     formatDate(element.ARRIVAL_DATE)

                ]).draw();

            });

            hideLoading($el);

        }, "Json");

    } catch (ex) {
        getTimerDialogMsg('danger', 'No se pudo mostrar el reporte, detalle: ' + ex.message, 'MovementsPanel', 3000);
    }

}

function GetDimmasNoBerryReport() {
    try {

        var $el = showLoading($("#dimmasNoBerryPanel"));

        $.get($('#tbDimmasNoBerry').data("url"), function (result) {

            if (!$.fn.DataTable.isDataTable('#tbDimmasNoBerry')) {
                tbDimmasNoBerry = $('#tbDimmasNoBerry').DataTable();
                relocatePag_Filt('#tbDimmasNoBerry');
            }
            tbDimmasNoBerry.clear().draw();

            $.each(result, function (i, element) {
                tbDimmasNoBerry.row.add([
                    element.CANV_CODE,
                    element.CANV_EDITION,
                    element.CANV_WEEK,
                    element.SUBSCR_ID,
                    element.MAIN_NAME,
                    element.PROD_GRP_CODE,
                    element.PROGRAM_STATUS,
                     element.DIMMAS_CO,
                     element.PROD_CO,
                    element.DIMMAS_CI,
                    element.PROD_CI,
                      element.DIF_CO,
                      element.DIF_CI,
                     formatDate(element.ARRIVAL_DATE)

                ]).draw();

            });

            hideLoading($el);

        }, "Json");

    } catch (ex) {
        getTimerDialogMsg('danger', 'No se pudo mostrar el reporte, detalle: ' + ex.message, 'MovementsPanel', 3000);
    }

}
//END #UNIVERSE COMPARE


function getChildTable(childTable, rowParent) {

    var rowNum = 1;

    var tableResl2 = '', tableResl = '<div><table  class="display table table-hover table-vcenter table-bordered table-child" cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">' +
        '<thead><th class="export">Fecha</th><th>Subscr ID</th><th>Canv Code</th><th>Canv Edition</th><th>Cargo</th><th>Venta</th><th>Ejecutivo</th><th class="export">Razon</th><th>Comentario</th><th>Fecha Aceptada</th><th>Digitador</th></thead>';

    $.each(_.where(childTable, { SUBSCR_ID: rowParent[2], CANV_EDITION: rowParent[4] }), function (i, d) {
        tableResl2 = tableResl2 + '<tr>' +
                  '<td>' + formatDate(d.REP_SALES_DATE) + '</td>' +
                  '<td>' + d.SUBSCR_ID + '</td>' +
                  '<td>' + d.CANV_CODE + '</td>' +
                  '<td>' + d.CANV_EDITION + '</td>' +
                  '<td>' + d.REP_CARGO + '</td>' +
                  '<td>' + d.NEW_ISSUE + '</td>' +
                  '<td>' + d.EJECUTIVO + '</td>' +
                  '<td>' + d.RAZON + '</td>' +
                  '<td>' + d.REP_COMENTARIO + '</td>' +
                  '<td>' + d.REP_CLOSEDATE + '</td>' +
                  '<td>' + d.DIGITADOR + '</td>' +
                  '</tr>';
        rowNum++;
    });


    tableResl = tableResl + tableResl2 + '</table></div>';

    return tableResl;
}

function GetTotalDifCompares(url) {

    var $el = showLoading($("#ChartPanel"));

    $.get($('#chart').data("url"), function (result) {

        var base = {},
            series = [],
            versions = {},
            drilldownSeries = [];

        $.each(result, function (i, table) {

            var tag = i;

            $.each(table, function (i, value) {
                var brand,
                    version;

                // Split into brand and version
                version = value.DIFERENCIA;

                // Create the main data
                if (!base[tag]) {
                    base[tag] = parseInt(value.TOTAL);
                } else {
                    base[tag] += parseInt(value.TOTAL);
                }

                // Create the version data
                if (version !== null) {
                    if (!versions[tag]) {
                        versions[tag] = [];
                    }
                    versions[tag].push([version, parseInt(value.TOTAL)]);
                }
            });


        });

        //Set Series
        $.each(base, function (name, y) {

            series.push({
                name: name,
                y: y,
                drilldown: versions[name] ? name : null
            });
        });

        //End Set Series

        //Set drilldownSeries
        $.each(versions, function (key, version) {
            drilldownSeries.push({
                name: key,
                id: key,
                data: version
            });
        });

        //End Set drilldownSeries

        $(function () {

            // Create the chart
            $('#chart').highcharts({
                chart: {
                    type: 'column'
                },
                title: {
                    text: ''
                },
                subtitle: {
                    text: ''
                },
                xAxis: {
                    type: 'category'
                },
                yAxis: {
                    title: {
                        text: 'Total Diferencias'
                    }
                },
                legend: {
                    enabled: false
                },
                plotOptions: {
                    series: {
                        borderWidth: 0,
                        dataLabels: {
                            enabled: true,
                            format: '{point.y:,.0f}'
                        }
                    }
                },

                tooltip: {
                    headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                    pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:,.0f}</b> total<br/>'
                },

                series: [{
                    name: 'base',
                    colorByPoint: true,
                    data: series
                }],
                drilldown: {
                    series: drilldownSeries
                }
            })
        });

        hideLoading($el);

    }, "Json");

    Highcharts.theme = {
        colors: ["#5aaedc", "#986291", "#50c7a7", "#9cc96b", "#aaeeee", "#ff0066", "#eeaaee",
           "#55BF3B", "#DF5353", "#7798BF", "#aaeeee"],
        chart: {
            backgroundColor: null,
            style: {
                fontFamily: "Dosis, sans-serif"
            }
        },
        title: {
            style: {
                fontSize: '16px',
                fontWeight: 'bold',
                textTransform: 'uppercase'
            }
        },
        tooltip: {
            borderWidth: 0,
            backgroundColor: 'rgba(219,219,216,0.8)',
            shadow: false
        },
        legend: {
            itemStyle: {
                fontWeight: 'bold',
                fontSize: '13px'
            }
        },

        plotOptions: {
            candlestick: {
                lineColor: '#404048'
            }
        },


        // General
        background2: '#F0F0EA'

    };

    // Apply the theme
    Highcharts.setOptions(Highcharts.theme);


}

function getStatusCompareReport() {
    
    var $el = showLoading($("#statusCompare"));

    $.get($('#tbStatusCompare').data("url"), function (result) {

        if (!$.fn.DataTable.isDataTable('#tbStatusCompare')) {
            tbStatusReport = $('#tbStatusCompare').DataTable({ paging: false, searching: false });
            relocatePag_Filt('#tbStatusCompare');
        }
        tbStatusReport.clear().draw();

        $.each(result, function (i, element) {
            tbStatusReport.row.add([
                element.REPORT,
                element.PENDIENTE,
                element.DEVUELTO,
                element.CORREGIDO,
                element.AUDITADO

            ]).draw();

        });

        hideLoading($el);

    }, "Json");

}

function setCompareStatusColor(status) {

    if (status == 'Pendiente') {
        return "<div class='label label-table label-purple'><span data-id='1'>" + status + "</span></div>";
    } else if (status == "Devuelto") {
        return "<div class='label label-table label-primary'><span data-id='2'>" + status + "</span></div>";
    } else if (status == 'Corregido') {
        return "<div class='label label-table label-mint'><span data-id='3'>" + status + "</span></div>";
    } else {
        return "<div class='label label-table label-default'><span data-id='4'" + status + "</span></div>";
    }
}

function auditProcess(btnID, tableID, report) {

    $(btnID).click(function () {

        var childRows = $(tableID + " input[type='checkbox']:checked");
        var transactions = [];

        childRows.each(function (i, v) {
            var comment = $(this).closest('tr').children(".not-details-control").children("input[type='text']").val();

            var transaction = {
                "Info": {
                    AuditID: $(v).data("id"), SemNumero: $(v).data("sem"),
                    RepAccountID: $(v).data("account"), CanvEdition: $(v).data("edition"),
                    CanvCode: $(v).data("canv"), RepSecuencia: $(v).data("seq"),
                    AuditComment: ""
                },
                "Action": $(v).data("move")
            };

            transactions.push(transaction);

        });
        if (transactions.length > 0) {

            changeText("procesar", 'Procesando..');

            $.ajax({
                type: "POST",
                url: $(btnID).data("url"),
                data: JSON.stringify({ "transactions": transactions }),
                contentType: 'application/json; charset=utf-8',

                success: function (data) {
                    if (data.split("-")[0].trim() != "Error") {

                        getTimerDialogMsg('success', data, 'ComparesPanel', 7000);

                        if (report == 'SMSVsDimmas') {
                            GetSMSVsDimmasReport();
                        }
                        else if (report == 'SMSVsDimmasRej') {
                            GetSMSVsDimmasRejReport();
                        }

                        getStatusCompareReport();
                    }
                    else {
                        getTimerDialogMsg('danger', data, 'ComparesPanel', 7000);

                    }


                    changeText("procesar", 'Procesar');

                },
                error: function (ex) {
                    getTimerDialogMsg('danger', data, 'ComparesPanel', 7000);
                    changeText("procesar", 'Procesar');

                },
                dataType: "json"
            });
        } else {
            getTimerDialogMsg('purple', 'Debe seleccionar una acción', 'MovementsPanel', 7000);
        }

    });
}

function checkLikeRadio() {

    $("input:checkbox").click(function () {
        var group = "input:checkbox[name='" + $(this).attr("name") + "']";
        var val = $(this).prop("checked");

        if (val == false) {
            $(this).prop("checked", false);
        } else {
            $(group).prop("checked", false);
            $(this).prop("checked", true);
        }

    });
}

$('#collapseOne').on('hidden.bs.collapse', function () {
    $("#reportsPanel").html('<i class="fa fa-plus-circle" style="font-size: 16px"></i>');
})

$('#collapseOne').on('show.bs.collapse', function () {
    $("#reportsPanel").html('<i class="fa fa-minus-circle" style="font-size: 16px"></i>');
})
