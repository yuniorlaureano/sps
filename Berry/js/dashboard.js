$(function () {

    openCloseWeekChart();
});

function openCloseWeekChart() {

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
                { y: 519960, name: "Abiertas", color: "#E7823A", label: "Abiertas" },
                { y: 363040, name: "Cerradas", color: "#546BC1", label: "Cerradas" },
                { y: 30470, name: "Corridas", color: "#00e600", label: "Corridas" }
            ]
        }]
    })).render();

}