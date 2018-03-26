var tbHistDetail, canvass, repSalesList, prodGrpList, weekList;

$(document).ready(function () {
    getCanvass();

    $("#btnSearch").click(function (e) {
        e.preventDefault();
        $("#tbTransactions").show();
        getHistReport();
        getRepSales();
        getProdGrp();
        getActiveCanvWeek();
    });

    $("input.chkDB").change(function () {
        setCanvass(canvass);
    });

    $("#btnAdd").click(function (e) {
        e.preventDefault;
        insertTransaction();
    })
})

function getHistReport() {

    var $el = showLoading($("#demo-panel-ref"));

    $.get($('#tbHistDetail').data("url"), {
        "edition": $("#ddlEdition").val(), "canvCode": $('#ddlCanvCode').val(),
        "db": $("input[name='db']:checked").val(), "subscrID": $('#txtCustID').val()
    }, function (report) {

        getHistTable(report);

    });

    hideLoading($el);
}

function getHistTable(report) {

    var rowNum = 1;

    if (!$.fn.DataTable.isDataTable('#tbHistDetail')) {

        tbHistDetail = $('#tbHistDetail').DataTable({
            "order": [[2, "desc"]]
        });
        relocatePag_Filt('#tbHistDetail');

    }
    tbHistDetail.clear().draw();

    $.each(report, function (i, d) {

        tbHistDetail.row.add([
            '<a href="#" id="rep' + rowNum + '" data-type="select" data-placement="right" class="repEditable" data-title="Seleccione el Representante">' + d.REPRESENTATIVE + '</a>',
            '<a href="#" id="grpProd' + rowNum + '" data-type="select" data-placement="right" class="prodGrpEditable" data-title="Seleccione el Group Code">' + d.PROD_GRP_CODE + '</a>',
            '<a href="#" id="week' + rowNum + '" data-type="select" data-placement="right" class="activeWeek" data-title="Seleccione la Semana">' + d.CANV_WEEK + '</a>',
             '<a href="#" id="assig' + rowNum + '" data-type="number" data-placement="right" class="editable" data-title="Seleccione la asignación">' + d.ASSIGNMENT_NO + '</a>',
             '<a href="#" id="prodCo' + rowNum + '" data-type="number" data-placement="right" class="editable" data-title="Introduzca el Prod CO">' + d.PROD_CO + '</a>',
             '<a href="#" id="prodCi' + rowNum + '" data-type="number" data-placement="right" class="editable" data-title="Introduzca el Prod CI">' + d.PROD_CI + '</a>',
             '<a href="#" id="renewCi' + rowNum + '" data-type="number" data-placement="right" class="editable" data-title="Introduzca el Renewe CI">' + d.RENEW_CI + '</a>',
             '<a href="#" id="increase' + rowNum + '" data-type="number" data-placement="right" class="editable" data-title="Introduzca el Increase CI">' + d.INCREASE_CI + '</a>',
             '<a href="#" id="decrease' + rowNum + '" data-type="number" data-placement="right" class="editable" data-title="Introduzca el Decrease CI">' + d.DECREASE_CI + '</a>',
             '<a href="#" id="ctrlLoss' + rowNum + '" data-type="number" data-placement="right" class="editable" data-title="Introduzca el Ctrl Loss">' + d.CTRL_LOSS + '</a>',
             '<a href="#" id="unctrlLoss' + rowNum + '" data-type="number" data-placement="right" class="editable" data-title="Introduzca el Unctrl Loss">' + d.UNCTRL_LOSS + '</a>',
             '<a href="#" id="op' + rowNum + '" data-type="number" data-placement="right" class="editable" data-title="Introduzca el OP CI">' + d.OP_CI + '</a>',
             '<a href="#" id="np' + rowNum + '" data-type="number" data-placement="right" class="editable" data-title="Introduzca el NP CI">' + d.NP_CI + '</a>',
              '<a href="#" id="otc' + rowNum + '" data-type="number" data-placement="right" class="editable" data-title="Introduzca el OTC CI">' + d.OTC_CI + '</a>',
             '<div class="btn-group"><button name="btnEdit" data-id="' + rowNum + '" data-key="' + d.KEY + '"  class="btn btn-default btn-sm btn-icon icon-lg fa fa-pencil" style="color:#5fa2dd !important"></button>' +
             '<button name="btnDelete" data-id="' + rowNum + '"  data-key="' + d.KEY + '" class="btn btn-default btn-sm btn-icon icon-lg fa fa-trash text-danger" style="color:#f76c51 !important"></button></div>'
        ]).draw().nodes().to$().addClass(d.STATUS);

        rowNum++;
    });

    getEditable();
    
    getRepresentativeEditable();
    getProdGrpEditable();
    getActiveWeekEditable();
    disableRows();

    $('button[name="btnEdit"]').click(function (e) {
        e.preventDefault;

        var rowID = $(this).data("id"),
            key = $(this).data("key");
        
        bootbox.confirm("Se modificará esta transacción, ¿está seguro que desea realizar esta acción?", function (result) {
            if (result) {
                updateTransaction(rowID, key)
            }
        });
    });

    $('button[name="btnDelete"]').click(function (e) {
        e.preventDefault;
        var rowID = $(this).data("id"),
              row = $(this).parents('tr'),
            key = $(this).data("key");

        bootbox.confirm("Se eliminará esta transacción, ¿está seguro que desea realizar esta acción?", function (result) {
            if (result) {
                deleteTransaction(rowID, row, key)
            }
        });


    });
}

function getCanvass() {

    $.get($('#ddlCanvCode').data("url"), function (canv_codes) {

        canvass = canv_codes;

        setCanvass(canv_codes);

    });
}

function setCanvass(canvList) {

    $('#ddlCanvCode')
    .find('option')
    .remove()
    .end();

    $(_.where(canvList, { DB: $("input[name='db']:checked").val() })).each(function (index, value) {
        var $option = $("<option/>").attr("value", value.CANV_CODE).text(value.CANV_CODE);
        $('#ddlCanvCode').append($option);
    });

    $('#ddlCanvCode')
          .selectpicker('refresh');
}

function getRepSales() {

    $.get($('#ddlRepSales').data("url"), function (repSales) {

        repSalesList = repSales;

        $('#ddlRepSales')
    .find('option')
    .remove()
    .end();


        $(repSales).each(function (index, value) {
            var $option = $("<option/>").attr("value", value.TARJETA).text(value.EJECUTIVO);
            $('#ddlRepSales').append($option);
        });

        $('#ddlRepSales')
              .selectpicker('refresh');

    });
}

function getProdGrp() {

    $.get($('#ddlProduct').data("url"), function (product) {

        $('#ddlProduct')
        .find('option')
        .remove()
        .end();

        prodGrpList = product;

        $(product).each(function (index, value) {
            var $option = $("<option/>").attr("value", value.PRODUCTOS).text(value.PRODUCTOS);
            $('#ddlProduct').append($option);
        });

        $('#ddlProduct')
              .selectpicker('refresh');

    });
}

function getActiveCanvWeek() {

    $.get($('#ddlActiveWeek').data("url"), { "canvEdition": $("#ddlEdition").val(), "canvCode": $('#ddlCanvCode').val(), "db": $("input[name='db']:checked").val() }, function (weeks) {

        weekList = weeks;
        $('#ddlActiveWeek')
        .find('option')
        .remove()
        .end();

        $(weeks).each(function (index, value) {
            var $option = $("<option/>").attr("value", value.CANV_WEEK).text(value.CANV_WEEK);
            $('#ddlActiveWeek').append($option);
        });

        $('#ddlActiveWeek')
              .selectpicker('refresh');

    });
}


// Actions
// =================================================================
function insertTransaction() {

    var $el = showLoading($("#newTrans-body"));

    var CommisionHistTrans = {
        "Subscr_ID": $('#txtCustID').val(), "Prod_Grp_Code": $('#ddlProduct').val(),
        "Canv_Code": $('#ddlCanvCode').val(), "Canv_Edition": $('#ddlEdition').val(),
        "Canv_Week": $('#ddlActiveWeek').val(), "Employee_id": $('#ddlRepSales').val(),
        "Prod_Co": $('#txtProdCo').val(), "Prod_Ci": $('#txtProdCi').val(),
        "Renew_Ci": $('#txtRenewCi').val(), "Increase_Ci": $('#txtIncrease').val(),
        "Decrease_Ci": $('#txtDecrease').val(), "Np_Ci": $('#txtNpCi').val(),
        "Op_Ci": $('#txtOpCi').val(), "Otc_Ci": $('#txtOtcCi').val(),
        "Ctrl_Loss": $('#txtCtrlLoss').val(), "Unctrl_Loss": $('#txtUnctrlLoss').val(),
        "DB": $("input[name='db']:checked").val()
    };

    $.get($("#btnAdd").data('url'), CommisionHistTrans, function (report) {

        //Close Modal
        $('#newTrans').modal('toggle');
        //Get Comission Report
        getHistTable(report);
        getTimerDialogMsg('success', 'Se ingresó la transacción exitosamente', 'histDetailPanel-body', 3000);
        $el.niftyOverlay('hide');

    }).fail(function (ex) {

        getTimerDialogMsg('danger', 'No se pudo insertar la transacción, favor validar los datos insertados.', 'newTrans-body', 3000);
        $el.niftyOverlay('hide');

    });
}

function updateTransaction(rowID, key) {

    var $el = showLoading($("#histDetailPanel-body"));

    var CommisionHistTrans = {
        "Subscr_ID": $('#txtCustID').val(), "Prod_Grp_Code": $("#grpProd" + rowID).text(),
        "Canv_Code": $('#ddlCanvCode').val(), "Canv_Edition": $('#ddlEdition').val(),
        "Assigment_No": $("#assig" + rowID).text(), "Canv_Week": $("#week" + rowID).text(),
        "Employee_Name": $("#rep" + rowID).text(), "Prod_Co": $("#prodCo" + rowID).text(),
        "Prod_Ci": $("#prodCi" + rowID).text(), "Renew_Ci": $("#renewCi" + rowID).text(),
        "Increase_Ci": $("#increase" + rowID).text(), "Decrease_Ci": $("#decrease" + rowID).text(),
        "Np_Ci": $("#np" + rowID).text(), "Op_Ci": $("#op" + rowID).text(),
        "Otc_Ci": $("#otc" + rowID).text(), "Ctrl_Loss": $("#ctrlLoss" + rowID).text(),
        "Unctrl_Loss": $("#unctrlLoss" + rowID).text(), "Key": key, "DB": $("input[name='db']:checked").val()
    };

    $.get($("#tbHistDetail").data('urledit'), CommisionHistTrans, function (data) {
       
        getHistReport();
        getTimerDialogMsg('success', data, 'histDetailPanel-body', 3000);
        $el.niftyOverlay('hide');


    }).fail(function (ex) {
        getTimerDialogMsg('danger', 'No se pudo modificar la transacción.', 'histDetailPanel-body', 3000);
        $el.niftyOverlay('hide');
    });
}

function deleteTransaction(rowID, tr, key) {

    var $el = showLoading($("#histDetailPanel-body"));
    var row = tr;

    var CommisionHistTrans = {
        "Key": key, "DB": $("input[name='db']:checked").val()
    };

    $.get($("#tbHistDetail").data('urldelete'), CommisionHistTrans, function (data) {

        if (data.split("-")[0].trim() != "Error") {
            tbHistDetail.row(row).remove().draw();
            getTimerDialogMsg('success', data, 'histDetailPanel-body', 3000);
        }
        else {
            getTimerDialogMsg('danger', data, 'histDetailPanel-body', 3000);
        }
        $el.niftyOverlay('hide');

    }).fail(function (ex) {
        getTimerDialogMsg('danger', 'No se pudo eliminar la transacción.', 'histDetailPanel-body', 3000);
        $el.niftyOverlay('hide');
    });
}


// Editables
// =================================================================
function getRepresentativeEditable() {

    var list = [];
    $(repSalesList).each(function (i, v) {
        list.push({ value: v.TARJETA, text: v.EJECUTIVO });
    })

    $('.repEditable').editable({
        value: 2,
        source: list
    });
}

function getProdGrpEditable() {

    var list = [];
    $(prodGrpList).each(function (i, v) {
        list.push({ value: v.PRODUCTOS, text: v.PRODUCTOS });
    })

    $('.prodGrpEditable').editable({
        source: list
    });
}

function getActiveWeekEditable() {
    var list = [];

    $(weekList).each(function (i, v) {
        list.push({ value: v.CANV_WEEK, text: v.CANV_WEEK });
    })

    $('.activeWeek').editable({
        source: list
    });
}

function disableRows() {
    $('#tbHistDetail tr.CLOSE').css('background-color', "#F5F5F5");
    $('#tbHistDetail tr.CLOSE .editable').editable('toggleDisabled');
    $('#tbHistDetail tr.CLOSE button').hide();

}

// CIRCULAR FORM WIZARD
// =================================================================
$('#add_trans_steps').bootstrapWizard({
    tabClass: 'wz-steps',
    nextSelector: '.next',
    previousSelector: '.previous',
    onTabClick: function (tab, navigation, index) {
        return false;
    },
    onInit: function () {
        $('#add_trans_steps').find('.finish').hide().prop('disabled', true);
    },
    onTabShow: function (tab, navigation, index) {
        var $total = navigation.find('li').length;
        var $current = index + 1;
        var $percent = (index / $total) * 100;
        var wdt = 100 / $total;
        var lft = wdt * index;
        var margin = (100 / $total) / 2;
        $('#add_trans_steps').find('.progress-bar').css({ width: $percent + '%', 'margin': 0 + 'px ' + margin + '%' });


        // If it's the last tab then hide the last button and show the finish instead
        if ($current >= $total) {
            $('#add_trans_steps').find('.next').hide();
            $('#add_trans_steps').find('.finish').show();
            $('#add_trans_steps').find('.finish').prop('disabled', false);
        } else {
            $('#add_trans_steps').find('.next').show();
            $('#add_trans_steps').find('.finish').hide().prop('disabled', true);
        }
    }
});