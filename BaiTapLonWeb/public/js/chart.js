const MONTHS = [
    "ThĂ¡ng 1",
    "ThĂ¡ng 2",
    "ThĂ¡ng 3",
    "ThĂ¡ng 4",
    "ThĂ¡ng 5",
    "ThĂ¡ng 6",
    "ThĂ¡ng 7",
    "ThĂ¡ng 8",
    "ThĂ¡ng 9",
    "ThĂ¡ng 10",
    "ThĂ¡ng 11",
    "ThĂ¡ng 12",
];

// Function to create chart for orders by month
function createOrdersChart(ordersByMonth, orderCounts, label) {
    const data1 = {
        labels: MONTHS,
        datasets: [
            {
                label: label,
                data: orderCounts,
                backgroundColor: [
                    "rgba(255, 99, 132, 0.2)",
                    "rgba(255, 159, 64, 0.2)",
                    "rgba(255, 205, 86, 0.2)",
                    "rgba(75, 192, 192, 0.2)",
                    "rgba(54, 162, 235, 0.2)",
                    "rgba(153, 102, 255, 0.2)",
                    "rgba(201, 203, 207, 0.2)",
                    "rgba(255, 99, 132, 0.2)",
                    "rgba(255, 159, 64, 0.2)",
                    "rgba(255, 205, 86, 0.2)",
                    "rgba(75, 192, 192, 0.2)",
                    "rgba(54, 162, 235, 0.2)",
                ],
                borderColor: [
                    "rgb(255, 99, 132)",
                    "rgb(255, 159, 64)",
                    "rgb(255, 205, 86)",
                    "rgb(75, 192, 192)",
                    "rgb(54, 162, 235)",
                    "rgb(153, 102, 255)",
                    "rgb(201, 203, 207)",
                    "rgb(255, 99, 132)",
                    "rgb(255, 159, 64)",
                    "rgb(255, 205, 86)",
                    "rgb(75, 192, 192)",
                    "rgb(54, 162, 235)",
                ],
                borderWidth: 1,
            },
        ],
    };

    const config1 = {
        type: "bar",
        data: data1,
        options: {
            scales: {
                y: {
                    beginAtZero: true,
                },
            },
        },
    };

    var ctx = document.getElementById("myChart").getContext("2d");
    var myChart = new Chart(ctx, config1);
}

// Function to create chart for daily status
