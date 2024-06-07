
let chartCompanies = [];
let chartTimes = [];
let chartParties = [];
let chartType = 1;
let chart = null, chart2 = null;

window.addEventListener('DOMContentLoaded', (event) => {



    //getData();


});


partyChecked = (e) => {
    //console.log(e.target.value);
}

companyChecked = (e) => {
    //console.log(e.target.value);
}

timeChecked = (e) => {
    //console.log(e.target.value);
}



function getData() {
    let url = '';
    let dates = [];
    let parties = [];
    let companies = [];
    let elections = [];

    switch (chartType) {
        case 1:

            $('#widgetDate').find('input').each((e, i) => {           
                if ($(i).is(':checked'))
                    dates.push($(i).val());
            
            });
            
            $('#widgetParty').find('input').each((e, i) => {
                if ($(i).is(':checked'))
                    parties.push($(i).val());
            });

            $('#widgetCompany').find('input').each((e, i) => {
                if ($(i).is(':checked'))
                    companies.push($(i).val());
            });

            $('#widgetElection').find('input').each((e, i) => {
                if ($(i).is(':checked'))
                    elections.push($(i).val());
            });

            if (parties.length == 0) {
                Swal.fire('Tarihe Göre', 'En az bir parti seçilmelidir', 'warning');
                return;
            }
            if (elections.length == 0) {
                Swal.fire('Seçime Göre', 'En az bir seçim seçilmelidir', 'warning');
                return;
            }
            
            if (elections.length > 1 && parties.length > 1) {
                Swal.fire('Seçime Göre', 'Aynı anda birden çok hem parti hem de seçim seçemezsiniz. Seçim veya partiden birisi tek olmalıdır.','warning');
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
            url = `/Polls/GetData?t=${chartType}&dates=${dates.join(",")}&parties=${parties.join(",")}&companies=${companies.join(",")}`;
            //$.get('/Polls/GetData', { t: 1, 'dates[]': dates, 'parties[]': parties, 'companies[]': companies }).done((r) => {
            //    //makeChartType1(r);
            //});
            $.ajax({
                type: 'get',
                url: '/Polls/GetData',
                data: { t: 1, dates: dates, parties: parties, companies: companies },
                traditional: true,
                success: (r) => { makeChartType2(r); }
            });

            break;


        case 3://partilere göre

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
                Swal.fire('En az bir anket şirketi seçilmelidir');
                return;
            }
            if (dates.length == 0) {
                Swal.fire('En az bir anket tarihi seçilmelidir');
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
                success: (r) => { makeChartType3(r); }
            });

            break;

    }




}


function makeChartType1(polls) {
    if (chart)
        chart.destroy();
    if (chart2) {
        chart2.destroy();
        chart2 = null;
    }
    
    //var monthName = months[dates[1] - 1];
    var data = polls.data;
    var parties = [];
    var companies = [];
    for (var i = 0; i < data.length; i++) {
        var c = companies.find((k) => k.id == data[i].companyId);
        if (!c)
            companies.push({ id: data[i].companyId, name: data[i].companyShortName });

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

    var title = "";
    if (parties.length == 1)
        title = parties[0].name + " Anket Sonuçları";
    else
        title = data[0].CompanyName + " Anket Sonuçları";

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
        labels:[],
        chart: {
            type: 'bar',
            height: 60 + data.length * 18 * parties.length
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
            
            formatter: function (val, opt) {
                
                return opt.w.globals.seriesNames[opt.seriesIndex] + ":  %" + val
            },
            offsetY: 0,
            offsetX: 0,
            textAnchor: 'start',
            style: {
                fontSize: '10px',
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
                //text: 'Oran (%)'
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
            text: "Anket Sonuçları",
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

    for (var i = 0; i < polls.dates.length; i++) {
       
        options.series.push({ name: polls.dates[i].year + '-' + months[polls.dates[i].month], data: [], year: polls.dates[i].year, month:polls.dates[i].month });
        //options.labels.push(polls.dates[i].year + '-' + months[polls.dates[i].month]);
    }

    if (parties.length == 1) {
        options.chart.height = 60 + polls.dates.length * 18 * companies.length
        options.title.text = parties[0].name + " Anket Sonuçları (Tarihe Göre)";
        for (var i = 0; i < companies.length; i++) {

            options.xaxis.categories.push(companies[i].name);

            for (var j = 0; j < options.series.length; j++) {
              
                var r = data.find((c) => c.companyId == companies[i].id && c.month == options.series[j].month && c.year == options.series[j].year);
                if (r) {
                    var r1 = r.results.find((p) => p.partyId == parties[0].id);
                    if (r1)
                        options.series[j].data.push(r1.ratio);
                    else
                        options.series[j].data.push(0);

                }
                else
                    options.series[j].data.push(0);

            }

        }
    }
    else {
        options.title.text = data[0].companyName + " Anket Sonuçları (Tarihe Göre)";
        options.chart.height= 60 + polls.dates.length * 18 * parties.length
        for (var i = 0; i < parties.length; i++) {

            options.xaxis.categories.push(parties[i].name);

            for (var j = 0; j < options.series.length; j++) {
                var r = data.find((c) => c.month == options.series[j].month && c.year == options.series[j].year);
                if (r) {
                    var r1 = r.results.find((p) => p.partyId == parties[i].id);
                    if (r1)
                        options.series[j].data.push(r1.ratio);
                    else
                        options.series[j].data.push(0);

                }
                else
                    options.series[j].data.push(0);

            }

        }
    }

    //for (var i = 0; i < parties.length; i++) {
    //    options.series.push({ name: parties[i].name, data: [], id: parties[i].id });
    //}


    //for (var i = 0; i < data.length; i++) {
    //    options.xaxis.categories.push(data[i].companyShortName);

    //    for (var j = 0; j < options.series.length; j++) {
    //        var r = data[i].results.find((p) => p.partyId == options.series[j].id);
    //        if (r)
    //            options.series[j].data.push(r.ratio);
    //        else
    //            options.series[j].data.push(0);

    //    }
    //}
    chart = new ApexCharts(document.querySelector("#chart"), options);
    chart.render();

    parties.sort((a, b) => b.avg - a.avg);
    if (parties.length>1) {
        var ps = parties;
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
                text: "Anket Ortalaması",
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

        var digerAdd = false;
        if (other > 0) {
            var p = ps.find((l) => l.name == 'Diğer');
            if (p)
                p.avg += other;
            else
                digerAdd = true;
        }

        for (var i = 0; i < ps.length; i++) {
            options2.series.push(Math.round(ps[i].avg * 10) / 10.0);
            options2.labels.push(ps[i].name);

        }

        if (digerAdd ) {
            
            options2.series.push(Math.round(other * 10) / 10);
            options2.labels.push("Diğer");

        }


        chart2 = new ApexCharts(document.querySelector("#chart2"), options2);
        chart2.render();
    }

}



function makeChartType2(polls) {
    if (chart)
        chart.destroy();
    if (chart2) {
        chart2.destroy();
        chart2 = null;
    }

    //var monthName = months[dates[1] - 1];
    var data = polls.data;
    var parties = [];
    var companies = [];
    for (var i = 0; i < data.length; i++) {
        var c = companies.find((k) => k.id == data[i].companyId);
        if (!c)
            companies.push({ id: data[i].companyId, name: data[i].companyShortName });

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

    var title = "";
    if (parties.length == 1)
        title = parties[0].name + " Anket Sonuçları";
    else
        title = data[0].CompanyName + " Anket Sonuçları";

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
        labels: [],
        chart: {
            type: 'bar',
            height: 60 + data.length * 18 * parties.length
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

            formatter: function (val, opt) {

                return opt.w.globals.seriesNames[opt.seriesIndex] + ":  %" + val
            },
            offsetY: 0,
            offsetX: 0,
            textAnchor: 'start',
            style: {
                fontSize: '10px',
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
                //text: 'Oran (%)'
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
            text: "Anket Sonuçları",
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

    for (var i = 0; i < polls.dates.length; i++) {

        options.series.push({ name: polls.dates[i].year + '-' + months[polls.dates[i].month], data: [], year: polls.dates[i].year, month: polls.dates[i].month });
        //options.labels.push(polls.dates[i].year + '-' + months[polls.dates[i].month]);
    }

    if (parties.length == 1) {
        options.chart.height = 60 + polls.dates.length * 18 * companies.length
        options.title.text = parties[0].name + " Anket Sonuçları (Tarihe Göre)";
        for (var i = 0; i < companies.length; i++) {

            options.xaxis.categories.push(companies[i].name);

            for (var j = 0; j < options.series.length; j++) {

                var r = data.find((c) => c.companyId == companies[i].id && c.month == options.series[j].month && c.year == options.series[j].year);
                if (r) {
                    var r1 = r.results.find((p) => p.partyId == parties[0].id);
                    if (r1)
                        options.series[j].data.push(r1.ratio);
                    else
                        options.series[j].data.push(0);

                }
                else
                    options.series[j].data.push(0);

            }

        }
    }
    

    //for (var i = 0; i < parties.length; i++) {
    //    options.series.push({ name: parties[i].name, data: [], id: parties[i].id });
    //}


    //for (var i = 0; i < data.length; i++) {
    //    options.xaxis.categories.push(data[i].companyShortName);

    //    for (var j = 0; j < options.series.length; j++) {
    //        var r = data[i].results.find((p) => p.partyId == options.series[j].id);
    //        if (r)
    //            options.series[j].data.push(r.ratio);
    //        else
    //            options.series[j].data.push(0);

    //    }
    //}
    chart = new ApexCharts(document.querySelector("#chart"), options);
    chart.render();

    parties.sort((a, b) => b.avg - a.avg);
    if (parties.length > 1) {
        var ps = parties;
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
                text: "Anket Ortalaması",
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

        var digerAdd = false;
        if (other > 0) {
            var p = ps.find((l) => l.name == 'Diğer');
            if (p)
                p.avg += other;
            else
                digerAdd = true;
        }

        for (var i = 0; i < ps.length; i++) {
            options2.series.push(Math.round(ps[i].avg * 10) / 10.0);
            options2.labels.push(ps[i].name);

        }

        if (digerAdd) {

            options2.series.push(Math.round(other * 10) / 10);
            options2.labels.push("Diğer");

        }


        chart2 = new ApexCharts(document.querySelector("#chart2"), options2);
        chart2.render();
    }

}



function makeChartType3(polls) {
    if (chart)
        chart.destroy();
    if (chart2) {
        chart2.destroy();
        chart2 = null;
    }

    //var monthName = months[dates[1] - 1];
    var data = polls.data;
    var parties = [];
    var companies = [];
    for (var i = 0; i < data.length; i++) {
        var c = companies.find((k) => k.id == data[i].companyId);
        if (!c)
            companies.push({ id: data[i].companyId, name: data[i].companyShortName });

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

    var title = "";
    if (parties.length == 1)
        title = parties[0].name + " Anket Sonuçları";
    else
        title = data[0].CompanyName + " Anket Sonuçları";

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
        labels: [],
        chart: {
            type: 'bar',
            height: 60 + data.length * 18 * parties.length
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

            formatter: function (val, opt) {

                return opt.w.globals.seriesNames[opt.seriesIndex] + ":  %" + val
            },
            offsetY: 0,
            offsetX: 0,
            textAnchor: 'start',
            style: {
                fontSize: '10px',
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
                //text: 'Oran (%)'
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
            text: "Anket Sonuçları",
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

    for (var i = 0; i < polls.dates.length; i++) {

        options.series.push({ name: polls.dates[i].year + '-' + months[polls.dates[i].month], data: [], year: polls.dates[i].year, month: polls.dates[i].month });
        //options.labels.push(polls.dates[i].year + '-' + months[polls.dates[i].month]);
    }


        options.title.text = data[0].companyName + " Anket Sonuçları (Tarihe Göre)";
        options.chart.height = 60 + polls.dates.length * 18 * parties.length
        for (var i = 0; i < parties.length; i++) {

            options.xaxis.categories.push(parties[i].name);

            for (var j = 0; j < options.series.length; j++) {
                var r = data.find((c) => c.month == options.series[j].month && c.year == options.series[j].year);
                if (r) {
                    var r1 = r.results.find((p) => p.partyId == parties[i].id);
                    if (r1)
                        options.series[j].data.push(r1.ratio);
                    else
                        options.series[j].data.push(0);

                }
                else
                    options.series[j].data.push(0);

            }

        }
    

    //for (var i = 0; i < parties.length; i++) {
    //    options.series.push({ name: parties[i].name, data: [], id: parties[i].id });
    //}


    //for (var i = 0; i < data.length; i++) {
    //    options.xaxis.categories.push(data[i].companyShortName);

    //    for (var j = 0; j < options.series.length; j++) {
    //        var r = data[i].results.find((p) => p.partyId == options.series[j].id);
    //        if (r)
    //            options.series[j].data.push(r.ratio);
    //        else
    //            options.series[j].data.push(0);

    //    }
    //}
    chart = new ApexCharts(document.querySelector("#chart"), options);
    chart.render();

    parties.sort((a, b) => b.avg - a.avg);
    console.log(parties);
    if (parties.length > 1) {
        var ps = parties;
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
                text: "Anket Ortalaması",
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

        var digerAdd = false;
        if (other > 0) {
            var p = ps.find((l) => l.name == 'Diğer');
            if (p)
                p.avg += other;
            else
                digerAdd = true;
        }

        for (var i = 0; i < ps.length; i++) {
            options2.series.push(Math.round(ps[i].avg * 10) / 10.0);
            options2.labels.push(ps[i].name);

        }

        if (digerAdd) {

            options2.series.push(Math.round(other * 10) / 10);
            options2.labels.push("Diğer");

        }


        chart2 = new ApexCharts(document.querySelector("#chart2"), options2);
        chart2.render();
    }

}


function selectAll(t) {
    var checked = false;
    var widget = "#widgetDate";
    switch (t) {
        case 2:
            widget ="#widgetElection";
            break;
        case 3:
            widget = "#widgetParty";
            break;
        case 4:
            widget = "#widgetCity";
            break;
    }
    $(widget).find('input').each((e, i) => {
        if (e == 0)
            checked = !$(i).is(':checked');
        $(i).prop('checked', checked);

    });

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
            $('#selectDate').hide();
            $('#widgetDate').show();
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
            $('#selectCompany').hide();
            $('#widgetCompany').show();
            $('#selectParty').show();
            $('#widgetParty').hide();
            chartType = 2;
            $('#infoDate').hide();

            break;
        case 3://parti
            $('#selectDate').hide();
            $('#widgetDate').show();
            $('#selectCompany').show();
            $('#widgetCompany').hide();
            $('#selectParty').hide();
            $('#widgetParty').show();
            chartType = 3;
            $('#infoDate').hide();


            break;

    }
}