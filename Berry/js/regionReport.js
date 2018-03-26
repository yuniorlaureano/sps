var table = [];

$(document).ready(function () {

    // getPivotReport();

    $('#btnExportReport').click(function (e) {
        e.preventDefault();
        exportBillingReport();
    });

    //$('#ddlCanvassEdition').change(function () {

    //    getPivotReport();
    //});

    //$('#weekRange').change(function () {

    //    getPivotReport();
    //});

})


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