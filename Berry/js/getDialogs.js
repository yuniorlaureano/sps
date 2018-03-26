//function getReactivRecla() {

$(document).ready(function () {
    $('.btnActiv').on('click', function () {
        var button = this;
        bootbox.dialog({
            title: "Reactivación de reclamo",
            message: '<div class="row"> ' + '<div class="col-md-6"> ' +
					'<form class="form-horizontal"> ' + '<div class="form-group"> ' +
					'<label class="col-md-4 control-label" for="dateReactiv">Fecha Inicio</label> ' +
					'<div  id="demo-dp-range" class="col-md-4"> <div class="input-daterange input-group" id="datepicker">' +
					'<input type="date" id="dateStart" class="form-control" name="start" />' +
					'</div></div> ' +
					'</div> ' + '<div class="form-group"> ' +
					'<label class="col-md-4 control-label" for="awesomeness">Ciclo</label> ' +
					'<div class="col-md-8"> <div class="form-block"> ' +
					'<input id="ciclo" type="number" pattern="[0-9]">' +
					'</div> </div>' + '</form> </div> </div><script></script>',

            buttons: {
                "Guardar": function (e) {
                    var dataToSend = $("#dateStart").val();
                    var ciclo = $("#ciclo").val();
                    var programID = $(button).data("programid");
                    var billNo = $(button).data("billno");
                    var edicion = $(button).data("edicion");
                    var sequency = $(button).data("sequency");
                    var canvCode = $(button).data("canvcode");

                    $.ajax({
                        type: "POST",
                        url: "/Home/ActivReclaim ",
                        data: JSON.stringify({
                            startDate: dataToSend,
                            cycle: ciclo,
                            programID: programID,
                            billNo: billNo,
                            edicion: edicion,
                            sequency: sequency,
                            canvCode: canvCode
                        }),
                        contentType: "application/json; charset=utf-8",
                        success: function () {
                            $("#alertError").css("display", "none")
                            $("#alertSuccess").css("display", "block")
                        }
                    }).done(function (msg) {

                    });
                },
                "Cancel": function () {
                }
            }

        });


    });


  

});


