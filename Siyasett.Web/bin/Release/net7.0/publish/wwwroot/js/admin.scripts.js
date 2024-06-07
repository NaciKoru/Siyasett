$('.select2').select2();


function blockUI(msg) {

    $.blockUI({
        message: '<h3><i class="fas fa-spinner fa-pulse"></i> ' + (msg ? msg : 'Lütfen bekleyiniz.') + '</h3>',
        baseZ: 19002,
    });
}
function unblockUI() {
    $.unblockUI();

}

function validMail(mail) {
    return /^(([^<>()\[\]\.,;:\s@\"]+(\.[^<>()\[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()\.,;\s@\"]+\.{0,1})+([^<>()\.,;:\s@\"]{2,}|[\d\.]+))$/.test(mail);
}


function showToast(message,icon) {
    if (!message)
        message='İşlem başarıyla yapıldı'

    if (!icon)
        icon = 'success';

    Swal.fire({
        position: 'center',
        icon: icon,
        title: message,
        showConfirmButton: false,
        timer: 1500
    });
}
