var ClientRegister = function () {
    var init = function () {
        bindEvents();
    };

    var bindEvents = function () {
        $('#btnRegister').click(function () {
            var isValid = true;

            var data = {
                CompanyName: $('#companyName').val(),
                ContactPerson: $('#contactPerson').val(),
                Email: $('#email').val(),
                Password: $('#password').val()
            };

            if (data.CompanyName.trim() === '' || data.ContactPerson.trim() === '' ||
                data.Email.trim() === '' || data.Password.trim().length < 6) {
                alert("Please fill all fields properly.");
                return;
            }

            $.ajax({
                url: '/Client/RegisterUser',
                type: 'POST',
                data: data,
                success: function (res) {
                    if (res == 1) {
                        alert("OTP sent to your email.");
                        $('#verification-message').show();
                        $('#otp-section').show();
                        $('#nonOTPSec').hide();
                    } else if (res == 2) {
                        alert("Email already registered.");
                    } else {
                        alert("Error occurred.");
                    }
                }
            });
        });

        $('#verify-otp').click(function () {
            var email = $('#email').val();
            var otp = $('#otp').val();

            if (otp.trim() === '') {
                alert("Enter OTP.");
                return;
            }

            $.ajax({
                url: '/Client/VerifyOTP',
                type: 'POST',
                data: { email: email, otp: otp },
                success: function (res) {
                    if (res == 1) {
                        alert("Verification successful!");
                        location.reload();
                    } else {
                        alert("Invalid OTP.");
                    }
                }
            });
        });
    };

    return {
        Init: init
    };
}();

$(document).ready(function () {
    ClientRegister.Init();
});
