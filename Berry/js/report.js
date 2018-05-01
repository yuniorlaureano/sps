var activeCanvWeekParams = {
    editionActivated: false
};

$(document).ready(function () {

    $("input.date").datepicker({ autoclose: true });

    $('#weekly-fecha-desde').change(function () {

        var startDate = $(this).val();
        var endDate = $('#weekly-fecha-hasta').val();

        if (activeCanvWeekParams.editionActivated) {

            activeCanvWeekParams.editionActivated = false;

            return activeCanvWeekParams.editionActivated;
        } else {

            if ((startDate !== '') && (endDate !== '')) {
                                
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
        }
    });

    $('#weekly-fecha-hasta').change(function () {

        var startDate = $('#weekly-fecha-desde').val();
        var endDate = $(this).val();

        if ((startDate !== '') && (endDate !== '')) {
                        
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
    });

    $('#canvweek').change(function () {

        var url = $(this).attr("data-url");
        var week = $(this).val();


        if (week !== 'none') {

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
    
    $(".weekle-report").on("click", function () {
        
        var startDate = $('#weekly-fecha-desde').val();
        var endDate = $('#weekly-fecha-hasta').val();

        if ((startDate !== '') && (endDate !== '')) {
            
            downloadPsGnReport(
                $(this).attr("data-url"),
                $(this).attr("data-download-url"),
                startDate,
                endDate);

        } else {

            alertNoty("Debe proveer las fechas", "Info", "danger");
        }
    });

    $(".weekle-report-personalize").on("click", function () {

        var startDate = $('#weekly-fecha-desde').val();
        var endDate = $('#weekly-fecha-hasta').val();

        if ((startDate !== '') && (endDate !== '')) {

            var url = $(this).attr("data-url") + "?startDate=" + startDate + "&endDate=" + endDate;
            window.open(url, '_blank');
           
        } else {

            alertNoty("Debe proveer las fechas", "Info", "danger");
        }
    });

});

function clearDates() {

    $('#weekly-fecha-desde').val("");
    $('#weekly-fecha-hasta').val("");
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


function addDetailRow(element, resultset) {

    element.empty();
    var html = "";
    
    $(resultset).each(function (key, value) {

        html = "";
        html += '<div class="col-sm-2 col-lg-2"><div class="panel panel-info panel-colorful">';
        html += '<div class="pad-all">';
        html += '<p class="text-lg text-semibold">Semana (' + value.CANV_WEEK + ')</p>';
        html += '<span class="text-bold">' + moment(value.DESDE).format('MM/DD/YYYY') + '-</span>';
        html += '<span class="text-bold">' + moment(value.HASTA).format('MM/DD/YYYY') + '</span>';
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

function downloadPsGnReport(url, file, startDate, endDate) {

    showColorLoading();
    $.post(url, { startDate: startDate, endDate: endDate }, function (result) {

        if (result == "OK") {
  
            window.location = file;
        } else {
            alertNoty("Error al descargar el archivo. Itente nuevamente.", "Info", "danger");
        }

        hideColorLoading();
    });
}