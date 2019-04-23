$(document).ready(function () {
    var registerUserCheckBox = $('#AcceptUserAggrement').click(onToggleRegisterUserDisabledClick);

    function onToggleRegisterUserDisabledClick() {
        $('.register-user-panel button').toggleClass('disabled')
    }
});