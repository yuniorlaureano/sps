$(document).ready(function () {

    $('#tbLogReportPanel').DataTable({
        "bDestroy": true,
        "deferRender": true,
        "bSort": false
    });

    $("input.date").datepicker({ autoclose: true });

    $('#tbLogReportPanel tbody').on("click", '.anabled-command-button', function () {

        var td = $(this).parents("tr").find("td");
        var week = $(td[0]).text();
        var startDate = $(td[1]).text();
        var endDate = $(td[2]).text();

        bootbox.confirm("Está seguro de cerrar la semana?", function (result) {

            if (result) {

                var url = $('#tbLogReportPanel').attr("data-url");

                closeWeek(url, startDate, endDate, week, function (response) {

                    if (response == "OK") {
                        alertNoty("Semana cerrada!!!", "Info", "success");
                    } else {

                        alertNoty("Error al cerrar la semana!!!", "Info", "Danger");
                    }

                    autoLoadCloseWeek($('#logReportPanel').attr("data-url"), function (httpResponse) {

                        var _data = JSON.parse(httpResponse);
                        var datatable = $('#tbLogReportPanel').DataTable();

                        if (_data.length > 0) {

                            addRow(datatable, _data);
                        } else {

                            datatable.clear().draw();
                        }
                    });

                });
            }
        });
    });

    autoLoadCloseWeek($('#logReportPanel').attr("data-url"), function (httpResponse) {

        var _data = JSON.parse(httpResponse);
        var datatable = $('#tbLogReportPanel').DataTable();

        if (_data.length > 0) {

            addRow(datatable, _data);
        } else {

            datatable.clear().draw();
        }
    });

});

function addRow(table, resultset) {

    table.clear().draw();

    $(resultset).each(function (key, value) {

        table.row.add([
        value.WEEK,
        moment(value.DATE_START_WEEK).format('MM/DD/YYYY'),
        moment(value.DATE_END_WEEK).format('MM/DD/YYYY'),
        "<button class='btn " + (key == 0 ? 'btn-primary' : 'btn-default') + " " + (key == 0 ? 'anabled-command-button' : 'disabled-command-button') + "' " + (key == 0 ? 'anabled' : 'disabled') + ">Cerrar</button>"
        ]).draw(false);
    });

}

function autoLoadCloseWeek(url, func) {

    var $el = showLoading($("#logReportPanel-body"));

    $.get(url).done(function (httpResponse) {

        func(httpResponse);
        hideLoading($el);
    });

}

function closeWeek(url, starDate, endDate, week, func) {

    $.post(url, { startWeek: starDate, endWeek: endDate, week: week }, function (response) {

        func(response);
    });
}

