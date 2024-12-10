$(document).ready(function () {
    $.ajaxSetup({
        headers: {
            "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val()
        }
    });

    // Khi nhấn nút "Cộng"
    $("#addContractButton").click(function () {
        const idHouse = $(this).data("id-house"); // Lấy idHouse từ data attribute

        $.ajax({
            url: "/Contract/AddContract",  // GET để lấy form
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
});


function updateContractStatus() {
    $(".table-section tbody tr").each(function () {
        const startDate = new Date($(this).find("td:nth-child(5)").text()); // Ngày vào ở (cột thứ 5)
        const time = parseInt($(this).find("td:nth-child(6)").text()); // Thời hạn hợp đồng (cột thứ 6)
        const endDate = new Date(startDate);
        endDate.setMonth(startDate.getMonth() + time); // Tính ngày kết thúc hợp đồng
        const statusElement = $(this).find("td:nth-child(7) span"); // Cột trạng thái (cột thứ 7)

        // Nếu ngày hiện tại vượt quá ngày kết thúc hợp đồng, thay đổi trạng thái
        if (new Date() > endDate) {
            statusElement.text("Hết hạn hợp đồng").removeClass("status-active").addClass("status-expired");
        }
    });
}

// Gọi hàm cập nhật trạng thái ngay khi trang load
updateContractStatus();



