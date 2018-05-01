$(function () {
    
    $('#tbPivotReport').DataTable({
        "bDestroy": true,
        "deferRender": true,
        "bSort": false,
        "lengthMenu": [[10, 25, 50, -1], [10, 25, 50]],
        "iDisplayLength": 15
    });

    $('#sales-report-subscr-details').DataTable({
        "bDestroy": true,
        "deferRender": true,
        "bSort": false,
        "lengthMenu": [[10, 25, 50, -1], [10, 25, 50]],
        "iDisplayLength": 15
    });

    //$("#generate-report").attr("disabled", true);

    $('#canvedition').change(function () {
        showColorLoading();

        var url = $('#canvweek').attr("data-url");
        var edition = $(this).val();
        var options = "<option value='none'>Todas</option>";
        var dateTemplate = "";

        if (edition !== 'none') {

            getGeneratedWeekByEdition(edition, url, function (data) {
              
                $(data).each(function (key, value) {

                    dateTemplate = moment(value.DESDE).format('MM/DD/YYYY') + " - " + moment(value.HASTA).format('MM/DD/YYYY');

                    options += "<option value='" + value.WEEK + " - " + dateTemplate + "'>" + value.WEEK + "- (" + dateTemplate + ")</option>";

                });

                $('#canvweek').html(options);
                $('#canvweek').selectpicker('refresh');
                //hideColorLoading();

                //$("#command-button-proccess").attr("disabled", false);

                hideColorLoading();
            });
            
        } else {

            $('#canvweek').html("<option value='none'>Todas</option>");
            $('#canvweek').selectpicker('refresh');
            //$("#command-button-proccess").attr("disabled", true);
            hideColorLoading();
        }
    });

    getGeneratedEditions(function (httpResponse) {

        
        var editions = JSON.parse(httpResponse);
        var options = "<option value='none'>Todas</option>";

        $(editions).each(function (key, value) {

            options += "<option value='" + value.CANV_EDITION + "'>" + value.CANV_EDITION + "</option>";
        });

        $('#canvedition').html(options);
        $('#canvedition').selectpicker('refresh');
        hideColorLoading();
    });

    $('#canvweek').change(function () {

        var url = $(this).attr("data-url-gn-report");
        var fullDate = $(this).val();
        var splitDates = [];
        var table = $('#tbPivotReport').DataTable();
        var datas = [];

        if (fullDate !== 'none') {

            showColorLoading();
            splitDates = fullDate.split(" - ");


            generateSalesReport(url, splitDates[1], splitDates[2], function (data) {

                datas = JSON.parse(data);
                
                if (datas.length > 0) {
                    
                    addRow(table, datas);
                } else {

                    table.clear().draw();
                }

                hideColorLoading();
            });
            

        } else {

            table.clear().draw();
        }

       
    });

    $(document).on("click", '.sales-employee-details', function () {

        showColorLoading();
        var table = $('#sales-report-subscr-details').DataTable();
        var splitDates = $('#canvweek').val().split(" - ");
        var url = $('#canvweek').data("url-details");

        generateSalesReportSubscrData(url, $(this).text(), splitDates[1], splitDates[2], function (resultset) {

            addSubscrDetailsRow(table, resultset);
            $("#sales-subscr-details-modal").modal({ show: 'true' });
            hideColorLoading();
        });
    });

    $("#generate-pdf-report").on("click", function () {
      
        showColorLoading();
        var splitDates = $('#canvweek').val().split(" - ");
        var edition = $('#canvedition').val();
        var url = $(this).data("url");
        var downloadUrl = $(this).data("url-download");
        var file = "";
        var fullFile = "";
        var fileName = "";
        
        downloadSalesReports(url, "PDF", edition, splitDates[0], splitDates[1], splitDates[2], function (resultset) {

            for (var i = 0; i < resultset.length; i++) {

                fullFile = resultset[i].FullPath + resultset[i].FileName + resultset[i].Format;
                fileName = resultset[i].FileName + resultset[i].Format;

                file = downloadUrl + "?fullFile=" + fullFile + "&mime=" + resultset[i].Mime + "&fileName=" + fileName;
                window.open(file, "_blank");
            }

            hideColorLoading();
        });
    });

    $("#generate-excel-report").on("click", function () {

        showColorLoading();
        var splitDates = $('#canvweek').val().split(" - ");
        var edition = $('#canvedition').val();
        var url = $(this).data("url");
        var downloadUrl = $(this).data("url-download");
        var file = "";
        var fullFile = "";
        var fileName = "";

        downloadSalesReports(url, "EXCEL", edition, splitDates[0], splitDates[1], splitDates[2], function (resultset) {
            
            for (var i = 0; i < resultset.length; i++) {
              
                fullFile = resultset[i].FullPath + resultset[i].FileName + resultset[i].Format;
                fileName = resultset[i].FileName + resultset[i].Format;

                file = downloadUrl + "?fullFile=" + fullFile + "&mime=" + resultset[i].Mime + "&fileName=" + fileName;
                window.open(file, "_blank");
            }
            
            hideColorLoading();
        });
    });

});

function getGeneratedEditions(func) {

    showColorLoading();

    $.get($('#canvedition').attr("data-url")).done(function (httpResponse) {

        func(httpResponse);
    });
}

function getGeneratedWeekByEdition(edition, url, func) {

    showColorLoading();
    $.get(url, { edition: edition }, function (result) {

        var _data = JSON.parse(result);


        if (_data.length > 0) {

            func(_data);
        }

        hideColorLoading();
    });
}

function generateSalesReport(url, startDate, endDate, func) {

    $.get(url, { startDate: startDate, endDate: endDate }).done(function (httpResponse) {

        func(httpResponse);
    });
}

function generateSalesReportSubscrData(url,employeeId, startDate, endDate, func) {

    $.get(url, { employeeId: employeeId, startDate: startDate, endDate: endDate }).done(function (httpResponse) {
        
        func(JSON.parse(httpResponse));
    });
}

function addRow(table, resultset) {
    
    table.clear().draw();

    var data = {
        digProdCo: 0,
        digProdNew: 0,
        digProdCi: 0,
        printProdCo: 0,
        printProdNew: 0,
        printProdCi: 0,
        totalProdCo: 0,
        totalProdNew: 0,
        totalProdCi: 0,
    }   

    $(resultset).each(function (key, value) {

        data.digProdCo += value.PROD_CO_DIGITAL;
        data.digProdNew += value.NEW_DIGITAL;
        data.digProdCi += value.PROD_CI_DIGITAL;
        data.printProdCo += value.PROD_CO_PRINT;
        data.printProdNew += value.NEW_PRINT;
        data.printProdCi += value.PROD_CI_PRINT;
        data.totalProdCo += value.PROD_CO;
        data.totalProdNew += value.NEW;
        data.totalProdCi += value.PROD_CI;

        table.row.add(
            $("<tr>" +
            "<td>" + value.SUPERVISOR + "</td>" +
            "<td><a  class='btn btn-default btn-labeled far fa-file-excel sales-employee-details' >" + value.EMPLOYEE_ID + "</a></td>" +
            "<td>" + value.REPRESENTATIVE + "</td>" +

            "<td class='dig-prod-ci'>" + value.PROD_CO_DIGITAL.toLocaleString(undefined, { maximumFractionDigits: 2 }) + "</td>" +
            "<td class='dig-prod-new'>" + value.NEW_DIGITAL.toLocaleString(undefined, { maximumFractionDigits: 2 }) + "</td>" +
            "<td class='dig-prod-co'>" + value.PROD_CI_DIGITAL.toLocaleString(undefined, { maximumFractionDigits: 2 }) + "</td>" +
           
            "<td class='print-prod-ci'>" + value.PROD_CO_PRINT.toLocaleString(undefined, { maximumFractionDigits: 2 }) + "</td>" +
            "<td class='print-prod-new'>" + value.NEW_PRINT.toLocaleString(undefined, { maximumFractionDigits: 2 }) + "</td>" +
            "<td class='print-prod-co'>" + value.PROD_CI_PRINT.toLocaleString(undefined, { maximumFractionDigits: 2 }) + "</td>" +
            
            "<td class='total-prod-ci'>" + value.PROD_CO.toLocaleString(undefined, { maximumFractionDigits: 2 }) + "</td>" +
            "<td class='total-prod-new'>" + value.NEW.toLocaleString(undefined, { maximumFractionDigits: 2 }) + "</td>" +
            "<td class='total-prod-co'>" + value.PROD_CI.toLocaleString(undefined, { maximumFractionDigits: 2 }) + "</td>" +
            "</tr>"
        )).draw(false);
    });

    var tb = $('#tbPivotReport');
    $("#dig-prod-co", tb).text(data.digProdCo.toLocaleString(undefined, { maximumFractionDigits: 2 }));
    $("#dig-prod-new", tb).text(data.digProdNew.toLocaleString(undefined, { maximumFractionDigits: 2 }));
    $("#dig-prod-ci", tb).text(data.digProdCi.toLocaleString(undefined, { maximumFractionDigits: 2 }));
    $("#print-prod-co", tb).text(data.printProdCo.toLocaleString(undefined, { maximumFractionDigits: 2 }));
    $("#print-prod-new", tb).text(data.printProdNew.toLocaleString(undefined, { maximumFractionDigits: 2 }));
    $("#print-prod-ci", tb).text(data.printProdCi.toLocaleString(undefined, { maximumFractionDigits: 2 }));
    $("#total-prod-co", tb).text(data.totalProdCo.toLocaleString(undefined, { maximumFractionDigits: 2 }));
    $("#total-prod-new", tb).text(data.totalProdNew.toLocaleString(undefined, { maximumFractionDigits: 2 }));
    $("#total-prod-ci", tb).text(data.totalProdCi.toLocaleString(undefined, { maximumFractionDigits: 2 }));
}

function addSubscrDetailsRow(table, resultset) {

    table.clear().draw();

    var data = {
        digProdCo: 0,
        digProdNew: 0,
        digProdCi: 0,
        printProdCo: 0,
        printProdNew: 0,
        printProdCi: 0,
        totalProdCo: 0,
        totalProdNew: 0,
        totalProdCi: 0,
    }

    $(resultset).each(function (key, value) {

        table.row.add(
            $("<tr>" +
            "<td>" + value.SUBSCR_ID + "</td>" +
            "<td>" + value.NAME + "</td>" +
            "<td>" + value.CANV_WEEK + "</td>" +
            "<td>" + value.CANV_CODE + "</td>" +
            "<td>" + value.CANV_EDITION + "</td>" +
            "<td>" + value.PROD_GRP_CODE + "</td>" +
            "<td>" + value.PROD_CO.toLocaleString(undefined, { maximumFractionDigits: 2 }) + "</td>" +
            "<td>" + value.PROD_CI.toLocaleString(undefined, { maximumFractionDigits: 2 }) + "</td>" +
            "<td>" + value.RENEW_CI.toLocaleString(undefined, { maximumFractionDigits: 2 }) + "</td>" +
            "<td>" + value.DECREASE_CI.toLocaleString(undefined, { maximumFractionDigits: 2 }) + "</td>" +
            "<td>" + value.NEW.toLocaleString(undefined, { maximumFractionDigits: 2 }) + "</td>" +
            "<td>" + value.CTRL_LOSS.toLocaleString(undefined, { maximumFractionDigits: 2 }) + "</td>" +
            "<td>" + value.UNCTRL_LOSS.toLocaleString(undefined, { maximumFractionDigits: 2 }) + "</td>" +
            "</tr>"
        )).draw(false);
    });

}

function downloadSalesReports(url, reportType, edition ,canvWeek , startDate,endDate, func) {

    $.get(url, { reportType: reportType, edition: edition, canvWeek: canvWeek, startDate: startDate, endDate: endDate }).done(function (httpResponse) {

        func(httpResponse);
    });
}