<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.9.3/Chart.js"></script>
    var ctx = document.querySelector("#revenue-linechart");
    var chart1 = new Chart(ctx, {
        type: "line",
        data: {
            labels: ['Jan', 'Feb', 'Mar', 'Apr', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
            datasets: [{
                label: "Xboox",
                fill: false,
                backgroundColor: 'rgba(153,50,204,0.3)',
                borderColor: 'rgb(123,55,190)',
                pointStyle: "circle",
                pointBackgroundColor: 'rgb(220,20,60)',
                pointRadius: 5,
                pointHoverRadius: 10,
                data: []
            }]
        },
        options: {
            responsive: true,
            title: {
                display: true,
                fontSize: 26,

            },
            tooltips: {
                mode: 'point',
                intersect: true,
            },
            hover: {
                mode: 'nearest',
                intersect: true
            },
            scales: {
                xAxes: [{
                    display: true,
                    scaleLabel: {
                        display: true,
                        labelString: '月份',
                        fontSize: 15
                    },
                    ticks: {
                        fontSize: 15
                    }
                }],
                yAxes: [{
                    display: true,
                    scaleLabel: {
                        display: true,
                        labelString: 'Million(NTD)',
                        fontSize: 15
                    },
                    ticks: {
                        fontSize: 15
                    }
                }]
            },
            animation: {
                duration: 2000
            }
        }
    });
