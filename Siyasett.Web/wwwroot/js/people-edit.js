var $image_crop;
var eid = parseInt('@Model.Id');

var cropper;
var $modal = $('#uploadimageModal');
var image = document.getElementById('image');
var input = document.getElementById('file1');

$(function () {

    $('#CvTr,#CvEn').summernote({
        placeholder: 'CV',
        tabsize: 2,
        height: 400
    });

    $('#FirstName,#LastName').on('input', () => {
        $('#txtPersonName').val($('#FirstName').val() + " " + $('#LastName').val());
    })


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
    $('#pnlAttachs').load('/Admin/Partials/PeopleAttachments/' + peopleId);

    positionInstitution_onChange();

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


function province_changed2() {


    $('#ddlDistrictPosition').empty();
    $('#ddlDistrictPosition').append('<option value=""></option>');


    var provinceId = $('#ddlProvincePosition').val();
    if (provinceId == "")
        return;
    provinceId = parseInt(provinceId);

    getDistricts2(provinceId);

}

function getDistricts2(id, districtId) {

    $.get("/Admin/Partials/GetDistricts/" + id).done((response) => {
        let ddl = $('#ddlDistrictPosition').empty();

        ddl.append('<option value=""></option>');

        for (var i = 0; i < response.length; i++) {
            ddl.append('<option value="' + response[i].id + '">' + response[i].name + '</option>');

        }

        if (districtId) {
            ddl.val(districtId);
        }


    });

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

function peopleCreateAddress() {

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
        peopleId: $('#modelIdDiv').attr('data-val'),
    };


    if (address.district == '' || address.province == '') {
        Swal.fire("Adres Ekle!", "Adres için il ve ilçe girilmelidir!", "warning")
        return;
    }
    if (address.address1 == '') {
        Swal.fire("Adres Ekle!", "Adres için en az 1 satırda giriş olmalı!", "warning")
        return;
    }
    blockUI();
    $.post('/admin/peoplemanagement/addaddress', { model: address }).done((response) => {
        unblockUI();
        if (response.success)
            $('#divAddresses').load('/admin/partials/PeopleAddresses/' + address.id);
        else
            Swal.fire("Adres Ekle!", "Adres kayıt sırasında hata oluştu!", "warning")

    });
}


function peopleCreatePhoneNumber() {
    var telefon = {
        phoneTypeId: $('#ddlPhoneType').val(),
        countryId: parseInt($('#phoneCountryId').val()),
        countryCode: $('#phoneCountryId option:selected').attr('data-code'),
        prefixCode: $('#txtTelOnKodu').val(),
        phoneNumber: $('#txtTelNo').val(),
        peopleId: peopleId,
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
    $.post('/admin/peoplemanagement/AddPhone', { model: telefon }).done((response) => {
        unblockUI();
        if (response.success) {
            $('#divPhones').load('/admin/partials/PeoplePhones/' + peopleId);
            $('#txtTelNo').val('');
            $('#txtTelOnKodu').val('');
            $('#ddlPhoneType').val('1');
            $('#ddlCommunicationType').val('1')
        }
        else
            Swal.fire("Telefon Ekle!", "Telefon kayıt sırasında hata oluştu!", "warning")

    });

}

function peopleCreateEposta() {

    var eposta = {
        communicationTypeId: $('#ddlEpostaTuru').val(),
        emailAddress: $('#txtePosta').val(),
        parentId: peopleId,
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
    $.post('/admin/peoplemanagement/addemail', { model: eposta }).done((response) => {
        unblockUI();
        if (response.success)
            $('#divEmails').load('/admin/partials/PeopleEmails/' + peopleId);
        else
            Swal.fire("E-posta Ekle!", "E-posta kayıt sırasında hata oluştu!", "warning")

    });
}

function peopleCreateSosyalMedya() {

    var SosyalMedia = {
        socialTypeId: $('#ddlSosyalTuru').val(),
        socialAddress: $('#txtSosyalAdres').val(),
        parentId: $('#modelIdDiv').attr('data-val'),
        id: $('#modelIdDiv').attr('data-val'),
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
    $.post('/admin/peoplemanagement/addsocialmedia', { model: SosyalMedia }).done((response) => {
        unblockUI();
        if (response.success) {
            $('#divSocialMedia').load('/admin/partials/PeopleSocialMedias/' + peopleId);
            $('#txtSosyalAdres').val('');
        }
        else
            Swal.fire("Sosyal Hesap Ekle!", "Sosyal hesap kayıt sırasında hata oluştu!", "warning")

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
            $.post('/admin/peoplemanagement/deleteadress', { id: id }).done((response) => {
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
            $.post('/admin/peoplemanagement/deleteemail', { id: id }).done((response) => {
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
            $.post('/admin/peoplemanagement/deletephone', { id: id }).done((response) => {
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
            $.post('/admin/peoplemanagement/deletesocialmedia', { id: id }).done((response) => {
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

    $.post('/admin/peoplemanagement/editadressgetrecord', { id: id }).done((response) => {

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

function peopleAdressUpdate() {
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
    $.post('/admin/peoplemanagement/updateaddress', { model: address }).done((response) => {
        unblockUI();
        if (response.success) {
            $('#divAddresses').load('/admin/partials/PeopleAddresses/' + peopleId);
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
    $.get('/admin/peoplemanagement/GetEmail', { id: id }).done((response) => {
        $('#ddlEpostaTuru').val(response.communicationTypeId);
        $('#txtePosta').val(response.emailAddress);
        $('#btnEpostaEkle').hide();
        $('#btnEpostaGuncelle').show();
        $('#btnEpostaIptal').show();
        //$('#btnEpostaGuncelle').attr('onclick', 'peopleUpdateEposta(' + id + ')')
    });
}

function peopleUpdateEposta() {
    var eposta = {
        communicationTypeId: $('#ddlEpostaTuru').val(),
        emailAddress: $('#txtePosta').val(),
        id: editEmailId,
    }
    if (!validMail(eposta.emailAddress)) {
        Swal.fire("Eposta Ekle!", "Geçerli bir e-posta adresi girilmelidir!", "warning")

        return;
    }
    $.post('/admin/peoplemanagement/updateemail', { model: eposta }).done((response) => {
        if (response.success) {
            $('#divEmails').load('/admin/partials/PeopleEmails/' + peopleId);
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
    $.get('/admin/peoplemanagement/editphonegetrecord', { id: id }).done((response) => {
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

function peopleUpdatePhoneNumber() {
    var telefon = {
        phoneTypeId: $('#ddlPhoneType').val(),
        communicationTypeId: $('#ddlCommunicationType').val(),
        countryId: parseInt($('#phoneCountryId').val()),
        countryCode: $('#phoneCountryId option:selected').attr('data-code'),
        prefixCode: $('#txtTelOnKodu').val(),
        phoneNumber: $('#txtTelNo').val(),
        peopleId: peopleId,
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
    $.post('/admin/peoplemanagement/updatephone', { model: telefon }).done((response) => {
        unblockUI();
        if (response.success) {
            $('#divPhones').load('/admin/partials/PeoplePhones/' + peopleId);
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
    $.get('/admin/peoplemanagement/editsocialmediagetrecord', { id: id }).done((response) => {
        $('#ddlSosyalTuru').val(response.socialTypeId);
        $('#txtSosyalAdres').val(response.socialAddress);


        $('#btnSosyalAdresEkle').hide();
        $('#btnSosyalAdresGuncelle').show();
        $('#btnSosyalAdresIptal').show();

    });
}

function peopleUpdateSosyalMedya() {

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
    $.post('/admin/peoplemanagement/updatesocialmedia', { model: SosyalMedia }).done((response) => {
        unblockUI();
        if (response.success) {
            $('#divSocialMedia').load('/admin/partials/PeopleSocialMedias/' + peopleId);
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

function peopleAddressUpCancel() {
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

function peopleUpdateCancelPhoneNumber() {
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

function peopleUpdateCancelEposta() {
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

function peopleUpdateCancelSosyalMedya() {
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
    fd.append('peopleId', peopleId);
    fd.append('nameTr', nameTr);
    fd.append('nameEn', nameEn);
    blockUI();
    $.ajax({
        url: '/Admin/PeopleManagement/UploadAttachment',
        type: 'post',
        data: fd,
        contentType: false,
        processData: false,
        success: function (response) {
            unblockUI();
            if (response.success) {
                Swal.fire('Dosya Ekle', 'Dosya başarıyla yüklendi.', 'success');
                $('#fileAttach').val('');
                $('#pnlAttachs').load('/Admin/Partials/PeopleAttachments/' + peopleId);
                $('#txtAttachmentNameTr').val('');
                $('#txtAttachmentNameEn').val('');
            }
            else {
                Swal.fire('Dosya Ekle', response.msg, 'warning');
            }
        },
    });


}

/*
 **********************************************
 *          POSITIONS
 * 
 * */

function positionInstitution_onChange() {
    var i = parseInt($('#ddlInstitutionType').val());
    $('#txtPeriod').val('');
    $('#ddlDistrictPosition').val('').trigger('change');
    $('#ddlProvincePosition').val('');
    $('#divPositionProvince').hide();
    $('#divPositionDistrict').hide();
    $('#divPositionPeriod').hide();

    if (i === 1) {
        $('#ddlParty').hide().val('');
        $('#txtInstutionName').show();
    }
    else if (i == 6) {
        $('#ddlPositionId').val(29);
        $('#divPositionProvince').show();
        $('#divPositionDistrict').show();
    }
    else {
        if (i === 2)//TBMM
        {
            $('#divPositionPeriod').show();
            $('#ddlPositionId').val(59);
            $('#divPositionProvince').show();

        }

        $('#ddlParty').show();
        $('#txtInstutionName').hide().val('');


        if (i === 4) {
            $('#divPositionProvince').show();
            $('#ddlPositionId').val(28);
        }

        if (i === 5) {
            $('#ddlPositionId').val(29);
            $('#divPositionProvince').show();
            $('#divPositionDistrict').show();

        }


    }
}

function addPosition() {

    var model = getPositionFromUI();

    if (model.institutionTypeId == 1 && model.institutionName === "") {
        Swal.fire('Görevler', 'Kurum adını girmelisiniz.', 'warning');
        return;
    }
    if (model.institutionTypeId > 1 && isNaN(model.partyId)) {
        Swal.fire('Görevler', 'Parti seçimi yapılmamış.', 'warning');
        return;
    }
    model.partyId = isNaN(model.partyId) ? null : model.partyId;

    if (model.startDay > 31 || model.startMonth > 12) {
        Swal.fire('Görevler', 'Görev başlangıç tarihi hatalı.', 'warning');
        return;
    }
    if (model.endDay > 31 || model.endMonth > 12) {
        Swal.fire('Görevler', 'Görev bitiş tarihi hatalı.', 'warning');
        return;
    }

    blockUI();
    $.post('/Admin/PeopleManagement/AddPosition', { model: model })
        .done((response) => {
            unblockUI();
            if (response.success) {
                $('#divPositions').load('/Admin/Partials/PeoplePositions/' + peopleId);
                Swal.fire('Görevler', 'Görev başarıyla eklendi', 'success');
                peopleUpdateCancelPosition();
            }
            else
                Swal.fire('Görevler', response.msg, 'warning');
        });

}

function peopleUpdatePosition() {

    var model = getPositionFromUI();
    model.id = editPositionId;

    if (model.institutionTypeId == 1 && model.institutionName === "") {
        Swal.fire('Görevler', 'Kurum adını girmelisiniz.', 'warning');
        return;
    }
    if (model.institutionTypeId > 1 && isNaN(model.partyId)) {
        Swal.fire('Görevler', 'Parti seçimi yapılmamış.', 'warning');
        return;
    }
    model.partyId = isNaN(model.partyId) ? null : model.partyId;


    if (model.startDay > 31 || model.startMonth > 12) {
        Swal.fire('Görevler', 'Görev başlangıç tarihi hatalı.', 'warning');
        return;
    }
    if (model.endDay > 31 || model.endMonth > 12) {
        Swal.fire('Görevler', 'Görev bitiş tarihi hatalı.', 'warning');
        return;
    }

    blockUI();
    $.post('/Admin/PeopleManagement/UpdatePosition', { model: model })
        .done((response) => {
            unblockUI();
            if (response.success) {
                $('#divPositions').load('/Admin/Partials/PeoplePositions/' + peopleId);
                Swal.fire('Görevler', 'Görev başarıyla güncellendi', 'success');
                peopleUpdateCancelPosition();
            }
            else
                Swal.fire('Görevler', response.msg, 'warning');
        });

}


var editPositionId = 0;
function editPosition_click(id) {
    blockUI();
    editPositionId = id;
    $.get('/Admin/PeopleManagement/GetPostionById/' + id)
        .done((response) => {
            if (response.success) {
                $('#btnAddPosition').hide();
                $('#btnUpdatePosition').show();
                $('#btnCancelPosition').show();
                $('#ddlInstitutionType').val(response.data.institutionTypeId);
                positionInstitution_onChange();

                $('#txtPositionStartDay').val(response.data.startDay);
                $('#txtPositionStartMonth').val(response.data.startMonth);
                $('#txtPositionStartYear').val(response.data.startYear);

                $('#txtPositionEndDay').val(response.data.endDay);
                $('#txtPositionEndMonth').val(response.data.endMonth);
                $('#txtPositionEndYear').val(response.data.endYear);

                $('#ddlProvincePosition').val(response.data.provinceId).trigger('change');

                if (response.data.provinceId && response.data.provinceId > 0) {
                    getDistricts2(response.data.provinceId, response.data.districtId)
                }

                $('#ddlPositionId').val(response.data.positionId).trigger('change');
                $('#ddlSectorId').val(response.data.sectorId);

                $('#txtInstutionName').val(response.data.institutionName);

                $('#txtPeriod').val(response.data.period);
                $('#ddlParty').val(response.data.partyId);



            }

            unblockUI();
        });

}

function position_onChange() {
    var posid = parseInt($('#ddlPositionId').val());

    $('#ddlSectorId').val('');

    if (posid == 17)
        $('#divSectors').show();
    else
        $('#divSectors').hide();

}

function peopleUpdateCancelPosition() {
    $('#btnAddPosition').show();
    $('#btnUpdatePosition').hide();
    $('#btnCancelPosition').hide();

    $('#txtPositionStartDay,#txtPositionStartMonth,#txtPositionStartYear').val("");
    $('#txtPositionEndDay,#txtPositionEndMonth,#txtPositionEndYear').val("");
    $('#txtInstutionName').val("");
    $('#ddlParty').val("");
    $('#ddlProvincePosition').val('').trigger('change');
    $('#txtPeriod').val("");

}

function deletePosition_click(id) {

    Swal.fire({
        title: "Emin misiniz?",
        text: "Kişinin görevi silinecektir!",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Evet, sil!",
        cancelButtonText: "Hayır",

        closeOnConfirm: true
    }).then((result) => {
        if (result.isConfirmed) {
            blockUI();
            $.post('/Admin/PeopleManagement/DeletePosition/' + id)
                .done((response) => {
                    unblockUI();
                    if (response.success) {
                        Swal.fire("Silindi!", "Görev başarıyla silindi.", "success");
                        $('#divPositions').load('/Admin/Partials/PeoplePositions/' + peopleId);
                    }
                });
        }
    });

}

function getPositionFromUI() {
    var i = parseInt($('#ddlInstitutionType').val());
    var partyId = i > 1 ? parseInt($('#ddlParty').val()) : null;
    var sectorId = parseInt($('#ddlSectorId').val());

    var model = {
        id: 0,
        peopleId: peopleId,
        partyId: partyId,
        positionId: parseInt($('#ddlPositionId').val()),
        startDay: parseInt($('#txtPositionStartDay').val()),
        startMonth: parseInt($('#txtPositionStartMonth').val()),
        startYear: parseInt($('#txtPositionStartYear').val()),
        endDay: parseInt($('#txtPositionEndDay').val()),
        endMonth: parseInt($('#txtPositionEndMonth').val()),
        endYear: parseInt($('#txtPositionEndYear').val()),
        provinceId: parseInt($('#ddlProvincePosition').val()),
        districtId: parseInt($('#ddlDistrictPosition').val()),
        institutionTypeId: i,
        institutionName: $('#txtInstutionName').val(),
        description: $('#txtPositionDesc').val(),
        period: $('#txtPeriod').val(),
        sectorId:isNaN(sectorId)?null:sectorId,
    }

    return model;
}
/**
 * 
 * -----------------------------------------------
 */

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
            $.post('/Admin/PeopleManagement/DeleteAttachment/', { id: id })
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


function socialSearch(firstname, lastname, type) {




    var links = ['https://twitter.com/search?q=' + firstname + ' ' + lastname, 'https://facebook.com/search/top?q=' + firstname + ' ' + lastname, 'https://www.google.com/search?q=' + firstname + '+' + lastname + '+' + ' instagram',
        'https://www.linkedin.com/pub/dir?firstName=' + firstname + '&lastName=' + lastname + '&trk=people-guest_people-search-bar_search-submit', 'https://tr.wikipedia.org/w/index.php?search=' + firstname + '+' + lastname + '&title=%C3%96zel%3AAra&ns0=1',
        'https://www.youtube.com/results?search_query=' + firstname + '+' + lastname, 'https://www.google.com/search?q=' + firstname + '+' + lastname+'+'+' web sitesi']

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