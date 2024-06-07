"use strict";

var iller = [];
let chart = null, chart2 = null;
var oylarTabloBasildimi = false;
var secilenpartisayisi = -1;

var seciliPartiler = [];
var seciliFirmalar = [];
var secimdetay;
var returnIller;

$.ajaxSetup({ "async": true, "timeout": 0, "headers": { "cache-control": "no-cache" } });

$(function () {




    var sim = $("#sim-form");
    sim.children("div").steps({
        headerTag: "h3",
        bodyTag: "fieldset",
        transitionEffect: "fade",
        stepsOrientation: "vertical",
        titleTemplate: '<div class="title"><span class="step-number">#index#</span><span class="step-text">#title#</span></div>',
        labels: {
            previous: 'Önceki',
            next: 'Sonraki',
            finish: 'Hazırla',
            current: ''
        },
        onStepChanging: function (event, currentIndex, newIndex) {
            if (currentIndex === 0) {
                sim.parent().parent().parent().append('<div class="footer footer-' + currentIndex + '"></div>');
            }
            if (currentIndex === 0) {
                if ($('#kaynak_1').is(':checked')) {
                    $('#kaynakAnket').show();
                    $('#kaynakSecim').hide();
                }
                else {
                    $('#kaynakAnket').hide();
                    $('#kaynakSecim').show();
                }

            }
            if (currentIndex === 2) {

                var secim = $('input[name=choose_poll]:checked').val();
                $('#iller').empty();
                if (secim == "poll") {
                    $.post('/Simulations/GecimSecimdeniller', { secimId: 11 })
                        .done((r) => {
                            for (var i = 0; i < r.length; i++) {
                                var asd = '<div class="col"><div class="form-check"><input onclick="ilSec_click()" class="form-check-input" type="checkbox" name="il" id="il_' + r[i].regionId + '" value="' + r[i].regionId + '"/><label class="form-check-label" for="il_' + r[i].regionId + '">' + r[i].name + '</label></div><div>';
                                $('#iller').append(asd);
                            }

                        });

                } else {

                    var secimId = $('#selectSecimId').val();

                    $.post('/Simulations/GecimSecimdeniller', { secimId: secimId })
                        .done((r) => {
                            for (var i = 0; i < r.length; i++) {
                                var asd = '<div class="col"><div class="form-check"><input onclick="ilSec_click()" class="form-check-input" type="checkbox" name="il" id="il_' + r[i].regionId + '" value="' + r[i].regionId + '"/><label class="form-check-label" for="il_' + r[i].regionId + '">' + r[i].name + '</label></div></div>';
                                $('#iller').append(asd);
                            }

                        });

                }

            }
            if (newIndex === 3) {
                var partiler = [];
                $('#partiler').find('input').each((i, el) => {
                    if ($(el).is(':checked'))
                        partiler.push($(el).val());

                });
                if (partiler.length != secilenpartisayisi) {
                    oylarTabloBasildimi = false;
                    $('#tblBodyGenel').empty();
                    secilenpartisayisi = partiler.length;
                }
                if (!oylarTabloBasildimi) {
                    blockUI();
                    genelOyOranlarinaGit();
                }
            }

            if (currentIndex === 4) {


            }

            return true;
        },
        onFinishing: function (event, currentIndex) {
            //form.validate().settings.ignore = ":disabled";
            var partiler = [];
            iller = [];
            var firmalar = [];


            //$('#simContainer1').hide();
            blockUI("Tüm iller için simülasyon hesaplarının hazırlanması sunucu yoğunluğuna göre birkaç dakika sürebilir. Lütfen bekleyiniz.");
            $('#partiler').find('input').each((i, el) => {
                if ($(el).is(':checked'))
                    partiler.push($(el).val());

            });


            $('#iller').find('input').each((i, el) => {
                if ($(el).is(':checked'))
                    iller.push($(el).val());

            });
            $('#firmalar').find('input').each((i, el) => {
                if ($(el).is(':checked'))
                    firmalar.push($(el).val());

            });


            var secim = $('input[name=choose_poll]:checked').val();

            if (secim == "poll") {
                var secimId = 11;

                $.post('/Simulations/GetPartiRatiosFromElection', { secimId: secimId, simPartiler: partiler, simIller: iller, })
                    .done((r) => {
                        secimdetay = r[0];
                        var returnparty = r[1];
                        returnIller = r[2];
                        var returnSonuc = r[3];
                        var partilerDb = r[4];
                        var tablosatir = "";

                        similuasyonHesaplagenel();


                    });

            } else {
                //similuasyonHesaplagenel();
                var secimId = $('#selectSecimId').val();

                $.post('/Simulations/GetPartiRatiosFromElection', { secimId: secimId, simPartiler: partiler, simIller: iller, })
                    .done((r) => {
                        secimdetay = r[0];
                        var returnparty = r[1];
                        returnIller = r[2];
                        var returnSonuc = r[3];
                        var partilerDb = r[4];
                        var tablosatir = "";

                        similuasyonHesaplagenel();


                    });

            }



            //return form.valid();
        },
        onFinished: function (event, currentIndex) {
            alert('Submited');
        },
        onStepChanged: function (event, currentIndex, priorIndex) {

            return true;
        }
    });


});

function hazirla_onClick() {

    var partiler = [];
    iller = [];
    var firmalar = [];


    //$('#simContainer1').hide();
    blockUI("Tüm iller için simülasyon hesaplarının hazırlanması sunucu yoğunluğuna göre birkaç dakika sürebilir. Lütfen bekleyiniz...");
    $('#partiler').find('input').each((i, el) => {
        if ($(el).is(':checked'))
            partiler.push($(el).val());

    });


    $('#iller').find('input').each((i, el) => {
        if ($(el).is(':checked'))
            iller.push($(el).val());

    });
    $('#firmalar').find('input').each((i, el) => {
        if ($(el).is(':checked'))
            firmalar.push($(el).val());

    });


    var secim = $('input[name=choose_poll]:checked').val();

    if (secim == "poll") {
        var secimId = 11;

        $.post('/Simulations/GetPartiRatiosFromElection', { secimId: secimId, simPartiler: partiler, simIller: iller, })
            .done((r) => {
                secimdetay = r[0];
                var returnparty = r[1];
                returnIller = r[2];
                var returnSonuc = r[3];
                var partilerDb = r[4];
                var tablosatir = "";

                similuasyonHesaplagenel();


            });

    } else {
        //similuasyonHesaplagenel();
        var secimId = $('#selectSecimId').val();

        $.post('/Simulations/GetPartiRatiosFromElection', { secimId: secimId, simPartiler: partiler, simIller: iller, })
            .done((r) => {
                secimdetay = r[0];
                var returnparty = r[1];
                returnIller = r[2];
                var returnSonuc = r[3];
                var partilerDb = r[4];
                var tablosatir = "";

                similuasyonHesaplagenel();


            });

    }

}

function step1_onClick() {

    if ($('#kaynak_1').is(':checked')) {
        $('#kaynakAnket').show();
        $('#kaynakSecim').hide();
    }
    else {
        $('#kaynakAnket').hide();
        $('#kaynakSecim').show();
    }

    $('#panel1').hide();

    $('#panel2').show();

    document.getElementById("simContainer1").scrollIntoView();


}
function step2_onClick() {

    $('#panel2').hide();

    $('#panel3').show();
    document.getElementById("simContainer1").scrollIntoView();

}
function step3_onClick() {

    preparePanel4();

    $('#panel3').hide();
    $('#panel4').show();

    document.getElementById("simContainer1").scrollIntoView();
}

function preparePanel4() {
    var partiler = [];
    $('#partiler').find('input').each((i, el) => {
        if ($(el).is(':checked'))
            partiler.push($(el).val());

    });
    if (partiler.length != secilenpartisayisi) {
        oylarTabloBasildimi = false;
        $('#tblBodyGenel').empty();
        secilenpartisayisi = partiler.length;
    }
    if (!oylarTabloBasildimi) {
        blockUI();
        genelOyOranlarinaGit();
    }
}

function step4_onClick() {

    blockUI();

    var secim = $('input[name=choose_poll]:checked').val();
    $('#iller').empty();
    if (secim == "poll") {
        $.post('/Simulations/GecimSecimdeniller', { secimId: 11 })
            .done((r) => {
                for (var i = 0; i < r.length; i++) {
                    var asd = '<div class="col"><div class="form-check"><input onclick="ilSec_click()" class="form-check-input" type="checkbox" name="il" id="il_' + r[i].regionId + '" value="' + r[i].regionId + '"/><label class="form-check-label" for="il_' + r[i].regionId + '">' + r[i].name + '</label></div><div>';
                    $('#iller').append(asd);
                }
                unblockUI();
                $('#panel4').hide();
                $('#panel5').show();
                document.getElementById("simContainer1").scrollIntoView();

            });

    } else {

        var secimId = $('#selectSecimId').val();

        $.post('/Simulations/GecimSecimdeniller', { secimId: secimId })
            .done((r) => {
                for (var i = 0; i < r.length; i++) {
                    var asd = '<div class="col"><div class="form-check"><input onclick="ilSec_click()" class="form-check-input" type="checkbox" name="il" id="il_' + r[i].regionId + '" value="' + r[i].regionId + '"/><label class="form-check-label" for="il_' + r[i].regionId + '">' + r[i].name + '</label></div></div>';
                    $('#iller').append(asd);
                }
                unblockUI();
                $('#panel4').hide();

                $('#panel5').show();
                document.getElementById("simContainer1").scrollIntoView();



            });

    }




}


function step2Prev_onClick() {
    $('#panel2').hide();

    $('#panel1').show();
    document.getElementById("simContainer1").scrollIntoView();

}
function step3Prev_onClick() {

    $('#panel3').hide();
    $('#panel2').show();
    document.getElementById("simContainer1").scrollIntoView();
}

function step4Prev_onClick() {

    $('#panel4').hide()
    $('#panel3').show();
    document.getElementById("simContainer1").scrollIntoView();

}
function step5Prev_onClick() {

    $('#panel5').hide();

    $('#panel4').show();
    document.getElementById("simContainer1").scrollIntoView();

}

function blockUI(message) {

    $.blockUI({ message: '<img src="/images/loading.gif" /><br/>' + (message ? message : "") });

}

function unblockUI() {

    $.unblockUI();

}

function anketFirma_ara() {

    var txt = $('#txtAnketAra').val().toUpperCase();
    if (txt.length < 1) {
        $('#firmalar > div').show();
    }
    else {

        $('#firmalar').find('div').each((i, el) => {
            var f = $(el).attr('data-adi').toUpperCase();
            if (f.indexOf(txt) > -1) {
                $(el).show();
            }
            else {
                $(el).hide();
                //$(el).find('input').prop("checked", false);
            }

        });
    }

}


function parti_ara() {

    var txt = $('#txtPartiAra').val().toUpperCase();
    if (txt.length < 1) {
        $('#partiler > div').show();
    }
    else {

        $('#partiler').find('div').each((i, el) => {
            var f = $(el).attr('data-adi').toUpperCase();
            if (f.indexOf(txt) > -1) {
                $(el).show();
            }
            else {
                $(el).hide();
                //$(el).find('input').prop("checked", false);
            }

        });
    }

}
function partiSec_click() {
    var secili = 0;

    $('#partiler').find('input').each((i, el) => {
        if ($(el).is(':checked'))
            secili++;

    });
    if (secili > 0) {
        $('#lblPartiSecim').text(secili + " parti seçildi.");
    }
    else {
        $('#lblPartiSecim').text("Seçili parti bulunmuyor.");

    }

}



function partiSecim_temizle() {

    $('#partiler').find('input').each((i, el) => {
        $(el).prop('checked', false);

    });

    $('#lblPartiSecim').text("Seçili parti bulunmuyor.");

}


function ilSec_click() {
    var secili = 0;

    $('#iller').find('input').each((i, el) => {
        if ($(el).is(':checked'))
            secili++;

    });
    if (secili > 0) {
        $('#lblilSecim').text(secili + " il seçildi.");
    }
    else {
        $('#lblilSecim').text("Seçili il bulunmuyor.");

    }

}

function ilSecim_temizle() {

    $('#iller').find('input').each((i, el) => {
        $(el).prop('checked', false);

    });

    $('#lblilSecim').text("Seçili il bulunmuyor.");

}

function anketSec_click() {
    var secili = 0;
    $('#firmalar').find('input').each((i, el) => {
        if ($(el).is(':checked'))
            secili++;

    });
    if (secili > 0) {
        $('#lblAnketSecim').text(secili + " firma seçildi.");
    }
    else {
        $('#lblAnketSecim').text("Seçili firma bulunmuyor.");

    }

}

function anketSecim_temizle() {

    $('#firmalar').find('input').each((i, el) => {
        $(el).prop('checked', false);

    });

    $('#lblAnketSecim').text("Seçili firma bulunmuyor.");



}


function gotoStep2_click() {


    var secimId = parseInt($('input[name="secim"]:checked').val());
    $('input[name="anket"]:checked').each((i, el) => {

        seciliFirmalar.push({
            adi: $(el).attr("data-adi"),
            id: parseInt($(el).val())
        });
    });
    $('input[name="parti"]:checked').each((i, el) => {

        seciliPartiler.push({
            adi: $(el).attr("data-adi"),
            id: parseInt($(el).val())
        });
    });

    if (seciliFirmalar.length == 0) {
        Swal.fire('Siyasett', 'En az bir tane anket firması seçilmelidir.', 'warning');
        return;
    }

    if (seciliPartiler.length == 0) {
        Swal.fire('Siyasett', 'En az bir tane siyasi parti seçilmelidir.', 'warning');
        return;
    }

    blockUI();

    $.post('/Simulations/GetStep2', { secimId: secimId })
        .done((r) => {

            var $il = $('#iller');
            for (var i = 0; i < r.length; i++) {
                var l = '<div data-adi="' + r[i].name + '">';
                l += '<input onclick="ilSec_click()" class="tmm-checkbox" type="checkbox" name="il" id="il_' + r[i].regionId + '" value="' + r[i].regionId + '">';
                l += '<label for="il_' + r[i].regionId + '">' + r[i].name + '</label>';
                $il.append(l);
            }
            $('#step2').show();
            $('#step1').hide();
            unblockUI();
        });

    //<div data-adi="AKSARAY">
    //    <input onclick="ilSec_click()" class="tmm-checkbox" type="checkbox" name="il" id="il_271682" value="271682">
    //        <label for="il_271682">AKSARAY</label>
    //</div>

}

function gotoStep1_click() {
    $('#step1').show();
    $('#step2').hide();

}


function ittifakEkleModal() {
    $('#ittifakEkleModal').find('input').val("");
    $('#ittifakEkleModal').modal('show');
}

function myfuncloseModalction() {
    $('#ittifakEkleModal').find('input').val("");
    $('#ittifakEkleModal').modal('hide');
}



function ittifakEkle() {
    var ad = $('#modalIttifakAdi').val();
    var th = '<th>' + ad + '</th>';
    var trlenght = $('#ittifaklarTablo thead tr th').length;
    $('#ittifaklarTablo thead tr').append(th);

    $('#tblBody').find('tr').each(function (i, el) {


        var check = '<td><input  class="form-check-input" type = "checkbox" name = "ittifak" id = "ittifak' + ad + '_' + i + '" onchange="ittifakCheckChanged(this)"/></td>'
        $(el).append(check);


    })
    th = '<th>' + ad + ' T.Oy</th>';
    $('#oranlarTabloHead').append(th);
    $('#tblBody2').find('tr').each(function (i, el) {

        var check = '<td id = "ittifak' + ad + '_countAvg_' + $(el).attr('data-id') + '">0</td>'
        $(el).append(check);
    })



    $('#ittifakEkleModal').modal('hide');

}


function guessAvgChange(el) {
    var parent;
    var children;
    var ittifakPartileri = [];
    var ittifaklistesi = [];

    var toplam = 0;
    var tr = $(el).parent().parent();
    $(tr).find('input').each(function (i, al) {
        toplam = parseFloat(toplam) + parseFloat($(al).val())
    })
    $('#avgVoteSum_' + $(tr).attr('data-id')).html(parseFloat(toplam).toFixed(2));

    $('#ittifaklarListesi th').each(function (a, el) {
        if (a != 0) {
            ittifaklistesi.push($(el).html());
        }

    })

    for (var i = 0; i < ittifaklistesi.length; i++) {
        var part = [];
        $('#tblBody input[id^=ittifak' + ittifaklistesi[i] + ']:checked').each(function (i, al) {

            parent = $(al).parent().parent();
            children = $(parent).children().eq(0);
            var partyID = $(children).attr('id').split('_')[1];
            part.push(partyID);

        });
        ittifakPartileri.push(part);

        var regionid;
        $('#tblBody2 tr').each(function (j, ol) {
            toplam = 0;
            $(ol).find('input').each(function (k, sl) {

                var partID = $(sl).attr('id').split('_')[1];
                regionid = $(sl).parent().parent().attr('data-id');
                if (ittifakPartileri[i].indexOf(partID) != -1) {
                    toplam = parseFloat($(sl).val()) + parseFloat(toplam);
                }
            })

            $('#tblBody2 td[id="ittifak' + ittifaklistesi[i] + '_countAvg_' + regionid + '"]').html(toplam.toFixed(2));
        });


    }

}

function ittifakCheckChanged(el) {

    var id = $(el).attr('id').split('_')[0];
    var orjid = $(el).attr('id');

    var tr = $(el).parent().parent();
    var oran;
    var parent;
    var children;
    var ittifakPartileri = [];
    var ittifaklistesi = [];

    if ($(el).is(':checked')) {
        $(tr).find('input').each(function (p, sl) {
            if ($(sl).attr('id') != $(el).attr('id')) {
                $(sl).prop('checked', false);
            }
        })
    }



    $('#ittifaklarListesi th').each(function (a, el) {
        if (a != 0) {
            ittifaklistesi.push($(el).html());
        }

    })

    for (var i = 0; i < ittifaklistesi.length; i++) {
        var part = [];
        $('#tblBody input[id^=ittifak' + ittifaklistesi[i] + ']:checked').each(function (i, al) {

            parent = $(al).parent().parent();
            children = $(parent).children().eq(0);
            var partyID = $(children).attr('id').split('_')[1];
            part.push(partyID);

        });
        ittifakPartileri.push(part);

        var regionid;
        $('#tblBody2 tr').each(function (j, ol) {
            var toplam = 0;
            $(ol).find('input').each(function (k, sl) {

                var partID = $(sl).attr('id').split('_')[1];
                regionid = $(sl).parent().parent().attr('data-id');
                if (ittifakPartileri[i].indexOf(partID) != -1) {
                    toplam = parseFloat($(sl).val()) + parseFloat(toplam);
                }
            })

            $('#tblBody2 td[id="ittifak' + ittifaklistesi[i] + '_countAvg_' + regionid + '"]').html(toplam.toFixed(2));
        });


    }
}





var catagoriesforChart = [];
var dataforchart = [];
var genelOylarAnket = [];
var genelOylarSecim = [];

function genelOyOranlarinaGit() {
    var partiler = [];
    var firmalar = [];
    if (chart)
        chart.destroy();
    if (chart2) {
        chart2.destroy();
        chart2 = null;
    }


    $('#partiler').find('input').each((i, el) => {
        if ($(el).is(':checked'))
            partiler.push($(el).val());

    });

    $('#firmalar').find('input').each((i, el) => {
        if ($(el).is(':checked'))
            firmalar.push($(el).val());

    });


    var secim = $('input[name=choose_poll]:checked').val();

    if (secim == "poll") {
        $.post('/Simulations/GetPartiRatiosFromPolls', { secimId: 11, simPartiler: partiler, simSirketler: firmalar })
            .done((r) => {
                var partiOranlarPoll = [];
                var anketSonuclar = r[3];

                var tablotd = "";
                var toplamYuzdeAvg = 0;
                var dbPartiler = r[1];

                for (var i = 0; i < partiler.length; i++) {
                    var toplam = 0;
                    var sonucPartiAyiklanmis = anketSonuclar.filter(a => a.partyId == partiler[i]);
                    if (sonucPartiAyiklanmis.length > 0) {
                        $(sonucPartiAyiklanmis).each(function (j, al) {
                            toplam = parseFloat(toplam) + parseFloat(al.ratio)
                        });
                        var obj = {
                            partiId: partiler[i],
                            oran: parseFloat(parseFloat(toplam) / sonucPartiAyiklanmis.length).toFixed(2),
                        };
                        partiOranlarPoll.push(obj);
                    } else {
                        var obj = {
                            partiId: partiler[i],
                            oran: 0,
                        };
                        partiOranlarPoll.push(obj);
                    }

                }
                genelOylarAnket = partiOranlarPoll;
                for (var i = 0; i < partiOranlarPoll.length; i++) {

                    toplamYuzdeAvg = parseFloat(toplamYuzdeAvg) + parseFloat(partiOranlarPoll[i].oran);
                    tablotd = '<tr id="genelPartiTr_' + partiOranlarPoll[i].partiId + '"><td>' + dbPartiler.find(a => a.id == partiOranlarPoll[i].partiId).partyNameShort + '</td><td>' + partiOranlarPoll[i].oran + '</td>' +
                        '<td class="text-center"><input id="partyAvgGenel_' + partiOranlarPoll[i].partiId + '"  class="form-control form-control-sm" style="max-width:80px;text-align:right;" type="text" value="' + partiOranlarPoll[i].oran + '" onchange="genelTahminOranDegisti()"/></td>' +
                        '<td class="text-center"><div class="form-check form-check-inline"><input type="checkbox" class="form-check-input" name="parti' + partiOranlarPoll[i].partiId + '" id="ittifak1_' + partiOranlarPoll[i].partiId + '" onchange="checkChangeGenel(this)"/></div></td>' +
                        '<td class="text-center"><div class="form-check form-check-inline"><input type="checkbox" class="form-check-input" name="parti' + partiOranlarPoll[i].partiId + '" id="ittifak2_' + partiOranlarPoll[i].partiId + '" onchange="checkChangeGenel(this)"/></div></td>' +
                        '<td class="text-center"><div class="form-check form-check-inline"><input type="checkbox" class="form-check-input" name="parti' + partiOranlarPoll[i].partiId + '" id="ittifak3_' + partiOranlarPoll[i].partiId + '" onchange="checkChangeGenel(this)"/></div></td>' +
                        '<td class="text-center"><div class="form-check form-check-inline"><input type="checkbox" class="form-check-input" name="parti' + partiOranlarPoll[i].partiId + '" id="ittifak4_' + partiOranlarPoll[i].partiId + '" onchange="checkChangeGenel(this)"/></div></td>' +
                        '<td class="text-center"><div class="form-check form-check-inline"><input type="checkbox" class="form-check-input" name="parti' + partiOranlarPoll[i].partiId + '" id="ittifak5_' + partiOranlarPoll[i].partiId + '" onchange="checkChangeGenel(this)"/></div></td></tr>';
                    $('#tblBodyGenel').append(tablotd);
                }


                //for (var i = 0; i < partiler.length; i++) {
                //    var tr = $('#genelPartiTr_' + partiler[i]);
                //    if (tr.length == 0) {
                //        tablotd = '<tr id="genelPartiTr_' + partiler[i] + '"><td>' + dbPartiler.find(a => a.id == partiler[i]).shortName + '</td><td>0</td><td><input id="partyAvgGenel_' + partiler[i] + '" type="text" value="0" onchange="genelTahminOranDegisti()"/></td><td><input type="checkbox" id="ittifak1_' + partiler[i] + '" onchange="checkChangeGenel(this)"/></td><td><input type="checkbox" id="ittifak2_' + partiler[i] + '" onchange="checkChangeGenel(this)"/></td><td><input type="checkbox" id="ittifak3_' + partiler[i] + '" onchange="checkChangeGenel(this)"/></td></tr>';
                //        $('#tblBodyGenel').append(tablotd);
                //    }
                //}
                tablotd = '<tr><td >Diğer</td><td></td><td id="digerToplam">' + (100 - toplamYuzdeAvg.toFixed(2)).toFixed(2) + '</td><td></td><td></td><td></td><td></td><td></td></tr>';
                $('#tblBodyGenel').append(tablotd);
                tablotd = '<tr><td>Toplam</td><td></td><td id="genelOyTahminToplam">' + 100 + '</td>' +
                    '<td class="text-center" id="ittifak1Toplam">0</td><td class="text-center" id="ittifak2Toplam">0</td>' +
                    '<td class="text-center" id="ittifak3Toplam">0</td><td class="text-center" id="ittifak4Toplam">0</td>' +
                    '<td class="text-center" id="ittifak5Toplam">0</td></tr>';
                $('#tblBodyGenel').append(tablotd);
                var digerToplam = (100 - toplamYuzdeAvg.toFixed(2)).toFixed(2);
                if (parseFloat(digerToplam) < 0) {
                    $('#digerToplam').css("color", "red");
                } else {
                    $('#digerToplam').css("color", "green");
                }

                $('#simContainer4').show();
                unblockUI();

            });

    } else {

        var secimId = $('#selectSecimId').val();

        $.post('/Simulations/GetPartiRatiosFromElection', { secimId: secimId, simPartiler: partiler, simIller: iller, })
            .done((r) => {
                var toplamAvg = 0;
                var tablotd = "";
                var asd = [];
                var returnParti = r[1];
                var toplamYuzdeAvg = 0;
                var dbPartiler = r[4];



                for (var j = 0; j < returnParti.length; j++) {

                    toplamAvg = parseFloat(toplamAvg) + parseFloat(returnParti[j].ratioAvg);

                }

                for (var i = 0; i < returnParti.length; i++) {

                    if (returnParti[i].id != 134 && returnParti[i].id != 136 && returnParti[i].name.indexOf('Silinecek') == -1 && partiler.indexOf(returnParti[i].id.toString()) != -1) {

                        var partyYuzde = parseFloat((parseFloat(returnParti[i].ratioAvg) / parseFloat(toplamAvg)) * 100).toFixed(2);
                        toplamYuzdeAvg = parseFloat(toplamYuzdeAvg) + parseFloat(partyYuzde);
                        tablotd = '<tr id="genelPartiTr_' + returnParti[i].id + '"><td>' + returnParti[i].partyNameShort + '</td><td>' + partyYuzde + '</td>' +
                            '<td class="text-center"><input  class="form-control form-control-sm" id="partyAvgGenel_' + returnParti[i].id + '" type="text" value="' + partyYuzde + '" onchange="genelTahminOranDegisti()" style="max-width:80px;text-align:right;"/></td>' +
                            '<td class="text-center"><div class="form-check form-check-inline"><input type="checkbox" class="form-check-input" name="parti' + returnParti[i].id + '" class="form-check-input" id="ittifak1_' + returnParti[i].id + '" onchange="checkChangeGenel(this)"/></td>' +
                            '<td class="text-center"><div class="form-check form-check-inline"><input type="checkbox" class="form-check-input" name="parti' + returnParti[i].id + '" id="ittifak2_' + returnParti[i].id + '" onchange="checkChangeGenel(this)"/></div></td>' +
                            '<td class="text-center"><div class="form-check form-check-inline"><input type="checkbox" class="form-check-input" name="parti' + returnParti[i].id + '" id="ittifak3_' + returnParti[i].id + '" onchange="checkChangeGenel(this)"/></div></td>' +
                            '<td class="text-center"><div class="form-check form-check-inline"><input type="checkbox" class="form-check-input" name="parti' + returnParti[i].id + '" id="ittifak4_' + returnParti[i].id + '" onchange="checkChangeGenel(this)"/></div></td>' +
                            '<td class="text-center"><div class="form-check form-check-inline"><input type="checkbox" class="form-check-input" name="parti' + returnParti[i].id + '" id="ittifak5_' + returnParti[i].id + '" onchange="checkChangeGenel(this)"/></div></td></tr>';
                        $('#tblBodyGenel').append(tablotd);

                        dataforchart.push(partyYuzde);
                        catagoriesforChart.push(returnParti[i].partyNameShort);
                    }
                }



                for (var i = 0; i < partiler.length; i++) {
                    var tr = $('#genelPartiTr_' + partiler[i]);
                    if (tr.length == 0) {
                        console.log(partiler[i]);
                        tablotd = '<tr id="genelPartiTr_' + partiler[i] + '"><td>' + dbPartiler.find(a => a.id == partiler[i]).shortName + '</td><td>0</td>' +
                            '<td class="text-center"><input  class="form-control form-control-sm" id="partyAvgGenel_' + partiler[i] + '" type="text" value="0" onchange="genelTahminOranDegisti()" style="max-width:80px;text-align:right;"/></td>' +
                            '<td class="text-center"><div class="form-check form-check-inline"><input class="form-check-input" type="checkbox" name="parti' + partiler[i] + '" id="ittifak1_' + partiler[i] + '" onchange="checkChangeGenel(this)"/></div></td>' +
                            '<td class="text-center"><div class="form-check form-check-inline"><input class="form-check-input" type="checkbox" name="parti' + partiler[i] + '" id="ittifak2_' + partiler[i] + '" onchange="checkChangeGenel(this)"/></div></td>' +
                            '<td class="text-center"><div class="form-check form-check-inline"><input class="form-check-input" type="checkbox" name="parti' + partiler[i] + '" id="ittifak3_' + partiler[i] + '" onchange="checkChangeGenel(this)"/></div></td>' +
                            '<td class="text-center"><div class="form-check form-check-inline"><input class="form-check-input" type="checkbox" name="parti' + partiler[i] + '" id="ittifak4_' + partiler[i] + '" onchange="checkChangeGenel(this)"/></div></td>' +
                            '<td class="text-center"><div class="form-check form-check-inline"><input class="form-check-input" type="checkbox" name="parti' + partiler[i] + '" id="ittifak5_' + partiler[i] + '" onchange="checkChangeGenel(this)"/></div></td></tr>';
                        $('#tblBodyGenel').append(tablotd);
                    }
                }
                tablotd = '<tr><td >Diğer</td><td></td><td id="digerToplam">' + (100 - toplamYuzdeAvg.toFixed(2)).toFixed(2) + '</td><td></td><td></td><td></td><td></td><td></td></tr>';
                $('#tblBodyGenel').append(tablotd);
                tablotd = '<tr><td>Toplam</td><td></td><td id="genelOyTahminToplam">' + 100 + '</td><td class="text-center" id="ittifak1Toplam">0</td>' +
                    '<td class="text-center" id="ittifak2Toplam">0</td>' +
                    '<td class="text-center" id="ittifak3Toplam">0</td><td class="text-center" id="ittifak4Toplam">0</td><td class="text-center" id="ittifak5Toplam">0</td></tr>';
                $('#tblBodyGenel').append(tablotd);

                var digerToplam = (100 - toplamYuzdeAvg.toFixed(2)).toFixed(2);
                if (parseFloat(digerToplam) < 0) {
                    $('#digerToplam').css("color", "red");
                } else {
                    $('#digerToplam').css("color", "green");
                }

                $('#simContainer4').show();
                unblockUI();

            });

    }

    oylarTabloBasildimi = true;
}



function checkChangeGenel(el) {
    if ($(el).is(':checked')) {
        var parent = $(el).closest('tr');
        $(parent).find('input[type=checkbox]').each(function (i, al) {

            if ($(al).attr('id') != $(el).attr('id')) {
                $(al).prop('checked', false)
            }
        })

        for (var i = 1; i < 6; i++) {
            var toplamoran = 0;
            $('#tblBodyGenel').find('input[id*=ittifak' + i + ']').each(function (i, al) {
                if ($(al).is(':checked')) {
                    var yuzde = $('#partyAvgGenel_' + $(al).attr('id').split('_')[1]);
                    toplamoran = parseFloat(toplamoran) + parseFloat(yuzde.val());
                }

            })
            $('#ittifak' + i + 'Toplam').html(toplamoran.toFixed(2));
        }

    } else {
        for (var i = 1; i < 6; i++) {
            var toplamoran = 0;
            $('#tblBodyGenel').find('input[id*=ittifak' + i + ']').each(function (i, al) {
                if ($(al).is(':checked')) {
                    var yuzde = $('#partyAvgGenel_' + $(al).attr('id').split('_')[1]);
                    toplamoran = parseFloat(toplamoran) + parseFloat(yuzde.val());
                }

            })
            $('#ittifak' + i + 'Toplam').html(toplamoran.toFixed(2));
        }
    }
}


function genelTahminOranDegisti() {
    var toplam = 0;
    $('#tblBodyGenel').find('input[type=text]').each(function (i, al) {
        toplam = parseFloat(toplam) + parseFloat($(al).val());
    })
    $('#digerToplam').html((100 - toplam.toFixed(2)).toFixed(2));
    if ((100 - toplam.toFixed(2)).toFixed(2) < 0) {
        $('#digerToplam').css("color", "red");
        $('#genelOyTahminToplam').html(toplam.toFixed(2));
    } else {
        $('#digerToplam').css("color", "green");
        $('#genelOyTahminToplam').html(100);
    }

    for (var i = 1; i < 6; i++) {
        var toplamoran = 0;
        $('#tblBodyGenel').find('input[id*=ittifak' + i + ']').each(function (i, al) {
            if ($(al).is(':checked')) {
                var yuzde = $('#partyAvgGenel_' + $(al).attr('id').split('_')[1]);
                toplamoran = parseFloat(toplamoran) + parseFloat(yuzde.val());
            }

        })
        $('#ittifak' + i + 'Toplam').html(toplamoran.toFixed(2));
    }


}



function similuasyonHesaplagenel() {
    //$('#simContainer1').hide();
    //$('#simContainer2').hide();
    blockUI();

    var secim = $('input[name=choose_poll]:checked').val();
    var partileryazildi = false;


    var secimId = $('#selectSecimId').val();
    var ideologies = [];
    var secilenPartilerIdler = [];
    var partyIdeologies;

    if (secim == "poll") {
        secimId = 11;
    } else {

        secimId = $('#selectSecimId').val();
    }
    $.ajaxSetup({ async: false });

    $('#tblBodyGenel tr').each(function (o, jk) {

        if (($('#tblBodyGenel').find('tr').length - 1) != o && ($('#tblBodyGenel').find('tr').length - 2) != o) {
            secilenPartilerIdler.push($(jk).attr('id').split('_')[1]);
        }
    });


    $.post('/Simulations/GetPartyIdeolojies', { partiler: secilenPartilerIdler })
        .done(function (response) {
            partyIdeologies = response;

        });

    $.post('/Simulations/GetParties')
        .done(function (responseDbPartiler) {
            dbPartiler = responseDbPartiler;
            $('#sonucTabloHead').empty();
            $('#tblBodySonuc').empty();
            $('#sonucTabloHeadAltBaslik').empty();
        });

    var dataArray = [];

    var simuleEdikcelIller = [];
    $('#iller input:checked').each(function (i, el) {
        var obj = {
            secimIlId: returnIller.find(a => a.regionId == $(el).val()).secimIllerId,
            cevreId: $(el).val(),
            secimturuId: secimId,
            secimdetay: secimdetay.id,
            klasikIlId: returnIller.find(a => a.regionId == $(el).val()).id
        }
        simuleEdikcelIller.push(obj);
    });
    var simuleIllerString = JSON.stringify(simuleEdikcelIller);



    $.post('/Simulations/TumIllerSonuclar', { ilVeriler: simuleIllerString })
        .done(function (response3) {
            for (var i = 0; i < response3.length; i++) {
                var partyTahminOylari = [];
                var ideolojiTahminOyCarpım = [];
                //var ilId = returnIller.find(a => a.regionId == $(el).val()).secimIllerId;
                var partilerMilVek = [];
                var hesaplanmisMilVek = [];
                var toplamGecerliOy = 0;
                var milVelSayisi = 0;
                milVelSayisi = response3[i][0];
                toplamGecerliOy = response3[i][2].reduce(function (s, a) {
                    return s + a.gecerli;
                }, 0);

                $('#tblBodyGenel tr').each(function (o, jk) {
                    if (($('#tblBodyGenel').find('tr').length - 1) != o && ($('#tblBodyGenel').find('tr').length - 2) != o) {
                        var tahminiOran = $(jk).find('input[type=text]').val();
                        var obj = {
                            partiId: $(jk).attr('id').split('_')[1],
                            oran: tahminiOran,
                        };
                        partyTahminOylari.push(obj);
                    }
                });


                for (var k = 0; k < partyIdeologies.length; k++) {
                    var partyOy = 0;

                    if (secim == "poll") {
                        partyOy = partyTahminOylari.find(a => a.partiId == partyIdeologies[k].partyId).oran;
                    } else {

                        var oncekiSecimOyOran = parseFloat($('#genelPartiTr_' + partyIdeologies[k].partyId).children().eq(1).text());
                        if (oncekiSecimOyOran != 0) {
                            var tahminOyOran = parseFloat(partyTahminOylari.find(a => a.partiId == partyIdeologies[k].partyId).oran);
                            var ilOyOran = parseFloat((parseFloat(response3[i][3].find(a => a.partyId == partyIdeologies[k].partyId).vote) / parseFloat(toplamGecerliOy)) * 100);
                            partyOy = (ilOyOran * tahminOyOran) / oncekiSecimOyOran;
                        } else {
                            partyOy = partyOy = partyTahminOylari.find(a => a.partiId == partyIdeologies[k].partyId).oran;
                        }
                    }
                    var value = partyIdeologies[k].value;
                    var ideolojicarpimOran = (parseFloat(partyOy) * parseFloat(value)) / 100;
                    var obj = {
                        ideoCarpim: ideolojicarpimOran,
                        ideoId: partyIdeologies[k].politicialIdeologyId,
                        partyid: partyIdeologies[k].partyId,
                        objIlId: returnIller.find(a => a.regionId == simuleEdikcelIller[i].cevreId).id,
                    }
                    ideolojiTahminOyCarpım.push(obj);
                }



                var ideolojiCarpanlariOranlar = [];
                for (var k = 0; k < response3[i][4].data.length; k++) {
                    var ideolojiOranToplam = 0;
                    for (var l = 0; l < ideolojiTahminOyCarpım.length; l++) {
                        if (ideolojiTahminOyCarpım[l].ideoId == response3[i][4].data[k].id) {
                            ideolojiOranToplam = parseFloat(ideolojiOranToplam) + parseFloat(ideolojiTahminOyCarpım[l].ideoCarpim);
                        }
                    }
                    var obj = {
                        ideoId: response3[i][4].data[k].id,
                        toplam: ideolojiOranToplam,
                        carpan: (1 / parseFloat(ideolojiOranToplam)) * 100
                    }
                    ideolojiCarpanlariOranlar.push(obj);
                }


                for (var k = 0; k < ideolojiTahminOyCarpım.length; k++) {
                    ideolojiTahminOyCarpım[k].ideoCarpim = parseFloat(ideolojiTahminOyCarpım[k].ideoCarpim) * parseFloat(ideolojiCarpanlariOranlar.find(a => a.ideoId == ideolojiTahminOyCarpım[k].ideoId).carpan);
                }


                var partySonucSecmenSayilari = [];
                var sonucSecmenToplamSayi = 0;
                for (var l = 0; l < ideolojiTahminOyCarpım.length; l++) {
                    var secmenSyi = (parseFloat(ideolojiTahminOyCarpım[l].ideoCarpim) / 100) * response3[i][4].data.find(a => a.id == ideolojiTahminOyCarpım[l].ideoId).ratioAvg;
                    sonucSecmenToplamSayi = parseFloat(sonucSecmenToplamSayi) + parseInt(parseFloat(secmenSyi).toFixed(0));
                    var obj = {
                        secmenSayi: parseFloat(secmenSyi).toFixed(0),
                        partySonucId: ideolojiTahminOyCarpım[l].partyid,
                    }
                    partySonucSecmenSayilari.push(obj);
                }



                var ekranPartyOranSonuc = [];

                for (var k = 0; k < secilenPartilerIdler.length; k++) {
                    var partyOytoplam = 0
                    for (var l = 0; l < partySonucSecmenSayilari.length; l++) {
                        if (secilenPartilerIdler[k] == partySonucSecmenSayilari[l].partySonucId) {
                            partyOytoplam = parseFloat(partyOytoplam) + parseFloat(partySonucSecmenSayilari[l].secmenSayi);
                        }

                    }
                    var oran = (parseFloat(partyOytoplam) / parseFloat(sonucSecmenToplamSayi)) * 100;
                    var obj = {
                        ekranaYazilcakPartyId: secilenPartilerIdler[k],
                        ekranPartyOran: oran.toFixed(2),
                        ekranilId: returnIller.find(a => a.regionId == simuleEdikcelIller[i].cevreId).id,
                    }
                    ekranPartyOranSonuc.push(obj);
                }

                if (partileryazildi == false) {
                    var td = '<th rowspan="2">İller</th>'
                    $('#sonucTabloHead').append(td);
                    for (var j = 0; j < ekranPartyOranSonuc.length; j++) {
                        td = '<th colspan="2">' + dbPartiler.find(a => a.id == ekranPartyOranSonuc[j].ekranaYazilcakPartyId).partyNameShort + '</th>'
                        $('#sonucTabloHead').append(td);
                        td = '<th>Oy</th><th>Mv.</th>';
                        $('#sonucTabloHeadAltBaslik').append(td);
                    }
                    partileryazildi = true;
                }

                for (var j = 0; j < ekranPartyOranSonuc.length; j++) {
                    var obj = { partiId: ekranPartyOranSonuc[j].ekranaYazilcakPartyId, oyOran: parseFloat(parseFloat(toplamGecerliOy) * (parseFloat(ekranPartyOranSonuc[j].ekranPartyOran) / 100)).toFixed(0), milSay: 0 };
                    var obj2 = { partiId: ekranPartyOranSonuc[j].ekranaYazilcakPartyId, oyOran: parseFloat(parseFloat(toplamGecerliOy) * (parseFloat(ekranPartyOranSonuc[j].ekranPartyOran) / 100)).toFixed(0), milSay: 0 };

                    var itt = $('[id ^=ittifak][id $=_' + ekranPartyOranSonuc[j].ekranaYazilcakPartyId + ']:checked');
                    if (itt.length != 0) {
                        //var ittId = $(itt).attr('id').split('_')[0];
                        //var ittToplamOran = parseFloat($('#' + ittId + 'Toplam').text());
                        //if (ittToplamOran>0) {
                        //    partilerMilVek.push(obj);
                        //    hesaplanmisMilVek.push(obj2);
                        //}
                        partilerMilVek.push(obj);
                        hesaplanmisMilVek.push(obj2);
                    } else {
                        var kiyasOran = parseFloat($('input[id="partyAvgGenel_' + ekranPartyOranSonuc[j].ekranaYazilcakPartyId + '"]').val());
                        if (kiyasOran > 7) {
                            partilerMilVek.push(obj);
                            hesaplanmisMilVek.push(obj2);
                        }
                    }


                }



                for (var j = 1; j <= milVelSayisi; j++) {
                    var max = hesaplanmisMilVek.reduce(function (prev, current) {
                        if (+current.oyOran > +prev.oyOran) {
                            return current;
                        } else {
                            return prev;
                        }
                    });
                    var milkazananparti = max.partiId;
                    hesaplanmisMilVek.find(a => a.partiId == milkazananparti).milSay = parseFloat(hesaplanmisMilVek.find(a => a.partiId == milkazananparti).milSay) + 1;
                    hesaplanmisMilVek.find(a => a.partiId == milkazananparti).oyOran = parseFloat(parseFloat(partilerMilVek.find(a => a.partiId == milkazananparti).oyOran) / (parseFloat(hesaplanmisMilVek.find(a => a.partiId == milkazananparti).milSay) + 1)).toFixed(0);
                }


                for (var j = 0; j < ekranPartyOranSonuc.length; j++) {
                    var kayitVarmi = hesaplanmisMilVek.find(a => a.partiId == ekranPartyOranSonuc[j].ekranaYazilcakPartyId);
                    if (!kayitVarmi) {
                        var obj = { partiId: ekranPartyOranSonuc[j].ekranaYazilcakPartyId, oyOran: parseFloat(parseFloat(toplamGecerliOy) * (parseFloat(ekranPartyOranSonuc[j].ekranPartyOran) / 100)).toFixed(0), milSay: 0 };
                        var obj2 = { partiId: ekranPartyOranSonuc[j].ekranaYazilcakPartyId, oyOran: parseFloat(parseFloat(toplamGecerliOy) * (parseFloat(ekranPartyOranSonuc[j].ekranPartyOran) / 100)).toFixed(0), milSay: 0 };
                        partilerMilVek.push(obj);
                        hesaplanmisMilVek.push(obj2);
                    }
                }



                for (var j = 0; j < iller.length; j++) {
                    if (iller[j] == simuleEdikcelIller[i].cevreId) {
                        var tr = '<tr><td>' + $('#il_' + simuleEdikcelIller[i].cevreId).parent().find('label').html() + '( Geç.Oy.Say. : ' + toplamGecerliOy.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.") + ') </td>';
                        for (var k = 0; k < ekranPartyOranSonuc.length; k++) {
                            tr += '<td>' + ekranPartyOranSonuc[k].ekranPartyOran + '</td><td>' + hesaplanmisMilVek.find(a => a.partiId == ekranPartyOranSonuc[k].ekranaYazilcakPartyId).milSay + '</td>'
                        }
                        tr += '</tr>';
                        $('#tblBodySonuc').append(tr);
                    }
                }


                genelOylarSecim = [];
                for (var j = 0; j < response3[i][3].length; j++) {
                    var obj = {
                        oran: parseFloat((parseFloat(response3[i][3][j].vote) / parseFloat(toplamGecerliOy)) * 100).toFixed(2),
                        partiId: response3[i][3][j].partyId,
                    }
                    genelOylarSecim.push(obj);
                }

                var data = {
                    data: {
                        dataSonucPartyi: ekranPartyOranSonuc,
                        iller: iller,
                        ilAdi: $('#il_' + simuleEdikcelIller[i].cevreId).parent().find('label').html(),
                        gecOySayi: toplamGecerliOy,
                        hesapMil: hesaplanmisMilVek,
                        topMilSay: milVelSayisi,
                        genelOylarAnket: genelOylarAnket,
                        genelOylarSecim: genelOylarSecim,
                        partyTahminOylar: partyTahminOylari,
                    }

                };
                dataArray.push(data);
                if ($('#iller input:checked').length == (i + 1)) {
                    var ittifaklar = [];
                    for (var s = 1; s < 6; s++) {

                        $('#tblBodyGenel').find('input[id*=ittifak' + s + ']').each(function (x, al) {
                            if ($(al).is(':checked')) {
                                var ittifakpartiid = $(al).attr('id').split('_')[1];
                                var ittifak = {
                                    ittifakno: 'ittifak_' + s,
                                    ittifakparti: ittifakpartiid,
                                    ittifakname: $('#ittifakName' + s).val(),
                                }
                                ittifaklar.push(ittifak);
                            }

                        })
                    }



                }


            }
            var simGraphic = JSON.stringify(dataArray);
            var itt = JSON.stringify(ittifaklar);
            $('#datasForinput').val(simGraphic);
            $('#ittForinput').val(itt);
            $('#simResultSubmit').submit();
        })
        .fail(() => {
            Swal.fire("Simulasyon", "İşlemler sırasında bir hata oluştu. Bu sunucu yoğunluğundan kaynaklanıyor olabilir. Lütfen tekrar deneyiniz.", "warning");
        });


    $.ajaxSetup({ async: true });
    unblockUI();
    var dbPartiler;








}


function selectAll(el) {
    var checked = false;
    var parent = $(el).parent().parent();
    $(parent).find('input').each((e, i) => {
        if (e == 0)
            checked = !$(i).is(':checked');
        $(i).prop('checked', checked);

    });
}