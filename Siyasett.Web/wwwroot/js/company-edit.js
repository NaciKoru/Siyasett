var $image_crop;
var eid = parseInt('@Model.Id');


$(function () {


    $image_crop = $('#image_demo').croppie({
        enableExif: false,
        viewport: {
            width: 240,
            height: 288,
            type: 'square' //circle
        },
        boundary: {
            width: 350,
            height: 350
        }
    });

    $('#file1').on('change', function () {
        if ($('#file1').val() == '')
            return;

        var reader = new FileReader();
        reader.onload = function (event) {
            $image_crop.croppie('bind', {
                url: event.target.result
            }).then(function () {
                console.log('jQuery bind complete');
            });
        }
        reader.readAsDataURL(this.files[0]);
        $('#uploadimageModal').modal('show');
    });

    $('.crop_image').click(function (event) {

        $image_crop.croppie('result', {
            type: 'canvas',
            size: 'original'
        }).then(function (response) {
            $('#UploadPhoto').val(response);
            $('#Photo').attr('src', response);
            $('#uploadimageModal').modal('hide');


        })
    });

    //load attachments
    $('#pnlAttachs').load('/Admin/Partials/CompanyAttachments/' + companyId);



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

function companyCreateAddress() {
    companyId: $('#modelIdDiv').attr('data-val');

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
    $.post('/admin/companymanagement/addaddress', { model: address, companyId: companyId }).done((response) => {
        unblockUI();
        if (response.success)
            $('#divAddresses').load('/admin/partials/CompanyAddresses/' + companyId);
        else
            Swal.fire("Adres Ekle!", "Adres kayıt sırasında hata oluştu!", "warning")

    });
}


function companyCreatePhoneNumber() {
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
    $.post('/admin/companymanagement/AddPhone', { model: telefon, id: companyId }).done((response) => {
        unblockUI();
        if (response.success) {
            $('#divPhones').load('/admin/partials/CompanyPhones/' + companyId);
            $('#txtTelNo').val('');
            $('#txtTelOnKodu').val('');
            $('#ddlPhoneType').val('1');
            $('#ddlCommunicationType').val('1')
        }
        else
            Swal.fire("Adres Ekle!", "Adres kayıt sırasında hata oluştu!", "warning")

    });

}

function companyCreateEposta() {

    var eposta = {
        communicationTypeId: $('#ddlEpostaTuru').val(),
        emailAddress: $('#txtePosta').val(),
        parentId: companyId,
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
    $.post('/admin/companymanagement/addemail', { model: eposta, id: companyId }).done((response) => {
        unblockUI();
        if (response.success)
            $('#divEmails').load('/admin/partials/CompanyEmails/' + companyId);
        else
            Swal.fire("Adres Ekle!", "Adres kayıt sırasında hata oluştu!", "warning")

    });
}

function companyCreateSosyalMedya() {

    var SosyalMedia = {
        socialTypeId: $('#ddlSosyalTuru').val(),
        socialAddress: $('#txtSosyalAdres').val(),
        parentId: companyId,
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
    $.post('/admin/companymanagement/addsocialmedia', { model: SosyalMedia, id: companyId }).done((response) => {
        unblockUI();
        if (response.success) {
            $('#divSocialMedia').load('/admin/partials/CompanySocialMedias/' + companyId);
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
            $.post('/admin/companymanagement/deleteadress', { id: id }).done((response) => {
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
            $.post('/admin/companymanagement/deleteemail', { id: id }).done((response) => {
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
            $.post('/admin/companymanagement/deletephone', { id: id }).done((response) => {
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
            $.post('/admin/companymanagement/deletesocialmedia', { id: id }).done((response) => {
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

    $.post('/admin/companymanagement/editadressgetrecord', { id: id }).done((response) => {


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

function companyAdressUpdate() {
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
    $.post('/admin/companymanagement/updateaddress', { model: address }).done((response) => {
        unblockUI();
        if (response.success) {
            $('#divAddresses').load('/admin/partials/CompanyAddresses/' + companyId);
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
    $.get('/admin/companymanagement/GetEmail', { id: id }).done((response) => {
        $('#ddlEpostaTuru').val(response.communicationTypeId);
        $('#txtePosta').val(response.emailAddress);
        $('#btnEpostaEkle').hide();
        $('#btnEpostaGuncelle').show();
        $('#btnEpostaIptal').show();
        $('#btnEpostaGuncelle').attr('onclick', 'companyUpdateEposta(' + id + ')')
    });
}

function companyUpdateEposta() {
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
    $.post('/admin/companymanagement/updateemail', { model: eposta }).done((response) => {
        unblockUI();
        if (response.success) {
            $('#divEmails').load('/admin/partials/CompanyEmails/' + companyId);
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
    $.get('/admin/companymanagement/editphonegetrecord', { id: id }).done((response) => {
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

function companyUpdatePhoneNumber() {
    var telefon = {
        phoneTypeId: $('#ddlPhoneType').val(),
        communicationTypeId: $('#ddlCommunicationType').val(),
        countryId: parseInt($('#phoneCountryId').val()),
        countryCode: $('#phoneCountryId option:selected').attr('data-code'),
        prefixCode: $('#txtTelOnKodu').val(),
        phoneNumber: $('#txtTelNo').val(),
        companyId: companyId,
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
    $.post('/admin/companymanagement/updatephone', { model: telefon }).done((response) => {
        unblockUI();
        if (response.success) {
            $('#divPhones').load('/admin/partials/CompanyPhones/' + companyId);
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
    $.get('/admin/companymanagement/editsocialmediagetrecord', { id: id }).done((response) => {
        $('#ddlSosyalTuru').val(response.socialTypeId);
        $('#txtSosyalAdres').val(response.socialAddress);


        $('#btnSosyalAdresEkle').hide();
        $('#btnSosyalAdresGuncelle').show();
        $('#btnSosyalAdresIptal').show();

    });
}

function companyUpdateSosyalMedya() {

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
    $.post('/admin/companymanagement/updatesocialmedia', { model: SosyalMedia }).done((response) => {
        unblockUI();
        if (response.success) {
            $('#divSocialMedia').load('/admin/partials/CompanySocialMedias/' + companyId);
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

function companyAddressUpCancel() {
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

function companyUpdateCancelPhoneNumber() {
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

function companyUpdateCancelEposta() {
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

function companyUpdateCancelSosyalMedya() {
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

function uploadAttachment_click() {

    let nameTr = $('#txtAttachmentNameTr').val();
    let nameEn = $('#txtAttachmentNameEn').val();

    var file = $('#fileAttach')[0].files[0];
    var fd = new FormData();
    fd.append('formFile', file);
    fd.append('companyId', company);
    fd.append('nameTr', nameTr);
    fd.append('nameEn', nameEn);
    blockUI();
    $.ajax({
        url: '/Admin/CompanyManagement/UploadAttachment',
        type: 'post',
        data: fd,
        contentType: false,
        processData: false,
        success: function (response) {
            unblockUI();
            if (response.success) {
                Swal.fire('Dosya Ekle', 'Dosya başarıyla yüklendi.', 'success');
                $('#fileAttach').val('');
                $('#pnlAttachs').load('/Admin/Partials/CompanyAttachments/' + companyId);
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
        text: "Kişinin ekli dosyası silinecektir!",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Evet, sil!",
        cancelButtonText: "Hayır",

        closeOnConfirm: true
    }).then((result) => {
        if (result.isConfirmed) {
            blockUI();
            $.post('/Admin/CompanyManagement/DeleteAttachment/', { id: id })
                .done((response) => {
                    unblockUI();
                    if (response.success) {
                        Swal.fire("Silindi!", "Ekli dosya başarıyla silindi.", "success");
                        $('#pnlAttachs').load('/Admin/Partials/PeopleAttachments/' + peopleId);
                    }
                });
        }
    });

}