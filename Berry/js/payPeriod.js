var payPeriodReport,  payPeriodPivotReport = [],   payPeriodReports;

$(document).ready(function () {

    getActualEditions();
    $("#ddlCanvassEdition").change(function () {
        getPayPeriods();
    });

    $("#ddlPayPeriod").change(function () {
        getPayPeriodDateRange();
    });


    $("#btnSearch").click(function (e) {
        e.preventDefault;
        getPivotReport();
    });

    $("#btnPDF").click(function (e) {
        e.preventDefault;
        exportBillingReport(1);
    });

    $("#btnXLS").click(function (e) {
        e.preventDefault;
        exportBillingReport(2);
    });

})

function getActualEditions() {

    showColorLoading();

    $.get($('#ddlCanvassEdition').data("url"), function (edition) {
        var html = "";

        $.each(edition, function (i, value) {

            html += "<option value=" + value.PROD_CYCLE + ">" + value.PROD_CYCLE + "</option>"
        });

        $("#ddlCanvassEdition")
            .html(html)
            .selectpicker('refresh');

        hideColorLoading();
    }).then(function () {
        getPayPeriods();
    });


}

function getPayPeriods() {

    showColorLoading();

    $.get($('#btnSearch').data("url"), { "edition": $("#ddlCanvassEdition").val() == null ? 0 : $("#ddlCanvassEdition").val() }, function (payPeriod) {
        var html = "";

        $.each(payPeriod, function (i, payPeriod) {

            html += "<option value=" + payPeriod.VALUE + " data-subtext=" + payPeriod.STATUS + " data-start=" + payPeriod.PERIOD_START_DATE + " data-end=" + payPeriod.PERIOD_END_DATE + ">" + payPeriod.TEXT + "</option>"
        });

        $("#ddlPayPeriod")
            .html(html)
            .selectpicker('refresh');

        getPayPeriodDateRange();

        hideColorLoading();

    });

}

function getPayPeriodDateRange() {
    $("#txtStart").val(moment($("#ddlPayPeriod :selected").data("start")).format('DD-MMM-YYYY'));
    $("#txtEnd").val(moment($("#ddlPayPeriod :selected").data("end")).format('DD-MMM-YYYY'));
}

function getChildTable(childTable, rowParent) {

    var rowNum = 1;
    var tableResl2 = '', tableResl = '<div><table  class="display table table-hover table-bordered table-child" cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">' +
        '<thead><th class="text-center">Subscr ID</th class="text-center"><th>Name</th><th>Representante</th><th class="text-center">Canv Week</th><th class="text-center">Canv</th><th class="text-center">Edition</th><th class="text-center">Prod Code</th><th class="text-center">Prod CO</th><th class="text-center">Prod CI</th><th class="text-center">Renew CI</th><th class="text-center">Decrease CI</th><th class="text-center">New</th><th class="text-center">Ctrl Loss</th><th class="text-center">Unctrl Loss</th></thead>';

    $.each(_.where(childTable, { EMPLOYEE_ID: rowParent[1] }), function (i, d) {
        tableResl2 = tableResl2 + '<tr>' +

                  '<td>' + d.SUBSCR_ID + '</td>' +
                  '<td class="text-left">' + d.NAME + '</td>' +
                  '<td class="text-left">' + d.REPRESENTATIVE + '</td>' +
                  '<td>' + d.CANV_WEEK + '</td>' +
                  '<td>' + d.CANV_CODE + '</td>' +
                  '<td>' + d.CANV_EDITION + '</td>' +
                  '<td>' + d.PROD_GRP_CODE + '</td>' +
                  '<td>' + d.PROD_CO + '</td>' +
                  '<td>' + d.PROD_CI + '</td>' +
                  '<td>' + d.RENEW_CI + '</td>' +
                  '<td>' + d.DECREASE_CI + '</td>' +
                  '<td>' + d.NEW + '</td>' +
                   '<td>' + d.CTRL_LOSS + '</td>' +
                  '<td>' + d.UNCTRL_LOSS + '</td>' +
                  '</tr>';
        rowNum++;
    });

    tableResl = tableResl + tableResl2 + '</table></div>';

    return tableResl;
}

function getPivotReport() {

    showColorLoading();

    $.get($('#tbPivotReport').data("url"), { "edition": $("#ddlCanvassEdition").val() == null ? 0 : $("#ddlCanvassEdition").val(), "periodID": $('#ddlPayPeriod').val() == null ? 0 : $('#ddlPayPeriod').val() }, function (result) {

        if (!$.fn.DataTable.isDataTable('#tbPivotReport')) {

            tbPivot = $('#tbPivotReport').DataTable({
                "order": [[0, 'asc']],
                "drawCallback": function (settings) {

                    var api = this.api();
                    var rows = api.rows({ page: 'current' }).nodes();
                    var last = null;
                    if (api.column(0).data().length > 0) {
                        api.column(0, { page: 'current' }).data().each(function (group, i) {

                            var values = [0, 0, 0, 0, 0, 0, 0, 0, 0];
                            var valuesClass = ['.prodCoDig', '.newDig', '.prodCiDig',
                                                '.prodCoPrint', '.newPrint', '.prodCiPrint',
                                                '.prodCoTotal', '.newTotal', '.prodCiTotal'];
                            var valuesCols = '';

                            if (last !== group) {


                                var rowSupGroup = $('#tbPivotReport tr td').filter(function () { return $(this).text() == $(group).text() }).closest('tr');

                                var total = [];

                                $(rowSupGroup).map(function (idx, val) {

                                    valuesClass.forEach(function (name, index) {
                                        values[index] += parseInt($(val).find(name).text());
                                    })

                                });
                                values.forEach(function (value, index) {
                                        valuesCols += '<td>' + value + '</td>';
                                    })

                               

                                $(rows).eq(i).before(
                                  '<tr class="group bg-gray" style="font-weight: 500; color: #000;">' +
                                  '<td colspan="3" class="text-left">' + group + '</td>' +
                                  valuesCols +
                                  '</tr>'
                              );

                                last = group;
                            }
                            $("span.text-left").closest("td").addClass("text-left");
                        });
                    }
                    },
                "footerCallback": function (row, data, start, end, display) {
                    var api = this.api(), data;
                    var pageTotal = [];

                    // Remove the formatting to get integer data for summation
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                            i : 0;
                    };

                    if (api.column(1).data().length > 0) {
                        // Total over all pages
                        for (i = 3; i <= 11; i++) {
                            // Total over all pages

                            total = api
                             .column(i)
                             .data()
                             .reduce(function (a, b) {
                                 return  ($(a).text().length > 0 ? intVal($(a).text()) : intVal(a) ) +
                                     ($(b).text().length > 0 ? intVal($(b).text()) : intVal(b));
                             });


                            // Update footer
                            $(api.column(i).footer()).html(
                                formatNumber(total)
                            );
                        }

                    }
                },

                "ordering": false

            })

        }
        //Clean Data in Datatable
        tbPivot.clear().draw();

        relocatePag_Filt('#tbPivotReport');

        var asInitVals = new Array();

        //Show table panel when the data is ready
        $("#pivotReportPanel").show();

        $.each(result.Table, function (i, element) {
              tbPivot.row.add([
              '<span class="text-left">' + element.SUPERVISOR + '</span>',
               element.EMPLOYEE_ID,
               '<span class="text-left">' + element.REPRESENTATIVE + '</span>',
               '<span class="prodCoDig">' + element.PROD_CO_DIGITAL + '</span>',
               '<span class="newDig">' + element.NEW_DIGITAL + '</span>',
               '<span class="prodCiDig">' + element.PROD_CI_DIGITAL + '</span>',
               '<span class="prodCoPrint">' + element.PROD_CO_PRINT + '</span>',
               '<span class="newPrint">' + element.NEW_PRINT + '</span>',
               '<span class="prodCiPrint">' + element.PROD_CI_PRINT + '</span>',
               '<span class="prodCoTotal">' + element.PROD_CO + '</span>',
              '<span class="newTotal">' + element.NEW + '</span>',
              '<span class="prodCiTotal">' + element.PROD_CI + '</span>'

            ]).draw().nodes().to$().addClass('details-control');

            $("span.text-left").closest("td").addClass("text-left");
        });

        payPeriodReport = result.Table1;
        payPeriodPivotReport = result.Table;

        payPeriodReports = {
            DetailedReport: payPeriodReport,
            PivotReport: payPeriodPivotReport
        }

        //Clean click
        $("#tbPivotReport tbody").off("click");

        //Assign child table
        $('#tbPivotReport tbody').on('click', 'tr.details-control', function () {
            var tr = $(this);
            var row = tbPivot.row(tr);

            if (row.child.isShown()) {
                // This row is already open - close it
                row.child.hide();
                tr.removeClass('shown');
            }
            else {
                // Open this row
                row.child(getChildTable(payPeriodReport, row.data())).show();
                tr.addClass('shown');
                row.child().addClass('bg-gray-light');
            }
        });

        hideColorLoading();

    });
}

function GetValidValue(value) {

    if (value) {
        return value;
    } else {
        return 0;
    }
}

function exportBillingReport(action) {

    showColorLoading();
        
        $.ajax({
            url: $('#btnExportReport').data("url"),
            type: 'post',
            dataType: 'json',
            contentType: "application/json",


            success: function (data) {
                data.file.map(function(file){
                    window.open(file);
                })
                hideColorLoading();
            },
            error: function (ex) {
                hideColorLoading();
            },
            data: JSON.stringify({
                "edition": $("#ddlCanvassEdition").val() == null ? 0 : $("#ddlCanvassEdition").val(),
                "periodID": $('#ddlPayPeriod').val() == null ? 0 : $('#ddlPayPeriod').val(),
                "startDate": $("#txtStart").val(),
                "endDate": $("#txtEnd").val(),
                "payPeriodReports": payPeriodReports,
                "action": action

            })


        });
}