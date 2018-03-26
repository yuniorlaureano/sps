var activeCanvWeekParams = {
    editionActivated: false
};

$(document).ready(function () {

    $("#command-button-proccess").attr("disabled", true);
    //$("#panel-canv-wee-details").hide();
    $("#post-berry-generated-chart-parent").hide();

    $('#tbLogReportPanel').DataTable({
        "bDestroy": true,
        "deferRender": true,
        "bSort": false
    });

    $("input.date").datepicker({ autoclose: true });

    $('#weekly-fecha-desde').change(function () {

        var startDate = $(this).val();
        var endDate = $('#weekly-fecha-hasta').val();
        var week = $('#canvweek').val();

        if (activeCanvWeekParams.editionActivated) {

            activeCanvWeekParams.editionActivated = false;

            return activeCanvWeekParams.editionActivated;
        } else {

            if ((startDate != '' || startDate != null) && (endDate != '' || endDate != null)) {
                                
                getOpenedCanvWeekDetailsByDate(startDate, endDate, function (data) {

                    showColorLoading();

                    var element = $('#panel-body-canv-wee-details div.row');

                    if (data.length > 0) {

                        $("#panel-canv-wee-details").show();
                        addDetailRow(element, data);
                    } else {

                        $("#panel-canv-wee-details").hide();
                        element.empty();
                    }
                });
            }

            showWeekTap();
        }
    });

    $('#weekly-fecha-hasta').change(function () {

        var startDate = $('#weekly-fecha-desde').val();
        var endDate = $(this).val();
        var week = $('#canvweek').val();

        if ((startDate != '' || startDate != null) && (endDate != '' || endDate != null)) {
                        
            getOpenedCanvWeekDetailsByDate(startDate, endDate, function (data) {

                showColorLoading();

                var element = $('#panel-body-canv-wee-details div.row');
                
                if (data.length > 0) {

                    $("#panel-canv-wee-details").show();
                    addDetailRow(element, data);
                } else {

                    $("#panel-canv-wee-details").hide();
                    element.empty();
                }                
            });
        }

        showWeekTap();
    });

    $('#canvweek').change(function () {

        var url = $(this).attr("data-url");
        var week = $(this).val();


        if (week != 'none') {

            getActiveDateByWeek( week, url, function (dates) {

                var desde = moment(dates.DESDE).format('MM/DD/YYYY');
                var hasta = moment(dates.HASTA).format('MM/DD/YYYY');

                activeCanvWeekParams.editionActivated = true;
                $('#weekly-fecha-desde').datepicker("update", desde);
                $('#weekly-fecha-hasta').datepicker("update", hasta);

            });

            $("#command-button-proccess").attr("disabled", false);
        } else {

            $("#command-button-proccess").attr("disabled", true);
        }
    });
    
    autoLoadWeekByDb(function (httpResponse) {

        showColorLoading();
        var editions = JSON.parse(httpResponse);
        var options = "<option value='none'>Todas</option>";

        $(editions).each(function (key, value) {

            options += "<option value='" + value.WEEK + "'>" + value.WEEK + "- (" + moment(value.DESDE).format('MM/DD/YYYY') + " - " + moment(value.HASTA).format('MM/DD/YYYY') + ")</option>";
        });

        $('#canvweek').html(options);
        $('#canvweek').selectpicker('refresh');
        hideColorLoading();
    });

    $('#command-button-proccess').click(function (e) {

        showColorLoading();
        var url = $(this).attr("data-url");

        bootbox.confirm({
            message: "<p class='text-semibold text-main'>Se generará el berry para las semanas saleccionadas, ¿está seguro que desea realizar esta acción?.</p>",
            buttons: {
                confirm: {
                    label: "Generar"
                }
            },
            callback: function (result) {

                if (result) {

                    generarWeekle(url);
                } else {

                    hideColorLoading();
                }
            },
            animateIn: 'swing',
            animateOut: 'hide'
        });
    });
    
    //paintPostGeneratedBerryChart();
    generatePostBerryBarChart();
});

function clearDates() {

    $('#weekly-fecha-desde').val("");
    $('#weekly-fecha-hasta').val("");
}

function showChartTap() {

    $("a[href='#demo-tabs-box-1']").parents("li").removeClass("active");
    $("#demo-tabs-box-1").removeClass("in active");

    $("a[href='#demo-tabs-box-2']").parents("li").addClass("active");
    $("#demo-tabs-box-2").addClass("in active");
}

function showWeekTap() {

    $("a[href='#demo-tabs-box-1']").parents("li").addClass("active");
    $("#demo-tabs-box-1").addClass("in active");

    $("a[href='#demo-tabs-box-2']").parents("li").removeClass("active");
    $("#demo-tabs-box-2").removeClass("in active");
}

function autoLoadWeekByDb(func) {
    
    $.get($('#form-berry-generator').attr("data-url")).done(function (httpResponse) {

        func(httpResponse);
    });
}

function getOpenedCanvWeekDetailsByDate(startDate, endDate, func) {
    
    $.get($('#panel-body-canv-wee-details').data("url"), { startDate: startDate, endDate: endDate }, function (result) {

        var _data = JSON.parse(result);

        func(_data);
        hideColorLoading();
    });
}

function getPostGeneratedBerryByDate(startDate, endDate) {

    $.get($('#post-berry-generated-chart').data("url"), { startDate: startDate, endDate: endDate }, function (result) {

        var _data = JSON.parse(result);

        var data = [
                    {   type: "column",
                        name: "PI",
                        showInLegend: true,
                        yValueFormatString: "#,##0.# Units",
                        dataPoints: []
                    },
                    {
                        type: "column",
                        name: "NI",
                        //axisYType: "secondary",
                        showInLegend: true,
                        yValueFormatString: "#,##0.# Units",
                        dataPoints: []
                    }
        ];

        $(_data).each(function (key, value) {

            data[0].dataPoints.push({ label: $.trim(value.BOOK_CODE), y: value.WEEKLY_NI });
            data[1].dataPoints.push({ label: $.trim(value.BOOK_CODE), y: value.WEEKLY_PI });
        });

        showChartTap();
        generatePostBerryBarChart(data);

        hideColorLoading();
        ///--funcion para el manejo del pequenio dashboard.
    });    
}

function addDetailRow(element, resultset) {

    element.empty();
    var html = "";
    
    $(resultset).each(function (key, value) {

        html = "";
        html += '<div class="col-sm-2 col-lg-2"><div class="panel panel-info panel-colorful">';
        html += '<div class="pad-all">';
        html += '<p class="text-lg text-semibold"><i class="demo-pli-wallet-2 icon-fw"></i>Semana <span>(' + value.CANV_WEEK + ')</span></p>';
        html += '<p class="mar-no"><span class="pull-right text-bold">' + moment(value.DESDE).format('MM/DD/YYYY') + '</span>Fecha de inicio</p>';
        html += '<p class="mar-no"><span class="pull-right text-bold">' + moment(value.HASTA).format('MM/DD/YYYY') + '</span>Fecha de fin</p>';
        html += '</div>';
        html += '</div></div>';

        element.append(html);
     
    });

}

function getActiveDateByWeek(week, url, func) {

    showColorLoading();
    $.get(url, { week: week }, function (result) {

        var _data = JSON.parse(result);


        if (_data.length > 0) {

            func(_data[0]);
        }

        hideColorLoading();
    });
}

function generarWeekle(url) {

    var startDate = $("#weekly-fecha-desde").val();
    var endDate = $("#weekly-fecha-hasta").val();

    if ((startDate != null || startDate != '') && (endDate != null || endDate != '')) {

        $.post(url, { startWeek: startDate, endWeek: endDate }).done(function () {

            alertNoty("Berry corrido", "Info", "success");
            getPostGeneratedBerryByDate(startDate, endDate);

        }).fail(function () {

            alertNoty("Error al correr Berry", "Info", "danger");
        })
    }
}

function generatePostBerryBarChart(data) {

    $("#post-berry-generated-chart-parent").show();

    var chart = new CanvasJS.Chart("post-berry-generated-chart", {
        exportEnabled: true,
        animationEnabled: true,
        title: {
            text: "NI y PI, por libros"
        },
        subtitles: [{
            text: "semana generadas"
        }],
        //axisX: {
        //    title: "States"
        //},
        axisY: {
            title: "Cantidad",
            titleFontColor: "#4F81BC",
            lineColor: "#4F81BC",
            labelFontColor: "#4F81BC",
            tickColor: "#4F81BC"
        },
        //axisY2: {
        //    title: "Clutch - Units",
        //    titleFontColor: "#C0504E",
        //    lineColor: "#C0504E",
        //    labelFontColor: "#C0504E",
        //    tickColor: "#C0504E"
        //},
        toolTip: {
            shared: true
        },
        legend: {
            cursor: "pointer",
            itemclick: toggleDataSeries
        },
        data: data
    });
    chart.render();

    function toggleDataSeries(e) {
        if (typeof (e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
            e.dataSeries.visible = false;
        } else {
            e.dataSeries.visible = true;
        }
        e.chart.render();
    }

}