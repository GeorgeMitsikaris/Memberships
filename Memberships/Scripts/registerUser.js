$(document).ready(function () {
    var registerUserCheckBox = $('#AcceptUserAggrement').click(onToggleRegisterUserDisabledClick);

    function onToggleRegisterUserDisabledClick() {
        $('.register-user-panel button').toggleClass('disabled');
    }

    var registerUserButton = $('.register-user-panel button').click(onRegisterUserClick);

    function onRegisterUserClick() {
        var url = "/Account/RegisterUserAsync";
        var antiforgery = $('[name="__RequestVerificationToken"]').val();
        var name = $('.first-name').val();
        var email = $('.email').val();
        var pwd = $('.password').val();

        $.post(url, { __RequestVerificationToken: antiforgery, Name: name, Email: email, Password: pwd, AcceptUserAggrement: true },
            function (data) {
                var parsed = $.parseHTML(data);
                var hasErrors = $(parsed).find('[data-valmsg-summary]').text().replace(/\n|\r/g, "").length>0;

                if (hasErrors) {
                    $('.register-user-panel').html(data);
                    registerUserCheckBox = $('#AcceptUserAggrement').click(onToggleRegisterUserDisabledClick);
                    registerUserButton = $('.register-user-panel button').click(onRegisterUserClick);
                    $('.register-user-panel button').removeClass('disabled');
                }
                else {
                    registerUserCheckBox = $('#AcceptUserAggrement').click(onToggleRegisterUserDisabledClick);
                    registerUserButton = $('.register-user-panel button').click(onRegisterUserClick);
                    location.href = '/Home/Index';
                }
            }
        ).fail(function (xhr, status, error) {
            
        });
    }
});

