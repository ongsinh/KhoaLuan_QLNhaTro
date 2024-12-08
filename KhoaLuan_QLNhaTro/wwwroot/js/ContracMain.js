$(document).ready(function () {
    // Khi nhấn nút "Cộng"
    $("#addContractButton").click(function () {
        const idHouse = $(this).data("id-house"); // Lấy idHouse từ data attribute

        $.ajax({
            url: "/Contract/AddContract",
            type: "GET",
            data: { idHouse: idHouse }, // Gửi idHouse qua AJAX
            success: function (data) {
                $("#modalBody").html(data); // Load nội dung vào modal
                $("#contractModal").modal("show"); // Hiển thị modal
            },
            error: function () {
                alert("Không thể tải form!");
            }
        });
    });

    // Sự kiện submit form hợp đồng
    $('#addContractForm').on('submit', function (e) {
        e.preventDefault(); // Ngừng hành động mặc định (submit form)

        var formData = $(this).serialize() + '&houseId=' + $("#addContractButton").data("id-house"); // Lấy toàn bộ dữ liệu của form
        console.log(formData); // Kiểm tra dữ liệu trước khi gửi

        $.ajax({
            url: '/Contract/AddContract', // Đường dẫn đến action POST
            type: 'POST',
            data: formData, // Dữ liệu form gửi đi
            success: function (response) {
                if (response.success) {
                    alert(response.message); // Hiển thị thông báo thành công
                    $("#contractModal").modal("hide"); // Đóng modal sau khi thêm hợp đồng
                } else {
                    alert(response.message); // Hiển thị thông báo lỗi
                }
            },
            error: function () {
                alert('Có lỗi xảy ra. Vui lòng thử lại!');
            }
        });
    });
});


