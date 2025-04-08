// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// site.js
$(document).ready(function () {
    // Обработка формы входа
    $('#loginForm').submit(function (e) {
        e.preventDefault();
        submitForm(this, '#loginModal');
    });

    // Обработка формы регистрации
    $('#registerForm').submit(function (e) {
        e.preventDefault();
        submitForm(this, '#registerModal');
    });
});

function submitForm(form, modalId) {
    $.ajax({
        url: $(form).attr('action'),
        method: 'POST',
        data: $(form).serialize(),
        success: function (response) {
            if (response.success) {
                $(modalId).modal('hide');
                window.location.href = response.redirectUrl;
            } else {
                // Обработка ошибок
                $(form).find('.text-danger').empty();
                $.each(response.errors, function (key, error) {
                    $('[data-valmsg-for="' + error.key + '"]').text(error.errorMessage);
                });
            }
        },
        error: function () {
            alert('Произошла ошибка');
        }
    });
}
// Write your JavaScript code.
