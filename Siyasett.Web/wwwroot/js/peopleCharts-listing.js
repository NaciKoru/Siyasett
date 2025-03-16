
let chartCompanies = [];
let chartTimes = [];
let chartParties = [];
let chartType = 1;
let chart = null, chart2 = null, chart3 = null, chart4 = null, chart5 = null, chart6 = null;

window.addEventListener('DOMContentLoaded', (event) => {
    let parties = document.querySelectorAll(".chart-party");



    //$('#widgetParties').hide();
    //$('#widgetRegion').hide();
    //$('#widgetProvince').hide();
    //$('#widgetAges').hide();
    //$('#widgetGender').hide();
    //$('#widgetJobs').hide();

});



function selectAll(t) {
    var checked = false;
    var widget = "#widgetDate";
    switch (t) {
        case 1:
            widget = "#widgetRegion";
            break;
        case 2:
            widget = "#widgetParties";
            break;
        case 3:
            widget = "#widgetProvince";
            break;
        case 4:
            widget = "#widgetAges";
            break;
        case 5:
            widget = "#widgetGender";
            break;
        case 6:
            widget = "#widgetJobs";
            break;
        case 7:
            widget = "#widgetTitle";
            break;
    }
    $(widget).find('input').each((e, i) => {
        if (e == 0)
            checked = !$(i).is(':checked');
        $(i).prop('checked', checked);

    });

}



function type_changed(t) {
    switch (t) {
        case 1://bölgelere göre
            $('#widgetRegion input ').each((i, el) => {
                $(el).prop('checked', true);
            });

            $('#widgetParties').show();
            $('#widgetRegion').hide();
            $('#widgetProvince').hide();
            $('#widgetAges').show();
            $('#widgetGender').show();
            $('#widgetJobs').show();
            chartType = 1;
            break;
        case 2://doğum yerine göre
            $('#widgetParties').show();
            $('#widgetRegion').show();
            $('#widgetProvince').hide();
            $('#widgetAges').show();
            $('#widgetGender').show();
            $('#widgetJobs').show();
            chartType = 2;

            break;
        case 3://görev unvanlarına göre
            $('#widgetParties').show();
            $('#widgetRegion').show();
            $('#widgetProvince').show();
            $('#widgetAges').show();
            $('#widgetGender').show();
            $('#widgetJobs').show();
            chartType = 3;

            break;
        case 4://mesleğine göre
            $('#widgetParties').show();
            $('#widgetRegion').show();
            $('#widgetProvince').show();
            $('#widgetAges').show();
            $('#widgetGender').show();
            $('#widgetJobs').show();
            chartType = 4;

            break;
        case 5://eğitim durumuna göre

            $('#widgetParties').show();
            $('#widgetRegion').hide();
            $('#widgetProvince').hide();
            $('#widgetAges').show();
            $('#widgetGender').show();
            $('#widgetJobs').show();
            chartType = 5;

            break;
        case 6://yaş grubuna göre
            $('#widgetAges input').each((i, el) => {
                $(el).prop('checked', true);
            });
            $('#widgetParties').show();
            $('#widgetRegion').show();
            $('#widgetProvince').hide();
            $('#widgetAges').hide();
            $('#widgetGender').show();
            $('#widgetJobs').show();
            chartType = 6;

            break;
        case 7://cinsiyete göre
            $('#widgetGender input').each((i, el) => {
                $(el).prop('checked', true);
            });
            $('#widgetParties').show();
            $('#widgetRegion').show();
            $('#widgetProvince').hide();
            $('#widgetAges').show();
            $('#widgetGender').hide();
            $('#widgetJobs').show();
            chartType = 7;

            break;
        case 8://partilere göre
            $('#widgetParties').show();
            $('#widgetRegion').show();
            $('#widgetProvince').hide();
            $('#widgetAges').show();
            $('#widgetGender').show();
            $('#widgetJobs').show();
            chartType = 8;
            break;

    }
}

let tablar = [];
function tabsControlShowHide(id) {
    if (id == 1) {
        $('#partytabli').show();
        $('#regiontabli').hide();
        $('#agetabli').show();
        $('#gendertabli').show();
        $('#titletabli').show();
        $('#jobtabli').show();


        tablar = ["partytab1", "agetab1", "gendertab1", "jobtab1", "titletab1"];

    } else if (id == 2) {
        $('#partytabli').show();
        $('#regiontabli').show();
        $('#agetabli').show();
        $('#gendertabli').show();
        $('#titletabli').hide();
        $('#jobtabli').show();




        tablar = ["partytab1", "regiontab1", "agetab1", "gendertab1", "jobtab1"];

    } else if (id == 3) {
        $('#partytabli').show();
        $('#regiontabli').show();
        $('#agetabli').show();
        $('#gendertabli').show();
        $('#titletabli').show();
        $('#jobtabli').hide();



        tablar = ["partytab1", "regiontab1", "agetab1", "gendertab1", "titletab1"];


    } else if (id == 4) {
        $('#partytabli').show();
        $('#regiontabli').show();
        $('#agetabli').show();
        $('#gendertabli').show();
        $('#titletabli').show();
        $('#jobtabli').show();


        tablar = ["partytab1", "regiontab1", "agetab1", "gendertab1", "jobtab1", "titletab1"];

    } else if (id == 5) {
        $('#partytabli').show();
        $('#regiontabli').show();
        $('#agetabli').hide();
        $('#gendertabli').show();
        $('#titletabli').show();
        $('#jobtabli').show();

        tablar = ["partytab1", "regiontab1", "gendertab1", "jobtab1", "titletab1"];

    } else if (id == 6) {
        $('#partytabli').show();
        $('#regiontabli').show();
        $('#agetabli').show();
        $('#gendertabli').hide();
        $('#titletabli').show();
        $('#jobtabli').show();

        tablar = ["partytab1", "regiontab1", "agetab1", "jobtab1", "titletab1"];

    } else if (id == 7) {
        $('#partytabli').hide();
        $('#regiontabli').show();
        $('#agetabli').show();
        $('#gendertabli').show();
        $('#titletabli').show();
        $('#jobtabli').show();
        tablar = ["regiontab1", "agetab1", "gendertab1", "jobtab1", "titletab1"];

    }

}

var parties = '';
var regions = '';
var provinces = '';
var ages = '';
var gender = '';
var jobs = '';
var titles = '';
var tabsValue = 0;

var totalTabs = $('.nav-tabs li').length;
var currentTab = 1;

function tabsControl(el, al) {



    tabsValue = $("input[name='lt']:checked").val();
    tabsControlShowHide(tabsValue);
    var activename = $('#myTabContent div.active').attr('id');



    if (activename == 'typetab') {
        $('#' + tablar[0] + '').click();

    } else {
        var tabindex = tablar.indexOf(activename + 1);
        $('#' + (tablar[tabindex + 1]) + '').click();
    }



    if (tabsValue == 2) {
        $('#nextButton2').hide();
        $('#readyButton2').show();
        $('#nextButton').show();
        $('#readyButton').hide();
    } else {
        $('#nextButton').hide();
        $('#readyButton').show();
        $('#nextButton2').show();
        $('#readyButton2').hide();
    }

}


function getData2() {




    parties = '';
    $('#widgetParties').find('input').each((e, i) => {
       

        if ($(i).is(':checked')) {
            
                parties += $(i).val() + ';';
           
        }
    });

    regions = '';
    $('#widgetRegion').find('input').each((e, i) => {
      

        if ($(i).is(':checked')) {
           
                regions += $(i).val() + ';';
          
        }

    });

    ages = '';
    $('#widgetAges').find('input').each((e, i) => {
        
        if ($(i).is(':checked')) {
           
                ages += $(i).val() + ';';
            
        }
    });
    gender = '';
    $('#widgetGender').find('input').each((e, i) => {

       
        if ($(i).is(':checked')) {
           
                gender += $(i).val() + ';';
           
        }
    });

    jobs = '';
    $('#widgetJobs').find('input').each((e, i) => {


        if ($(i).is(':checked')) {

            jobs += $(i).val() + ';';

        }

    });

    titles = '';
    $('#widgetTitle').find('input').each((e, i) => {

        if ($(i).is(':checked')) {

            titles += $(i).val() + ';';
        }
        
    });

    parties = parties.slice(0, -1);
    regions = regions.slice(0, -1);
    ages = ages.slice(0, -1);
    gender =gender.slice(0, -1);
    jobs = jobs.slice(0, -1);
    titles= titles.slice(0, -1);

    var isAvtive = 2;
    if ($("#chkPositionaktif").is(':checked') == true) {
        isAvtive = 1;
    }



    $.ajax({
        type: 'get',
        url: '/People/GetData',
        data: { t: tabsValue, partiesstr: parties, regionsstr: regions, agesstr: ages, genderstr: gender, jobsstr: jobs, titlesstr: titles, isactive: isAvtive },
        traditional: true,
        success: (r) => { makeNewChart(tabsValue, r) }
    });


}


function makeNewChart(el, data) {
    if (chart != null) {
        chart.destroy();
        chart = null;
    }

    var siralama = new Array();
    for (var i = 0; i < data[0].length; i++) {
        if (data[1][i] != 0) {
            var asd = [data[1][i], data[0][i]];
            siralama.push(asd);
        }
    }

    siralama.sort(function (a, b) {
        return a[0] - b[0];
    });

    siralama.reverse();
    var newdata = new Array();
    var newcat = new Array();

    for (var i = 0; i < siralama.length; i++) {
        newdata.push(siralama[i][0]);
        newcat.push(siralama[i][1]);
    }


    var options = {
        series: [{
            data: newdata,
        }],
        chart: {
            type: 'bar',
            height: 85 + data[0].length * 30
        },
        plotOptions: {
            bar: {
                borderRadius: 4,
                horizontal: true,
                dataLabels: {
                    position: 'top',

                },
            }
        },
        dataLabels: {
            enabled: true,
            offsetY: 0,
            offsetX: 12,
            textAnchor: 'start',
            style: {
                fontSize: '12px',
                colors: ["#304758"]
            },
            formatter: function (val) {
                return val.toString().replace(/(\d+)(\d{3})/, '$1' + ',' + '$2');
            },
        },
        xaxis: {
            categories: newcat,
        },
        title: {
            text: "",
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



    chart = new ApexCharts(document.querySelector("#chart"), options);
    chart.render();


}