
function getChildTable(childTable, rowParent) {

    var rowNum = 1;

    var tableResl2 = '', tableResl = '<div><table  class="display table table-hover table-vcenter table-bordered table-child" cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">' +
        '<thead><th class="export">Fecha</th><th>Subscr ID</th><th>Canv Code</th><th>Canv Edition</th><th>Cargo</th><th>Venta</th><th>Ejecutivo</th><th class="export">Razon</th><th>Comentario</th><th>status</th><th>Fecha Aceptada</th><th>Aceptada Por</th><th>Digitador</th></thead>';

    $.each(_.where(childTable, { SUBSCR_ID: rowParent[1], CANV_EDITION: rowParent[3] }), function (i, d) {
        tableResl2 = tableResl2 + '<tr>' +
                  '<td>' + d.REP_SALES_DATE + '</td>' +
                  '<td>' + d.SUBSCR_ID + '</td>' +
                  '<td>' + d.CANV_CODE + '</td>' +
                  '<td>' + d.CANV_EDITION + '</td>' +
                  '<td>' + d.REP_CARGO + '</td>' +
                  '<td>' + d.NEW_ISSUE + '</td>' +
                  '<td>' + d.EJECUTIVO + '</td>' +
                  '<td>' + d.RAZON + '</td>' +
                  '<td>' + d.REP_COMENTARIO + '</td>' +
                  '<td>' + d.STS_CODIGO + '</td>' +
                  '<td>' + d.REP_CLOSEDATE + '</td>' +
                   '<td>' + d.USR_CLOSEPUB + '</td>' +
                  '<td>' + d.DIGITADOR + '</td>' +
                  '</tr>';

        rowNum++;
    });


    tableResl = tableResl + tableResl2 + '</table></div>';

    return tableResl;
}