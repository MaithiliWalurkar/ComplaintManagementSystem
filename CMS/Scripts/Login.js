var Login = function () {
    var init = function () {
        bindEvents();
    };

    var bindEvents = function () {
        $('#email').on('input', function () {
            $(this).val($(this).val().replace(/[^A-Za-z0-9@.]/g, ''));
        });

        $('#password').on('input', function () {
            $(this).val($(this).val().replace(/\s/g, ''));
        });

        $('#btnLogin').click(function () {
            var isValid = true;

            var email = $('#email').val();
            var password = $('#password').val();

            if (email.trim() === '') {
                isValid = false;
                alert("Please enter Email.");
                return;
            }

            var emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            if (!emailPattern.test(email)) {
                isValid = false;
                alert("Please enter a valid Email.");
                return;
            }

            if (password.trim().length < 6) {
                isValid = false;
                alert("Password must be at least 6 characters.");
                return;
            }

            var Data = {
                UserEmail: email,
                UserPassword: password
            };

            $.ajax({
                url: '/Login/LoginUser',
                type: 'POST',
                data: Data,
                success: function (res) {
                    if (res === "Admin") {
                        alert("Welcome Admin!");
                        window.location.href = "/Admin/AdminDashboard";
                    } else if (res === "Staff") {
                        alert("Welcome Staff!");
                        window.location.href = "/Staff/StaffDashboard";
                    } else if (res === "Client") {
                        alert("Welcome Client!");
                        window.location.href = "/Client/ClientDashboard";
                    } else if (res === "Invalid") {
                        alert("Invalid credentials or email not verified.");
                    } else {
                        alert("Login failed. Try again.");
                    }
                },
                error: function () {
                    alert("Server error occurred.");
                }
            });
        });
    };

    return {
        Init: init
    };
}();

$(document).ready(function () {
    Login.Init();
});
