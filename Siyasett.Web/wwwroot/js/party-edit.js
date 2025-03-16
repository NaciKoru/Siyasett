var $image_crop;
var eid = parseInt('@Model.Id');

var cropper;
var $modal = $('#uploadimageModal');
var image = document.getElementById('image');
var input = document.getElementById('file1');

$(function () {

    $('#txtMetinAdres').summernote({
        placeholder: '',
        tabsize: 2,
        height: 400
    });


    $('#file1').on('change', function (e) {
        if ($('#file1').val() == '')
            return;

        var done = function (url) {
            input.value = '';
            image.src = url;
          
            $modal.modal('show');
        };

        var files = e.target.files;

        var reader;
        var file;
        var url;

        if (files && files.length > 0) {
            file = files[0];

            if (URL) {
                done(URL.createObjectURL(file));
            } else if (FileReader) {
                reader = new FileReader();
                reader.onload = function (e) {
                    done(reader.result);
                };
                reader.readAsDataURL(file);
            }
        }

    });

    $('#aspectButtons').on('change', 'input', function () {
        var $this = $(this);
        if (cropper) {
            cropper.setAspectRatio($this.val());
        }

    });

    $modal.on('shown.bs.modal', function () {
        cropper = new Cropper(image, {
            aspectRatio: 0.83,
            viewMode: 0,
        });
    }).on('hidden.bs.modal', function () {
        cropper.destroy();
        cropper = null;
    });
    document.getElementById('cropImage').addEventListener('click', function () {

        var canvas;
        if (cropper) {
            canvas = cropper.getCroppedCanvas({
                maxWidth: 2048,
                maxHeight: 2048,
            });
            $('#Photo').attr('src', canvas.toDataURL());
            $('#UploadPhoto').val(canvas.toDataURL());
        }
        $modal.modal('hide');
    });

    //load attachments
    $('#pnlAttachs').load('/Admin/Partials/PartyAttachments/' + partyId);




});

function resetPhoto() {
    $('#UploadPhoto').val('');
    $('#Photo').attr('src', '/images/person/user2.png');


}

function selectFile() {

    $('#file1').click();
}

function cancel_upload() {
    $('#file1').val('');

}

function district_changed() {
    $('#txtDistrict').val($('#ddlDistrict option:selected').text());

}
function province_changed() {

    $('#txtProvince').val($('#ddlProvince option:selected').text());
    if ($('#ddlDistrict').hasClass("select2-hidden-accessible"))
        $('#ddlDistrict').select2("destroy");

    $('#ddlDistrict').empty();
    $('#ddlDistrict').append('<option value=""></option>');
    $('#txtDistrict').val("");
    $('#ddlDistrict').select2();

    let countryId = parseInt($('#ddlCountry').val());
    if (countryId != 220)
        return;

    $('#ddlDistrict').val("");
    $('#ddlDistrict').trigger('change');

    var provinceId = $('#ddlProvince').val();
    if (provinceId == "")
        return;
    provinceId = parseInt(provinceId);

    getDistricts(provinceId);

}


function getDistricts(id, districtId) {

    $.get("/Admin/Partials/GetDistricts/" + id).done((response) => {
        let ddl = $('#ddlDistrict').empty();
        if (ddl.hasClass("select2-hidden-accessible"))
            ddl.select2("destroy");

        ddl.append('<option value=""></option>');

        for (var i = 0; i < response.length; i++) {
            ddl.append('<option value="' + response[i].id + '">' + response[i].name + '</option>');

        }

        ddl.select2();

        if (districtId) {
            ddl.val(districtId);
            ddl.trigger('change');

        }


    });

}

function country_changed() {

    $('#ddlProvince').val("");
    $('#txtProvince').val("");
    $('#ddlDistrict').val("");
    $('#txtDistrict').val("");


    $('#ddlProvince').trigger('change');
    $('#ddlDistrict').trigger('change');
    checkCountry();
}

function checkCountry() {
    let countryId = parseInt($('#ddlCountry').val());
    if (countryId == 220)//türkiye
    {
        $('#ddlProvince').show().select2();
        $('#txtProvince').hide();
        $('#ddlDistrict').show().select2();
        $('#txtDistrict').hide();
    }
    else {
        if ($('#ddlDistrict').hasClass("select2-hidden-accessible"))
            $('#ddlDistrict').select2("destroy");

        if ($('#ddlProvince').hasClass("select2-hidden-accessible"))
            $('#ddlProvince').select2("destroy");


        $('#ddlProvince').val('').hide();
        $('#txtProvince').show();
        $('#ddlDistrict').empty().append('<option value=""></option>').hide();
        $('#txtDistrict').show();
    }
}

function partyCreateAddress() {
    partyId: $('#modelIdDiv').attr('data-val');

    var address = {
        address1: $('#txtAddress1').val(),
        address2: $('#txtAddress2').val(),
        addressTypeId: parseInt($('#ddlAddresType').val()),


        district: $('#txtDistrict').val(),
        id: $('#modelIdDiv').attr('data-val'),
        countryId: parseInt($('#ddlCountry').val()),
        postalCode: $('#txtPostalCode').val(),
        provinceId: $('#ddlProvince').val(),
        districtId: $('#ddlDistrict').val(),
        province: $('#txtProvince').val(),

    };
    if (address.nameEn == '' || address.nameTr == '') {
        Swal.fire("Adres Ekle!", "Adres adı gereklidir!", "warning")
        return;
    }

    if (address.district == '' || address.province == '') {
        Swal.fire("Adres Ekle!", "Adres için il ve ilçe girilmelidir!", "warning")
        return;
    }
    if (address.address1 == '') {
        Swal.fire("Adres Ekle!", "Adres için en az 1 satırda giriş olmalı!", "warning")
        return;
    }
    blockUI();
    $.post('/admin/partymanagement/addaddress', { model: address, partyId: partyId }).done((response) => {
        unblockUI();
        if (response.success)
            $('#divAddresses').load('/admin/partials/PartyAddresses/' + partyId);
        else
            Swal.fire("Adres Ekle!", "Adres kayıt sırasında hata oluştu!", "warning")

    });
}


function partyCreatePhoneNumber() {
    var telefon = {
        phoneTypeId: $('#ddlPhoneType').val(),

        countryId: parseInt($('#phoneCountryId').val()),
        countryCode: $('#phoneCountryId option:selected').attr('data-code'),
        prefixCode: $('#txtTelOnKodu').val(),
        phoneNumber: $('#txtTelNo').val(),
        communicationTypeId: $('#ddlCommunicationType').val(),

    };

    if (telefon.phoneTypeId == '') {
        Swal.fire("Telefon Ekle!", "Telefon için tür girilmelidir!", "warning")
        return;
    }
    if (telefon.phoneType == '') {
        Swal.fire("Telefon Ekle!", "Telefon için tipi girilmelidir!", "warning")
        return;
    }
    if (telefon.countryCode == '') {
        Swal.fire("Telefon Ekle!", "Telefon için ülke kodu girilmelidir!", "warning")
        return;
    }
    if (telefon.phoneNumber == '') {
        Swal.fire("Telefon Ekle!", "Telefon için telefon no girilmelidir!", "warning")
        return;
    }
    blockUI();
    $.post('/admin/partymanagement/AddPhone', { model: telefon, id: partyId }).done((response) => {
        unblockUI();
        if (response.success) {
            $('#divPhones').load('/admin/partials/PartyPhones/' + partyId);
            $('#txtTelNo').val('');
            $('#txtTelOnKodu').val('');
            $('#ddlPhoneType').val('1');
            $('#ddlCommunicationType').val('1')
        }
        else
            Swal.fire("Adres Ekle!", "Adres kayıt sırasında hata oluştu!", "warning")

    });

}

function partyCreateEposta() {

    var eposta = {
        communicationTypeId: $('#ddlEpostaTuru').val(),
        emailAddress: $('#txtePosta').val(),
        parentId: partyId,
        id: $('#modelIdDiv').attr('data-val'),
    };
    if (!validMail(eposta.emailAddress)) {
        Swal.fire("Eposta Ekle!", "Geçerli bir e-posta adresi girilmelidir!", "warning")

        return;
    }

    if (eposta.EmailType == '') {
        Swal.fire("Eposta Ekle!", "Eposta için tip girilmelidir!", "warning")
        return;
    }
    if (eposta.Email == '') {
        Swal.fire("Eposta Ekle!", "Eposta  girilmelidir!", "warning")
        return;
    }
    blockUI();
    $.post('/admin/partymanagement/addemail', { model: eposta, id: partyId }).done((response) => {
        unblockUI();
        if (response.success)
            $('#divEmails').load('/admin/partials/PartyEmails/' + partyId);
        else
            Swal.fire("Adres Ekle!", "Adres kayıt sırasında hata oluştu!", "warning")

    });
}

function partyCreateSosyalMedya() {

    var SosyalMedia = {
        socialTypeId: $('#ddlSosyalTuru').val(),
        socialAddress: $('#txtSosyalAdres').val(),
        parentId: partyId,
    };
    if (SosyalMedia.socialTypeId == '') {
        Swal.fire("Sosyal Medya Adres Ekle!", "Adres için tip girilmelidir!", "warning")
        return;
    }
    if (SosyalMedia.socialAddress == '') {
        Swal.fire("Sosyal Medya Adres Ekle!", "Adres  girilmelidir!", "warning")
        return;
    }
    blockUI();
    $.post('/admin/partymanagement/addsocialmedia', { model: SosyalMedia, id: partyId }).done((response) => {
        unblockUI();
        if (response.success) {
            $('#divSocialMedia').load('/admin/partials/PartySocialMedias/' + partyId);
            $('#txtSosyalAdres').val('');
        }
        else
            Swal.fire("Adres Ekle!", "Adres kayıt sırasında hata oluştu!", "warning")

    });

}

function deleteAddress_click(id) {


    Swal.fire({
        title: "Emin misiniz?",
        text: "Adres kaydı silinecektir!",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Evet, sil!",
        cancelButtonText: "Hayır",

        closeOnConfirm: true
    }).then((result) => {
        if (result.isConfirmed) {
            blockUI();
            $.post('/admin/partymanagement/deleteadress', { id: id }).done((response) => {
                unblockUI();
                if (response.success) {
                    $('#tr_address_' + id).remove();
                    Swal.fire("Adres Sil!", "Adres kaydı başarıyla silindi!", "success")
                }
                else
                    Swal.fire("Adres Sil!", "Adres silme sırasında hata oluştu!", "warning")
            });
        }
    });


}

function deleteEmail_click(id) {



    Swal.fire({
        title: "Emin misiniz?",
        text: "E-Posta kaydı silinecektir!",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Evet, sil!",
        cancelButtonText: "Hayır",

        closeOnConfirm: true
    }).then((result) => {
        if (result.isConfirmed) {
            blockUI();
            $.post('/admin/partymanagement/deleteemail', { id: id }).done((response) => {
                unblockUI();
                if (response.success) {
                    $('#tr_emails_' + id).remove();
                    Swal.fire("E-Posta Sil!", "E-Posta kaydı başarıyla silindi!", "success")
                }
                else
                    Swal.fire("E-Posta Sil!", "E-posta silme sırasında hata oluştu!", "warning")
            });
        }
    });


}

function deletePhone_click(id) {

    Swal.fire({
        title: "Emin misiniz?",
        text: "Telefon kaydı silinecektir!",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Evet, sil!",
        cancelButtonText: "Hayır",

        closeOnConfirm: true
    }).then((result) => {
        if (result.isConfirmed) {
            blockUI();
            $.post('/admin/partymanagement/deletephone', { id: id }).done((response) => {
                unblockUI();
                if (response.success) {
                    $('#tr_phones_' + id).remove();
                    Swal.fire("Telefon Sil!", "Telefon kaydı başarıyla silindi!", "success")
                }
                else
                    Swal.fire("Telefon Sil!", "Telefon silme sırasında hata oluştu!", "warning")
            });
        }
    });



}

function deleteSocialMedia_click(id) {


    Swal.fire({
        title: "Emin misiniz?",
        text: "Sosyal Medya kaydı silinecektir!",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Evet, sil!",
        cancelButtonText: "Hayır",

        closeOnConfirm: true
    }).then((result) => {
        if (result.isConfirmed) {
            blockUI();
            $.post('/admin/partymanagement/deletesocialmedia', { id: id }).done((response) => {
                unblockUI();
                if (response.success) {
                    $('#tr_socialadress_' + id).remove();
                    Swal.fire("Sosyal Medya Adresi Sil!", "Sosyal Medya kaydı başarıyla silindi!", "success")
                }
                else
                    Swal.fire("Sosyal Medya Adresi Sil!", "Sosyal Medya silme sırasında hata oluştu!", "warning")
            });
        }
    });

}

let editAddressId = 0;
function editAddress_click(id) {
    editAddressId = id;

    $.post('/admin/partymanagement/editadressgetrecord', { id: id }).done((response) => {


        $('#ddlAddresType').val(response.addressTypeId);
        $('#txtAddress1').val(response.address1);
        $('#txtAddress2').val(response.address2);
        $('#ddlCountry').val(response.countryId);

        checkCountry();
        if (response.countryId == 220)//tr
        {

            $('#ddlProvince').val(response.provinceId).show();
            $('#ddlProvince').trigger('change');

            $('txtProvince').hide();
            $('txtDistrict').hide();
            getDistricts(response.provinceId, response.districtId);


        }
        else {
            $('#ddlProvince').hide();
            $('txtProvince').show().val(response.province);
            $('txtDistrict').show().val(response.district);

            $('#ddlDistrict').hide();
        }

        $('#ddlDistrict').val(response.district);


        $('#txtPostalCode').val(response.postalCode);
        $('#btnAdresEkle').hide();
        $('#btnAdresGuncelle').show();
        $('#btnAdresIptal').show();

    });
}

function partyAdressUpdate() {
    var address = {
        address1: $('#txtAddress1').val(),
        address2: $('#txtAddress2').val(),
        addressTypeId: parseInt($('#ddlAddresType').val()),


        district: $('#txtDistrict').val(),
        id: editAddressId,
        countryId: parseInt($('#ddlCountry').val()),
        postalCode: $('#txtPostalCode').val(),
        province: $('#txtProvince').val(),
        peopleId: $('#modelIdDiv').attr('data-val'),
        provinceId: $('#ddlProvince').val(),
        districtId: $('#ddlDistrict').val()
    };
    if (address.nameEn == '' || address.nameTr == '') {
        Swal.fire("Adres Ekle!", "Adres adı gereklidir!", "warning")
        return;
    }

    if (address.district == '' || address.province == '') {
        Swal.fire("Adres Ekle!", "Adres için il ve ilçe girilmelidir!", "warning")
        return;
    }
    if (address.address1 == '') {
        Swal.fire("Adres Ekle!", "Adres için en az 1 satırda giriş olmalı!", "warning")
        return;
    }
    blockUI();
    $.post('/admin/partymanagement/updateaddress', { model: address }).done((response) => {
        unblockUI();
        if (response.success) {
            $('#divAddresses').load('/admin/partials/PartyAddresses/' + partyId);
            $('#tab-adres').find('input').each(function (i, al) {
                $(al).val('');
            });

            $('#btnAdresEkle').show();
            $('#btnAdresGuncelle').hide();
            $('#btnAdresIptal').hide();
        }
        else
            Swal.fire("Adres Güncelle!", "Adres kaydı güncelleme sırasında hata oluştu!", "warning")

    });
}

let editEmailId = 0;
function editEmail_click(id) {
    editEmailId = id;
    $.get('/admin/partymanagement/GetEmail', { id: id }).done((response) => {
        $('#ddlEpostaTuru').val(response.communicationTypeId);
        $('#txtePosta').val(response.emailAddress);
        $('#btnEpostaEkle').hide();
        $('#btnEpostaGuncelle').show();
        $('#btnEpostaIptal').show();
        $('#btnEpostaGuncelle').attr('onclick', 'partyUpdateEposta(' + id + ')')
    });
}

function partyUpdateEposta() {
    var eposta = {
        communicationTypeId: $('#ddlEpostaTuru').val(),
        emailAddress: $('#txtePosta').val(),
        id: editEmailId,
    }
    if (!validMail(eposta.emailAddress)) {
        Swal.fire("Eposta Ekle!", "Geçerli bir e-posta adresi girilmelidir!", "warning")

        return;
    }
    blockUI();
    $.post('/admin/partymanagement/updateemail', { model: eposta }).done((response) => {
        unblockUI();
        if (response.success) {
            $('#divEmails').load('/admin/partials/PartyEmails/' + partyId);
            $('#txtePosta').val("");
            $('#ddlEpostaTuru').val("1");

            $('#btnEpostaEkle').show();
            $('#btnEpostaGuncelle').hide();
            $('#btnEpostaIptal').hide();
        }
        else
            Swal.fire("E-Posta Güncelle!", response.msg, "warning")

    });

}

let editPhoneId = 0;
function editPhone_click(id) {
    editPhoneId = id;
    $.get('/admin/partymanagement/editphonegetrecord', { id: id }).done((response) => {
        $('#ddlCommunicationType').val(response.communicationTypeId);
        $('#ddlPhoneType').val(response.phoneTypeId);
        $('#phoneCountryId').val(response.countryId).trigger('change');
        $('#txtTelOnKodu').val(response.prefixCode);
        $('#txtTelNo').val(response.phoneNumber);

        $('#btnTelefonEkle').hide();
        $('#btnTelefonGuncelle').show();
        $('#btnTelefonIptal').show();

    });
}

function partyUpdatePhoneNumber() {
    var telefon = {
        phoneTypeId: $('#ddlPhoneType').val(),
        communicationTypeId: $('#ddlCommunicationType').val(),
        countryId: parseInt($('#phoneCountryId').val()),
        countryCode: $('#phoneCountryId option:selected').attr('data-code'),
        prefixCode: $('#txtTelOnKodu').val(),
        phoneNumber: $('#txtTelNo').val(),
        partyId: partyId,
        id: editPhoneId,
    };

    if (telefon.PhoneTypeId == '') {
        Swal.fire("Telefon Ekle!", "Telefon için tür girilmelidir!", "warning")
        return;
    }
    if (telefon.PhoneType == '') {
        Swal.fire("Telefon Ekle!", "Telefon için tipi girilmelidir!", "warning")
        return;
    }
    if (telefon.CountryCode == '') {
        Swal.fire("Telefon Ekle!", "Telefon için ülke kodu girilmelidir!", "warning")
        return;
    }
    if (telefon.PhoneNumber == '') {
        Swal.fire("Telefon Ekle!", "Telefon için telefon no girilmelidir!", "warning")
        return;
    }
    blockUI();
    $.post('/admin/partymanagement/updatephone', { model: telefon }).done((response) => {
        unblockUI();
        if (response.success) {
            $('#divPhones').load('/admin/partials/PartyPhones/' + partyId);
            $('#tab-telefonlar').find('input').each(function (i, al) {
                $(al).val('');
            });
            $('#tab-telefonlar').find('select').each(function (i, al) {
                $(al).val('');
            });
            $('#btnTelefonEkle').show();
            $('#btnTelefonGuncelle').hide();
            $('#btnTelefonIptal').hide();
        }
        else
            Swal.fire("Telefon Güncelle!", "Telefon kaydı güncelleme sırasında hata oluştu!", "warning")

    });
}

let editSocialMediaId = 0;
function editSocialMedia_click(id) {
    editSocialMediaId = id;
    $.get('/admin/partymanagement/editsocialmediagetrecord', { id: id }).done((response) => {
        $('#ddlSosyalTuru').val(response.socialTypeId);
        $('#txtSosyalAdres').val(response.socialAddress);


        $('#btnSosyalAdresEkle').hide();
        $('#btnSosyalAdresGuncelle').show();
        $('#btnSosyalAdresIptal').show();

    });
}

function partyUpdateSosyalMedya() {

    var SosyalMedia = {
        socialTypeId: $('#ddlSosyalTuru').val(),
        socialAddress: $('#txtSosyalAdres').val(),
        parentId: $('#modelIdDiv').attr('data-val'),
        Id: editSocialMediaId,
    };
    if (SosyalMedia.socialTypeId == '') {
        Swal.fire("Sosyal Medya Adres Ekle!", "Adres için tip girilmelidir!", "warning")
        return;
    }
    if (SosyalMedia.socialAddress == '') {
        Swal.fire("Sosyal Medya Adres Ekle!", "Adres  girilmelidir!", "warning")
        return;
    }
    blockUI();
    $.post('/admin/partymanagement/updatesocialmedia', { model: SosyalMedia }).done((response) => {
        unblockUI();
        if (response.success) {
            $('#divSocialMedia').load('/admin/partials/PartySocialMedias/' + partyId);
            $('#tab-social').find('input').each(function (i, al) {
                $(al).val('');
            });
            $('#tab-social').find('select').each(function (i, al) {
                $(al).val('');
            });
            $('#btnSosyalAdresEkle').show();
            $('#btnSosyalAdresGuncelle').hide();
            $('#btnSosyalAdresIptal').hide();
        }
        else
            Swal.fire("Sosyal Medya Adresi Güncelle!", "Sosyal medya adresi güncelleme sırasında hata oluştu!", "warning")

    });
}

function partyAddressUpCancel() {
    $('#tab-adres').find('input').each(function (i, al) {
        $(al).val('');
    });
    $('#tab-adres').find('select').each(function (i, al) {
        $(al).val('');
    });
    $('#btnAdresEkle').show();
    $('#btnAdresGuncelle').hide();
    $('#btnAdresIptal').hide();
}

function partyUpdateCancelPhoneNumber() {
    $('#tab-telefonlar').find('input').each(function (i, al) {
        $(al).val('');
    });
    $('#tab-telefonlar').find('select').each(function (i, al) {
        $(al).val('');
    });
    $('#btnTelefonEkle').show();
    $('#btnTelefonGuncelle').hide();
    $('#btnTelefonIptal').hide();
}

function partyUpdateCancelEposta() {
    $('#tab-eposta').find('input').each(function (i, al) {
        $(al).val('');
    });
    $('#tab-eposta').find('select').each(function (i, al) {
        $(al).val('');
    });
    $('#btnEpostaEkle').show();
    $('#btnEpostaGuncelle').hide();
    $('#btnEpostaIptal').hide();
}

function partyUpdateCancelSosyalMedya() {
    $('#tab-social').find('input').each(function (i, al) {
        $(al).val('');
    });
    $('#tab-social').find('select').each(function (i, al) {
        $(al).val('');
    });
    $('#btnSosyalAdresEkle').show();
    $('#btnSosyalAdresGuncelle').hide();
    $('#btnSosyalAdresIptal').hide();
}

function partyUpdateCancelText() {
    $('#txtMetinAdres').summernote('code', '');
    
    $('#btnTextEkle').show();
    $('#btnTextGuncelle').hide();
    $('#btnTextIptal').hide();
}

function uploadAttachment_click() {

    let nameTr = $('#txtAttachmentNameTr').val();
    let nameEn = $('#txtAttachmentNameEn').val();

    var file = $('#fileAttach')[0].files[0];
    var fd = new FormData();
    fd.append('formFile', file);
    fd.append('partyId', partyId);
    fd.append('nameTr', nameTr);
    fd.append('nameEn', nameEn);
    blockUI();
    $.ajax({
        url: '/Admin/PartyManagement/UploadAttachment',
        type: 'post',
        data: fd,
        contentType: false,
        processData: false,
        success: function (response) {
            unblockUI();
            if (response.success) {
                Swal.fire('Dosya Ekle', 'Dosya başarıyla yüklendi.', 'success');
                $('#fileAttach').val('');
                $('#pnlAttachs').load('/Admin/Partials/PartyAttachments/' + partyId);
                $('#txtAttachmentNameTr').val('');
                $('#txtAttachmentNameEn').val('');
            }
            else {
                Swal.fire('Dosya Ekle', response.msg, 'warning');
            }
        },
    });


}

function deleteAttachment_click(id) {

    Swal.fire({
        title: "Emin misiniz?",
        text: "Partinin ekli dosyası silinecektir!",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Evet, sil!",
        cancelButtonText: "Hayır",

        closeOnConfirm: true
    }).then((result) => {
        if (result.isConfirmed) {
            blockUI();
            $.post('/Admin/PartyManagement/DeleteAttachment/', { id: id })
                .done((response) => {
                    unblockUI();
                    if (response.success) {
                        Swal.fire("Silindi!", "Ekli dosya başarıyla silindi.", "success");
                        $('#pnlAttachs').load('/Admin/Partials/PartyAttachments/' + partyId);
                    }
                });
        }
    });

}

function partyCreateText() {
    var text = {
        SectorId: $('#ddlMetinTuru').val(),
        Text: $('#txtMetinAdres').val(),
        parentId: partyId,
    };
    console.log($('#ddlMetinTuru').val());
    if (text.SectorId == '') {
        Swal.fire("Metin Ekle!", "Sektör girilmelidir!", "warning")
        return;
    }
    if (text.Text == '') {
        Swal.fire("Metin Ekle!", "Metin  girilmelidir!", "warning")
        return;
    }
    blockUI();
    $.post('/admin/partymanagement/addtext', { model: text, id: partyId }).done((response) => {
        unblockUI();
        if (response.success) {
            $('#divMetin').load('/admin/partials/PartyTexts/' + partyId);
            $('#ddlMetinTuru option').prop('selected', false);
            $('#ddlMetinTuru').trigger('change.select2');
            $('#txtMetinAdres').summernote('code', '');
        }
        else
            Swal.fire("Adres Ekle!", "Adres kayıt sırasında hata oluştu!", "warning")

    });
}

let edittextId = 0;
function editPartyText_click(id) {
    edittextId = id;
    $.get('/admin/partymanagement/edittextgetrecord', { id: id }).done((response) => {

        $('#ddlMetinTuru option').prop('selected', false);
        $('#ddlMetinTuru').trigger('change.select2');
        for (var i = 0; i < response[0].sectorId.length; i++) {
            
            $('#ddlMetinTuru option[value="' + response[0].sectorId[i] + '"]').prop('selected', true);
            $('#ddlMetinTuru').trigger('change.select2');

        }

        $("#txtMetinAdres").summernote('code',response[0].text);

        $('#btnTextEkle').hide();
        $('#btnTextGuncelle').show();
        $('#btnTextIptal').show();

    });
}

function partyUpdateText() {

    var Text = {
        SectorId: $('#ddlMetinTuru').val(),
        Text: $('#txtMetinAdres').val(),
        Partyid: $('#modelIdDiv').attr('data-val'),
        Id: edittextId,
    };
    if (Text.SectorId == '') {
        Swal.fire("Metin Kaydı Ekle!", "Sektör seçilmelidir!", "warning")
        return;
    }
    if (Text.Text == '') {
        Swal.fire("Metin Kaydı Ekle!", "Metin  girilmelidir!", "warning")
        return;
    }
    blockUI();
    $.post('/admin/partymanagement/updatetext', { model: Text }).done((response) => {
        unblockUI();
        if (response.success) {
            $('#divMetin').load('/admin/partials/PartyTexts/' + partyId);
            $('#txtMetinAdres').summernote('code', '');
            $('#ddlMetinTuru option').prop('selected', false);
            $('#ddlMetinTuru').trigger('change.select2');
            $('#btnTextEkle').show();
            $('#btnTextGuncelle').hide();
            $('#btnTextIptal').hide();
        }
        else
            Swal.fire("Metin Kaydı Güncelle!", "Metin kaydını güncelleme sırasında hata oluştu!", "warning")

    });
}

function deletePartyText_click(id) {


    Swal.fire({
        title: "Emin misiniz?",
        text: "Metin kaydı silinecektir!",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Evet, sil!",
        cancelButtonText: "Hayır",

        closeOnConfirm: true
    }).then((result) => {
        if (result.isConfirmed) {
            blockUI();
            $.post('/admin/partymanagement/deletetext', { id: id }).done((response) => {
                unblockUI();
                if (response.success) {
                    $('#tr_partytexts_' + id).remove();
                    $('#txtMetinAdres').summernote('code', '');
                    Swal.fire("Metin Kaydını Sil!", "Metin Kaydı başarıyla silindi!", "success")
                }
                else
                    Swal.fire("Metin Kaydını Sil!", "Metin Kaydı silme sırasında hata oluştu!", "warning")
            });
        }
    });

}

$('#ddlSosyalTuru').on('change', function () {
    var value = this.value;
    if (value == 1) {
        $('#facebook').hide();
        $('#instagram').hide();
        $('#linkedin').hide();
        $('#twitter').show();
        $('#google').hide();
        $('#wikipedia').hide();
        $('#youtube').hide();

    }
    else if (value == 2) {
        $('#facebook').show();
        $('#instagram').hide();
        $('#linkedin').hide();
        $('#twitter').hide();
        $('#google').hide();
        $('#wikipedia').hide();
        $('#youtube').hide();
    }
    else if (value == 3) {
        $('#facebook').hide();
        $('#instagram').hide();
        $('#linkedin').hide();
        $('#twitter').hide();
        $('#google').hide();
        $('#wikipedia').hide();
        $('#youtube').show();
    } else if (value == 4) {
        $('#facebook').hide();
        $('#instagram').show();
        $('#linkedin').hide();
        $('#twitter').hide();
        $('#google').hide();
        $('#wikipedia').hide();
        $('#youtube').hide();
    } else if (value == 6) {
        $('#facebook').hide();
        $('#instagram').hide();
        $('#linkedin').hide();
        $('#twitter').hide();
        $('#google').show();
        $('#wikipedia').hide();
        $('#youtube').hide();
    } else if (value == 7) {
        $('#facebook').hide();
        $('#instagram').hide();
        $('#linkedin').hide();
        $('#twitter').hide();
        $('#google').hide();
        $('#wikipedia').show();
        $('#youtube').hide();
    } else if (value == 8) {
        $('#facebook').hide();
        $('#instagram').hide();
        $('#linkedin').show();
        $('#twitter').hide();
        $('#google').hide();
        $('#wikipedia').hide();
        $('#youtube').hide();
    }
})


function socialSearch(firstname, type) {

    var links = ['https://twitter.com/search?q=' + firstname, 'https://facebook.com/search/top?q=' + firstname, 'https://www.google.com/search?q=' + firstname  + '+' + ' web sitesi',
    'https://www.linkedin.com/pub/dir?firstName=' + firstname + '&trk=people-guest_people-search-bar_search-submit', 'https://tr.wikipedia.org/w/index.php?search=' + firstname + '&title=%C3%96zel%3AAra&ns0=1',
    'https://www.youtube.com/results?search_query=' + firstname, 'https://www.google.com/search?q=' + firstname+' web sitesi']

    if (type == 'Facebook') {
        window.open(links[1], '_blank');
    }
    else if (type == 'Instagram') {
        window.open(links[2], '_blank');
    }
    else if (type == 'Linkedin') {
        window.open(links[3], '_blank');
    } else if (type == 'Google') {
        window.open(links[6], '_blank');
    } else if (type == 'Wikipedia') {
        window.open(links[4], '_blank');
    } else if (type == 'Youtube') {
        window.open(links[5], '_blank');
    }
    else if (type == 'Twitter') {
        window.open(links[0], '_blank');
    }
}