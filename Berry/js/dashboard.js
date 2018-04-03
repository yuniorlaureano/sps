$(function () {

    $('#tbl-generated-lastweek').DataTable({
        "bDestroy": true,
        "deferRender": true,
        "bSort": false
    });

    $('#tbl-closed-week-dash').DataTable({
        "bDestroy": true,
        "deferRender": true,
        "bSort": false
    });

    getLastRunRow();
    generateOpenCloseWeekChart();
    autoLoadCloseWeek();

});

function autoLoadCloseWeek() {

    var $el = showLoading($("#tbl-closed-week-dash-panel"));
    var table = $("#tbl-closed-week-dash").DataTable();;

    $.get($("#tbl-closed-week-dash").attr("data-url")).done(function (httpResponse) {

        addRow(table, JSON.parse(httpResponse));
        hideLoading($el);
    });

}

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

function openCloseWeekChart(abierta, cerrada, generada) {

    var totalVisitors = 883000;
   
    (new CanvasJS.Chart("chart-cls-opn-rn-week", {
        animationEnabled: true,
        theme: "light1",
        title: {
            text: "Semanas Cerradas vs Semanas abiertas"
        },
        legend: {
            fontFamily: "calibri",
            fontSize: 17,
            itemTextFormatter: function (e) {
                //return e.dataPoint.name + ": " + Math.round(e.dataPoint.y / totalVisitors * 100) + "%";
                return e.dataPoint.name + ": " + e.dataPoint.y;
            }
        },
        data: [{
            cursor: "pointer",
            explodeOnClick: false,
            innerRadius: "75%",
            legendMarkerType: "square",
            name: "Semanas cerradas vs semanas abiertas",
            radius: "100%",
            showInLegend: true,
            startAngle: 90,
            type: "doughnut",
            dataPoints: [
                { y: abierta, name: "Abiertas", color: "#E7823A", label: "Abiertas" },
                { y: cerrada, name: "Cerradas", color: "#546BC1", label: "Cerradas" },
                { y: generada, name: "Corridas", color: "#00e600", label: "Corridas" }
            ]
        }]
    })).render();
}

function getLastRunRow() {

    var $el = showLoading($("#last-run-week-panel"));

    $.get($('#tbl-generated-lastweek').data("url"), function (result) {

        var _data = JSON.parse(result);
        var table = $('#tbl-generated-lastweek').DataTable();

        table.clear().draw();

        addLastRunRow(table, _data);

        hideLoading($el);
        ///--funcion para el manejo del pequenio dashboard.
    });
}

function generateOpenCloseWeekChart() {

    showColorLoading();

    $.get($('#chart-cls-opn-rn-week').data("url"), function (result) {

        var _data = JSON.parse(result);

        openCloseWeekChart(_data[0].OPENED, _data[0].CLOSED, _data[0].GENERATED);
        ///--funcion para el manejo del pequenio dashboard.
        hideColorLoading();
    });
}

function addLastRunRow(table, _data) {

    $(_data).each(function (key, value) {

        table.row.add([
             value.BOOK_CODE,
             value.WEEKLY_PI,
             value.WEEKLY_NI,
             value.INCREASE,
             value.DECREASE,
             value.CANCEL,
             value.NETO
        ]).draw(false);
    });
}