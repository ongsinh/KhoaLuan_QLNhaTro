$(document).ready(function () {
    $('#registration-form').submit(function (event) {
        // Ngừng gửi form nếu không hợp lệ
        event.preventDefault();

        var isValid = true;

        // Lấy giá trị từ các trường
        var name = $('#name').val(); // Đảm bảo trường nhập liệu có id là 'accountName'
        var phone = $('#phone').val();
        var email = $('#email').val();
        var password = $('#password').val();
        var repassword = $('#repassword').val();

        // Kiểm tra họ tên
        if (name.trim() === "") {
            $('#error-name').show();
            isValid = false;
        } else {
            $('#error-name').hide();
        }

        // Kiểm tra số điện thoại (10-12 số)
        if (!/^\d{10,12}$/.test(phone)) {
            $('#error-phone').show();
            isValid = false;
        } else {
            $('#error-phone').hide();
        }

        // Kiểm tra email
        var emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailPattern.test(email)) {
            $('#error-email').show();
            isValid = false;
        } else {
            $('#error-email').hide();
        }

        // Kiểm tra mật khẩu (ít nhất 8 ký tự)
        if (password.length < 8) {
            $('#error-password').show();
            isValid = false;
        } else {
            $('#error-password').hide();
        }

        // Kiểm tra mật khẩu nhập lại
        if (password !== repassword) {
            $('#error-repassword').show();
            isValid = false;
        } else {
            $('#error-repassword').hide();
        }

        // Nếu tất cả đều hợp lệ, submit form
        if (isValid) {
            this.submit(); // Gửi form khi không có lỗi
        }
    });
});
