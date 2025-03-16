
let chartCompanies = [];
let chartTimes = [];
let chartParties = [];
let chartType = 1;
let chart = null, chart2 = null;
var months = 'Ocak_Şubat_Mart_Nisan_Mayıs_Haziran_Temmuz_Ağustos_Eylül_Ekim_Kasım_Aralık'.split('_');
window.addEventListener('DOMContentLoaded', (event) => {
    let parties = document.querySelectorAll(".chart-party");

    parties.forEach((item) => {
        if (item.checked)
            chartParties.push(item.value);
        item.addEventListener('click', partyChecked)
    });

    document.querySelectorAll(".chart-company").forEach((item) => {
        if (item.checked)
            chartCompanies.push(item.value);
        item.addEventListener('click', companyChecked)
    });
    document.querySelectorAll(".time-company").forEach((item) => {
        if (item.checked)
            chartTimes.push(item.value);
        item.addEventListener('click', timeChecked)
    });


    getData();


});


partyChecked = (e) => {
    console.log(e.target.value);
}

companyChecked = (e) => {
    console.log(e.target.value);
}

timeChecked = (e) => {
    console.log(e.target.value);
}



function getData() {
    let url = '';
    let dates = [];
    let parties = [];
    let companies = [];
    chartType = $('#ltDate').is(':checked') ? 1 : $('#ltCompany').is(':checked') ? 2 : 3;
    console.log(chartType);
    switch (chartType) {
        case 1:

            dates.push($('#ddlDate').val());
            $('#widgetParty').find('input').each((e, i) => {

                if ($(i).is(':checked'))
                    parties.push($(i).val());
            });
            $('#widgetCompany').find('input').each((e, i) => {
                if ($(i).is(':checked'))
                    companies.push($(i).val());
            });

            if (parties.length == 0) {
                Swal.fire('En az bir parti seçilmelidir');
                return;
            }
            if (companies.length == 0) {
                Swal.fire('En az bir anket şirketi seçilmelidir');
                return;
            }
            url = `/Polls/GetData?t=${chartType}&dates=${dates.join(",")}&parties=${parties.join(",")}&companies=${companies.join(",")}`;
            //$.get('/Polls/GetData', { t: 1, 'dates[]': dates, 'parties[]': parties, 'companies[]': companies }).done((r) => {
            //    //makeChartType1(r);
            //});
            $.ajax({
                type: 'get',
                url: '/Polls/GetData',
                data: { t: 1, dates: dates, parties: parties, companies: companies },
                traditional: true,
                success: (r) => { makeChartType1(r); }
            });

            break;

        case 2://şirketlere göre
            console.log('şirkete göre çek');
            companies.push($('#ddlCompany').val());

            $('#widgetParty').find('input').each((e, i) => {

                if ($(i).is(':checked'))
                    parties.push($(i).val());
            });
            $('#widgetDate').find('input').each((e, i) => {
                if ($(i).is(':checked'))
                    dates.push($(i).val());
            });

            if (parties.length == 0) {
                Swal.fire('En az bir parti seçilmelidir');
                return;
            }
            if (dates.length == 0) {
                Swal.fire('En az bir anket tarihi seçilmelidir');
                return;
            }

            $.ajax({
                type: 'get',
                url: '/Polls/GetData',
                data: { t: 2, dates: dates, parties: parties, companies: companies },
                traditional: true,
                success: (r) => { makeChartType2(r); }
            });

            break;

        case 3://partilere göre
            
            parties.push($('#ddlParty').val());

            $('#widgetCompany').find('input').each((e, i) => {

                if ($(i).is(':checked'))
                    companies.push($(i).val());
            });
            $('#widgetDate').find('input').each((e, i) => {
                if ($(i).is(':checked'))
                    dates.push($(i).val());
            });

            if (companies.length == 0) {
                Swal.fire('En az bir anket şirketi seçilmelidir');
                return;
            }

            if (dates.length == 0) {
                Swal.fire('En az bir anket tarihi seçilmelidir');
                return;
            }

            $.ajax({
                type: 'get',
                url: '/Polls/GetData',
                data: { t: 3, dates: dates, parties: parties, companies: companies },
                traditional: true,
                success: (r) => { makeChartType3(r); }
            });

            break;

    }




}

function makeChartType1(data) {
    if (chart)
        chart.destroy();
    if (chart2) {
        chart2.destroy();
        chart2 = null;
    }
    var dates = $('#ddlDate').val().split('-');
    var monthName = months[dates[1] - 1];

    var parties = [];
    for (var i = 0; i < data.length; i++) {
        for (var j = 0; j < data[i].results.length; j++) {
            var p = parties.find((p) => p.id == data[i].results[j].partyId);
            if (!p)// parties.length==0 || 
                parties.push({ id: data[i].results[j].partyId, name: data[i].results[j].partyShortName, total: data[i].results[j].ratio, avg: 0, count: data[i].results[j].ratio == 0 ? 0 : 1 });
            else {
                p.total += data[i].results[j].ratio;
                p.count += data[i].results[j].ratio == 0 ? 0 : 1;
            }
        }

    }
    for (var i = 0; i < parties.length; i++) {
        parties[i].avg = parties[i].total / parties[i].count;
    }


    var optimalColumnWidthPercent = 20 + (60 / (1 + 30 * Math.exp(-data.length / 3)));

    var options = {
        //series: [{
        //    name: 'Net Profit',
        //    data: [44, 55, 57, 56, 61, 58, 63, 60, 66]
        //}, {
        //    name: 'Revenue',
        //    data: [76, 85, 101, 98, 87, 105, 91, 114, 94]
        //}, {
        //    name: 'Free Cash Flow',
        //    data: [35, 41, 36, 26, 45, 48, 52, 53, 41]
        //}],
        series: [],
        chart: {
            type: 'bar',
            height: 60 + data.length * 24 * parties.length
        },

        plotOptions: {
            bar: {
                horizontal: true,
                borderRadius: 2,
                //columnWidth: optimalColumnWidthPercent+'%',
                barHeight: '80%',
                dataLabels: {
                    position: 'top', // top, center, bottom
                },
            }
        },
        dataLabels: {
            enabled: true,
            formatter: function (val) {
                return "%" + val;
            },
            offsetY: 0,
            offsetX: 12,
            textAnchor: 'start',
            style: {
                fontSize: '12px',
                colors: ["#304758"]
            }
        },

        stroke: {
            show: true,
            width: 2,
            colors: ['transparent']
        },
        xaxis: {
            categories: []// ['Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct'],
        },
        yaxis: {
            title: {
                text: 'Oran (%)'
            }
        },
        fill: {
            opacity: 1
        },
        tooltip: {
            y: {
                formatter: function (val) {
                    return "%" + val;
                }
            }
        },
        title: {
            text: dates[0] + " " + monthName + " Anketleri",
            align: 'center',
            margin: 15,
            offsetX: 0,
            offsetY: 0,
            floating: false,
            style: {
                fontSize: '16px',
                fontWeight: 'bold',
                fontFamily: undefined,
                color: '#263238'
            },
        }


    };
    for (var i = 0; i < parties.length; i++) {
        options.series.push({ name: parties[i].name, data: [], id: parties[i].id });
    }


    for (var i = 0; i < data.length; i++) {
        options.xaxis.categories.push(data[i].companyShortName);

        for (var j = 0; j < options.series.length; j++) {
            var r = data[i].results.find((p) => p.partyId == options.series[j].id);
            if (r)
                options.series[j].data.push(r.ratio);
            else
                options.series[j].data.push(0);

        }
    }
    chart = new ApexCharts(document.querySelector("#chart"), options);
    chart.render();

    parties.sort((a, b) => b.avg - a.avg);
    console.log(parties);
    if (data.length > 1) {
        var ps = parties.slice(0, 8);
        var total = 0;
        for (var i = 0; i < ps.length; i++) {
            total += ps[i].avg;
        }
        var other = 100 - total;

        var options2 = {
            series: [],
            chart: {
                width: '100%',
                type: 'pie',
            },
            labels: [],
            responsive: [{
                breakpoint: 480,
                options: {
                    chart: {
                        width: 200
                    },
                    legend: {
                        position: 'bottom'
                    }
                }
            }],
            title: {
                text: dates[0] + " " + monthName + " Anket Ortalaması",
                align: 'center',
                margin: 15,
                offsetX: 0,
                offsetY: 0,
                floating: false,
                style: {
                    fontSize: '16px',
                    fontWeight: 'bold',
                    fontFamily: undefined,
                    color: '#263238'
                },
            }
        };
        for (var i = 0; i < ps.length; i++) {
            options2.series.push(Math.round(ps[i].avg * 10) / 10.0);
            options2.labels.push(ps[i].name);

        }
        if (other > 0) {
            options2.series.push(Math.round(other * 10) / 10);
            options2.labels.push("Diğer");
        }
        chart2 = new ApexCharts(document.querySelector("#chart2"), options2);
        chart2.render();
    }

}

//şirketlere göre chart
function makeChartType2(data) {
    if (chart)
        chart.destroy();
    if (chart2) {
        chart2.destroy();
        chart2 = null;
    }
    var companyid = parseInt( $('#ddlCompany').val());
    var companyName = $('#ddlCompany option:selected').text();
    var pollDates = [];
    var parties = [];
    for (var i = 0; i < data.length; i++) {
        pollDates.push(data[i].year + " " + months[data[i].month]);
        for (var j = 0; j < data[i].results.length; j++) {
            var p = parties.find((p) => p.id == data[i].results[j].partyId);
            if (!p)// parties.length==0 || 
                parties.push({ id: data[i].results[j].partyId, name: data[i].results[j].partyShortName, total: data[i].results[j].ratio, avg: 0, count: data[i].results[j].ratio == 0 ? 0 : 1 });
            else {
                p.total += data[i].results[j].ratio;
                p.count += data[i].results[j].ratio == 0 ? 0 : 1;
            }
        }

    }
    for (var i = 0; i < parties.length; i++) {
        parties[i].avg = parties[i].total / parties[i].count;
    }


    var optimalColumnWidthPercent = 20 + (60 / (1 + 30 * Math.exp(-data.length / 3)));

    var options = {
        //series: [{
        //    name: 'Net Profit',
        //    data: [44, 55, 57, 56, 61, 58, 63, 60, 66]
        //}, {
        //    name: 'Revenue',
        //    data: [76, 85, 101, 98, 87, 105, 91, 114, 94]
        //}, {
        //    name: 'Free Cash Flow',
        //    data: [35, 41, 36, 26, 45, 48, 52, 53, 41]
        //}],
        series: [],
        chart: {
            type: 'bar',
            height: 60 + data.length * 24 * parties.length
        },

        plotOptions: {
            bar: {
                horizontal: true,
                borderRadius: 2,
                //columnWidth: optimalColumnWidthPercent+'%',
                barHeight: '80%',
                dataLabels: {
                    position: 'top', // top, center, bottom
                },
            }
        },
        dataLabels: {
            enabled: true,
            formatter: function (val) {
                return "%" + val;
            },
            offsetY: 0,
            offsetX: 12,
            textAnchor: 'start',
            style: {
                fontSize: '12px',
                colors: ["#304758"]
            }
        },

        stroke: {
            show: true,
            width: 2,
            colors: ['transparent']
        },
        xaxis: {
            categories: []// ['Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct'],
        },
        yaxis: {
            title: {
                text: 'Oran (%)'
            }
        },
        fill: {
            opacity: 1
        },
        tooltip: {
            y: {
                formatter: function (val) {
                    return "%" + val;
                }
            }
        },
        title: {
            text: companyName + " Anketleri",
            align: 'center',
            margin: 15,
            offsetX: 0,
            offsetY: 0,
            floating: false,
            style: {
                fontSize: '16px',
                fontWeight: 'bold',
                fontFamily: undefined,
                color: '#263238'
            },
        }


    };
    for (var i = 0; i < parties.length; i++) {
        options.series.push({ name: parties[i].name, data: [], id: parties[i].id });
    }


    for (var i = 0; i < data.length; i++) {
        options.xaxis.categories.push(data[i].year + " " + months[data[i].month]);

        for (var j = 0; j < options.series.length; j++) {
            var r = data[i].results.find((p) => p.partyId == options.series[j].id);
            if (r)
                options.series[j].data.push(r.ratio);
            else
                options.series[j].data.push(0);

        }
    }
    chart = new ApexCharts(document.querySelector("#chart"), options);
    chart.render();

    parties.sort((a, b) => b.avg - a.avg);
    console.log(parties);
    if (data.length > 1) {
        var ps = parties.slice(0, 8);
        var total = 0;
        for (var i = 0; i < ps.length; i++) {
            total += ps[i].avg;
        }
        var other = 100 - total;

        var options2 = {
            series: [],
            chart: {
                width: '100%',
                type: 'pie',
            },
            labels: [],
            responsive: [{
                breakpoint: 480,
                options: {
                    chart: {
                        width: 200
                    },
                    legend: {
                        position: 'bottom'
                    }
                }
            }],
            title: {
                text: companyName + " Anket Ortalaması",
                align: 'center',
                margin: 15,
                offsetX: 0,
                offsetY: 0,
                floating: false,
                style: {
                    fontSize: '16px',
                    fontWeight: 'bold',
                    fontFamily: undefined,
                    color: '#263238'
                },
            }
        };
        for (var i = 0; i < ps.length; i++) {
            options2.series.push(Math.round(ps[i].avg * 10) / 10.0);
            options2.labels.push(ps[i].name);

        }
        if (other > 0) {
            options2.series.push(Math.round(other * 10) / 10);
            options2.labels.push("Diğer");
        }
        chart2 = new ApexCharts(document.querySelector("#chart2"), options2);
        chart2.render();
    }

}
//şirketlere göre chart
function makeChartType3(data) {
    if (chart)
        chart.destroy();
    if (chart2) {
        chart2.destroy();
        chart2 = null;
    }
    var partyid = parseInt($('#ddlParty').val());
    var partyName = $('#ddlParty option:selected').text();

    var pollDates = [];
    var companies = [];


    for (var i = 0; i < data.length; i++) {

        var d1 = pollDates.find( d => d.year == data[i].year && d.month == data[i].month);
        var c1 = companies.find(c => c.id == data[i].companyId);
        if (!c1) {
            c1 = { id: data[i].companyId, name: data[i].companyShortName, avg: 0, total: 0, count: 0 };
            companies.push(c1);
        }
        if (!d1) {
            d1 = { year: data[i].year, month: data[i].month, name:data[i].year + ' ' + months[ data[i].month-1],avg:0,total:0,count:0 };
            pollDates.push(d1);
        }

        for (var j = 0; j < data[i].results.length; j++) {
            d1.count += data[i].results[j].ratio == 0 ? 0 : 1;
            d1.total += data[i].results[j].ratio;
            c1.count += data[i].results[j].ratio == 0 ? 0 : 1;
            c1.total += data[i].results[j].ratio;


        }

    }
    for (var i = 0; i < pollDates.length; i++) {
        pollDates[i].avg = pollDates[i].total / pollDates[i].count;
    }
    for (var i = 0; i < companies.length; i++) {
        companies[i].avg = companies[i].total / companies[i].count;
    }
    

    var optimalColumnWidthPercent = 20 + (60 / (1 + 30 * Math.exp(-data.length / 3)));

    var options = {
        //series: [{
        //    name: 'Net Profit',
        //    data: [44, 55, 57, 56, 61, 58, 63, 60, 66]
        //}, {
        //    name: 'Revenue',
        //    data: [76, 85, 101, 98, 87, 105, 91, 114, 94]
        //}, {
        //    name: 'Free Cash Flow',
        //    data: [35, 41, 36, 26, 45, 48, 52, 53, 41]
        //}],
        series: [],
        chart: {
            type: 'bar',
            height: 60 + pollDates.length * 24 * companies.length
        },

        plotOptions: {
            bar: {
                horizontal: true,
                borderRadius: 2,
                //columnWidth: optimalColumnWidthPercent+'%',
                barHeight: '80%',
                dataLabels: {
                    position: 'top', // top, center, bottom
                },
            }
        },
        dataLabels: {
            enabled: true,
            formatter: function (val) {
                return "%" + val;
            },
            offsetY: 0,
            offsetX: 12,
            textAnchor: 'start',
            style: {
                fontSize: '12px',
                colors: ["#304758"]
            }
        },

        stroke: {
            show: true,
            width: 2,
            colors: ['transparent']
        },
        xaxis: {
            categories: []// ['Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct'],
        },
        yaxis: {
            title: {
                text: 'Oran (%)'
            }
        },
        fill: {
            opacity: 1
        },
        tooltip: {
            y: {
                formatter: function (val) {
                    return "%" + val;
                }
            }
        },
        title: {
            text: partyName + " Anketleri",
            align: 'center',
            margin: 15,
            offsetX: 0,
            offsetY: 0,
            floating: false,
            style: {
                fontSize: '16px',
                fontWeight: 'bold',
                fontFamily: undefined,
                color: '#263238'
            },
        }


    };
    for (var i = 0; i < companies.length; i++) {
        options.series.push({ name: companies[i].name, data: [], id: companies[i].id });
    }

    for (var i = 0; i < pollDates.length; i++) {
        options.xaxis.categories.push(pollDates[i].name);

        for (var j = 0; j < options.series.length; j++) {
            var r = data.find((p) =>p.month==pollDates[i].month && p.year==pollDates[i].year &&  p.companyId == options.series[j].id);
            if (r)
                options.series[j].data.push(r.results.length>0? r.results[0].ratio:0);
            else
                options.series[j].data.push(0);

        }
    }

    chart = new ApexCharts(document.querySelector("#chart"), options);
    chart.render();

    
    
    if (pollDates.length > 0) {
        var options2 = {
            series: [{
                name: 'Oran',
                data: []
            }],
            chart: {
                height: 350,
                type: 'line',
            },

            dataLabels: {
                enabled: true,
                formatter: function (val) {
                    return val + "%";
                },
                offsetY: -10,
                style: {
                    fontSize: '12px',
                    colors: ["#304758"]
                }
            },
            grid: {
                row: {
                    colors: ['#f3f3f3', 'transparent'], // takes an array which will be repeated on columns
                    opacity: 0.5
                },
            },
            xaxis: {
                categories: [],
                position: 'bottom',
             
                crosshairs: {
                    fill: {
                        type: 'gradient',
                        gradient: {
                            colorFrom: '#D8E3F0',
                            colorTo: '#BED1E6',
                            stops: [0, 100],
                            opacityFrom: 0.4,
                            opacityTo: 0.5,
                        }
                    }
                },
                tooltip: {
                    enabled: false,
                }
            },
            yaxis: {
                title: {
                    text: 'Oranlar %',
                },
                labels: {
                    show: true,
                    formatter: function (val) {
                        return val + "%";
                    }
                }

            },
            title: {
                text: 'Tüm Anket Ortalama',
                align: 'center',
              
            }
           
        };
        for (var i = 0; i < pollDates.length; i++) {
            options2.series[0].data.push(Math.round( pollDates[i].avg*10)/10);
            options2.xaxis.categories.push(pollDates[i].name);

        }
        chart2 = new ApexCharts(document.querySelector("#chart2"), options2);
        chart2.render();
    }

}



function type_changed(t) {

    if (chart) {
        chart.destroy();
        chart = null;
    }
    if (chart2) {
        chart2.destroy();
        chart2 = null;
    }

    switch (t) {
        case 1://tarihe
            $('#selectDate').show();
            $('#widgetDate').hide();
            $('#selectCompany').hide();
            $('#widgetCompany').show();
            $('#selectParty').hide();
            $('#widgetParty').show();
            chartType = 1;
            $('#infoDate').show();
            break;
        case 2://şirket
            $('#selectDate').hide();
            $('#widgetDate').show();
            $('#selectCompany').show();
            $('#widgetCompany').hide();
            $('#selectParty').hide();
            $('#widgetParty').show();
            chartType = 2;
            $('#infoDate').hide();

            break;
        case 3://parti
            $('#selectDate').hide();
            $('#widgetDate').show();
            $('#selectCompany').hide();
            $('#widgetCompany').show();
            $('#selectParty').show();
            $('#widgetParty').hide();
            chartType = 3;
            $('#infoDate').hide();

            break;

    }
}

function selectAll(t) {
    var checked = false;
    var widget = "#widgetDate";
    switch (t) {
        case 2:
            widget = "#widgetCompany";
            break;
        case 3:
            widget = "#widgetParty";
            break;
    }
    $(widget).find('input').each((e, i) => {
        if (e == 0)
            checked = !$(i).is(':checked');
        $(i).prop('checked', checked);

    });

}